if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptProductionByCoupleTypeSalesRoomOutside]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptProductionByCoupleTypeSalesRoomOutside]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por tipo de pareja y sala (Outside)
** 
** [wtorres]	26/Ago/2010 Creado
** [wtorres]	19/Oct/2011 Elimine el uso de la funcion UFN_OR_GetCoupleTypeSalesRoomInOuts
** [wtorres]	17/Ene/2014 Agregue los tipos de pareja: Couple with quinella y Quinella (Additional guests)
**
*/
create procedure [dbo].[USP_OR_RptProductionByCoupleTypeSalesRoomOutside]
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
	-- Tipo de pareja
	(case D.CoupleType
		when 1 then 'COUPLE'
		when 2 then 'FAMILY'
		when 3 then 'COUPLE WITH QUINELLA'
		when 4 then 'QUINELLA (ADDITIONAL GUESTS)'
		end) as CoupleType,
	-- Sala de ventas
	D.SalesRoom as SalesRoomID,
	S.srN as SalesRoomN,
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
	select IsNull(CoupleType, 0) as CoupleType, SalesRoom, Books, 0 as GrossBooks, 0 as InOuts, 0 as Shows, 0 as Sales, 0 as SalesAmount, 0 as SalesOOP,
		0 as SalesAmountOOP, 0 as SalesCancel, 0 as SalesAmountCancel
	from UFN_OR_GetCoupleTypeSalesRoomBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @FilterDepositAlternate, default)
	-- Bookings netos
	union all
	select IsNull(CoupleType, 0), SalesRoom, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetCoupleTypeSalesRoomBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @FilterDepositAlternate, 0)
	-- In & Outs
	union all
	select IsNull(CoupleType, 0), SalesRoom, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetCoupleTypeSalesRoomShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @FilterDeposit, 1)
	-- Shows
	union all
	select IsNull(CoupleType, 0), SalesRoom, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetCoupleTypeSalesRoomShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @FilterDeposit, 0)
	-- Ventas procesables
	union all
	select IsNull(CoupleType, 0), SalesRoom, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0
	from UFN_OR_GetCoupleTypeSalesRoomSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, @FilterDeposit)
	-- Monto de ventas procesables
	union all
	select IsNull(CoupleType, 0), SalesRoom, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0
	from UFN_OR_GetCoupleTypeSalesRoomSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, @FilterDeposit)
	-- Ventas Out Of Pending
	union all
	select IsNull(CoupleType, 0), SalesRoom, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0
	from UFN_OR_GetCoupleTypeSalesRoomSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, 1, default, @FilterDeposit)
	-- Monto de ventas Out Of Pending
	union all
	select IsNull(CoupleType, 0), SalesRoom, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0
	from UFN_OR_GetCoupleTypeSalesRoomSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, 1, default, @FilterDeposit)
	-- Ventas canceladas
	union all
	select IsNull(CoupleType, 0), SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetCoupleTypeSalesRoomSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, @FilterDeposit)
	-- Monto de ventas canceladas
	union all
	select IsNull(CoupleType, 0), SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetCoupleTypeSalesRoomSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, @FilterDeposit)
) as D
	left join SalesRooms S on D.SalesRoom = S.srID
group by D.CoupleType, D.SalesRoom, S.srN
order by D.SalesRoom, SalesAmount_TOTAL desc, Shows desc, Books desc, D.CoupleType

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

