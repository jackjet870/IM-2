USE [OrigosVCPalace]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByFlightSalesRoom]    Script Date: 09/10/2016 14:22:38 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptProductionByFlightSalesRoom]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptProductionByFlightSalesRoom]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByHotel]    Script Date: 09/10/2016 14:22:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptProductionByHotel]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptProductionByHotel]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByHotelGroup]    Script Date: 09/10/2016 14:22:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptProductionByHotelGroup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptProductionByHotelGroup]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByHotelGroupSalesRoom]    Script Date: 09/10/2016 14:22:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptProductionByHotelGroupSalesRoom]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptProductionByHotelGroupSalesRoom]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByHotelSalesRoom]    Script Date: 09/10/2016 14:22:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptProductionByHotelSalesRoom]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptProductionByHotelSalesRoom]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByWave]    Script Date: 09/10/2016 14:22:41 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptProductionByWave]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptProductionByWave]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByWaveSalesRoom]    Script Date: 09/10/2016 14:22:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptProductionByWaveSalesRoom]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptProductionByWaveSalesRoom]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptFoliosInvitationByDateFolio]    Script Date: 09/10/2016 14:22:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptFoliosInvitationByDateFolio]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptFoliosInvitationByDateFolio]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptPRStats]    Script Date: 09/10/2016 14:22:43 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptPRStats]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptPRStats]
GO

USE [OrigosVCPalace]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByFlightSalesRoom]    Script Date: 09/10/2016 14:22:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por vuelo y sala
** 
** [VKU] 13/May/2016 Creado
** [VKU] 17/May/2016 Modified. Agregue las columnas de ventas pendientes
**
*/
CREATE procedure [dbo].[USP_IM_RptProductionByFlightSalesRoom]
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
	-- Vuelo
	ISNULL(Cast(D.Flight as VARCHAR(13)), 'Not Specified') as FlightID,
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
	select Flight, SalesRoom, Books, 0 as GrossBooks, 0 as InOuts, 0 as Shows, 0 as Sales, 0 as SalesAmount, 0 as SalesOOP, 0 as SalesAmountOOP, 0 as SalesPending, 0 as SalesAmountPending,
		0 as SalesCancel, 0 as SalesAmountCancel
	from UFN_IM_GetFlightSalesRoomBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @FilterDepositAlternate, default)
	-- Bookings netos
	union all
	select Flight, SalesRoom, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_IM_GetFlightSalesRoomBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @FilterDepositAlternate, 0)
	-- In & Outs
	union all
	select Flight, SalesRoom, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_IM_GetFlightSalesRoomShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @FilterDeposit, 1)
	-- Shows
	union all
	select Flight, SalesRoom, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_IM_GetFlightSalesRoomShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @FilterDeposit, default)
	-- Ventas procesables
	union all
	select Flight, SalesRoom, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0, 0, 0
	from UFN_IM_GetFlightSalesRoomSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, @FilterDeposit, default)
	-- Monto de ventas procesables
	union all
	select Flight, SalesRoom, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0, 0, 0
	from UFN_IM_GetFlightSalesRoomSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, @FilterDeposit, default)
	-- Ventas Out Of Pending
	union all
	select Flight, SalesRoom, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0
	from UFN_IM_GetFlightSalesRoomSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, 1, default, @FilterDeposit, default)
	-- Monto de ventas Out Of Pending
	union all
	select Flight, SalesRoom, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0
	from UFN_IM_GetFlightSalesRoomSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, 1, default, @FilterDeposit, default)
	
	
	-- Ventas pendientes
	union all
	select Flight, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0
	from UFN_IM_GetFlightSalesRoomSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, @FilterDeposit, 1)
	-- Monto de ventas pendientes
	union all
	select Flight, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0
	from UFN_IM_GetFlightSalesRoomSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, @FilterDeposit, 1 )
	
	
	
	-- Ventas canceladas
	union all
	select Flight, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_IM_GetFlightSalesRoomSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, @FilterDeposit, default)
	-- Monto de ventas canceladas
	union all
	select Flight, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_IM_GetFlightSalesRoomSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, @FilterDeposit, default)
) as D
	left join SalesRooms S on D.SalesRoom = S.srID
