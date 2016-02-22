if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferUpdateGuestsAvailabilityUnavailableMotives]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferUpdateGuestsAvailabilityUnavailableMotives]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Actualiza la disponibilidad y los motivos de indisponibilidad de huespedes en el proceso de transferencia
**
** [wtorres] 	22/Abr/2009 Creado
** [wtorres] 	03/Nov/2011 Ahora tambien actualiza el campo de disponible por sistema
** [wtorres] 	25/Jun/2012 Ahora si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
** [wtorres]	27/Ago/2012 Ahora se valida la disponibilidad mediante el campo tum
**
*/
create procedure [dbo].[USP_OR_TransferUpdateGuestsAvailabilityUnavailableMotives]
as
set nocount on

update Guests
-- es disponible si tiene motivo de indisponibilidad 0 - AVAILABLE y tiene Check In
set guAvail = case when T.tum = 0 and G.guCheckIn = 1 then 1 else 0 end,
	guOriginAvail = case when T.tum = 0 and G.guCheckIn = 1 then 1 else 0 end,
	guAvailBySystem = case when T.tum = 0 and G.guCheckIn = 1 then 1 else 0 end,
	guum = T.tum
from Guests G
	inner join osTransfer T on G.guHReservID = T.tHReservID and G.gulsOriginal = T.tls
	inner join LeadSources L on G.gulsOriginal = L.lsID
where
	-- Motivo de indisponibilidad diferente
	G.guum <> T.tum
	-- No contactado
	and G.guInfo = 0
	-- Motivo disponible
	and (G.guum = 0
	-- Motivo no disponible y ningun PR ha modificado su disponibilidad
	or (G.guum > 0 and G.guPRAvail is null))
	-- si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
	and ((L.lsUseOpera = 0) or (L.lsUseOpera = 1 and G.guIdProfileOpera <> ''))

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

