if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptProductionByPRSalesRoomInhouse]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptProductionByPRSalesRoomInhouse]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por Salas de Ventas y PR (Inhouse)
** 
** [axperez]	03/Dic/2013 creado; copiado de USP_OR_RptProductionByPRInhouse
** [wtorres]	10/Dic/2013 Ahora la eficiencia se divide entre la columna UPS en lugar de Shows
** [wtorres]	26/May/2014 Ahora el porcentaje de cierre se divide entre la columna UPS en lugar de Shows
** [lormartinez] 22/Sep/2014 Modified. Se agrega parametro para filtro basado en bookingdate en funciones
**
*/
create procedure [dbo].[USP_OR_RptProductionByPRSalesRoomInhouse]
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0,	-- Indica si se debe considerar quinielas
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada
as
SET NOCOUNT ON

select
	-- Sala de Ventas
	IsNull(SR.srN, 'NO SALESROOM') as SalesRoom,
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
	select PR, SalesRoom, Assigns, 0 as Contacts, 0 as Availables, 0 as Directs, 0 as Books, 0 as Shows, 0 as ShowsNoDirects, 0 as UPS, 0 as InOuts,
		0 as WalkOuts, 0 as Tours, 0 as CourtesyTours, 0 as SaveTours, 0 as Sales, 0 as SalesAmount, 0 as SalesPending, 0 as SalesAmountPending,
		0 as SalesOOP, 0 as SalesAmountOOP
	from UFN_OR_GetPRSalesRoomAssigns(@DateFrom, @DateTo, @LeadSources)
	-- Contactos
	union all
	select PR, SalesRoom, 0, Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomContacts(@DateFrom, @DateTo, @LeadSources, default, @BasedOnArrival)
	-- Disponibles
	union all
	select PR, SalesRoom, 0, 0, Availables, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomAvailables(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, default, @BasedOnArrival)
	-- Directas
	union all
	select PR, SalesRoom, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomBookings(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, 1, default, @BasedOnArrival)
	-- Bookings
	union all
	select PR, SalesRoom, 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomBookings(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, default, @BasedOnArrival)
	-- Shows
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomShows(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, default, default, default,
		default, default, @BasedOnArrival, default)
	-- Shows sin directas no antes In & Out
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomShows(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, default, default, default,
		1, default, @BasedOnArrival, default)
	-- UPS (son todos los shows con tour o venta)
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomShows(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, default, default, default,
		default, 1, @BasedOnArrival, default)
	-- In & Outs
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomShows(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, 1, default, default, 
		default, default, @BasedOnArrival, default)
	-- Walk Outs
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomShows(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, default, 1, default, 
		default, default, @BasedOnArrival, default)
	-- Tours regulares
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomShows(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, default, default, 1, 
		default, default, @BasedOnArrival, default)
	-- Tours de cortesia
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomShows(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, default, default, 2, 
		default, default, @BasedOnArrival, default)
	-- Tours de rescate
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomShows(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, default, default, 3, 
		default, default, @BasedOnArrival, default)
	-- Ventas
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomSales(@DateFrom, @DateTo, @LeadSources, default, default, default, default, default, default, default, @BasedOnArrival, default)
	-- Monto de ventas
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomSalesAmount(@DateFrom, @DateTo, @LeadSources, default, default, default, default, default, default, 
		default, @BasedOnArrival, default)
	-- Ventas pendientes
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0
	from UFN_OR_GetPRSalesRoomSales(@DateFrom, @DateTo, @LeadSources, default, default, default, default, default, default, 1, @BasedOnArrival, default)
	-- Monto de ventas pendientes
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0
	from UFN_OR_GetPRSalesRoomSalesAmount(@DateFrom, @DateTo, @LeadSources, default, default, default, default, default, default, 
		1, @BasedOnArrival, default)
	-- Ventas Out Of Pending
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetPRSalesRoomSales(@DateFrom, @DateTo, @LeadSources, default, default, default, 1, default, default, 
		default, @BasedOnArrival, default)
	-- Monto de ventas Out Of Pending
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetPRSalesRoomSalesAmount(@DateFrom, @DateTo, @LeadSources, default, default, default, 1, default, default, 
		default, @BasedOnArrival, default)
) as D
	left join Personnel P on D.PR = P.peID
	left join TeamsGuestServices T on P.peTeam = T.tgID and P.pePlaceID = T.tglo and P.peTeamType = 'GS'
	left join Locations L on T.tglo = L.loID
	left join Personnel PL on T.tgLeader = PL.peID 
	left join SalesRooms SR on D.SalesRoom = SR.srID
group by SR.srN, D.PR, P.peN, P.peps, L.loN, T.tgN, PL.peN
order by SR.srN, Location, Team, Status, SalesAmount_PROC desc, Efficiency desc, Shows desc, Books desc, Contacts desc, Availables desc, Assigns desc, 
	D.PR

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