group by D.Flight, D.SalesRoom, S.srN
order by D.SalesRoom, SalesAmount_TOTAL desc, Shows desc, Books desc, D.Flight










GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByHotel]    Script Date: 09/10/2016 14:22:47 ******/
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
CREATE procedure [dbo].[USP_IM_RptProductionByHotel]
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











GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByHotelGroup]    Script Date: 09/10/2016 14:22:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por grupo hotelero
** 
** [VKU] 14/May/2016 Creado
** [VKU] 18/May/2016 Modified. Agregue las columnas de ventas pendientes
**
*/
CREATE procedure [dbo].[USP_IM_RptProductionByHotelGroup]
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
	-- Grupo Hotelero
	HG.hgN as hoGroup,
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
	from UFN_IM_GetHotelSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, @FilterDeposit, default)
	-- Monto de ventas canceladas
	union all
	select Hotel, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_IM_GetHotelSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, @FilterDeposit, default)
) as D
	left join Hotels H on D.Hotel = H.hoID
	left join HotelGroups HG on HG.hgID = H.hoGroup
group by D.Hotel, HG.hgN
order by SalesAmount_TOTAL desc, Shows desc, Books desc, D.Hotel












GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByHotelGroupSalesRoom]    Script Date: 09/10/2016 14:22:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por grupo hotelero y sala
** 
** [VKU] 14/May/2016 Creado
** [VKU] 18/May/2016 Modified. Agregue las columnas de ventas pendientes
**
*/
CREATE procedure [dbo].[USP_IM_RptProductionByHotelGroupSalesRoom]
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
		-- Grupo Hotelero
	HG.hgN as hoGroup,
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
	select Hotel, SalesRoom, Books, 0 as GrossBooks, 0 as InOuts, 0 as Shows, 0 as Sales, 0 as SalesAmount, 0 as SalesOOP, 0 as SalesAmountOOP, 0 as SalesPending, 0 as SalesAmountPending,
		0 as SalesCancel, 0 as SalesAmountCancel
	from UFN_IM_GetHotelSalesRoomBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @FilterDepositAlternate, default)
	-- Bookings netos
	union all
	select Hotel, SalesRoom, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_IM_GetHotelSalesRoomBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @FilterDepositAlternate, 0)
	-- In & Outs
	union all
	select Hotel, SalesRoom, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_IM_GetHotelSalesRoomShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @FilterDeposit, 1)
	-- Shows
	union all
	select Hotel, SalesRoom, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_IM_GetHotelSalesRoomShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @FilterDeposit, default)
	-- Ventas procesables
	union all
	select Hotel, SalesRoom, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0, 0, 0
	from UFN_IM_GetHotelSalesRoomSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, @FilterDeposit, default)
	-- Monto de ventas procesables
	union all
	select Hotel, SalesRoom, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0, 0, 0
	from UFN_IM_GetHotelSalesRoomSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, @FilterDeposit, default)
	-- Ventas Out Of Pending
	union all
	select Hotel, SalesRoom, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0
	from UFN_IM_GetHotelSalesRoomSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, 1, default, @FilterDeposit, default)
	-- Monto de ventas Out Of Pending
	union all
	select Hotel, SalesRoom, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0
	from UFN_IM_GetHotelSalesRoomSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, 1, default, @FilterDeposit, default)
	
	
	-- Ventas canceladas
	union all
	select Hotel, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0
	from UFN_IM_GetHotelSalesRoomSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, @FilterDeposit, 1)
	-- Monto de ventas canceladas
	union all
	select Hotel, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0
	from UFN_IM_GetHotelSalesRoomSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, @FilterDeposit, 1)
	
	
	-- Ventas canceladas
	union all
	select Hotel, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_IM_GetHotelSalesRoomSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, @FilterDeposit, default)
	-- Monto de ventas canceladas
	union all
	select Hotel, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_IM_GetHotelSalesRoomSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, @FilterDeposit, default)
) as D
	left join Hotels H on D.Hotel = H.hoID
	left join HotelGroups HG on HG.hgID = H.hoGroup
	left join SalesRooms S on D.SalesRoom = S.srID
