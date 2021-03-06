if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptFollowUpByAgency]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptFollowUpByAgency]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de seguimiento por agencia
** 
** [wtorres]	26/May/2010 Created
** [wtorres]	14/Oct/2010 Modified. Agregue el parametro @BasedOnArrival
** [wtorres]	23/Dic/2011 Modified. Agregue el campo de descripcion de agencia
** [alesanchez]	03/Sep/2013 Modified. Se adecuaron los parametros a la funcion UFN_OR_GetAgencyMarketOriginallyAvailableShows
** [lchairez]	06/Mar/2014 Modified. Se agrega un left join con la tabla Markets
*/
create procedure [dbo].[USP_OR_RptFollowUpByAgency]
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0,	-- Indica si se debe considerar quinielas
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada
as
set nocount on

select
	-- Agencia
	D.Agency,
	IsNull(A.agN, 'Not Specified') as AgencyN,
	-- Mercado
	IsNull(M.mkN, 'Not Specified')as Market,
	-- Originalmente disponible
	D.OriginallyAvailable,
	-- Contactos
	Sum(D.Contacts) as Contacts,
	-- Disponibles
	Sum(D.Availables) as Availables,
	-- Seguimientos
	Sum(D.FollowUps) as FollowUps,
	-- Porcentaje de seguimientos
	[dbo].UFN_OR_SecureDivision(Sum(D.FollowUps), Sum(D.Availables)) as FollowUpsFactor,
	-- Bookings
	Sum(D.Books) as Books,
	-- Porcentaje de bookings
	[dbo].UFN_OR_SecureDivision(Sum(D.Books), Sum(D.FollowUps)) as BooksFactor,
	-- Shows
	Sum(D.Shows) as Shows,
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
from (
	-- Contactos
	select Agency, Market, OriginallyAvailable, Contacts, 0 as Availables, 0 as FollowUps, 0 as Books, 0 as Shows, 0 as Sales,
		0 as SalesAmount
	from UFN_OR_GetAgencyMarketOriginallyAvailableContacts(@DateFrom, @DateTo, @LeadSources, default, default, default, default, default, @BasedOnArrival)
	-- Disponibles
	union all
	select Agency, Market, OriginallyAvailable, 0, Availables, 0, 0, 0, 0, 0
	from UFN_OR_GetAgencyMarketOriginallyAvailableAvailables(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, default, default, default, default, default)
	-- Seguimientos
	union all
	select Agency, Market, OriginallyAvailable, 0, 0, FollowUps, 0, 0, 0, 0
	from UFN_OR_GetAgencyMarketOriginallyAvailableFollowUps(@DateFrom, @DateTo, @LeadSources, @BasedOnArrival)
	-- Bookings
	union all
	select Agency, Market, OriginallyAvailable, 0, 0, 0, Books, 0, 0, 0
	from UFN_OR_GetAgencyMarketOriginallyAvailableBookings(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, default, default, default,
		default, default, default, @BasedOnArrival)
	-- Shows
	union all
	select Agency, Market, OriginallyAvailable, 0, 0, 0, 0, Shows, 0, 0 
	from UFN_OR_GetAgencyMarketOriginallyAvailableShows(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, default, default, default,
		default, default, default, default, default, default, default, @BasedOnArrival)
	-- Ventas
	union all
	select Agency, Market, 'ORIGINALLY AVAILABLES', 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetAgencyMarketSales(@DateFrom, @DateTo, @LeadSources, default, default, default, default, default, @BasedOnArrival)
	-- Monto de ventas
	union all
	select Agency, Market, 'ORIGINALLY AVAILABLES', 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetAgencyMarketSalesAmount(@DateFrom, @DateTo, @LeadSources, default, default, default, default, default, @BasedOnArrival)
) as D
	left join Agencies A on A.agID = D.Agency
	left join Markets M on D.Market = M.mkID
group by D.Agency, A.agN, M.mkN, D.OriginallyAvailable
order by OriginallyAvailable, Market, SalesAmount desc, Efficiency desc, Shows desc, Books desc, FollowUps desc, Availables desc, Contacts desc,
	Agency

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

