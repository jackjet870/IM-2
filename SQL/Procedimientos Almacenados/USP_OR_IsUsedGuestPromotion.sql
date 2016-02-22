if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_IsUsedGuestPromotion]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_IsUsedGuestPromotion]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Determina si un regalo ha sido usado como una promocion de Opera
** 
** [wtorres]	18/Jun/2014 Creado
**
*/
create procedure [dbo].[USP_OR_IsUsedGuestPromotion]
	@Receipt int,		-- Clave del recibo de regalos
	@Gift varchar(10)	-- Clave del regalo
as
set nocount on

if exists (select top 1 null from GuestsPromotions where gpgr = @Receipt and gpgi = @Gift and gpBalance < gpQty)
	select 1 as Used
else
	select 0 as Used

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

