if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptProductionByAgencyInhouse]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptProductionByAgencyInhouse]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de producccion por agencia (Inhouse)
**	1. Reporte
**	2. Tipos de membresia
**	3. Ventas por tipo de membresia
** 
** [wtorres]	24/Abr/2009 Modified. Ahora se pasa la lista de Lead Sources como un solo parametro
** [wtorres]	13/Ago/2009 Modified. Ahora el campo de originalmente diponible se devuelve como cadena
**							y se establecen valores default a los campos de agencia y mercado.
**							Ahora se devuelve la descripcion de los tipos de membresia y se ordenan por nivel
** [wtorres]	23/Oct/2009 Modified. Ahora se devuelven los datos agrupados
** [wtorres]	19/Abr/2010 Modified. Agregue las columnas para diferenciar las cantidades netas y totales de bookings y shows
** [wtorres]	14/Oct/2010 Modified. Agregue el parametro @BasedOnArrival
** [wtorres]	23/Dic/2011 Modified. Agregue el campo de descripcion de agencia
** [alesanchez]	03/Sep/2013 Modified. Agregue el campos IO, WO, RT, CT, Save, T Tours, UPS
** [wtorres]	16/Dic/2013 Modified. Ahora solo devuelve los tipos de membresia que tenga el reporte
** [wtorres]	26/May/2014 Modified. Ahora la eficiencia y el porcentaje de cierre se dividen entre la columna UPS en lugar de Shows
** [wtorres]	04/Jun/2015 Modified. Agregue el parametro @External
**
*/
create procedure [dbo].[USP_OR_RptProductionByAgencyInhouse]
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0,		-- Indica si se debe considerar quinielas
	@ConsiderNights bit = 0,		-- Indica si se debe considerar el numero de noches
	@NightsFrom int = 0,			-- Numero de noches desde
	@NightsTo int = 0,				-- Numero de noches hasta
	@SalesByMembershipType bit = 0,	-- Indica si se desean las ventas por tipo de membresia
	@OnlyQuinellas bit = 0,			-- Indica si se desean solo las quinielas
	@External int = 0,				-- Filtro de invitaciones externas
									--		0. Sin filtro
									--		1. Excluir invitaciones externas
									--		2. Solo invitaciones externas
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
as
set nocount on

