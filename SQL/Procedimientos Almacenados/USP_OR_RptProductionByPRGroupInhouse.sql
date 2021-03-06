if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptProductionByPRGroupInhouse]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptProductionByPRGroupInhouse]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de producción por PR y grupo (Inhouse)
** 
** [wtorres]	11/Ago/2010 Creado
** [wtorres]	18/Nov/2010 Agregué el parámetro @BasedOnArrival
**
*/
create procedure [dbo].[USP_OR_RptProductionByPRGroupInhouse]
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0,	-- Indica si se debe considerar quinielas
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada
as
set nocount on

-- Tabla de reporte
declare @Table table(
	PR varchar(10),
	Type varchar(7),
	Groups int,
	Integrants int,
	Assigns int,
	Contacts money,
	Availables money,
	GrossBooks money,
	Directs money,
	Books money,
	GrossShows money,
	Shows money,
	Sales money,
	SalesAmount money,
	SalesSelfGen money,
	SalesAmountSelfGen money
)

-- Tabla de PR (para ordenar)
declare @TablePR table(
	[Order] int identity (1, 1),
	PR varchar(10)
)

-- Datos
-- =============================================
select
	-- PR
	T.PR,
	-- Tipo
	case when T.[Group] is null then 'COUPLES' else 'GROUPS' end as Type,
	-- Integrantes
	(select Count(*) from GuestsGroupsIntegrants where gjgx = T.[Group]) as Integrants,
	Sum(T.Assigns) as Assigns,
	Sum(T.Contacts) as Contacts,
	Sum(T.Availables) as Availables,
	Sum(T.Books) as Books,
	Sum(T.GrossBooks) as GrossBooks,
	Sum(T.Directs) as Directs,
	Sum(T.Shows) as Shows,
	Sum(T.GrossShows) as GrossShows,
	Sum(T.Sales) as Sales,
	Sum(T.SalesAmount) as SalesAmount,
	Sum(T.SalesSelfGen) as SalesSelfGen,
	Sum(T.SalesAmountSelfGen) as SalesAmountSelfGen
into #tblData
from (
	-- Asignaciones
	select PR, [Group], Assigns, 0 as Contacts, 0 as Availables, 0 as Books, 0 as GrossBooks, 0 as Directs, 0 as Shows,
		0 as GrossShows, 0 as Sales, 0 as SalesAmount, 0 as SalesSelfGen, 0 as SalesAmountSelfGen
	from UFN_OR_GetPRGroupAssigns(@DateFrom, @DateTo, @LeadSources)
	-- Contactos
	union all
	select PR, [Group], 0, Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRGroupContacts(@DateFrom, @DateTo, @LeadSources, @BasedOnArrival)
	-- Disponibles
	union all
	select PR, [Group], 0, 0, Availables, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRGroupAvailables(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, @BasedOnArrival)
	-- Bookings
	union all
	select PR, [Group], 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRGroupBookings(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, default, @BasedOnArrival)
	-- Bookings netos
	union all
	select PR, [Group], 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRGroupBookings(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, 0, @BasedOnArrival)
	-- Directas
	union all
	select PR, [Group], 0, 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRGroupBookings(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, 1, @BasedOnArrival)
	-- Shows
	union all
	select PR, [Group], 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0
	from UFN_OR_GetPRGroupShows(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, default, @BasedOnArrival)
	-- Shows netos
	union all
	select PR, [Group], 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0
	from UFN_OR_GetPRGroupShows(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, 1, @BasedOnArrival)
	-- Ventas
	union all
	select PR, [Group], 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0
	from UFN_OR_GetPRGroupSales(@DateFrom, @DateTo, @LeadSources, default, @BasedOnArrival)
	-- Monto de ventas
	union all
	select PR, [Group], 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0
	from UFN_OR_GetPRGroupSalesAmount(@DateFrom, @DateTo, @LeadSources, default, @BasedOnArrival)
	-- Ventas Self Gen
	union all
	select PR, [Group], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetPRGroupSales(@DateFrom, @DateTo, @LeadSources, 1, @BasedOnArrival)
	-- Monto de ventas Self Gen
	union all
	select PR, [Group], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetPRGroupSalesAmount(@DateFrom, @DateTo, @LeadSources, 1, @BasedOnArrival)
) as T
group by T.PR, T.[Group]

-- Couples / Groups
-- =============================================
insert into @Table
select
	-- PR
	D.PR,
	-- Tipo
	D.Type,
	-- Grupos
	case when Type = 'COUPLES' then 0 else Count(*) end as Groups,
	-- Integrantes
	D.Integrants,
	-- Asignaciones
	Sum(D.Assigns) as Assigns,
	-- Contactos
	Sum(D.Contacts) as Contacts,
	-- Disponibles
	Sum(D.Availables) as Availables,
	-- Bookings netos
	Sum(D.GrossBooks) as GrossBooks,
	-- Directas
	Sum(D.Directs) as Directs,
	-- Bookings
	Sum(D.Books) as Books,
	-- Shows netos
	Sum(D.GrossShows) as GrossShows,
	-- Shows
	Sum(D.Shows) as Shows,
	-- Ventas
	Sum(D.Sales) as Sales,
	-- Monto de ventas
	Sum(D.SalesAmount) as SalesAmount,
	-- Ventas Self Gen
	Sum(D.SalesSelfGen) as SalesSelfGen,
	-- Monto de ventas SelfGen
	Sum(D.SalesAmountSelfGen) as SalesAmountSelfGen
