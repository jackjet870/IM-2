if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetGuestPromotion]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetGuestPromotion]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta una promocion de Opera
** 
** [wtorres]	18/Jun/2014 Created
** [wtorres]	12/Jul/2014 Modified. Agregue los campos de fecha y notificado
**
*/
create procedure [dbo].[USP_OR_GetGuestPromotion]
	@Receipt int,		-- Clave del recibo de regalos
	@Gift varchar(10)	-- Clave del regalo
as
set nocount on

select gpgu, gpgr, gpgi, gpPromotion, gpPromotionOpera, gpQty, gpBalance, gpD, gpHotel, gpFolio, gpNotified
from GuestsPromotions
where gpgr = @Receipt and gpgi = @Gift

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

