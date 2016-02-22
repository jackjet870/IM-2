if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptProductionByPROutside]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptProductionByPROutside]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por PR (Outside)
** 
** [wtorres]		27/Oct/2009 Modified. Ahora se pasa la lista de Lead Sources como un solo parametro y devuelve los datos agrupados
** [wtorres]		02/Ene/2010 Modified. No se estaban contando bien los bookings netos ni los shows netos
** [wtorres]		16/Abr/2010 Modified. Agregue los parametros @PRs y @Program
** [wtorres]		19/Oct/2011 Modified. Elimine el uso de la funcion UFN_OR_GetPRInOuts
** [wtorres]		01/Nov/2011 Modified. Agregue los campos de Walk Outs, Courtesy Tours y Save Tours
** [wtorres]		16/Nov/2011 Modified. Ahora el numero total de shows solo cuenta los shows con tour o venta y agregue las columnas de ventas pendientes
** [lormartinez]	22/Sep/2014 Modified. Se agrega parametro para filtro basado en fecha de booking
** [lormartinez]  15/Dic/2015 Modified. Se agrega paranetro default en las funciones de 
                  GetBooking y GetShows
**
*/
create procedure [dbo].[USP_OR_RptProductionByPROutside]
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',		-- Claves de PRs
	@Program varchar(10) = 'ALL',	-- Clave de programa
	@FilterDeposit tinyint = 0,		-- Filtro de depositos:
									--		0. Sin filtro
									--		1. Con deposito (Deposits)
									--		2. Sin deposito (Flyers)
									--		3. Con deposito y shows sin deposito (Deposits & Flyers Show)
	@BasedOnBooking bit = 0			-- Indica si se debe basar en la fecha de booking
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
	-- Bookings
	Sum(D.Books) as Books,
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
	-- Bookings
	select PR, Books, 0 as GrossBooks, 0 as InOuts, 0 as WalkOuts, 0 as CourtesyTours, 0 as SaveTours, 0 as Shows, 0 as GrossShows, 0 as Directs,
		0 as Sales, 0 as SalesAmount, 0 as SalesOOP, 0 as SalesAmountOOP, 0 as SalesPending, 0 as SalesAmountPending, 0 as SalesCancel,
		0 as SalesAmountCancel
	from UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, 0, default, default,default)
	-- Bookings netos
	union all
	select PR, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, 0, 0, default,default)
	-- In & Outs
	union all
	select PR, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, 1, default, default, default, default,
		default, @BasedOnBooking,default)
	-- Walk Outs
	union all
	select PR, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, default, 1, default, default, default,
		default, @BasedOnBooking,default)
	-- Tours de cortesia
	union all
	select PR, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, default, default, 2, default, default,
		default, @BasedOnBooking,default)
	-- Tours de rescate
	union all
	select PR, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, default, default, 3, default, default,
		default,@BasedOnBooking,default)
	-- Shows
	union all
	select PR, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, default, default, default, default, 1,
		default, @BasedOnBooking,default)
	-- Shows netos
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, 1, 0, default, default, default, default,
		default, @BasedOnBooking,default)
	-- Directas
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, 1, default, default,default)
	-- Ventas procesables
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, @FilterDeposit, default, default, @BasedOnBooking)
	-- Monto de ventas procesables
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, @FilterDeposit, default, default, @BasedOnBooking)
	-- Ventas Out Of Pending
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, default, @FilterDeposit, default, default, @BasedOnBooking)
	-- Monto de ventas Out Of Pending
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0
	from UFN_OR_GetPRSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, default, @FilterDeposit, default, default, @BasedOnBooking)
	-- Ventas pendientes
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0
	from UFN_OR_GetPRSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, @FilterDeposit, 1, default, @BasedOnBooking)
	-- Monto de ventas pendientes
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0
	from UFN_OR_GetPRSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, @FilterDeposit, 1, default, @BasedOnBooking)
	-- Ventas canceladas
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetPRSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, 1, @FilterDeposit, default, default, @BasedOnBooking)
	-- Monto de ventas canceladas
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetPRSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, 1, @FilterDeposit, default, default, @BasedOnBooking)
) as D
	left join Personnel P on D.PR = P.peID
group by D.PR, P.peN, P.peps
order by Status, SalesAmount_TOTAL desc, Shows desc, Books desc, D.PR

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

