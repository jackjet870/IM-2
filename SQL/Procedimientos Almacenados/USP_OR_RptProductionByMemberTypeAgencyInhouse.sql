if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptProductionByMemberTypeAgencyInhouse]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptProductionByMemberTypeAgencyInhouse]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de producccion por tipo de socio y agencia (Inhouse)
** 
** [wtorres]	26/Ene/2012 Creado
**
*/
create procedure [dbo].[USP_OR_RptProductionByMemberTypeAgencyInhouse]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000),			-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',		-- Claves de mercados
	@Agencies varchar(8000) = 'ALL',	-- Claves de agencias
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
as
set nocount on

select
	-- Tipo de socio
	D.MemberType,
	-- Agencia
	D.Agency,
	IsNull(A.agN, 'Not Specified') as AgencyN,
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
	-- Porcentaje de Bookings
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
	select MemberType, Agency, Arrivals, 0 as Contacts, 0 as Availables, 0 as Books, 0 as GrossBooks, 0 as Directs, 0 as Shows, 0 as GrossShows,
		0 as Sales, 0 as SalesAmount, 0 as SalesSelfGen, 0 as SalesAmountSelfGen
	from UFN_OR_GetMemberTypeAgencyArrivals(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies)
	-- Contactos
	union all
	select MemberType, Agency, 0, Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetMemberTypeAgencyContacts(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @BasedOnArrival)
	-- Disponibles
	union all
	select MemberType, Agency, 0, 0, Availables, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetMemberTypeAgencyAvailables(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @ConsiderQuinellas)
	-- Bookings
	union all
	select MemberType, Agency, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetMemberTypeAgencyBookings(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @ConsiderQuinellas, default, @BasedOnArrival)
	-- Bookings netos
	union all
	select MemberType, Agency, 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetMemberTypeAgencyBookings(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @ConsiderQuinellas, 0, @BasedOnArrival)
	-- Directas
	union all
	select MemberType, Agency, 0, 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetMemberTypeAgencyBookings(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @ConsiderQuinellas, 1, @BasedOnArrival)
	-- Shows
	union all
	select MemberType, Agency, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0
	from UFN_OR_GetMemberTypeAgencyShows(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @ConsiderQuinellas, default, @BasedOnArrival)
	-- Shows netos
	union all
	select MemberType, Agency, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0
	from UFN_OR_GetMemberTypeAgencyShows(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @ConsiderQuinellas, 1, @BasedOnArrival)
	-- Ventas
	union all
	select MemberType, Agency, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0
	from UFN_OR_GetMemberTypeAgencySales(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, default, @BasedOnArrival)
	-- Monto de ventas
	union all
	select MemberType, Agency, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0
	from UFN_OR_GetMemberTypeAgencySalesAmount(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, default, @BasedOnArrival)
	-- Ventas Self Gen
	union all
	select MemberType, Agency, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetMemberTypeAgencySales(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, 1, @BasedOnArrival)
	-- Monto de ventas Self Gen
	union all
	select MemberType, Agency, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetMemberTypeAgencySalesAmount(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, 1, @BasedOnArrival)
) as D
	left join Agencies A on A.agID = D.Agency
group by D.MemberType, D.Agency, A.agN
order by D.Agency, SalesAmount_TOTAL desc, Efficiency desc, Shows desc, Books desc, Availables desc, Contacts desc, Arrivals desc, D.MemberType

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

