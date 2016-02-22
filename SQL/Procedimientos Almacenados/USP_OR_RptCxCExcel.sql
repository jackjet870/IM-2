if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptCxCExcel]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptCxCExcel]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de CxC (Excel)
** 
** [wtorres]	20/Feb/2014 Depurado
** [lchairez]	27/Mar/2014 Se agregan 3 columnas nuevas "Total CxC", "CxC Paid US" y "CxC Paid MN"
**
*/
create procedure [dbo].[USP_OR_RptCxCExcel]
	@DateFrom datetime,		-- Fecha desde
	@DateTo datetime,		-- Fecha hasta
	@SalesRoom varchar(10)	-- Clave de sala de ventas
as
set nocount on

-- CxC de regalos
-- ===================================
select
	R.grpe,
	R.grD,
	R.grID,
	R.grNum,
	R.grsr,
	R.grls,
	p.peID,
	p.peN,
	Cast('REGALOS' as varchar(20)) as Comments,
	R.grCxCGifts + R.grCxCAdj as CxC,
	IsNull(E.exExchRate, 1) as exExchRate,
	(R.grCxCGifts + R.grCxCAdj) / IsNull(E.exExchRate, 1) as CxCP,
	R.grCxCGifts + R.grCxCAdj + R.grCxCPRDeposit * IsNull(ED.exExchRate, 1) + R.grCxCTaxiOut * IsNull(ET.exExchRate, 1) as TotalCxC,
	IsNull(R.grAmountToPay, 0) as ToPayUS,
	IsNull(R.grAmountToPay, 0) / IsNull(E.exExchRate,1) as ToPayMN,
	IsNull(R.grAmountPaid, 0) as PaidUS,
	IsNull(R.grAmountPaid, 0) / IsNull(E.exExchRate,1) as PaidMN,
	IsNull(R.grBalance, 0) as BalanceUS,
	IsNull(R.grBalance, 0) / IsNull(E.exExchRate,1) as BalanceMN,
	R.grComments as GiftComments,
	G.guID,
	G.guHReservID,
	G.guOutInvitNum,
	'True' as AuthSts,
	R.grCxCAppD,
	R.grAuthorizedBy, 
	PA.peN as AuthName,
	R.grcxcComments as CxCComments
into #Report
from GiftsReceipts R
	inner join Personnel P on P.peID = R.grpe
	inner join Guests G on G.guID = R.grgu
	left join ExchangeRate ED on ED.exD = R.grD and ED.excu = R.grcuCxCPRDeposit
	left join ExchangeRate E on E.exD = R.grD and E.excu = 'MEX'
	left join ExchangeRate ET on ET.exD = R.grD and ET.excu = R.grcuCxCTaxiOut
	left join Personnel PA on PA.peID = R.grAuthorizedBy
where
	-- Sala de ventas
	R.grsr = @SalesRoom
	-- Fecha de autorizacion de CxC
	and R.grCxCAppD between @DateFrom and @DateTo
	-- No considerar los recibos cancelados
	and R.grCancel = 0
	-- Tengan CxC de regalos
	and R.grCxCGifts + R.grCxCAdj > 0
	-- Esten autorizados
	and IsNull(R.grAuthorizedBy, '') <> ''
order by R.grID, R.grpe 

-- CxC de taxis
-- ===================================
insert into #Report
select
	R.grpe,
	R.grD,
	R.grID,
	R.grNum,
	R.grsr,
	R.grls,
	p.peID,
	p.peN,
	'TAXIS' as Comments,
	R.grTaxiOutDiff as CxC,
	0 as exExchRate,
	R.grTaxiOutDiff as CxCP,
	0 as TotalCxC,
	IsNull(R.grAmountToPay, 0) as ToPayUS,
	IsNull(R.grAmountToPay, 0) / IsNull(E.exExchRate,1) as ToPayMN,
	IsNull(R.grAmountPaid, 0) as PaidUS,
	IsNull(R.grAmountPaid, 0) / IsNull(E.exExchRate,1) as PaidMN,
	IsNull(R.grBalance, 0) as BalanceUS,
	IsNull(R.grBalance, 0) / IsNull(E.exExchRate,1) as BalanceMN,
	R.grComments as GiftComments,
	G.guID,
	G.guHReservID,
	G.guOutInvitNum,
	'True' as AuthSts,
	R.grCxCAppD,
	R.grAuthorizedBy, 
	PA.peN as AuthName,
	R.grcxcComments as CxCComments
from GiftsReceipts R
	inner join Guests G on G.guID = R.grgu
	left join Personnel P on P.peID = R.grpe
	left join ExchangeRate E on E.exD = R.grD and E.excu = 'MEX'
	left join Personnel PA on PA.peID = R.grAuthorizedBy
