if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptProductionByAgencyOutside]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptProductionByAgencyOutside]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por agencia (Outside)
** 
** [wtorres]	06/Mar/2010 Ahora se pasa la lista de Lead Sources como un solo parametro y se devuelven los datos agrupados
** [wtorres]	19/Oct/2011 Elimine el uso de la funcion UFN_OR_GetAgencyInOuts
** [wtorres]	23/Dic/2011 Agregue el campo de descripcion de agencia
** [alesanchez]	02/Sep/2013 Se agregaron campos de IO, WO, RT, CT , Save, T Tours y UPS
** [wtorres]	16/Dic/2013 Ahora solo devuelve los tipos de membresia que tenga el reporte
**
*/
create procedure [dbo].[USP_OR_RptProductionByAgencyOutside]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@FilterDeposit tinyint = 0,			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
										--		3. Con deposito y shows sin deposito (Deposits & Flyers Show)
	@SalesByMembershipType bit = 0	-- Indica si se desean las ventas por tipo de membresia
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
	-- Walk Outs
	Sum(D.WalkOuts) as WalkOuts,
	-- Tours regulares
	Sum(D.Tours) as Tours,
	-- Tours de cortesia
	Sum(D.CourtesyTours) as CourtesyTours,
	-- Tours de rescate
	Sum(D.SaveTours) as SaveTours,
	-- Total de tours
	Sum(D.Tours) + Sum(D.CourtesyTours) + Sum(D.SaveTours) + Sum(D.WalkOuts) as TotalTours,	
	-- Tours Regulares o Venta
	Sum(D.UPS)  as UPS,		
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
	Sum(D.SalesAmount + D.SalesAmountCancel) as Subtotal,
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
	select Agency, Books, 0 as GrossBooks, 0 as InOuts, 0 as Shows, 0 as WalkOuts, 0 as Tours, 0 as CourtesyTours, 0 as SaveTours, 0 as UPS, 0 as Sales, 0 as SalesAmount, 0 as SalesOOP, 0 as SalesAmountOOP,
		0 as SalesCancel, 0 as SalesAmountCancel
	from UFN_OR_GetAgencyBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, default, default)		
	-- Bookings netos
	union all
	select Agency, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetAgencyBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, default, 0)
	-- In & Outs
	union all
	select Agency, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetAgencyShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, 1, default, default, default)
	-- Shows
	union all
	select Agency, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetAgencyShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, 0, default, default, default)	
	-- Walk Outs
	union all
	select Agency, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetAgencyShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, 0, 1, default, default)																
	-- Tours regulares
	union all
	select Agency, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetAgencyShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, 0, 0, 1, default)	
	-- Tours de cortesia
	union all
	select Agency, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetAgencyShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, 0, 0, 2, default)
	-- Tours de rescate
	union all
	select Agency, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetAgencyShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, 0, 0, 3, default)	
	-- Total de shows (son todos los shows con tour o venta)
	union all
	select Agency, 0, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0 
	from UFN_OR_GetAgencyShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, 0, 0, default, 1)	
	-- Ventas procesables
	union all
	select Agency, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0
	from UFN_OR_GetAgencySales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, @FilterDeposit)
	-- Monto de ventas procesables
	union all
	select Agency, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0
	from UFN_OR_GetAgencySalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, @FilterDeposit)
	-- Ventas Out Of Pending
	union all
	select Agency, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0
	from UFN_OR_GetAgencySales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, 1, default, @FilterDeposit)
	-- Monto de ventas Out Of Pending
	union all
	select Agency, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0
	from UFN_OR_GetAgencySalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, 1, default, @FilterDeposit)
	-- Ventas canceladas
	union all
	select Agency, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetAgencySales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, @FilterDeposit)
	-- Monto de ventas canceladas
	union all
	select Agency, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetAgencySalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, @FilterDeposit)
) as D
	left join Agencies A on A.agID = D.Agency
group by D.Agency, A.agN
order by SalesAmount_TOTAL desc, Shows desc, Books desc, D.Agency

-- si se desean las ventas por tipo de membresia
if @SalesByMembershipType = 1 begin

	-- Ventas por tipo de membresia
	select
		-- Agencia
		IsNull(D.Agency, 'Not Specified') as Agency,
		-- Tipo de membresia
		D.MembershipType,
		-- Ventas
		Sum(D.Sales) as Sales,
		-- Monto de ventas
		Sum(D.SalesAmount) as SalesAmount
	into #SalesByMembershipType
	from (
		-- Ventas
		select Agency, MembershipType, Sales, 0 as SalesAmount
		from UFN_OR_GetAgencyMembershipTypeSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, @FilterDeposit)			
		-- Monto de ventas
		union all
		select Agency, MembershipType, 0, SalesAmount
		from UFN_OR_GetAgencyMembershipTypeSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, @FilterDeposit)
	) as D
	group by D.Agency,D.MembershipType
	order by D.Agency,D.MembershipType
	
	-- 2. Tipos de membresia
	select distinct M.mtID, M.mtN, M.mtLevel
	from MemberShipTypes M
		inner join #SalesByMembershipType S on S.MembershipType = M.mtID
	order by M.mtLevel

	-- 3. Ventas por tipo de membresia
	select * from #SalesByMembershipType
end
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

