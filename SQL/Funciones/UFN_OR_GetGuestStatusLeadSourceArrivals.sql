if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetGuestStatusLeadSourceArrivals]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetGuestStatusLeadSourceArrivals]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el número de llegadas por estatus de huésped y Lead Source
** 
** [wtorres]	27/Abr/2010 Creado
**
*/
create function [dbo].[UFN_OR_GetGuestStatusLeadSourceArrivals](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000)		-- Claves de Lead Sources
)
returns @Table table (
	GuestStatus varchar(5),
	LeadSource varchar(10),
	Arrivals int
)
as
begin

insert @Table
select
	guGStatus,
	guls,
	Count(*)
from Guests
where
	-- Fecha de llegada
	guCheckInD between @DateFrom and @DateTo
	-- Con Check In
	and guCheckIn = 1
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
group by guGStatus, guls

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

