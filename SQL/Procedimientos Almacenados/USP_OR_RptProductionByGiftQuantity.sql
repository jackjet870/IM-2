if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptProductionByGiftQuantity]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptProductionByGiftQuantity]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por regalo y cantidad
** 
** [wtorres]	26/Nov/2009 Creado
** [wtorres]	27/Abr/2010 Ahora el numero de shows netos son todos los shows menos las directas no In & Outs
** [wtorres]	17/Nov/2010 Agregue el parametro @BasedOnArrival
** [wtorres]	19/Oct/2011 Elimine el uso de la funcion UFN_OR_GetGiftQuantityInOuts
**
*/
create procedure [dbo].[USP_OR_RptProductionByGiftQuantity]
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@GiftsQuantitys varchar(8000),	-- Lista de cantidades y regalos. Por ejemplo: 2-GORRAS,5-PLAYERAS,3-BATAS
	@ConsiderQuinellas bit = 0,		-- Indica si se debe considerar quinielas
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
as
SET NOCOUNT ON

select
	-- Clave del regalo
	D.Gift as GiftID,
	-- Descripcion del regalo
	G.giN as GiftN,
	-- Clave del PR
	D.PR as PRID,
	-- Nombre del PR
	IsNull(P.peN, 'CODE NOT FOUND!') as PRN,
	-- Cantidad
	Sum(D.Quantity) as Quantity,
	-- Monto
	Sum(D.Amount) as Amount,
	-- Bookings netos
	Sum(D.GrossBooks) as GrossBooks,
	-- Directas
	Sum(D.Directs) as Directs,
	-- Bookings
	Sum(D.Books) as Books,
	-- Depositos
	Sum(D.Deposits) as Deposits,
	-- Shows netos
	Sum(D.GrossShows) as GrossShows,
	-- Porcentaje de shows
	[dbo].UFN_OR_SecureDivision(Sum(D.GrossShows), Sum(D.GrossBooks)) as ShowsFactor,
	-- In & Outs
	Sum(D.InOuts) as InOuts,
	-- Shows
	Sum(D.Shows) as Shows,
	-- Self Gens
	Sum(D.SelfGens) as SelfGens,
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
from (
	-- Cantidades y montos 
	select Gift, PR, Quantity, Amount, 0 as Directs, 0 as Books, 0 as GrossBooks, 0 as Deposits, 0 as Shows, 0 as GrossShows, 0 as InOuts,
		0 as SelfGens, 0 as Sales, 0 as SalesAmount, 0 as SalesSelfGen, 0 as SalesAmountSelfGen
	from UFN_OR_GetGiftQuantityQuantitysAmount(@DateFrom, @DateTo, @LeadSources, @GiftsQuantitys)
	-- Directas
	union all
	select Gift, PR, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetGiftQuantityBookings(@DateFrom, @DateTo, @LeadSources, @GiftsQuantitys, @ConsiderQuinellas, 1, @BasedOnArrival)
	-- Bookings
	union all
	select Gift, PR, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetGiftQuantityBookings(@DateFrom, @DateTo, @LeadSources, @GiftsQuantitys, @ConsiderQuinellas, default, @BasedOnArrival)
	-- Bookings netos
	union all
	select Gift, PR, 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetGiftQuantityBookings(@DateFrom, @DateTo, @LeadSources, @GiftsQuantitys, @ConsiderQuinellas, 0, @BasedOnArrival)
	-- Depositos
	union all
	select Gift, PR, 0, 0, 0, 0, 0, Deposits, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetGiftQuantityDeposits(@DateFrom, @DateTo, @LeadSources, @GiftsQuantitys, @ConsiderQuinellas, @BasedOnArrival)
	-- Shows
	union all
	select Gift, PR, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetGiftQuantityShows(@DateFrom, @DateTo, @LeadSources, @GiftsQuantitys, @ConsiderQuinellas, default, default, @BasedOnArrival)
	-- Shows netos
	union all
	select Gift, PR, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetGiftQuantityShows(@DateFrom, @DateTo, @LeadSources, @GiftsQuantitys, @ConsiderQuinellas, default, 1, @BasedOnArrival)
	-- In & Outs
	union all
	select Gift, PR, 0, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0
	from UFN_OR_GetGiftQuantityShows(@DateFrom, @DateTo, @LeadSources, @GiftsQuantitys, @ConsiderQuinellas, 1, default, @BasedOnArrival)
	-- Self Gens
	union all
	select Gift, PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, SelfGens, 0, 0, 0, 0
	from UFN_OR_GetGiftQuantitySelfGens(@DateFrom, @DateTo, @LeadSources, @GiftsQuantitys, @ConsiderQuinellas, @BasedOnArrival)
	-- Ventas
	union all
	select Gift, PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0
	from UFN_OR_GetGiftQuantitySales(@DateFrom, @DateTo, @LeadSources, @GiftsQuantitys, default, default, default, @BasedOnArrival)
	-- Monto de ventas
	union all
	select Gift, PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0
	from UFN_OR_GetGiftQuantitySalesAmount(@DateFrom, @DateTo, @LeadSources, @GiftsQuantitys, default, default, default, @BasedOnArrival)
	-- Ventas Self Gen
	union all
	select Gift, PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetGiftQuantitySales(@DateFrom, @DateTo, @LeadSources, @GiftsQuantitys, 1, default, default, @BasedOnArrival)
	-- Monto de ventas Self Gen
	union all
	select Gift, PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetGiftQuantitySalesAmount(@DateFrom, @DateTo, @LeadSources, @GiftsQuantitys, 1, default, default, @BasedOnArrival)
) as D
	left join Gifts G on D.Gift = G.giID
	left join Personnel P on D.PR = P.peID
group by D.Gift, G.giN, D.PR, P.peN
order by D.Gift, SalesAmount_TOTAL desc, Efficiency desc, Shows desc, Books desc, D.PR

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