group by D.Hotel, HG.hgN, D.SalesRoom, S.srN
order by D.SalesRoom, SalesAmount_TOTAL desc, Shows desc, Books desc, D.Hotel













GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByHotelSalesRoom]    Script Date: 09/10/2016 14:22:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por hotel y sala
** 
** [VKU] 13/May/2016 Creado
** [VKU] 18/May/2016 Modified. Agregue las columnas de ventas pendientes
**
*/
CREATE procedure [dbo].[USP_IM_RptProductionByHotelSalesRoom]
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
	select Hotel, SalesRoom, Books, 0 as GrossBooks, 0 as InOuts, 0 as Shows, 0 as Sales, 0 as SalesAmount, 0 as SalesOOP, 0 as SalesAmountOOP, 0 as SalesPending, 0 as SalesAmountPending,
		0 as SalesCancel, 0 as SalesAmountCancel
	from UFN_IM_GetHotelSalesRoomBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @FilterDepositAlternate, default)
	-- Bookings netos
	union all
	select Hotel, SalesRoom, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_IM_GetHotelSalesRoomBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @FilterDepositAlternate, 0)
	-- In & Outs
	union all
	select Hotel, SalesRoom, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_IM_GetHotelSalesRoomShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @FilterDeposit, 1)
	-- Shows
	union all
	select Hotel, SalesRoom, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_IM_GetHotelSalesRoomShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @FilterDeposit, default)
	-- Ventas procesables
	union all
	select Hotel, SalesRoom, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0, 0, 0
	from UFN_IM_GetHotelSalesRoomSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, @FilterDeposit, default)
	-- Monto de ventas procesables
	union all
	select Hotel, SalesRoom, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0, 0, 0
	from UFN_IM_GetHotelSalesRoomSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, @FilterDeposit, default)
	-- Ventas Out Of Pending
	union all
	select Hotel, SalesRoom, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0
	from UFN_IM_GetHotelSalesRoomSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, 1, default, @FilterDeposit, default)
	-- Monto de ventas Out Of Pending
	union all
	select Hotel, SalesRoom, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0
	from UFN_IM_GetHotelSalesRoomSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, 1, default, @FilterDeposit, default)
	
	
	-- Ventas pendientes
	union all
	select Hotel, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0
	from UFN_IM_GetHotelSalesRoomSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, @FilterDeposit, 1)
	-- Monto de ventas pendientes
	union all
	select Hotel, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0
	from UFN_IM_GetHotelSalesRoomSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, @FilterDeposit, 1)
	
	
	-- Ventas canceladas
	union all
	select Hotel, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_IM_GetHotelSalesRoomSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, @FilterDeposit, default)
	-- Monto de ventas canceladas
	union all
	select Hotel, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_IM_GetHotelSalesRoomSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, @FilterDeposit, default)
) as D
	left join Hotels H on D.Hotel = H.hoID
	left join SalesRooms S on D.SalesRoom = S.srID
group by D.Hotel, D.SalesRoom, S.srN
order by D.SalesRoom, SalesAmount_TOTAL desc, Shows desc, Books desc, D.Hotel












GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByWave]    Script Date: 09/10/2016 14:22:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por horario
** 
** [VKU] 14/May/2016 Created
** [VKU] 18/May/2016 Modified. Agregue los campos de ventas pendientes
**
*/
CREATE procedure [dbo].[USP_IM_RptProductionByWave]
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
	-- BookTime
	ISNULL(CONVERT(varchar(50),D.BookTime, 120),'Not Specified') as BookTime,
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
	select BookTime, Books, 0 as GrossBooks, 0 as InOuts, 0 as Shows, 0 as Sales, 0 as SalesAmount, 0 as SalesOOP, 0 as SalesAmountOOP, 0 as SalesPending, 0 as SalesAmountPending,
		0 as SalesCancel, 0 as SalesAmountCancel
	from UFN_IM_GetWaveBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @FilterDepositAlternate, default)
	-- Bookings netos
	union all
	select BookTime, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_IM_GetWaveBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @FilterDepositAlternate, 0)
	-- In & Outs
	union all
	select BookTime, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_IM_GetWaveShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @FilterDeposit, 1)
	-- Shows
	union all
	select BookTime, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_IM_GetWaveShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @FilterDeposit, default)
	-- Ventas procesables
	union all
	select BookTime, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0, 0, 0
	from UFN_IM_GetWaveSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, @FilterDeposit, default)
	-- Monto de ventas procesables
	union all
	select BookTime, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0, 0, 0
	from UFN_IM_GetWaveSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, @FilterDeposit, default)
	-- Ventas Out Of Pending
	union all
	select BookTime, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0
	from UFN_IM_GetWaveSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, 1, default, @FilterDeposit, default)
	-- Monto de ventas Out Of Pending
	union all
	select BookTime, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0
	from UFN_IM_GetWaveSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, 1, default, @FilterDeposit, default)
	
	
	-- Ventas pendientes
	union all
	select BookTime, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0
	from UFN_IM_GetWaveSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, @FilterDeposit, 1)
	-- Monto de ventas pendientes
	union all
	select BookTime, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0
	from UFN_IM_GetWaveSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, @FilterDeposit, 1)
	
	
	-- Ventas canceladas
	union all
	select BookTime, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_IM_GetWaveSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, @FilterDeposit, default)
	-- Monto de ventas canceladas
	union all
	select BookTime, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_IM_GetWaveSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, @FilterDeposit, default)
) as D
group by D.BookTime
order by SalesAmount_TOTAL desc, Shows desc, Books desc, D.BookTime












GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByWaveSalesRoom]    Script Date: 09/10/2016 14:23:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por horario y sala
** 
** [VKU] 16/May/2016 Creado
** [VKU] 18/May/2016 Modified. Agregue las columnas de ventas pendientes
**
*/
CREATE procedure [dbo].[USP_IM_RptProductionByWaveSalesRoom]
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
	-- BookTime
	ISNULL(CONVERT(varchar(50),D.BookTime, 120),'Not Specified') as BookTime,
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
	select BookTime, SalesRoom, Books, 0 as GrossBooks, 0 as InOuts, 0 as Shows, 0 as Sales, 0 as SalesAmount, 0 as SalesOOP, 0 as SalesAmountOOP, 0 as SalesPending, 0 as SalesAmountPending,
		0 as SalesCancel, 0 as SalesAmountCancel
	from UFN_IM_GetWaveSalesRoomBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @FilterDepositAlternate, default)
	-- Bookings netos
	union all
	select BookTime, SalesRoom, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_IM_GetWaveSalesRoomBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @FilterDepositAlternate, 0)
	-- In & Outs
	union all
	select BookTime, SalesRoom, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_IM_GetWaveSalesRoomShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @FilterDeposit, 1)
	-- Shows
	union all
	select BookTime, SalesRoom, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_IM_GetWaveSalesRoomShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, @FilterDeposit, default)
	-- Ventas procesables
	union all
	select BookTime, SalesRoom, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0, 0, 0
	from UFN_IM_GetWaveSalesRoomSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, @FilterDeposit, default)
	-- Monto de ventas procesables
	union all
	select BookTime, SalesRoom, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0, 0, 0
	from UFN_IM_GetWaveSalesRoomSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, @FilterDeposit, default)
	-- Ventas Out Of Pending
	union all
	select BookTime, SalesRoom, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0
	from UFN_IM_GetWaveSalesRoomSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, 1, default, @FilterDeposit, default)
	-- Monto de ventas Out Of Pending
	union all
	select BookTime, SalesRoom, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0
	from UFN_IM_GetWaveSalesRoomSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, 1, default, @FilterDeposit, default)
	
	
	-- Ventas pendientes
	union all
	select BookTime, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0
	from UFN_IM_GetWaveSalesRoomSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, @FilterDeposit, 1)
	-- Monto de ventas pendientes
	union all
	select BookTime, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0
	from UFN_IM_GetWaveSalesRoomSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, @FilterDeposit, 1)
	
	
	-- Ventas canceladas
	union all
	select BookTime, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_IM_GetWaveSalesRoomSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, @FilterDeposit, default)
	-- Monto de ventas canceladas
	union all
	select BookTime, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_IM_GetWaveSalesRoomSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, @FilterDeposit, default)
) as D
	left join SalesRooms S on D.SalesRoom = S.srID
