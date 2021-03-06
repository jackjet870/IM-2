if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetLeadSourceMarketMonthShows]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetLeadSourceMarketMonthShows]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por Lead Source, mercado y mes
** 
** [wtorres]	04/Feb/2010 Created
** [wtorres]	14/May/2010 Modified. Agregue el parametro @ConsiderDirectsAntesInOut
** [wtorres]	24/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [caduran]    16/Dic/2014 Modified. Se agregaron los parametros @LeadSources, @Program, @InOut, @WalkOut, @TourType, @ConsiderTourSale;
**							Se agrega columna Program
** [wtorres]	04/Jun/2015 Modified. Agregue el parametro @External
**
*/
create function [dbo].[UFN_OR_GetLeadSourceMarketMonthShows](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@InOut int = -1,					-- Filtro de In & Outs:
										--		-1. Sin filtro
										--		 0. No In & Outs
										--		 1. In & Outs
	@WalkOut int = -1,					-- Filtro de Walk Outs:
										--		-1. Sin filtro
										--		 0. No Walk Outs
										--		 1. Walk & Outs		
	@TourType int = 0,					-- Filtro de tipo de tour:
										--		0. Sin filtro
										--		1. Tours regulares
										--		2. Tours de cortesia
										--		3. Tours de rescate	
	@ConsiderDirectsAntesInOut bit = 0,	-- Indica si se debe considerar directas Antes In & Out
	@ConsiderTourSale bit = 0,			-- Indica si se debe considerar los shows con tour o venta
	@External int = 0,					-- Filtro de invitaciones externas
										--		0. Sin filtro
										--		1. Excluir invitaciones externas
										--		2. Solo invitaciones externas
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	LeadSource varchar(10),
	Market varchar(10),
	[Year] int,
	[Month] int,
	Program varchar(10),
	Shows int
)
as
begin

-- si no se debe basar en la fecha de llegada
if @BasedOnArrival = 0

	insert @Table
	select
		G.guls,
		G.gumk,
		Year(G.guShowD),
		Month(G.guShowD),
		L.lspg,
		Sum(case when @ConsiderQuinellas = 0 then 1 else G.guShowsQty end)
	from Guests G
		left join LeadSources L on L.lsID = G.guls
	where
		-- Fecha de show
		G.guShowD between @DateFrom and @DateTo
		-- No Quiniela Split
		and G.guQuinellaSplit = 0
		-- Programa
		and (@Program = 'ALL' or L.lspg = @Program)
		-- Originalmente disponibles
		and (G.guOriginAvail = 1 or G.guInvit = 1)
		-- No Directas no Antes In & Out
		and (@ConsiderDirectsAntesInOut = 0 or not (guDirect = 1 and guAntesIO = 0))
		-- Lead Sources
		and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ',')))
		-- In & Outs
		and (@InOut = -1 or G.guInOut = @InOut)
		-- Walk Outs
		and (@WalkOut = -1 or G.guWalkOut = @WalkOut)
		-- Filtro de tipo de tour
		and (@TourType = 0 or (@TourType = 1 and G.guTour = 1) or (@TourType = 2 and G.guCTour = 1) or (@TourType = 3 and G.guSaveProgram = 1))
		-- Con tour o venta
		and (@ConsiderTourSale = 0 or (G.guTour = 1 or G.guSale = 1))
		-- Invitaciones externas
		and (@External = 0
			or (@External = 1 and (L.lspg = 'OUT' or (L.lspg = 'IH' and G.guHReservID is not null)))
			or (@External = 2 and L.lspg = 'IH' and G.guHReservID is null))
	group by G.guls, G.gumk, Year(G.guShowD), Month(G.guShowD), L.lspg

-- si se debe basar en la fecha de llegada
else

	insert @Table
	select
		G.guls,
		G.gumk,
		Year(G.guCheckInD),
		Month(G.guCheckInD),
		L.lspg,
		Sum(case when @ConsiderQuinellas = 0 then 1 else G.guShowsQty end)
	from Guests G
		left join LeadSources L on L.lsID = G.guls
	where
		-- Fecha de llegada
		G.guCheckInD between @DateFrom and @DateTo
		-- Con show
		and G.guShow = 1
		-- No Quiniela Split
		and G.guQuinellaSplit = 0
		-- Programa
		and (@Program = 'ALL' or L.lspg = @Program)
		-- Originalmente disponibles
		and (G.guOriginAvail = 1 or G.guInvit = 1)
		-- No Directas no Antes In & Out
		and (@ConsiderDirectsAntesInOut = 0 or not (guDirect = 1 and guAntesIO = 0))
		-- Lead Sources
		and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ',')))
		-- In & Outs
		and (@InOut = -1 or G.guInOut = @InOut)
		-- Walk Outs
		and (@WalkOut = -1 or G.guWalkOut = @WalkOut)
		-- Filtro de tipo de tour
		and (@TourType = 0 or (@TourType = 1 and G.guTour = 1) or (@TourType = 2 and G.guCTour = 1) or (@TourType = 3 and G.guSaveProgram = 1))
		-- Con tour o venta
		and (@ConsiderTourSale = 0 or (G.guTour = 1 or G.guSale = 1))
		-- Invitaciones externas
		and (@External = 0
			or (@External = 1 and (L.lspg = 'OUT' or (L.lspg = 'IH' and G.guHReservID is not null)))
			or (@External = 2 and L.lspg = 'IH' and G.guHReservID is null))
	group by G.guls, G.gumk, Year(G.guCheckInD), Month(G.guCheckInD), L.lspg

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

