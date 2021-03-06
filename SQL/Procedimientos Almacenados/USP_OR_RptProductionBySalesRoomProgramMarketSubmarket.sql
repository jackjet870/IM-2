if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptProductionBySalesRoomProgramMarketSubmarket]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptProductionBySalesRoomProgramMarketSubmarket]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por sala de ventas, programa, mercado y submercado
** 
** [wtorres]	06/Jul/2011 Creado
**
*/
create procedure [dbo].[USP_OR_RptProductionBySalesRoomProgramMarketSubmarket]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@SalesRooms varchar(8000) = 'ALL',	-- Claves de salas de ventas
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
as
set nocount on

select
	-- Sala de ventas
	S.srN as SalesRoom,
	-- Programa
	P.pgN as Program,
	-- Mercado
	IsNull(Cast(D.Market as varchar(13)), 'Not Specified') as Market,
	-- Submercado
	D.Submarket,
	-- Bookings netos
	Sum(D.GrossBooks) as GrossBooks,
	-- Directas
	Sum(D.Directs) as Directs,
	-- Bookings
	Sum(D.Books) as Books,
	-- Shows netos
	Sum(D.GrossShows) as GrossShows,
	-- Shows
	Sum(D.Shows) as Shows,
	-- Porcentaje de shows
	[dbo].UFN_OR_SecureDivision(Sum(D.GrossShows), Sum(D.GrossBooks)) as ShowsFactor,
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
	-- Bookings
	select SalesRoom, Program, Market, Submarket, Books, 0 as GrossBooks, 0 as Directs, 0 as Shows, 0 as GrossShows, 0 as Sales, 0 as SalesAmount
	from UFN_OR_GetSalesRoomProgramMarketSubmarketBookings(@DateFrom, @DateTo, @SalesRooms, @ConsiderQuinellas, default, @BasedOnArrival)
	-- Bookings netos
	union all
	select SalesRoom, Program, Market, Submarket, 0, Books, 0, 0, 0, 0, 0
	from UFN_OR_GetSalesRoomProgramMarketSubmarketBookings(@DateFrom, @DateTo, @SalesRooms, @ConsiderQuinellas, 0, @BasedOnArrival)
	-- Directas
	union all
	select SalesRoom, Program, Market, Submarket, 0, 0, Books, 0, 0, 0, 0
	from UFN_OR_GetSalesRoomProgramMarketSubmarketBookings(@DateFrom, @DateTo, @SalesRooms, @ConsiderQuinellas, 1, @BasedOnArrival)
	-- Shows
	union all
	select SalesRoom, Program, Market, Submarket, 0, 0, 0, Shows, 0, 0, 0
	from UFN_OR_GetSalesRoomProgramMarketSubmarketShows(@DateFrom, @DateTo, @SalesRooms, @ConsiderQuinellas, default, @BasedOnArrival)
	-- Shows netos
	union all
	select SalesRoom, Program, Market, Submarket, 0, 0, 0, 0, Shows, 0, 0
	from UFN_OR_GetSalesRoomProgramMarketSubmarketShows(@DateFrom, @DateTo, @SalesRooms, @ConsiderQuinellas, 1, @BasedOnArrival)
	-- Ventas
	union all
	select SalesRoom, Program, Market, Submarket, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetSalesRoomProgramMarketSubmarketSales(@DateFrom, @DateTo, @SalesRooms, @BasedOnArrival)
	-- Monto de ventas
	union all
	select SalesRoom, Program, Market, Submarket, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetSalesRoomProgramMarketSubmarketSalesAmount(@DateFrom, @DateTo, @SalesRooms, @BasedOnArrival)
) as D
	inner join SalesRooms S on D.SalesRoom = S.srID
	inner join Programs P on D.Program = P.pgID
group by S.srN, P.pgN, D.Market, D.Submarket
order by P.pgN, D.Market, D.Submarket, SalesAmount desc, Shows desc, Books desc, S.srN

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

