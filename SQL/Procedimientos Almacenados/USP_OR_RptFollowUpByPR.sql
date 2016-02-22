USE [OrigosVCPalace];
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de seguimiento por PR
** 
** [wtorres]		26/May/2010 Created
** [wtorres]		18/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [lormartinez]	22/Sep/2014 Modified. Se agrega parametro para filtro basado en fecha de booking
** [lormartinez]  15/Dic/2015 Modified. Se agrega paranetro default en las funciones de 
                  GetBooking y GetShows
*/
CREATE procedure [dbo].[USP_OR_RptFollowUpByPR]
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0,	-- Indica si se debe considerar quinielas
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada
as
set nocount on

select
	-- Clave del PR
	D.PR as PRID,
	-- Nombre del PR
	IsNull(P.peN, 'CODE NOT FOUND!') as PRN,
	-- Estatus del PR
	IsNull(P.peps, 'INACTIVE') as Status,	
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
	select PR, Contacts, 0 as Availables, 0 as FollowUps, 0 as Books, 0 as Shows, 0 as Sales, 0 as SalesAmount
	from UFN_OR_GetPRContacts(@DateFrom, @DateTo, @LeadSources, 1, @BasedOnArrival)
	-- Disponibles
	union all
	select PR, 0, Availables, 0, 0, 0, 0, 0
	from UFN_OR_GetPRAvailables(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, 1, @BasedOnArrival)
	-- Seguimientos
	union all
	select PR, 0, 0, FollowUps, 0, 0, 0, 0
	from UFN_OR_GetPRFollowUps(@DateFrom, @DateTo, @LeadSources, @BasedOnArrival)
	-- Bookings
	union all
	select PR, 0, 0, 0, Books, 0, 0, 0
	from UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, default, @BasedOnArrival,default)
	-- Shows
	union all
	select PR, 0, 0, 0, 0, Shows, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, default, default, default,
		default, default, @BasedOnArrival, default,default)
	-- Ventas
	union all
	select PR, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetPRSales(@DateFrom, @DateTo, @LeadSources, default, default, default, default, default, default, default, @BasedOnArrival, default)
	-- Monto de ventas
	union all
	select PR, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetPRSalesAmount(@DateFrom, @DateTo, @LeadSources, default, default, default, default, default, default, default, @BasedOnArrival, default)
) as D
	left join Personnel P on D.PR = P.peID
group by D.PR, P.peN, P.peps
order by Status, SalesAmount desc, Efficiency desc, Shows desc, Books desc, FollowUps desc, Availables desc, Contacts desc, D.PR
GO