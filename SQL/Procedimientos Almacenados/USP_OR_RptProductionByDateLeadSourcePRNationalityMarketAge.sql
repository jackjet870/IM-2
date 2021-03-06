if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptProductionByDateLeadSourcePRNationalityMarketAge]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptProductionByDateLeadSourcePRNationalityMarketAge]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por fecha, Lead Source, PR, nacionalidad, mercado y edad
** 
** [wtorres]	25/Jun/2010 Creado
** [wtorres]	19/Oct/2011 Elimine el uso de la funcion UFN_OR_GetDateLeadSourcePRCountryMarketAgeInOuts
**
*/
create procedure [dbo].[USP_OR_RptProductionByDateLeadSourcePRNationalityMarketAge]
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@ConsiderQuinellas bit = 0	-- Indica si se debe considerar quinielas
as
set nocount on

select
	-- Fecha
	D.[Date],
	-- Lead Source
	IsNull(D.LeadSource, '') as LeadSource,
	-- PR
	IsNull(D.PR, '') as PR,
	-- Nombre del PR
	IsNull(P.peN, 'Not Specified') as PRName,
	-- Nacionalidad
	IsNull(C.coNationality, 'Not Specified') as Nationality,
	-- Mercado
	IsNull(D.Market, '') as Market,
	-- Edad
	A.daN as Age,
	-- Contactos
	Sum(D.Contacts) as Contacts,
	-- Disponibles
	Sum(D.Availables) as Availables,
	-- Bookings netos
	Sum(D.GrossBooks) as GrossBooks,
	-- Directas
	Sum(D.Directs) as Directs,
	-- Bookings
	Sum(D.Books) as Books,
	-- Shows netos
	Sum(D.GrossShows) as GrossShows,
	-- In & Outs
	Sum(D.InOuts) as InOuts,
	-- Shows
	Sum(D.Shows) as Shows,
	-- Self Gens
	Sum(D.SelfGens) as SelfGens,
	-- Ventas
	Sum(D.Sales) as Sales,
	-- Monto de ventas
	Sum(D.SalesAmount) as SalesAmount
from (
	-- Contactos
	select [Date], LeadSource, PR, Country, Market, IsNull(Age, 0) as Age, Contacts, 0 as Availables, 0 as Books, 0 as GrossBooks, 0 as Directs,
		0 as Shows, 0 as GrossShows, 0 as InOuts, 0 as SelfGens, 0 as Sales, 0 as SalesAmount
	from UFN_OR_GetDateLeadSourcePRCountryMarketAgeContacts(@DateFrom, @DateTo)
	-- Disponibles
	union all
	select [Date], LeadSource, PR, Country, Market, IsNull(Age, 0), 0, Availables, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetDateLeadSourcePRCountryMarketAgeAvailables(@DateFrom, @DateTo, @ConsiderQuinellas)
	-- Bookings
	union all
	select [Date], LeadSource, PR, Country, Market, IsNull(Age, 0), 0, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetDateLeadSourcePRCountryMarketAgeBookings(@DateFrom, @DateTo, @ConsiderQuinellas, default)
	-- Bookings netos
	union all
	select [Date], LeadSource, PR, Country, Market, IsNull(Age, 0), 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetDateLeadSourcePRCountryMarketAgeBookings(@DateFrom, @DateTo, @ConsiderQuinellas, 1)
	-- Directas
	union all
	select [Date], LeadSource, PR, Country, Market, IsNull(Age, 0), 0, 0, 0, 0, Directs, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetDateLeadSourcePRCountryMarketAgeDirects(@DateFrom, @DateTo, @ConsiderQuinellas)
	-- Shows
	union all
	select [Date], LeadSource, PR, Country, Market, IsNull(Age, 0), 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0
	from UFN_OR_GetDateLeadSourcePRCountryMarketAgeShows(@DateFrom, @DateTo, @ConsiderQuinellas, default, default)
	-- Shows netos
	union all
	select [Date], LeadSource, PR, Country, Market, IsNull(Age, 0), 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0
	from UFN_OR_GetDateLeadSourcePRCountryMarketAgeShows(@DateFrom, @DateTo, @ConsiderQuinellas, default, 1)
	-- In & Outs
	union all
	select [Date], LeadSource, PR, Country, Market, IsNull(Age, 0), 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0
	from UFN_OR_GetDateLeadSourcePRCountryMarketAgeShows(@DateFrom, @DateTo, @ConsiderQuinellas, 1, default)
	-- Self Gens
	union all
	select [Date], LeadSource, PR, Country, Market, IsNull(Age, 0), 0, 0, 0, 0, 0, 0, 0, 0, SelfGens, 0, 0
	from UFN_OR_GetDateLeadSourcePRCountryMarketAgeSelfGens(@DateFrom, @DateTo, @ConsiderQuinellas)
	-- Ventas
	union all
	select [Date], LeadSource, PR, Country, Market, IsNull(Age, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetDateLeadSourcePRCountryMarketAgeSales(@DateFrom, @DateTo)
	-- Monto de ventas
	union all
	select [Date], LeadSource, PR, Country, Market, IsNull(Age, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetDateLeadSourcePRCountryMarketAgeSalesAmount(@DateFrom, @DateTo)
) as D
	left join Personnel P on D.PR = P.peID
	left join Countries C on D.Country = C.coID
	left join DemAge A on D.Age between A.daFrom and A.daTo
group by D.[Date], D.LeadSource, D.PR, P.peN, C.coNationality, D.Market, A.daN
order by D.[Date], D.LeadSource, D.PR, C.coNationality, D.Market, A.daN

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

