if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetGroupArrivals]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetGroupArrivals]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el número de llegadas por grupo
** 
** [wtorres]	21/Jul/2010 Creado
**
*/
create function [dbo].[UFN_OR_GetGroupArrivals](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000)	-- Claves de Lead Sources
)
returns @Table table (
	[Group] int,
	Arrivals int
)
as
begin

insert @Table
select
	I.gjgx,
	Count(*)
from Guests G
	inner join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- Fecha de llegada
	G.guCheckInD between @DateFrom and @DateTo
	-- Con Check In
	and G.guCheckIn = 1
	-- No Rebook
	and G.guRef is null
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Lead Source
	and G.guls in (select item from split(@LeadSources, ','))
group by I.gjgx

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

