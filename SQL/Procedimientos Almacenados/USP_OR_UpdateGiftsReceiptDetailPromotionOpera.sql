if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_UpdateGiftsReceiptDetailPromotionOpera]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_UpdateGiftsReceiptDetailPromotionOpera]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Indica que un regalo de un recibo se guardo como promocion de Opera
** 
** [wtorres]	19/Jun/2014 Creado
**
*/
create procedure [dbo].[USP_OR_UpdateGiftsReceiptDetailPromotionOpera]
	@Receipt int,				-- Clave del recibo de regalos
	@Gift varchar(10),			-- Clave del regalo
	@PromotionOpera varchar(15)	-- Clave de la promocion de Opera
as
set nocount on

update GiftsReceiptsC
set geAsPromotionOpera = 1,
	gePromotionOpera = @PromotionOpera
where gegr = @Receipt and gegi = @Gift

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

