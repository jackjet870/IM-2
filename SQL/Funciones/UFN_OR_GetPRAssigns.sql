if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetPRAssigns]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetPRAssigns]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el número de asignaciones por PR
** 
** [wtorres]	18/Sep/2009 Creado
** [wtorres]	23/Sep/2009 Convertido a función. Agregué el parámetro @LeadSources
**
*/
create function [dbo].[UFN_OR_GetPRAssigns](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000)	-- Clave de los Lead Sources
)
returns @Table table (
	PR varchar(10),
	Assigns int
)
as
begin

insert @Table
select
	guPRAssign,
	Count(*)
from Guests
where
	-- PR asignado
	guPRAssign is not null
	-- Fecha de llegada
	and guCheckInD between @DateFrom and @DateTo
	-- No rebook
	and guRef is null
	-- Con Check In
	and guCheckIn = 1
	-- No antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
group by guPRAssign

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

