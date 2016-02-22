if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptGiftsSale]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptGiftsSale]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de regalos tipo venta
**		1. Recibos de regalos
**		2. Pagos
**		3. Origenes de pago
**		4. Monedas
**		5. Formas de pago
** 
** [wtorres]	28/Ago/2014 Created
** [wtorres]	10/Sep/2014 Modified. Agregue el parametro @Sale
** [caduran]	09/Oct/2014 Modified. Se agrega columna ExchangeRate que servirá como 
**							'label' secundario en el encabezado del	agrupador por fecha
**							que se agregó al reporte.
**
*/
create procedure [dbo].[USP_OR_RptGiftsSale]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@SalesRooms varchar(8000) = 'ALL',	-- Claves de salas
	@Categories varchar(8000) = 'ALL',	-- Claves de categorias de regalos
	@Gifts varchar(8000) = 'ALL',		-- Claves de regalos
	@Sale tinyint = 0					-- Indica si se desean los regalos que fueron venta
										--		0. Todos los regalos
										--		1. Solo los regalos que fueron venta
										--		1. Solo los regalos que no fueron venta
as
set nocount on

-- Recibos de regalos (Tabla temporal)
-- =============================================
select
	PG.pgN as Program,
	R.grID as Receipt,
	R.grD as [Date],
	R.grCancel as Cancel,
	R.grCancelD as CancelDate,
	S.srN as SalesRoom,
	L.lsN as LeadSource,
	R.grpe as PR,
	P.peN as PRN,
	GU.guOutInvitNum as OutInvit,
	R.grgu as GuestID,
	GU.guLastName1 as LastName,
	GU.guFirstName1 as FirstName,
	D.gegi as Gift,
	G.giN as GiftN,
	case when D.geSale = 0 then ''
		when R.grCancel = 0 then 'P' else 'O'
		end as GiftSale,
	D.geAdults * (case when R.grCancelD between @DateFrom and @DateTo or D.geQty < 0 then -1 else 1 end) as Adults,
	D.geMinors * (case when R.grCancelD between @DateFrom and @DateTo or D.geQty < 0 then -1 else 1 end) as Minors,
	D.geExtraAdults * (case when R.grCancelD between @DateFrom and @DateTo or D.geQty < 0 then -1 else 1 end) as ExtraAdults,
	
	-- Precios
	(D.gePriceAdult + D.gePriceMinor + D.gePriceExtraAdult)
		* (case when R.grCancelD between @DateFrom and @DateTo then -1 else 1 end) as PriceUS,
	(D.gePriceAdult + D.gePriceMinor + D.gePriceExtraAdult) / IsNull(EM.exExchRate, 1)
		* (case when R.grCancelD between @DateFrom and @DateTo then -1 else 1 end) as PriceMX,
	(D.gePriceAdult + D.gePriceMinor + D.gePriceExtraAdult) / IsNull(EC.exExchRate, 1)
		* (case when R.grCancelD between @DateFrom and @DateTo then -1 else 1 end) as PriceCAN,
	
	-- Total a pagar
	case when D.geSale = 1 then D.gePriceAdult + D.gePriceMinor + D.gePriceExtraAdult else 0 end
		* (case when R.grCancelD between @DateFrom and @DateTo then -1 else 1 end) as TotalToPay,
	
	-- Pago total
	Cast(0 as money) as PaymentTotal,
	
	-- Diferencia entre el monto a pagar y el pago
	case when D.geSale = 1 then D.gePriceAdult + D.gePriceMinor + D.gePriceExtraAdult else 0 end
		* (case when R.grCancelD between @DateFrom and @DateTo then -1 else 1 end) as [Difference],
	
	-- Usuarios que ingresaron los pagos
	Cast('' as varchar(8000)) as [User],
	Cast('' as varchar(8000)) as UserN,
	
	-- Comentarios
	R.grComments as Comments,
	dbo.UFN_OR_GetFormatForExchangeRateInGiftSaleReport(EM.exExchRate,EC.exExchRate) as ExchangeRate
into #GiftsReceipts
from GiftsReceipts R
	inner join GiftsReceiptsC D on D.gegr = R.grID
	left join Gifts G on D.gegi = G.giID
	left join ExchangeRate EM on EM.exD = R.grD and EM.excu = 'MEX'
	left join ExchangeRate EC on EC.exD = R.grD and EC.excu = 'CAN'
	left join Personnel P on P.peID = R.grpe
	left join Guests GU on GU.guID = R.grgu
	left join LeadSources L on L.lsID = R.grls
	left join Programs PG on PG.pgID = L.lspg
	left join SalesRooms S on S.srID = R.grsr
where
	-- Fecha del recibo de regalos
	(R.grD between @DateFrom and @DateTo
	-- Fecha de cancelacion del recibo de regalos
	or R.grCancelD between @DateFrom and @DateTo)
	-- Salas de ventas
	and (@SalesRooms = 'ALL' or R.grsr in (select item from split(@SalesRooms, ',')))
	-- Categorias de regalos
	and (@Categories = 'ALL' or G.gigc in (select item from split(@Categories, ',')))
	-- Regalos
	and (@Gifts = 'ALL' or D.gegi in (select item from split(@Gifts, ',')))
	-- Regalos que fueron venta
	and ((@Sale = 0 and (G.giSale = 1 or D.geSale = 1))
		or (@Sale = 1 and D.geSale = 1)
		or (@Sale = 2 and G.giSale = 1 and D.geSale = 0))
order by PG.pgN, R.grD, L.lsN, R.grID, D.geSale, G.giN

-- 1. Recibos de regalos
-- =============================================
select * from #GiftsReceipts

-- Pagos (Tabla temporal)
-- =============================================
select
	P.gygr as Receipt,
	P.gysb as [Source],
	P.gycu as Currency,
	P.gypt as PaymentType,
	P.gyAmount * (case when R.grCancelD between @DateFrom and @DateTo then -1 else 1 end) as Amount,
	IsNull(E.exExchRate, 1) as ExchangeRate,
	P.gyAmount * IsNull(E.exExchRate, 1) * (case when R.grCancelD between @DateFrom and @DateTo then -1 else 1 end) as AmountUS,
	P.gype as [User],
	PE.peN as UserN
into #GiftsReceiptsPayments
from GiftsReceiptsPayments P
	left join GiftsReceipts R on R.grID = P.gygr
	left join Personnel PE on PE.peID = P.gype
	left join ExchangeRate E on E.exD = R.grD and E.excu = P.gycu
where
	-- Recibo de regalos
	P.gygr in (select distinct Receipt from #GiftsReceipts)
order by R.grD, P.gygr, P.gysb, P.gycu, P.gypt, P.gyAmount

-- 2. Pagos
-- =============================================
select * from #GiftsReceiptsPayments

-- 3. Origenes de pago
-- =============================================
select distinct S.sbID, S.sbN
from #GiftsReceiptsPayments P
	left join SourcePayments S on S.sbID = P.[Source]
order by S.sbN

-- 4. Monedas
-- =============================================
select distinct C.cuID, C.cuN
from #GiftsReceiptsPayments P
	left join Currencies C on C.cuID = P.Currency
order by C.cuN

-- 5. Formas de pago
-- =============================================
select distinct PT.ptID, PT.ptN
from #GiftsReceiptsPayments P
	left join PaymentTypes PT on PT.ptID = P.PaymentType
order by PT.ptN


GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO