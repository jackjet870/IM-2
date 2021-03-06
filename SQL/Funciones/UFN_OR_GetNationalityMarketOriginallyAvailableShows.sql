if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetNationalityMarketOriginallyAvailableShows]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetNationalityMarketOriginallyAvailableShows]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por nacionalidad, mercado y originalmente disponible
** 
** [wtorres]	11/Oct/2010	Creado
** [wtorres]	18/Nov/2010 Agregue el parametro @BasedOnArrival
** [wtorres]	19/Jul/2013 Agregue el parametro @IncludeSaveCourtesyTours
** [wtorres]	25/Jul/2013 Renombre el parametro @IncludeSaveCourtesyTours por @FilterSaveCourtesyTours
**
*/
create function [dbo].[UFN_OR_GetNationalityMarketOriginallyAvailableShows](
	@DateFrom datetime,						-- Fecha desde
	@DateTo datetime,						-- Fecha hasta
	@LeadSources varchar(8000),				-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',				-- Claves de PRs
	@Program varchar(10) = 'ALL',			-- Clave de programa
	@ConsiderQuinellas bit = 0,				-- Indica si se debe considerar quinielas
	@ConsiderDirectsAntesInOut bit = 0,		-- Indica si se debe considerar directas Antes In & Out
	@FilterSaveCourtesyTours tinyint = 0,	-- Filtro de tours de rescate y cortesia
											--		0. Sin filtro
											--		1. Excluir tours de rescate y cortesia
											--		2. Excluir tours de rescate y cortesia sin venta
	@BasedOnArrival bit = 0					-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	Nationality varchar(25),
	Market varchar(10),
	OriginallyAvailable varchar(30),
	Shows int
)
as
begin

insert @Table
select
	C.coNationality,
	G.gumk,
	-- Si tiene invitacion, considerarlo en originalmente disponible
	case when (G.guOriginAvail | G.guInvit) = 1 then 'ORIGINALLY AVAILABLES' else 'ORIGINALLY UNAVAILABLES' end,
	Sum(case when @ConsiderQuinellas = 0 then 1 else G.guShowsQty end)
from Guests G
	inner join Countries C on G.guco = C.coID
	inner join LeadSources L on L.lsID = G.guls
where
	-- Fecha de show
	((@BasedOnArrival = 0 and G.guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Con show
	and G.guShow = 1))
	-- No quinielas split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- Lead Source
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ',')))
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- No Directas no Antes In & Out
	and (@ConsiderDirectsAntesInOut = 0 or not (G.guDirect = 1 and G.guAntesIO = 0))
	-- Tours de rescate y cortesia
	and (@FilterSaveCourtesyTours = 0
		or (@FilterSaveCourtesyTours = 1 and G.guSaveProgram = 0 and G.guCTour = 0)
		or (@FilterSaveCourtesyTours = 2 and ((G.guSaveProgram = 0 and G.guCTour = 0) or dbo.UFN_OR_GetGuestSales(G.guID) > 0)))
group by C.coNationality, G.gumk, (G.guOriginAvail | G.guInvit)

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

