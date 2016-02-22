if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptSalesByLocationMonthly]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptSalesByLocationMonthly]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene los datos del reporte de ventas por locacion (mensual)
** 
** [wtorres]	22/Oct/2009 Creado
** [wtorres]	26/Sep/2012 Renombre el campo Shows
**
*/
create procedure [dbo].[USP_OR_RptSalesByLocationMonthly]
	@DateFrom datetime,		-- Fecha desde
	@DateTo datetime,		-- Fecha hasta
	@SalesRoom varchar(10)	-- Clave de la sala de ventas
as
set nocount on

select
	-- Locacion
	L.loN as Location,
	-- Total de locacion
	Cast(0 as bit) as LocationTotal,
	-- Año
	D.[Year],
	-- Mes
	D.[Month],
	-- Nombre del mes
	DateName(Month, dbo.DateSerial(D.[Year], D.[Month], 1)) as MonthN,
	-- Shows
	Sum(D.Shows) as Shows,
	-- Ventas
	Sum(D.Sales) as Sales,
	-- Monto de ventas total
	Sum(SalesAmount + SalesAmountCancel) as SalesAmountTotal,
	-- Monto de ventas canceladas
	Sum(SalesAmountCancel) as SalesAmountCancel,
	-- Monto de ventas
	Sum(SalesAmount) as SalesAmount,
	-- Porcentaje de cierre
	[dbo].UFN_OR_SecureDivision(Sum(D.Sales), Sum(D.Shows)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Shows)) as Efficiency,
	-- Venta promedio
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Sales)) as AverageSale
into #tbl_Data
from (
	-- Shows
	select Location, [Year], [Month], Shows, 0 as Sales, 0 as SalesAmount, 0 as SalesAmountCancel
	from UFN_OR_GetLocationMonthShows(@DateFrom, @DateTo, @SalesRoom)
	-- Numero de ventas
	union all
	select Location, [Year], [Month], 0, Sales, 0, 0 from UFN_OR_GetLocationMonthSales(@DateFrom, @DateTo, @SalesRoom)
	-- Monto de ventas
	union all
	select Location, [Year], [Month], 0, 0, SalesAmount, 0 from UFN_OR_GetLocationMonthSalesAmount(@DateFrom, @DateTo, @SalesRoom, default)
	-- Monto de ventas canceladas
	union all
	select Location, [Year], [Month], 0, 0, 0, SalesAmount from UFN_OR_GetLocationMonthSalesAmount(@DateFrom, @DateTo, @SalesRoom, 1)
) as D
	left join Locations L on D.Location = L.loID
group by L.loN, D.[Year], D.[Month]
order by L.loN, D.[Year], D.[Month]

-- Datos con totales
-- =============================================
-- Datos
select * from #tbl_Data

-- Totales por año
union all
select
	-- Locacion
	'TOTAL LOCATIONS' as Location,
	-- Total de locacion
	Cast(1 as bit) as LocationTotal,
	-- Año
	[Year],	
	-- Mes
	[Month],
	-- Nombre del mes
	MonthN,
	-- Shows
	Sum(D.Shows) as Shows,
	-- Ventas
	Sum(D.Sales) as Sales,
	-- Monto de ventas total
	Sum(SalesAmount + SalesAmountCancel) as SalesAmountTotal,
	-- Monto de ventas canceladas
	Sum(SalesAmountCancel) as SalesAmountCancel,
	-- Monto de ventas
	Sum(SalesAmount) as SalesAmount,
	-- Porcentaje de cierre
	[dbo].UFN_OR_SecureDivision(Sum(D.Sales), Sum(D.Shows)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Shows)) as Efficiency,
	-- Venta promedio
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Sales)) as AverageSale
from #tbl_Data as D
group by [Year], [Month], MonthN
order by LocationTotal, Location, [Year], [Month]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

