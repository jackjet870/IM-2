USE [OrigosVCPalace]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptCloserStatistics]    Script Date: 09/21/2016 16:58:58 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptCloserStatistics]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptCloserStatistics]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptDailyGiftSimple]    Script Date: 09/21/2016 16:58:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptDailyGiftSimple]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptDailyGiftSimple]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptGiftsKardex]    Script Date: 09/21/2016 16:58:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptGiftsKardex]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptGiftsKardex]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptLinerStatistics]    Script Date: 09/21/2016 16:59:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptLinerStatistics]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptLinerStatistics]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptLoginLog]    Script Date: 09/21/2016 16:59:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptLoginLog]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptLoginLog]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptManifestByLSRange]    Script Date: 09/21/2016 16:59:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptManifestByLSRange]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptManifestByLSRange]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptManifestRange]    Script Date: 09/21/2016 16:59:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptManifestRange]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptManifestRange]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByFlightSalesRoom]    Script Date: 09/21/2016 16:59:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptProductionByFlightSalesRoom]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptProductionByFlightSalesRoom]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByHotel]    Script Date: 09/21/2016 16:59:03 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptProductionByHotel]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptProductionByHotel]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByHotelGroup]    Script Date: 09/21/2016 16:59:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptProductionByHotelGroup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptProductionByHotelGroup]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByHotelGroupSalesRoom]    Script Date: 09/21/2016 16:59:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptProductionByHotelGroupSalesRoom]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptProductionByHotelGroupSalesRoom]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByHotelSalesRoom]    Script Date: 09/21/2016 16:59:05 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptProductionByHotelSalesRoom]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptProductionByHotelSalesRoom]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByWave]    Script Date: 09/21/2016 16:59:05 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptProductionByWave]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptProductionByWave]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByWaveSalesRoom]    Script Date: 09/21/2016 16:59:06 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptProductionByWaveSalesRoom]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptProductionByWaveSalesRoom]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptWeeklyGiftsItemsSimple]    Script Date: 09/21/2016 16:59:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptWeeklyGiftsItemsSimple]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptWeeklyGiftsItemsSimple]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptWeeklyMonthlyHostess]    Script Date: 09/21/2016 16:59:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptWeeklyMonthlyHostess]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptWeeklyMonthlyHostess]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetCxC]    Script Date: 09/21/2016 16:59:08 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_GetCxC]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_GetCxC]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptCxC]    Script Date: 09/21/2016 16:59:08 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptCxC]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptCxC]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptCxCExcel]    Script Date: 09/21/2016 16:59:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptCxCExcel]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptCxCExcel]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptFoliosInvitationByDateFolio]    Script Date: 09/21/2016 16:59:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptFoliosInvitationByDateFolio]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptFoliosInvitationByDateFolio]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptGifts]    Script Date: 09/21/2016 16:59:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptGifts]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptGifts]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptProductionReferral]    Script Date: 09/21/2016 16:59:11 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptProductionReferral]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptProductionReferral]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptPRStats]    Script Date: 09/21/2016 16:59:11 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptPRStats]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptPRStats]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_SaveGiftsReceiptLog]    Script Date: 09/21/2016 16:59:12 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_SaveGiftsReceiptLog]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_SaveGiftsReceiptLog]
GO

USE [OrigosVCPalace]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptCloserStatistics]    Script Date: 09/21/2016 16:59:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de estadisticas de Closers
** 
** [edgrodriguez]	05/May/2016 Created
**
*/
CREATE PROCEDURE [dbo].[USP_IM_RptCloserStatistics]
	@DateFrom as DateTime,			-- Fecha desde
	@DateTo as DateTime,			-- Fecha hasta
	@SalesRooms as varchar(8000)	-- Claves de las salas de ventas
as

set nocount on

SELECT 
	saCloser,
	sum(Shows) Shows,	
	(sum(TotalProcSales)-sum(OOPSales)+sum(CancelSales)) ProcSales,
	(sum(TotalProcAmount)-sum(OOPAmount)+ sum(CancelAmount)) ProcAmount,
	sum(OOPSales) OOPSales,
	Sum(OOPAmount) OOPAmount,
	Sum(PendSales) PendSales,
	sum(PendAmount) PendAmount,
	sum(CancelSales) CancelSales,
	sum(CancelAmount) CancelAmount,
	sum(TotalProcAmount) TotalProcAmount,
	sum(TotalProcSales) TotalProcSales
into #closerStats 
FROM
(
--Shows
select Closer saCloser,sum(Shows) Shows,0 ProcSales,
0 ProcAmount,0 OOPSales,0 OOPAmount,0 PendSales, 
0 PendAmount,0 CancelSales,0 CancelAmount,
0 TotalProcAmount,0 TotalProcSales from dbo.UFN_IM_getCloserShows(@DateFrom,@DateTo,@SalesRooms) GROUP by closer
UNION ALL
--OOPAmount
select Closer saCloser,0 Shows,0 ProcSales,
0 ProcAmount,0 OOPSales,Sum(SalesAmount) OOPAmount,0 PendSales, 
0 PendAmount,0 CancelSales,0 CancelAmount,
0 TotalProcAmount,0 TotalProcSales from dbo.UFN_IM_GetCloserSalesAmount(@DateFrom,@DateTo,@SalesRooms,1,default,default,default,default,default) group by Closer
UNION ALL
--PendAmount
select Closer saCloser,0 Shows,0 ProcSales,
0 ProcAmount,0 OOPSales,0 OOPAmount,0 PendSales, 
Sum(SalesAmount) PendAmount,0 CancelSales,0 CancelAmount,
0 TotalProcAmount,0 TotalProcSales from dbo.UFN_IM_GetCloserSalesAmount(@DateFrom,@DateTo,@SalesRooms,default,default,1,default,default,default) group by Closer
UNION ALL
--CancelAmount
select Closer saCloser,0 Shows,0 ProcSales,
0 ProcAmount,0 OOPSales,0 OOPAmount,0 PendSales, 
0 PendAmount,0 CancelSales,Sum(SalesAmount) CancelAmount,
0 TotalProcAmount,0 TotalProcSales from dbo.UFN_IM_GetCloserSalesAmount(@DateFrom,@DateTo,@SalesRooms,default,1,default,default,default,default) group by Closer
UNION ALL
--TotalProcAmount
select Closer saCloser,0 Shows,0 ProcSales,
0 ProcAmount,0 OOPSales,0 OOPAmount,0 PendSales, 
0 PendAmount,0 CancelSales,0 CancelAmount,
 Sum(SalesAmount) TotalProcAmount,0 TotalProcSales from dbo.UFN_IM_GetCloserSalesAmount(@DateFrom,@DateTo,@SalesRooms,default,default,default,default,default,default) group by Closer
--OOPSales
UNION ALL
select Closer saCloser,0 Shows,0 ProcSales,
0 ProcAmount,SUM(Sales) OOPSales,0 OOPAmount,0 PendSales, 
0 PendAmount,0 CancelSales,0 CancelAmount,
0 TotalProcAmount,0 TotalProcSales 
from dbo.UFN_IM_GetCloserSales(@DateFrom,@DateTo,@SalesRooms,1,default,default) GROUP BY Closer
UNION ALL
--PendSales
select Closer saCloser,0 Shows,0 ProcSales,
0 ProcAmount,0 OOPSales,0 OOPAmount,SUM(Sales) PendSales, 
0 PendAmount,0 CancelSales,0 CancelAmount,
0 TotalProcAmount,0 TotalProcSales 
from dbo.UFN_IM_GetCloserSales(@DateFrom,@DateTo,@SalesRooms,default,default,1) GROUP BY Closer
UNION ALL
--Cancel Sales
select Closer saCloser,0 Shows,0 ProcSales,
0 ProcAmount,0 OOPSales,0 OOPAmount,0 PendSales, 
0 PendAmount,SUM(Sales) CancelSales,0 CancelAmount,
0 TotalProcAmount,0 TotalProcSales 
from dbo.UFN_IM_GetCloserSales(@DateFrom,@DateTo,@SalesRooms,default,1,default) GROUP BY Closer
UNION ALL
select Closer saCloser,0 Shows,0 ProcSales,
0 ProcAmount,0 OOPSales,0 OOPAmount,0 PendSales, 
0 PendAmount,0 CancelSales,0 CancelAmount,
0 TotalProcAmount,SUM(Sales) TotalProcSales 
from dbo.UFN_IM_GetCloserSales(@DateFrom,@DateTo,@SalesRooms,default,default,default) GROUP BY Closer
) AS CloserStats

group by saCloser
order by procAMount DESC

Select saCloser,
	pe.peN saCloserN,
	pe.peps saCloserps,
	Shows,	
	ProcSales,
	ProcAmount,
	OOPSales,
	OOPAmount,
	PendSales,
	PendAmount,
	CancelSales,
	CancelAmount,
	TotalProcAmount,
	TotalProcSales,
	dbo.UFN_OR_SecureDivision(TotalProcAmount,Shows) Efficiency,
	dbo.UFN_OR_SecureDivision(TotalProcSales,Shows) ClosingF,
	dbo.UFN_OR_SecureDivision(TotalProcAmount,TotalProcSales) AvgSales,
	(ProcAmount+OOPAmount) TotalProc,
	case 
	when (ProcAmount+OOPAmount) > 0 THEN (dbo.UFN_OR_SecureDivision(CancelAmount,(ProcAmount+OOPAmount)))
	when (ProcAmount+OOPAmount) = 0 THEN 0
	else 1 end CancelF
	from #CloserStats c
left join Personnel pe on c.saCloser=pe.peID;
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptDailyGiftSimple]    Script Date: 09/21/2016 16:59:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene la cantidad de regalos por Sala de ventas.
** 
** [edgrodriguez]	07/Abr/2016 Created
** [wtorres]		05/Ago/2016 Modified. Optimizacion del WHERE de "Charge To"
**
*/
CREATE PROCEDURE [dbo].[USP_IM_RptDailyGiftSimple]
	@Date as datetime,
	@SalesRooms varchar(8000) = 'ALL'
as
set nocount on

SELECT 
	R.grCancel,
	R.grID,
	G.giN,
	G.giShortN,
	D.geQty,
	R.grlo
FROM GiftsReceipts R
	INNER JOIN GiftsReceiptsC D ON D.gegr = R.grID
	INNER JOIN Gifts G ON G.giID = D.gegi
WHERE
	-- Sala de ventas
	(@SalesRooms = 'ALL' or R.grsr in (select item from split(@SalesRooms, ',')))
	-- Fecha del recibo
	AND R.grD = @Date
	-- No cancelados
	AND R.grCancel = 0
	-- No cargados a vendedores
	AND R.grct NOT IN ('PR', 'LINER', 'CLOSER')
ORDER BY D.gegi
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptGiftsKardex]    Script Date: 09/21/2016 16:59:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de kardex de regalos
** 
** [edgrodriguez]	02/Jun/2016 Created.
**
*/
CREATE procedure [dbo].[USP_IM_RptGiftsKardex]
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@Warehouse varchar(10),		-- Clave del almacen
	@Gifts varchar(max) = 'ALL'	-- Claves de regalos
as

set fmtonly off
set nocount on

declare
	@FirstMonth datetime,		-- Sirve para ir al primer dia del mes de la fecha inicial
	@LastDayPrevious datetime,	-- Sirve para ir a un dia antes de la fecha inicial
	@TemporalInventory bit		-- indicamos que se genero el inventario temporal del primer mes

 --Tabla de inventario
declare @Inventory table (
	InvGi varchar(10),
	InvQty int
);

-- =============================================
--					Inventario
-- =============================================

--			Inventario del mes inicial
-- =============================================
-- generamos el inventario del mes inicial si no existe

-- obtenemos el primer dia del mes de la fecha inicial
set @FirstMonth = DATEADD(dd,-(DAY(@DateFrom)-1),@DateFrom)

-- si no existe el inventario del mes inicial
if (select Count(*) from GiftsInventory where gvD = @FirstMonth and gvwh = @Warehouse) = 0
begin

	-- indicamos que se genero el inventario temporal del primer mes
	set @TemporalInventory = 1

	-- generamos el inventario temporal del primer mes
	exec USP_OR_GetMonthInventory @Warehouse, @FirstMonth, @DateTo
end
else
	-- indicamos que no se genero el inventario temporal del primer mes
	set @TemporalInventory = 0