group by D.BookTime, D.SalesRoom, S.srN
order by D.SalesRoom, SalesAmount_TOTAL desc, Shows desc, Books desc, D.BookTime













GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptFoliosInvitationByDateFolio]    Script Date: 09/10/2016 14:23:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de Folios Invitations Outhouse)
**
** [lormartinez]	28/Ago/2014 Created
** [wtorres]		08/May/2015 Modified. Agregue la fecha de booking
** [lormartinez]	08/Ene/2015 Modified.Agregue los parametros @LeadSources y @PRs
**
*/
CREATE procedure [dbo].[USP_OR_RptFoliosInvitationByDateFolio]
	@DateFrom datetime = null,	-- Fecha desde
	@DateTo datetime = null,	-- Fecha hasta
	@Serie varchar(5) = 'ALL',	-- Serie
	@FolioFrom integer = 0,		-- Folio desde
	@FolioTo integer = 0,		-- Folio hasta,
	@LeadSources varchar(MAX) ='ALL', --Lista de LeadSources
	@PRs varchar(MAX) ='ALL' --Lista de PRs
as
set nocount on
 
select
	G.guOutInvitNum,
	G.guPRInvit1 as PR,
	P.peN as PRN, 
	G.guLastName1,
	G.guBookD,
	L.lsN
from Guests G
	left join LeadSources L on G.guls = L.lsID
	left join Personnel P on G.guPRInvit1 = P.peID
	outer apply (select Substring(G.guOutInvitNum, CharIndex('-', G.guOutInvitNum) + 1, Len(G.guOutInvitNum) - CharIndex('-', G.guOutInvitNum)) as Folio ) F
where
	-- Programa Outhouse
	L.lspg = 'OUT'
	-- Serie
	and (@Serie = 'ALL' or G.guOutInvitNum like @Serie + '-%')
	-- Rango de folios
	and ((@FolioFrom = 0 and @FolioTo = 0) 
		or (@FolioFrom = 0 and (@FolioTo > 0 and F.Folio <= @FolioTo))
		or (@FolioTo = 0 and  (@FolioFrom > 0 and F.Folio >= @FolioFrom))
		or (@FolioTo > 0 and @FolioFrom > 0 and (F.Folio between @FolioFrom and @FolioTo)  
	))
	-- Fecha de booking
	and(@DateFrom is null or G.guBookD Between @DateFrom and @DateTo)
	-- Lead Sources
	AND (@LeadSources = 'ALL' OR L.lsID IN (select item from split(@LeadSources, ',')))
	--PRs
	AND (@PRs ='ALL' OR P.peID IN (select item from split(@PRs, ',')))
order by G.guOutInvitNum
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptPRStats]    Script Date: 09/10/2016 14:23:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene las estadisticasdel modulo PRStats
** 
** [erosado]	01/Mar/2016 Created
** [lchairez]	18/Abr/2016 Modified. Agregué el parámetro @BasedOnPrLocation en la función UFN_OR_GetPRSales
** [lchairez]	18/Abr/2016 Modified. Agregué el parámetro @BasedOnPrLocation en la función UFN_OR_GetPRSalesAmount
** [erosado]	08/09/2016	Modified. Se agregaron los campos P_Status y T_Books y se adiciono campos de ordenamiento.
**
*/
CREATE procedure [dbo].[USP_OR_RptPRStats] 
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(max) = 'ALL',	-- Clave de los Lead Sources
	@SalesRooms varchar(max) = 'ALL',	-- Clave de las salas de ventas
	@Countries varchar(max) = 'ALL',	-- Clave de los paises
	@Agencies varchar(max) = 'ALL',		-- Clave de las agencias
	@Markets varchar(max) = 'ALL'		-- Clave de los mercados
as
set nocount on;

