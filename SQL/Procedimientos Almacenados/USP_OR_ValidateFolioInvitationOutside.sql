if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_ValidateFolioInvitationOutside]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_ValidateFolioInvitationOutside]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Valida que el folio de invitacion outhouse exista en el catalogo y que no haya sido utilizado en otra invitacion
** 
** [lchairez]	05/Dic/2013 Created
** [wtorres]	15/Ene/2014 Modified. Renombre la tabla y algunos campos del catalogo de folios de invitacion outhouse
** [wtorres]	06/Mar/2015 Modified. Elimine el parametro @Folio. Ahora solo se valida la serie y el numero
** 
*/
create procedure [dbo].[USP_OR_ValidateFolioInvitationOutside]
	@Serie varchar(5),
	@Number int,
	@GuestID int
as
set nocount on

declare
	@GuestIDUsed as int,
	@Message varchar(500)

set @Message = ''

-- validamos que el folio de invitacion se encuentre en el catalogo de folios de invitacion outhouse
if not exists(select top 1 null from FolioInvitationsOutside where fiA = 1 and fiSerie = @Serie and @Number between fiFrom and fiTo)
	
	set @Message = 'The Outhouse Invitation Folio does not exists'
	
-- validamos que el folio no haya sido utilizado por otra invitacion
else begin
	
	select top 1 @GuestIDUsed = guID from Guests where guID <> @GuestID and dbo.UFN_OR_EqualFolio(guOutInvitNum, @Serie, @Number) = 1
	if @GuestIDUsed is not null
		set @Message = 'The Outhouse Invitation Folio was already used for Guest ID: ' + Cast(@GuestIDUsed as varchar)		
end

-- devolvemos el mensaje de error
select @Message as [Message]
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