-- si la fecha inicial no es el primer dia del mes
if (Day(@DateFrom)) > 1
begin

	-- Inventario hasta un dia antes de la fecha inicial
	-- =============================================

	-- obtenemos la fecha de un dia antes de la fecha inicial
	set @LastDayPrevious = DateAdd(Day, -1 , @DateFrom)

	-- Inventario del primer mes
	insert into @Inventory
	select Gift, Quantity from UFN_OR_GetGiftsInventory(@FirstMonth, @Warehouse, @Gifts)

	-- Movimientos de almacen anteriores
	-- =============================================
	select Gift, Quantity
	into #WarehouseMovementsPrevious
	from UFN_OR_GetGiftWarehouseMovementsQuantity(@FirstMonth, @LastDayPrevious, @Warehouse, @Gifts)
	
	-- actualizamos el inventario, sumandole los movimientos de almacen anteriores
	update @Inventory
	set	InvQty = InvQty + M.Quantity
	from @Inventory I
		inner join #WarehouseMovementsPrevious M on M.Gift = I.InvGi

	-- agregamos los movimientos de almacen anteriores de los regalos inexistentes en el inventario
	insert into @Inventory
	select
		Gift,
		case when Quantity > 0 then Quantity else Quantity * -1 end
	from #WarehouseMovementsPrevious
	where
		-- Regalo que no existe en el inventario
		not exists (select InvGi from @Inventory)

	-- Recibos de regalos anteriores
	-- =============================================
	select Gift, Quantity
	into #GiftsReceiptsPrevious
	from UFN_OR_GetGiftGiftsReceiptsQuantity(@FirstMonth, @LastDayPrevious, @Warehouse, default, @Gifts)
	
	-- actualizamos el inventario, restandole los recibos anteriores
	update @Inventory
	set	InvQty = InvQty - R.Quantity
	from @Inventory I
		inner join #GiftsReceiptsPrevious R on R.Gift = I.InvGi

	-- agregamos los recibos anteriores de los regalos inexistentes en el inventario
	insert into @Inventory
	select 
		Gift, 
		case when Quantity > 0 then Quantity * -1 else Quantity end
	from #GiftsReceiptsPrevious
	where
		-- Regalo que no existe en el inventario
		not exists (select InvGi from @Inventory)
end

-- si la fecha inicial es el primer dia del mes
else
begin

	-- Inventario al primer dia del mes
	-- =============================================
	insert into @Inventory
	select Gift, Quantity from UFN_OR_GetGiftsInventory(@DateFrom, @Warehouse, @Gifts)
end

-- eliminamos los inventarios temporales, si se generaron
if @TemporalInventory = 1
	delete from GiftsInventory
	where gvwh = @Warehouse 
		and gvD >= (select srGiftsRcptCloseD from SalesRooms where srwh = @Warehouse)

-- Movimientos de almacen
-- =============================================
select * into #WarehouseMovements from (
	-- Entradas
	select [Date] as MovD, Gift as MovGi, Quantity as MovQty
	from UFN_OR_GetDateGiftWarehouseMovementsQuantity(@DateFrom, @DateTo, @Warehouse, @Gifts, 1)
	-- Salidas
	union all
	select [Date], Gift, Quantity
	from UFN_OR_GetDateGiftWarehouseMovementsQuantity(@DateFrom, @DateTo, @Warehouse, @Gifts, 0)
) as D

 --5. Recibos de regalos
 --=============================================
select * into #GiftsReceipts from (
	-- Salidas
	select [Date] as RecD, Gift as RecGi, Quantity as RecQty
	from UFN_OR_GetDateGiftGiftsReceiptsQuantity(@DateFrom, @DateTo, @Warehouse, @Gifts, 1)
	-- Devoluciones
	union all
	select [Date], Gift, Quantity
	from UFN_OR_GetDateGiftGiftsReceiptsQuantity(@DateFrom, @DateTo, @Warehouse, @Gifts, 0)
) as D


-- Reporte GiftKardex
-- =============================================
declare @RptKardex table(
	MovD datetime,
	MovGi varchar(150),
	MovGiN varchar(400),
	[InQty] int,
	[OutQty] int,
	InvQty int
);

WHILE(@DateFrom <= @DateTo)
Begin
	Declare @Gift varchar(150), @GiftN varchar(500), @InvQty int;
		
	--Creamos un cursor con los regalores inventariables.
	DECLARE GiftsCursor Cursor Fast_Forward For select I.InvGi,G.giN, ISNULL(I.InvQty,0) from @Inventory I left join Gifts G on I.InvGi = G.giID;
	OPEN GiftsCursor;
	FETCH GiftsCursor INTO @Gift,@GiftN, @InvQty;
	WHILE (@@FETCH_STATUS = 0 )
	BEGIN
		--Declaramos las variables de WarehouseQty y ReceiptQty
		Declare @WQty int, @RQty int;
		set @WQty=0;
		set @RQty=0;
		
		--Obtenemos la cantidad de salida/entrada de gifts de Warehouse y Receipts.
		Select @WQty=ISNULL(MovQty , 0) from #WarehouseMovements WHERE MovD=@DateFrom and MovGi=@Gift;
		Select @RQty=ISNULL(RecQty , 0) from #GiftsReceipts WHERE RecD=@DateFrom and RecGi=@Gift
		
		--Entradas
		Select @InvQty = CASE
			--Si la cantidad de Gifts de Warehouse es Positiva y la cantidad de Gifts de Receipts es negativa
			--entonces sumamos los valores absolutos. (Entrada al Inventario.)
			When ISNULL(@WQty,0) >= 0 AND ISNULL(@RQty,0) <= 0 THEN (@InvQty +(ABS(@WQty)+ ABS(@RQty)))
		ELSE @InvQty END
		
		--Salidas
		Select @InvQty = CASE
			--Si la cantidad de Gifts de Warehouse es Negativa y la cantidad de Gifts de Receipts es Positiva
			--entonces sumamos los valores absolutos. (Entrada al Inventario.)
			When ISNULL(@WQty,0) <= 0 AND ISNULL(@RQty,0) >= 0 THEN (@InvQty - (ABS(@WQty)+ ABS(@RQty)))
		ELSE @InvQty END
		
		--Insertamos los datos a la tabla del reporte.
		INSERT INTO @RptKardex
		SELECT @DateFrom, @Gift, @GiftN,
		CASE
			When ISNULL(@WQty,0) >= 0 AND ISNULL(@RQty,0) <= 0 THEN (ABS(@WQty)+ ABS(@RQty))
		ELSE 0 END [In],
		CASE
			When ISNULL(@WQty,0) <= 0 AND ISNULL(@RQty,0) >= 0 THEN (ABS(@WQty)+ ABS(@RQty))
		ELSE 0 END [Out],		
		@InvQty;
		
		--Actualizamos el inventario.
		Update @Inventory set InvQty=@InvQty where InvGi = @Gift;
		
		FETCH GiftsCursor 
		INTO @Gift,@GiftN, @InvQty;
	END
	CLOSE GiftsCursor;
	DEALLOCATE GiftsCursor;
	
	SET @DateFrom = DATEADD(day,1,@DateFrom)
End

select * from @RptKardex
Order by MovGi,MovD


GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptLinerStatistics]    Script Date: 09/21/2016 16:59:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_IM_RptLinerStatistics]
	@DateFrom as DateTime,			-- Fecha desde
	@DateTo as DateTime,			-- Fecha hasta
	@SalesRooms as varchar(8000)	-- Claves de las salas de ventas
as

set nocount on

SELECT
Liner,
(SUM(TotalShows)+SUM(WO)) Shows,
SUM(WO) WalkOut,
SUM(TotalShows)TotalShows,
(SUM(TotalProcSales) - SUM(OOPSales) + SUM(CancSales)) ProcSales,
(SUM(TotalProcAmount) - SUM(OOPAmount) + SUM(CancAmount)) ProcAmount,
SUM(OOPSales) OOPSales,
SUM(OOPAmount) OOPAmount,
SUM(CancSales) CancSales,
SUM(CancAmount) CancAmount,
SUM(TotalProcSales) TotalProcSales,
SUM(TotalProcAmount) TotalProcAmount,
SUM(ProcSalesLn) ProcSalesLn,
SUM(ProcAmountLn) ProcAmountLn,
SUM(ProcSalesFM) ProcSalesFM,
SUM(ProcAmountFM) ProcAmountFM,
SUM(ProcSalesFB) ProcSalesFB,
SUM(ProcAmountFB) ProcAmountFB,
SUM(PendSales) PendSales,
SUM(PendAmount) PendAmount
into #linerStats 
FROM
(
--WalkOut
select Liner,0 TotalShows,SUM(Shows) WO,0 ProcSales,0 ProcAmount,0 OOPSales,0 OOPAmount,0 CancSales,0 CancAmount,0 TotalProcSales,0 TotalProcAmount,0 ProcSalesLn,0 ProcAmountLn,0 ProcSalesFM,0 ProcAmountFM,0 ProcSalesFB,0 ProcAmountFB,0 PendSales,0 PendAmount 
from dbo.UFN_IM_GetLinerShows (@DateFrom, @DateTo, @SalesRooms, 3,default) group by Liner
UNION ALL
--Shows
select Liner,sum(Shows) TotalShows,0 WO,0 ProcSales,0 ProcAmount,0 OOPSales,0 OOPAmount,0 CancSales,0 CancAmount,0 TotalProcSales,0 TotalProcAmount,0 ProcSalesLn,0 ProcAmountLn,0 ProcSalesFM,0 ProcAmountFM,0 ProcSalesFB,0 ProcAmountFB,0 PendSales,0 PendAmount 
from dbo.UFN_IM_GetLinerShows (@DateFrom, @DateTo, @SalesRooms, 1,default) group by Liner
UNION ALL
--OOPSales
Select Liner,0 TotalShows,0 WO,0 ProcSales,0 ProcAmount,sum(Sales) OOPSales,0 OOPAmount,0 CancSales,0 CancAmount,0 TotalProcSales,0 TotalProcAmount,0 ProcSalesLn,0 ProcAmountLn,0 ProcSalesFM,0 ProcAmountFM,0 ProcSalesFB,0 ProcAmountFB,0 PendSales,0 PendAmount 
from dbo.UFN_IM_GetLinerSales(@DateFrom,@DateTo,@SalesRooms,1,default,default,default,default) group by liner
UNION ALL
--OOPAmount
select Liner,0 TotalShows,0 WO,0 ProcSales,0 ProcAmount,0 OOPSales,Sum(SalesAmount) OOPAmount,0 CancSales,0 CancAmount,0 TotalProcSales,0 TotalProcAmount,0 ProcSalesLn,0 ProcAmountLn,0 ProcSalesFM,0 ProcAmountFM,0 ProcSalesFB,0 ProcAmountFB,0 PendSales,0 PendAmount 
from dbo.UFN_IM_GetLinerSalesAmount(@DateFrom,@DateTo,@SalesRooms,1,default,default,default,default,default,default) group by Liner
UNION ALL
--CancSales
Select Liner,0 TotalShows,0 WO,0 ProcSales,0 ProcAmount,0 OOPSales,0 OOPAmount,sum(Sales) CancSales,0 CancAmount,0 TotalProcSales,0 TotalProcAmount,0 ProcSalesLn,0 ProcAmountLn,0 ProcSalesFM,0 ProcAmountFM,0 ProcSalesFB,0 ProcAmountFB,0 PendSales,0 PendAmount
from dbo.UFN_IM_GetLinerSales(@DateFrom,@DateTo,@SalesRooms,default,1,default,default,default) group by Liner
UNION ALL
--CancAmount
select Liner,0 TotalShows,0 WO,0 ProcSales,0 ProcAmount,0 OOPSales,0 OOPAmount,0 CancSales,sum(SalesAmount) CancAmount,0 TotalProcSales,0 TotalProcAmount,0 ProcSalesLn,0 ProcAmountLn,0 ProcSalesFM,0 ProcAmountFM,0 ProcSalesFB,0 ProcAmountFB,0 PendSales,0 PendAmount 
from dbo.UFN_IM_GetLinerSalesAmount(@DateFrom,@DateTo,@SalesRooms,default,1,default,default,default,default,default) group by Liner
UNION ALL
--PendSales
Select Liner,0 TotalShows,0 WO,0 ProcSales,0 ProcAmount,0 OOPSales,0 OOPAmount,0 CancSales,0 CancAmount,0 TotalProcSales,0 TotalProcAmount,0 ProcSalesLn,0 ProcAmountLn,0 ProcSalesFM,0 ProcAmountFM,0 ProcSalesFB,0 ProcAmountFB,sum(Sales) PendSales,0 PendAmount 
from dbo.UFN_IM_GetLinerSales(@DateFrom,@DateTo,@SalesRooms,default,default,1,default,default) group by Liner
UNION ALL
--PendAmount
select Liner,0 Shows,0 WO,0 ProcSales,0 ProcAmount,0 OOPSales,0 OOPAmount,0 CancSales,0 CancAmount,0 TotalProcSales,0 TotalProcAmount,0 ProcSalesLn,0 ProcAmountLn,0 ProcSalesFM,0 ProcAmountFM,0 ProcSalesFB,0 ProcAmountFB,0 PendSales,sum(SalesAmount) PendAmount
from dbo.UFN_IM_GetLinerSalesAmount(@DateFrom,@DateTo,@SalesRooms,default,default,1,default,default,default,default) group by Liner
UNION ALL
--ProcSalesLn
Select Liner,0 TotalShows,0 WO,0 ProcSales,0 ProcAmount,0 OOPSales,0 OOPAmount,0 CancSales,0 CancAmount,0 TotalProcSales,0 TotalProcAmount,sum(Sales) ProcSalesLn,0 ProcAmountLn,0 ProcSalesFM,0 ProcAmountFM,0 ProcSalesFB,0 ProcAmountFB,0 PendSales,0 PendAmount
from dbo.UFN_IM_GetLinerSales(@DateFrom,@DateTo,@SalesRooms,default,default,default,0,default)group by Liner
UNION ALL
--ProcAmountLn
Select Liner,0 TotalShows,0 WO,0 ProcSales,0 ProcAmount,0 OOPSales,0 OOPAmount,0 CancSales,0 CancAmount,0 TotalProcSales,0 TotalProcAmount,0 ProcSalesLn,sum(SalesAmount) ProcAmountLn,0 ProcSalesFM,0 ProcAmountFM,0 ProcSalesFB,0 ProcAmountFB,0 PendSales,0 PendAmount
from dbo.UFN_IM_GetLinerSalesAmount(@DateFrom,@DateTo,@SalesRooms,default,default,default,0,default,default,default)group by Liner
UNION ALL
--ProcSalesFM
Select Liner,0 TotalShows,0 WO,0 ProcSales,0 ProcAmount,0 OOPSales,0 OOPAmount,0 CancSales,0 CancAmount,0 TotalProcSales,0 TotalProcAmount,0 ProcSalesLn,0 ProcAmountLn,sum(Sales) ProcSalesFM,0 ProcAmountFM,0 ProcSalesFB,0 ProcAmountFB,0 PendSales,0 PendAmount
from dbo.UFN_IM_GetLinerSales(@DateFrom,@DateTo,@SalesRooms,default,default,default,1,default)group by Liner
UNION ALL
--TotalProcAmountFM
select Liner,0 TotalShows,0 WO,0 ProcSales,0 ProcAmount,0 OOPSales,0 OOPAmount,0 CancSales,0 CancAmount,0 TotalProcSales,0 TotalProcAmount,0 ProcSalesLn,0 ProcAmountLn,0 ProcSalesFM,sum(SalesAmount) ProcAmountFM,0 ProcSalesFB,0 ProcAmountFB,0 PendSales,0 PendAmount
from dbo.UFN_IM_GetLinerSalesAmount(@DateFrom,@DateTo,@SalesRooms,default,default,default,1,default,default,default)group by Liner
UNION ALL
--ProcSalesFB
Select Liner,0 TotalShows,0 WO,0 ProcSales,0 ProcAmount,0 OOPSales,0 OOPAmount,0 CancSales,0 CancAmount,0 TotalProcSales,0 TotalProcAmount,0 ProcSalesLn,0 ProcAmountLn,0 ProcSalesFM,0 ProcAmountFM,sum(Sales) ProcSalesFB,0 ProcAmountFB,0 PendSales,0 PendAmount
from dbo.UFN_IM_GetLinerSales(@DateFrom,@DateTo,@SalesRooms,default,default,default,2,default)group by Liner
UNION ALL
--TotalProcAmountFB
select Liner,0 TotalShows,0 WO,0 ProcSales,0 ProcAmount,0 OOPSales,0 OOPAmount,0 CancSales,0 CancAmount,0 TotalProcSales,0 TotalProcAmount,0 ProcSalesLn,0 ProcAmountLn,0 ProcSalesFM,0 ProcAmountFM,0 ProcSalesFB,sum(SalesAmount) ProcAmountFB,0 PendSales,0 PendAmount
from dbo.UFN_IM_GetLinerSalesAmount(@DateFrom,@DateTo,@SalesRooms,default,default,default,2,default,default,default)group by Liner
UNION ALL
--TotalProcSales
Select Liner,0 TotalShows,0 WO,0 ProcSales,0 ProcAmount,0 OOPSales,0 OOPAmount,0 CancSales,0 CancAmount,sum(Sales) TotalProcSales,0 TotalProcAmount,0 ProcSalesLn,0 ProcAmountLn,0 ProcSalesFM,0 ProcAmountFM,0 ProcSalesFB,0 ProcAmountFB,0 PendSales,0 PendAmount
from dbo.UFN_IM_GetLinerSales(@DateFrom,@DateTo,@SalesRooms,default,default,default,default,default)group by Liner
UNION ALL
--TotalProcAmount
Select Liner,0 TotalShows,0 WO,0 ProcSales,0 ProcAmount,0 OOPSales,0 OOPAmount,0 CancSales,0 CancAmount,0 TotalProcSales,sum(SalesAmount) TotalProcAmount,0 ProcSalesLn,0 ProcAmountLn,0 ProcSalesFM,0 ProcAmountFM,0 ProcSalesFB,0 ProcAmountFB,0 PendSales,0 PendAmount
from dbo.UFN_IM_GetLinerSalesAmount(@DateFrom,@DateTo,@SalesRooms,default,default,default,default,default,default,default)group by Liner

) as LinerStats
GROUP BY Liner
ORDER BY TotalProcAmount desc

