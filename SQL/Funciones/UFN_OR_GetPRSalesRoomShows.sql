if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetPRSalesRoomShows]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetPRSalesRoomShows]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por PR y sala
** 
** [wtorres]		30/Oct/2009 Created
** [wtorres]		01/Abr/2010 Modified. Agregue los parametros @ConsiderDirects y @ConsiderInOuts
** [wtorres]		16/Abr/2010 Modified. Agregue los parametros @PRs y @Program
** [wtorres]		19/Oct/2011 Modified. Reemplace el parametro @ConsiderInOuts por @InOut
** [wtorres]		16/Nov/2011 Modified. Agregue los parametros @WalkOut, @TourType y @ConsiderTourSale
** [axperez]		04/Dic/2013 Modified. Agregue los parametros @ConsiderDirectsAntesInOut y @BasedOnArrival
** [lormartinez]	22/Sep/2014 Modified. Se agrega parametro para filtro basado en fecha de booking
** [wtorres]		15/Jul/2015 Modified. Correccion de error. No estaba contando solo los shows cuando se basaba en la fecha de booking
** [edgrodriguez]	05/May/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/
CREATE function [dbo].[UFN_OR_GetPRSalesRoomShows](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000),			-- Clave de los Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@FilterDeposit tinyint = 0,			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
	@ConsiderDirects bit = 0,			-- Indica si se debe considerar directas
	@InOut int = -1,					-- Filtro de In & Outs:
										--		-1. Sin filtro
										--		 0. No In & Outs
										--		 1. In & Outs
	@WalkOut int = -1,					-- Filtro de Walk Outs:
										--		-1. Sin filtro
										--		 0. No Walk Outs
										--		 1. Walk Outs
	@TourType int = 0,					-- Filtro de tipo de tour:
										--		0. Sin filtro
										--		1. Tours regulares
										--		2. Tours de cortesia
										--		3. Tours de rescate
	@ConsiderDirectsAntesInOut bit = 0,	-- Indica si se debe considerar directas Antes In & Out
	@ConsiderTourSale bit = 0,			-- Indica si se debe considerar los shows con tour o venta
	@BasedOnArrival bit = 0,			-- Indica si se debe basar en la fecha de llegada
	@BasedOnBooking bit = 0				-- Indica si se debe basar en la fecha de booking
)
returns @Table table (
	PR varchar(10),
	SalesRoom varchar(10),
	Shows money
)
as
begin

insert @Table

-- Shows (PR 1)
-- =============================================
select
	G.guPRInvit1,
	G.gusr,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) * case when @ConsiderQuinellas = 0 then 1 else G.guShowsQty end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- PR 1
	G.guPRInvit1 is not null
	-- Fecha de show
	and ((@BasedOnArrival = 0 and @BasedOnBooking = 0 and G.guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (((@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo)
	-- Fecha de booking
	or (@BasedOnBooking = 1 and G.guBookD between @DateFrom and @DateTo))
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
	-- No Directas
	and (@ConsiderDirects = 0 or G.guDirect = 0)
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
	-- Walk Outs
	and (@WalkOut = -1 or G.guWalkOut = @WalkOut)
	-- Filtro de tipo de tour
	and (@TourType = 0 or (@TourType = 1 and G.guTour = 1) or (@TourType = 2 and G.guCTour = 1) or (@TourType = 3 and G.guSaveProgram = 1))
	-- No Directas no Antes In & Out
	and (@ConsiderDirectsAntesInOut = 0 or not (G.guDirect = 1 and G.guAntesIO = 0))
	-- Con tour o venta
	and (@ConsiderTourSale = 0 or (G.guTour = 1 or G.guSale = 1))
group by G.guPRInvit1, G.gusr

-- Shows (PR 2)
-- =============================================
union all
select
	G.guPRInvit2,
	G.gusr,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) * case when @ConsiderQuinellas = 0 then 1 else G.guShowsQty end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- PR 2
	G.guPRInvit2 is not null
	-- Fecha de show
	and ((@BasedOnArrival = 0 and @BasedOnBooking = 0 and G.guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (((@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo)
	-- Fecha de booking
	or (@BasedOnBooking = 1 and G.guBookD between @DateFrom and @DateTo))
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
	-- No Directas
	and (@ConsiderDirects = 0 or G.guDirect = 0)
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
	-- Walk Outs
	and (@WalkOut = -1 or G.guWalkOut = @WalkOut)
	-- Filtro de tipo de tour
	and (@TourType = 0 or (@TourType = 1 and G.guTour = 1) or (@TourType = 2 and G.guCTour = 1) or (@TourType = 3 and G.guSaveProgram = 1))
	-- No Directas no Antes In & Out
	and (@ConsiderDirectsAntesInOut = 0 or not (G.guDirect = 1 and G.guAntesIO = 0))
	-- Con tour o venta
	and (@ConsiderTourSale = 0 or (G.guTour = 1 or G.guSale = 1))
group by G.guPRInvit2, G.gusr

-- Shows (PR 3)
-- =============================================
union all
select
	G.guPRInvit3,
	G.gusr,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) * case when @ConsiderQuinellas = 0 then 1 else G.guShowsQty end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- PR 3
	G.guPRInvit3 is not null
	-- Fecha de show
	and ((@BasedOnArrival = 0 and @BasedOnBooking = 0 and G.guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (((@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo)
	-- Fecha de booking
	or (@BasedOnBooking = 1 and G.guBookD between @DateFrom and @DateTo))
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
	-- No Directas
	and (@ConsiderDirects = 0 or G.guDirect = 0)
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
	-- Walk Outs
	and (@WalkOut = -1 or G.guWalkOut = @WalkOut)
	-- Filtro de tipo de tour
	and (@TourType = 0 or (@TourType = 1 and G.guTour = 1) or (@TourType = 2 and G.guCTour = 1) or (@TourType = 3 and G.guSaveProgram = 1))
	-- No Directas no Antes In & Out
	and (@ConsiderDirectsAntesInOut = 0 or not (G.guDirect = 1 and G.guAntesIO = 0))
	-- Con tour o venta
	and (@ConsiderTourSale = 0 or (G.guTour = 1 or G.guSale = 1))
group by G.guPRInvit3, G.gusr

return
end