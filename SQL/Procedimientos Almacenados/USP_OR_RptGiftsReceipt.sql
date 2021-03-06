USE [OrigosVCPalace]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptGiftsReceipt]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptGiftsReceipt]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos del reporte de un recibo de regalos
**
** [wtorres]		17/Jul/2009 Created
** [wtorres]		02/Jul/2010 Modified. Ahora toma en cuenta el cargo de deposito en el total del cargo
** [wtorres]		28/Dic/2011 Modified. Agregue el campo descripcion de la agencia
** [wtorres]		17/May/2012 Modified. Agregue las leyendas de los productos
** [wtorres]		12/Jun/2013 Modified. Ahora las leyendas de los productos contemplan la tabla de textos de reportes
** [alesanchez]		07/Nov/2013 Modified. Agregue la descripcion de las Formas de Pago para que se muestren en el Chargeback Voucher
** [lchairez]		19/Dic/2013 Modified. Agregue quien autorizo el pago
** [axperez]		24/Dic/2013 Modified. Agregue motivos de reimpresion
** [wtorres]		07/Ene/2014 Modified. Agregue los campos de folio de la reservacion inhouse, el folio de la invitacion outhouse y programa
** [lchairez]		16/Ene/2014 Modified. Agregue el cargo por el pago de taxi de salida al cargo
** [wtorres]		30/Ene/2014 Modified. Agregue los campos de adultos y menores a los regalos del recibo
** [wtorres]		17/Feb/2014 Modified. Fusione la clave y la descripcion del PR y de la autorizador del CxC en un solo campo respectivamente
** [wtorres]		20/Feb/2014 Modified. Corregi el calculo del CxC de regalos, le faltaba incluir el ajuste.
**							Corregi el calculo del CxC total, le faltaba multiplicar el CxC de taxi de salida por su tipo de cambio
**							Corregi el calculo del CxC en pesos. Ahora se toma de la tabla de tipos de cambio
** [wtorres]		19/Jun/2014 Modified. Agregue las leyendas de promociones de Opera. Renombrado. Antes se llamaba USP_OR_RptChargeback
** [edgrodriguez]	12/Jul/2016 Modified. Los campos TotalUSD, grCxCGifts, exExchRate, TotalMX retornan 0 en vez de null
**
*/
CREATE procedure [dbo].[USP_OR_RptGiftsReceipt]
	@Receipt int,		-- Clave del recibo de regalos
	@IsCharge bit = 0	-- Indica si se desea el reporte de cargo al PR
as
set nocount on

declare
	@Report varchar(50),
	@Total money,
	@CxC money

set @Report = 'GiftsReceipt'

-- CxC = CxC de regalos (CxC + Ajuste) + CxC de deposito + CxC de taxi de salida
select @CxC = R.grCxCGifts + R.grCxCAdj + R.grCxCPRDeposit * IsNull(ED.exExchRate, 1) + R.grCxCTaxiOut * IsNull(ET.exExchRate, 1)
from GiftsReceipts R
	left join ExchangeRate ED on ED.exD = R.grD and ED.excu = R.grcuCxCPRDeposit
	left join ExchangeRate ET on ET.exD = R.grD and ET.excu = R.grcuCxCTaxiOut
where R.grID = @Receipt

-- si no es el reporte de cargo al PR
if @IsCharge = 0 begin

	-- Total del costo de los regalos
	select @Total = Sum(IsNull(D.gePriceA + D.gePriceM, 0))
	from GiftsReceiptsC D
		left join GiftsReceipts R on D.gegr = R.grID
	where R.grID = @Receipt

	set @Total = @Total - @CxC
end else
	set @Total = @CxC