Select 
	Liner saLiner,
	pe.peN saLinerN,
	pe.peps saLinerps,
	Shows,
	WalkOut,
	TotalShows,
	ProcSales,
	ProcAmount,
	OOPSales,
	OOPAmount,
	CancSales,
	CancAmount,
	TotalProcSales,
	TotalProcAmount,
	ProcSalesLn,
	ProcAmountLn,
	ProcSalesFM,
	ProcAmountFM,
	ProcSalesFB,
	ProcAmountFB,
	PendSales,
	PendAmount,
	dbo.UFN_OR_SecureDivision(TotalProcAmount,Shows) Efficiency,
	dbo.UFN_OR_SecureDivision(TotalProcSales,Shows) ClosingF,
	dbo.UFN_OR_SecureDivision(TotalProcAmount,TotalProcSales) AvgSales,
	(ProcAmount + OOPAmount) TotalProc,
	case 
	when (ProcAmount + OOPAmount) > 0 THEN (dbo.UFN_OR_SecureDivision(CancAmount,(ProcAmount + OOPAmount)))
	when (ProcAmount + OOPAmount) = 0 THEN 0
	else 1 end CancelF
	from #linerStats li
left join Personnel pe on li.Liner=pe.peID
order by saLinerps asc,TotalProcAmount desc, Efficiency desc;


GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptLoginLog]    Script Date: 09/21/2016 16:59:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de Log de Login
** 
** [edgrodriguez]	26/04/2016 Created
*/
CREATE PROCEDURE [dbo].[USP_IM_RptLoginLog]
@DateFrom datetime,
@DateTo datetime,
@Location varchar(8000)='ALL',
@PCName varchar(8000)='ALL',
@Personnel varchar(8000)='ALL'
AS
SET NOCOUNT OFF
BEGIN
	SELECT 
		LG.llID 'Date/Time',L.loN Location,LG.llpe Code,P.peN Name,LG.llPCName PC
	FROM LoginsLog LG
	INNER JOIN Locations L ON LG.lllo=L.loID
	INNER JOIN Personnel P ON LG.llpe=P.peID
	WHERE 
		LG.llID BETWEEN @DateFrom AND @DateTo
		and (@Location = 'ALL' or L.loID=@Location)
		and (@PCName ='ALL' or LG.llPCName=@PCName)
		and (@Personnel = 'ALL' or P.peID=@Personnel)
	ORDER BY LG.llID DESC
END
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptManifestByLSRange]    Script Date: 09/21/2016 16:59:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de manifiesto (ProcessorGeneral) permitiendo seleccionar varias salas de ventas y un rango de fechas
**		1. Reporte Manifiesto
**		2. Bookings
**
** [edgrodriguez]	01/Jun/2016 Created
** [edgrodriguez]	20/Jun/2016 Modified Se agregaron las columnas DownPayment,DownPaymentPercentage,DownPaymentPaid,DownPaymentPaidPercentage
** [edgrodriguez]	21/Sep/2016 Modified Se quito el OuterApply y se uso una tabla temporal para obtener los Guest Additionals.
*/
CREATE procedure [dbo].[USP_IM_RptManifestByLSRange]
	@DateFrom Datetime,				-- Fecha desde
	@DateTo Datetime,				-- Fecha hasta
	@SalesRoom varchar(8000) = 'ALL'	-- Claves de las salas de ventas
as
--set fmtonly off
set nocount on

-- ================================================
-- 					GUEST ADDITIONAL
-- ================================================

DECLARE @AddGuest table(gagu integer,
                        gaShow datetime,
                        gaLoc varchar(50),
                        gaFlyers varchar(50),
                        gaLoN varchar(50))


INSERT INTO @AddGuest       
SELECT  ga.gaAdditional,      
    CASE WHEN isnull(ga.gagu,0) > 0 AND g.gushowD is null THEN 
      g2.guShowD 
    ELSE
      NULL
    END [gaShow],
    g2.guloInvit [gaLoc],
    l2.loFlyers [gaFlyers],
    l2.loN [gaLoN]    
FROM guests g
INNER JOIN dbo.GuestsAdditional ga ON ga.gaAdditional = g.guid                
INNER JOIN dbo.Guests g2 ON g2.guID = ga.gagu
LEFT JOIN dbo.Locations l2 ON l2.loID= g2.guloInvit
WHERE 
(@SalesRoom = 'ALL' or g2.gusr in (select item from split(@SalesRoom, ',')))
AND (@SalesRoom = 'ALL' or g.gusr in (select item from split(@SalesRoom, ',')))
AND g2.guShowD between @DateFrom and @DateTo


-- ================================================
-- 					MANIFIESTO
-- ================================================
select
	D.*,
	-- Programa de show
	SH.skN as ShowProgramN
  INTO #Manifest
from (
	select
		DateManifest = CASE
			WHEN G.guShowD IS NOT NULL THEN G.guShowD
			ELSE
			(SELECT gaShow from @AddGuest where gagu=G.guID)
		END,
		SaleType = CASE
			WHEN G.guCTour = 1 THEN 1
			WHEN G.guSaveProgram = 1 THEN 2
			WHEN S.sagu IS NOT NULL THEN
				CASE 
					WHEN S.sast<>'BUMP' and S.sast<>'REGEN'
					AND (S.saD BETWEEN @DateFrom and @DateTo) 
					AND (S.sast='DNG' OR S.sast='DP') THEN 5
					ELSE
					0
				END
			ELSE
			0
		END ,
		SaleTypeN = CASE
			WHEN G.guCTour = 1 THEN 'COURTESY TOUR'
			WHEN G.guSaveProgram = 1 THEN 'SAVE TOUR'
			WHEN S.sagu IS NOT NULL THEN
				CASE 
					WHEN S.sast<>'BUMP' and S.sast<>'REGEN'
					AND (S.saD BETWEEN @DateFrom and @DateTo) 
					AND (S.sast='DNG' OR S.sast='DP') THEN 'DOWNGRADES'
					ELSE
					'MANIFEST'
				END
			ELSE
			'MANIFEST'
		END,
		G.guID,
		G.gusr,
		G.guls,
		G.guShowSeq,
		isnull(G.guloInvit,(SELECT gaLoc from @AddGuest where gagu=G.guID)) as guloInvit,
		isnull(L.loN,(SELECT gaLoN from @AddGuest where gagu=G.guID)) as loN,
		isnull(L.loFlyers,(SELECT gaFlyers from @AddGuest where gagu=G.guID)) as loFlyers,
		G.guDeposit,
		G.guHotel, 
		G.guRoomNum, 
		G.guPax,
		dbo.AddString(G.guLastName1, G.guLastName2, ' / ') as guLastName1,
		dbo.AddString(G.guFirstName1, G.guFirstName2, ' / ') as guFirstName1,
		G.guag,
		A.agN,
		G.guco,
		C.coN, 
		G.guDirect,
		G.guTimeInT,
		G.guTimeOutT, 
		G.guCheckInD,
		G.guCheckOutD,
		G.guResch,
		G.guReschD,
		G.guShow,
		G.guTour,
		G.guInOut, 
		G.guWalkOut,
		G.guCTour,
		G.guSaveProgram,
		G.guPRInvit1 PR1, 
		P1.peN as PR1N,
		G.guPRInvit2 PR2, 
		P2.peN as PR2N,
		G.guPRInvit3 PR3, 
		P3.peN as PR3N,
		G.guLiner1 Liner1, 
		L1.peN as Liner1N,
		G.guLiner2 Liner2, 
		L2.peN as Liner2N,
		G.guCloser1 Closer1, 
		C1.peN as Closer1N,
		G.guCloser2 Closer2, 
		C2.peN as Closer2N,
		G.guCloser3 Closer3,
		C3.peN as Closer3N,
		G.guExit1 Exit1, 
		E1.peN as Exit1N,
		G.guExit2 Exit2, 
		E2.peN as Exit2N,
		G.guEntryHost, 
		G.guWcomments, 
		G.guBookD, 
		G.guShowD,
		G.guInvitD, 
		G.guDepSale,
		G.guDepositSaleNum,
		G.guDepositSaleD,
		S.sagu,
		S.sast,
		ST.ststc,
		S.saMembershipNum,
		S.saOriginalAmount,
		S.saGrossAmount, 
		S.saNewAmount, 
		S.saD,
		S.saProcD,
		S.saCancelD,
		S.saClosingCost,
		S.saComments,
		S.saProcRD,
		S.saDownPaymentPercentage / 100 as saDownPaymentPercentage,
		(S.saDownPaymentPercentage / 100) * S.saGrossAmount as saDownPayment,
		S.saDownPaymentPaidPercentage / 100 as saDownPaymentPaidPercentage,
		(S.saDownPaymentPaidPercentage / 100)* S.saGrossAmount as saDownPaymentPaid,
		dbo.UFN_OR_GetShowProgram(G.guSaveProgram, G.guCTour, 0, SR.srAppointment) as ShowProgram,
		gashow = CASE
			WHEN G.guShowD IS NOT NULL THEN NULL
			ELSE
			(SELECT gaShow from @AddGuest where gagu=G.guID)
		END,
		NULL CnxSale,
		S.sasr,
		S.salo  
	from Guests G
		left outer join Sales S on G.guID = S.sagu and (S.saD between @DateFrom and @DateTo or S.saProcD between @DateFrom and @DateTo or S.saCancelD between @DateFrom and @DateTo) 
		left join SaleTypes ST on ST.stID = S.sast
		left join Personnel P1 on P1.peID = G.guPRInvit1
		left join Personnel P2 on P2.peID = G.guPRInvit2
		left join Personnel P3 on P3.peID = G.guPRInvit3
		left join Personnel L1 on L1.peID = G.guLiner1
		left join Personnel L2 on L2.peID = G.guLiner2
		left join Personnel C1 on C1.peID = G.guCloser1
		left join Personnel C2 on C2.peID = G.guCloser2
		left join Personnel C3 on C3.peID = G.guCloser3
		left join Personnel E1 on E1.peID = G.guExit1
		left join Personnel E2 on E2.peID = G.guExit2
		left join Locations L on L.loID = G.guloInvit
		left join SalesRooms SR on SR.srID = G.gusr
		left join Agencies A on A.agID = G.guag
		left join Countries C on C.coID = G.guco
	where
		-- Fecha de show
		(G.guShowD BETWEEN @DateFrom and @DateTo
		or G.guID in (select gagu from @AddGuest)
		)
		-- Sala de ventas
		and (@SalesRoom = 'ALL' or G.gusr in (select item from split(@SalesRoom, ',')))
) as D
	inner join ShowPrograms SH on D.ShowProgram = SH.skID
