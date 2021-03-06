if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_ReplaceGift]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_ReplaceGift]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Reemplaza la clave de un regalo por otra
** 
** [wtorres]	18/Dic/2013 Depurado
**
*/
create procedure [dbo].[USP_OR_ReplaceGift]
	@Old varchar(10),	-- Clave anterior a reemplazar
	@New varchar(10)	-- Clave nueva
as
set nocount on

-- agregamos un regalo identico al que queremos reemplazar
-- =============================================
insert into Gifts (giID, giN, giShortN, giPrice1, giPrice2, giPrice3, giPrice4, giPack, gigc, giInven, giA, giWFolio, giWPax, giO, giUnpack,
	giMaxQty, giMonetary, giAmount, giProductGiftsCard, giCountInPackage, giCountInCancelledReceipts, gipr, giPVPPromotion, giWCost,
	giPublicPrice, giAmountModifiable, giOperaTransactionType)
select @New, giN, giShortN, giPrice1, giPrice2, giPrice3, giPrice4, giPack, gigc, giInven, giA, giWFolio, giWPax, giO, giUnpack,
	giMaxQty, giMonetary, giAmount, giProductGiftsCard, giCountInPackage, giCountInCancelledReceipts, gipr, giPVPPromotion, giWCost,
	giPublicPrice, giAmountModifiable, giOperaTransactionType
from Gifts where giID = @Old

-- actualizamos las tablas dependientes con el nuevo regalo
-- =============================================
-- Regalos por locacion
update GiftsByLoc set glgi = @New where glgi = @Old

-- Historico del inventario
update GiftsInventory set gvgi = @New where gvgi = @Old

-- Historico de regalos
update GiftsLog set gggi = @New where gggi = @Old

-- Paquetes de regalos
update GiftsPacks set gpPack = @New where gpPack = @Old
update GiftsPacks set gpgi = @New where gpgi = @Old

-- Regalos de recibos de regalos
update GiftsReceiptsC set gegi = @New where gegi = @Old

-- Paquetes de regalos de recibos de regalos
update GiftsReceiptsPacks set gkPack = @New where gkPack = @Old
update GiftsReceiptsPacks set gkgi = @New where gkgi = @Old

-- Regalos de invitaciones
update InvitsGifts set iggi = @New where iggi = @Old

-- Movimientos de almacen
update WhsMovs set wmgi = @New where wmgi = @Old

-- eliminamos el regalo anterior
-- =============================================
delete from Gifts where giID = @Old

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

