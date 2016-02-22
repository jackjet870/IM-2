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
** Devuelve los datos para el reporte de produccion por PR (Inhouse)
** 
** [wtorres]		14/Oct/2008 Modified. Agregue los campos de locacion y equipos
** [wtorres]		24/Abr/2009 Modified. Ahora se pasa la lista de Lead Sources como un solo parametro
** [wtorres]		08/Sep/2009 Modified. Ahora ya devuelve los datos agrupados
** [wtorres]		30/Oct/2009 Modified. Agregue el parametro @ConsiderQuinellas
** [wtorres]		02/Ene/2010 Modified. No se estaban contando bien los shows netos
** [wtorres]		15/Abr/2010 Modified. Ahora el numero de shows netos son todos los shows menos las directas no In & Outs
** [wtorres]		18/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [wtorres]		21/Ene/2011 Modified. Ahora el numero total de shows no incluye a los In & Outs cuando no se consideran quinielas
** [wtorres]		11/Mar/2011 Modified. Agregue los campos de tours y walk outs
** [wtorres]		21/May/2011 Modified. Agregue los campos de no calificados, tours de cortesia y tours netos
** [wtorres]		30/May/2011 Modified. Agregue el campo de calificados y cambie la forma de contar los tours netos: ahora son los tours calificados
** [wtorres]		10/Ago/2011 Modified. Elimine los campos de calificados, no calificados y tours netos y agregue el campo de shows del programa de rescate
** [wtorres]		21/Sep/2011 Modified. Elimine los campos de depositos y selfgens y agregue los campos de shows sin directas, total de tours y total de shows.
**								Elimine las columnas de ventas self gen y PR y agregue las ventas pendientes
** [wtorres]		26/Sep/2011 Modified. Ahora el numero de bookings sin directas se calcula mediante una diferencia en vez de hacer un select
**								Ahora el numero de total de tours se calcula mediante una suma en vez de hacer un select
** [wtorres]		12/Oct/2011 Modified. Ahora al numero de total de tours se le suman los Walk Outs y agregue las columnas de ventas out of pending
** [wtorres]		19/Oct/2011 Modified. Elimine el uso de las funciones UFN_OR_GetPRInOuts y UFN_OR_GetPRWalkOuts
** [wtorres]		01/Nov/2011 Modified. Elimine el uso de las funciones UFN_OR_GetPRCourtesyTours, UFN_OR_GetPRSaveTours y UFN_OR_GetPRTours
** [wtorres]		03/Dic/2013 Modified. Ahora la eficiencia se divide entre la columna UPS en lugar de Shows
** [wtorres]		26/May/2014 Modified. Ahora el porcentaje de cierre se divide entre la columna UPS en lugar de Shows
** [lormartinez]	22/Sep/2014 Modified. Se agrega parametro para filtro basado en fecha de booking
** [lormartinez]  15/Dic/2015 Modified. Se agrega paranetro default en las funciones de 
                  GetBooking y GetShows
**
*/
CREATE procedure [dbo].[USP_OR_RptProductionByPRInhouse]
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0,	-- Indica si se debe considerar quinielas
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada
as
SET NOCOUNT ON

