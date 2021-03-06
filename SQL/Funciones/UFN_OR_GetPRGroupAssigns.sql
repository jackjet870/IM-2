if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetPRGroupAssigns]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetPRGroupAssigns]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el número de asignaciones por PR y grupo
** 
** [wtorres]	11/Ago/2009 Creado
**
*/
create function [dbo].[UFN_OR_GetPRGroupAssigns](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000)	-- Clave de los Lead Sources
)
returns @Table table (
	PR varchar(10),
	[Group] int,
	Assigns int
)
as
begin

insert @Table
select
	G.guPRAssign,
	I.gjgx,
	Count(*)
from Guests G
	left join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- PR asignado
	G.guPRAssign is not null
	-- Fecha de llegada
	and G.guCheckInD between @DateFrom and @DateTo
	-- No rebook
	and G.guRef is null
	-- Con Check In
	and G.guCheckIn = 1
	-- No antes In & Out
	and G.guAntesIO = 0
	-- Lead Source
	and G.guls in (select item from split(@LeadSources, ','))
group by G.guPRAssign, I.gjgx

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

