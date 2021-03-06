if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptProductionByShowProgramProgram]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptProductionByShowProgramProgram]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por programa de show y programa
** 
** [wtorres]	22/Sep/2011 Creado
**
*/
create procedure [dbo].[USP_OR_RptProductionByShowProgramProgram]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@SalesRooms varchar(8000) = 'ALL',	-- Claves de salas de ventas
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
as
set nocount on

select
	-- Programa
	P.pgN as Program,
	-- Categoria de programa de show
	C.sgN as ShowProgramCategory,
	-- Programa de show
	S.skN as ShowProgram,
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
	select Program, ShowProgram, Books, 0 as GrossBooks, 0 as Directs, 0 as Shows, 0 as GrossShows, 0 as Sales, 0 as SalesAmount
	from UFN_OR_GetShowProgramProgramBookings(@DateFrom, @DateTo, @SalesRooms, @ConsiderQuinellas, default, @BasedOnArrival)
	-- Bookings netos
	union all
	select Program, ShowProgram, 0, Books, 0, 0, 0, 0, 0
	from UFN_OR_GetShowProgramProgramBookings(@DateFrom, @DateTo, @SalesRooms, @ConsiderQuinellas, 0, @BasedOnArrival)
	-- Directas
	union all
	select Program, ShowProgram, 0, 0, Books, 0, 0, 0, 0
	from UFN_OR_GetShowProgramProgramBookings(@DateFrom, @DateTo, @SalesRooms, @ConsiderQuinellas, 1, @BasedOnArrival)
	-- Shows
	union all
	select Program, ShowProgram, 0, 0, 0, Shows, 0, 0, 0
	from UFN_OR_GetShowProgramProgramShows(@DateFrom, @DateTo, @SalesRooms, @ConsiderQuinellas, default, @BasedOnArrival)
	-- Shows netos
	union all
	select Program, ShowProgram, 0, 0, 0, 0, Shows, 0, 0
	from UFN_OR_GetShowProgramProgramShows(@DateFrom, @DateTo, @SalesRooms, @ConsiderQuinellas, 1, @BasedOnArrival)
	-- Ventas
	union all
	select Program, ShowProgram, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetShowProgramProgramSales(@DateFrom, @DateTo, @SalesRooms, @BasedOnArrival)
	-- Monto de ventas
	union all
	select Program, ShowProgram, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetShowProgramProgramSalesAmount(@DateFrom, @DateTo, @SalesRooms, @BasedOnArrival)
) as D
	inner join ShowPrograms S on D.ShowProgram = S.skID
	inner join ShowProgramsCategories C on S.sksg = C.sgID
	inner join Programs P on D.Program = P.pgID
group by P.pgN, C.sgN, S.skN
order by P.pgN, C.sgN, SalesAmount desc, Shows desc, Books desc

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