-- 1. Reporte
select
	-- Agencia
	D.Agency,
	IsNull(A.agN, 'Not Specified') as AgencyN,
	-- Mercado
	IsNull(Cast(D.Market as varchar(13)), 'Not Specified') as Market,
	IsNull(Cast(M.mkN as varchar(20)), 'Not Specified') as MarketN,
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
	-- Bookings sin directas
	Sum(D.Books) - Sum(D.Directs) as GrossBooks,
	-- Directas
	Sum(D.Directs) as Directs,
	-- Bookings
	Sum(D.Books) as Books,
	-- Porcentaje de Bookings
	[dbo].UFN_OR_SecureDivision(Sum(D.Books), Sum(D.Availables)) as BooksFactor,
	-- Shows netos
	Sum(D.GrossShows) as GrossShows,
	-- Shows
	Sum(D.Shows) as Shows,
	-- Porcentaje de shows
	[dbo].UFN_OR_SecureDivision(Sum(D.GrossShows), Sum(D.Books) - Sum(D.Directs)) as ShowsFactor,
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
	[dbo].UFN_OR_SecureDivision(Sum(D.Sales), Sum(D.UPS)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.UPS)) as Efficiency,
	-- Venta promedio
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Sales)) as AverageSale
from (
	-- Llegadas
	select Agency, Market, OriginallyAvailable, Arrivals, 0 as Contacts, 0 as Availables, 0 as Books, 0 as Directs, 0 as Shows,
		0 as GrossShows, 0 as InOuts, 0 as WalkOuts, 0 as Tours, 0 as CourtesyTours, 0 as SaveTours, 0 as UPS, 0 as Sales, 0 as SalesAmount
	from UFN_OR_GetAgencyMarketOriginallyAvailableArrivals(@DateFrom, @DateTo, @LeadSources, @ConsiderNights, @NightsFrom, @NightsTo,
		@OnlyQuinellas, @External)
	-- Contactos
	union all
	select Agency, Market, OriginallyAvailable, 0, Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetAgencyMarketOriginallyAvailableContacts(@DateFrom, @DateTo, @LeadSources, @ConsiderNights, @NightsFrom, @NightsTo,
		@OnlyQuinellas, @External, @BasedOnArrival)
	-- Disponibles
	union all
	select Agency, Market, OriginallyAvailable, 0, 0, Availables, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetAgencyMarketOriginallyAvailableAvailables(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, @ConsiderNights, @NightsFrom,
		@NightsTo, @OnlyQuinellas, @External)
	-- Bookings
	union all
	select Agency, Market, OriginallyAvailable, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetAgencyMarketOriginallyAvailableBookings(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, @ConsiderNights, @NightsFrom,
		@NightsTo, @OnlyQuinellas, default, @External, @BasedOnArrival)
	-- Directas
	union all
	select Agency, Market, OriginallyAvailable, 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetAgencyMarketOriginallyAvailableBookings(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, @ConsiderNights, @NightsFrom,
		@NightsTo, @OnlyQuinellas, 1, @External, @BasedOnArrival)
	-- Shows
	union all
	select Agency, Market, OriginallyAvailable, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetAgencyMarketOriginallyAvailableShows(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, @ConsiderNights, @NightsFrom,
		@NightsTo, @OnlyQuinellas, default, default, default, default, default, @External, @BasedOnArrival)
	-- Shows netos
	union all
	select Agency, Market, OriginallyAvailable, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetAgencyMarketOriginallyAvailableShows(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, @ConsiderNights, @NightsFrom,
		@NightsTo, @OnlyQuinellas, default, default, default, 1, default, @External, @BasedOnArrival)
	-- In & Outs
	union all
	select Agency, Market, OriginallyAvailable, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetAgencyMarketOriginallyAvailableShows(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, @ConsiderNights, @NightsFrom,
		@NightsTo, @OnlyQuinellas, 1, default, default, default, default, @External, @BasedOnArrival)
	-- Walk Outs
	union all
	select Agency, Market, OriginallyAvailable, 0, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetAgencyMarketOriginallyAvailableShows(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, @ConsiderNights, @NightsFrom,
		@NightsTo, @OnlyQuinellas, default, 1, default, default, default, @External, @BasedOnArrival)
	-- Tours regulares
	union all
	select Agency, Market, OriginallyAvailable, 0, 0, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0
	from UFN_OR_GetAgencyMarketOriginallyAvailableShows(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, @ConsiderNights, @NightsFrom,
		@NightsTo, @OnlyQuinellas, default, default, 1, default, default, @External, @BasedOnArrival)
	-- Tours de cortesia
	union all
	select Agency, Market, OriginallyAvailable, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0
	from UFN_OR_GetAgencyMarketOriginallyAvailableShows(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, @ConsiderNights, @NightsFrom,
		@NightsTo, @OnlyQuinellas, default, default, 2, default, default, @External, @BasedOnArrival)
	-- Tours de rescate
	union all
	select Agency, Market, OriginallyAvailable, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0
	from UFN_OR_GetAgencyMarketOriginallyAvailableShows(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, @ConsiderNights, @NightsFrom,
		@NightsTo, @OnlyQuinellas, default, default, 3, default, default, @External, @BasedOnArrival)
	-- UPS (son todos los shows con tour o venta)
	union all
	select Agency, Market, OriginallyAvailable, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0
	from UFN_OR_GetAgencyMarketOriginallyAvailableShows(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, @ConsiderNights, @NightsFrom,
		@NightsTo, @OnlyQuinellas, default, default, default, default, 1, @External, @BasedOnArrival)
	-- Ventas
	union all
	select Agency, Market, 'ORIGINALLY AVAILABLES', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetAgencyMarketSales(@DateFrom, @DateTo, @LeadSources, @ConsiderNights, @NightsFrom, @NightsTo, @OnlyQuinellas, @External, @BasedOnArrival)
	-- Monto de ventas
	union all
	select Agency, Market, 'ORIGINALLY AVAILABLES', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetAgencyMarketSalesAmount(@DateFrom, @DateTo, @LeadSources, @ConsiderNights, @NightsFrom, @NightsTo, @OnlyQuinellas, @External, @BasedOnArrival)
) as D
	left join Agencies A on A.agID = D.Agency
	left join Markets M on M.mkID = D.Market
group by D.Agency, A.agN, D.Market, M.mkN, D.OriginallyAvailable
order by D.OriginallyAvailable, D.Market, SalesAmount desc, Efficiency desc, Shows desc, Books desc, Availables desc, Contacts desc, Arrivals desc,
	D.Agency

-- si se desean las ventas por tipo de membresia
if @SalesByMembershipType = 1 begin

	-- Ventas por tipo de membresia
	select
		-- Agencia
		IsNull(D.Agency, 'Not Specified') as Agency,
		-- Mercado
		IsNull(Cast(D.Market as varchar(13)), 'Not Specified') as Market,
		-- Tipo de membresia
		D.MembershipType,
		-- Ventas
		Sum(D.Sales) as Sales,
		-- Monto de ventas
		Sum(D.SalesAmount) as SalesAmount
	into #SalesByMembershipType
	from (
		-- Ventas
		select Agency, Market, MembershipType, Sales, 0 as SalesAmount
		from UFN_OR_GetAgencyMarketMembershipTypeSales(@DateFrom, @DateTo, @LeadSources, @ConsiderNights, @NightsFrom, @NightsTo, @OnlyQuinellas,
			@External, @BasedOnArrival)
		-- Monto de ventas
		union all
		select Agency, Market, MembershipType, 0, SalesAmount
		from UFN_OR_GetAgencyMarketMembershipTypeSalesAmount(@DateFrom, @DateTo, @LeadSources, @ConsiderNights, @NightsFrom, @NightsTo,
			@OnlyQuinellas, @External, @BasedOnArrival)
	) as D
	group by D.Agency, D.Market, D.MembershipType
	order by D.Market, D.Agency, D.MembershipType
	
	-- 2. Tipos de membresia
	select distinct M.mtID, M.mtN, M.mtLevel
	from MemberShipTypes M
		inner join #SalesByMembershipType S on S.MembershipType = M.mtID
	order by M.mtLevel

	-- 3. Ventas por tipo de membresia
	select * from #SalesByMembershipType
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

