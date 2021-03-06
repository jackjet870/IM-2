if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptProductionByLeadSourceMarketMonthly]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptProductionByLeadSourceMarketMonthly]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de producción por Lead Source, mercado y mes
** 
** [wtorres]	03/Feb/2010 Created
** [wtorres]	14/May/2010 Modified. Agregue las columnas de bookings netas y shows netos
** [wtorres]	24/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [caduran]    16/Dic/2014 Modified. Se agregaron las columnas Directs, InOuts, WalkOuts, Tours, CourtesyTours,
**							SaveTours, TotalTours, UPS y AverageSales
**							Se agregaron los parametros @LeadSources y @Program
** [wtorres]	04/Jun/2015 Modified. Agregue el parametro @External
**
*/
create procedure [dbo].[USP_OR_RptProductionByLeadSourceMarketMonthly]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@External int = 0,					-- Filtro de invitaciones externas
										--		0. Sin filtro
										--		1. Excluir invitaciones externas
										--		2. Solo invitaciones externas
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
as
set nocount on

-- Datos
-- =============================================
select
	-- Año
	D.[Year],
	-- Mes
	D.[Month],
	-- Nombre del mes
	DateName(Month, dbo.DateSerial(D.[Year], D.[Month], 1)) as MonthN,
	-- Mercado
	D.Market,
	-- Lead Source
	D.LeadSource,
	-- Programa
	D.Program,
	-- Llegadas
	Sum(D.Arrivals) as Arrivals,
	-- Contactos
	Sum(D.Contacts) as Contacts,
	-- Porcentaje de contactación
	[dbo].UFN_OR_SecureDivision(Sum(D.Contacts), Sum(D.Arrivals)) as ContactsFactor,
	-- Disponibles
	Sum(D.Availables) as Availables,
	-- Porcentaje de disponibles
	[dbo].UFN_OR_SecureDivision(Sum(D.Availables), Sum(D.Contacts)) as AvailablesFactor,
	-- Bookings sin directas
	Sum(D.Books) - Sum(D.Directs) as GrossBooks,
	-- Directas
	Sum(D.Directs) as Directs,
	-- Bookings
	Sum(D.Books) as Books,
	-- Porcentaje de bookings
	[dbo].UFN_OR_SecureDivision(Sum(D.Books), Sum(D.Availables)) as BooksFactor,
	-- Shows netos
	Sum(D.GrossShows) as GrossShows,
	-- Shows
	Sum(D.Shows) as Shows,
	-- Porcentaje de shows
	[dbo].UFN_OR_SecureDivision(Sum(D.GrossShows), Sum(D.Books) - Sum(D.Directs)) as ShowsFactor,
	-- Porcentaje de shows / llegadas
	[dbo].UFN_OR_SecureDivision(Sum(D.Shows), Sum(D.Arrivals)) as ShowsArrivalsFactor,
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
	Sum(D.Sales) as Sales,
	-- Monto de ventas
	Sum(D.SalesAmount) as SalesAmount,
	-- Porcentaje de cierre
	[dbo].UFN_OR_SecureDivision(Sum(D.Sales), Sum(D.Shows)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Shows)) as Efficiency,
	-- Venta promedio
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Sales)) as AverageSale
into #Data
from (
	-- Llegadas
	select LeadSource, Market, [Year], [Month], Program, Arrivals, 0 as Contacts, 0 as Availables, 0 as Directs, 0 as Books, 0 as Shows,
		0 as GrossShows, 0 as InOuts, 0 as WalkOuts, 0 as Tours, 0 as CourtesyTours, 0 as SaveTours, 0 as UPS, 0 as Sales, 0 as SalesAmount
	from UFN_OR_GetLeadSourceMarketMonthArrivals(@DateFrom, @DateTo, @LeadSources, @Program, @External)
	-- Contactos
	union all
	select LeadSource, Market, [Year], [Month], Program, 0, Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetLeadSourceMarketMonthContacts(@DateFrom, @DateTo, @LeadSources, @Program, @External, @BasedOnArrival)
	-- Disponibles
	union all
	select LeadSource, Market, [Year], [Month], Program, 0, 0, Availables, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetLeadSourceMarketMonthAvailables(@DateFrom, @DateTo, @LeadSources, @Program, @ConsiderQuinellas, @External)
	-- Directas
	union all
	select LeadSource, Market, [Year], [Month], Program, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetLeadSourceMarketMonthBookings(@DateFrom, @DateTo, @LeadSources, @Program, @ConsiderQuinellas, 1, @External, @BasedOnArrival)
	-- Bookings
	union all
	select LeadSource, Market, [Year], [Month], Program, 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetLeadSourceMarketMonthBookings(@DateFrom, @DateTo, @LeadSources, @Program, @ConsiderQuinellas, default, @External, @BasedOnArrival)
	-- Shows
	union all
	select LeadSource, Market, [Year], [Month], Program, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetLeadSourceMarketMonthShows(@DateFrom, @DateTo, @LeadSources, @Program, @ConsiderQuinellas, default, default, default, default, default, @External, @BasedOnArrival)
	-- Shows netos
	union all
	select LeadSource, Market, [Year], [Month], Program, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetLeadSourceMarketMonthShows(@DateFrom, @DateTo, @LeadSources, @Program, @ConsiderQuinellas, default, default, default, 1, default, @External, @BasedOnArrival)
	-- In & Outs
	union all
	select LeadSource, Market, [Year], [Month], Program, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetLeadSourceMarketMonthShows(@DateFrom, @DateTo, @LeadSources, @Program, @ConsiderQuinellas, 1, default, default, default, default, @External, @BasedOnArrival)
	-- Walk Outs
	union all
	select LeadSource, Market, [Year], [Month], Program, 0, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetLeadSourceMarketMonthShows(@DateFrom, @DateTo, @LeadSources, @Program, @ConsiderQuinellas, default, 1, default, default, default, @External, @BasedOnArrival)
	-- Tours regulares
	union all
	select LeadSource, Market, [Year], [Month], Program, 0, 0, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0
	from UFN_OR_GetLeadSourceMarketMonthShows(@DateFrom, @DateTo, @LeadSources, @Program, @ConsiderQuinellas, default, default, 1, default, default, @External, @BasedOnArrival)
	-- Tours de cortesia
	union all
	select LeadSource, Market, [Year], [Month], Program, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0
	from UFN_OR_GetLeadSourceMarketMonthShows(@DateFrom, @DateTo, @LeadSources, @Program, @ConsiderQuinellas, default, default, 2, default, default, @External, @BasedOnArrival)
	-- Tours de rescate
	union all
	select LeadSource, Market, [Year], [Month], Program, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0
	from UFN_OR_GetLeadSourceMarketMonthShows(@DateFrom, @DateTo, @LeadSources, @Program, @ConsiderQuinellas, default, default, 3, default, default, @External, @BasedOnArrival)
	-- UPS (son todos los shows con tour o venta)
	union all
	select LeadSource, Market, [Year], [Month], Program, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0
	from UFN_OR_GetLeadSourceMarketMonthShows(@DateFrom, @DateTo, @LeadSources, @Program, @ConsiderQuinellas, default, default, default, default, 1, @External, @BasedOnArrival)
	-- Numero de ventas
	union all
	select LeadSource, Market, [Year], [Month], Program, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetLeadSourceMarketMonthSales(@DateFrom, @DateTo, @LeadSources, @Program, @External, @BasedOnArrival)
	-- Monto de ventas
	union all
	select LeadSource, Market, [Year], [Month], Program, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetLeadSourceMarketMonthSalesAmount(@DateFrom, @DateTo, @LeadSources, @Program, @External, @BasedOnArrival)
) as D
group by D.[Year], D.[Month], D.LeadSource, D.Market, D.Program

