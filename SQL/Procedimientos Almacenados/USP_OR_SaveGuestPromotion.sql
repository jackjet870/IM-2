if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_SaveGuestPromotion]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_SaveGuestPromotion]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Guarda una promocion de Opera
** 
** [wtorres]	18/Jun/2014 Created
** [wtorres]	12/Jul/2014 Modified. Agregue el campo de fecha
**
*/
create procedure [dbo].[USP_OR_SaveGuestPromotion]
	@Receipt int,					-- Clave del recibo de regalos
	@Gift varchar(10),				-- Clave del regalo
	@PromotionOpera varchar(20),	-- Clave de la promocion de Opera
	@Guest int,						-- Clave del huesped
	@Quantity int,					-- Cantidad
	@Date datetime					-- Fecha
as
set nocount on

-- si no existe la promocion, la agregamos
if not exists (select top 1 null from GuestsPromotions where gpgr = @Receipt and gpgi = @Gift)
	insert into GuestsPromotions (gpgu, gpgr, gpgi, gpPromotionOpera, gpQty, gpBalance, gpD)
	values (@Guest, @Receipt, @Gift, @PromotionOpera, @Quantity, @Quantity, @Date)

-- si existe la promocion, la actualizamos
else
	update GuestsPromotions
	set gpQty = @Quantity
	where gpgr = @Receipt and gpgi = @Gift

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

