if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptProductionByNationalityOutside]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptProductionByNationalityOutside]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por nacionalidad (Outside)
** 
** [wtorres]	27/Ago/2009 Ahora se pasa la lista de Lead Sources como un solo parametro
**					- Elimine los campos guID, guRef, guPRInfo, guInfo, guInfoD, guInvitD, sagu, saProc, saCancel, samt
**					- Si la nacionalidad no esta especificada devuelve Not Specified
**					- Agregue el campo srN
**					- Ya no se cuentan los contactos
** [wtorres]	23/Nov/2009 Ahora se devuelven los datos agrupados
** [wtorres]	19/Oct/2011 Elimine el uso de la funcion UFN_OR_GetNationalityInOuts
** [wtorres]	04/Jul/2013 Agregue el parametro @IncludeSaveCourtesyTours
** [wtorres]	25/Jul/2013 Renombre el parametro @IncludeSaveCourtesyTours por @FilterSaveCourtesyTours
**
*/
create procedure [dbo].[USP_OR_RptProductionByNationalityOutside]
	@DateFrom datetime,						-- Fecha desde
	@DateTo datetime,						-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',		-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',				-- Claves de PRs
	@Program varchar(10) = 'ALL',			-- Clave de programa
	@FilterDeposit tinyint = 0,				-- Filtro de depositos:
											--		0. Sin filtro
											--		1. Con deposito (Deposits)
											--		2. Sin deposito (Flyers)
											--		3. Con deposito y shows sin deposito (Deposits & Flyers Show)
	@FilterSaveCourtesyTours tinyint = 0	-- Filtro de tours de rescate y cortesia
											--		0. Sin filtro
											--		1. Excluir tours de rescate y cortesia
											--		2. Excluir tours de rescate y cortesia sin venta
as
set nocount on

declare	@FilterDepositAlternate tinyint	-- Filtro de depositos alterno (Aplica para bookings)

set @FilterDepositAlternate = @FilterDeposit
-- si se desea el reporte con deposito y shows sin deposito (Deposits & Flyers Show)
if @FilterDeposit = 3
	set @FilterDeposit = 0	-- Sin filtro

select
	-- Nacionalidad
	IsNull(D.Nationality, 'Not specified') as Nationality,
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
	select Nationality, Books, 0 as GrossBooks, 0 as InOuts, 0 as Shows, 0 as Sales, 0 as SalesAmount, 0 as SalesOOP, 0 as SalesAmountOOP,
		0 as SalesCancel, 0 as SalesAmountCancel
	from UFN_OR_GetNationalityBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, default, default, default)
	-- Bookings netos
	union all
	select Nationality, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetNationalityBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, default, 0, default)
	-- In & Outs
	union all
	select Nationality, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetNationalityShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, 1, default,
		@FilterSaveCourtesyTours, default)
	-- Shows
	union all
	select Nationality, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetNationalityShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, 0, default,
		@FilterSaveCourtesyTours, default)
	-- Ventas procesables
	union all
	select Nationality, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0
	from UFN_OR_GetNationalitySales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, @FilterDeposit,
		@FilterSaveCourtesyTours, default)
	-- Monto de ventas procesables
	union all
	select Nationality, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0
	from UFN_OR_GetNationalitySalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, @FilterDeposit,
		@FilterSaveCourtesyTours, default)
	-- Ventas Out Of Pending
	union all
	select Nationality, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0
	from UFN_OR_GetNationalitySales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, default, @FilterDeposit,
		@FilterSaveCourtesyTours, default)
	-- Monto de ventas Out Of Pending
	union all
	select Nationality, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0
	from UFN_OR_GetNationalitySalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, default, @FilterDeposit,
		@FilterSaveCourtesyTours, default)
	-- Ventas canceladas
	union all
	select Nationality, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetNationalitySales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, 1, @FilterDeposit,
		@FilterSaveCourtesyTours, default)
	-- Monto de ventas canceladas
	union all
	select Nationality, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetNationalitySalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, 1, @FilterDeposit,
		@FilterSaveCourtesyTours, default)
) as D
group by D.Nationality
order by SalesAmount_TOTAL desc, Shows desc, Books desc, D.Nationality

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

