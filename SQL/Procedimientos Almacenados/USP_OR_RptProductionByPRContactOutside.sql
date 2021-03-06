if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptProductionByPRContactOutside]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptProductionByPRContactOutside]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por PR de contactos(Outside)
** 
** [lchairez]	10/Dic/2013 Created
** [erosado]	04/Mar/2016 Modified. Agregue los parametros @SalesRooms, @Countries, @Agencies y @Markets a la funcion UFN_OR_GetPRContacts
**
*/
CREATE procedure [dbo].[USP_OR_RptProductionByPRContactOutside]
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',		-- Claves de PRs
	@Program varchar(10) = 'ALL',	-- Clave de programa
	@FilterDeposit tinyint = 0		-- Filtro de depositos:
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
	-- Clave del PR
	D.PR,
	-- Nombre del PR
	IsNull(P.peN, 'CODE NOT FOUND!') as PRN,
	-- Estatus del PR
	IsNull(P.peps, 'INACTIVE') as Status,
	-- Contact
	Sum(D.Contacts) as Contacts,
	-- Bookings
	Sum(D.Books) as Books,
	-- Porcentaje de Books
	[dbo].UFN_OR_SecureDivision(Sum(D.Books), Sum(D.Contacts)) as BooksFactor,
	-- In & Outs
	Sum(D.InOuts) as InOuts,
	-- Bookings netos
	Sum(D.GrossBooks) as GrossBooks,
	-- Shows netos
	Sum(D.GrossShows) as GrossShows,
	-- Porcentaje de shows
	[dbo].UFN_OR_SecureDivision(Sum(D.GrossShows), Sum(D.GrossBooks)) as ShowsFactor,
	-- Directas
	Sum(D.Directs) as Directs,
	-- Walk Outs
	Sum(D.WalkOuts) as WalkOuts,
	-- Tours de cortesia
	Sum(D.CourtesyTours) as CourtesyTours,
	-- Tours de rescate
	Sum(D.SaveTours) as SaveTours,
	-- Shows
	Sum(D.Shows) as Shows,
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
	[dbo].UFN_OR_SecureDivision(Sum(D.Sales), Sum(D.Shows)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Shows)) as Efficiency,
	-- Venta promedio
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Sales)) as AverageSale
from (
	-- Contact
	select PR, Contacts , 0 Books, 0 as GrossBooks, 0 as InOuts, 0 as WalkOuts, 0 as CourtesyTours, 0 as SaveTours, 0 as Shows, 0 as GrossShows, 0 as Directs,
		0 as Sales, 0 as SalesAmount, 0 as SalesOOP, 0 as SalesAmountOOP, 0 as SalesPending, 0 as SalesAmountPending, 0 as SalesCancel,
		0 as SalesAmountCancel
	from UFN_OR_GetPRContacts(@DateFrom, @DateTo, @LeadSources, default, default, default, default, default, default)
	-- Bookings
	union all
	select PR, 0 Contacts, Books, 0 as GrossBooks, 0 as InOuts, 0 as WalkOuts, 0 as CourtesyTours, 0 as SaveTours, 0 as Shows, 0 as GrossShows, 0 as Directs,
		0 as Sales, 0 as SalesAmount, 0 as SalesOOP, 0 as SalesAmountOOP, 0 as SalesPending, 0 as SalesAmountPending, 0 as SalesCancel,
		0 as SalesAmountCancel
	from UFN_OR_GetPRContactBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, 0, default, default)
	-- Bookings netos
	union all
	select PR, 0 Contacts, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRContactBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, 0, 0, default)
	-- In & Outs
	union all
	select PR, 0 Contacts, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRContactShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, 1, default, default,
		default, default, default)
	-- Walk Outs
	union all
	select PR, 0 Contacts, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRContactShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, default, 1, default,
		default, default, default)
	-- Tours de cortesia
	union all
	select PR, 0 Contacts, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRContactShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, default, default, 2,
		default, default, default)
	-- Tours de rescate
	union all
	select PR, 0 Contacts, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRContactShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, default, default, 3,
		default, default, default)
	-- Shows
	union all
	select PR, 0 Contacts, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRContactShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, default, default,
		default, default, 1, default)
	-- Shows netos
	union all
	select PR, 0 Contacts, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRContactShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, 1, 0, default, default, default,
		default, default)
	-- Directas
	union all
	select PR, 0 Contacts, 0, 0, 0, 0, 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRContactBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, 1, default, default)
	-- Ventas procesables
	union all
	select PR, 0 Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRContactSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, @FilterDeposit, default,
		default)
	-- Monto de ventas procesables
	union all
	select PR, 0 Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRContactSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, @FilterDeposit, default,
		default)
	-- Ventas Out Of Pending
	union all
	select PR, 0 Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0
	from UFN_OR_GetPRContactSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, default, @FilterDeposit, default, default)
	-- Monto de ventas Out Of Pending
	union all
	select PR, 0 Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0
	from UFN_OR_GetPRContactSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, default, @FilterDeposit, default,
		default)
	-- Ventas pendientes
	union all
	select PR, 0 Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0
	from UFN_OR_GetPRContactSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, @FilterDeposit, default, 1)
	-- Monto de ventas pendientes
	union all
	select PR, 0 Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0
	from UFN_OR_GetPRContactSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, @FilterDeposit, default,
		1)
	-- Ventas canceladas
	union all
	select PR, 0 Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetPRContactSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, 1, @FilterDeposit, default, default)
	-- Monto de ventas canceladas
	union all
	select PR, 0 Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetPRContactSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, 1, @FilterDeposit, default,
		default)
) as D
	left join Personnel P on D.PR = P.peID
group by D.PR, P.peN, P.peps
order by Status, SalesAmount_TOTAL desc, Shows desc, Books desc, D.PR