order by D.guID

--Select * from #Manifest

-- ================================================
-- 					Ventas Deposito
-- ================================================
select
	D.*,
	-- Programa de show
	SH.skN as ShowProgramN
	into #DepositSales
from (
	select
		NULL DateManifest,
		12 SaleType,
		'DEPOSIT SALE' SaleTypeN,
		G.guID,
		G.gusr,
		G.guls,
		G.guShowSeq,
		G.guloInvit,
		L.loN,
		NULL loFlyers,
		G.guDeposit,
		G.guHotel, 
		G.guRoomNum, 
		G.guPax,
		dbo.AddString(G.guLastName1, G.guLastName2, ' / ') as guLastName1,
		dbo.AddString(G.guFirstName1, G.guFirstName2, ' / ') as guFirstName1,
		G.guag,
		A.agN, 
		G.guco,
		C.coN, 
		G.guDirect,
		G.guTimeInT,
		G.guTimeOutT, 
		G.guCheckInD,
		G.guCheckOutD,
		G.guResch,
		G.guReschD,
		G.guShow,
		G.guTour,
		G.guInOut, 
		G.guWalkOut,
		G.guCTour,
		G.guSaveProgram,
		G.guPRInvit1 PR1, 
		P1.peN as PR1N,
		G.guPRInvit2 PR2, 
		P2.peN as PR2N,
		G.guPRInvit3 PR3, 
		P3.peN as PR3N,
		G.guLiner1 Liner1, 
		L1.peN as Liner1N,
		G.guLiner2 Liner2, 
		L2.peN as Liner2N,
		G.guCloser1 Closer1, 
		C1.peN as Closer1N,
		G.guCloser2 Closer2, 
		C2.peN as Closer2N,
		G.guCloser3 Closer3,
		C3.peN as Closer3N,
		G.guExit1 Exit1, 
		E1.peN as Exit1N,
		G.guExit2 Exit2, 
		E2.peN as Exit2N,
		G.guEntryHost, 
		G.guWcomments, 
		G.guBookD, 
		G.guShowD,
		G.guInvitD, 
		G.guDepSale,
		G.guDepositSaleNum,
		G.guDepositSaleD,
		'' sagu,
		'' sast,
		'' ststc,
		'' saMemberShip,
		NULL saOriginalAmount,
		NULL saGrossAmount,
		NULL saNewAmount,
		NULL saD,
		NULL saProcD,
		NULL saCancelD,
		NULL saClosingCost,
		'' saComments,
		NULL saProcRD,
		NULL saDownPaymentPercentage,
		NULL saDownPayment,
		NULL saDownPaymentPaidPercentage,
		NULL saDownPaymentPaid,
		dbo.UFN_OR_GetShowProgram(G.guSaveProgram, G.guCTour, 0, SR.srAppointment) as ShowProgram,
		'' gaShow,
		NULL CnxSale,
		'' sasr,
		'' salo	
		from Guests G
		left join Personnel P1 on P1.peID = G.guPRInvit1
		left join Personnel P2 on P2.peID = G.guPRInvit2
		left join Personnel P3 on P3.peID = G.guPRInvit3
		left join Personnel L1 on L1.peID = G.guLiner1
		left join Personnel L2 on L2.peID = G.guLiner2
		left join Personnel C1 on C1.peID = G.guCloser1
		left join Personnel C2 on C2.peID = G.guCloser2
		left join Personnel C3 on C3.peID = G.guCloser3
		left join Personnel E1 on E1.peID = G.guExit1
		left join Personnel E2 on E2.peID = G.guExit2
		left join Locations L on L.loID = G.guloInvit
		left join SalesRooms SR on SR.srID = G.gusr
		left join Agencies A on A.agID = G.guag
		left join Countries C on C.coID = G.guco
	where
		-- Fecha de venta deposito
		G.guDepositSaleD between @DateFrom and @DateTo
		-- Fecha de show diferente de la fecha de venta deposito
		and G.guShowD <> G.guDepositSaleD
		-- Sala de ventas
		and (@SalesRoom = 'ALL' or G.gusr in (select item from split(@SalesRoom, ',')))
) as D
	inner join ShowPrograms SH on D.ShowProgram = SH.skID
order by D.guID

--Select * from #DepositSales

-- ================================================
-- 					OTRAS VENTAS
-- ================================================
-- Ventas de otros dias, Be Backs, OOP, Cancellations, Regens, Deposit Before, etc.
select
	D.*,
	-- Programa de show
	SH.skN as ShowProgramN
	into #OtherSales
from (
	select
	CASE
		WHEN S.saCancelD IS NOT NULL THEN S.saCancelD
		ELSE
		S.saProcD
	END DateManifest,
		dbo.UFN_IM_GetSaleType(@DateFrom,@DateTo,S.sast,ST.ststc,G.guDepSale,S.saD,S.saProcD,S.saCancelD,G.gusr,S.sasr,S.saByPhone) SaleType,
		CASE 
		dbo.UFN_IM_GetSaleType(@DateFrom,@DateTo,S.sast,ST.ststc,G.guDepSale,S.saD,S.saProcD,S.saCancelD,G.gusr,S.sasr,S.saByPhone)
		WHEN 3 THEN 'BE BACKS'
		WHEN 4 THEN 'UPGRADES'
		WHEN 5 THEN 'DOWNGRADES'
		WHEN 6 THEN 'OUT OF PENDING'
		WHEN 7 THEN 'CANCELATION'
		WHEN 8 THEN 'BUMP'
		WHEN 9 THEN 'REGEN'
		WHEN 10 THEN 'DEPOSIT BEFORE'
		WHEN 11 THEN 'PHONE SALES'
		WHEN 13 THEN 'SALES FROM OTHER SALES ROOMS TOURS'
		WHEN 14 THEN 'UNIFICATIONS'
	END SaleTypeN,
		G.guID,
		G.gusr,
		G.guls,
		G.guShowSeq,
		G.guloInvit,
		L.loN,
		'' loFlyers,
		G.guDeposit,
		G.guHotel, 
		G.guRoomNum, 
		G.guPax, 
		dbo.AddString(S.saLastName1, S.saLastName2, ' / ') as saLastName1,
		dbo.AddString(S.saFirstName1, S.saFirstName2, ' / ') as saFirstName1,
		G.guag,
		A.agN,
		G.guco,
		C.coN,
		Cast(0 as bit) guDirect,
		NULL guTimeInT,
		NULL guTimeOutT, 
		NULL guCheckInD,
		NULL guCheckOutD,
		Cast(0 as bit) guResch,
		NULL guReschD,
		Cast(0 as bit) guShow,
		Cast(0 as bit) guTour,
		Cast(0 as bit) guInOut, 
		Cast(0 as bit) guWalkOut,
		Cast(0 as bit) guCTour,
		Cast(0 as bit) guSaveProgram,
		S.saPR1 PR1, 
		P1.peN as PR1N,
		S.saPR2 PR2, 
		P2.peN as PR2N,
		S.saPR3 PR3, 
		P3.peN as PR3N,
		S.saLiner1 Liner1, 
		L1.peN as Liner1N,
		S.saLiner2 Liner2, 
		L2.peN as Liner2N,
		S.saCloser1 Closer1, 
		C1.peN as Closer1N,
		S.saCloser2 Closer2, 
		C2.peN as Closer2N,
		S.saCloser3 Closer3,
		C3.peN as Closer3N,
		S.saExit1 Exit1, 
		E1.peN as Exit1N,
		S.saExit2 Exit2, 
		E2.peN as Exit2N,
		'' guEntryHost,
		'' guWComments,
		NULL guBookD,		
		G.guShowD,
		NULL guInvitD,
		G.guDepSale,
		NULL guDepositSaleNum,
		NULL guDepositSaleD,
		S.sagu,
		S.sast,
		ST.ststc,
		S.saMembershipNum,
		S.saOriginalAmount, 
	    Case when s.sacancelD is not null and s.sacanceld  between @DateFrom and @DateTo then
		S.saGrossAmount * -1 else S.saGrossAmount end saGrossAmount,
		S.saNewAmount, 
		S.saD,
		S.saProcD,
		S.saCancelD,
		S.saClosingCost,
		S.saComments,
		S.saProcRD,
		S.saDownPaymentPercentage / 100 as saDownPaymentPercentage,
		(S.saDownPaymentPercentage / 100) * S.saGrossAmount as saDownPayment,
		S.saDownPaymentPaidPercentage / 100 as saDownPaymentPaidPercentage,
		(S.saDownPaymentPaidPercentage / 100)* S.saGrossAmount as saDownPaymentPaid,
		dbo.UFN_OR_GetShowProgram(G.guSaveProgram, G.guCTour, 0, SR.srAppointment) as ShowProgram,
		NULL gaShow,
		Case when s.sacancelD is not null and s.sacanceld  between @DateFrom and @DateTo then
		-1 else 1 end as CnxSale,
		S.sasr,
		S.salo
	from Sales S
		left join SaleTypes ST on ST.stID = S.sast
		left join Guests G on G.guID = S.sagu
		left join Personnel P1 on P1.peID = S.saPR1
		left join Personnel P2 on P2.peID = S.saPR2
		left join Personnel P3 on P3.peID = S.saPR3
		left join Personnel L1 on L1.peID = S.saLiner1
		left join Personnel L2 on L2.peID = S.saLiner2
		left join Personnel C1 on C1.peID = S.saCloser1
		left join Personnel C2 on C2.peID = S.saCloser2
		left join Personnel C3 on C3.peID = S.saCloser3
		left join Personnel E1 on E1.peID = S.saExit1
		left join Personnel E2 on E2.peID = S.saExit2
		left join Locations L on L.loID = S.salo
		left join SalesRooms SR on SR.srID = S.sasr
		left join Agencies A on A.agID = G.guag
		left join Countries C on C.coID = G.guco
    left join #Manifest m ON m.guid = S.sagu AND m.gashow is not null
	where
		-- No shows
		(((G.guShowD is null or not(G.guShowD between @DateFrom and @DateTo))
		-- Fecha de venta
		and (S.saD between @DateFrom and @DateTo 
		-- Fecha de venta procesable
		or S.saProcD between @DateFrom and @DateTo
		-- Fecha de cancelacion
		or S.saCancelD between @DateFrom and @DateTo)) 
		--Considera los Bumps y Regen del dia de hoy
		--*20070702 EJZ Incluir ventas de otra sala del mismo dia
		or ((S.sast in ('BUMP', 'REGEN') or G.gusr <> S.sasr) and S.saD between @DateFrom and @DateTo))
		-- Sala de ventas
		and (@SalesRoom = 'ALL' or S.sasr in (select item from split(@SalesRoom, ',')))
    --Que no esten en el manifesto
    and m.guid is null
    
) as D
	inner join ShowPrograms SH on D.ShowProgram = SH.skID

-- ================================================
--			Reporte Manifest Range By LS
-- ================================================

