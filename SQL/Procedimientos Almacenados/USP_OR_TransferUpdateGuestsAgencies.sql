if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferUpdateGuestsAgencies]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferUpdateGuestsAgencies]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Actualiza las agencias de huespedes en el proceso de transferencia
** 
** [wtorres]	21/Dic/2011 Creado
** [wtorres] 	25/Jun/2012 Ahora si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
**
*/
create procedure [dbo].[USP_OR_TransferUpdateGuestsAgencies]
as

update Guests
set guag = T.toagID
from Guests G
	inner join osTransfer T on G.guHReservID = T.tHReservID and G.gulsOriginal = T.tls
	inner join LeadSources L on G.gulsOriginal = L.lsID
where (G.guag is null or G.guag <> T.toagID) and T.toagID <> ''
	-- actualizamos solo si aun no tiene invitacion
	and G.guInvit = 0
	-- si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
	and ((L.lsUseOpera = 0) or (L.lsUseOpera = 1 and G.guIdProfileOpera <> ''))

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