where
	-- Sala de ventas
	R.grsr = @SalesRoom
	-- Fecha de autorizacion de CxC
	and R.grCxCAppD between @DateFrom and @DateTo 
	-- No considerar los recibos cancelados
	and R.grCancel = 0
	-- Tengan CxC de deposito igual a la diferencia del taxi de salida
	and R.grCxCPRDeposit > 0 and R.grCxCPRDeposit = R.grTaxiOutDiff
   	-- Esten autorizados
	and IsNull(R.grAuthorizedBy, '') <> ''
order by R.grpe

-- CxC de depositos
-- ===================================
insert into #Report
select
	R.grpe,
	R.grD,
	R.grID, 
	R.grNum,
	R.grsr,
	R.grls,
	p.peID,
	p.peN,
	'DEPOSITOS' Comments,
	R.grCxCPRDeposit * IsNull(ED.exExchRate, 1) as CxC,
	IsNull(E.exExchRate, 1) as exExchRate,
	R.grCxCPRDeposit * IsNull(ED.exExchRate, 1) / IsNull(E.exExchRate, 1) as CxCP,
	R.grCxCGifts + R.grCxCAdj + R.grCxCPRDeposit * IsNull(ED.exExchRate, 1) + R.grCxCTaxiOut * IsNull(ET.exExchRate, 1) as TotalCxC,
	IsNull(R.grAmountToPay, 0) ToPayUS,
	IsNull(R.grAmountToPay, 0) / IsNull(E.exExchRate,1) ToPayMN,
	IsNull(R.grAmountPaid, 0) PaidUS,
	IsNull(R.grAmountPaid, 0) / IsNull(E.exExchRate,1) PaidMN,
	IsNull(R.grBalance, 0) BalanceUS,
	IsNull(R.grBalance, 0) / IsNull(E.exExchRate,1) BalanceMN,
	R.grComments GiftComments,
	G.guID,
	G.guHReservID,
	G.guOutInvitNum,
	'True' as AuthSts,
	R.grCxCAppD,
	R.grAuthorizedBy, 
	PA.peN as AuthName,
	R.grcxcComments as CxCComments
from GiftsReceipts R
	inner join Guests G on G.guID = R.grgu
	left join Personnel P on P.peID = R.grpe
	left join ExchangeRate ED on ED.exD = R.grD and ED.excu = R.grcuCxCPRDeposit
	left join ExchangeRate E on E.exD = R.grD and E.excu = 'MEX'
	left join ExchangeRate ET on ET.exD = R.grD and ET.excu = R.grcuCxCTaxiOut
	left join Personnel PA on PA.peID = R.grAuthorizedBy
where
	-- Sala de ventas
	R.grsr = @SalesRoom
	-- Fecha de autorizacion de CxC
	and R.grCxCAppD between @DateFrom and @DateTo
	-- No considerar los recibos cancelados
	and R.grCancel = 0
	-- Tengan CxC de deposito diferente a la diferencia del taxi de salida
	and R.grCxCPRDeposit > 0 and R.grCxCPRDeposit <> R.grTaxiOutDiff
	-- Esten autorizados
	and IsNull(R.grAuthorizedBy, '') <> ''
order by R.grpe

-- devolvemos los datos del reporte
select R.grpe,
	   R.grD,
	   R.grID,
	   R.grNum,
	   R.grsr,  
	   R.grls,
	   R.peID,
	   R.peN,
	   R.Comments,
	   Cast(R.CxC as decimal(15,2)) as CxC,
	   Cast(R.exExchRate as decimal(15,2)) as exExchRate,	   
	   Cast(R.CxCP as decimal(15,2)) as CxCP,
	   Cast(R.TotalCxC as decimal(15,2)) as TotalCxC,
	   Cast(R.ToPayUS as decimal(15,2)) as ToPayUS, 
	   Cast(R.ToPayMN as decimal(15,2)) as ToPayMN,
	   Cast(R.PaidUS as decimal(15,2)) as PaidUS,	   
	   Cast(R.PaidMN as decimal(15,2)) as PaidMN,
	   Cast(R.BalanceUS as decimal(15,2)) as BalanceUS,
	   Cast(R.BalanceMN as decimal(15,2)) as BalanceMN,
	   R.GiftComments,
	   R.guID,
	   R.guHReservID,
	   R.guOutInvitNum,
	   R.AuthSts,
	   R.grCxCAppD,
	   R.grAuthorizedBy, 
	   R.AuthName,
	   R.CxCComments 
from #Report R
order by grpe, grD, Comments

DROP TABLE #Report

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