select
	-- Locacion
	IsNull(L.loN, 'NO LOCATION') as Location,
	-- Equipo
	IsNull(T.tgN, 'NO TEAM') as Team,
	-- Lider del equipo
	PL.peN as Leader,
	-- Clave del PR
	D.PR as PRID,
	-- Nombre del PR
	IsNull(P.peN, 'CODE NOT FOUND!') as PRN,
	-- Estatus del PR
	IsNull(P.peps, 'INACTIVE') as Status,
	-- Asignaciones
	Sum(D.Assigns) as Assigns,
	-- Contactos
	Sum(D.Contacts) as Contacts,
	-- Porcentaje de contactacion
	[dbo].UFN_OR_SecureDivision(Sum(D.Contacts), Sum(D.Assigns)) as ContactsFactor,
	-- Disponibles
	Sum(D.Availables) as Availables,
	-- Porcentaje de disponibles
	[dbo].UFN_OR_SecureDivision(Sum(D.Availables), Sum(D.Contacts)) as AvailablesFactor,
	-- Bookings sin directas
	Sum(D.Books) - Sum(D.Directs) as BooksNoDirects,
	-- Directas
	Sum(D.Directs) as Directs,
	-- Bookings
	Sum(D.Books) as Books,
	-- Porcentaje de bookings
	[dbo].UFN_OR_SecureDivision(Sum(D.Books), Sum(D.Availables)) as BooksFactor,
	-- Shows sin directas no antes In & Out
	Sum(D.ShowsNoDirects) as ShowsNoDirects,
	-- Porcentaje de shows
	[dbo].UFN_OR_SecureDivision(Sum(D.ShowsNoDirects), Sum(D.Books) - Sum(D.Directs)) as ShowsFactor,
	-- Shows
	Sum(D.Shows) as Shows,
	-- In & Outs
	Sum(D.InOuts) as InOuts,
	-- Walk Outs
	Sum(D.WalkOuts) as WalkOuts,
	-- Tours regulares
	Sum(D.Tours) as Tours,
	-- Tours de cortesia
	Sum(D.CourtesyTours) as CourtesyTours,
	-- Tours de rescate
	Sum(D.SaveTours) as SaveTours,
	-- Total de tours
	Sum(D.Tours) + Sum(D.CourtesyTours) + Sum(D.SaveTours) + Sum(D.WalkOuts) as TotalTours,
	-- UPS (Unidades producidas)
	Sum(D.UPS) as UPS,
	-- Ventas
	Sum(D.Sales) as Sales_PROC,
	-- Monto de ventas
	Sum(D.SalesAmount) as SalesAmount_PROC,
	-- Ventas pendientes
	Sum(D.SalesPending) as Sales_PEND,
	-- Monto de ventas pendientes
	Sum(D.SalesAmountPending) as SalesAmount_PEND,
	-- Ventas Out Of Pending
	Sum(D.SalesOOP) as Sales_OOP,
	-- Monto de ventas Out Of Pending
	Sum(D.SalesAmountOOP) as SalesAmount_OOP,
	-- Porcentaje de cierre
	[dbo].UFN_OR_SecureDivision(Sum(D.Sales), Sum(D.UPS)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.UPS)) as Efficiency,
	-- Venta promedio
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Sales)) as AverageSale
from (
	-- Asignaciones
	select PR, Assigns, 0 as Contacts, 0 as Availables, 0 as Directs, 0 as Books, 0 as Shows, 0 as ShowsNoDirects, 0 as UPS, 0 as InOuts,
		0 as WalkOuts, 0 as Tours, 0 as CourtesyTours, 0 as SaveTours, 0 as Sales, 0 as SalesAmount, 0 as SalesPending, 0 as SalesAmountPending,
		0 as SalesOOP, 0 as SalesAmountOOP
	from UFN_OR_GetPRAssigns(@DateFrom, @DateTo, @LeadSources)
	-- Contactos
	union all
	select PR, 0, Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRContacts(@DateFrom, @DateTo, @LeadSources, default, @BasedOnArrival)
	-- Disponibles
	union all
	select PR, 0, 0, Availables, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRAvailables(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, default, @BasedOnArrival)
	-- Directas
	union all
	select PR, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, 1, default, @BasedOnArrival,default)
	-- Bookings
	union all
	select PR, 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, default, @BasedOnArrival,default)
	-- Shows
	union all
	select PR, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, default, default, default,
		default, default, @BasedOnArrival, default,default)
	-- Shows sin directas no antes In & Out
	union all
	select PR, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, default, default, default,
		1, default, @BasedOnArrival, default,default)
	-- UPS (son todos los shows con tour o venta)
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, default, default, default,
		default, 1, @BasedOnArrival, default,default)
	-- In & Outs
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, 1, default, default, default,
		default, @BasedOnArrival, default,default)
	-- Walk Outs
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, default, 1, default, default,
		default, @BasedOnArrival, default,default)
	-- Tours regulares
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, default, default, 1, default,
		default, @BasedOnArrival, default,default)
	-- Tours de cortesia
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, default, default, 2, default,
		default, @BasedOnArrival, default,default)
	-- Tours de rescate
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, default, default, 3, default,
		default, @BasedOnArrival, default,default)
	-- Ventas
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSales(@DateFrom, @DateTo, @LeadSources, default, default, default, default, default, default, default, @BasedOnArrival, default)
	-- Monto de ventas
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0
	from UFN_OR_GetPRSalesAmount(@DateFrom, @DateTo, @LeadSources, default, default, default, default, default, default, default, @BasedOnArrival, default)
	-- Ventas pendientes
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0
	from UFN_OR_GetPRSales(@DateFrom, @DateTo, @LeadSources, default, default, default, default, default, default, 1, @BasedOnArrival, default)
	-- Monto de ventas pendientes
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0
	from UFN_OR_GetPRSalesAmount(@DateFrom, @DateTo, @LeadSources, default, default, default, default, default, default, 1, @BasedOnArrival, default)
	-- Ventas Out Of Pending
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetPRSales(@DateFrom, @DateTo, @LeadSources, default, default, default, 1, default, default, default, @BasedOnArrival, default)
	-- Monto de ventas Out Of Pending
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetPRSalesAmount(@DateFrom, @DateTo, @LeadSources, default, default, default, 1, default, default, default, @BasedOnArrival, default)
) as D
	left join Personnel P on D.PR = P.peID
	left join TeamsGuestServices T on P.peTeam = T.tgID and P.pePlaceID = T.tglo and P.peTeamType = 'GS'
	left join Locations L on T.tglo = L.loID
	left join Personnel PL on T.tgLeader = PL.peID
group by D.PR, P.peN, P.peps, L.loN, T.tgN, PL.peN
order by Location, Team, Status, SalesAmount_PROC desc, Efficiency desc, Shows desc, Books desc, Contacts desc, Availables desc, Assigns desc, D.PR
GO