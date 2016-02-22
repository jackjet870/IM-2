if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptCxC]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptCxC]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de CxC
** 
** [wtorres]		28/Feb/2013 Modified. Depurado
** [wtorres]		19/Feb/2014 Modified. Agregue el campo de CxC de taxi de salida y elimine los campos de hotel y locacion
** [lchairez]		27/Mar/2014	Modified. Se cambia nombre de campo "Total CxC US" por "Total CxC", Se elimina la columna "Total CxC MN"
**								Modified. Se agregan 2 columnas nuevas CxC Paid US y CxC Paid MN
** [lormartinez]	13/Jul/2015 Modified. Se cambia la columna grAmountPaid por grAmountToPaid
**
*/
create procedure [dbo].[USP_OR_RptCxC]
	@DateFrom datetime,		-- Fecha desde
	@DateTo datetime,		-- Fecha hasta
	@SalesRoom varchar(10)	-- Clave de sala de ventas
as
set nocount on

select
	-- separamos los vales de los chargebacks
	case when R.grNum LIKE '%VALE%' then Cast('VALE' as varchar(11)) else Cast('CHARGE BACK' as varchar(11)) end as grGroup,
	R.grpe,
	P.peN,
	R.grID,
	R.grNum,	
	R.grD,
	R.grgu,
	R.grGuest,
	IsNull(D.geQty,0) as geQty,
	Cast(G.giN as varchar(50)) as giN,
	IsNull(D.geAdults, 0) as geAdults,
	IsNull(D.geMinors, 0) as geMinors,
	D.geFolios,
	IsNull(D.gePriceA + D.gePriceM, 0) as TotalGift,
	-- CxC de regalos
	R.grCxCGifts,
	R.grCxCAdj,
	-- CxC de deposito
	R.grCxCPRDeposit,
	R.grcuCxCPRDeposit,
	IsNull(ED.exExchRate, 1) as ExchRateDeposit,
	R.grCxCPRDeposit * IsNull(ED.exExchRate, 1) as CxCDepositUS,
	R.grCxCPRDeposit * IsNull(ED.exExchRate, 1) / IsNull(ES.exExchRate, 1) as CxCDepositMN,
	-- CxC de taxi de salida
	R.grCxCTaxiOut,
	R.grcuCxCTaxiOut,
	IsNull(ET.exExchRate, 1) as ExchRateTaxiOut,
	R.grCxCTaxiOut * IsNull(ET.exExchRate, 1) as CxCTaxiOutUS,
	R.grCxCTaxiOut * IsNull(ET.exExchRate, 1) / IsNull(ES.exExchRate, 1) as CxCTaxiOutMN,
	-- Total de CxC
	R.grCxCGifts + R.grCxCAdj + R.grCxCPRDeposit * IsNull(ED.exExchRate, 1) + R.grCxCTaxiOut * IsNull(ET.exExchRate, 1) as TotalCxC,
	COALESCE(R.grAmountToPay,0) as CxCPaidUS,
	COALESCE(R.grAmountToPay,0) / COALESCE(E.exExchRate,1)CxCPaidMN,
	-- Tipo de cambio de la sala de ventas
	IsNull(ES.exExchRate, 1) as ExchRateSalesRoom,
	R.grCxCComments,
	R.grComments
from GiftsReceipts R
	left join GiftsReceiptsC D on D.gegr  = R.grID
	left join Gifts G on G.giID  = D.gegi
	left join Personnel P on P.peID = R.grpe
	left join SalesRooms S on S.srID = R.grsr
	left join ExchangeRate ES on ES.exD = R.grD and ES.excu = S.srcu
	left join ExchangeRate ED on ED.exD = R.grD and ED.excu = R.grcuCxCPRDeposit
	left join ExchangeRate ET on ET.exD = R.grD and ET.excu = R.grcuCxCTaxiOut
	left join ExchangeRate E on E.exD = R.grD and E.excu = 'MEX'
where
	-- Fecha de autorizacion de CxC
	R.grCxCAppD between @DateFrom and @DateTo
	-- Sala de ventas
	and R.grsr = @SalesRoom
	-- No considerar los recibos cancelados
	and R.grCancel = 0
	-- Tengan CxC (de regalos, de deposito o de taxi de salida)
	and (R.grCxCGifts + R.grCxCAdj <> 0 or R.grCxCPRDeposit > 0 or R.grCxCTaxiOut > 0)
order by R.grNum, grGroup, R.grpe, R.grID, R.grgu

-- indicamos si la sala de ventas ya hizo el cierre de CxC para la fecha indicada
select Cast(case when srCxCCloseD >= @DateTo then 1 else 0 end as bit) as SalesRoomClosed
from SalesRooms
where srID = @SalesRoom

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

