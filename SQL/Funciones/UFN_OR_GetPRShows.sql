USE [OrigosVCPalace];
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por PR
** 
** [wtorres]		18/Sep/2009 Created
** [wtorres]		23/Sep/2009 Modified. Convertido a funcion. Agregue los parametros @LeadSources, @ConsiderShowsQty y @ConsiderQuinellasSplit
** [wtorres]		27/Oct/2009 Agregue los parametros @ConsiderQuinellas y @FilterDeposit y elimine los parametros @ConsiderShowsQty
								y @ConsiderQuinellasSplit
** [wtorres]		02/Ene/2010 Modified. Agregue los parametros @ConsiderDirects y @ConsiderInOuts
** [wtorres]		16/Abr/2010 Modified. Agregue los parametros @PRs, @Program y @ConsiderDirectsAntesInOut
** [wtorres]		18/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [wtorres]		19/Oct/2011 Modified. Reemplace el parametro @ConsiderInOuts por @InOut y agregue el parametro @WalkOut
** [wtorres]		01/Nov/2011 Modified. Agregue los parametros @TourType y @ConsiderTourSale
** [gmaya]			11/Ago/2014 Modified. Agregue el parametro de formas de pago
** [lormartinez]	22/Sep/2014 Modified. Se agrega parametro para filtro basado en fecha de booking
** [wtorres]		15/Jul/2015 Modified. Correccion de error. No estaba contando solo los shows cuando se basaba en la fecha de booking
** [Lormartinez]  15/Dic/2015 Modified. Se reimplementa filtro por formas de pago
**
*/
CREATE function [dbo].[UFN_OR_GetPRShows](
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
	@BasedOnBooking bit = 0,				-- Indica si se debe basar en la fecha de booking,
  @PaymentTypes varchar(MAX) ='ALL' --Indica los modos de pago
)
returns @Table table (
	PR varchar(10),
	Shows money
)
as
begin

declare @tmpPaymentTypes table(item varchar(20))

--Si hay PaymentTypes se llena la lista
IF @PaymentTypes <> 'ALL'
BEGIN
 INSERT INTO @tmpPaymentTypes
 SELECT ITEM FROM dbo.Split(@PaymentTypes,',')
END

insert @Table

-- Shows (PR 1)
-- =============================================
select
	G.guPRInvit1,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3) * case when @ConsiderQuinellas = 0 then 1 else G.guShowsQty end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
  inner join BookingDeposits b on b.bdgu= G.guID
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
  -- Filtro de tipos de pago
  and (@PaymentTypes ='ALL' or (@PaymentTypes <> 'ALL' and b.bdpt IN (select item from @tmpPaymentTypes)) )
group by G.guPRInvit1

-- Shows (PR 2)
-- =============================================
union all
select
	G.guPRInvit2,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3) * case when @ConsiderQuinellas = 0 then 1 else G.guShowsQty end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
  inner join BookingDeposits b on b.bdgu= G.guID
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
  -- Filtro de tipos de pago
  and (@PaymentTypes ='ALL' or (@PaymentTypes <> 'ALL' and b.bdpt IN (select item from @tmpPaymentTypes)) )
group by G.guPRInvit2

-- Shows (PR 3)
-- =============================================
union all
select
	G.guPRInvit3,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3) * case when @ConsiderQuinellas = 0 then 1 else G.guShowsQty end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
  inner join BookingDeposits b on b.bdgu= G.guID
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
  -- Filtro de tipos de pago
  and (@PaymentTypes ='ALL' or (@PaymentTypes <> 'ALL' and b.bdpt IN (select item from @tmpPaymentTypes)) )
group by G.guPRInvit3

return
end
GO