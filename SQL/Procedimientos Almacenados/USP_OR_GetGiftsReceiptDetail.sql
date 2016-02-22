if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetGiftsReceiptDetail]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetGiftsReceiptDetail]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta los regalos de un recibo
** 
** [wtorres]	13/Sep/2014 Created
**
*/
create procedure [dbo].[USP_OR_GetGiftsReceiptDetail]
	@Receipt int,		-- Clave del recibo de regalos
	@Package bit = 0	-- Indica si se desean los paquetes de regalos
as
set nocount on

select
	D.gegr,
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
	-- Campos que posiblemente se eliminen
	D.gect,
	D.geCxC,
	D.geCharge,
	-- Folios
	D.geFolios,
	-- Regalo tipo venta
	D.geSale,
	-- Monedero electronico
	D.geInElectronicPurse,
	D.geConsecutiveElectronicPurse,
	D.geCancelElectronicPurse,
	-- Promociones de PVP
	D.geInPVPPromo,
	D.geCancelPVPPromo,
	-- Cargos a habitacion de Opera
	D.geInOpera,
	-- Promociones de Opera
	D.geAsPromotionOpera,
	-- Comentarios
	D.geComments,
	'' as Package
from GiftsReceiptsC D
	left join Gifts G on G.giID = D.gegi
where
	-- Recibo
	D.gegr = @Receipt
	-- Paquetes de regalos
	and (@Package = 0 or G.giPack = 1)
order by G.giN
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

