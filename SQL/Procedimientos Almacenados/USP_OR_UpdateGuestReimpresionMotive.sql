if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_UpdateGuestReimpresionMotive]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_UpdateGuestReimpresionMotive]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Actualiza el contador de reimpresion y el motivo del Guest Registration de un huesped
** 
** [wtorres]	12/Sep/2014 Created
**
*/
create procedure [dbo].[USP_OR_UpdateGuestReimpresionMotive]
	@Guest int,					-- Clave del huesped
	@ReimpresionMotive tinyint	-- Clave del motivo de reimpresion
as
set nocount on

update Guests
set guReimpresion = IsNull(guReimpresion, 0) + 1,
	gurm = @ReimpresionMotive
where guID = @Guest

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

