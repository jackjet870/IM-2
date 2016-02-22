if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferUpdateGuestsCheckOutsEarly]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferUpdateGuestsCheckOutsEarly]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Verifica las salidas anticipadas de huespedes en el proceso de transferencia
**
** [wtorres] 	22/Abr/2009 Creado
** [wtorres]	13/Jul/2010 Elimine el parametro @Date
** [wtorres] 	25/Jun/2012 Ahora si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
**
*/
create procedure [dbo].[USP_OR_TransferUpdateGuestsCheckOutsEarly]
as
set nocount on

-- obtenemos la fecha actual
declare @Date datetime
set @Date = Convert(varchar, GetDate(), 112)

-- actualizamos la fecha de salida de los huespedes que salieron anticipadamente
update Guests
set guCheckOutD = @Date
from Guests G
	inner join osTransfer T on G.guHReservID = T.tHReservID and G.gulsOriginal = T.tls
	inner join LeadSources L on G.gulsOriginal = L.lsID
where T.tGuestStatus = 'O' and G.guCheckOutD > @Date
	-- actualizamos solo si aun no tiene invitacion
	and G.guInvit = 0
	-- si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
	and ((L.lsUseOpera = 0) or (L.lsUseOpera = 1 and G.guIdProfileOpera <> ''))

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

