if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetNationalityArrivals]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetNationalityArrivals]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el número de llegadas por nacionalidad
** 
** [wtorres]	17/Abr/2010 Creado
**
*/
create function [dbo].[UFN_OR_GetNationalityArrivals](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',		-- Claves de PRs
	@Program varchar(10) = 'ALL'	-- Clave de programa
)
returns @Table table (
	Nationality varchar(25),
	Arrivals int
)
as
begin

insert @Table
select
	C.coNationality,
	Count(*)
from Guests G
	inner join Countries C on G.guco = C.coID
	inner join LeadSources L on L.lsID = G.guls
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
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
group by C.coNationality

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