SELECT
	-- PR ID
	D.PR AS 'PR_ID',
	-- PR Name
	P.peN AS 'PR_NAME',
	-- PR Status
	P.peps AS 'P_Status',
	-- Assigns
	Sum(Assigns) AS Assign,
	-- Contacts
	SUM(D.Contacts) AS Conts,
	-- Contacts Factor (Contacts / Assigns)
	dbo.UFN_OR_SecureDivision(SUM(D.Contacts),SUM(D.Assigns)) AS 'C_Factor',
	-- Availables
	SUM(D.Availables)AS Avails,
	-- Availables Factor (Availables / Contacts)
	dbo.UFN_OR_SecureDivision(SUM(D.Availables), SUM(D.Contacts))  AS 'A_Factor',
	-- Bookings Netos (Sin Directas)
	SUM(D.GrossBooks) AS Bk,
	-- Bookings Factor (Books / Availables)
	dbo.UFN_OR_SecureDivision(SUM(D.Books), SUM(D.Availables)) AS 'Bk_Factor',
	-- Total Books
	SUM(D.Books) AS 'T_Books',
	-- Deposits (Bookings)
	SUM(D.Deposits) AS Dep,
	-- 	Directs (Bookings)
	SUM(D.Directs) AS Dir,
	-- Shows Netos (Shows WithOut Directs Without In & Outs)
	SUM(D.GrossShows) AS Sh,
	--	In & Outs (Shows)
	SUM(D.InOuts) AS 'IO',
	-- Shows Factor (Shows / Bookings Netos)
	dbo.UFN_OR_SecureDivision(SUM(D.Shows), SUM(D.GrossBooks)) AS 'Sh_Factor',
	-- Total Shows
	SUM(D.Shows) AS 'TSh',
	-- Self Gen Tours (Guests)
	SUM(D.SelfGenShows) AS SG,
	-- Processable Number
	SUM(D.ProcessableNumber) AS	'Proc_Number',	
	-- Processable Amount
	SUM(D.ProcessableAmount) - SUM(D.OutPendingAmount) AS Processable,
	-- Out Pending Number 
	SUM(D.OutPendingNumber) AS	'OutP_Number',
	-- Out Pending Amount 
	SUM(D.OutPendingAmount) AS	'Out_Pending',
	-- Cancelled Number 
	SUM(D.CancelledNumber) AS	'C_Number',
	-- Cancelled Amount 
	SUM(D.CancelledAmount) AS	'Cancelled',
	-- Total Number
	SUM(D.ProcessableNumber) AS 'Total_Number',
	-- Total Amount
	SUM(D.ProcessableAmount) AS Total,
	-- Proc PR Number
	SUM(D.ProcessableNumber) - SUM(D.SelfGenNumber) AS 'Proc_PR_Number',
	-- Proc PR Amount
	SUM(D.ProcessableAmount) - SUM(D.SelfGenAmount) AS 'Proc_PR',
	-- Proc SG Number(ConsidererSelfGen=1)	
	SUM(D.SelfGenNumber)AS 'Proc_SG_Number',
	-- Proc SG Amount (ConsidererSelfgen=1)
	SUM(D.SelfGenAmount)AS 'Proc_SG',
	-- Efficient
	dbo.UFN_OR_SecureDivision(SUM(D.ProcessableAmount),SUM(D.Shows)) AS Eff,
	-- Clossing Factor
	dbo.UFN_OR_SecureDivision(SUM(D.ProcessableNumber),SUM(D.Shows)) AS 'Cl_Factor',
	-- Canceladas Factor
	dbo.UFN_OR_SecureDivision(SUM(D.CancelledAmount),SUM(D.ProcessableAmount)) AS 'Ca_Factor',
	-- Avg Sale
	dbo.UFN_OR_SecureDivision(SUM(D.ProcessableAmount),SUM(D.ProcessableNumber)) AS 'Avg_Sale'
