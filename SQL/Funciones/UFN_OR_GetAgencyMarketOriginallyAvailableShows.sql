if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetAgencyMarketOriginallyAvailableShows]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetAgencyMarketOriginallyAvailableShows]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por agencia, mercado y originalmente disponible
** 
** [wtorres]	23/Oct/2009 Created
** [wtorres]	16/Abr/2010 Modified. Agregue el parametro @ConsiderDirectsAntesInOut
** [wtorres]	15/Oct/2010 Modified. Agregue el parametro @BasedOnArrival
** [alesanchez]	04/Sep/2013 Modified. Agregue el parametros @InOut, @WalkOut, @TourType, @ConsiderTourSale
** [wtorres]	04/Jun/2015 Modified. Agregue el parametro @External
**
*/
create function [dbo].[UFN_OR_GetAgencyMarketOriginallyAvailableShows](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000),			-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@ConsiderNights bit = 0,			-- Indica si se debe considerar el numero de noches
	@NightsFrom int = 0,				-- Numero de noches desde
	@NightsTo int = 0,					-- Numero de noches hasta
	@OnlyQuinellas bit = 0,				-- Indica si se desean solo las quinielas
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
	Agency varchar(35),
	Market varchar(10),
	OriginallyAvailable varchar(30),
	Shows int
)
as
begin

insert @Table
select
	G.guag,
	G.gumk,
	-- Si tiene invitacion, considerarlo en originalmente disponible
	case when (G.guOriginAvail | G.guInvit) = 1 then 'ORIGINALLY AVAILABLES' else 'ORIGINALLY UNAVAILABLES' end,
	Sum(case when @ConsiderQuinellas = 0 then 1 else G.guShowsQty end)
from Guests G
	left join LeadSources L on L.lsID = G.guls
where
	-- Fecha de show
	((@BasedOnArrival = 0 and G.guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Con show
	and G.guShow = 1))
	-- Lead Source
	and G.guls in (select item from split(@LeadSources, ','))
	-- No quinielas split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- Numero de noches
	and (@ConsiderNights = 0 or DateDiff(Day, G.guCheckInD, G.guCheckOutD) between @NightsFrom and @NightsTo)
	-- Quinielas
	and (@OnlyQuinellas = 0 or G.guQuinella = 1)
	-- No Directas no Antes In & Out
	and (@ConsiderDirectsAntesInOut = 0 or not (G.guDirect = 1 and G.guAntesIO = 0))
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
group by G.guag, G.gumk, (G.guOriginAvail | G.guInvit)

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