from #tblData as D
group by D.PR, D.Type, D.Integrants

-- Totales
-- =============================================
union all
select
	-- PR
	D.PR,
	-- Tipo
	'TOTAL',
	-- Grupos
	Sum(case when Type = 'COUPLES' then 0 else 1 end) as Groups,
	-- Integrantes
	Sum(D.Integrants) as Integrants,
	-- Asignaciones
	Sum(D.Assigns) as Assigns,
	-- Contactos
	Sum(D.Contacts) as Contacts,
	-- Disponibles
	Sum(D.Availables) as Availables,
	-- Bookings netos
	Sum(D.GrossBooks) as GrossBooks,
	-- Directas
	Sum(D.Directs) as Directs,
	-- Bookings
	Sum(D.Books) as Books,
	-- Shows netos
	Sum(D.GrossShows) as GrossShows,
	-- Shows
	Sum(D.Shows) as Shows,
	-- Ventas
	Sum(D.Sales) as Sales,
	-- Monto de ventas
	Sum(D.SalesAmount) as SalesAmount,
	-- Ventas Self Gen
	Sum(D.SalesSelfGen) as SalesSelfGen,
	-- Monto de ventas SelfGen
	Sum(D.SalesAmountSelfGen) as SalesAmountSelfGen
from #tblData as D
group by D.PR

-- PR en orden
-- =============================================
insert into @TablePR (PR)
select PR
from @Table
where Type = 'TOTAL'
order by SalesAmount desc, Shows desc, Books desc, Availables desc, Contacts desc, Assigns desc

-- Reporte
-- =============================================
select
	-- PR
	D.PR,
	-- Nombre del PR
	P.peN as PRN,
	-- Tipo
	D.Type,
	-- Grupos
	Sum(D.Groups) as Groups,
	-- Integrantes
	(case when D.Type = 'TOTAL' then 0 else D.Integrants end) as Integrants,
	-- Total de integrantes
	(case when D.Type = 'TOTAL' then D.Integrants else (Sum(D.Groups) * D.Integrants) end) as TotalIntegrants,
	-- Asignaciones
	Sum(D.Assigns) as Assigns,
	-- Contactos
	Sum(D.Contacts) as Contacts,
	-- Porcentaje de contactación
	[dbo].UFN_OR_SecureDivision(Sum(D.Contacts), Sum(D.Assigns)) as ContactsFactor,
	-- Disponibles
	Sum(D.Availables) as Availables,
	-- Porcentaje de disponibles
	[dbo].UFN_OR_SecureDivision(Sum(D.Availables), Sum(D.Contacts)) as AvailablesFactor,
	-- Bookings netos
	Sum(D.GrossBooks) as GrossBooks,
	-- Directas
	Sum(D.Directs) as Directs,
	-- Bookings
	Sum(D.Books) as Books,
	-- Porcentaje de bookings
	[dbo].UFN_OR_SecureDivision(Sum(D.Books), Sum(D.Availables)) as BooksFactor,
	-- Shows netos
	Sum(D.GrossShows) as GrossShows,
	-- Shows
	Sum(D.Shows) as Shows,
	-- Porcentaje de shows
	[dbo].UFN_OR_SecureDivision(Sum(D.GrossShows), Sum(D.GrossBooks)) as ShowsFactor,
	-- Ventas
	Sum(D.Sales) as Sales_TOTAL,
	-- Monto de ventas
	Sum(D.SalesAmount) as SalesAmount_TOTAL,
	-- Ventas PR
	Sum(D.Sales - D.SalesSelfGen) as Sales_PR,
	-- Monto de ventas PR
	Sum(D.SalesAmount - D.SalesAmountSelfGen) as SalesAmount_PR,
	-- Ventas Self Gen
	Sum(D.SalesSelfGen) as Sales_SELFGEN,
	-- Monto de ventas SelfGen
	Sum(D.SalesAmountSelfGen) as SalesAmount_SELFGEN,
	-- Porcentaje de cierre
	[dbo].UFN_OR_SecureDivision(Sum(D.Sales), Sum(D.Shows)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Shows)) as Efficiency,
	-- Venta promedio
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Sales)) as AverageSale
from @Table as D
	inner join @TablePR T on D.PR = T.PR
	left join Personnel P on D.PR = P.peID
group by D.PR, D.Type, D.Integrants, P.peN, T.[Order]
order by T.[Order], D.Type

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