FROM(
	-- Asignaciones
	SELECT 
	PR							/*1*/
	--	PR Name Join Personnel
	,Assigns					/*2*/
	,0 AS Contacts				/*3*/
	-- Contacts Factor	
	,0 AS Availables			/*4*/
	-- Availables Factor
	,0 AS GrossBooks			/*5*/
	-- Bookings Factor
	,0 AS Books					/*6*/
	,0 AS Deposits				/*7*/
	-- Shows Factor
	,0 AS Directs				/*8*/
	,0 AS GrossShows			/*9*/
	,0 AS InOuts				/*10*/
	,0 AS Shows					/*11*/
	,0 AS SelfGenShows			/*12*/
	,0 AS ProcessableNumber		/*13*/
	,0 AS ProcessableAmount		/*14*/
	,0 AS OutPendingNumber		/*15*/
	,0 AS OutPendingAmount		/*16*/
	,0 AS CancelledNumber		/*17*/
	,0 AS CancelledAmount		/*18*/
	-- Total Number
	-- Total Amount 	
	,0 AS SelfGenNumber			/*19*/
	,0 AS SelfGenAmount			/*20*/
	-- Efficient Factor
	-- Closing Factor
	-- Cancelled Factor
	-- Avg Sales
	
	FROM dbo.UFN_OR_GetPRAssigns(@DateFrom, @DateTo, @LeadSources, @SalesRooms, @Countries, @Agencies, @Markets)
	-- Contacts
	UNION ALL
	SELECT PR,0,Contacts,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	FROM UFN_OR_GetPRContacts(@DateFrom, @DateTo, @LeadSources, DEFAULT, DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Availables
	UNION ALL
	SELECT PR,0,0,Availables,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	FROM UFN_OR_GetPRAvailables(@DateFrom, @DateTo, @LeadSources, DEFAULT, DEFAULT, DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Bookings Netos (Sin Directas)
	UNION ALL
	SELECT PR,0,0,0,Books,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	FROM UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, DEFAULT, DEFAULT, DEFAULT, DEFAULT,0
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Bookings
	UNION ALL
	SELECT PR,0,0,0,0,Books,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	FROM UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, DEFAULT, DEFAULT, DEFAULT, DEFAULT,DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Depositos
	UNION ALL
	SELECT PR,0,0,0,0,0,Books,0,0,0,0,0,0,0,0,0,0,0,0,0
	FROM UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, DEFAULT, DEFAULT, DEFAULT,1,DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Directos
	UNION ALL
	SELECT PR,0,0,0,0,0,0,Books,0,0,0,0,0,0,0,0,0,0,0,0
	FROM UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, DEFAULT, DEFAULT, DEFAULT,DEFAULT,1
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Shows Netos
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,Shows,0,0,0,0,0,0,0,0,0,0,0
	FROM dbo.UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,DEFAULT,DEFAULT,1,0
	,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- In & Outs
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,Shows,0,0,0,0,0,0,0,0,0,0
	FROM UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,1
	,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Shows
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,0,Shows,0,0,0,0,0,0,0,0,0
	FROM dbo.UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT, DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Self Gen Shows
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,0,0,Shows,0,0,0,0,0,0,0,0
	FROM dbo.UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT, DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,1,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Number Processable 
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,0,0,0,Sales,0,0,0,0,0,0,0
	FROM dbo.UFN_OR_GetPRSales(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT, DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)	
	-- Amount Processable 
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,0,0,0,0,SalesAmount,0,0,0,0,0,0
	FROM dbo.UFN_OR_GetPRSalesAmount(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT, DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)	
	-- Number Out Of Pending 
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,0,0,0,0,0,Sales,0,0,0,0,0
	FROM dbo.UFN_OR_GetPRSales(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,DEFAULT,1,DEFAULT,DEFAULT,DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)	
	-- Amount Out Of Pending	
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,0,0,0,0,0,0,SalesAmount,0,0,0,0
	FROM dbo.UFN_OR_GetPRSalesAmount(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,DEFAULT,1,DEFAULT,DEFAULT,DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Number Cancelled
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,Sales,0,0,0
	FROM dbo.UFN_OR_GetPRSales(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,DEFAULT,DEFAULT,1,DEFAULT,DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Amount Cancelled
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,SalesAmount,0,0
	FROM dbo.UFN_OR_GetPRSalesAmount(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,DEFAULT,DEFAULT,1,DEFAULT,DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Number Self Gen
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,Sales,0
	FROM dbo.UFN_OR_GetPRSales(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,1,DEFAULT,DEFAULT,DEFAULT,DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Amount Self Gen
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,SalesAmount
	FROM dbo.UFN_OR_GetPRSalesAmount(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,1,DEFAULT,DEFAULT,DEFAULT,DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	
)AS D
	LEFT JOIN Personnel P ON D.PR = P.peID
GROUP BY PR, P.peN, P.peps
ORDER BY P_Status ASC,Processable DESC, Eff DESC, TSh DESC, Bk DESC

GO


