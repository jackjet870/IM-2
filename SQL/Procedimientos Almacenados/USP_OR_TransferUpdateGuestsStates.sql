if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferUpdateGuestsStates]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferUpdateGuestsStates]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Actualiza los estados de huespedes en el proceso de transferencia
**
** [wtorres] 	18/Ago/2010 Creado
** [wtorres] 	25/Jun/2012 Ahora si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
**
*/
create procedure [dbo].[USP_OR_TransferUpdateGuestsStates] 
as
set nocount on

update Guests
set guState = T.tState
from Guests G
	inner join osTransfer T on G.guHReservID = T.tHReservID and G.gulsOriginal = T.tls
	inner join LeadSources L on G.gulsOriginal = L.lsID
where (G.guState is null or G.guState <> T.tState) and T.tState <> ''
	-- actualizamos solo si aun no tiene show
	and G.guShow = 0
	-- si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
	and ((L.lsUseOpera = 0) or (L.lsUseOpera = 1 and G.guIdProfileOpera <> ''))

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

