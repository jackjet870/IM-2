USE [OrigosVCPalace]
GO
/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByHotel]    Script Date: 07/26/2016 09:39:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por hotel
** 
** [VKU] 13/May/2016 Creado
** [VKU] 18/May/2016 Modified. Agregue las columnas de ventas pendientes
**
*/
ALTER procedure [dbo].[USP_IM_RptProductionByHotel]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@FilterDeposit tinyint = 0			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
										--		3. Con deposito y shows sin deposito (Deposits & Flyers Show)

AS
set nocount on

declare	@FilterDepositAlternate tinyint	-- Filtro de depositos alterno (Aplica para bookings)

set @FilterDepositAlternate = @FilterDeposit
-- si se desea el reporte con deposito y shows sin deposito (Deposits & Flyers Show)
if @FilterDeposit = 3
	set @FilterDeposit = 0	-- Sin filtro

select
	-- Hotel
	ISNULL(D.Hotel, 'Not Specified') as HotelID,
	-- Bookings
	Sum(D.Books) as Books,
	-- In & Outs
	Sum(D.InOuts) as InOuts,
	-- Bookings netos
	Sum(D.GrossBooks) as GrossBooks,
	-- Shows
	Sum(D.Shows - D.InOuts) as Shows,
	-- Porcentaje de shows
	[dbo].UFN_OR_SecureDivision(Sum(D.Shows - D.InOuts), Sum(D.GrossBooks)) as ShowsFactor,
	-- Ventas
	Sum(D.Sales) as Sales_TOTAL,
	-- Monto de ventas
	Sum(D.SalesAmount) as SalesAmount_TOTAL,
	-- Ventas procesables
	Sum(D.Sales - D.SalesOOP + D.SalesCancel) as Sales_PROC,
	-- Monto de ventas procesables
	Sum(D.SalesAmount - D.SalesAmountOOP + D.SalesAmountCancel) as SalesAmount_PROC,
	-- Ventas Out Of Pending
	Sum(D.SalesOOP) as Sales_OOP,
	-- Monto de ventas Out Of Pending
	Sum(D.SalesAmountOOP) as SalesAmount_OOP,
	
	
	-- Ventas pendientes
	Sum(D.SalesPending) as Sales_PEND,
	-- Monto de ventas pendientes
	Sum(D.SalesAmountPending) as SalesAmount_PEND,
	
	
	-- Ventas canceladas
	Sum(D.SalesCancel) as Sales_CANCEL,
	-- Monto de ventas canceladas
	Sum(D.SalesAmountCancel) as SalesAmount_CANCEL,
	-- Subtotal
	Sum(D.SalesAmount) as Subtotal,
	-- Porcentaje de cancelacion
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmountCancel), Sum(D.SalesAmount + D.SalesAmountCancel)) as CancelFactor,
	-- Porcentaje de cierre
	[dbo].UFN_OR_SecureDivision(Sum(D.Sales), Sum(D.Shows - D.InOuts)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Shows - D.InOuts)) as Efficiency,
	-- Venta promedio
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Sales)) as AverageSale
from (
	-- Bookings
	select Hotel, Books, 0 as GrossBooks, 0 as InOuts, 0 as Shows, 0 as Sales, 0 as SalesAmount, 0 as SalesOOP, 0 as SalesAmountOOP, 0 as SalesPending, 0 as SalesAmountPending,
		0 as SalesCancel, 0 as SalesAmountCancel
	from UFN_IM_GetHotelBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @FilterDepositAlternate, default)
	-- Bookings netos
	union all
	select Hotel, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_IM_GetHotelBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @FilterDepositAlternate, 0)
	-- In & Outs
	union all
	select Hotel, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_IM_GetHotelShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @FilterDeposit, 1)
	-- Shows
	union all
	select Hotel, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_IM_GetHotelShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @FilterDeposit, default)
	-- Ventas procesables
	union all
	select Hotel, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0, 0, 0
	from UFN_IM_GetHotelSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, @FilterDeposit, default)
	-- Monto de ventas procesables
	union all
	select Hotel, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0, 0, 0
	from UFN_IM_GetHotelSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, @FilterDeposit, default)
	-- Ventas Out Of Pending
	union all
	select Hotel, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0
	from UFN_IM_GetHotelSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, 1, default, @FilterDeposit, default)
	-- Monto de ventas Out Of Pending
	union all
	select Hotel, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0
	from UFN_IM_GetHotelSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, 1, default, @FilterDeposit, default)
	
	
	-- Ventas pendientes
	union all
	select Hotel, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0
	from UFN_IM_GetHotelSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, @FilterDeposit, 1)
	-- Monto de ventas pendientes
	union all
	select Hotel, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0
	from UFN_IM_GetHotelSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, @FilterDeposit, 1)
	
	
	-- Ventas canceladas
	union all
	select Hotel, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_IM_GetHotelSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, @FilterDeposit,default)
	-- Monto de ventas canceladas
	union all
	select Hotel, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_IM_GetHotelSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, @FilterDeposit, default)
) as D
	left join Hotels H on D.Hotel = H.hoID
group by D.Hotel
order by SalesAmount_TOTAL desc, Shows desc, Books desc, D.Hotel










