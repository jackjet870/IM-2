if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetGiftsReceiptDetailCancel]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetGiftsReceiptDetailCancel]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta los regalos de un recibo que tienen asociados productos externos y que pueden ser cancelados
** 
** [wtorres]	15/Sep/2014 Created
**
*/
create procedure [dbo].[USP_OR_GetGiftsReceiptDetailCancel]
	@Receipt int,				-- Clave del recibo de regalos
	@ExternalProducts tinyint,	-- Tipo de producto externo
								--		1. Monedero electronico
								--		2. Promociones de PVP
	@Package bit = 0			-- Indica si se desean los paquetes de regalos
as
set nocount on

-- Regalos del recibo guardados en el sistema externo y no cancelados
-- =============================================
select
	D.gegr,
	D.geCancelElectronicPurse,
	D.geCancelPVPPromo,
	D.geQty,
	'' as QtyUnit,
	D.gegi,
	-- Pax
	D.geAdults,
	D.geMinors,
	D.geExtraAdults,
	-- Costos
	D.gePriceA,
	D.gePriceM,
	-- Precios
	D.gePriceAdult,
	D.gePriceMinor,
	D.gePriceExtraAdult,
	-- Campo que posiblemente se elimine
	D.gect,
	-- Folios
	D.geFolios,
	-- Regalo tipo venta
	D.geSale,
	-- Monedero electronico
	D.geInElectronicPurse,
	D.geConsecutiveElectronicPurse,
	-- Promociones de PVP
	D.geInPVPPromo,
	IsNull(D.gePVPPromotion, G.giPVPPromotion) as gePVPPromotion,
	-- Comentarios
	D.geComments,
	'' as Package
from GiftsReceiptsC D
	left join Gifts G on G.giID = D.gegi
where
	-- Recibo
	D.gegr = @Receipt
	-- Guardados en el monedero electronico y no cancelados aun
	and ((@ExternalProducts = 1 and D.geInElectronicPurse = 1 and D.geCancelElectronicPurse = 0)
	-- Guardados en promociones de PVP y no cancelados aun
	or (@ExternalProducts = 2 and D.geInPVPPromo = 1 and D.geCancelPVPPromo = 0))
	-- Paquetes de regalos
	and @Package = 0
	
-- Paquetes del recibo que tienen regalos guardados en el monedero electronico y no cancelados
-- =============================================
union all
select distinct
	D.gegr,
	D.geCancelElectronicPurse,
	D.geCancelPVPPromo,
	D.geQty,
	'' as QtyUnit,
	D.gegi,
	-- Pax
	D.geAdults,
	D.geMinors,
	D.geExtraAdults,
	-- Costos
	D.gePriceA,
	D.gePriceM,
	-- Precios
	D.gePriceAdult,
	D.gePriceMinor,
	D.gePriceExtraAdult,
	-- Campo que posiblemente se elimine
	D.gect,
	-- Folios
	D.geFolios,
	-- Regalo tipo venta
	D.geSale,
	-- Monedero electronico
	D.geInElectronicPurse,
	D.geConsecutiveElectronicPurse,
	-- Promociones de PVP
	D.geInPVPPromo,
	IsNull(gePVPPromotion, G.giPVPPromotion) as gePVPPromotion,
	-- Comentarios
	D.geComments,
	'' as Package
from GiftsReceiptsC D
	left join GiftsReceiptsPacks P on P.gkPack = D.gegi
	left join Gifts G on G.giID = D.gegi
where
	-- Recibo
	D.gegr = @Receipt
	-- Guardados en el monedero electronico y no cancelados aun
	and ((@ExternalProducts = 1 and P.gkInElectronicPurse = 1 and P.gkCancelElectronicPurse = 0)
	-- Guardados en promociones de PVP y no cancelados aun
	or (@ExternalProducts = 2 and P.gkInPVPPromo = 1 and P.gkCancelPVPPromo = 0))
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

