if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferUpdateGuestsCheckInDates]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferUpdateGuestsCheckInDates]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Actualiza las fechas de llegada de huespedes en el proceso de transferencia
**
** [wtorres] 	22/Abr/2009 Creado
** [wtorres] 	14/Oct/2011 Ahora la restriccion para actualizar la fecha de llegada es que no haya sido invitado en lugar de que no haya hecho
**							Check In
** [wtorres] 	25/Jun/2012 Ahora si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
**
*/
create procedure [dbo].[USP_OR_TransferUpdateGuestsCheckInDates] 
as
set nocount on

update Guests
set guCheckInD = T.tCheckInD
from Guests G
	inner join osTransfer T on G.guHReservID = T.tHReservID and G.gulsOriginal = T.tls
	inner join LeadSources L on G.gulsOriginal = L.lsID
where G.guCheckInD <> T.tCheckInD
	-- actualizamos solo si aun no tiene invitacion
	and G.guInvit = 0
	-- si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
	and ((L.lsUseOpera = 0) or (L.lsUseOpera = 1 and G.guIdProfileOpera <> ''))

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

