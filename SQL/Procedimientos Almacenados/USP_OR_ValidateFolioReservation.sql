if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_ValidateFolioReservation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_ValidateFolioReservation]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Valida que el folio de reservacion no haya sido utilizado en otra invitacion
** 
** [wtorres]	15/Ene/2014 Created
** [wtorres]	24/Feb/2014 A los rebooks no se les valida su numero de reservacion porque no se les puede modificar
** 
*/
create procedure [dbo].[USP_OR_ValidateFolioReservation]
	@LeadSource varchar(10),
	@Folio varchar(15),
	@GuestID int
as
set nocount on

declare @GuestIDUsed int

-- si no es un rebook, validamos que el folio no haya sido utilizado por otra invitacion
if (select guRef from Guests where guID = @GuestID) is null 
	and exists(select guID from Guests where gulsOriginal = @LeadSource and guHReservID = @Folio and guID <> @GuestID) begin
	
	select top 1 @GuestIDUsed = guID from Guests where gulsOriginal = @LeadSource and guHReservID = @Folio order by guID desc
	
	select 'The Reservation Folio was already used for Guest ID: ' + Cast(@GuestIDUsed as varchar)
		
end else
	select ''

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