SELECT
	SaleType,
	SaleTypeN,
	DateManifest,
	guID = CASE 
		WHEN sagu IS NOT NULL THEN sagu
		ELSE guID END,
	Location= CASE 
		WHEN SaleType=0 OR SaleType=1 OR SaleType=2 THEN
			CASE loFlyers
				WHEN 1 THEN 
				CASE
					WHEN guDeposit = 0 THEN guloInvit + 'F'
				ELSE
					guloInvit
				END
			ELSE
				guloInvit
			END
		WHEN SaleType=12 THEN
			guloInvit
		ELSE
			salo
		END,			
	LocationN = CASE 
		WHEN SaleType = 0 OR SaleType = 1 OR SaleType = 2 THEN
			CASE loFlyers
				WHEN 1 THEN 
				CASE
					WHEN guDeposit = 0 THEN loN + ' FLYERS'
					ELSE
						loN
					END
				ELSE
				loN
			END
		WHEN SaleType > 2 THEN
			loN
		END,
	ShowProgramN,
	SalesRoom = CASE 
		WHEN SaleType = 0 OR SaleType = 1 OR SaleType = 2 THEN gusr
		ELSE sasr END,
	guls LeadSource,
	Sequency = CASE 
		WHEN SaleType = 0 OR SaleType = 1 OR SaleType = 2 THEN guShowSeq end,
	guHotel Hotel,
	guRoomNum Room,
	Pax = CASE 
		WHEN SaleType = 0 OR SaleType = 1 OR SaleType = 2 THEN guPax end,
	guLastName1 LastName,
	guFirstName1 FirstName,
	guag Agency,
	agN AgencyN,
	guShowD ShowD,
	guco Country,
	coN CountryD,
	TimeInT = CASE 
		WHEN SaleType = 0 OR SaleType = 1 OR SaleType = 2 OR SaleType = 12 THEN guTimeInT
		else NULL end,
	TimeOutT = CASE 
		WHEN SaleType = 0 OR SaleType = 1 OR SaleType = 2 OR SaleType = 12 THEN guTimeOutT
		else NULL end,
	guTour Tour,
	guInOut IO,
	guWalkOut WO,
	guCTour CTour,
	guSaveProgram STour,
	guDirect Direct,
	Resch = Cast(CASE
		WHEN guResch=1 and (guReschD BETWEEN @DateFrom AND @DateTo) THEN 1
		ELSE 0 END as bit),
	--Tours = CASE 
	--	WHEN guTour=1 OR guWalkOut=1 OR (guCtour=1 OR guSaveProgram=1) AND sagu IS NOT NULL THEN 1
	--	ELSE 0 END,
	--guShow Shows,
	--RealShows = CASE
	--	WHEN guTour=1 OR guInOut=1 OR guWalkOut=1 or guCTour=1 OR guSaveProgram=1 THEN 1
	--	ELSE 0 END,
	PR1,
	PR1N,
	PR2,
	PR2N,
	PR3,
	PR3N,
	Liner1,
	Liner1N,
	Liner2,
	Liner2N,
	Closer1,
	Closer1N,
	Closer2,
	Closer2N,
	Closer3,
	Closer3N,
	Exit1,
	Exit1N,
	Exit2,
	Exit2N,
	Hostess = CASE 
		WHEN SaleType = 0 OR SaleType = 1 OR SaleType = 2 OR SaleType = 12 THEN guEntryHost
		ELSE NULL END,
	ProcSales=Cast(CASE
		WHEN sagu IS NOT NULL 
		AND saProcD IS NOT NULL
		AND (saProcD BETWEEN @DateFrom AND @DateTo) 
		AND (saCancelD IS NULL OR saCancelD NOT BETWEEN @DateFrom AND @DateTo) THEN 1
		else 0
		end as int),
	ProcOriginal=CASE
		WHEN sagu IS NOT NULL 
		AND saProcD IS NOT NULL
		AND (saProcD BETWEEN @DateFrom AND @DateTo) 
		AND (saCancelD IS NULL OR saCancelD NOT BETWEEN @DateFrom AND @DateTo) THEN saOriginalAmount
		end,
	ProcNew=CASE
		WHEN sagu IS NOT NULL 
		AND saProcD IS NOT NULL
		AND (saProcD BETWEEN @DateFrom AND @DateTo) 
		AND (saCancelD IS NULL OR saCancelD NOT BETWEEN @DateFrom AND @DateTo) THEN saNewAmount
		end,
	ProcGross=CASE
		WHEN sagu IS NOT NULL 
		AND saProcD IS NOT NULL
		AND (saProcD BETWEEN @DateFrom AND @DateTo) 
		AND (saCancelD IS NULL OR saCancelD NOT BETWEEN @DateFrom AND @DateTo) THEN saGrossAmount
		end	,
	PendSales=CAST(CASE
		WHEN sagu IS NOT NULL 
		AND ((saProcD NOT BETWEEN @DateFrom AND @DateTo) OR (saProcD IS NULL))
		AND (saCancelD IS NULL OR saCancelD NOT BETWEEN @DateFrom AND @DateTo) THEN 1
		else 0
		end as int),
	PendOriginal=CASE
		WHEN sagu IS NOT NULL 
		AND ((saProcD NOT BETWEEN @DateFrom AND @DateTo) OR (saProcD IS NULL))
		AND (saCancelD IS NULL OR saCancelD NOT BETWEEN @DateFrom AND @DateTo) THEN saOriginalAmount
		end,
	PendNew=CASE
		WHEN sagu IS NOT NULL 
		AND ((saProcD NOT BETWEEN @DateFrom AND @DateTo) OR (saProcD IS NULL))
		AND (saCancelD IS NULL OR saCancelD NOT BETWEEN @DateFrom AND @DateTo) THEN saNewAmount
		end,
	PendGross=CASE
		WHEN sagu IS NOT NULL 
		AND ((saProcD NOT BETWEEN @DateFrom AND @DateTo) OR (saProcD IS NULL))
		AND (saCancelD IS NULL OR saCancelD NOT BETWEEN @DateFrom AND @DateTo) THEN saGrossAmount
		end,
	DepositSale = CASE
		WHEN guDepositSaleD BETWEEN @DateFrom AND @DateTo THEN guDepSale
		ELSE NULL END,
	DepositSaleNum = CASE
		WHEN guDepositSaleD BETWEEN @DateFrom AND @DateTo THEN guDepositSaleNum
		ELSE NULL END,
	--saMembership Membership,
	CC = CASE 
		WHEN SaleType = 0 OR SaleType = 1 OR SaleType = 2 OR SaleType = 12 THEN (select STUFF((
             SELECT ',' + (CASE 
              WHEN gdQuantity > 1 THEN Convert(varchar(10),gdQuantity) ELSE '' END)+ ' ' + gdcc
             FROM GuestsCreditCards
             WHERE gdgu=(CASE 
		WHEN sagu IS NOT NULL THEN sagu
		ELSE guID END)
             FOR XML PATH('')
       ),1,1,''))
		ELSE NULL END,
	Comments = CASE 
		WHEN SaleType = 0 OR SaleType = 1 OR SaleType = 2  OR SaleType = 12 THEN guWComments
		ELSE saComments END,
	saMembershipNum,
	guShow Show,
	saDownPaymentPercentage,
	saDownPayment,
	saDownPaymentPaidPercentage,
	saDownPaymentPaid
FROM( 
SELECT * FROM #Manifest
UNION ALL
SELECT * FROM #DepositSales
UNION ALL 
SELECT * FROM #OtherSales
) RptManifest
ORDER BY SaleType, Location, ShowProgramN, Sequency, TimeInt, LastName

-- ================================================
-- 					BOOKINGS
-- ================================================
SELECT
 guloInvit,
 LocationN,
 guBookT,
 Count(guBookT) Bookings
 FROM(
select 
	case when G.guDeposit = 0 and L.loFlyers = 1 then G.guloInvit + 'F' else G.guloInvit end as guloInvit,
	case when G.guDeposit = 0 and L.loFlyers = 1 then loN + ' FLYERS' else loN end as LocationN,
	case
		 when CONVERT(VARCHAR(8),G.guBookT,108) <= '09:29:59' then '08:30'
		 when CONVERT(VARCHAR(8),G.guBookT,108) between '09:30:00' and '10:29:59' then '09:30'
		 when CONVERT(VARCHAR(8),G.guBookT,108) between '10:30:00' and '11:29:59' then '10:30'
		 when CONVERT(VARCHAR(8),G.guBookT,108) between '11:30:00' and '12:29:59' then '11:30'
		 when CONVERT(VARCHAR(8),G.guBookT,108) >= '12:30:00' then '12:30'
		 else NULL 
	end guBookT
from Guests G
	inner join Locations L on G.guloInvit = L.loID
where
	-- Fecha de booking
	G.guBookD between @DateFrom and @DateTo
	-- Sala de ventas
	and (@SalesRoom = 'ALL' or G.gusr in (select item from split(@SalesRoom, ',')))
	-- No directas
	and G.guDirect = 0
	-- No reschedules
	and (G.guReschD is null or not(G.guReschD between @DateFrom and @DateTo))
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- No bookings cancelados
	and G.guBookCanc = 0
	) Bookings
GROUP BY guloInvit,LocationN,guBookT
order by guloInvit, guBookT

GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptManifestRange]    Script Date: 09/21/2016 16:59:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de manifiesto por rango de fechas
**		1. Shows
**		2. Ventas
**
** [edgrodriguez]	27/May/2016 Created
** [edgrodriguez]	05/Jul/2016 Modified Se corrigio una validacion al separar los procAmounts.
*/
CREATE procedure [dbo].[USP_IM_RptManifestRange]
	@DateFrom Datetime,				-- Fecha desde
	@DateTo Datetime,				-- Fecha hasta
	@SalesRoom varchar(8000) = 'ALL',	-- Claves de las salas de ventas
	@Program varchar(10) = 'ALL'	-- Clave del programa
as
set fmtonly off;
set nocount on

-- ================================================
-- 						SHOWS
-- ================================================
SELECT
	CASE 
		WHEN G.guShowD IS NULL THEN gadd.gaShow 
		else G.guShowD 
	END DateManifest,
	0 SaleType,
	'MANIFEST' SaleTypeN,
	G.guID,
	G.gusr,
	isnull(G.guloInvit,gadd.gaLoc) as Location,
	G.guHotel,
	G.guRoomNum,
	G.guPax,
	G.guLastName1 LastName,
	G.guFirstName1 FirstName,
	G.guag,
	A.agN,
	G.guco,
	C.coN,
	G.guShowD,
	G.guTimeInT,
	G.guTimeOutT,	
	G.guCheckInD,
	G.guCheckOutD,	
	G.guDirect,
	G.guTour,
	G.guInOut,
	G.guWalkOut,
	G.guCTour,
	G.guSaveProgram,	
	-- PRs
	G.guPRInvit1 as PR1,
	P1.peN as PR1N,
	G.guPRInvit2 as PR2,
	P2.peN as PR2N,
	G.guPRInvit3 as PR3,
	P3.peN as PR3N,
	-- Liners
	G.guLiner1 as Liner1,
	L1.peN as Liner1N,
	G.guLiner2 as Liner2,
	L2.peN as Liner2N,
	-- Closers
	G.guCloser1 as Closer1,
	C1.peN as Closer1N,
	G.guCloser2 as Closer2,
	C2.peN as Closer2N,
	G.guCloser3 as Closer3,
	C3.peN as Closer3N,
	-- Exits
	G.guExit1 as Exit1,
	E1.peN as Exit1N,
	G.guExit2 as Exit2,
	E2.peN as Exit2N,	
	G.guEntryHost,
	
	S.saOriginalAmount,
	S.saNewAmount,
	S.saGrossAmount,

	S.saClosingCost,
	S.saMembershipNum,
	G.guWcomments Comments, 
	G.guBookD,
	G.guInvitD,
	-- Campos de la tabla de ventas
	S.sagu,
	S.sast,
	ST.ststc,
	S.saD,
	S.saProcD,
	S.saCancelD,
	s.saByPhone,
	G.guDepSale,	
	gadd.gaShow,
	S.sasr
	into #Manifest
from Guests G
	left join Sales S on S.sagu = G.guID
	left join SaleTypes ST on ST.stID = S.sast
	left join LeadSources L on L.lsID = G.guls
	left join Personnel P1 on P1.peID = G.guPRInvit1
	left join Personnel P2 on P2.peID = G.guPRInvit2
	left join Personnel P3 on P3.peID = G.guPRInvit3
	left join Personnel L1 on L1.peID = G.guLiner1
	left join Personnel L2 on L2.peID = G.guLiner2
	left join Personnel C1 on C1.peID = G.guCloser1
	left join Personnel C2 on C2.peID = G.guCloser2
	left join Personnel C3 on C3.peID = G.guCloser3
	left join Personnel E1 on E1.peID = G.guExit1
	left join Personnel E2 on E2.peID = G.guExit2
	left join Agencies A on A.agID = G.guag
	left join Countries C on C.coID = G.guco  
  OUTER APPLY (SELECT 
                  CASE WHEN isnull(ga.gagu,0) > 0 AND g.gushowD is null THEN 
                    g2.guShowD 
                  ELSE
                    NULL
                  END [gaShow],
                  g2.guloInvit [gaLoc]
               FROM dbo.GuestsAdditional ga
               INNER JOIN dbo.Guests g2 ON g2.guID = ga.gagu
               WHERE ga.gaAdditional = g.guid
               AND (@SalesRoom = 'ALL' or g2.gusr in (select item from split(@SalesRoom, ',')))
              ) gadd
where 
	-- Fecha de show
	ISNULL(gadd.gashow,G.guShowD) between @DateFrom and @DateTo
	-- Sala de ventas
	and (@SalesRoom = 'ALL' or G.gusr in (select item from split(@SalesRoom, ',')))
	-- Programa
	and (@Program = 'ALL' or lspg = @Program)
	--Ventas por Sala
	and (@SalesRoom = 'ALL' or S.sasr in (select item from split(@SalesRoom, ',')))
