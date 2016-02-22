if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetGiftsInvitationWithoutReceipt]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetGiftsInvitationWithoutReceipt]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta los regalos de una invitacion sin recibo de regalos.
** Estos regalos serviran como base para un nuevo recibo de regalos.
** 
** [wtorres]	18/Jun/2014 Created
** [gmaya]		22/Ago/2014 Modified. Agregue la cantidad de adultos extra y los precios de menores y adultos extra
** [wtorres]	10/Sep/2014 Modified. Agregue el campo que indica si el regalo es venta
** [wtorres]	13/Sep/2014 Modified. Agregue el parametro @Package
**
*/
create procedure [dbo].[USP_OR_GetGiftsInvitationWithoutReceipt]
	@Guest int,			-- Clave del huesped
	@Package bit = 0	-- Indica si se desean los paquetes de regalos
as
set nocount on

select
	'' as gegr,
	I.igQty as geQty,
	'' as QtyUnit,
	I.iggi as gegi,
	-- Pax
	I.igAdults as geAdults,
	I.igMinors as geMinors,
	I.igExtraAdults as geExtraAdults,
	-- Costos
	Cast((I.igQty * (I.igAdults + I.igExtraAdults) * G.giPrice1) as money) as gePriceA,
	Cast((I.igQty * I.igMinors * G.giPrice2) as money) as gePriceM,
	-- Precios
	Cast((I.igQty * I.igAdults * G.giPublicPrice) as money) as gePriceAdult,
	Cast((I.igQty * I.igMinors * G.giPriceMinor) as money) as gePriceMinor, 
	Cast((I.igQty * I.igExtraAdults * G.giPriceExtraAdult) as money) as gePriceExtraAdult,
	-- Campos que posiblemente se eliminen
	I.igct as gect,
	'' as geCxC,
	Cast(0 as money) as geCharge,
	-- Folios
	I.igFolios as geFolios,
	-- Regalo tipo venta
	G.giSale as geSale,
	-- Monedero electronico
	Cast(0 as bit) as geInElectronicPurse,
	'' as geConsecutiveElectronicPurse,
	Cast(0 as bit) as geCancelElectronicPurse,
	-- Promociones de PVP
	Cast(0 as bit) as geInPVPPromo,
	Cast(0 as bit) as geCancelPVPPromo,
	-- Cargos a habitacion de Opera
	Cast(0 as bit) as geInOpera,
	-- Promociones de Opera
	Cast(0 as bit) as geAsPromotionOpera,
	-- Comentarios
	I.igComments as geComments,
	'' as Package
from InvitsGifts I
	left join Gifts G on G.giID = I.iggi
where
	-- Clave del huesped
	I.iggu = @Guest
	-- Regalos sin recibo
	and I.iggr is null
	-- Paquetes de regalos
	and (@Package = 0 or G.giPack = 1)
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