-- Datos con totales
-- =============================================
-- Datos
select
	-- Año
	D.[Year],
	-- Mes
	D.[Month],
	-- Nombre del mes
	DateName(Month, dbo.DateSerial(D.[Year], D.[Month], 1)) as MonthN,
	-- MarketTotal de mercados
	Cast(0 as bit) as MarketTotal,
	-- Mercado
	D.Market,
	-- Lead Source
	L.lsN as LeadSource,
	-- Programa
	P.pgN as Program,
	-- Llegadas
	D.Arrivals,
	-- Contactos
	D.Contacts,
	-- Porcentaje de contactación
	D.ContactsFactor,
	-- Disponibles
	D.Availables,
	-- Porcentaje de disponibles
	D.AvailablesFactor,
	-- Bookings netos
	D.GrossBooks,
	-- Bookings
	D.Books,
	-- Porcentaje de bookings
	D.BooksFactor,
	-- Shows netos
	D.GrossShows,
	-- Shows
	D.Shows,
	-- Porcentaje de shows
	D.ShowsFactor,
	-- Porcentaje de shows / llegadas
	D.ShowsArrivalsFactor,
	-- Ventas
	D.Sales,
	-- Monto de ventas
	D.SalesAmount,
	-- Porcentaje de cierre
	D.ClosingFactor,
	-- Eficiencia
	D.Efficiency,
	-- Directs
	D.Directs,
	-- InOuts
	D.InOuts,
	-- WalkOuts
	D.WalkOuts,
	-- Tours
	D.Tours,
	-- CourtesyTours
	D.CourtesyTours,
	-- SaveTours
	D.SaveTours,
	-- TotalTours
	D.TotalTours,
	-- UPS
	D.UPS,
	-- Venta promedio
	D.AverageSale