order by G.guShowD, S.sagu


-- ================================================
-- 						VENTAS
-- ================================================
select	
	CASE
		WHEN S.saCancelD IS NOT NULL THEN S.saCancelD
		ELSE
		S.saProcD
	END DateManifest,
	dbo.UFN_IM_GetSaleType(@DateFrom,@DateTo,S.sast,ST.ststc,G.guDepSale,S.saD,S.saProcD,S.saCancelD,G.gusr,S.sasr,S.saByPhone) SaleType,
	CASE 
		dbo.UFN_IM_GetSaleType(@DateFrom,@DateTo,S.sast,ST.ststc,G.guDepSale,S.saD,S.saProcD,S.saCancelD,G.gusr,S.sasr,S.saByPhone)
		WHEN 3 THEN 'BE BACKS'
		WHEN 4 THEN 'UPGRADES'
		WHEN 5 THEN 'DOWNGRADES'
		WHEN 6 THEN 'OUT OF PENDING'
		WHEN 7 THEN 'CANCELATION'
		WHEN 8 THEN 'BUMP'
		WHEN 9 THEN 'REGEN'
		WHEN 10 THEN 'DEPOSIT BEFORE'
		WHEN 11 THEN 'PHONE SALES'
		WHEN 13 THEN 'SALES FROM OTHER SALES ROOMS TOURS'
		WHEN 14 THEN 'UNIFICATIONS'
	END SaleTypeN,	
	G.guID,
	G.gusr,
	S.salo as Location,	
	G.guHotel,
	G.guRoomNum,
	G.guPax,
	S.saLastName1 as LastName,
	S.saFirstName1 as FirstName,
	G.guag,
	A.agN,
	G.guco,
	C.coN,
	G.guShowD,
	NULL guTimeInT,
	NULL guTimeOutT,
	NULL guCheckInD,
	NULL guCheckOutD,
	NULL guDirect,
	NULL guTour,
	NULL guInOut,
	NULL guWalkOut,
	NULL guCTour,
	NULL guSaveProgram,
	--PRs
	S.saPR1 as PR1,
	P1.peN as PR1N,
	S.saPR2 as PR2,
	P2.peN as PR2N,
	S.saPR3 as PR3,
	P3.peN as PR3N,
	--Liners
	S.saLiner1 as Liner1,
	L1.peN as Liner1N,
	S.saLiner2 as Liner2,
	L2.peN as Liner2N,
	--Closers
	S.saCloser1 as Closer1,
	C1.peN as Closer1N,
	S.saCloser2 as Closer2,
	C2.peN as Closer2N,
	S.saCloser3 as Closer3,
	C3.peN as Closer3N,
	--Exits
	S.saExit1 as Exit1,
	E1.peN as Exit1N,
	S.saExit2 as Exit2,
	E2.peN as Exit2N,	
	'' guEntryHost,
	S.saOriginalAmount,
	S.saNewAmount,
	S.saGrossAmount,
	S.saClosingCost,
	S.saMembershipNum,	
	S.saComments as Comments,
	G.guBookD,
	G.guInvitD,
	S.sagu,
	S.sast,
	ST.ststc,	
	S.saD,
	S.saProcD,
	S.saCancelD,
	S.saByPhone,
	--Campos de la tabla de Guests
	G.guDepSale,	
	NULL gashow,
	S.sasr
	into #SalesManifest
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
	left join LeadSources L on L.lsID = S.sals
	left join Personnel P1 on P1.peID = S.saPR1
	left join Personnel P2 on P2.peID = S.saPR2
	left join Personnel P3 on P3.peID = S.saPR3
	left join Personnel L1 on L1.peID = S.saLiner1
	left join Personnel L2 on L2.peID = S.saLiner2
	left join Personnel C1 on C1.peID = S.saCloser1
	left join Personnel C2 on C2.peID = S.saCloser2
	left join Personnel C3 on C3.peID = S.saCloser3
	left join Personnel E1 on E1.peID = S.saExit1
	left join Personnel E2 on E2.peID = S.saExit2
	left join Agencies A on A.agID = G.guag
	left join Countries C on C.coID = G.guco
	left join #Manifest m ON m.guid = S.sagu AND m.gashow is not null
where
	 --Fecha de venta
	(((G.guShowD is null or G.guShowD <> S.saD) and (S.saD between @DateFrom and @DateTo))
	 --Fecha de venta procesable
	or ((G.guShowD is null or G.guShowD <> S.saProcD) and (S.saProcD between @DateFrom and @DateTo))
	 --Fecha de cancelacion
	or ((G.guShowD is null or G.guShowD <> S.saCancelD) and (S.saCancelD between @DateFrom and @DateTo)))
	 --Solo se consideran ventas procesables o no canceladas
	and (not S.saProcD is null or S.saCancelD is null)
	 --Sala de ventas
	and (@SalesRoom = 'ALL' or S.sasr in (select item from split(@SalesRoom, ',')))
	 --Programa
	and (@Program = 'ALL' or lspg = @Program)
	and m.guid is null

-- ================================================
-- 						Reporte
-- ================================================

SELECT
	DateManifest,
	SaleType,
	SaleTypeN,
	CASE 
		WHEN sagu IS NOT NULL THEN sagu
		ELSE guID
	end guID,
	CASE SALETYPE
		WHEN 0 THEN gusr
		ELSE
		sasr 
	END SalesRoom,
	Location,
	guHotel Hotel,
	guRoomNum Room,
	guPax Pax,
	LastName,
	FirstName,
	guag Agency,
	agN AgencyN,
	guco Country,
	coN CountryN,
	guShowD showD,
	guTimeInT TimeInT,
	guTimeOutT TimeOutT,
	guCheckInD CheckIn,
	guCheckOutD CheckOut,
	guDirect Direct,
	guTour Tour,
	guInOut InOut,
	guWalkOut WalkOut,
	guCTour CTour,
	guSaveProgram SaveTour,
	PR1,
	PR1N,
	PR2,
	PR2N,
	PR3,
	PR3N,
	Liner1,
	Liner1N,
	Liner2,
	Liner2N,
	Closer1,
	Closer1N,
	Closer2,
	Closer2N,
	Closer3,
	Closer3N,
	Exit1,
	Exit1N,
	Exit2,
	Exit2N,
	guEntryHost Hostess,
	NULL ProcSales,
	CASE
		WHEN sagu IS NOT NULL 
		AND saProcD IS NOT NULL
		and (saProcD BETWEEN @DateFrom AND @DateTo) 
		AND (saCancelD IS NULL OR saCancelD NOT BETWEEN @DateFrom AND @DateTo) THEN saOriginalAmount
		end
	ProcOriginal,
	CASE
		WHEN sagu IS NOT NULL 
		AND saProcD IS NOT NULL
		and (saProcD BETWEEN @DateFrom AND @DateTo) 
		AND (saCancelD IS NULL OR saCancelD NOT BETWEEN @DateFrom AND @DateTo) THEN saNewAmount
		end
	ProcNew,
	CASE
		WHEN sagu IS NOT NULL 
		AND saProcD IS NOT NULL
		and (saProcD BETWEEN @DateFrom AND @DateTo) 
		AND (saCancelD IS NULL OR saCancelD NOT BETWEEN @DateFrom AND @DateTo) THEN saGrossAmount
		end
	ProcGross,
	NULL PendSales,
	CASE
		WHEN sagu IS NOT NULL 
		AND ((saProcD NOT BETWEEN @DateFrom AND @DateTo) or saProcD IS NULL)
		AND (saCancelD IS NULL or saCancelD NOT BETWEEN @DateFrom AND @DateTo) THEN saOriginalAmount
		end
	PendOriginal,
	CASE
		WHEN sagu IS NOT NULL 
		AND ((saProcD NOT BETWEEN @DateFrom AND @DateTo) or saProcD IS NULL)
		AND (saCancelD IS NULL or saCancelD NOT BETWEEN @DateFrom AND @DateTo) THEN saNewAmount
		end
	PendNew,
	CASE
		WHEN sagu IS NOT NULL 
		AND ((saProcD NOT BETWEEN @DateFrom AND @DateTo) or saProcD IS NULL)
		AND (saCancelD IS NULL or saCancelD NOT BETWEEN @DateFrom AND @DateTo) THEN saGrossAmount
		end
	PendGross,
	saClosingCost ClosingCost,
	saMembershipNum MemberShip,
	Comments	
FROM
(
SELECT * FROM #Manifest
UNION ALL
SELECT * FROM #SalesManifest
)AS RptManifest
order by SaleType,SalesRoom, showD, TimeInT,TimeOutT,LastName
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByFlightSalesRoom]    Script Date: 09/21/2016 16:59:30 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByHotel]    Script Date: 09/21/2016 16:59:33 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByHotelGroup]    Script Date: 09/21/2016 16:59:36 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByHotelGroupSalesRoom]    Script Date: 09/21/2016 16:59:39 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByHotelSalesRoom]    Script Date: 09/21/2016 16:59:42 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByWave]    Script Date: 09/21/2016 16:59:45 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByWaveSalesRoom]    Script Date: 09/21/2016 16:59:48 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_IM_RptWeeklyGiftsItemsSimple]    Script Date: 09/21/2016 16:59:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene la cantidad de regalos por semana por Sala de ventas.
** 
** [edgrodriguez]	08/Abr/2016 Creado
**
*/
CREATE procedure [dbo].[USP_IM_RptWeeklyGiftsItemsSimple]
	@StartDate as datetime,
	@EndDate as datetime,
	@SalesRooms as varchar(8000)= 'ALL'
	as
	set nocount on
	select 
		grD,
		giN	as Gift,
		giShortN as ShortN,
		sum(geQty) as Qty
	from GiftsReceipts inner join GiftsReceiptsC on grID=gegr
		inner join Gifts on gegi = giID
	where
		(@SalesRooms = 'ALL' or grsr in (select item from split(@SalesRooms, ',')))
		and grD between @StartDate and @EndDate
		and gigc='ITEMS'
		and grCancel=0
		and (grct<> 'PR'
		and grct<> 'LINER'
		and grct<> 'CLOSER')
	group by grD, giN,giShortN
	order by giN
	
	select 
		cast(case when srGiftsRcptCloseD >= @EndDate then 1 else 0 end as bit) as GiftsClosed  
	from 
		SalesRooms 
	where
	(@SalesRooms = 'ALL' or srID in (select item from split(@SalesRooms, ',')))
	
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptWeeklyMonthlyHostess]    Script Date: 09/21/2016 16:59:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte Semanal y Mensual de Hostess
** 
** [edgrodriguez]	20/May/2016 Created
**
*/
CREATE PROCEDURE [dbo].[USP_IM_RptWeeklyMonthlyHostess]
	@DateFrom as DateTime,			-- Fecha desde
	@DateTo as DateTime,			-- Fecha hasta
	@SalesRooms as varchar(8000)	-- Claves de las salas de ventas
as
set nocount on

