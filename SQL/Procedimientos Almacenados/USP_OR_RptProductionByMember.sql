if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptProductionByMember]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptProductionByMember]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por membresia
** 
** [wtorres]	25/Feb/2015 Created
** [wtorres]	19/Mar/2015 Modified. Ahora se agrupa por tipo de huesped
**
*/
create procedure [dbo].[USP_OR_RptProductionByMember]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000),			-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@Application varchar(15) = 'ALL',	-- Clave de membresia
	@Company decimal(2,0) = 0,			-- Clave de compania
	@Club int = 0,						-- Clave de club
	@OnlyWholesalers bit = 0,			-- Indica si se desean solo mayoristas
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
as
set nocount on

select
	-- Membresia
	case when W.wsApplication is not null then 'Wholesaler' else 'Retail' end as Wholesaler,
	C.clN as Club,
	case D.GuestType
		when 'M' then 1
		when 'G' then 2
		when 'R' then 3
	end as GuestTypeOrder,
	case D.GuestType
		when 'M' then 'Members'
		when 'G' then 'Guests'
		when 'R' then 'Referrals'
	end as GuestType,
	D.Company,
	D.Application,
	dbo.StringToTitle(dbo.Trim(case when D.Club = 2 then M.name else MP.name end)) as Name,
	case when D.Club = 2 then M.tour_date else MP.tour_date end as [Date],
	case when D.Club = 2 then M.total else MP.total end as Amount,
	
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
	select Club, Company, Application, GuestType, Arrivals, 0 as Contacts, 0 as Availables, 0 as Books, 0 as GrossBooks, 0 as Directs, 0 as Shows, 0 as GrossShows,
		0 as Sales, 0 as SalesAmount, 0 as SalesSelfGen, 0 as SalesAmountSelfGen
	from UFN_OR_GetMemberArrivals(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @Application, @Company, @Club, @OnlyWholesalers)
	-- Contactos
	union all
	select Club, Company, Application, GuestType, 0, Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetMemberContacts(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @Application, @Company, @Club, @OnlyWholesalers,
		@BasedOnArrival)
	-- Disponibles
	union all
	select Club, Company, Application, GuestType, 0, 0, Availables, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetMemberAvailables(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @ConsiderQuinellas, @Application, @Company, @Club,
		@OnlyWholesalers)
	-- Bookings
	union all
	select Club, Company, Application, GuestType, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetMemberBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @ConsiderQuinellas, default, default, default,
		@Application, @Company, @Club, @OnlyWholesalers, @BasedOnArrival)
	-- Bookings netos
	union all
	select Club, Company, Application, GuestType, 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetMemberBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @ConsiderQuinellas, default, 0, default,
		@Application, @Company, @Club, @OnlyWholesalers, @BasedOnArrival)
	-- Directas
	union all
	select Club, Company, Application, GuestType, 0, 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetMemberBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @ConsiderQuinellas, default, 1, default,
		@Application, @Company, @Club, @OnlyWholesalers, @BasedOnArrival)
	-- Shows
	union all
	select Club, Company, Application, GuestType, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0
	from UFN_OR_GetMemberShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @ConsiderQuinellas, default, default, default,
		@Application, @Company, @Club, @OnlyWholesalers, @BasedOnArrival)
	-- Shows netos
	union all
	select Club, Company, Application, GuestType, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0
	from UFN_OR_GetMemberShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @ConsiderQuinellas, default, default, 1,
		 @Application, @Company, @Club, @OnlyWholesalers, @BasedOnArrival)
	-- Ventas
	union all
	select Club, Company, Application, GuestType, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0
	from UFN_OR_GetMemberSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, default,
		@Application, @Company, @Club, @OnlyWholesalers, @BasedOnArrival)
	-- Monto de ventas
	union all
	select Club, Company, Application, GuestType, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0
	from UFN_OR_GetMemberSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, default,
		@Application, @Company, @Club, @OnlyWholesalers, @BasedOnArrival)
	-- Ventas Self Gen
	union all
	select Club, Company, Application, GuestType, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetMemberSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, 1, default, default, default,
		@Application, @Company, @Club, @OnlyWholesalers, @BasedOnArrival)
	-- Monto de ventas Self Gen
	union all
	select Club, Company, Application, GuestType, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetMemberSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, 1, default, default, default,
		@Application, @Company, @Club, @OnlyWholesalers, @BasedOnArrival)
) as D
	left join Clubs C on C.clID = D.Club
	left join Hotel.analista_h.clmember M on D.Club = 2 and M.company = D.Company and M.application = D.Application
	left join [svr-mssql-test].Hotel.analista_h.clmember MP on D.Club = 1 and MP.company = D.Company and MP.application = D.Application
	left join Wholesalers W on W.wscl = D.Club and W.wsCompany = D.Company and W.wsApplication = D.Application
group by W.wsApplication, D.Club, C.clN, D.Company, D.Application, M.name, MP.name, M.tour_date, MP.tour_date, M.total, MP.total, D.GuestType
order by Wholesaler desc, C.clN, GuestTypeOrder, SalesAmount_TOTAL desc, Shows desc, Books desc, Availables desc, Contacts desc, Arrivals desc, D.Application

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

