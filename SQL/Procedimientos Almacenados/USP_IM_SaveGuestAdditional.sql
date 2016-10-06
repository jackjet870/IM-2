
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_IM_SaveGuestAdditional]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_IM_SaveGuestAdditional]
GO
SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO
/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Agrega los Guest Adicionales. De un guest principal.
**
** [edgrodriguez] 18/08/2016 Created
**
*/
create procedure [dbo].[USP_IM_SaveGuestAdditional]
	@guID			int,
	@AdditionalGuestID	varchar(max)
AS
SET NOCOUNT ON

DELETE FROM GuestsAdditional WHERE gagu=@guID;

INSERT INTO GuestsAdditional select @guID,item from split(@AdditionalGuestID, ',');

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO