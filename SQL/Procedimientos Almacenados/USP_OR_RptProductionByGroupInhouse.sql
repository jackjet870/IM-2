if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptProductionByGroupInhouse]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptProductionByGroupInhouse]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de producción por grupo (Inhouse)
** 
** [wtorres]	20/Jul/2010 Creado
** [wtorres]	17/Nov/2010 Agregué el parámetro @BasedOnArrival
**
*/
create procedure [dbo].[USP_OR_RptProductionByGroupInhouse]
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0,	-- Indica si se debe considerar quinielas
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada
as
set nocount on

select
	-- Grupos
	Count(*) as [Groups],
	-- Integrantes
	D.Integrants,
	-- Total de integrantes
	(Count(*) * D.Integrants) as TotalIntegrants,
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
	select
		-- Grupo
		T.[Group],
		-- Integrantes
		(select Count(*) from GuestsGroupsIntegrants where gjgx = T.[Group]) as Integrants,
		Sum(T.Arrivals) as Arrivals,
		Sum(T.Contacts) as Contacts,
		Sum(T.Availables) as Availables,
		Sum(T.Books) as Books,
		Sum(T.GrossBooks) as GrossBooks,
		Sum(T.Directs) as Directs,
		Sum(T.Shows) as Shows,
		Sum(T.GrossShows) as GrossShows,
		Sum(T.Sales) as Sales,
		Sum(T.SalesAmount) as SalesAmount,
		Sum(T.SalesSelfGen) as SalesSelfGen,
		Sum(T.SalesAmountSelfGen) as SalesAmountSelfGen
	from (
		-- Llegadas
		select [Group], Arrivals, 0 as Contacts, 0 as Availables, 0 as Books, 0 as GrossBooks, 0 as Directs, 0 as Shows,
			0 as GrossShows, 0 as Sales, 0 as SalesAmount, 0 as SalesSelfGen, 0 as SalesAmountSelfGen
		from UFN_OR_GetGroupArrivals(@DateFrom, @DateTo, @LeadSources)
		-- Contactos
		union all
		select [Group], 0, Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
		from UFN_OR_GetGroupContacts(@DateFrom, @DateTo, @LeadSources, @BasedOnArrival)
		-- Disponibles
		union all
		select [Group], 0, 0, Availables, 0, 0, 0, 0, 0, 0, 0, 0, 0
		from UFN_OR_GetGroupAvailables(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas)
		-- Bookings
		union all
		select [Group], 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0
		from UFN_OR_GetGroupBookings(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, default, @BasedOnArrival)
		-- Bookings netos
		union all
		select [Group], 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0
		from UFN_OR_GetGroupBookings(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, 0, @BasedOnArrival)
		-- Directas
		union all
		select [Group], 0, 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0
		from UFN_OR_GetGroupBookings(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, 1, @BasedOnArrival)
		-- Shows
		union all
		select [Group], 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0
		from UFN_OR_GetGroupShows(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, default, @BasedOnArrival)
		-- Shows netos
		union all
		select [Group], 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0
		from UFN_OR_GetGroupShows(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, 1, @BasedOnArrival)
		-- Ventas
		union all
		select [Group], 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0
		from UFN_OR_GetGroupSales(@DateFrom, @DateTo, @LeadSources, default, @BasedOnArrival)
		-- Monto de ventas
		union all
		select [Group], 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0
		from UFN_OR_GetGroupSalesAmount(@DateFrom, @DateTo, @LeadSources, default, @BasedOnArrival)
		-- Ventas Self Gen
		union all
		select [Group], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
		from UFN_OR_GetGroupSales(@DateFrom, @DateTo, @LeadSources, 1, @BasedOnArrival)
		-- Monto de ventas Self Gen
		union all
		select [Group], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
		from UFN_OR_GetGroupSalesAmount(@DateFrom, @DateTo, @LeadSources, 1, @BasedOnArrival)
	) as T
	group by T.[Group]
) as D
group by D.Integrants
order by SalesAmount_TOTAL desc, Shows desc, Books desc, Availables desc, Contacts desc, Arrivals desc

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