-- Recibo de regalos
-- =============================================
select
	R.grID,
	R.grNum,
	R.grGuest,
	R.grHotel,
	G.guHReservID,
	G.guOutInvitNum,
	R.grRoomNum,
	R.grpe + ' - ' + PR.peN as grpe,
	L.loN,
	R.grD,
	R.grPax,
	G.guag,
	A.agN,
	R.grMemberNum,
	R.grDeposit,
	R.grDepositTwisted,
	R.grcu,
	P.ptN,
	R.grTaxiIn,
	R.grTaxiOut,
	cast(ISNULL(@Total, 0) as money) as TotalUSD,
	cast(ISNULL(E.exExchRate, 0) as money)as exExchRate,
	cast(ISNULL((@Total / E.exExchRate), 0) as money) as TotalMX,
	case when @IsCharge = 0 then dbo.AddString(R.grComments, case when dbo.UFN_OR_GetGuestPromotionsQuantity(@Receipt) > 0
			then Replace(dbo.UFN_OR_GetReportText(@Report,
			case when dbo.UFN_OR_GetGuestPromotionsQuantity(@Receipt) = 1 then 'Promotion' else 'Promotions' end, G.gula),
			'[Promotions]', dbo.UFN_OR_GetGuestPromotionsCodes(@Receipt)) else '' end, Char(13))
		else R.grCxCComments
		end as grComments,
	GH.peN as GiftsHostess,
	G.gula,
	R.grCxCPRDeposit,
	R.grcuCxCPRDeposit,
	R.grCxCTaxiOut,
	R.grcuCxCTaxiOut,
	Cast(ISNULL((R.grCxCGifts + R.grCxCAdj), 0)as money) as grCxCGifts,
	R.grAuthorizedBy + ' - ' + PA.peN as grAuthorizedBy,
	R.grReimpresion,
	RM.rmN,
	LS.lspg
into #GiftsReceipt
from GiftsReceipts R
	left join Personnel PR on PR.peID = R.grpe
	left join Personnel GH on GH.peID = R.grHost
	left join Locations L on L.loID = R.grlo
	left join Guests G on G.guID = R.grgu
	left join Agencies A on A.agID = G.guag
	left join PaymentTypes P on P.ptID = R.grpt
	left join Personnel PA on PA.peID = R.grAuthorizedBy
	left join ReimpresionMotives RM on RM.rmID = R.grrm
	left join LeadSources LS on LS.lsID = G.guls
	left join ExchangeRate E on E.exD = R.grD and E.excu = 'MEX'
where R.grID = @Receipt

select * from #GiftsReceipt

-- si no es el reporte de cargo al PR
if @IsCharge = 0 begin

	-- Regalos
	-- =============================================
	select D.geQty, D.gegi, G.giN, D.geAdults, D.geMinors, D.geFolios, D.geComments, G.gipr
	into #GiftsReceiptDetail
	from GiftsReceiptsC D
		left join GiftsReceipts R on D.gegr = R.grID
		left join Gifts G on D.gegi = G.giID
	where R.grID = @Receipt

	select * from #GiftsReceiptDetail

	-- Leyendas de productos
	-- =============================================
	-- Leyendas para todos los productos
	select 'General' as reType, R.reO, R.reTextID, '' as prN, R.reText
	from ReportsTexts R
		inner join #GiftsReceipt GR on 1 = 1
	where R.reReport = @Report
		and R.rela = GR.gula
		and (R.retextID = 'AllGifts'
			or (R.retextID = 'NotExchangeableCash_CurrentStay'
				and (select Count(*) from #GiftsReceiptDetail where gipr = 'NREG') < (select Count(*) from #GiftsReceiptDetail))
			or (R.retextID = 'ReservationInPVPDesks'
				and (select Count(*) from #GiftsReceiptDetail where gipr in ('HOTELSHOP', 'STOREMONEY', 'NREG')) < (select Count(*) from #GiftsReceiptDetail)))
	-- Leyendas para cada producto
	union all
	select 'Product', 9999, L.pxpr,
		case when L.pxpr <> 'NREG' then P.prN else dbo.UFN_OR_GetReportText(@Report, 'ComplimentaryNight', GR.gula) end,
		L.pxText
	from ProductsLegends L
		inner join Products P on P.prID = L.pxpr
		inner join #GiftsReceipt GR on 1 = 1
	where pxpr in (select distinct gipr from #GiftsReceiptDetail)
		and pxla = GR.gula
	order by reO, reTextID
end


