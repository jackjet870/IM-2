if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptDepositsPaymentByPR]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptDepositsPaymentByPR]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de pago de depositos a los PRs (Comisiones de PRs de Outside)
**
** [alesanchez]		19/Nov/2013 Created
** [gmaya]			11/Ago/2014 Modified. Agregue el parametro de formas de pago
** [lormartinez]	22/Sep/2014 Modified. Se agrega parametro para filtro basado en fecha de booking
**
*/
create procedure [dbo].[USP_OR_RptDepositsPaymentByPR]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000),			-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@PaymentTypes varchar(10) = 'ALL',	-- Claves de formas de pago
	@FilterDeposit tinyint = 0			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
										--		3. Con deposito y shows sin deposito (Deposits & Flyers Show)
as
set nocount on

declare	@FilterDepositAlternate tinyint	-- Filtro de depositos alterno (Aplica para bookings)

set @FilterDepositAlternate = @FilterDeposit
-- si se desea el reporte con deposito y shows sin deposito (Deposits & Flyers Show)
if @FilterDeposit = 3
	set @FilterDeposit = 0	-- Sin filtro

select
	'TO RETAIN' AS Category,
	-- Clave del PR
	T.PR,
	-- Nombre del PR
	IsNull(P.peN, 'CODE NOT FOUND!') as PRN,
	-- Bookings
	Sum(T.Books) as Books,
	-- In & Outs
	Sum(T.InOuts) as InOuts,
	-- Bookings netos
	Sum(T.GrossBooks) as GrossBooks,
	-- Shows netos dentro de booking
	Sum (T.GrossShowsInBooks) as GrossShowsInBooks, 
	-- No Shows netos dentro de booking
	Sum(T.GrossNoShowsInBooks) as GrossNoShowsInBooks,
	-- Porcentaje de show factor
	[dbo].UFN_OR_SecureDivision(Sum(T.GrossShowsInBooks), Sum(T.GrossBooks)) as ShowsFactor, 
	-- Shows netos fuera de booking
	Sum (T.GrossShowsOutBooks) as GrossShowsOutBooks, 
	-- Shows netos
	Sum(T.GrossShows) as GrossShows	
into #PRs 
from (
	-- Bookings
	select PR, Books, 0 as GrossBooks, 0 as InOuts, 0 as GrossShows, 0 as GrossShowsInBooks, 0 as GrossShowsOutBooks,
		0 as GrossNoShowsInBooks
	from UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, 0, default, default/*,@PaymentTypes*/)
	-- Bookings netos
	union all
	select PR, 0, Books, 0, 0, 0, 0, 0
	from UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, 0, 0, default/*, @PaymentTypes*/)
	-- In & Outs
	union all
	select PR, 0, 0, Shows, 0, 0, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, 0, 1, default, default, default, default,
		default, default/*, @PaymentTypes*/)
	-- Shows netos
	union all
	select PR, 0, 0, 0, Shows, 0, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, 0, 0, default, default, default, default,
		default, default/*, @PaymentTypes*/)
	-- GrossShowsInBooks
	union all
	select PR, 0, 0, 0, 0, Shows, 0, 0
	from UFN_OR_GetPRShowsInBooks(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, 0, 0, default, default, default, default,
		default, @PaymentTypes)
	-- GrossShowsOutBooks
	union all
	select PR, 0, 0, 0, 0, 0, Shows, 0
	from UFN_OR_GetPRShowsOutBooks(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, 0, 0, default, default, default, default,
		default, @PaymentTypes)
	-- GrossNoShowsInBooks
	union all
	select PR, 0, 0, 0, 0, 0, 0, Shows
	from UFN_OR_GetPRNoShowsInBooks(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, 0, 0, default, default, default, default,
		default, @PaymentTypes)
) AS T 
	left join Personnel P on T.PR = P.peID
group by T.PR, P.peN
order by ShowsFactor desc, GrossShows desc, Books desc

update #PRs set Category = 'TO PAY' where ShowsFactor >= .70   

-- PRs
select * from #PRs

