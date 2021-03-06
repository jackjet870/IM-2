if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetGuestStatusLeadSourceAvailables]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetGuestStatusLeadSourceAvailables]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el número de disponibles por estatus de huésped y Lead Source
** 
** [wtorres]	27/Abr/2010 Creado
** [wtorres]	08/Jun/2010 Eliminé el Join con la tabla de LeadSources
**
*/
create function [dbo].[UFN_OR_GetGuestStatusLeadSourceAvailables](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0		-- Indica si se debe considerar quinielas
)
returns @Table table (
	GuestStatus varchar(5),
	LeadSource varchar(10),
	Availables int
)
as
begin

insert @Table
select
	guGStatus,
	guls,
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then 1 else guShowsQty end)
from Guests
where
	-- Fecha de llegada
	guCheckInD between @DateFrom and @DateTo
	-- Con Check In
	and guCheckIn = 1
	-- Disponible
	and guAvail = 1
	-- No Rebook
	and guRef is null
	-- Contactado
	and guInfo = 1
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

