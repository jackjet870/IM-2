if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptProductionByNationalityMarketOriginallyAvailableInhouse]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptProductionByNationalityMarketOriginallyAvailableInhouse]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por nacionalidad, mercado y originalmente disponible (Inhouse)
** 
** [wtorres]	11/Oct/2010 Creado
** [wtorres]	18/Nov/2010 Agregue el parametro @BasedOnArrival
** [wtorres]	19/Jul/2013 Agregue el parametro @IncludeSaveCourtesyTours
** [wtorres]	25/Jul/2013 Renombre el parametro @IncludeSaveCourtesyTours por @FilterSaveCourtesyTours
** [lchairez]	06/Mar/2014 Se agrega un left join con la tabla Markets
*/
create procedure [dbo].[USP_OR_RptProductionByNationalityMarketOriginallyAvailableInhouse]
	@DateFrom datetime,						-- Fecha desde
	@DateTo datetime,						-- Fecha hasta
	@LeadSources varchar(8000),				-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',				-- Claves de PRs
	@Program varchar(10) = 'ALL',			-- Clave de programa
	@ConsiderQuinellas bit = 0,				-- Indica si se debe considerar quinielas
	@FilterSaveCourtesyTours tinyint = 0,	-- Filtro de tours de rescate y cortesia
											--		0. Sin filtro
											--		1. Excluir tours de rescate y cortesia
											--		2. Excluir tours de rescate y cortesia sin venta
	@BasedOnArrival bit = 0					-- Indica si se debe basar en la fecha de llegada
as
set nocount on

select
	-- Nacionalidad
	IsNull(D.Nationality, 'Not specified') as Nationality,
	-- Mercado
	IsNull(Cast(M.mkN as varchar(13)), 'Not Specified') as Market,
	-- Originalmente disponible
	D.OriginallyAvailable,
	-- Llegadas
	Sum(D.Arrivals) as Arrivals,
	-- Contactos
	Sum(D.Contacts) as Contacts,
	-- Porcentaje de contactacion
	[dbo].UFN_OR_SecureDivision(Sum(D.Contacts), Sum(D.Arrivals)) as ContactsFactor,
	-- Disponibles
	Sum(D.Availables) as Availables,
	-- Porcentaje de disponibles
	[dbo].UFN_OR_SecureDivision(Sum(D.Availables), Sum(D.Contacts)) as AvailablesFactor,
	-- Bookings netos
	Sum(D.GrossBooks) as GrossBooks,
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
	[dbo].UFN_OR_SecureDivision(Sum(D.GrossShows), Sum(D.GrossBooks)) as ShowsFactor,
	-- Ventas
	Sum(D.Sales) as Sales_TOTAL,
	-- Monto de ventas
	Sum(D.SalesAmount) as SalesAmount_TOTAL,
	-- Ventas PR
	Sum(D.Sales - D.SalesSelfGen) as Sales_PR,
	-- Monto de ventas PR
	Sum(D.SalesAmount - D.SalesAmountSelfGen) as SalesAmount_PR,
	-- Ventas Self Gen
	Sum(D.SalesSelfGen) as Sales_SELFGEN,
	-- Monto de ventas SelfGen
	Sum(D.SalesAmountSelfGen) as SalesAmount_SELFGEN,
	-- Porcentaje de cierre
	[dbo].UFN_OR_SecureDivision(Sum(D.Sales), Sum(D.Shows)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Shows)) as Efficiency,
	-- Venta promedio
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Sales)) as AverageSale
from (
	-- Llegadas
	select Nationality, Market, OriginallyAvailable, Arrivals, 0 as Contacts, 0 as Availables, 0 as Books, 0 as GrossBooks, 0 as Directs, 0 as Shows,
		0 as GrossShows, 0 as Sales, 0 as SalesAmount, 0 as SalesSelfGen, 0 as SalesAmountSelfGen
	from UFN_OR_GetNationalityMarketOriginallyAvailableArrivals(@DateFrom, @DateTo, @LeadSources, @PRs, @Program)
	-- Contactos
	union all
	select Nationality, Market, OriginallyAvailable, 0, Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetNationalityMarketOriginallyAvailableContacts(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @BasedOnArrival)
	-- Disponibles
	union all
	select Nationality, Market, OriginallyAvailable, 0, 0, Availables, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetNationalityMarketOriginallyAvailableAvailables(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @ConsiderQuinellas)
	-- Bookings
	union all
	select Nationality, Market, OriginallyAvailable, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetNationalityMarketOriginallyAvailableBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @ConsiderQuinellas, default,
		@BasedOnArrival)
	-- Bookings netos
	union all
	select Nationality, Market, OriginallyAvailable, 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetNationalityMarketOriginallyAvailableBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @ConsiderQuinellas, 0,
		@BasedOnArrival)
	-- Directas
	union all
	select Nationality, Market, OriginallyAvailable, 0, 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetNationalityMarketOriginallyAvailableBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @ConsiderQuinellas, 1,
		@BasedOnArrival)
	-- Shows
	union all
	select Nationality, Market, OriginallyAvailable, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0
	from UFN_OR_GetNationalityMarketOriginallyAvailableShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @ConsiderQuinellas, default,
		@FilterSaveCourtesyTours, @BasedOnArrival)
	-- Shows netos
	union all
	select Nationality, Market, OriginallyAvailable, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0
	from UFN_OR_GetNationalityMarketOriginallyAvailableShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @ConsiderQuinellas, 1,
		@FilterSaveCourtesyTours, @BasedOnArrival)
	-- Ventas
	union all
	select Nationality, Market, 'ORIGINALLY AVAILABLES', 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0
	from UFN_OR_GetNationalityMarketSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterSaveCourtesyTours, @BasedOnArrival)
	-- Monto de ventas
	union all
	select Nationality, Market, 'ORIGINALLY AVAILABLES', 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0
	from UFN_OR_GetNationalityMarketSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterSaveCourtesyTours, @BasedOnArrival)
	-- Ventas Self Gen
	union all
	select Nationality, Market, 'ORIGINALLY AVAILABLES', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetNationalityMarketSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, 1, @FilterSaveCourtesyTours, @BasedOnArrival)
	-- Monto de ventas Self Gen
	union all
	select Nationality, Market, 'ORIGINALLY AVAILABLES', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetNationalityMarketSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, 1, @FilterSaveCourtesyTours, @BasedOnArrival)
) as D
	left join Markets M on D.Market = M.mkID
group by D.Nationality, M.mkN, D.OriginallyAvailable
order by D.OriginallyAvailable, Market, SalesAmount_TOTAL desc, Shows desc, Books desc, Availables desc, Contacts desc, Arrivals desc,
	D.Nationality

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

