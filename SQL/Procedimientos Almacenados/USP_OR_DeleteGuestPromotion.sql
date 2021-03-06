if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_DeleteGuestPromotion]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_DeleteGuestPromotion]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Elimina una promocion
** 
** [wtorres]	19/Jun/2014 Creado
**
*/
create procedure [dbo].[USP_OR_DeleteGuestPromotion]
	@Receipt int,		-- Clave del recibo de regalos
	@Gift varchar(10)	-- Clave del regalo
as
set nocount on

delete from GuestsPromotions where gpgr = @Receipt and gpgi = @Gift

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

