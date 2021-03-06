if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptGiftsCertificates]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptGiftsCertificates]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de certificados de regalos
** 
** [wtorres] 06/Feb/2010 Created
** [wtorres] 17/Feb/2010 Modified. Agregue el campo CxC
** [wtorres] 22/Mar/2010 Modified. Agregue el campo ExtraAdults y ahora devuelve las diferentes monedas de los pagos
** [wtorres] 19/May/2010 Modified. Considerar solo los regalos cargados a Marketing
** [wtorres] 08/Jun/2010 Modified. Agregue los campos de Status y Comments. Ahora se devuelven los recibos cancelados
** [wtorres] 22/Jun/2010 Modified. Agregue el campo Refund
** [axperez] 01/Nov/2013 Modified. Agregue el parametro @Categories
**
*/
create procedure [dbo].[USP_OR_RptGiftsCertificates]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@SalesRooms varchar(8000),			-- Claves de salas de ventas
	@Categories varchar(8000) = 'ALL',	-- Claves de categorias de regalos
	@Gifts varchar(8000) = 'ALL'		-- Claves de regalos
as
set nocount on

select
	G.giID as GiftID,
	G.giN as GiftN,
	R.grID as Receipt,
	case when R.grCancel = 0 then 'ACTIVE' else 'CANCELED' end as Status,
	D.geFolios as Folios,
	R.grD as [Date],
	H.peN as Host,
	R.grGuest as Guest,
	L.loN as Location,
	D.geAdults as Adults,
	D.geMinors as Minors,
	D.geExtraAdults as ExtraAdults,
	dbo.UFN_OR_GetPax(D.geAdults, D.geMinors) as Pax,
	-- CxC = CxC de regalos + CxC de deposito + CxC de taxi de salia
	R.grCxCGifts + R.grCxCAdj + R.grCxCPRDeposit * IsNull(ED.exExchRate, 1) + R.grCxCTaxiOut * IsNull(ET.exExchRate, 1) as CxC,
	P.gypt as PaymentType,
	P.gycu as Currency,
	P.gyAmount as Paid,
	P.gyRefund as Refund,
	R.grComments as Comments
into #GiftsReceipts
from GiftsReceipts R
	left join GiftsReceiptsC D on D.gegr = R.grID
	left join Gifts G on D.gegi = G.giID
	left join GiftsReceiptsPayments P on R.grID = P.gygr
	left join Personnel H on R.grHost = H.peID
	left join Locations L on R.grlo = L.loID
	left join ExchangeRate ED on ED.exD = R.grD and ED.excu = R.grcuCxCPRDeposit
	left join ExchangeRate ET on ET.exD = R.grD and ET.excu = R.grcuCxCTaxiOut
where
	-- Fecha del recibo
	R.grD between @DateFrom and @DateTo
	-- Salas de ventas
	and R.grsr in (select item from split(@SalesRooms, ','))
	-- Categorias de Regalos
	and ( @Categories ='ALL' or G.gigc in (select item from split(@Categories,',')))
	-- Regalo
	and (@Gifts = 'ALL' or D.gegi in (select item from split(@Gifts, ',')))
	-- Cargados a marketing
	and R.grct = 'MARKETING'
order by Status, R.grID, G.giN

-- 1. Regalos
-- =============================================
select * from #GiftsReceipts

-- 2. Tipos de pago
-- =============================================
select distinct T.ptID, T.ptN
from #GiftsReceipts R
	inner join PaymentTypes T on T.ptID = R.PaymentType

-- 3. Monedas
-- =============================================
select distinct C.cuID, C.cuN
from #GiftsReceipts R
	inner join Currencies C on C.cuID = R.Currency

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

