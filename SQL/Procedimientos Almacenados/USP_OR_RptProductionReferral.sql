if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptProductionReferral]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptProductionReferral]
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion de referidos
** Los referidos son aquellas invitaciones con código de contrato (guO1) igual a REFREE-7 para 2007, REFREE-8 para 2008, etc
**
** [wtorres] 	08/Jul/2009 Created
** [wtorres] 	19/Nov/2013 Modified. Reemplace el uso de las funciones UFN_OR_GetMonthArrivalsByContract, UFN_OR_GetMonthArrivalsByContractLike,
**							UFN_OR_GetMonthShowsByContract, UFN_OR_GetMonthShowsByContractLike, UFN_OR_GetMonthSalesByContract,
**							UFN_OR_GetMonthSalesByContractLike, UFN_OR_GetMonthSalesAmountByContract y UFN_OR_GetMonthSalesAmountByContractLike
**							por UFN_OR_GetMonthArrivals, UFN_OR_GetMonthShows, UFN_OR_GetMonthSales y UFN_OR_GetMonthSalesAmount
** [wtorres] 	19/Feb/2015 Modified. Ahora contempla los huespedes que tengan Rate Code que empiece con CLRF, CLRP, RCIR, REF99 y CLR9.
**							Antes solo tomaba exactamente los Rate Codes CLRF, CLRP y RCIR
**
*/
CREATE procedure [dbo].[USP_OR_RptProductionReferral]
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime				-- Fecha hasta
as
set nocount on

declare @Contracts varchar(8000)

set @Contracts = 'REFREE%,CLRF%,CLRP%,RCIR%,REF99%,CLR9%'

select
	-- Nombre del mes
	DateName(Month, dbo.DateSerial([Year], [Month], 1)) as MonthN,
	-- Mes
	[Month],
	-- Año
	[Year],
	-- Llegadas
	Sum(Arrivals) as Arrivals,
	-- Shows
	Sum(Shows) as Shows,
	-- Porcentaje de shows
	[dbo].UFN_OR_SecureDivision(Sum(Shows), Sum(Arrivals)) as ShowsFactor,
	-- Ventas
	Sum(Sales) as Sales,
	-- Monto de ventas
	Sum(SalesAmount) as SalesAmount,
	-- Porcentaje de cierre
	[dbo].UFN_OR_SecureDivision(Sum(Sales), Sum(Shows)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(SalesAmount), Sum(Shows)) as Efficiency,
	-- Venta promedio
	[dbo].UFN_OR_SecureDivision(Sum(SalesAmount), Sum(Sales)) as AverageSale
from (
	-- Llegadas
	select [Year], [Month], Arrivals, 0 as Shows, 0 as Sales, 0 as SalesAmount
	from UFN_OR_GetMonthArrivals(@DateFrom, @DateTo, default, default, default, default, default, default, default, @Contracts, default)
	-- Shows
	union all
	select [Year], [Month], 0, Shows, 0, 0
	from UFN_OR_GetMonthShows(@DateFrom, @DateTo, default, default, default, default, default, default, default, default, default, @Contracts, default, default)
	-- Numero de ventas
	union all
	select [Year], [Month], 0, 0, Sales, 0
	from UFN_OR_GetMonthSales(@DateFrom, @DateTo, default, default, default, default, default, default, @Contracts, default, default)
	-- Monto de ventas
	union all
	select [Year], [Month], 0, 0, 0, SalesAmount
	from UFN_OR_GetMonthSalesAmount	(@DateFrom, @DateTo, default, default, default, default, default, default, @Contracts, default, default)
) as D
group by [Year], [Month]
order by [Year], [Month]


