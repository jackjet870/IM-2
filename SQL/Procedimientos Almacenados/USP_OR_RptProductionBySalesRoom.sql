if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptProductionBySalesRoom]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptProductionBySalesRoom]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de producción por sala de ventas
** 
** [wtorres]	06/Jun/2011 Creado
** [wtorres]	11/Jun/2011 Agregue el parametro @SalesRooms
**
*/
create procedure [dbo].[USP_OR_RptProductionBySalesRoom]
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
	select SalesRoom, Books, 0 as GrossBooks, 0 as Directs, 0 as Shows, 0 as GrossShows, 0 as Sales, 0 as SalesAmount
	from UFN_OR_GetSalesRoomBookings(@DateFrom, @DateTo, @SalesRooms, @ConsiderQuinellas, default, @BasedOnArrival)
	-- Bookings netos
	union all
	select SalesRoom, 0, Books, 0, 0, 0, 0, 0
	from UFN_OR_GetSalesRoomBookings(@DateFrom, @DateTo, @SalesRooms, @ConsiderQuinellas, 0, @BasedOnArrival)
	-- Directas
	union all
	select SalesRoom, 0, 0, Books, 0, 0, 0, 0
	from UFN_OR_GetSalesRoomBookings(@DateFrom, @DateTo, @SalesRooms, @ConsiderQuinellas, 1, @BasedOnArrival)
	-- Shows
	union all
	select SalesRoom, 0, 0, 0, Shows, 0, 0, 0
	from UFN_OR_GetSalesRoomShows(@DateFrom, @DateTo, @SalesRooms, @ConsiderQuinellas, default, @BasedOnArrival)
	-- Shows netos
	union all
	select SalesRoom, 0, 0, 0, 0, Shows, 0, 0
	from UFN_OR_GetSalesRoomShows(@DateFrom, @DateTo, @SalesRooms, @ConsiderQuinellas, 1, @BasedOnArrival)
	-- Ventas
	union all
	select SalesRoom, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetSalesRoomSales(@DateFrom, @DateTo, @SalesRooms, @BasedOnArrival)
	-- Monto de ventas
	union all
	select SalesRoom, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetSalesRoomSalesAmount(@DateFrom, @DateTo, @SalesRooms, @BasedOnArrival)
) as D
	inner join SalesRooms S on D.SalesRoom = S.srID
group by S.srN
order by SalesAmount desc, Shows desc, Books desc, S.srN

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

