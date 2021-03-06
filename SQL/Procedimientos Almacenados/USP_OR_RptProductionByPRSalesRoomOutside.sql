if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptProductionByPRSalesRoomOutside]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptProductionByPRSalesRoomOutside]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por PR y sala (Outhouse)
** 
** [wtorres]		30/Oct/2009 Created
** [wtorres]		01/Abr/2010 Modified. No se estaban contando bien las reservaciones netas ni los shows netos
** [wtorres]		16/Abr/2010 Modified. Agregue los parametros @PRs y @Program
** [wtorres]		19/Oct/2011 Modified. Elimine el uso de la funcion UFN_OR_GetPRSalesRoomInOuts
** [wtorres]		16/Nov/2011 Modified. Agregue los campos de Walk Outs, Courtesy Tours y Save Tours.
**								Ahora el numero total de shows solo cuenta los shows con tour o venta y agregue las columnas de ventas pendientes
** [axperez]		03/Dic/2013 Modified. Paso valor defualt para parametro @BasedOnArrival de la funcion UFN_OR_GetPRSalesRoomBookings
** [axperez]		04/Dic/2013 Modified. Paso valor defualt para parametros @ConsiderDirectsAntesInOut, @BasedOnArrival de la funcion UFN_OR_GetPRSalesRoomShows
** [lormartinez]	22/Sep/2014 Modified. Se agrega parametro para filtro basado en fecha de booking
** [wtorres]		28/Jul/2016 Modified. Elimine la funcion UFN_OR_GetPRSalesRoomDirects
**
*/
create procedure [dbo].[USP_OR_RptProductionByPRSalesRoomOutside]
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
	-- Clave de la sala
	D.SalesRoom as SalesRoomID,
	-- Descripcion de la sala
	S.srN as SalesRoomN,
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
	select PR, SalesRoom, Books, 0 as GrossBooks, 0 as InOuts, 0 as WalkOuts, 0 as CourtesyTours, 0 as SaveTours, 0 as Shows, 0 as GrossShows,
		0 as Directs, 0 as Sales, 0 as SalesAmount, 0 as SalesOOP, 0 as SalesAmountOOP, 0 as SalesPending, 0 as SalesAmountPending,
		0 as SalesCancel, 0 as SalesAmountCancel
	from UFN_OR_GetPRSalesRoomBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, 0, default, default)
	-- Bookings netos
	union all
	select PR, SalesRoom, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, 0, 0, default)
	-- In & Outs
	union all
	select PR, SalesRoom, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, 1, default, default, default, 
		default, default, @BasedOnBooking)
	-- Walk Outs
	union all
	select PR, SalesRoom, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, default, 1, default, default, 
		default, default, @BasedOnBooking)
	-- Tours de cortesia
	union all
	select PR, SalesRoom, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, default, default, 2, default, 
		default, default, @BasedOnBooking)
	-- Tours de rescate
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, default, default, 3, default, 
		default, default, @BasedOnBooking)
	-- Shows
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, default, default, default, default, 
		1, default, @BasedOnBooking)
	-- Shows netos
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, 1, 0, default, default, default, 
		default, default, @BasedOnBooking)
	-- Directas
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, 1, default, default)
	-- Ventas procesables
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, @FilterDeposit, default, default, @BasedOnBooking )
	-- Monto de ventas procesables
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, @FilterDeposit, default, 
		default, @BasedOnBooking)
	-- Ventas Out Of Pending
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, default, @FilterDeposit, default, default, @BasedOnBooking )
	-- Monto de ventas Out Of Pending
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, default, @FilterDeposit, default, 
		default, @BasedOnBooking)
	-- Ventas pendientes
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0
	from UFN_OR_GetPRSalesRoomSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, @FilterDeposit, 1, default, @BasedOnBooking)
	-- Monto de pendientes
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0
	from UFN_OR_GetPRSalesRoomSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, @FilterDeposit, 1, 
		default, @BasedOnBooking)
	-- Ventas canceladas
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetPRSalesRoomSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, 1, @FilterDeposit, default, default, @BasedOnBooking )
	-- Monto de ventas canceladas
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetPRSalesRoomSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, 1, @FilterDeposit, default, 
		default, @BasedOnBooking)
) as D
	left join Personnel P on D.PR = P.peID
	left join SalesRooms S on D.SalesRoom = S.srID
group by D.PR, P.peN, P.peps, D.SalesRoom, S.srN
order by D.SalesRoom, Status, SalesAmount_TOTAL desc, Shows desc, Books desc, D.PR

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


