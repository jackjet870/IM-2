if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferDeleteReservationsCancelled]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferDeleteReservationsCancelled]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Elimina las reservaciones canceladas en el proceso de transferencia
** 
** [wtorres]	22/Abr/2009 Creado
** [wtorres]	23/Nov/2010 Ahora valida que la reservacion cancelada no pertenezca a un grupo de huespedes ni tenga notas de PR
** [wtorres] 	25/Jun/2012 Ahora si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
**
*/
create procedure [dbo].[USP_OR_TransferDeleteReservationsCancelled] 
as
set nocount on

delete from Guests
where
	-- No contactado
	guInfo = 0
	-- Que no pertenezca a un grupo de huespedes
	and guGroup = 0
	-- Sin notas de PR
	and guPRNote = 0
	-- Con reservacion cancelada
	and exists (
		select T.tHReservID
		from osTransfer T
			inner join LeadSources L on T.tls = L.lsID
		where T.tHReservID = guHReservID and T.tls = gulsOriginal and T.tGuestStatus = 'X'
			-- si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
			and ((L.lsUseOpera = 0) or (L.lsUseOpera = 1 and T.tIdProfileOpera <> '')))

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