Declare 
	@dtmFirsDate as datetime, --Primer dia del mes
	@dtmLastDate as datetime --Ultimo dia del mes
	set @dtmFirsDate = dateadd(d, day(@DateFrom) * (-1) + 1, @DateFrom)
	set @dtmLastDate = dateadd(m, 1, @dtmFirsDate) - 1
	-----------------------------------------------------------------------
	--Weekly & Monthly Report (Directs, InOut, WalkOuts
	-----------------------------------------------------------------------
select 
		guls,
		guloInvit,
		guDirect,
		guPRInvit1,
		guPRInvit2,
		guPRInvit3,
		pe1.peN as guPRInvit1N, 
		pe2.peN as guPRInvit2N, 
		pe3.peN as guPRInvit3N, 
		guShowD,
		guInOut,
		guWalkOut,
		----------
		gusr,
		----------
		guTimeInT
	into #Guests 
	from 
		Guests
		inner join Personnel pe1 on guPRInvit1 = pe1.peID 
		left outer join Personnel pe2 on guPRInvit2 = pe2.peID 
		left outer join Personnel pe3 on guPRInvit3 = pe3.peID 
	where 
		(@SalesRooms = 'ALL' or gusr in (select item from Split(@SalesRooms, ',')))
		and (guShowD between @dtmFirsDate and @dtmLastDate)
	order by 
		guPRInvit1
		
SELECT [Type],guls,guloInvit,Sum(guDirect) guDirect,sum(guInOut) guInOut,sum(guWalkOut) guWalkOut, guPRInvit guPRInvit,guPRInvitN guPRInvitN,gusr  
FROM
(		
	Select 'Weekly' [Type],guls,guloInvit,Sum(Convert(int,guDirect)) guDirect,sum(Convert(int,guInOut)) guInOut,sum(Convert(int,guWalkOut)) guWalkOut, guPRInvit1 guPRInvit,guPRInvit1N guPRInvitN,gusr 
	from #Guests
	WHERE
	guPRInvit1 IS NOT NULL
	and (guShowD between @DateFrom and @DateTo)
	GROUP by guPRInvit1,guPRInvit1n,guls,guloInvit,guShowD,gusr
union ALL
	Select 'Weekly' [Type],guls,guloInvit,Sum(Convert(int,guDirect)) guDirect,sum(Convert(int,guInOut)) guInOut,sum(Convert(int,guWalkOut)) guWalkOut, guPRInvit2 guPRInvit,guPRInvit2N guPRInvitN,gusr 
	from #Guests
	WHERE
	guPRInvit2 IS NOT NULL
	and (guShowD between @DateFrom and @DateTo)
	GROUP by guPRInvit2,guPRInvit2n,guls,guloInvit,guShowD,gusr
union all
	Select 'Weekly' [Type],guls,guloInvit,Sum(Convert(int,guDirect)) guDirect,sum(Convert(int,guInOut)) guInOut,sum(Convert(int,guWalkOut)) guWalkOut, guPRInvit3 guPRInvit,guPRInvit3N guPRInvitN,gusr 
	from #Guests
	WHERE
	guPRInvit3 IS NOT NULL
	and (guShowD between @DateFrom and @DateTo)
	GROUP by guPRInvit3,guPRInvit3n,guls,guloInvit,guShowD,gusr
union all
	Select 'Monthly' [Type],guls,guloInvit,Sum(Convert(int,guDirect)) guDirect,sum(Convert(int,guInOut)) guInOut,sum(Convert(int,guWalkOut)) guWalkOut, guPRInvit1 guPRInvit,guPRInvit1N guPRInvitN,gusr 
	from #Guests
	WHERE
	guPRInvit1 IS NOT NULL
	GROUP by guPRInvit1,guPRInvit1n,guls,guloInvit,guShowD,gusr
union ALL
	Select 'Monthly' [Type],guls,guloInvit,Sum(Convert(int,guDirect)) guDirect,sum(Convert(int,guInOut)) guInOut,sum(Convert(int,guWalkOut)) guWalkOut, guPRInvit2 guPRInvit,guPRInvit2N guPRInvitN,gusr 
	from #Guests
	WHERE
	guPRInvit2 IS NOT NULL
	GROUP by guPRInvit2,guPRInvit2n,guls,guloInvit,guShowD,gusr
union all
	Select 'Monthly' [Type],guls,guloInvit,Sum(Convert(int,guDirect)) guDirect,sum(Convert(int,guInOut)) guInOut,sum(Convert(int,guWalkOut)) guWalkOut, guPRInvit3 guPRInvit,guPRInvit3N guPRInvitN,gusr 
	from #Guests
	WHERE
	guPRInvit3 IS NOT NULL
	GROUP by guPRInvit3,guPRInvit3n,guls,guloInvit,guShowD,gusr
) Gu
group by GU.guPRInvit,GU.guPRInvitN,GU.guls,[Type],GU.gusr, GU.guloInvit
order by Gu.guls

	-----------------------------------------------------------------------
	--Weekly Times (Bookings, Shows, Directs)
	-----------------------------------------------------------------------

	-- Bookings
	select 
		guls,
		guloInvit,
		guBookD,
		guDirect, 
		case
		 when CONVERT(VARCHAR(8),guBookT,108) <= '09:29:59' then '08:30'
		 when CONVERT(VARCHAR(8),guBookT,108) between '09:30:00' and '10:29:59' then '09:30'
		 when CONVERT(VARCHAR(8),guBookT,108) between '10:30:00' and '11:29:59' then '10:30'
		 when CONVERT(VARCHAR(8),guBookT,108) between '11:30:00' and '12:29:59' then '11:30'
		 when CONVERT(VARCHAR(8),guBookT,108) >= '12:30:00' then '12:30'
		 else NULL end guBookT
		into #Bookings 
	FROM 
		Guests  INNER JOIN Leadsources ls ON guls = lsID
	WHERE 
		(@SalesRooms = 'ALL' or gusr in (select item from Split(@SalesRooms, ',')))
		AND guBookD BETWEEN @DateFrom AND @DateTo
		AND (lspg = 'IH' OR (lspg = 'OUT' AND guDeposit <> 0)) --No considerar invit. outside sin depósito
		AND (guReschD IS NULL OR guReschD <> guBookD) --No considere Reschedules
		AND guAntesIO = 0

	-- Shows
	SELECT 
		guls,
		guloInvit,
		guShowD,
		case
		 when CONVERT(VARCHAR(8),guTimeInT,108) <= '09:29:59' then '08:30'
		 when CONVERT(VARCHAR(8),guTimeInT,108) between '09:30:00' and '10:29:59' then '09:30'
		 when CONVERT(VARCHAR(8),guTimeInT,108) between '10:30:00' and '11:29:59' then '10:30'
		 when CONVERT(VARCHAR(8),guTimeInT,108) between '11:30:00' and '12:29:59' then '11:30'
		 when CONVERT(VARCHAR(8),guTimeInT,108) >= '12:30:00' then '12:30'
		 else NULL end guTimeInT
		into #Shows 
	FROM 
		Guests INNER JOIN Leadsources ls ON guls = lsID
	WHERE 
		(@SalesRooms = 'ALL' or gusr in (select item from Split(@SalesRooms, ',')))
		AND guShowD BETWEEN @DateFrom AND @DateTo
		AND (guTour=1 OR guInOut=1 OR guWalkOut=1)


SELECT
 guls, guD, [Time], sum(guBook) guBook, sum(guShow) guShow, sum(guDirect)guDirect
FROM
(
-- Bookings
select guls, guBookD guD, guBookT [Time], Count(guBookD) - Sum(Cast(guDirect as int)) guBook, 0 guShow, Sum(Cast(guDirect as int)) guDirect
from #Bookings
GROUP BY guls, guBookD, guBookT
-- Shows
union all
select guls, guShowD, guTimeInT, 0, Count(guShowD), 0
from #Shows
GROUP BY guls, guShowD, guTimeInT
-- Total de Bookings
UNION ALL
select guls, guBookD, 'Total', Count(guBookD) - Sum(Cast(guDirect as int)), 0, Sum(Cast(guDirect as int))
from #Bookings
GROUP BY guls, guBookD
-- Total de Shows
union all
select guls, guShowD, 'Total', 0, Count(guShowD), 0
from #Shows
GROUP BY guls, guShowD
) HostessTime 
group by  guls, guD, [Time]
Order by guls,guD,[Time]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetCxC]    Script Date: 09/21/2016 16:59:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene las CxC cargadas a los PR
** 
** [wtorres]	02/Jul/2010 Creado
** [lchairez]	21/Dic/2013 Agregue los campos de Autorizado por, Monto pagado, motivo de pago incompleto y Log
** [lchairez]	08/Ene/2014 Agregue los campos de CxC de regalos, CxC de deposito y CxC de taxi de salida
** [wtorres]	17/Feb/2014 Correccion del calculo del CxC de taxi de salida, faltaba multiplicarlo por el tipo de cambio de su moneda
**							Agregue la columna de Lead Source
**
*/
create procedure [dbo].[USP_OR_GetCxC]
	@Authorized bit,		-- Indica si se desean las CxC autorizadas
	@SalesRoom varchar(10)	-- Clave de la sala de ventas
as
set nocount on

-- Recibos de regalos
select
	R.grID,
	R.grNum,
	R.grls,
	R.grgu,
	R.grGuest,
	R.grD,
	R.grpe,
	PR.peN,
	R.grCxCGifts + R.grCxCAdj as grCxCGifts,
	(R.grCxCPRDeposit * IsNull(ED.exExchRate, 1)) as grCxCPRDeposit,
	(R.grCxCTaxiOut * IsNull(ET.exExchRate, 1)) as grCxCTaxiOut,
	-- CxC = CxC de regalos (Cargo + Ajuste) + CxC del deposito + CxC del taxi de salida
	(R.grCxCGifts + R.grCxCAdj) + (R.grCxCPRDeposit * IsNull(ED.exExchRate, 1)) + (R.grCxCTaxiOut * IsNull(ET.exExchRate, 1)) as CxC,
	@Authorized as Authorized,
	R.grCxCAppD,
	R.grAuthorizedBy,
	PA.peN as grAuthorizedName,
	R.grAmountToPay as grAmountPaid,
	R.grup,
	R.grcxcAuthComments,
	R.grcxcComments,
	'' as [Log]
from GiftsReceipts R
	left join SalesRooms S on R.grsr = S.srID
	left join Personnel PR on R.grpe = PR.peID
	left join Personnel PA on R.grAuthorizedBy = PA.peID
	left join ExchangeRate ED on ED.exD = R.grD and ED.excu = R.grcuCxCPRDeposit
	left join ExchangeRate ET on ET.exD = R.grD and ET.excu = R.grcuCxCTaxiOut
where
	-- Autorizadas
    ((@Authorized = 1 and R.grCxCAppD is not null
		-- Con fecha de recibo mayor a la ultima fecha de cierre de CxC de la sala de ventas
		and R.grD > S.srCxCCloseD)
	-- No autorizadas
	or (@Authorized = 0 and R.grCxCAppD is null))
	-- Sala de ventas
	and R.grsr = @SalesRoom
	-- Tengan cargo (de regalos, de deposito o de taxi de salida)
    and ((grCxCGifts + grCxCAdj <> 0) or grCxCPRDeposit > 0 or grCxCTaxiOut > 0)
order by PR.peN, R.grD

-- Fecha de cierre de CxC
select srCxCCloseD from SalesRooms where srID = @SalesRoom

GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptCxC]    Script Date: 09/21/2016 16:59:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de CxC
** 
** [wtorres]	28/Feb/2013 Depurado
** [wtorres]	19/Feb/2014 Agregue el campo de CxC de taxi de salida y elimine los campos de hotel y locacion
** [lchairez]	27/Mar/2014 Se cambia nombre de campo "Total CxC US" por "Total CxC", Se elimina la columna "Total CxC MN"
**							Se agregan 2 columnas nuevas CxC Paid US y CxC Paid MN
*/
create procedure [dbo].[USP_OR_RptCxC]
	@DateFrom datetime,		-- Fecha desde
	@DateTo datetime,		-- Fecha hasta
	@SalesRoom varchar(10)	-- Clave de sala de ventas
as
set nocount on

select
	-- separamos los vales de los chargebacks
	case when R.grNum LIKE '%VALE%' then Cast('VALE' as varchar(11)) else Cast('CHARGE BACK' as varchar(11)) end as grGroup,
	R.grpe,
	P.peN,
	R.grID,
	R.grNum,	
	R.grD,
	R.grgu,
	R.grGuest,
	IsNull(D.geQty,0) as geQty,
	Cast(G.giN as varchar(50)) as giN,
	IsNull(D.geAdults, 0) as geAdults,
	IsNull(D.geMinors, 0) as geMinors,
	D.geFolios,
	IsNull(D.gePriceA + D.gePriceM, 0) as TotalGift,
	-- CxC de regalos
	R.grCxCGifts,
	R.grCxCAdj,
	-- CxC de deposito
	R.grCxCPRDeposit,
	R.grcuCxCPRDeposit,
	IsNull(ED.exExchRate, 1) as ExchRateDeposit,
	R.grCxCPRDeposit * IsNull(ED.exExchRate, 1) as CxCDepositUS,
	R.grCxCPRDeposit * IsNull(ED.exExchRate, 1) / IsNull(ES.exExchRate, 1) as CxCDepositMN,
	-- CxC de taxi de salida
	R.grCxCTaxiOut,
	R.grcuCxCTaxiOut,
	IsNull(ET.exExchRate, 1) as ExchRateTaxiOut,
	R.grCxCTaxiOut * IsNull(ET.exExchRate, 1) as CxCTaxiOutUS,
	R.grCxCTaxiOut * IsNull(ET.exExchRate, 1) / IsNull(ES.exExchRate, 1) as CxCTaxiOutMN,
	-- Total de CxC
	R.grCxCGifts + R.grCxCAdj + R.grCxCPRDeposit * IsNull(ED.exExchRate, 1) + R.grCxCTaxiOut * IsNull(ET.exExchRate, 1) as TotalCxC,
	COALESCE(R.grAmountToPay,0) as CxCPaidUS,
	COALESCE(R.grAmountToPay,0) / COALESCE(E.exExchRate,1)CxCPaidMN,
	-- Tipo de cambio de la sala de ventas
	IsNull(ES.exExchRate, 1) as ExchRateSalesRoom,
	R.grCxCComments,
	R.grComments
from GiftsReceipts R
	left join GiftsReceiptsC D on D.gegr  = R.grID
	left join Gifts G on G.giID  = D.gegi
	left join Personnel P on P.peID = R.grpe
	left join SalesRooms S on S.srID = R.grsr
	left join ExchangeRate ES on ES.exD = R.grD and ES.excu = S.srcu
	left join ExchangeRate ED on ED.exD = R.grD and ED.excu = R.grcuCxCPRDeposit
	left join ExchangeRate ET on ET.exD = R.grD and ET.excu = R.grcuCxCTaxiOut
	left join ExchangeRate E on E.exD = R.grD and E.excu = 'MEX'
where
	-- Fecha de autorizacion de CxC
	R.grCxCAppD between @DateFrom and @DateTo
	-- Sala de ventas
	and R.grsr = @SalesRoom
	-- No considerar los recibos cancelados
	and R.grCancel = 0
	-- Tengan CxC (de regalos, de deposito o de taxi de salida)
	and (R.grCxCGifts + R.grCxCAdj <> 0 or R.grCxCPRDeposit > 0 or R.grCxCTaxiOut > 0)
