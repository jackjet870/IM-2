if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptProductionByAgencyMarketHotelOutside]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptProductionByAgencyMarketHotelOutside]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por agencia, mercado y hotel (Outside)
** 
** [wtorres]	24/Dic/2011 Ahora se pasa la lista de Lead Sources como un solo parametro
**
*/
create procedure [dbo].[USP_OR_RptProductionByAgencyMarketHotelOutside]
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
as
set nocount on

declare	@FilterDepositAlternate tinyint	-- Filtro de depositos alterno (Aplica para bookings)

set @FilterDepositAlternate = @FilterDeposit
-- si se desea el reporte con deposito y shows sin deposito (Deposits & Flyers Show)
if @FilterDeposit = 3
	set @FilterDeposit = 0	-- Sin filtro

select
	-- Agencia
	D.Agency,
	IsNull(A.agN, 'Not Specified') as AgencyN,
	-- Mercado
	IsNull(Cast(D.Market as varchar(13)), 'Not Specified') as Market,
	-- Hotel
	D.Hotel,
	-- Bookings
	Sum(D.Books) as Books,
	-- In & Outs
	Sum(D.InOuts) as InOuts,
	-- Bookings netos
	Sum(D.GrossBooks) as GrossBooks,
	-- Shows
	Sum(D.Shows) as Shows,
	-- Porcentaje de shows
	[dbo].UFN_OR_SecureDivision(Sum(D.Shows), Sum(D.GrossBooks)) as ShowsFactor,
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
	-- Ventas canceladas
	Sum(D.SalesCancel) as Sales_CANCEL,
	-- Monto de ventas canceladas
	Sum(D.SalesAmountCancel) as SalesAmount_CANCEL,
	-- Subtotal
	Sum(D.SalesAmount) as Subtotal,
	-- Porcentaje de cancelacion
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmountCancel), Sum(D.SalesAmount + D.SalesAmountCancel)) as CancelFactor,
	-- Porcentaje de cierre
	[dbo].UFN_OR_SecureDivision(Sum(D.Sales), Sum(D.Shows)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Shows)) as Efficiency,
	-- Venta promedio
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Sales)) as AverageSale
from (
	-- Bookings
	select Agency, Market, Hotel, Books, 0 as GrossBooks, 0 as InOuts, 0 as Shows, 0 as Sales, 0 as SalesAmount, 0 as SalesOOP, 0 as SalesAmountOOP,
		0 as SalesCancel, 0 as SalesAmountCancel
	from UFN_OR_GetAgencyMarketHotelBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, default, default,
		default)
	-- Bookings netos
	union all
	select Agency, Market, Hotel, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetAgencyMarketHotelBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, default, 0,
		default)
	-- In & Outs
	union all
	select Agency, Market, Hotel, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetAgencyMarketHotelShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, 1, default, default)
	-- Shows
	union all
	select Agency, Market, Hotel, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetAgencyMarketHotelShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, 0, default, default)
	-- Ventas procesables
	union all
	select Agency, Market, Hotel, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0
	from UFN_OR_GetAgencyMarketHotelSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, @FilterDeposit, default)
	-- Monto de ventas procesables
	union all
	select Agency, Market, Hotel, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0
	from UFN_OR_GetAgencyMarketHotelSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, @FilterDeposit, default)
	-- Ventas Out Of Pending
	union all
	select Agency, Market, Hotel, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0
	from UFN_OR_GetAgencyMarketHotelSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, default, @FilterDeposit, default)
	-- Monto de ventas Out Of Pending
	union all
	select Agency, Market, Hotel, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0
	from UFN_OR_GetAgencyMarketHotelSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, default, @FilterDeposit, default)
	-- Ventas canceladas
	union all
	select Agency, Market, Hotel, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetAgencyMarketHotelSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, 1, @FilterDeposit, default)
	-- Monto de ventas canceladas
	union all
	select Agency, Market, Hotel, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetAgencyMarketHotelSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, 1, @FilterDeposit, default)
) as D
	left join Agencies A on A.agID = D.Agency
group by D.Agency, A.agN, D.Market, D.Hotel
order by D.Hotel, D.Market, SalesAmount_TOTAL desc, Shows desc, Books desc, D.Agency

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