-- Depositos
select P.PR, G.guID, G.guLastName1, G.guFirstName1, G.guAge1, G.guBookD, G.guShowD, G.guOutInvitNum, G.guls, 
	G.gusr, D.bdpt, D.bdcu, 
	case when (D.bdpc = C.pcID) then (C.pcN)		
	end as bdpc,
	case when (G.guShowD between @DateFrom and @DateTo and G.guBookD < @DateFrom and G.guShow = 1) then 'Show Out Book'
		 when (G.guBookD between @DateFrom and @DateTo and G.guShow = 0 ) then 'No Show'
	end as tpshow,
	case when (G.guShowD between @DateFrom and @DateTo and G.guBookD < @DateFrom and G.guShow = 1) then D.bdAmount * -1
	     when (G.guBookD between @DateFrom and @DateTo and G.guShow = 0) then D.bdAmount
	end as bdAmount, 
	case when (G.guShowD between @DateFrom and @DateTo and G.guBookD < @DateFrom and G.guShow = 1 and not D.bdpt ='CC' ) then D.bdReceived * -1
	     when (G.guShowD between @DateFrom and @DateTo and G.guBookD < @DateFrom and G.guShow = 1 and D.bdpt ='CC' and P.Category = 'TO PAY') then [dbo].[UFN_OR_GetTotalReceivedRound](D.bdReceived, 3.33) * -1
	     when (G.guShowD between @DateFrom and @DateTo and G.guBookD < @DateFrom and G.guShow = 1 and D.bdpt ='CC' and P.Category = 'TO RETAIN') then [dbo].[UFN_OR_GetTotalReceivedRound](D.bdReceived, 3.33) * -1
	     when (G.guBookD between @DateFrom and @DateTo and G.guShow = 0 and not D.bdpt ='CC') then D.bdReceived
		 when (G.guBookD between @DateFrom and @DateTo and G.guShow = 0 and D.bdpt ='CC' and P.Category = 'TO PAY') then [dbo].[UFN_OR_GetTotalReceivedRound](D.bdReceived, 3.33)
		 when (G.guBookD between @DateFrom and @DateTo and G.guShow = 0 and D.bdpt ='CC' and P.Category = 'TO RETAIN') then D.bdReceived
	end as bdReceived,
	case when (G.guShowD between @DateFrom and @DateTo and G.guBookD < @DateFrom and G.guShow = 1) then (abs(D.bdAmount) - abs(D.bdReceived)) * -1
		 when (G.guBookD between @DateFrom and @DateTo and G.guShow = 0 and P.Category = 'TO PAY') then (D.bdAmount - D.bdReceived)
		 when (G.guBookD between @DateFrom and @DateTo and G.guShow = 0 and P.Category = 'TO RETAIN') then (D.bdReceived - D.bdAmount) 	
	end as topay
	
into #Deposits
from #PRs P
	inner join Guests G on G.guPRInvit1 = P.PR
	inner join BookingDeposits D on D.bdgu = G.guID 
	left join PaymentPlaces C on D.bdpc = C.pcID
where
    -- Todos los Book dentro del rango de fecha
    (G.guBookD between @DateFrom and @DateTo and G.guShow = 0  and (@LeadSources = 'ALL' or G.guls  in (select item from split(@LeadSources, ','))) and (@PaymentTypes = 'ALL' or D.bdpt  in (select item from split(@PaymentTypes, ',')))) 
    or
    -- Todos los Show Out Book
	(G.guShowD between @DateFrom and @DateTo and G.guBookD < @DateFrom and G.guShow = 1 and (@LeadSources = 'ALL' or G.guls  in (select item from split(@LeadSources, ','))) and (@PaymentTypes = 'ALL' or D.bdpt  in (select item from split(@PaymentTypes, ',')))) 
   	
order by P.PR

select * from #Deposits

--  Monedas
-- =============================================
select distinct C.cuID, C.cuN
from #Deposits D
	left join Currencies C on D.bdcu = C.cuID

--  Formas de Pago
-- =============================================
select distinct T.ptID, T.ptN 
from #Deposits D
	left join PaymentTypes T on D.bdpt = T.ptID
	
--  Lugares de Pago
-- =============================================
select distinct P.pcID, P.pcN 
from #Deposits D
	left join PaymentPlaces P on D.bdpc = P.pcID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

