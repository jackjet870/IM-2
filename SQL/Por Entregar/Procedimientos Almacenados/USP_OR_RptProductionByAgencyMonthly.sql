if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptProductionByAgencyMonthly]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptProductionByAgencyMonthly]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por agencia y mes
** 
** [wtorres]	09/Sep/2009 Creado
** [wtorres]	14/May/2010 Agregue las columnas de bookings netos y shows netos
** [wtorres]	24/Nov/2010 Agregue el parametro @BasedOnArrival
** [wtorres]	20/Dic/2013 Agregue el parametro @ConsiderQuinellas
** [aalcocer]	10/Jun/2016 Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create procedure [dbo].[USP_OR_RptProductionByAgencyMonthly]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
as
set nocount on

-- Datos
-- =============================================
select
	-- Año
	[Year],
	-- Total de agencia
	Cast(0 as bit) as AgencyTotal,
	-- Agencia
	Agency,
	IsNull(A.agN, 'Not Specified') as AgencyN,
	-- Total de Lead Source
	Cast(0 as bit) as LeadSourceTotal,
	-- Lead Source
	LeadSource,
	-- Mes
	[Month],
	-- Nombre del mes
	DateName(Month, dbo.DateSerial([Year], [Month], 1)) as MonthN,
	-- Llegadas
	Sum(Arrivals) as Arrivals,
	-- Contactos
	Sum(Contacts) as Contacts,
	-- Disponibles
	Sum(Availables) as Availables,
	-- Bookings netos
	Sum(GrossBooks) as GrossBooks,
	-- Bookings
	Sum(Books) as Books,
	-- Shows netos
	Sum(D.GrossShows) as GrossShows,
	-- Shows
	Sum(Shows) as Shows,
	-- Ventas
	Sum(Sales) as Sales,
	-- Monto de ventas
	Sum(SalesAmount) as SalesAmount,
	-- Porcentaje de cierre
	[dbo].UFN_OR_SecureDivision(Sum(Sales), Sum(Shows)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(SalesAmount), Sum(Shows)) as Efficiency
into #tbl_Data
from (
	-- Llegadas
	select [Year], [Month], Agency, LeadSource, Arrivals, 0 as Contacts, 0 as Availables, 0 as Books, 0 as GrossBooks, 0 as Shows,
		0 as GrossShows, 0 as Sales, 0 as SalesAmount
	from UFN_OR_GetMonthAgencyLeadSourceArrivals(@DateFrom, @DateTo, @Agencies)
	-- Contactos
	union all
	select [Year], [Month], Agency, LeadSource, 0, Contacts, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetMonthAgencyLeadSourceContacts(@DateFrom, @DateTo, @Agencies, @BasedOnArrival)
	-- Disponibles
	union all
	select [Year], [Month], Agency, LeadSource, 0, 0, Availables, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetMonthAgencyLeadSourceAvailables(@DateFrom, @DateTo, @Agencies, @ConsiderQuinellas)
	-- Bookings
	union all
	select [Year], [Month], Agency, LeadSource, 0, 0, 0, Books, 0, 0, 0, 0, 0
	from UFN_OR_GetMonthAgencyLeadSourceBookings(@DateFrom, @DateTo, @Agencies, @ConsiderQuinellas, default, @BasedOnArrival)
	-- Bookings netos
	union all
	select [Year], [Month], Agency, LeadSource, 0, 0, 0, 0, Books,  0, 0, 0, 0
	from UFN_OR_GetMonthAgencyLeadSourceBookings(@DateFrom, @DateTo, @Agencies, @ConsiderQuinellas, 0, @BasedOnArrival)
	-- Shows
	union all
	select [Year], [Month], Agency, LeadSource, 0, 0, 0, 0, 0, Shows, 0, 0, 0
	from UFN_OR_GetMonthAgencyLeadSourceShows(@DateFrom, @DateTo, @Agencies, @ConsiderQuinellas, default, @BasedOnArrival)
	-- Shows netos
	union all
	select [Year], [Month], Agency, LeadSource, 0, 0, 0, 0, 0, 0, Shows, 0, 0
	from UFN_OR_GetMonthAgencyLeadSourceShows(@DateFrom, @DateTo, @Agencies, @ConsiderQuinellas, 1, @BasedOnArrival)
	-- Numero de ventas
	union all
	select [Year], [Month], Agency, LeadSource, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetMonthAgencyLeadSourceSales(@DateFrom, @DateTo, @Agencies, @BasedOnArrival)
	-- Monto de ventas
	union all
	select [Year], [Month], Agency, LeadSource, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetMonthAgencyLeadSourceSalesAmount(@DateFrom, @DateTo, @Agencies, @BasedOnArrival)
) as D
	left join Agencies A on A.agID = D.Agency
group by [Year], Agency, A.agN, LeadSource, [Month]

-- Datos con totales
-- =============================================
-- Datos
select * from #tbl_Data

-- Totales por agencia
union all
select
	-- Año
	[Year],
	-- Total de agencia
	Cast(0 as bit) as AgencyTotal,
	-- Agencia
	Agency,
	'' as AgencyN,
	-- Total de Lead Source
	Cast(1 as bit) as LeadSourceTotal,
	-- Lead Source
	'TOTAL ' + Agency as LeadSource,
	-- Mes
	[Month],
	-- Nombre del mes
	MonthN,
	-- Llegadas
	Sum(Arrivals) as Arrivals,
	-- Contactos
	Sum(Contacts) as Contacts,
	-- Disponibles
	Sum(Availables) as Availables,
	-- Bookings netos
	Sum(GrossBooks) as GrossBooks,
	-- Bookings
	Sum(Books) as Books,
	-- Shows netos
	Sum(GrossShows) as GrossShows,
	-- Shows
	Sum(Shows) as Shows,
	-- Ventas
	Sum(Sales) as Sales,
	-- Monto de ventas
	Sum(SalesAmount) as SalesAmount,
	-- Porcentaje de cierre
	[dbo].UFN_OR_SecureDivision(Sum(Sales), Sum(Shows)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(SalesAmount), Sum(Shows)) as Efficiency
from #tbl_Data
group by [Year], Agency, [Month], MonthN

-- Totales por año
union all
select
	-- Año
	[Year],
	-- Total de agencia
	Cast(1 as bit) as AgencyTotal,
	-- Agencia
	'TOTAL ' + Cast([Year] as varchar(4)) as Agency,
	'' as AgencyN,
	-- Total de Lead Source
	Cast(1 as bit) as LeadSourceTotal,
	-- Lead Source
	'TOTAL AGENCIES' as LeadSource,
	-- Mes
	[Month],
	-- Nombre del mes
	MonthN,
	-- Llegadas
	Sum(Arrivals) as Arrivals,
	-- Contactos
	Sum(Contacts) as Contacts,
	-- Disponibles
	Sum(Availables) as Availables,
	-- Bookings netos
	Sum(GrossBooks) as GrossBooks,
	-- Bookings
	Sum(Books) as Books,
	-- Shows netos
	Sum(GrossShows) as GrossShows,
	-- Shows
	Sum(Shows) as Shows,
	-- Ventas
	Sum(Sales) as Sales,
	-- Monto de ventas
	Sum(SalesAmount) as SalesAmount,
	-- Porcentaje de cierre
	[dbo].UFN_OR_SecureDivision(Sum(Sales), Sum(Shows)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(SalesAmount), Sum(Shows)) as Efficiency
from #tbl_Data
group by [Year], [Month], MonthN
order by [Year], AgencyTotal, Agency, LeadSourceTotal, LeadSource, [Month]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

