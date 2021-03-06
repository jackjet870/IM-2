if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferUpdateGuestsLastNames]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferUpdateGuestsLastNames]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Actualiza los apellidos de huespedes en el proceso de transferencia
** 
** [wtorres]	22/Abr/2009 Created
** [wtorres] 	25/Jun/2012 Modified. Ahora si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
** [wtorres] 	27/Ago/2015 Modified. Agregue el apellido de la reservacion
**
*/
create procedure [dbo].[USP_OR_TransferUpdateGuestsLastNames] 
as
set nocount on

-- actualizamos el apellido del huesped
update Guests
set guLastName1 = T.tLastName
from Guests G
	inner join osTransfer T on G.guHReservID = T.tHReservID and G.gulsOriginal = T.tls
	inner join LeadSources L on G.gulsOriginal = L.lsID
where (G.guLastName1 is null or G.guLastName1 <> T.tLastName) and T.tLastName <> ''
	-- actualizamos solo si aun no ha sido contactado
	and G.guInfo = 0
	-- si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
	and ((L.lsUseOpera = 0) or (L.lsUseOpera = 1 and G.guIdProfileOpera <> ''))

-- actualizamos el apellido de la reservacion
update Guests
set guLastNameOriginal = T.tLastName
from Guests G
	inner join osTransfer T on G.guHReservID = T.tHReservID and G.gulsOriginal = T.tls
	inner join LeadSources L on G.gulsOriginal = L.lsID
where (G.guLastNameOriginal is null or G.guLastNameOriginal <> T.tLastName) and T.tLastName <> ''
	-- si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
	and ((L.lsUseOpera = 0) or (L.lsUseOpera = 1 and G.guIdProfileOpera <> ''))
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