order by R.grNum, grGroup, R.grpe, R.grID, R.grgu

-- indicamos si la sala de ventas ya hizo el cierre de CxC para la fecha indicada
select Cast(case when srCxCCloseD >= @DateTo then 1 else 0 end as bit) as SalesRoomClosed
from SalesRooms
where srID = @SalesRoom

GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptCxCExcel]    Script Date: 09/21/2016 16:59:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de CxC por tipo (regalos, depositos y taxis)
** 
** [wtorres]		20/Feb/2014 Modified. Depurado
** [lchairez]		27/Mar/2014 Modified. Se agregan 3 columnas nuevas "Total CxC", "CxC Paid US" y "CxC Paid MN"
**
*/
CREATE procedure [dbo].[USP_OR_RptCxCExcel]
	@DateFrom datetime,		-- Fecha desde
	@DateTo datetime,		-- Fecha hasta
	@SalesRoom varchar(10)	-- Clave de sala de ventas
as
set fmtonly off
set nocount on

-- CxC de regalos
-- ===================================
select
	R.grpe,
	P.peN,
	R.grD,
	R.grNum,
	R.grsr,
	Cast('REGALOS' as varchar(20)) as Comments,
	R.grCxCGifts + R.grCxCAdj as CxC,
	IsNull(E.exExchRate, 1) as exExchRate,
	(R.grCxCGifts + R.grCxCAdj) / IsNull(E.exExchRate, 1) as CxCP,
	R.grCxCGifts + R.grCxCAdj + R.grCxCPRDeposit * IsNull(ED.exExchRate, 1) + R.grCxCTaxiOut * IsNull(ET.exExchRate, 1) as TotalCxC,
	COALESCE(R.grAmountToPay,0) as CxCPaidUS,
	COALESCE(R.grAmountToPay,0) / COALESCE(E.exExchRate,1)CxCPaidMN
into #Report
from GiftsReceipts R
	inner join Personnel P on P.peID = R.grpe
	left join ExchangeRate ED on ED.exD = R.grD and ED.excu = R.grcuCxCPRDeposit
	left join ExchangeRate E on E.exD = R.grD and E.excu = 'MEX'
	left join ExchangeRate ET on ET.exD = R.grD and ET.excu = R.grcuCxCTaxiOut
where
	-- Sala de ventas
	R.grsr = @SalesRoom
	-- Fecha de autorizacion de CxC
	and R.grCxCAppD between @DateFrom and @DateTo
	-- No considerar los recibos cancelados
	and R.grCancel = 0
	-- Tengan CxC de regalos
	and R.grCxCGifts + R.grCxCAdj > 0
order by R.grpe

-- CxC de taxis
-- ===================================
insert into #Report
select
	R.grpe,
	P.peN,
	R.grD,
	R.grNum,
	R.grsr,
	'TAXIS' as Comments,
	R.grTaxiOutDiff as CxC,
	0 as exExchRate,
	R.grTaxiOutDiff as CxCP,
	0 as TotalCxC,
	COALESCE(R.grAmountToPay,0) as CxCPaidUS,
	0 AS CxCPaidMN
from GiftsReceipts R
	left join Personnel P on P.peID = R.grpe
where
	-- Sala de ventas
	R.grsr = @SalesRoom
	-- Fecha de autorizacion de CxC
	and R.grCxCAppD between @DateFrom and @DateTo 
	-- No considerar los recibos cancelados
	and R.grCancel = 0
	-- Tengan CxC de deposito igual a la diferencia del taxi de salida
	and R.grCxCPRDeposit > 0 and R.grCxCPRDeposit = R.grTaxiOutDiff
order by R.grpe

-- CxC de depositos
-- ===================================
insert into #Report
select
	R.grpe,
	P.peN,
	R.grD,
	R.grNum,
	R.grsr,
	'DEPOSITOS' as Comments,
	R.grCxCPRDeposit * IsNull(ED.exExchRate, 1) as CxC,
	IsNull(E.exExchRate, 1) as exExchRate,
	R.grCxCPRDeposit * IsNull(ED.exExchRate, 1) / IsNull(E.exExchRate, 1) as CxCP,
	R.grCxCGifts + R.grCxCAdj + R.grCxCPRDeposit * IsNull(ED.exExchRate, 1) + R.grCxCTaxiOut * IsNull(ET.exExchRate, 1) as TotalCxC,
	COALESCE(R.grAmountToPay,0) as CxCPaidUS,
	COALESCE(R.grAmountToPay,0) / COALESCE(E.exExchRate,1)CxCPaidMN
from GiftsReceipts R
	left join Personnel P on P.peID = R.grpe
	left join ExchangeRate ED on ED.exD = R.grD and ED.excu = R.grcuCxCPRDeposit
	left join ExchangeRate E on E.exD = R.grD and E.excu = 'MEX'
	left join ExchangeRate ET on ET.exD = R.grD and ET.excu = R.grcuCxCTaxiOut
where
	-- Sala de ventas
	R.grsr = @SalesRoom
	-- Fecha de autorizacion de CxC
	and R.grCxCAppD between @DateFrom and @DateTo
	-- No considerar los recibos cancelados
	and R.grCancel = 0
	-- Tengan CxC de deposito diferente a la diferencia del taxi de salida
	and R.grCxCPRDeposit > 0 and R.grCxCPRDeposit <> R.grTaxiOutDiff
order by R.grpe

-- devolvemos los datos del reporte
select * from #Report order by grpe, grD, Comments

DROP TABLE #Report

GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptFoliosInvitationByDateFolio]    Script Date: 09/21/2016 17:00:01 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_OR_RptGifts]    Script Date: 09/21/2016 17:00:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene informacion de gifts para el reporte.
** 
** [edgrodriguez] 16/Mar/2016 Created. 
**
*/
CREATE PROCEDURE [dbo].[USP_OR_RptGifts]	
as
set nocount on

	Select giID, giN, giShortN, giO, giPrice1, giPrice2, giPrice3, giPrice4, giPack, gcN, giInven, giWFolio, giWPax, giA 
	from Gifts inner join GiftsCategs on gigc = gcID

GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptProductionReferral]    Script Date: 09/21/2016 17:00:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion de referidos
** Los referidos son aquellas invitaciones con código de contrato (guO1) igual a REFREE-7 para 2007, REFREE-8 para 2008, etc
**
** [wtorres] 	08/Jul/2009 Created
** [wtorres] 	19/Nov/2013 Modified. Reemplace el uso de las funciones UFN_OR_GetMonthArrivalsByContract, UFN_OR_GetMonthArrivalsByContractLike,
**							UFN_OR_GetMonthShowsByContract, UFN_OR_GetMonthShowsByContractLike, UFN_OR_GetMonthSalesByContract,
**							UFN_OR_GetMonthSalesByContractLike, UFN_OR_GetMonthSalesAmountByContract y UFN_OR_GetMonthSalesAmountByContractLike
**							por UFN_OR_GetMonthArrivals, UFN_OR_GetMonthShows, UFN_OR_GetMonthSales y UFN_OR_GetMonthSalesAmount
** [wtorres] 	19/Feb/2015 Modified. Ahora contempla los huespedes que tengan Rate Code que empiece con CLRF, CLRP, RCIR, REF99 y CLR9.
**							Antes solo tomaba exactamente los Rate Codes CLRF, CLRP y RCIR
**
*/
CREATE procedure [dbo].[USP_OR_RptProductionReferral]
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime				-- Fecha hasta
as
set nocount on

declare @Contracts varchar(8000)

set @Contracts = 'REFREE%,CLRF%,CLRP%,RCIR%,REF99%,CLR9%'

select
	-- Nombre del mes
	DateName(Month, dbo.DateSerial([Year], [Month], 1)) as MonthN,
	-- Mes
	[Month],
	-- Año
	[Year],
	-- Llegadas
	Sum(Arrivals) as Arrivals,
	-- Shows
	Sum(Shows) as Shows,
	-- Porcentaje de shows
	[dbo].UFN_OR_SecureDivision(Sum(Shows), Sum(Arrivals)) as ShowsFactor,
	-- Ventas
	Sum(Sales) as Sales,
	-- Monto de ventas
	Sum(SalesAmount) as SalesAmount,
	-- Porcentaje de cierre
	[dbo].UFN_OR_SecureDivision(Sum(Sales), Sum(Shows)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(SalesAmount), Sum(Shows)) as Efficiency,
	-- Venta promedio
	[dbo].UFN_OR_SecureDivision(Sum(SalesAmount), Sum(Sales)) as AverageSale
from (
	-- Llegadas
	select [Year], [Month], Arrivals, 0 as Shows, 0 as Sales, 0 as SalesAmount
	from UFN_OR_GetMonthArrivals(@DateFrom, @DateTo, default, default, default, default, default, default, default, @Contracts, default)
	-- Shows
	union all
	select [Year], [Month], 0, Shows, 0, 0
	from UFN_OR_GetMonthShows(@DateFrom, @DateTo, default, default, default, default, default, default, default, default, default, @Contracts, default, default)
	-- Numero de ventas
	union all
	select [Year], [Month], 0, 0, Sales, 0
	from UFN_OR_GetMonthSales(@DateFrom, @DateTo, default, default, default, default, default, default, @Contracts, default, default)
	-- Monto de ventas
	union all
	select [Year], [Month], 0, 0, 0, SalesAmount
	from UFN_OR_GetMonthSalesAmount	(@DateFrom, @DateTo, default, default, default, default, default, default, @Contracts, default, default)
) as D
group by [Year], [Month]
order by [Year], [Month]



GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptPRStats]    Script Date: 09/21/2016 17:00:07 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_OR_SaveGiftsReceiptLog]    Script Date: 09/21/2016 17:00:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Agrega un registro en el historico de un recibo de regalos si su informacion relevante cambio
** 
** [wtorres]	01/Jun/2010 Optimizacion
** [alesanchez] 09/Nov/2013 Agregue campo de Forma de pago
** [axperez]	20/Dic/2013 Agregue campo de Motivo de Reimpresion y Contador de Reimpresiones
** [lchairez]	21/Dic/2013 Se omite parametro de @TotalGifts para que se calcule desde aquí
** [LoreMartinez] 13/Jul/2015 Se cambia la columna grAmountPaid por grAmountToPaid
**
*/
create procedure [dbo].[USP_OR_SaveGiftsReceiptLog]
	@Receipt int,			-- Clave del recibo de regalos
	@HoursDif smallint,		-- Horas de diferencia
	@ChangedBy varchar(10)	-- Clave del usuario que esta haciendo el cambio
as
set nocount on

declare
	@Count int,
	@TotalGifts money

-- determinamos si cambio algun campo relevante
select @Count = Count(*)
from GiftsReceiptsLog
	inner join GiftsReceipts on gogr = grID
where
	gogr = @Receipt
	and (goD = grD or (goD is null and grD is null))
	and (goHost = grHost or (goHost is null and grHost is null))
	and (goDeposit = grDeposit or (goDeposit is null and grDeposit is null))
	and (goBurned = grDepositTwisted or (goBurned is null and grDepositTwisted is null))
	and (gocu = grcu or (gocu is null and grcu is null))
	and (goCXCPRDeposit = grcxcPRDeposit or (goCXCPRDeposit is null and grcxcPRDeposit is null))
	and (goTaxiOut = grTaxiOut or (goTaxiOut is null and grTaxiOut is null))
	and (goct = grct or (goct is null and grct is null))
	and (gope = grpe or (gope is null and grpe is null))
	and (goCXCGifts = grcxcGifts or (goCXCGifts is null and grcxcGifts is null))
	and (goCXCAdj = grcxcAdj or (goCXCAdj is null and grcxcAdj is null))
	and (goTaxiOutDiff = grTaxiOutDiff or (goTaxiOutDiff is null and grTaxiOutDiff is null))
	and (gopt = grpt or (gopt is null and grpt is null))
	and (goReimpresion = grReimpresion or (goReimpresion is null and grReimpresion is null))
	and (gorm = grrm or (gorm is null and grrm is null))
	and (goAuthorizedBy = grAuthorizedBy or (goAuthorizedBy is null and grAuthorizedBy is null))
	and (goAmountPaid = grAmountToPay or (goAmountPaid is null and grAmountToPay is null))
	and (goup = grup or (goup is null and grup is null))
	and goID in (select Max(goID) from GiftsReceiptsLog where gogr = @Receipt)
	and (goCancelD = grCancelD or (goCancelD is null and grCancelD is null))

-- obtenemos el costo total de los regalos del recibo
select @TotalGifts = IsNull(Sum(gePriceA + gePriceM), 0) from GiftsReceiptsC where gegr = @Receipt

-- agregamos un registro en el historico, si cambio algun campo relevante
insert into GiftsReceiptsLog
select
	DateAdd(hh, @HoursDif, GetDate()),
	grID,
	grD,
	grHost,
	grDeposit,
	grDepositTwisted,
	grcu,
	grCXCPRDeposit,
	grTaxiOut,
	@TotalGifts,
	grct,
	grpe,
	grCXCGifts,
	grCXCAdj,
	grTaxiOutDiff,
	@ChangedBy,
	grpt,
	grReimpresion,
	grrm,
	grAuthorizedBy,
	grAmountToPay,
	grup,
	grCancelD
from GiftsReceipts
where grID = @Receipt and @Count = 0

GO


