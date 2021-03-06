if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptProductionByAgeMarketOriginallyAvailableInhouse]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptProductionByAgeMarketOriginallyAvailableInhouse]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por edad, mercado y originalmente disponible (Inhouse)
** 
** [wtorres]	23/Abr/2010 Ahora se pasa la lista de Lead Sources como un solo parametro y devuelve los datos agrupados
** [wtorres]	18/Oct/2010 Agregue el parametro @BasedOnArrival
** [lchairez]	06/Mar/2014 Se agrega un left join con la tabla Markets
*/
create procedure [dbo].[USP_OR_RptProductionByAgeMarketOriginallyAvailableInhouse]
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0,	-- Indica si se debe considerar quinielas
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada
as
set nocount on

select
	-- Descripcion del rango de edades
	A.daN as Age,
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
	select IsNull(Age, 0) as Age, Market, OriginallyAvailable, Arrivals, 0 as Contacts, 0 as Availables, 0 as Books, 0 as GrossBooks,
		0 as Directs, 0 as Shows, 0 as GrossShows, 0 as Sales, 0 as SalesAmount, 0 as SalesSelfGen, 0 as SalesAmountSelfGen
	from UFN_OR_GetAgeMarketOriginallyAvailableArrivals(@DateFrom, @DateTo, @LeadSources)
	-- Contactos
	union all
	select IsNull(Age, 0), Market, OriginallyAvailable, 0, Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetAgeMarketOriginallyAvailableContacts(@DateFrom, @DateTo, @LeadSources, @BasedOnArrival)
	-- Disponibles
	union all
	select IsNull(Age, 0), Market, OriginallyAvailable, 0, 0, Availables, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetAgeMarketOriginallyAvailableAvailables(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas)
	-- Bookings
	union all
	select IsNull(Age, 0), Market, OriginallyAvailable, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetAgeMarketOriginallyAvailableBookings(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, default, @BasedOnArrival)
	-- Bookings netos
	union all
	select IsNull(Age, 0), Market, OriginallyAvailable, 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetAgeMarketOriginallyAvailableBookings(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, 0, @BasedOnArrival)
	-- Directas
	union all
	select IsNull(Age, 0), Market, OriginallyAvailable, 0, 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetAgeMarketOriginallyAvailableBookings(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, 1, @BasedOnArrival)
	-- Shows
	union all
	select IsNull(Age, 0), Market, OriginallyAvailable, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0
	from UFN_OR_GetAgeMarketOriginallyAvailableShows(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, default, @BasedOnArrival)
	-- Shows netos
	union all
	select IsNull(Age, 0), Market, OriginallyAvailable, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0
	from UFN_OR_GetAgeMarketOriginallyAvailableShows(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, 1, @BasedOnArrival)
	-- Ventas
	union all
	select IsNull(Age, 0), Market, 'ORIGINALLY AVAILABLES', 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0
	from UFN_OR_GetAgeMarketSales(@DateFrom, @DateTo, @LeadSources, default, @BasedOnArrival)
	-- Monto de ventas
	union all
	select IsNull(Age, 0), Market, 'ORIGINALLY AVAILABLES', 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0
	from UFN_OR_GetAgeMarketSalesAmount(@DateFrom, @DateTo, @LeadSources, default, @BasedOnArrival)
	-- Ventas Self Gen
	union all
	select IsNull(Age, 0), Market, 'ORIGINALLY AVAILABLES', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetAgeMarketSales(@DateFrom, @DateTo, @LeadSources, 1, @BasedOnArrival)
	-- Monto de ventas Self Gen
	union all
	select IsNull(Age, 0), Market, 'ORIGINALLY AVAILABLES', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetAgeMarketSalesAmount(@DateFrom, @DateTo, @LeadSources, 1, @BasedOnArrival)
) as D
	left join DemAge A on D.Age between A.daFrom and A.daTo
	left join Markets M on D.Market = M.mkID
group by A.daN, A.daO, M.mkN, D.OriginallyAvailable
order by D.OriginallyAvailable, Market, SalesAmount_TOTAL desc, Shows desc, Books desc, Availables desc, Contacts desc, Arrivals desc,
	A.daO

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