from #Data D
	left join LeadSources L on D.LeadSource = L.lsID
	left join Programs P on D.Program = P.pgID
union all

-- Totales
select
	-- Año
	D.[Year],
	-- Mes
	D.[Month],
	-- Nombre del mes
	DateName(Month, dbo.DateSerial(D.[Year], D.[Month], 1)) as MonthN,
	-- MarketTotal de mercados
	Cast(1 as bit) as MarketTotal,
	-- Mercado
	'TOTAL' as Market,
	-- Lead Source
	L.lsN as LeadSource,
	-- Programa
	P.pgN as Program,
	-- Llegadas
	Sum(D.Arrivals) as Arrivals,
	-- Contactos
	Sum(D.Contacts) as Contacts,
	-- Porcentaje de contactación
	[dbo].UFN_OR_SecureDivision(Sum(D.Contacts), Sum(D.Arrivals)) as ContactsFactor,
	-- Disponibles
	Sum(D.Availables) as Availables,
	-- Porcentaje de disponibles
	[dbo].UFN_OR_SecureDivision(Sum(D.Availables), Sum(D.Contacts)) as AvailablesFactor,
	-- Bookings netos
	Sum(D.GrossBooks) as GrossBooks,
	-- Bookings
	Sum(D.Books) as Books,
	-- Porcentaje de bookings
	[dbo].UFN_OR_SecureDivision(Sum(D.Books), Sum(D.Availables)) as BooksFactor,
	-- Shows netos
	Sum(D.GrossShows) as GrossShows,
	-- Shows
	Sum(D.Shows) as Shows,
	-- Porcentaje de shows
	[dbo].UFN_OR_SecureDivision(Sum(D.GrossShows), Sum(D.GrossBooks)) as ShowsFactor,
	-- Porcentaje de shows / llegadas
	[dbo].UFN_OR_SecureDivision(Sum(D.Shows), Sum(D.Arrivals)) as ShowsArrivalsFactor,
	-- Ventas
	Sum(D.Sales) as Sales,
	-- Monto de ventas
	Sum(D.SalesAmount) as SalesAmount,
	-- Porcentaje de cierre
	[dbo].UFN_OR_SecureDivision(Sum(D.Sales), Sum(D.Shows)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Shows)) as Efficiency,
	-- Directs
	Sum(D.Directs) as Directs,
	-- InOuts
	Sum(D.InOuts) as InOuts,
	-- WalkOuts
	Sum(D.WalkOuts) as WalkOuts,
	-- Tours
	Sum(D.Tours) as Tours,
	-- CourtesyTours
	Sum(D.CourtesyTours) as CourtesyTours,
	-- SaveTours
	Sum(D.SaveTours) as SaveTours,
	-- TotalTours
	Sum(D.TotalTours) as TotalTours,
	-- UPS
	Sum(D.UPS) as UPS,
	-- Venta promedio
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Sales)) as AverageSale
from #Data D
	left join LeadSources L on D.LeadSource = L.lsID
	left join Programs P on D.Program = P.pgID
group by D.[Year], D.[Month], L.lsN, P.pgN
order by P.pgN, D.[Year], D.[Month], MarketTotal desc, D.Market,  L.lsN

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


