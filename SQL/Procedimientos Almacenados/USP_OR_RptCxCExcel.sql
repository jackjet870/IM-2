if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptCxCExcel]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptCxCExcel]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de CxC por tipo (regalos, depositos y taxis)
** 
** [wtorres]		20/Feb/2014 Modified. Depurado
** [lchairez]		27/Mar/2014 Modified. Se agregan 3 columnas nuevas "Total CxC", "CxC Paid US" y "CxC Paid MN"
**
*/
CREATE procedure [dbo].[USP_OR_RptCxCExcel]
	@DateFrom datetime,		-- Fecha desde
	@DateTo datetime,		-- Fecha hasta
	@SalesRoom varchar(10)	-- Clave de sala de ventas
as
set fmtonly off
set nocount on

-- CxC de regalos
-- ===================================
select
	R.grpe,
	P.peN,
	R.grD,
	R.grNum,
	R.grsr,
	Cast('REGALOS' as varchar(20)) as Comments,
	R.grCxCGifts + R.grCxCAdj as CxC,
	IsNull(E.exExchRate, 1) as exExchRate,
	(R.grCxCGifts + R.grCxCAdj) / IsNull(E.exExchRate, 1) as CxCP,
	R.grCxCGifts + R.grCxCAdj + R.grCxCPRDeposit * IsNull(ED.exExchRate, 1) + R.grCxCTaxiOut * IsNull(ET.exExchRate, 1) as TotalCxC,
	COALESCE(R.grAmountToPay,0) as CxCPaidUS,
	COALESCE(R.grAmountToPay,0) / COALESCE(E.exExchRate,1)CxCPaidMN
into #Report
from GiftsReceipts R
	inner join Personnel P on P.peID = R.grpe
	left join ExchangeRate ED on ED.exD = R.grD and ED.excu = R.grcuCxCPRDeposit
	left join ExchangeRate E on E.exD = R.grD and E.excu = 'MEX'
	left join ExchangeRate ET on ET.exD = R.grD and ET.excu = R.grcuCxCTaxiOut
where
	-- Sala de ventas
	R.grsr = @SalesRoom
	-- Fecha de autorizacion de CxC
	and R.grCxCAppD between @DateFrom and @DateTo
	-- No considerar los recibos cancelados
	and R.grCancel = 0
	-- Tengan CxC de regalos
	and R.grCxCGifts + R.grCxCAdj > 0
order by R.grpe

-- CxC de taxis
-- ===================================
insert into #Report
select
	R.grpe,
	P.peN,
	R.grD,
	R.grNum,
	R.grsr,
	'TAXIS' as Comments,
	R.grTaxiOutDiff as CxC,
	0 as exExchRate,
	R.grTaxiOutDiff as CxCP,
	0 as TotalCxC,
	COALESCE(R.grAmountToPay,0) as CxCPaidUS,
	0 AS CxCPaidMN
from GiftsReceipts R
	left join Personnel P on P.peID = R.grpe
where
	-- Sala de ventas
	R.grsr = @SalesRoom
	-- Fecha de autorizacion de CxC
	and R.grCxCAppD between @DateFrom and @DateTo 
	-- No considerar los recibos cancelados
	and R.grCancel = 0
	-- Tengan CxC de deposito igual a la diferencia del taxi de salida
	and R.grCxCPRDeposit > 0 and R.grCxCPRDeposit = R.grTaxiOutDiff
order by R.grpe

-- CxC de depositos
-- ===================================
insert into #Report
select
	R.grpe,
	P.peN,
	R.grD,
	R.grNum,
	R.grsr,
	'DEPOSITOS' as Comments,
	R.grCxCPRDeposit * IsNull(ED.exExchRate, 1) as CxC,
	IsNull(E.exExchRate, 1) as exExchRate,
	R.grCxCPRDeposit * IsNull(ED.exExchRate, 1) / IsNull(E.exExchRate, 1) as CxCP,
	R.grCxCGifts + R.grCxCAdj + R.grCxCPRDeposit * IsNull(ED.exExchRate, 1) + R.grCxCTaxiOut * IsNull(ET.exExchRate, 1) as TotalCxC,
	COALESCE(R.grAmountToPay,0) as CxCPaidUS,
	COALESCE(R.grAmountToPay,0) / COALESCE(E.exExchRate,1)CxCPaidMN
from GiftsReceipts R
	left join Personnel P on P.peID = R.grpe
	left join ExchangeRate ED on ED.exD = R.grD and ED.excu = R.grcuCxCPRDeposit
	left join ExchangeRate E on E.exD = R.grD and E.excu = 'MEX'
	left join ExchangeRate ET on ET.exD = R.grD and ET.excu = R.grcuCxCTaxiOut
where
	-- Sala de ventas
	R.grsr = @SalesRoom
	-- Fecha de autorizacion de CxC
	and R.grCxCAppD between @DateFrom and @DateTo
	-- No considerar los recibos cancelados
	and R.grCancel = 0
	-- Tengan CxC de deposito diferente a la diferencia del taxi de salida
	and R.grCxCPRDeposit > 0 and R.grCxCPRDeposit <> R.grTaxiOutDiff
order by R.grpe

-- devolvemos los datos del reporte
select * from #Report order by grpe, grD, Comments

DROP TABLE #Report
