if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptStatsByLocationMonthly]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptStatsByLocationMonthly]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de estadisticas por locacion (mensual)
** 
** [wtorres]	25/Sep/2009 Creado
** [wtorres]	26/Sep/2012 Renombre el campo Shows
**
*/
create procedure [dbo].[USP_OR_RptStatsByLocationMonthly]
	@DateFrom datetime,		-- Fecha desde
	@DateTo datetime,		-- Fecha hasta
	@SalesRoom varchar(10)	-- Clave de la sala de ventas
as
set nocount on

declare
	@DateFromPrevious datetime,	-- Fecha desde anterior
	@DateToPrevious datetime	-- Fecha hasta anterior

set @DateFromPrevious = DateAdd(Year, -1, @DateFrom)
set @DateToPrevious = DateAdd(Year, -1, @DateTo)

select
	-- Programa
	P.pgN as Program,
	-- Locacion
	L.loN as Location,
	-- Meta
	IsNull(G.goGoal, 0) as Goal,
	-- Reservaciones
	Sum(D.Books) as Books,
	-- Shows netos
	Sum(D.Shows - D.Directs) as GrossUPS,
	-- Porcentaje de show
	[dbo].UFN_OR_SecureDivision(Sum(D.Shows - D.Directs), Sum(D.Books)) as ShowsFactor,
	-- Directas
	Sum(D.Directs) as Directs,
	-- Shows
	Sum(D.Shows) as Shows,
	-- Ventas
	Sum(D.Sales) as Sales,
	-- Monto de ventas
	Sum(SalesAmount) as SalesAmount,
	-- Porcentaje de cierre
	[dbo].UFN_OR_SecureDivision(Sum(D.Sales), Sum(D.Shows)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Shows)) as Efficiency,
	-- Venta promedio
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Sales)) as AverageSale,
	-- Shows anteriores
	Sum(D.UPSPrevious) as UPSPrevious,
	-- Monto de ventas anterior
	Sum(SalesAmountPrevious) as SalesAmountPrevious
from (
	-- Reservaciones
	select Location, Books, 0 as Shows, 0 as Directs, 0 as Sales, 0 as SalesAmount, 0 as UPSPrevious, 0 as SalesAmountPrevious
	from UFN_OR_GetLocationBookings(@DateFrom, @DateTo, @SalesRoom)
	-- Shows
	union all
	select Location, 0, Shows, 0, 0, 0, 0, 0 from UFN_OR_GetLocationShows(@DateFrom, @DateTo, @SalesRoom)
	-- Directas
	union all
	select Location, 0, 0, Directs, 0, 0, 0, 0 from UFN_OR_GetLocationDirects(@DateFrom, @DateTo, @SalesRoom)
	-- Numero de ventas
	union all
	select Location, 0, 0, 0, Sales, 0, 0, 0 from UFN_OR_GetLocationSales(@DateFrom, @DateTo, @SalesRoom, default)
	-- Monto de ventas
	union all
	select Location, 0, 0, 0, 0, SalesAmount, 0, 0 from UFN_OR_GetLocationSalesAmount(@DateFrom, @DateTo, @SalesRoom, default)
	-- Shows anteriores
	union all
	select Location, 0, 0, 0, 0, 0, Shows, 0 from UFN_OR_GetLocationShows(@DateFromPrevious, @DateToPrevious, @SalesRoom)
	-- Monto de ventas
	union all
	select Location, 0, 0, 0, 0, 0, 0, SalesAmount from UFN_OR_GetLocationSalesAmount(@DateFromPrevious, @DateToPrevious, @SalesRoom, default)
) as D
	left join Locations L on D.Location = L.loID
	left join LeadSources LS on L.lols = LS.lsID
	left join Programs P on LS.lspg = P.pgID
	left join Goals G on G.gopy = 'LO' and D.Location = G.goPlaceID and G.goDateFrom = @DateFrom 
		and G.goDateTo = @DateTo and G.gopd = 'M'
group by P.pgN, L.loN, G.goGoal
order by P.pgN, SalesAmount desc, Efficiency desc, Shows desc, L.loN

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

