if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetGiftsReceiptPackage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetGiftsReceiptPackage]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta los items de un paquete de un recibo
** 
** [wtorres]	15/Sep/2014 Created
**
*/
create procedure [dbo].[USP_OR_GetGiftsReceiptPackage]
	@Receipt int,			-- Clave del recibo de regalos
	@Package varchar(10)	-- Clave del paquete de regalos
as
set nocount on

select
	P.gkgr,
	P.gkPack,
	P.gkgi,
	P.gkQty,
	-- Pax
	P.gkAdults,
	P.gkMinors,
	P.gkExtraAdults,
	-- Costos
	P.gkPriceA,
	P.gkPriceM,
	-- Precios
	P.gkPriceMinor,
	P.gkPriceAdult,
	P.gkPriceExtraAdult,
	-- Folios
	P.gkFolios,
	-- Monedero electronico
	P.gkInElectronicPurse,
	P.gkConsecutiveElectronicPurse,
	P.gkCancelElectronicPurse,
	-- Promociones de PVP
	P.gkInPVPPromo,
	P.gkCancelPVPPromo,
	-- Cargos a habitacion de Opera
	P.gkInOpera,
	-- Promociones de Opera
	P.gkAsPromotionOpera,
	P.gkPromotionOpera,
	-- Comentarios
	P.gkComments
from GiftsReceiptsPacks P
	left join Gifts G on G.giID = P.gkgi
where
	-- Recibo
	P.gkgr = @Receipt
	-- Paquete
	and P.gkPack = @Package
	-- Items activos
	and G.giA = 1
order by G.giN
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

