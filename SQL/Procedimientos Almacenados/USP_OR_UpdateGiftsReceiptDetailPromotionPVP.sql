if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_UpdateGiftsReceiptDetailPromotionPVP]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_UpdateGiftsReceiptDetailPromotionPVP]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Indica que a un regalo de un recibo se le guardo su promocion en PVP
** 
** [wtorres]	17/Jun/2014 Creado
**
*/
create procedure [dbo].[USP_OR_UpdateGiftsReceiptDetailPromotionPVP]
	@Receipt int,				-- Clave del recibo de regalos
	@Gift varchar(10),			-- Clave del regalo
	@PromotionPVP varchar(15)	-- Clave de la promocion de PVP
as
set nocount on

update GiftsReceiptsC
set geInPVPPromo = 1,
	gePVPPromotion = @PromotionPVP
where gegr = @Receipt and gegi = @Gift

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

