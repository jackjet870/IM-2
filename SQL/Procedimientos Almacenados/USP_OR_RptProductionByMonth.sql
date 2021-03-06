if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptProductionByMonth]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptProductionByMonth]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por mes
** 
** [wtorres]	09/Oct/2009 Created
** [wtorres]	10/May/2010 Modified. Agregue los parametros @ConsiderQuinellas y @ConsiderOriginallyAvailable
**							Ahora acepta varios Lead Sources
**							Agregue las columnas de bookings netos y shows netos
** [wtorres]	23/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [wtorres]	04/Jun/2015 Modified. Agregue el parametro @External
**
*/
create procedure [dbo].[USP_OR_RptProductionByMonth]
	@DateFrom datetime,						-- Fecha desde
	@DateTo datetime,						-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',		-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0,				-- Indica si se debe considerar quinielas
	@Market varchar(10) = 'ALL',			-- Clave del mercado
	@ConsiderNights bit = 0,				-- Indica si se debe considerar el numero de noches
	@NightsFrom int = 0,					-- Numero de noches desde
	@NightsTo int = 0,						-- Numero de noches hasta
	@Agency varchar(35) = 'ALL',			-- Clave de agencia
	@ConsiderOriginallyAvailable bit = 0,	-- Indica si se debe considerar originalmente disponible
	@External int = 0,						-- Filtro de invitaciones externas
											--		0. Sin filtro
											--		1. Excluir invitaciones externas
											--		2. Solo invitaciones externas
	@BasedOnArrival bit = 0					-- Indica si se debe basar en la fecha de llegada
as
set nocount on

select
	-- Año
	[Year],
	-- Mes
	[Month],
	-- Llegadas
	Sum(Arrivals) as Arrivals,
	-- Contactos
	Sum(Contacts) as Contacts,
	-- Porcentaje de contactacion
	[dbo].UFN_OR_SecureDivision(Sum(Contacts), Sum(Arrivals)) as ContactsFactor,
	-- Disponibles
	Sum(Availables) as Availables,
	-- Porcentaje de disponibles
	[dbo].UFN_OR_SecureDivision(Sum(Availables), Sum(Contacts)) as AvailablesFactor,
	-- Bookings netos
	Sum(GrossBooks) as GrossBooks,
	-- Bookings
	Sum(Books) as Books,
	-- Porcentaje de bookings
	[dbo].UFN_OR_SecureDivision(Sum(Books), Sum(Availables)) as BooksFactor,
	-- Shows netos
	Sum(GrossShows) as GrossShows,
	-- Shows
	Sum(Shows) as Shows,
	-- Porcentaje de shows
	[dbo].UFN_OR_SecureDivision(Sum(GrossShows), Sum(GrossBooks)) as ShowsFactor,
	-- Porcentaje de shows / llegadas
	[dbo].UFN_OR_SecureDivision(Sum(Shows), Sum(Arrivals)) as ShowsArrivalsFactor,
	-- Ventas
	Sum(Sales) as Sales,
	-- Monto de ventas
	Sum(SalesAmount) as SalesAmount,
	-- Porcentaje de cierre
	[dbo].UFN_OR_SecureDivision(Sum(Sales), Sum(Shows)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(SalesAmount), Sum(Shows)) as Efficiency,
	-- Venta promedio
	[dbo].UFN_OR_SecureDivision(Sum(SalesAmount), Sum(Sales)) as AverageSale
from (
	-- Llegadas
	select [Year], [Month], Arrivals, 0 as Contacts, 0 as Availables, 0 as Books, 0 as GrossBooks, 0 as Shows, 0 as GrossShows, 0 as Sales,
		0 as SalesAmount
	from UFN_OR_GetMonthArrivals(@DateFrom, @DateTo, @LeadSources, @Market, @ConsiderNights, @NightsFrom, @NightsTo, @Agency,
		@ConsiderOriginallyAvailable, default, @External)
	-- Contactos
	union all
	select [Year], [Month], 0, Contacts, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetMonthContacts(@DateFrom, @DateTo, @LeadSources, @Market, @ConsiderNights, @NightsFrom, @NightsTo, @Agency,
		@ConsiderOriginallyAvailable, @External, @BasedOnArrival)
	-- Disponibles
	union all
	select [Year], [Month], 0, 0, Availables, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetMonthAvailables(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, @Market, @ConsiderNights, @NightsFrom, @NightsTo,
		@Agency, @ConsiderOriginallyAvailable, @External)
	-- Bookings
	union all
	select [Year], [Month], 0, 0, 0, Books, 0, 0, 0, 0, 0
	from UFN_OR_GetMonthBookings(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, @Market, @ConsiderNights, @NightsFrom, @NightsTo,
		@Agency, @ConsiderOriginallyAvailable, default, @External, @BasedOnArrival)
	-- Bookings netos
	union all
	select [Year], [Month], 0, 0, 0, 0, Books, 0, 0, 0, 0
	from UFN_OR_GetMonthBookings(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, @Market, @ConsiderNights, @NightsFrom, @NightsTo,
		@Agency, @ConsiderOriginallyAvailable, 0, @External, @BasedOnArrival)
	-- Shows
	union all
	select [Year], [Month], 0, 0, 0, 0, 0, Shows, 0, 0, 0
	from UFN_OR_GetMonthShows(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, @Market, @ConsiderNights, @NightsFrom, @NightsTo,
		@Agency, @ConsiderOriginallyAvailable, default, default, @External, @BasedOnArrival)
	-- Shows netos
	union all
	select [Year], [Month], 0, 0, 0, 0, 0, 0, Shows, 0, 0
	from UFN_OR_GetMonthShows(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, @Market, @ConsiderNights, @NightsFrom, @NightsTo,
		@Agency, @ConsiderOriginallyAvailable, 1, default, @External, @BasedOnArrival)
	-- Numero de ventas
	union all
	select [Year], [Month], 0, 0, 0, 0, 0, 0, 0, Sales, 0 
	from UFN_OR_GetMonthSales(@DateFrom, @DateTo, @LeadSources, @Market, @ConsiderNights, @NightsFrom, @NightsTo, @Agency, default, @External, @BasedOnArrival)
	-- Monto de ventas
	union all
	select [Year], [Month], 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetMonthSalesAmount(@DateFrom, @DateTo, @LeadSources, @Market, @ConsiderNights, @NightsFrom, @NightsTo, @Agency, default, @External, @BasedOnArrival)
) as D
group by [Year], [Month]
order by [Year], [Month]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


