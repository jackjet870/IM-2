if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferUpdateGuestsCheckIns]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferUpdateGuestsCheckIns]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Actualiza los check ins de huespedes en el proceso de transferencia
**
** [wtorres] 	22/Abr/2009 Creado
** [wtorres] 	25/Jun/2012 Ahora si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
**
*/
create procedure [dbo].[USP_OR_TransferUpdateGuestsCheckIns] 
as
set nocount on

update Guests
set guCheckIn = 1
from Guests G
	inner join osTransfer T on G.guHReservID = T.tHReservID and G.gulsOriginal = T.tls
	inner join LeadSources L on G.gulsOriginal = L.lsID
where G.guCheckIn = 0 and T.tGuestStatus = 'I'
	-- si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
	and ((L.lsUseOpera = 0) or (L.lsUseOpera = 1 and G.guIdProfileOpera <> ''))

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

