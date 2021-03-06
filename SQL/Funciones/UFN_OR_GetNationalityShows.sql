if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetNationalityShows]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetNationalityShows]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por nacionalidad
** 
** [wtorres]	23/Nov/2009 Creado
** [wtorres]	17/Abr/2010 Agregue los parametros @ConsiderQuinellas, @ConsiderInOuts y @ConsiderDirectsAntesInOut
** [wtorres]	18/Nov/2010 Agregue el parametro @BasedOnArrival
** [wtorres]	19/Oct/2011 Reemplace el parametro @ConsiderInOuts por @InOut
** [wtorres]	04/Jul/2013 Agregue el parametro @IncludeSaveCourtesyTours
** [wtorres]	25/Jul/2013 Renombre el parametro @IncludeSaveCourtesyTours por @FilterSaveCourtesyTours
**
*/
create function [dbo].[UFN_OR_GetNationalityShows](
	@DateFrom datetime,						-- Fecha desde
	@DateTo datetime,						-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',		-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',				-- Claves de PRs
	@Program varchar(10) = 'ALL',			-- Clave de programa
	@ConsiderQuinellas bit = 0,				-- Indica si se debe considerar quinielas
	@FilterDeposit tinyint = 0,				-- Filtro de depositos:
											--		0. Sin filtro
											--		1. Con deposito (Deposits)
											--		2. Sin deposito (Flyers)
	@InOut int = -1,						-- Filtro de In & Outs:
											--		-1. Sin filtro
											--		 0. No In & Outs
											--		 1. In & Outs
	@ConsiderDirectsAntesInOut bit = 0,		-- Indica si se debe considerar directas Antes In & Out
	@FilterSaveCourtesyTours tinyint = 0,	-- Filtro de tours de rescate y cortesia
											--		0. Sin filtro
											--		1. Excluir tours de rescate y cortesia
											--		2. Excluir tours de rescate y cortesia sin venta
	@BasedOnArrival bit = 0					-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	Nationality varchar(25),
	Shows int
)
as
begin

insert @Table
select
	C.coNationality,
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
	-- No Quiniela Split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
	-- No Directas no Antes In & Out
	and (@ConsiderDirectsAntesInOut = 0 or not (G.guDirect = 1 and G.guAntesIO = 0))
	-- Tours de rescate y cortesia
	and (@FilterSaveCourtesyTours = 0
		or (@FilterSaveCourtesyTours = 1 and G.guSaveProgram = 0 and G.guCTour = 0)
		or (@FilterSaveCourtesyTours = 2 and ((G.guSaveProgram = 0 and G.guCTour = 0) or dbo.UFN_OR_GetGuestSales(G.guID) > 0)))
group by C.coNationality

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

