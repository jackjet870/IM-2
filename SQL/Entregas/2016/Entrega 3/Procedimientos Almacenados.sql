USE [OrigosVCPalace]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_ADDEfficiencySalesmen]    Script Date: 09/22/2016 19:12:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_ADDEfficiencySalesmen]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_ADDEfficiencySalesmen]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_DeleteEfficiencySalesmen]    Script Date: 09/22/2016 19:12:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_DeleteEfficiencySalesmen]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_DeleteEfficiencySalesmen]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_GetEfficiencyByWeeks]    Script Date: 09/22/2016 19:12:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_GetEfficiencyByWeeks]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_GetEfficiencyByWeeks]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptCloserStatistics]    Script Date: 09/22/2016 19:12:55 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptCloserStatistics]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptCloserStatistics]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptConcentrateDailySales]    Script Date: 09/22/2016 19:12:56 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptConcentrateDailySales]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptConcentrateDailySales]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptDailyGiftSimple]    Script Date: 09/22/2016 19:12:56 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptDailyGiftSimple]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptDailyGiftSimple]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptEfficiencyWeekly]    Script Date: 09/22/2016 19:12:57 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptEfficiencyWeekly]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptEfficiencyWeekly]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptFTMInOutHouse]    Script Date: 09/22/2016 19:12:57 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptFTMInOutHouse]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptFTMInOutHouse]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptGiftsKardex]    Script Date: 09/22/2016 19:12:58 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptGiftsKardex]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptGiftsKardex]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptLinerStatistics]    Script Date: 09/22/2016 19:12:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptLinerStatistics]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptLinerStatistics]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptLoginLog]    Script Date: 09/22/2016 19:12:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptLoginLog]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptLoginLog]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptManifest]    Script Date: 09/22/2016 19:13:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptManifest]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptManifest]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptManifestByLSRange]    Script Date: 09/22/2016 19:13:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptManifestByLSRange]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptManifestByLSRange]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptManifestRange]    Script Date: 09/22/2016 19:13:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptManifestRange]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptManifestRange]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByFlightSalesRoom]    Script Date: 09/22/2016 19:13:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptProductionByFlightSalesRoom]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptProductionByFlightSalesRoom]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByHotel]    Script Date: 09/22/2016 19:13:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptProductionByHotel]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptProductionByHotel]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByHotelGroup]    Script Date: 09/22/2016 19:13:03 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptProductionByHotelGroup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptProductionByHotelGroup]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByHotelGroupSalesRoom]    Script Date: 09/22/2016 19:13:03 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptProductionByHotelGroupSalesRoom]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptProductionByHotelGroupSalesRoom]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByHotelSalesRoom]    Script Date: 09/22/2016 19:13:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptProductionByHotelSalesRoom]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptProductionByHotelSalesRoom]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByWave]    Script Date: 09/22/2016 19:13:05 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptProductionByWave]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptProductionByWave]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByWaveSalesRoom]    Script Date: 09/22/2016 19:13:05 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptProductionByWaveSalesRoom]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptProductionByWaveSalesRoom]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptSelfGenTeam]    Script Date: 09/22/2016 19:13:06 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptSelfGenTeam]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptSelfGenTeam]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptStatisticsByCloser]    Script Date: 09/22/2016 19:13:06 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptStatisticsByCloser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptStatisticsByCloser]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptStatisticsByExitCloser]    Script Date: 09/22/2016 19:13:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptStatisticsByExitCloser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptStatisticsByExitCloser]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptStatisticsByFTB]    Script Date: 09/22/2016 19:13:08 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptStatisticsByFTB]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptStatisticsByFTB]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptStatisticsByFTBCategories]    Script Date: 09/22/2016 19:13:08 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptStatisticsByFTBCategories]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptStatisticsByFTBCategories]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptStatisticsByFTBLocations]    Script Date: 09/22/2016 19:13:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptStatisticsByFTBLocations]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptStatisticsByFTBLocations]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptStatisticsBySegments]    Script Date: 09/22/2016 19:13:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptStatisticsBySegments]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptStatisticsBySegments]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptWeeklyGiftsItemsSimple]    Script Date: 09/22/2016 19:13:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptWeeklyGiftsItemsSimple]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptWeeklyGiftsItemsSimple]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptWeeklyMonthlyHostess]    Script Date: 09/22/2016 19:13:11 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptWeeklyMonthlyHostess]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptWeeklyMonthlyHostess]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetCxC]    Script Date: 09/22/2016 19:13:11 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_GetCxC]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_GetCxC]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptCxC]    Script Date: 09/22/2016 19:13:12 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptCxC]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptCxC]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptCxCExcel]    Script Date: 09/22/2016 19:13:12 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptCxCExcel]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptCxCExcel]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptDailySalesDetail]    Script Date: 09/22/2016 19:13:13 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptDailySalesDetail]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptDailySalesDetail]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptDailySalesHeader]    Script Date: 09/22/2016 19:13:14 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptDailySalesHeader]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptDailySalesHeader]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptFoliosInvitationByDateFolio]    Script Date: 09/22/2016 19:13:14 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptFoliosInvitationByDateFolio]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptFoliosInvitationByDateFolio]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptGifts]    Script Date: 09/22/2016 19:13:15 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptGifts]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptGifts]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptManifestByLS]    Script Date: 09/22/2016 19:13:15 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptManifestByLS]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptManifestByLS]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptManifestByLSRange]    Script Date: 09/22/2016 19:13:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptManifestByLSRange]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptManifestByLSRange]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptManifestRange]    Script Date: 09/22/2016 19:13:17 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptManifestRange]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptManifestRange]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptProductionReferral]    Script Date: 09/22/2016 19:13:17 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptProductionReferral]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptProductionReferral]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptPRStats]    Script Date: 09/22/2016 19:13:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptPRStats]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptPRStats]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_SaveGiftsReceiptLog]    Script Date: 09/22/2016 19:13:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_SaveGiftsReceiptLog]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_SaveGiftsReceiptLog]
GO

USE [OrigosVCPalace]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_ADDEfficiencySalesmen]    Script Date: 09/22/2016 19:13:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Agrega a la tabla de EfficiencySalesmen
**
** [ecanul] 18/08/2016 Created
**
*/

CREATE PROCEDURE [dbo].[USP_IM_ADDEfficiencySalesmen]
	@EfficiencyID int,		--ID de la eficiencya
	@SalemanID varchar(10)	-- ID del Personal a agregar
AS
INSERT INTO dbo.EfficiencySalesmen VALUES(@EfficiencyID,@SalemanID)
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_DeleteEfficiencySalesmen]    Script Date: 09/22/2016 19:13:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Elimina los vendedores que tengan la eficiencia seleccionada
**
** [ecanul] 18/08/2016 Created
**
*/
CREATE PROCEDURE [dbo].[USP_IM_DeleteEfficiencySalesmen]
	@EfficiencyID int		--ID de la eficiencia a eliminar
AS
DELETE FROM dbo.EfficiencySalesmen 
WHERE esef = @EfficiencyID
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_GetEfficiencyByWeeks]    Script Date: 09/22/2016 19:13:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve un listado con fechas de Efficiency que esten dentro del mes seleccionado
**
** [ecanul] 25/07/2016 Created
**
*/

CREATE PROCEDURE [dbo].[USP_IM_GetEfficiencyByWeeks]
		@SalesRoom varchar(5),			-- Sala de ventas
		@DateFrom datetime,				-- Fecha Inicio
		@DateTo datetime				-- Fecha Fin
AS
SELECT DISTINCT CAST(1 as BIT)AS Include, efDateFrom, efDateTo
FROM dbo.Efficiency
WHERE 
	--Sala de ventas
	efsr = @SalesRoom
	-- Año y mes inicial
	AND ( (YEAR(efDateFrom) = YEAR(@DateFrom) AND MONTH(efDateFrom) = MONTH(@DateFrom))
	-- Año y mes Final
	OR (YEAR(efDateTo) = YEAR(@DateTo) AND MONTH(efDateTo) = MONTH(@DateTo)))
	-- fecha final no sea mayor que la fecha final cerrada
	AND efDateTo < @DateTo
ORDER BY efDateFrom
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptCloserStatistics]    Script Date: 09/22/2016 19:13:25 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_IM_RptConcentrateDailySales]    Script Date: 09/22/2016 19:13:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/* 
** Devuelve los datos para el reporte Concentrate Daily Sales
**	1. SalesRoom
**	2. UPS 	(SHOWS)
**	3. Sales (Total Sales)
**	4. SalesAmountOPP Monto de ventas Out Of Pending
**	5. SalesAmountFall Ventas Canceladas
**	6. SalesAmountCancel Monto de ventas canceladas
**	7. SalesAmount Monto de ventas
**	8. DownPac DownPayment
**	9. DownColl DownPayment Collected

** [ecanul] 12/05/2016 Created
*/


Create procedure [dbo].[USP_IM_RptConcentrateDailySales]
 @DateFrom datetime,			-- Fecha inicial
 @DateTo datetime,				-- Fecha fin
 @ListSalesRoom varchar(Max)	-- Listado de sales Room
as 

set nocount on
--Variables
DECLARE @Pos int;					-- Posicion del contador
DECLARE @len int;					-- Largo de la cadena
DECLARE @SalesRoom varchar(50);		-- SalesRoom
DECLARE @UPS int;					-- Shows
DECLARE @Sales INT;					-- Total Sales
DECLARE @Amount money;				-- Sales Amount
DECLARE @OPP money;					--				OPP
DECLARE @SACancel money;			--				Cancel
DECLARE @Fall money;				-- Ventas canceladas FALL
DECLARE @DownPact money;			-- DownPayment
DECLARE	@DownColl money;			-- DownPayment Collected
DECLARE @DownPactFac money;			-- DownPayment Factor
DECLARE	@DownCollFac money;			-- DownPayment Collected Factor

-- Tabla temporal
DECLARE @ConcentrateTable table(
	SalesRoom varchar(300),
	UPS int,
	Sales int,
	SalesAmountOPP money,
	SalesAmountFall money,
	SalesAmountCancel money,
	SalesAmount money,
	DownPact money,
	DownColl money
	)
--Las variables aqui declaradas deben de ser los parametros recibidos
/*SET @DateFrom = '2016/05/04';
SET @DateTo = GETDATE();
SET @ListSalesRoom = 'MPS,MP';
*/
-- Asignacion de variabes segun se necesiten
SET @ListSalesRoom = @ListSalesRoom + ',';
SET @len = 0;
SET @Pos= 0;
--Llenado de la tabla
WHILE CHARINDEX(',',@ListSalesRoom,@Pos+1)>0
BEGIN
	set @len = CHARINDEX(',', @ListSalesRoom, @pos+1) - @pos;
	SET @SalesRoom = SUBSTRING(@ListSalesRoom, @pos, @len)
	
	--Shows
	SET @UPS = dbo.UFN_OR_GetShows(@DateFrom, @DateTo, default, @SalesRoom, default, 4, default, default)
	
	-----------Total SALES
	-- Numero de ventas regulares
	Declare @REG INT 
	SET @REG = dbo.UFN_OR_GetSales(@DateFrom, @DateTo, default, @SalesRoom, 'REG', default, default, default)
	-- Numero de ventas exit
	DECLARE @EXIT INT
	SET @EXIT =dbo.UFN_OR_GetSales(@DateFrom, @DateTo, default, @SalesRoom, 'EXIT', default, default, default)
	-- Numero de ventas VIP
	DECLARE @VIP INT
	SET @VIP = dbo.UFN_OR_GetSales(@DateFrom, @DateTo, default, @SalesRoom, 'VIP,DIAMOND', default, default, default)
	--Total Sales
	SET @Sales = @REG + @EXIT + @VIP
	-------- SALES
		
	-- Monto de ventas Out Of Pending -- OPP
	SET @OPP = dbo.UFN_OR_GetSalesAmount(@DateFrom, @DateTo, default, @SalesRoom, default, 1, default, default)
	-- Ventas Canceladas -- FALL
	SET @Fall =	dbo.UFN_OR_GetCnxSalesAmount(@DateFrom, @DateTo,@SalesRoom)
	-- Monto de ventas canceladas -Cancel
	SET @SACancel = dbo.UFN_OR_GetSalesAmount(@DateFrom, @DateTo, default, @SalesRoom, default, default, 1, default)
	-- Monto de ventas SalesAmount
	SET @Amount = dbo.UFN_OR_GetSalesAmount(@DateFrom, @DateTo, default, @SalesRoom, default, default, default, default)
	
	SET @DownCollFac = 0.0;
	SET @DownPactFac = 0.0;	
	
	--DownPayment ---
	SET @DownPact = dbo.UFN_OR_GetSalesDownPayment(@DateFrom, @DateTo,@SalesRoom,0)
	--SET @DownPact Factor
	SET @DownPactFac = dbo.UFN_OR_SecureDivision(@DownPact/1.1,@Amount)
	
    --DownPayment Collected---
    Set @DownColl = dbo.UFN_OR_GetSalesDownPayment(@DateFrom, @DateTo,@SalesRoom,1)	
	--SET @DownPact Factor	
	SET @DownCollFac =  dbo.UFN_OR_SecureDivision(@DownColl/1.1,@Amount)
	
	--SalesRoom Name al final para poder hacer los valores en tiempo
	SET @SalesRoom = (SELECT srN FROM SalesRooms WHERE srId = @SalesRoom)	
		
	INSERT INTO @ConcentrateTable VALUES
	(@SalesRoom,@UPS,@Sales,@OPP,@Fall,@SACancel,@Amount,@DownPactFac,@DownCollFac)
	
	--Incrementa el contado 
	set @pos = CHARINDEX(',', @ListSalesRoom, @pos+@len) +1
	
END
--Selecr
SELECT * from @ConcentrateTable
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptDailyGiftSimple]    Script Date: 09/22/2016 19:13:29 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_IM_RptEfficiencyWeekly]    Script Date: 09/22/2016 19:13:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de Efficiency Weekly
**
** [ecanul] 13/08/2016 Created
**
*/
CREATE PROCEDURE [dbo].[USP_IM_RptEfficiencyWeekly]
	@SalesRoom varchar(10),			--Sala de Ventas
	@DatesFrom varchar(MAX),		--Fechas desde
	@DatesTo varchar(MAX)			--Fechas hasta
AS

DECLARE @DatesFromTable TABLE(id INT IDENTITY(1,1),DateFrom DateTime);
DECLARE @DatesToTable TABLE(id INT IDENTITY(1,1),DateTo DateTime);
DECLARE @DateFrom DateTime,
		@DateTo DateTime;
DECLARE @Count int;

INSERT INTO @DatesFromTable(DateFrom) SELECT CONVERT(DATETIME,item) FROM dbo.Split(@DatesFrom,',');
INSERT INTO @DatesToTable(DateTo) SELECT CONVERT(DATETIME,item) FROM dbo.Split(@DatesTo,',');

DECLARE @Report TABLE(
	SalemanID varchar(10),
	SalemanName varchar(40),
	EfficiencyType varchar(80)
);


--======================================================================================
--================================		STATS BY FTB	================================
--======================================================================================

DECLARE @StatsByFTB table (
	SalemanID varchar(10),
	SalemanName varchar(40),
	PostName varchar(50),
	Team varchar(85),
	SalesmanStatus VARCHAR(10),
	--OWN
	OwnAmount money,
	OwnOPP int,
	OwnUPS money,	
	OwnRegular money,
	OwnExit money,
	OwnSales money,	
	OwnEfficiency money,
	OwnClosingFactor money,
	OwnSaleAverage money,
	--WITH
	WithAmount money,
	WithOPP int,
	WithUPS money,	
	WithRegular money,
	WithExit money,
	WithSales money,	
	WithEfficiency money,
	WithClosingFactor money,
	WithSaleAverage money,
	--TOTAL
	TotalAmount money,
	TotalOPP int,
	TotalUPS money,	
	TotalRegular money,
	TotalExit money,
	TotalSales money,	
	TotalEfficiency money,
	TotalClosingFactor money,
	TotalSaleAverage money,
	--AS A Closer
	AsAmount money,
	AsOPP int,
	AsUPS money,	
	AsRegular money,
	AsExit money,
	AsSales money,	
	AsEfficiency money,
	AsClosingFactor money,
	AsSaleAverage money
	--assistances int default 0
);

--- Siempre se toma la primera fecha para iniciar el reporte
SET @DateFrom = (SELECT DateFrom FROM @DatesFromTable WHERE id = 1);
SET @DateTo = (SELECT DateTo FROM @DatesToTable WHERE id = 1);

INSERT @StatsByFTB EXEC USP_IM_RptStatisticsByFTB @DateFrom, @DateTo, @SalesRoom
----------------------------------------------------------------------------------------------------
-- =============================== SECCION TotalsSalesRoomFTB ======================================
DECLARE @UPSFTB money;
DECLARE @SalesFTB money;
DECLARE @SalesAmountFTB  money;
DECLARE @SalesEfficiencyFTB money;
DECLARE @SalesClosingFactorFTB money;

SET @UPSFTB = (SELECT SUM(OwnUPS)FROM @StatsByFTB);
SET @SalesFTB = (SELECT SUM(OwnSales + WithSales) FROM @StatsByFTB);
SET @SalesAmountFTB = (SELECT SUM(OwnAmount + WithAmount) FROM @StatsByFTB);

SET @SalesEfficiencyFTB = dbo.UFN_OR_SecureDivision(@SalesAmountFTB, @UPSFTB);
SET @SalesClosingFactorFTB = dbo.UFN_OR_SecureDivision(@SalesFTB, @UPSFTB);
--------------------------------------------------------------------------------
--se disminuye la eficiencia de la sala en un 10%
SET @SalesEfficiencyFTB = @SalesEfficiencyFTB * 0.9;
SET @SalesClosingFactorFTB = @SalesClosingFactorFTB * 0.9;

----------------------------------------------------------------------------------------------------
-- =============================== SECCION CLOSERS =================================================
INSERT INTO @Report (SalemanID, SalemanName, EfficiencyType)
SELECT DISTINCT SalemanID, SalemanName, 'Closers' 
FROM @StatsByFTB 
WHERE 
	-- Todos los que tengan el puesto de Closer o Exit Closer
	PostName IN('Closer','Exit Closer');
------ Si no hubo Closers y Exit Closers se crea uno vacio 
SET @Count = (SELECT COUNT(SalemanID) FROM @Report);
IF @Count = 0 BEGIN
	insert into @Report (SalemanID, SalemanName, EfficiencyType) VALUES('','Nobody','Closer');
	SET @Count = 0;
END
----------------------------------------------------------------------------------------------------
-- =============================== Best FTB previous weeks =========================================
CREATE Table #BestFTBprevWeeks(
	SalemanID varchar(10),
	SalemanName varchar(40),
	);
DECLARE @TDates int;
SET @TDates = (SELECT COUNT(id) FROM @DatesFromTable);
IF @TDates >= 2 BEGIN
	SET @Count = 2;
	WHILE @Count <= @TDates
	BEGIN
		SET @DateFrom = (SELECT DateFrom FROM @DatesFromTable WHERE id = @Count);
		SET @DateTo = (SELECT DateTo FROM @DatesToTable WHERE id = @Count);
		INSERT INTO #BestFTBprevWeeks (SalemanID,SalemanName) SELECT * FROM dbo.UFN_IM_GetBestFTBS(@SalesRoom,@DateFrom,@DateTo);
		SET @Count = @Count + 1;
	END
END
----------------------------------------------------------------------------------------------------
-- =============================== SECCION FTBs ====================================================
------------------ BESTS FTBS ------------------------------------
INSERT INTO @Report (SalemanID, SalemanName, EfficiencyType)
SELECT DISTINCT SalemanID, SalemanName, 'Best Front To Backs' 
FROM @StatsByFTB
WHERE
	-- QUE TENGAN AL MENOS UNA VENTA OWNER
	OwnSales >= 1
	-- Que cubra la eficiencia de la sala menos el 10%
	AND TotalEfficiency >= @SalesEfficiencyFTB 
	-- Que cubra el Closing Factor de la sala menos el 10%
	AND TotalClosingFactor >= @SalesClosingFactorFTB
	-- Que tenga el puesto de Front To Back
	AND PostName = 'Front To Back'
-------- Se agregan los mejores FTBs de la s semanas anteriores
INSERT INTO @Report (SalemanID, SalemanName, EfficiencyType)
SELECT SalemanID, SalemanName, 'Best Front To Backs'
FROM #BestFTBprevWeeks
WHERE --Se agregan los mejores vendedores de las semanas pasadas que no se encuentren ya en el reporte como Best FTB
	SalemanID NOT IN (SELECT SalemanID FROM @Report WHERE EfficiencyType = 'Best Front To Backs')
----Si no hay Best Front To Backs Se agrega uno vacio
SET @Count = (SELECT COUNT(SalemanID) FROM @Report WHERE EfficiencyType = 'Best Front To Backs');
IF @Count <= 0
BEGIN
	INSERT INTO @Report (SalemanID, SalemanName, EfficiencyType) VALUES ('','Nobody','Best Front To Backs');
END
------------------------- Remaining front to backs -----------------
INSERT INTO @Report (SalemanID,SalemanName,EfficiencyType) 
SELECT DISTINCT SalemanID, SalemanName, 'Front To Backs'
FROM @StatsByFTB 
WHERE 
	-- El puesto sea FTB
	PostName = 'Front To Back'
	-- Tengan menos de 1 venta owner o Eficiencya sea menor que el de la sala o que C% sea menor que el de la sala
	AND (OwnSales < 1 OR TotalEfficiency < @SalesEfficiencyFTB OR TotalClosingFactor < @SalesClosingFactorFTB)
	--El vendedor no se encuenre entre los mejores vendedores de las semanas pasadas
	AND SalemanID NOT IN (SELECT SalemanID FROM #BestFTBprevWeeks)
-- Si no hay Front To Backs
SET @Count = (SELECT COUNT(SalemanID) FROM @Report WHERE EfficiencyType = 'Front To Backs');
IF @Count <= 0
BEGIN
	INSERT INTO @Report (SalemanID, SalemanName, EfficiencyType) VALUES ('','Nobody','Front To Backs');
END
----------------------------------------------------------------------------------------------------
-- =============================== SECCION FTBs ====================================================
----------------------- BESTS FTMs
INSERT INTO @Report (SalemanID, SalemanName, EfficiencyType)
SELECT SalemanID, SalemanName, 'Best Front To Backs In Training'
FROM @StatsByFTB
WHERE
	--el puesto sea FTM
	PostName = 'Front To Middle'
	--la Eficiencia sea mayor o igual a la eficiencia de la sala - 10%
	AND TotalEfficiency >= @SalesEfficiencyFTB
	--el Closing Factor sea mayor o igual al C% de la sala - 10%
	AND TotalClosingFactor >= @SalesClosingFactorFTB
-- Si no hubo mejores FTM se agrega uno vacio 
SET @Count = (SELECT COUNT(SalemanID) FROM @Report WHERE EfficiencyType = 'Best Front To Backs In Training')
IF @Count <= 0
BEGIN
	INSERT INTO @Report (SalemanID, SalemanName, EfficiencyType) VALUES ('','Nobody','Best Front To Backs In Training');
END
---------------	FTMs Remaning
INSERT INTO @Report (SalemanID, SalemanName, EfficiencyType)
SELECT SalemanID, SalemanName, 'Front To Backs In Training'
FROM @StatsByFTB
WHERE 
-- Sean FTM
	PostName = 'Front To Middle'
	--Que el La eficiencia sea menor que el de la sala -10%
	AND (TotalEfficiency < @SalesEfficiencyFTB
	-- O que el porcentaje de cierre sea menor que el de la sala -10%
	OR TotalClosingFactor < @SalesClosingFactorFTB)
-- Si no hubo FTMs se agrega uno vacio 
SET @Count = (SELECT COUNT(SalemanID) FROM @Report WHERE EfficiencyType = 'Front To Backs In Training')
IF @Count <= 0
BEGIN
	INSERT INTO @Report (SalemanID, SalemanName, EfficiencyType) VALUES ('','Nobody','Front To Backs In Training');
END
----------------------------------------------------------------------------------------------------
-- =============================== SECCION Liners ====================================================
INSERT INTO @Report (SalemanID, SalemanName, EfficiencyType)
SELECT SalemanID, SalemanName, 'Liners'
FROM @StatsByFTB
WHERE 
	--El puesto sea liner
	PostName = 'Liner'
-- Si no hubo lINER se agrega uno vacio 
SET @Count = (SELECT COUNT(SalemanID) FROM @Report WHERE EfficiencyType = 'Liners')
IF @Count <= 0
BEGIN
	INSERT INTO @Report (SalemanID, SalemanName, EfficiencyType) VALUES ('','Nobody','Liners');
END
----------------------------------------------------------------------------------------------------
-- =============================== Mejor Closer ====================================================
DECLARE @StatsByCloser table (
	SalemanID varchar(10),
	SalemanName varchar(40),
	Team varchar(85),
	SalesmanStatus VARCHAR(10),
	TSalesAmount money,
	TOOP money,
	TUPS money,
	TSalesRegular money,
	TSalesExit money, 
	TSales money, 
	TEfficiency money, 
	TClosingFactor money,
	TSaleAverage money,
	--- Closers
	CSalesAmount money,
	COOP money,
	CUPS money,
	CSalesRegular money,
	CSalesExit money,
	CSales money,
	CEfficiency money,
	CClosingFactor money,
	CSaleAverage money,
	--- As FTB
	AsSalesAmount money,
	AsOOP money,
	AsUPS money,
	AsSalesRegular money,
	AsSalesExit money,
	AsSales money, 
	AsEfficiency money,
	AsClosingFactor money,
	AsSaleAverage money,
	--- With Junior
	WSalesAmount money,
	WOOP money,
	WUPS money,
	WSalesRegular money,
	WSalesExit money,
	WSales money,
	WEfficiency money,
	WClosingFactor money,
	WSaleAverage money,
	--- Tot As FTB and With Junior
	AWSalesAmount money,
	AWOOP money,
	AWUPS money,
	AWSalesRegular money,
	AWSalesExit money,
	AWSales money,
	AWEfficiency money,
	AWClosingFactor money,
	AWSaleAverage money
);
--- se reinician las fechas para escoger las primeras del reporte
SET @DateFrom = (SELECT DateFrom FROM @DatesFromTable WHERE id = 1);
SET @DateTo = (SELECT DateTo FROM @DatesToTable WHERE id = 1);

INSERT @StatsByCloser EXEC USP_IM_RptStatisticsByCloser @DateFrom, @DateTo, @SalesRoom;
SELECT top 1 SalemanID, SalemanName, (CSalesAmount+ WSalesAmount + AsSalesAmount) Total INTO #temp FROM @StatsByCloser ORDER BY Total DESC

INSERT INTO @Report (SalemanID, SalemanName, EfficiencyType)
SELECT SalemanID, SalemanName, 'Best Closer' FROM #temp
SET @Count = (SELECT COUNT(SalemanID) FROM @Report WHERE EfficiencyType = 'Best Closer')
IF @Count <= 0
BEGIN
	INSERT INTO @Report (SalemanID, SalemanName, EfficiencyType) VALUES ('','Nobody','Best Closer');
END
DROP TABLE #temp
----------------------------------------------------------------------------------------------------
-- =============================== Mejor FTB ====================================================
INSERT INTO @Report (SalemanID, SalemanName, EfficiencyType)
SELECT TOP 1 SalemanID, SalemanName, 'Best Front To Back' FROM @StatsByFTB WHERE PostName = 'Front To Back' ORDER BY OwnAmount DESC
SET @Count = (SELECT COUNT(SalemanID) FROM @Report WHERE EfficiencyType = 'Best Front To Back')
IF @Count <= 0
BEGIN
	INSERT INTO @Report (SalemanID, SalemanName, EfficiencyType) VALUES ('','Nobody','Best Front To Back');
END
----------------------------------------------------------------------------------------------------
-- =============================== FTBs Front To Backs que tuvieron mayor porcentaje de cierre que la temporada=========
DECLARE @ClosingF money
SET @ClosingF = (dbo.UFN_IM_GetClosingFactorBySeasons(@DateFrom, @DateTo) / 100)
-- si el pocentaje cierre de la sala no alcanzo al de la temporada
IF @SalesClosingFactorFTB < @ClosingF
BEGIN
	-- se aumenta un 5% por no alcanzar el porcentaje de cierre de la temporada
	SET @ClosingF = @ClosingF + 0.05
END
--- Se crea una subconsulta con los FTBs que tengan un C% >= al C% de la teporada
DECLARE @FTBSeasson TABLE(
	id int identity (1,1),
	SalemanID varchar(10), 
	SalemanName varchar(40), 
	PostName varchar(40),
	TotalClosingFactor money
)
INSERT INTO @FTBSeasson
SELECT SalemanID, SalemanName, PostName, TotalClosingFactor
FROM @StatsByFTB
WHERE
	-- % de cierre sea mayor o igual el porcentaje de la sala
	TotalClosingFactor >= @ClosingF
	-- sea FTB o Closer o Exit
	AND PostName IN ('Front To Back','Closer','Exit Closer')
DECLARE @var int;
SET @var = 1;
SET @Count = (SELECT COUNT(SalemanID) FROM @FTBSeasson)

while @var <= @Count
begin
	DECLARE @nAssistances int
	DECLARE @salemanID varchar(10)
	SET @salemanID = (SELECT SalemanID FROM @FTBSeasson WHERE id = @var)
	SET @nAssistances = dbo.UFN_IM_GetPersonnelAssistancesByWeek(@SalesRoom, @salemanID,@DateFrom,@DateTo)
	--si la asistencia es mayor a 5 se agrega al reporte
	IF @nAssistances >= 5
	begin
		insert into @Report
		SELECT SalemanID,SalemanName, 'Front To Backs with closing factor greater than Season' from @FTBSeasson where id = @var		
	end
	-- si la asistencia es -1 se agrega como asistencia no definida
	ELSE
		INSERT INTO @Report 
		SELECT SalemanID,SalemanName, 'Undefined Assistance' from @FTBSeasson where id = @var
	SET @var = @var +1
end	
	
---- Si no hubo Front To Backs with closing factor greater than Season se agrega uno vacio
SET @Count = (SELECT COUNT(SalemanID) FROM @Report WHERE EfficiencyType = 'Front To Backs with closing factor greater than Season')
IF @Count <= 0
BEGIN
	INSERT INTO @Report (SalemanID, SalemanName, EfficiencyType) VALUES ('','Nobody','Front To Backs with closing factor greater than Season');
END
DROP TABLE #BestFTBprevWeeks

SELECT * FROM @Report
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptFTMInOutHouse]    Script Date: 09/22/2016 19:13:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de F.T.M. In & Out House
** [ecanul] 01/Jul/2016 Created
**
*/
CREATE PROCEDURE [dbo].[USP_IM_RptFTMInOutHouse]
	@DateFrom Datetime,						-- Fecha Inicio
	@DateTo Datetime,						-- Fecha Fin
	@SalesRooms varchar(10),				-- Sala de venta
	@SalesmanID varchar(10) = 'ALL'			-- Clave de un vendedor
AS

DECLARE @Table Table(
	Liner varchar(10),
	--Overflow
	OFSalesAmount money,
	OFShows money,
	OFSales money,
	OFExit money,
	OFTotal money,
	--Regen
	RSalesAmount money,
	RShows money,
	RSales money,
	RExit money,
	RTotal money,
	--Normal
	NSalesAmount money,
	NShows money,
	NSales money,
	NExit money,
	NTotal money,
	--TOTAL
	TSalesAmount money,
	TShows money,
	TSales money,
	TExit money,
	TTotal money
);

--- =============================== LINERS --------------------------------------------

SELECT DISTINCT *
INTO #Liners
FROM (
-- ==============================GUESTS=======================================
-- ==================Guests.Liner1
SELECT G.guLiner1 Liner
FROM dbo.Guests G
WHERE 
	--Fecha Show
	G.guShowD BETWEEN @DateFrom AND @DateTo
	--Sala de ventas
	AND (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
	-- Con Que Tenga Liner 1
	AND G.guLiner1 IS NOT NULL
	-- Tour, Walk Out o (Courtesy Tour, Save Tour con venta)) "QUE SEA SHOW"
	AND (G.guTour = 1 OR G.guWalkOut = 1 OR ((G.guCTour = 1 OR G.guSaveProgram = 1) AND G.guSale = 1))
	-- Que no sea denta depocito de otro dia 
	AND (G.guDepositSaleD IS NULL OR NOT G.guDepositSaleD BETWEEN @DateFrom AND @DateTo)
	--Que sea selfgen marcado por los hostess
	AND (G.guSelfGen = 1
	-- Liner 1 configurado como Front To Middle
	OR G.guLiner1 IN (SELECT peLinerID FROM dbo.Personnel WHERE peLinerID IS NOT NULL))
	-- Liner Especifico
	AND (@SalesmanID = 'ALL' OR G.guLiner1 = @SalesmanID)
UNION ALL
-- ================= Guests.Liner2
SELECT G.guLiner2 Liner
FROM dbo.Guests G
WHERE 
	--Fecha Show
	G.guShowD BETWEEN @DateFrom AND @DateTo
	--Sala de ventas
	AND (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
	-- Con Que Tenga Liner 2
	AND G.guLiner2 IS NOT NULL
	-- Tour, Walk Out o (Courtesy Tour, Save Tour con venta)) "QUE SEA SHOW"
	AND (G.guTour = 1 OR G.guWalkOut = 1 OR ((G.guCTour = 1 OR G.guSaveProgram = 1) AND G.guSale = 1))
	-- Que no sea denta depocito de otro dia 
	AND (G.guDepositSaleD IS NULL OR NOT G.guDepositSaleD BETWEEN @DateFrom AND @DateTo)
	--Que sea selfgen marcado por los hostess
	AND (G.guSelfGen = 1
	-- Liner 1 configurado como Front To Middle
	OR G.guLiner2 IN (SELECT peLinerID FROM dbo.Personnel WHERE peLinerID IS NOT NULL))
	-- Liner Especifico
	AND (@SalesmanID = 'ALL' OR G.guLiner2 = @SalesmanID)
UNION ALL
-- ================================ SALES
-- ==================Sales.Liner1
SELECT S.saLiner1 Liber
FROM dbo.Sales S
WHERE 
	--Fecha de venta
	S.saProcD BETWEEN @DateFrom AND @DateTo
	--Sala de Ventas
	AND (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
	-- Con Liner1
	AND S.saLiner1 IS NOT NULL
	--No canceladas
	AND S.saCancel = 0
	-- Selfgen marcado por los hostess
	AND (S.saSelfGen = 1
	--Liner 1 como Front To Middle
	OR S.saLiner1 IN (SELECT peLinerID FROM dbo.Personnel WHERE peLinerID IS NOT NULL))	
	-- Liner Especifico
	AND (@SalesmanID = 'ALL' OR S.saLiner1 = @SalesmanID)
UNION ALL 
-- =================Sales.Liner2
SELECT S.saLiner2 Liner
FROM dbo.Sales S
WHERE 
	--Fecha de venta
	S.saProcD BETWEEN @DateFrom AND @DateTo
	--Sala de Ventas
	AND (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
	-- Con Liner2
	AND S.saLiner2 IS NOT NULL
	--No canceladas
	AND S.saCancel = 0
	-- Selfgen marcado por los hostess
	AND (S.saSelfGen = 1
	--Liner 1 como Front To Middle
	OR S.saLiner2 IN (SELECT peLinerID FROM dbo.Personnel WHERE peLinerID IS NOT NULL))	
	-- Liner Especifico
	AND (@SalesmanID = 'ALL' OR s.saLiner2 = @SalesmanID)
) Liners 

SELECT Liner, 
-- OVERFLOW
SUM(OFSalesAmount) OFSalesAmount, SUM(OFShows) OFShows, SUM(OFSales) OFSales, SUM(OFExit) OFExit, SUM(OFTotal) OFTotal, 
-- REGEN
SUM(RSalesAmount) RSalesAmount, SUM(RShows) RShows, SUM(RSales) RSales, SUM(RExit) RExit, SUM(RTotal) RTotal,
-- TOTAL
SUM(TSalesAmount) TSalesAmount, SUM(TShows) TShows, SUM(TSales) TSales, SUM(TExit) TExit, SUM(TTotal) TTotal
INTO #dbTemp
FROM (
-- ================== OVERFLOW =========================
-- Overlow
SELECT O.Liner AS Liner, 
-- OVERFLOW
0 OFSalesAmount, sum(O.Overflow) OFShows, 0 OFSales, 0 OFExit, 0 OFTotal, 
-- REGEN
0 RSalesAmount,0 RShows, 0 RSales, 0 RExit, 0 RTotal,
-- TOTAL
0 TSalesAmount,0 TShows, 0 TSales, 0 TExit, 0 TTotal
FROM dbo.UFN_IM_GetLinerOverflow(@DateFrom,@DateTo, @SalesRooms) AS O
LEFT JOIN #Liners L ON O.Liner = L.Liner
WHERE O.Liner = L.Liner
GROUP BY O.Liner
UNION ALL
-- =================== REGEN ==========================
-- ===Regen SalesAmount
select SA.Liner AS Liner,
-- OVERFLOW
0 OFSalesAmount, 0 OFShows, 0 OFSales, 0 OFExit, 0 OFTotal, 
-- REGEN
sum(SalesAmount) RSalesAmount,0 RShows, 0 RSales, 0 RExit, 0 RTotal,
-- TOTAL
0 TSalesAmount,0 TShows, 0 TSales, 0 TExit, 0 TTotal
from dbo.UFN_IM_GetLinerSalesAmount(@DateFrom,@DateTo,@SalesRooms,default,default,default,default,default,default,1) AS SA
LEFT JOIN #Liners L ON SA.Liner = L.Liner
WHERE SA.Liner = L.Liner
GROUP BY SA.Liner
UNION ALL
-- ===Total Sales Exit
SELECT SE.Liner AS Liner,
-- OVERFLOW
0 OFSalesAmount, 0 OFShows, 0 OFSales, 0 OFExit, 0 OFTotal, 
-- REGEN
0 RSalesAmount,0 RShows, 0 RSales, sum(SE.ExitSales) RExit, 0 RTotal,
-- TOTAL
0 TSalesAmount,0 TShows, 0 TSales, 0 TExit, 0 TTotal
FROM dbo.UFN_IM_GetLinerExitSales(@DateFrom,@DateTo,@SalesRooms,default,default,default,default,1) AS SE
LEFT JOIN #Liners L ON SE.Liner = L.Liner
WHERE SE.Liner = L.Liner
GROUP BY SE.Liner
UNION ALL
-- ===Total Sales
Select TS.Liner AS Liner,
-- OVERFLOW
0 OFSalesAmount, 0 OFShows, 0 OFSales, 0 OFExit, 0 OFTotal, 
-- REGEN
0 RSalesAmount,0 RShows, 0 RSales, 0 RExit, sum(TS.Sales) RTotal,
-- TOTAL
0 TSalesAmount,0 TShows, 0 TSales, 0 TExit, 0 TTotal
from dbo.UFN_IM_GetLinerSales(@DateFrom,@DateTo,@SalesRooms,default,default,default,default,1) AS TS
LEFT JOIN #Liners L ON TS.Liner = L.Liner
WHERE TS.Liner = L.Liner
GROUP BY TS.Liner
UNION ALL
-- =================== TOTALS ==================
-- ===Total SalesAmount
select TSA.Liner AS Liner,
-- OVERFLOW
0 OFSalesAmount, 0 OFShows, 0 OFSales, 0 OFExit, 0 OFTotal, 
-- REGEN
0 RSalesAmount,0 RShows, 0 RSales, 0 RExit, 0 RTotal,
-- TOTAL
sum(TSA.SalesAmount) TSalesAmount,0 TShows, 0 TSales, 0 TExit, 0 TTotal
from dbo.UFN_IM_GetLinerSalesAmount(@DateFrom,@DateTo,@SalesRooms,default,default,default,default,default,default,default) AS TSA
LEFT JOIN #Liners L ON TSA.Liner = L.Liner
WHERE TSA.Liner = L.Liner
GROUP BY TSA.Liner
UNION ALL
-- ===TOTAL Shows
SELECT TSH.Liner AS Liner,
-- OVERFLOW
0 OFSalesAmount, 0 OFShows, 0 OFSales, 0 OFExit, 0 OFTotal, 
-- REGEN
0 RSalesAmount,0 RShows, 0 RSales, 0 RExit, 0 RTotal,
-- TOTAL
0 TSalesAmount,sum(TSH.shows) TShows, 0 TSales, 0 TExit, 0 TTotal
FROM dbo.UFN_IM_GetLinerShows(@DateFrom,@DateTo,@SalesRooms,default,1) AS TSH
LEFT JOIN #Liners L ON TSH.Liner = L.Liner
WHERE TSH.Liner = L.Liner
GROUP BY TSH.Liner
UNION ALL
--===Total Sales Exit
SELECT TSE.Liner AS Liner,
-- OVERFLOW
0 OFSalesAmount, 0 OFShows, 0 OFSales, 0 OFExit, 0 OFTotal, 
-- REGEN
0 RSalesAmount,0 RShows, 0 RSales, 0 RExit, 0 RTotal,
-- TOTAL
0 TSalesAmount,0 TShows, 0 TSales, sum(TSE.ExitSales) TExit, 0 TTotal
FROM dbo.UFN_IM_GetLinerExitSales(@DateFrom,@DateTo,@SalesRooms,default,default,default,default,default) TSE
LEFT JOIN #Liners L ON TSE.Liner = L.Liner
WHERE TSE.Liner = L.Liner
GROUP BY TSE.Liner
UNION ALL
--===Total Sales
Select TT.Liner AS Liner,
-- OVERFLOW
0 OFSalesAmount, 0 OFShows, 0 OFSales, 0 OFExit, 0 OFTotal, 
-- REGEN
0 RSalesAmount,0 RShows, 0 RSales, 0 RExit, 0 RTotal,
-- TOTAL
0 TSalesAmount,0 TShows, 0 TSales, 0 TExit, sum(TT.Sales) TTotal
from dbo.UFN_IM_GetLinerSales(@DateFrom,@DateTo,@SalesRooms,default,default,default,default,default) AS TT
LEFT JOIN #Liners L ON TT.Liner = L.Liner
WHERE TT.Liner = L.Liner
GROUP BY TT.Liner
) AS LinerInfo 
GROUP BY Liner;

SELECT Liner,
--OverFlow
OFSalesAmount, OFShows, OFSales, OFExit, OFTotal,
--REGEN
RSalesAmount, RShows,(RTotal - RExit) RSales, RExit, RTotal,
--TOTAL
TSalesAmount, TShows, (TTotal - TExit) TSales, TExit, TTotal
--NORMAL
INTO #tb2
from #dbTemp
DROP TABLE #dbTemp
--
INSERT INTO @Table
SELECT 
Liner,
--OverFlow
OFSalesAmount, OFShows, OFSales, OFExit, OFTotal,
--REGEN
RSalesAmount, RShows, RSales, RExit, RTotal,
--NORMAL
(TSalesAmount -(RSalesAmount + OFSalesAmount)) NSalesAmount,
(TShows - (OFShows + RShows)) NShows,
(TSales - (OFSales + RSales)) NSales,
(TExit - (OFExit + RExit)) NExit,
(TTotal - (OFTotal + RTotal)) NTotal,
--TOTAL
TSalesAmount, TShows, TSales, TExit, TTotal
FROM #tb2
DROP TABLE #tb2

SELECT 
--Personnel
T.Liner, P.peN, 
-- OOP
CASE WHEN OOP.Sales IS NULL
THEN 0
ELSE OOP.Sales END OOP,
--Overflow
T.OFSalesAmount,T.OFShows,T.OFSales,T.OFExit,T.OFTotal,
--Regen
T.RSalesAmount, T.RShows,T.RSales,T.RExit,T.RTotal,
--Normal
T.NSalesAmount,T.NShows,T.NSales,T.NExit,T.NTotal,
--TOTAL
T.TSalesAmount,T.TShows,T.TSales,T.TExit,T.TTotal
FROM @Table T
LEFT JOIN #Liners L ON L.Liner = T.Liner
LEFT JOIN dbo.Personnel P ON P.peID = T.Liner
LEFT JOIN dbo.UFN_IM_GetLinerSales(@DateFrom,@DateTo,@SalesRooms,1,default,default,default,default) OOP ON OOP.Liner = L.Liner
WHERE L.Liner = T.Liner
ORDER BY T.TSalesAmount DESC,T.TShows DESC, T.Liner

DROP TABLE #Liners
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptGiftsKardex]    Script Date: 09/22/2016 19:13:36 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_IM_RptLinerStatistics]    Script Date: 09/22/2016 19:13:38 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_IM_RptLoginLog]    Script Date: 09/22/2016 19:13:40 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_IM_RptManifest]    Script Date: 09/22/2016 19:13:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de manifiesto (Processor Sales)
**		Manifiesto , Deposit Sales,	3. Ventas de otros dias, Be Backs, OOP, Cancellations, Regens, Deposit Before, etc.
**
** [ecanul] 03/Jun/2016 Created
** [ecanul] 06/Jun/2016 Modified, ya no se selecciona el ID de la tabla temporal
** [ecanul] 14/06/2016 Modified, se corrigio el llamado de la funcion UFN_IM_GetPerssonelPostsIDByDate por UFN_IM_GetPersonnelPostIDByDate
**
*/

CREATE PROCEDURE [dbo].[USP_IM_RptManifest]
--#region Parametros 
		@DateFrom datetime,						-- Fecha desde
		@DateTo datetime,						-- Fecha hasta
		@SalesRoom varchar(10),					-- Clave de la sala
		@SalesmanID varchar(10) = 'ALL',		-- Clave de un vendedor
		@SalesmanRoles varchar(20) = 'ALL',		-- Roles del vendedor (PR, Liner, Closer, Exit)
		@Segments varchar(8000) = 'ALL',		-- Claves de segmentos
		@Programs varchar(8000) = 'ALL',		-- Programs
		@BySegmentsCategories bit = 0,			-- Indica si es por categorias de segmentos
		@ByLocationsCategories bit= 0			-- Indica si es por categorias de locaciones
----------------------------------------------------------------------------------------
AS
SET ansi_warnings OFF -- Para que no se muestren los errores por codificacion ANSI
--=================== TABLA MANIFEST =============================
--#region ManifestTable
DECLARE @Manifest table (
--campos de huespedes
	id int identity(1,1),
	guID int,
	guLastName1 varchar(65),
	guFirstName1 varchar(40),
	guShow bit,
	guTour bit,
	guWalkout bit,
	guCTour bit,
	guSaveProgram bit,
	guSelfGen bit,
	guOverflow bit,
	salesmanDate DateTime,
	sold bit,
	sale bit,
	SaleType int,
	SaleTypeN varchar(100),
	-- Campos de la tabla de ventas	
	saID int,
	saMembershipNum varchar(10),
	samt varchar(10), 
	procSales int,	
	saGrossAmount money,
	-- personal de show y venta	
	-- PR1
	PR1 varchar(10),
	PR1N varchar(40),
	PR1P varchar(10),
	-- PR 2
	PR2 varchar(10),
	PR2N varchar(40),
	PR2P varchar(10),
	-- PR 3
	PR3 varchar(10),
	PR3N varchar(40),
	PR3P varchar(10),
	-- Vendedores
	-- Liner 1
	Liner1 varchar(10),
	Liner1N varchar(40),
	Liner1P varchar(10),
	-- Liner 2
	Liner2 varchar(10),
	Liner2N varchar(40),
	Liner2P varchar(10),
	-- Closer 1
	Closer1 varchar(10),
	Closer1N varchar(40),
	Closer1P varchar(10),
	-- Closer 2
	Closer2 varchar(10),
	Closer2N varchar(40),
	Closer2p varchar(10),
	-- Closer 3
	Closer3 varchar(10),
	Closer3N varchar(40),
	Closer3p varchar(10),
	-- Exit 1
	Exit1 varchar(10),
	Exit1N varchar(40),
	Exit1P varchar(10),
	-- Exit 2
	Exit2 varchar(10),
	Exit2N varchar(40),
	Exit2P varchar(10));	
--#endregion
--#region Guest
--#region InsertGuest
--===================================================================
--=======================   MANIFEST    =============================
--===================================================================
INSERT into @Manifest (guID, guLastName1, guFirstName1, salesmanDate, guTour, guWalkout, guCTour, guSaveProgram, guSelfGen, guOverflow, 
SaleType, SaleTypeN,procSales, PR1, PR1N, PR1P, PR2, PR2N, PR2P, PR3, PR3N, PR3P, Liner1, Liner1N, Liner1P, Liner2, Liner2N, Liner2P, 
Closer1, Closer1N, Closer1P, Closer2, Closer2N, Closer2p, Closer3, Closer3N, Closer3p, Exit1, Exit1N, Exit1P, Exit2, Exit2N, Exit2P)
select 
	--#region Campos de la tabla de huespedes
	G.guID,
	G.guLastName1,
	G.guFirstName1,
	G.guShowD,
	G.guTour,
	G.guWalkOut,
	G.guCTour,
	G.guSaveProgram,
	G.guSelfGen,
	G.guOverflow,	
	CASE WHEN G.guCTour = 1 THEN 1
		WHEN G.guSaveProgram = 1 THEN 2
		ELSE
			0
	END [SaleType],
	CASE WHEN G.guCTour = 1 THEN 'COURTESY TOURS'
		WHEN G.guSaveProgram = 1 THEN 'SAVE TOURS'
		ELSE
			'MANIFEST'
	END [SaleTypeN],
	0 procSales,
	--#endregion	
	--#region PERSONAL DEL SHOW
	-- Guest Services
	-- PR 1
	G.guPRInvit1 as guPR1,
	GP1.peN as guPR1N,
	CASE WHEN G.guPRInvit1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guPRInvit1) ELSE NULL END [PR1P],
	-- PR2
	G.guPRInvit2 as guPR2,
	GP2.peN as guPR2N,
	CASE WHEN G.guPRInvit2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guPRInvit2) ELSE NULL END [PR2P],
	-- PR3
	G.guPRInvit3 as guPR3,
	GP3.peN as guPR3N,
	CASE WHEN G.guPRInvit3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guPRInvit3) ELSE NULL END [PR3P],
	-- Vendedores
	-- Liner 1
	G.guLiner1 as guLiner1,
	GL1.peN as guLiner1N,
	CASE WHEN G.guLiner1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guLiner1) ELSE NULL END [Liner1P],
	-- Liner 2
	G.guLiner2 as guLiner2,
	GL2.peN as guLiner2N,
	CASE WHEN G.guLiner2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guLiner2) ELSE NULL END [Liner2P],
	-- Closer 1
	G.guCloser1 as guCloser1,
	GC1.peN as guCloser1N,
	CASE WHEN G.guCloser1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser1) ELSE NULL END [Closer1P],
	-- Closer 2
	G.guCloser2 as guCloser2,
	GC2.peN as guCloser2N,
	CASE WHEN G.guCloser2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser2) ELSE NULL END [Closer2P],
	-- Closer 3
	G.guCloser3 as guCloser3,
	GC3.peN as guCloser3N,
	CASE WHEN G.guCloser3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser3) ELSE NULL END [Closer3P],
	-- Exit 1
	G.guExit1 as guExit1,
	GE1.peN as guExit1N,
	CASE WHEN G.guExit1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guExit1) ELSE NULL END [Exit1P],
	-- Exit 2
	G.guExit2 as guExit2,
	GE2.peN as guExit2N,
	CASE WHEN G.guExit2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guExit2) ELSE NULL END [Exit2P]
	--#endregion
from Guests G	
	--#region PERSONAL DEL SHOW
	-- Guest Services
	left join Personnel GP1 on GP1.peID = G.guPRInvit1
	left join Personnel GP2 on GP2.peID = G.guPRInvit2
	left join Personnel GP3 on GP3.peID = G.guPRInvit3
	-- Vendedores
	left join Personnel GL1 on GL1.peID = G.guLiner1
	left join Personnel GL2 on GL2.peID = G.guLiner2
	left join Personnel GC1 on GC1.peID = G.guCloser1
	left join Personnel GC2 on GC2.peID = G.guCloser2
	left join Personnel GC3 on GC3.peID = G.guCloser3
	left join Personnel GE1 on GE1.peID = G.guExit1
	left join Personnel GE2 on GE2.peID = G.guExit2	
	--#endregion	
	--#region Segment and Location	
	left join Agencies A on A.agID = G.guag
	left join SegmentsByAgency SA on SA.seID = A.agse
	left join Locations LO on LO.loID = G.guloInvit
	left join LocationsCategories LC on LC.lcID = LO.lolc
	left join LeadSources LS on LS.lsID = G.guls
	left join SegmentsByLeadSource SL on SL.soID = LS.lsso
	left join SegmentsCategories SCA on SCA.scID = SA.sesc
	left join SegmentsCategories SCL on SCL.scID = SL.sosc 
	--#endregion
where
	--#region Where
	-- Fecha de show
	G.guShowD between @DateFrom and @DateTo
	-- Sala de ventas
	and G.gusr = @SalesRoom
	and (@SalesmanID = 'ALL'
		-- Rol de PR
		or ((@SalesmanRoles = 'ALL' or 'PR' in (select item from split(@SalesmanRoles, ',')))
			and (@SalesmanID = G.guPRInvit1 or @SalesmanID = G.guPRInvit2 or @SalesmanID = G.guPRInvit3))
		-- Rol de Liner
		or ((@SalesmanRoles = 'ALL' or 'LINER' in (select item from split(@SalesmanRoles, ',')))
			and (@SalesmanID = G.guLiner1 or @SalesmanID = G.guLiner2))
		-- Rol de Closer
		or ((@SalesmanRoles = 'ALL' or 'CLOSER' in (select item from split(@SalesmanRoles, ',')))
			and (@SalesmanID = G.guCloser1 or @SalesmanID = G.guCloser2 or @SalesmanID = G.guCloser3))
		-- Rol de Exit
		or ((@SalesmanRoles = 'ALL' or 'EXIT' in (select item from split(@SalesmanRoles, ',')))
			and (@SalesmanID = G.guExit1 or @SalesmanID = G.guExit2)))			
	-- Segmento
	and (@Segments = 'ALL' or A.agse in (select item from split(@Segments, ',')))
	-- Programa
	and (@Programs = 'ALL' or LS.lspg in (select item from split(@Programs, ','))) 
	--#endregion
order by G.guID;
--#endregion
--#endregion
--=================== Insert SALES =============================
--#region insertSales
DECLARE @nReg int; -- numero de regitros
SET @nReg=(SELECT COUNT(*) FROM @Manifest);
DECLARE @cont int; -- almacena la cantidad de veces que se recorre el while
SET @cont = 1;
DECLARE @gu int; -- si tiene ventas
WHILE @nReg >= @cont 
BEGIN
	SET @gu = (SELECT guid FROM @Manifest WHERE id = @cont); --obtiene el id del gest	
	DECLARE @sale int;
	SET @sale = (SELECT TOP 1 sagu FROM Sales WHERE saD BETWEEN @DateFrom and @DateTo AND sagu = @gu);	
	--#Region IF @sale
	IF @sale IS NOT NULL		
	BEGIN
		UPDATE @Manifest SET sold = 1 WHERE id = @cont;
		SELECT 
		--Datos de la venta
			S.sagu AS GUID,
			CASE WHEN ST.ststc = 'DG' THEN 5 ELSE 0 END [SaleType],
			CASE WHEN ST.ststc = 'DG' THEN 'DOWNGRADES' ELSE 'MANIFEST' END [SaleTypeN],
			S.saID,
			S.saMembershipNum,
			S.samt,	
			S.saD,	
			S.saGrossAmount,
		--Datos de los vendedores
			S.saPR1 as saPR1,
			SP1.peN as saPR1N,
			CASE WHEN S.saPR1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saPR1) ELSE NULL END [PR1P],
			-- PR 2
			S.saPR2 as saPR2,
			SP2.peN as saPR2N,
			CASE WHEN S.saPR2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saPR2) ELSE NULL END [PR2P],
			-- PR 3
			S.saPR3 as saPR3,
			SP3.peN as saPR3N,
			CASE WHEN S.saPR3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saPR3) ELSE NULL END [PR3P],
			-- Liner 1
			S.saLiner1 as saLiner1,
			SL1.peN as saLiner1N,
			CASE WHEN S.saLiner1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saLiner1) ELSE NULL END [Liner1P],
			-- Liner 2
			S.saLiner2 as saLiner2,
			SL2.peN as saLiner2N,
			CASE WHEN S.saLiner2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saLiner2) ELSE NULL END [Liner2P],
			-- Closer 1
			S.saCloser1 as saCloser1,
			SC1.peN as saCloser1N,
			CASE WHEN S.saCloser1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saCloser1) ELSE NULL END [Closer1P],
			-- Closer 2
			S.saCloser2 as saCloser2,
			SC2.peN as saCloser2N,
			CASE WHEN S.saCloser2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saCloser2) ELSE NULL END [Closer2P],
			-- Closer 3
			S.saCloser3 as saCloser3,
			SC3.peN as saCloser3N,
			CASE WHEN S.saCloser3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saCloser3) ELSE NULL END [Closer3P],
			-- Exit 1
			S.saExit1 as saExit1,
			SE1.peN as saExit1N,
			CASE WHEN S.saExit1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saExit1) ELSE NULL END [Exit1P],
			-- Exit 2
			S.saExit2 as saExit2,
			SE2.peN as saExit2N,
			CASE WHEN S.saExit2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saExit2) ELSE NULL END [Exit2P],
			1 Sold,
			1 procSales
		INTO #tbTemp
		FROM Guests G
			LEFT JOIN Sales S on S.sagu = G.guID
			LEFT JOIN SaleTypes ST on ST.stID = S.sast
			-- Guest Services
			left join Personnel SP1 on SP1.peID = S.saPR1
			left join Personnel SP2 on SP2.peID = S.saPR2
			left join Personnel SP3 on SP3.peID = S.saPR3
			-- Vendedores
			left join Personnel SL1 on SL1.peID = S.saLiner1
			left join Personnel SL2 on SL2.peID = S.saLiner2
			left join Personnel SC1 on SC1.peID = S.saCloser1
			left join Personnel SC2 on SC2.peID = S.saCloser2
			left join Personnel SC3 on SC3.peID = S.saCloser3
			left join Personnel SE1 on SE1.peID = S.saExit1
			left join Personnel SE2 on SE2.peID = S.saExit2			
		WHERE
			S.saD BETWEEN @DateFrom and @DateTo AND sagu = @gu
			AND S.sast NOT IN ('BUMP','REGEN')
			AND S.saProcD BETWEEN @DateFrom AND @DateTo
			AND (S.saCancel = 0 OR NOT S.saCancelD BETWEEN @DateFrom AND @DateTo)
			AND S.sasr = @SalesRoom
			and (@SalesmanID = 'ALL'
				-- Rol de PR
				or ((@SalesmanRoles = 'ALL' or 'PR' in (select item from split(@SalesmanRoles, ',')))
					and (@SalesmanID = S.saPR1 or @SalesmanID = S.saPR2 or @SalesmanID = S.saPR3))
				-- Rol de Liner
				or ((@SalesmanRoles = 'ALL' or 'LINER' in (select item from split(@SalesmanRoles, ',')))
					and (@SalesmanID = S.saLiner1 or @SalesmanID = S.saLiner2))
				-- Rol de Closer
				or ((@SalesmanRoles = 'ALL' or 'CLOSER' in (select item from split(@SalesmanRoles, ',')))
					and (@SalesmanID = S.saCloser1 or @SalesmanID = S.saCloser2 or @SalesmanID = S.saCloser3))
				-- Rol de Exit
				or ((@SalesmanRoles = 'ALL' or 'EXIT' in (select item from split(@SalesmanRoles, ',')))
					and (@SalesmanID = S.saExit1 or @SalesmanID = S.saExit2))
				);
		INSERT INTO @Manifest (guID,SaleType,SaleTypeN,saID,saMembershipNum,samt,salesmanDate,saGrossAmount,PR1,PR1N,PR1P,PR2,PR2N,PR2P, PR3,PR3N,PR3P,
		Liner1,Liner1N,Liner1P,Liner2,Liner2N,Liner2P,Closer1,Closer1N,Closer1P,Closer2,Closer2N,Closer2p,Closer3,Closer3N,Closer3p,
		Exit1,Exit1N,Exit1P,Exit2,Exit2N,Exit2P,sold,procSales,sale)
		SELECT *, 1 as sale FROM #tbTemp;
		DROP TABLE #tbTemp;
	END		
	--#Region IF @sale	
	SET @cont = @cont +1;
END;
--=================== CAMPOS CALCULADOS ==================
UPDATE @Manifest SET guShow = CASE WHEN guTour = 1 OR guWalkout = 1 OR ((guCTour=1 OR guSaveProgram=1) AND saID IS NOT NULL)
							THEN 1 ELSE 0 END;
--===================================================================
--===================  DEPOSIT SALES    =============================
--===================================================================
INSERT into @Manifest (guID, guLastName1, guFirstName1, salesmanDate, guTour, guWalkout, guCTour, guSaveProgram, guSelfGen, guOverflow, 
SaleType, SaleTypeN,procSales, PR1, PR1N, PR1P, PR2, PR2N, PR2P, PR3, PR3N, PR3P, Liner1, Liner1N, Liner1P, Liner2, Liner2N, Liner2P, 
Closer1, Closer1N, Closer1P, Closer2, Closer2N, Closer2p, Closer3, Closer3N, Closer3p, Exit1, Exit1N, Exit1P, Exit2, Exit2N, Exit2P)
select 
	--#region Campos de la tabla de huespedes
	G.guID,
	G.guLastName1,
	G.guFirstName1,
	G.guShowD,
	G.guTour,
	G.guWalkOut,
	G.guCTour,
	G.guSaveProgram,
	G.guSelfGen,
	G.guOverflow,	
	12 SaleType,
	'DEPOSIT SALES' AS SaleTypeN,
	0 procSales,
	--#endregion	
	--#region PERSONAL DEL SHOW
	-- Guest Services
	-- PR 1
	G.guPRInvit1 as guPR1,
	GP1.peN as guPR1N,
	CASE WHEN G.guPRInvit1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guPRInvit1) ELSE NULL END [PR1P],
	-- PR2
	G.guPRInvit2 as guPR2,
	GP2.peN as guPR2N,
	CASE WHEN G.guPRInvit2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guPRInvit2) ELSE NULL END [PR2P],
	-- PR3
	G.guPRInvit3 as guPR3,
	GP3.peN as guPR3N,
	CASE WHEN G.guPRInvit3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guPRInvit3) ELSE NULL END [PR3P],
	-- Vendedores
	-- Liner 1
	G.guLiner1 as guLiner1,
	GL1.peN as guLiner1N,
	CASE WHEN G.guLiner1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guLiner1) ELSE NULL END [Liner1P],
	-- Liner 2
	G.guLiner2 as guLiner2,
	GL2.peN as guLiner2N,
	CASE WHEN G.guLiner2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guLiner2) ELSE NULL END [Liner2P],
	-- Closer 1
	G.guCloser1 as guCloser1,
	GC1.peN as guCloser1N,
	CASE WHEN G.guCloser1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser1) ELSE NULL END [Closer1P],
	-- Closer 2
	G.guCloser2 as guCloser2,
	GC2.peN as guCloser2N,
	CASE WHEN G.guCloser2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser2) ELSE NULL END [Closer2P],
	-- Closer 3
	G.guCloser3 as guCloser3,
	GC3.peN as guCloser3N,
	CASE WHEN G.guCloser3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser3) ELSE NULL END [Closer3P],
	-- Exit 1
	G.guExit1 as guExit1,
	GE1.peN as guExit1N,
	CASE WHEN G.guExit1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guExit1) ELSE NULL END [Exit1P],
	-- Exit 2
	G.guExit2 as guExit2,
	GE2.peN as guExit2N,
	CASE WHEN G.guExit2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guExit2) ELSE NULL END [Exit2P]
	--#endregion
from Guests G	
--#region PERSONAL DEL SHOW
	-- Guest Services
	left join Personnel GP1 on GP1.peID = G.guPRInvit1
	left join Personnel GP2 on GP2.peID = G.guPRInvit2
	left join Personnel GP3 on GP3.peID = G.guPRInvit3
	-- Vendedores
	left join Personnel GL1 on GL1.peID = G.guLiner1
	left join Personnel GL2 on GL2.peID = G.guLiner2
	left join Personnel GC1 on GC1.peID = G.guCloser1
	left join Personnel GC2 on GC2.peID = G.guCloser2
	left join Personnel GC3 on GC3.peID = G.guCloser3
	left join Personnel GE1 on GE1.peID = G.guExit1
	left join Personnel GE2 on GE2.peID = G.guExit2	
	--#endregion	
	--#region Segment and Location	
	left join Agencies A on A.agID = G.guag
	left join SegmentsByAgency SA on SA.seID = A.agse
	left join Locations LO on LO.loID = G.guloInvit
	left join LocationsCategories LC on LC.lcID = LO.lolc
	left join LeadSources LS on LS.lsID = G.guls
	left join SegmentsByLeadSource SL on SL.soID = LS.lsso
	left join SegmentsCategories SCA on SCA.scID = SA.sesc
	left join SegmentsCategories SCL on SCL.scID = SL.sosc 
	--#endregion
where
	G.guDepositSaleD between @DateFrom and @DateTo
	-- Fecha de show diferente de la fecha de venta deposito
	and G.guShowD <> G.guDepositSaleD
	-- Sala de ventas
	and G.gusr = @SalesRoom
	-- Vendedor y rol
	and (@SalesmanID = 'ALL'
		-- Rol de PR
		or ((@SalesmanRoles = 'ALL' or 'PR' in (select item from split(@SalesmanRoles, ',')))
			and (@SalesmanID = G.guPRInvit1 or @SalesmanID = G.guPRInvit2 or @SalesmanID = G.guPRInvit3))
		-- Rol de Liner
		or ((@SalesmanRoles = 'ALL' or 'LINER' in (select item from split(@SalesmanRoles, ',')))
			and (@SalesmanID = G.guLiner1 or @SalesmanID = G.guLiner2))
		-- Rol de Closer
		or ((@SalesmanRoles = 'ALL' or 'CLOSER' in (select item from split(@SalesmanRoles, ',')))
			and (@SalesmanID = G.guCloser1 or @SalesmanID = G.guCloser2 or @SalesmanID = G.guCloser3))
		-- Rol de Exit
		or ((@SalesmanRoles = 'ALL' or 'EXIT' in (select item from split(@SalesmanRoles, ',')))
			and (@SalesmanID = G.guExit1 or @SalesmanID = G.guExit2)))			
	-- Segmento
	and (@Segments = 'ALL' or A.agse in (select item from split(@Segments, ',')))
	-- Programa
	and (@Programs = 'ALL' or LS.lspg in (select item from split(@Programs, ',')))
order by G.guID;
--===================================================================
--======================  OTHER SALES    ============================
--===================================================================
INSERT into @Manifest (guID, guSelfGen, salesmanDate, sold, sale,SaleType, SaleTypeN, saID, saMembershipNum, samt, procSales, saGrossAmount, PR1, PR1N, PR1P, 
PR2, PR2N, PR2P, PR3, PR3N, PR3P, Liner1, Liner1N, Liner1P, Liner2, Liner2N, Liner2P, Closer1, Closer1N, Closer1P, Closer2, Closer2N, Closer2p, 
Closer3, Closer3N, Closer3p, Exit1, Exit1N, Exit1P, Exit2, Exit2N, Exit2P)
SELECT 
--Datos de la venta
	S.sagu AS GUID,
	S.saSelfGen,
	S.saD,
	1 Sold,
	1 Sale,
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
	S.saID,
	S.saMembershipNum,
	S.samt,	
	CASE WHEN S.saProcD BETWEEN @DateFrom AND @DateTo THEN 1 ELSE 0 END [procSales],
	CASE WHEN S.saProcD BETWEEN @DateFrom AND @DateTo THEN S.saGrossAmount ELSE 0 END [saGrossAmount],
--Datos de los vendedores
	S.saPR1 as saPR1,
	SP1.peN as saPR1N,
	CASE WHEN S.saPR1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saPR1) ELSE NULL END [PR1P],
	-- PR 2
	S.saPR2 as saPR2,
	SP2.peN as saPR2N,
	CASE WHEN S.saPR2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saPR2) ELSE NULL END [PR2P],
	-- PR 3
	S.saPR3 as saPR3,
	SP3.peN as saPR3N,
	CASE WHEN S.saPR3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saPR3) ELSE NULL END [PR3P],
	-- Liner 1
	S.saLiner1 as saLiner1,
	SL1.peN as saLiner1N,
	CASE WHEN S.saLiner1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saLiner1) ELSE NULL END [Liner1P],
	-- Liner 2
	S.saLiner2 as saLiner2,
	SL2.peN as saLiner2N,
	CASE WHEN S.saLiner2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saLiner2) ELSE NULL END [Liner2P],
	-- Closer 1
	S.saCloser1 as saCloser1,
	SC1.peN as saCloser1N,
	CASE WHEN S.saCloser1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saCloser1) ELSE NULL END [Closer1P],
	-- Closer 2
	S.saCloser2 as saCloser2,
	SC2.peN as saCloser2N,
	CASE WHEN S.saCloser2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saCloser2) ELSE NULL END [Closer2P],
	-- Closer 3
	S.saCloser3 as saCloser3,
	SC3.peN as saCloser3N,
	CASE WHEN S.saCloser3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saCloser3) ELSE NULL END [Closer3P],
	-- Exit 1
	S.saExit1 as saExit1,
	SE1.peN as saExit1N,
	CASE WHEN S.saExit1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saExit1) ELSE NULL END [Exit1P],
	-- Exit 2
	S.saExit2 as saExit2,
	SE2.peN as saExit2N,
	CASE WHEN S.saExit2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saExit2) ELSE NULL END [Exit2P]
FROM Guests G --para obtener la fecha del show
	LEFT JOIN Sales S on S.sagu = G.guID
	LEFT JOIN SaleTypes ST on ST.stID = S.sast
	-- Guest Services
	left join Personnel SP1 on SP1.peID = S.saPR1
	left join Personnel SP2 on SP2.peID = S.saPR2
	left join Personnel SP3 on SP3.peID = S.saPR3
	-- Vendedores
	left join Personnel SL1 on SL1.peID = S.saLiner1
	left join Personnel SL2 on SL2.peID = S.saLiner2
	left join Personnel SC1 on SC1.peID = S.saCloser1
	left join Personnel SC2 on SC2.peID = S.saCloser2
	left join Personnel SC3 on SC3.peID = S.saCloser3
	left join Personnel SE1 on SE1.peID = S.saExit1
	left join Personnel SE2 on SE2.peID = S.saExit2			
WHERE
	((--NO SHOWS
	(G.guShowD IS NULL OR NOT G.guShowD BETWEEN @DateFrom AND @DateTo)
	--FECHA DE --- VENTA	-----------------	Venta Procesable	-----------------------
	AND (S.saD BETWEEN @DateFrom AND @DateTo OR S.saProcD BETWEEN @DateFrom AND @DateTo OR s.saCancelD BETWEEN @DateFrom AND @DateTo))
	-- Incluye Bumps, Regen y ventas de otras salas
	OR ((S.sast IN ('BUMP','REGEN') OR G.gusr <> S.sasr) AND S.saD BETWEEN @DateFrom AND @DateTo) )
	AND S.sasr = @SalesRoom
	and (@SalesmanID = 'ALL'
		-- Rol de PR
		or ((@SalesmanRoles = 'ALL' or 'PR' in (select item from split(@SalesmanRoles, ',')))
			and (@SalesmanID = S.saPR1 or @SalesmanID = S.saPR2 or @SalesmanID = S.saPR3))
		-- Rol de Liner
		or ((@SalesmanRoles = 'ALL' or 'LINER' in (select item from split(@SalesmanRoles, ',')))
			and (@SalesmanID = S.saLiner1 or @SalesmanID = S.saLiner2))
		-- Rol de Closer
		or ((@SalesmanRoles = 'ALL' or 'CLOSER' in (select item from split(@SalesmanRoles, ',')))
			and (@SalesmanID = S.saCloser1 or @SalesmanID = S.saCloser2 or @SalesmanID = S.saCloser3))
		-- Rol de Exit
		or ((@SalesmanRoles = 'ALL' or 'EXIT' in (select item from split(@SalesmanRoles, ',')))
			and (@SalesmanID = S.saExit1 or @SalesmanID = S.saExit2))
		)
ORDER BY SaleType, S.sagu;
--===================================================================
--===================  SELECT MANIFEST    ===========================
--===================================================================
SELECT guID, guLastName1,guFirstName1,	guShow,	guTour,	guWalkout, guCTour,	guSaveProgram, guSelfGen, guOverflow, salesmanDate,	sold, sale,SaleType, SaleTypeN,
saID,saMembershipNum, samt, procSales, saGrossAmount, PR1, PR1N, PR1P, PR2, PR2N, PR2P, PR3, PR3N, PR3P, Liner1, Liner1N, Liner1P,
Liner2,	Liner2N, Liner2P, Closer1, Closer1N, Closer1P, Closer2, Closer2N, Closer2p, Closer3, Closer3N, Closer3p, Exit1, Exit1N, Exit1P,
Exit2, Exit2N, Exit2P
from @Manifest ORDER BY SaleType, guID
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptManifestByLSRange]    Script Date: 09/22/2016 19:13:47 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_IM_RptManifestRange]    Script Date: 09/22/2016 19:13:50 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByFlightSalesRoom]    Script Date: 09/22/2016 19:13:52 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByHotel]    Script Date: 09/22/2016 19:13:55 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByHotelGroup]    Script Date: 09/22/2016 19:13:58 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByHotelGroupSalesRoom]    Script Date: 09/22/2016 19:14:01 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByHotelSalesRoom]    Script Date: 09/22/2016 19:14:05 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByWave]    Script Date: 09/22/2016 19:14:08 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_IM_RptProductionByWaveSalesRoom]    Script Date: 09/22/2016 19:14:11 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_IM_RptSelfGenTeam]    Script Date: 09/22/2016 19:14:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte Self Gen & Self Gen Team (Processor Sales)
**		1.- SelfGen 2.- SelfGen Team
**
** [ecanul] 25/07/2016 Created
** [ecanul] 05/09/2016 Modified corregifo error que agregaba un show a algunos vendedores
**
*/

CREATE PROCEDURE [dbo].[USP_IM_RptSelfGenTeam]
		@DateFrom Datetime,				--Fecha Inicio
		@DateTo Datetime,				--Fecha Fin
		@SalesRooms varchar(50),			-- SalesRooms
		@SalesmanID varchar(10) = 'ALL' -- Id del Vendedor
AS

DECLARE @table Table(
	id int identity(1,1),
	GUID int,
	Liner varchar(10),
	peN varchar(40),
	Type varchar(10),
	Show money,
	Sale money,
	saID int,
	SalesAmount money,
	SelfGenType int
);
-- =========================================================
-- ===============		 Datos Brutos	====================
-- =========================================================
SELECT *
INTO #personnel
FROM(
-- ==============================GUESTS=======================================
-- ==================Guests.Liner1
SELECT 
	G.guID Guid,
	G.guLiner1 Liner,
	P.peN,
	'Guest' [Type],
	CASE WHEN G.guTour = 1 OR G.guWalkOut = 1 OR ((G.guCTour = 1 OR G.guSaveProgram = 1) AND G.guSale = 1) THEN 1 ELSE 0 END Show,
	0 guSale,
	0 saID,
	0 SalesAmount,
	dbo.UFN_IM_GetSelfGenType(G.guLiner1,G.guPRInvit1,G.guPRInvit2,G.guPRInvit3) SelfGenType
FROM dbo.Guests G
LEFT JOIN dbo.Personnel P on P.peID = G.guLiner1
WHERE 
	--Fecha Show
	G.guShowD BETWEEN @DateFrom AND @DateTo 
	-- Sala de ventas
	AND (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
	-- Que Tenga Liner 1
	AND G.guLiner1 IS NOT NULL
	-- que sea show
	AND (G.guTour = 1 OR G.guWalkOut = 1 OR ((G.guCTour = 1 OR G.guSaveProgram = 1) AND G.guSale = 1))
	-- Que no sea venta depocito de otro dia 
	AND (G.guDepositSaleD IS NULL OR NOT G.guDepositSaleD BETWEEN @DateFrom AND @DateTo)
	-- Que sea SelfGen marcado por los hostess
	AND (G.guSelfGen = 1
	-- Liner 1 configurado como Front To Middle (FTM)
	OR G.guLiner1 IN (SELECT peLinerID FROM dbo.Personnel WHERE peLinerID IS NOT NULL))
UNION ALL
-- ==================Guests.Liner2
SELECT 
	G.guID Guid,
	G.guLiner2 Liner,
	P.peN,
	'Guest' [Type],
	CASE WHEN g.guTour =  1 OR g.guWalkOut =  1 OR ((G.guCTour = 1 OR G.guSaveProgram = 1) AND G.guSale = 1) THEN 1 ELSE 0 END Show,
	0 guSale,
	0 saID,
	0 SalesAmount,
	dbo.UFN_IM_GetSelfGenType(G.guLiner2,G.guPRInvit1,G.guPRInvit2,G.guPRInvit3) SelfGenType
FROM dbo.Guests G
LEFT JOIN dbo.Personnel P on P.peID = G.guLiner2
WHERE 
	--Fecha Show
	G.guShowD BETWEEN @DateFrom AND @DateTo 
	-- Sala de ventas
	AND (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
	-- Que Tenga Liner 2
	AND G.guLiner2 IS NOT NULL
	-- que sea show
	AND (G.guTour = 1 OR G.guWalkOut = 1 OR ((G.guCTour = 1 OR G.guSaveProgram = 1) AND G.guSale = 1))
	-- Que no sea venta depocito de otro dia 
	AND (G.guDepositSaleD IS NULL OR NOT G.guDepositSaleD BETWEEN @DateFrom AND @DateTo)
	-- Que sea SelfGen marcado por los hostess
	AND (G.guSelfGen = 1
	-- Liner 2 configurado como Front To Middle (FTM)
	OR G.guLiner2 IN (SELECT peLinerID FROM dbo.Personnel WHERE peLinerID IS NOT NULL))
UNION ALL
-- ================================ SALES
-- ==================Sales.Liner1
SELECT 
	S.sagu Guid,
	S.saLiner1 Liner,
	P.peN,
	'Sale1' [Type],
	0 Show,
	dbo.UFN_OR_GetPercentageSalesman(s.saLiner1,s.saLiner2,default,default,default) guSale,
	S.saID,
	dbo.UFN_IM_GetSalesAmount(S.saGrossAmount,S.saLiner1,S.saLiner2,DEFAULT,DEFAULT,DEFAULT) SalesAmount,
	dbo.UFN_IM_GetSelfGenType(S.saLiner1,S.saPR1,S.saPR2,S.saPR3) SelfGenType
FROM dbo.Guests G
LEFT JOIN dbo.Sales S on S.sagu = G.guID
LEFT JOIN dbo.Personnel P on P.peID = S.saLiner1
WHERE
	--Fecha de venta
	S.saProcD BETWEEN @DateFrom AND @DateTo
	--Sala de Ventas
	AND (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
	-- Con Liner1
	AND S.saLiner1 IS NOT NULL
	--No canceladas
	AND S.saCancel = 0
	-- Selfgen marcado por los hostess
	AND (S.saSelfGen = 1
	--Liner 1 como Front To Middle
	OR S.saLiner1 IN (SELECT peLinerID FROM dbo.Personnel WHERE peLinerID IS NOT NULL))
UNION ALL
-- =================Sales.Liner2
SELECT
	S.sagu Guid,
	S.saLiner2 Liner,
	P.peN,
	'Sale2' [Type],
	0 Show,
	dbo.UFN_OR_GetPercentageSalesman(s.saLiner1,s.saLiner2,default,default,default) guSale,
	S.saID,
	dbo.UFN_IM_GetSalesAmount(S.saGrossAmount,S.saLiner1,S.saLiner2,DEFAULT,DEFAULT,DEFAULT) SalesAmount,
	dbo.UFN_IM_GetSelfGenType(S.saLiner2,S.saPR1,S.saPR2,S.saPR3) SelfGenType
FROM dbo.Guests G
LEFT JOIN dbo.Sales S on S.sagu = G.guID
LEFT JOIN dbo.Personnel P on P.peID = S.saLiner2
WHERE 
	--Fecha de venta
	S.saProcD BETWEEN @DateFrom AND @DateTo
	--Sala de Ventas
	AND (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
	-- Con Liner2
	AND S.saLiner2 IS NOT NULL
	--No canceladas
	AND S.saCancel = 0
	-- Selfgen marcado por los hostess
	AND (S.saSelfGen = 1
	--Liner 2 como Front To Middle
	OR S.saLiner2 IN (SELECT peLinerID FROM dbo.Personnel WHERE peLinerID IS NOT NULL))
) PE

INSERT INTO @table
SELECT * 
FROM #personnel 
WHERE (@SalesmanID = 'ALL' OR Liner = @SalesmanID)
ORDER BY SelfGenType DESC, Liner, gusale DESC, said ;
DROP TABLE #personnel;
-- =========================================================
-- ================	  FIN Datos Brutos	====================
-- =========================================================

-- =========================================================
-- ===============	Procesado de Datos 	====================
-- =========================================================

DECLARE @TotReg int;	-- Total de registros
DECLARE @count int;		-- Contador

SET @count = 1;
SET @TotReg = (SELECT COUNT(id) FROM @table);

DECLARE @tt table(
	id int identity (1,1),
	Liner varchar(10),
	--OOP
	OOP money,
	--Overflow
	OFVol money,
	OFUPS money,
	OFSales money,
	--REGEN
	RGVol money,
	RGUPS money,
	RGSales money,
	--TOTAL
	TVol money,
	TShows money,
	TSales money,
	-- Otros Datos
	SelfGenType int,
	saID int
);

WHILE @TotReg >= @count
BEGIN
	INSERT INTO @tt
	SELECT 
		T.Liner, 
		--OPP
		CASE WHEN S.saD <> S.saProcD THEN (1 * dbo.UFN_OR_GetPercentageSalesman(s.saLiner1,s.saLiner2,default,default,default)) ELSE 0 END OOP,
		--Overflow
		0 OFVol,
		(G.guOverflow * dbo.UFN_OR_GetPercentageSalesman(G.guLiner1, G.guLiner2,default,default,default)) OFUPS,
		0 OFSales,
		--REGEN
		CASE WHEN S.saProcD BETWEEN @DateFrom AND @DateTo AND S.saCancel = 0 AND S.sast = 'REGEN' THEN
			(S.saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(s.saLiner1,s.saLiner2,default,default,default))
		ELSE 
			0
		END RGVol,
		0 RGUPS,
		CASE WHEN S.saProcD BETWEEN @DateFrom AND @DateTo AND S.saCancel = 0 AND S.sast = 'REGEN' THEN
			(dbo.UFN_OR_GetPercentageSalesman(S.saLiner1,S.saLiner2,default,default,default))
		ELSE 0 END RGSales,
		--TOTAL
		T.SalesAmount TVol,
		T.Show TShow,
		T.Sale TSales,
		--NORMAL
		T.SelfGenType,
		T.saID
	from @table T
	LEFT JOIN dbo.Guests G ON G.guID = T.GUID
	LEFT JOIN dbo.Sales S ON S.saID = T.saID
	WHERE T.id = @count
	GROUP BY T.Liner, S.saID, S.saProcD, S.saD, G.guOverflow, G.guLiner1, S.saCancel, S.sast, T.saID,
	G.guLiner2, S.saGrossAmount, S.saLiner1, S.saLiner2, T.SalesAmount, T.Show, T.Sale, T.SelfGenType
	-- incrementa el contador
	SET @count = @count + 1; 
END;
-- =========================================================
-- ===============	FIN Procesado de Datos 	================
-- =========================================================

-- =========================================================
-- ===============	Calculo de Subtotales 	================
-- =========================================================

SELECT Liner, SUM(OOP) OOP, 
--Overflow
SUM(OFVol) OFVol, SUM(OFUPS) OFUPS, SUM(OFSales) OFSales, 
--Regen
SUM(RGVol) RGVol, SUM(RGUPS) RGUPS, SUM(RGSales) RGSales,
--NORMAL
	--VOL
(SUM(TVol) - (SUM(OFVol) + SUM(RGVol))) NVol,
	--UPS
(SUM(TShows)- (SUM(OFUPS) + SUM(RGUPS))) NUPS,
	--Sales
(SUM(TSales) - (SUM(OFSales) + SUM(RGSales))) NSales,
--Total
SUM(TVol) TVol, SUM(TShows) TUPS, SUM(TSales) TSales,
CASE WHEN SelfGenType = 1 THEN 'SELF GEN' ELSE 'SELF GEN TEAM' END SelfGenType
INTO #Totals
from @tt GROUP BY Liner, SelfGenType
ORDER by SelfGenType DESC
-- =========================================================
-- ===============	Fin Calculo de Subtotales 	============
-- =========================================================

DECLARE @Report table(
	Liner varchar(10),
	SalesmanName varchar(40),
	OOP int,
	OFVol money,
	OFUPS money,
	OFSales money,
	RGVol money,
	RGUPS money,
	RGSales money,
	NVol money,
	NUPS money,
	NSales money,
	TVol money,
	TUPS money,
	TSales money,
	SelfGenType varchar(15)
);

INSERT INTO @Report
SELECT 
	--Info
	T.Liner, P.peN, T.OOP, 
	-- Overflow
	T.OFVol, T.OFUPS, T.OFSales,
	-- Regen
	T.RGVol, T.RGUPS, T.RGSales,
	-- Normal
	T.NVol, T.NUPS, T.NSales,
	-- Totales
	T.TVol, T.TUPS, T.TSales,
	-- SelfGenType
	T.SelfGenType
FROM #Totals T
LEFT JOIN dbo.Personnel P ON P.peID = T.Liner
DROP TABLE #Totals

SELECT * FROM @Report ORDER BY SelfGenType, TVol DESC, TSales DESC, TUPS DESC

GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptStatisticsByCloser]    Script Date: 09/22/2016 19:14:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de estadisticas por Closer (Processor Sales)
**
** [aalcocer]	13/Jul/2016 Created
** [ecanul]		12/08/2016 Modified. Ahora el SP regresa todos los campos a pintar en la tabla
**							Agregado parametro Group by teams, para  que traiga o no informacion en dichos campos
**
*/
CREATE procedure [dbo].[USP_IM_RptStatisticsByCloser]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@SalesRoom varchar(10),				-- Clave de la sala
	@SalesmanID varchar(10) = 'ALL',	-- Clave de un vendedor	
	@Segments varchar(8000) = 'ALL',	-- Claves de segmentos
	@Programs varchar(8000) = 'ALL',	-- Programs
	@IncludeAllSalesmen bit = 0,		-- si se desea que esten todos los vendedores de la sala
	@GroupByTeams bit  = 0				-- Si se desea la informacion por equipos e status
as
--SET FMTONLY OFF
set nocount on
--=================== TABLA MANIFEST =============================
--#region ManifestTable
DECLARE @Manifest table (
--campos de huespedes
	id int identity(1,1),
	guID int,	
	guShow bit default 0,
	own bit,	
	TeamSelfGen varchar(20),
	guSelfGen bit,
	salesmanDate DateTime,
	sold bit default 0,
	Opp int default 0,
	MembershipGroup varchar(10),
	-- Campos de la tabla de ventas	
	saID int,
	procSales int,	
	saGrossAmount money default 0,
	-- Vendedores
	-- Liner 1
	Liner1 varchar(10),
	Liner1N varchar(40),
	Liner1P varchar(10),
	-- Liner 2
	Liner2 varchar(10),
	Liner2N varchar(40),
	Liner2P varchar(10),
	-- Closer 1
	Closer1 varchar(10),
	Closer1N varchar(40),
	Closer1P varchar(10),
	-- Closer 2
	Closer2 varchar(10),
	Closer2N varchar(40),
	Closer2p varchar(10),
	-- Closer 3
	Closer3 varchar(10),
	Closer3N varchar(40),
	Closer3p varchar(10),
	-- Exit 1
	Exit1 varchar(10),
	Exit1N varchar(40),
	Exit1P varchar(10),
	-- Exit 2
	Exit2 varchar(10),
	Exit2N varchar(40),
	Exit2P varchar(10));	
--#endregion

--#region Guest
--===================================================================
--=======================   MANIFEST    =============================
--===================================================================
INSERT into @Manifest (guID, guShow, own, salesmanDate, guSelfGen, TeamSelfGen,
saID, procSales, MembershipGroup, Liner1, Liner1N, Liner1P, Liner2, Liner2N, Liner2P,
Closer1, Closer1N, Closer1P, Closer2, Closer2N, Closer2p, Closer3, Closer3N, Closer3p, Exit1, Exit1N, Exit1P, Exit2, Exit2N, Exit2P)
select DISTINCT
	--Campos de la tabla de huespedes
	G.guID,	
	case WHEN (G.guTour=1 OR g.guWalkOut=1 OR ((G.guCTour=1 OR G.guSaveProgram=1) AND S.saID IS NOT NULL)) THEN 1 ELSE 0 END,
	dbo.UFN_IM_StringComparer(G.guLiner1,G.guLiner2,G.guCloser1,G.guCloser2,G.guCloser3,G.guExit1,G.guExit2),
	G.guShowD,
	G.guSelfGen,
	G.guts,	
	S.sagu,
	0 procSales,
	MT.mtGroup,
	-- PERSONAL DEL SHOW	
	-- Vendedores
	-- Liner 1
	G.guLiner1 as guLiner1,
	GL1.peN as guLiner1N,
	CASE WHEN G.guLiner1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guLiner1) ELSE NULL END [Liner1P],
	-- Liner 2
	G.guLiner2 as guLiner2,
	GL2.peN as guLiner2N,
	CASE WHEN G.guLiner2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guLiner2) ELSE NULL END [Liner2P],
	-- Closer 1
	G.guCloser1 as guCloser1,
	GC1.peN as guCloser1N,
	CASE WHEN G.guCloser1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser1) ELSE NULL END [Closer1P],
	-- Closer 2
	G.guCloser2 as guCloser2,
	GC2.peN as guCloser2N,
	CASE WHEN G.guCloser2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser2) ELSE NULL END [Closer2P],
	-- Closer 3
	G.guCloser3 as guCloser3,
	GC3.peN as guCloser3N,
	CASE WHEN G.guCloser3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser3) ELSE NULL END [Closer3P],
	-- Exit 1
	G.guExit1 as guExit1,
	GE1.peN as guExit1N,
	CASE WHEN G.guExit1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guExit1) ELSE NULL END [Exit1P],
	G.guExit2 as guExit2,
	GE2.peN as guExit2N,
	CASE WHEN G.guExit2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guExit2) ELSE NULL END [Exit2P]
from Guests G
	left join Sales S on sagu = G.guID	
	left join MembershipTypes MT on MT.mtID = S.samt
	-- PERSONAL DEL SHOW	
	-- Vendedores
	left join Personnel GL1 on GL1.peID = G.guLiner1
	left join Personnel GL2 on GL2.peID = G.guLiner2
	left join Personnel GC1 on GC1.peID = G.guCloser1
	left join Personnel GC2 on GC2.peID = G.guCloser2
	left join Personnel GC3 on GC3.peID = G.guCloser3
	left join Personnel GE1 on GE1.peID = G.guExit1
	left join Personnel GE2 on GE2.peID = G.guExit2		
	--Segment 
	left join Agencies A on A.agID = G.guag
	left join SegmentsByAgency SA on SA.seID = A.agse
	left join LeadSources LS on LS.lsID = G.guls
	left join SegmentsByLeadSource SL on SL.soID = LS.lsso
	left join SegmentsCategories SCA on SCA.scID = SA.sesc
	left join SegmentsCategories SCL on SCL.scID = SL.sosc
where
	-- Fecha de show
	G.guShowD between @DateFrom and @DateTo
	-- Sala de ventas
	and G.gusr = @SalesRoom	
	-- Vendedor
	and (@SalesmanID = 'ALL'		
		-- Rol de Liner
		or (@SalesmanID = G.guLiner1 or @SalesmanID = G.guLiner2 or @SalesmanID = S.saLiner1 or @SalesmanID = S.saLiner2)
		-- Rol de Closer
		or (@SalesmanID = G.guCloser1 or @SalesmanID = G.guCloser2 or @SalesmanID = G.guCloser3 
			or @SalesmanID = S.saCloser1 or @SalesmanID = S.saCloser2 or @SalesmanID = S.saCloser3)
		-- Rol de Exit
		or (@SalesmanID = G.guExit1 or @SalesmanID = G.guExit2 or @SalesmanID = S.saExit1 or @SalesmanID = S.saExit2))
	-- Segmento
	and (@Segments = 'ALL' or A.agse in (select item from split(@Segments, ',')))
	-- Programa
	and (@Programs = 'ALL' or LS.lspg in (select item from split(@Programs, ',')))
order by G.guID;
--#endregion
--=================== Insert SALES =============================
--#region insertSales

DECLARE @nReg int; -- numero de regitros
SET @nReg=(SELECT COUNT(*) FROM @Manifest);
DECLARE @cont int; -- almacena la cantidad de veces que se recorre el while
SET @cont = 1;
DECLARE @gu int; -- si tiene ventas
WHILE @nReg >= @cont 
BEGIN
	SET @gu = (SELECT guid FROM @Manifest WHERE id = @cont); --obtiene el id del gest	
	DECLARE @sale int;
	SET @sale = (SELECT TOP 1 sagu FROM Sales WHERE sagu = @gu);	
	--#region IF @sale

	IF @sale IS NOT NULL		
	BEGIN		
		UPDATE @Manifest SET sold = 1 WHERE id = @cont;
		SELECT 
		--Datos de la venta
			S.sagu AS GUID,
			dbo.UFN_IM_StringComparer(S.saLiner1,S.saLiner2,S.saCloser1,S.saCloser2,S.saCloser3,S.saExit1,S.saExit2) own,
			S.saID,
			S.saD,	
			S.saGrossAmount,			
			MT.mtGroup,
		--Datos de los vendedores			
			-- Liner 1
			S.saLiner1 as saLiner1,
			SL1.peN as saLiner1N,
			CASE WHEN S.saLiner1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saLiner1) ELSE NULL END [Liner1P],
			-- Liner 2
			S.saLiner2 as saLiner2,
			SL2.peN as saLiner2N,
			CASE WHEN S.saLiner2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saLiner2) ELSE NULL END [Liner2P],
			-- Closer 1
			S.saCloser1 as saCloser1,
			SC1.peN as saCloser1N,
			CASE WHEN S.saCloser1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saCloser1) ELSE NULL END [Closer1P],
			-- Closer 2
			S.saCloser2 as saCloser2,
			SC2.peN as saCloser2N,
			CASE WHEN S.saCloser2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saCloser2) ELSE NULL END [Closer2P],
			-- Closer 3
			S.saCloser3 as saCloser3,
			SC3.peN as saCloser3N,
			CASE WHEN S.saCloser3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saCloser3) ELSE NULL END [Closer3P],
			-- Exit 1
			S.saExit1 as saExit1,
			SE1.peN as saExit1N,
			CASE WHEN S.saExit1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saExit1) ELSE NULL END [Exit1P],
			-- Exit 2
			S.saExit2 as saExit2,
			SE2.peN as saExit2N,
			CASE WHEN S.saExit2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saExit2) ELSE NULL END [Exit2P],			
			1 Sold,
			CASE WHEN ST.ststc = 'DG' THEN 0 ELSE 1 END procSales
		INTO #tbTemp
		FROM Guests G
			LEFT JOIN Sales S on S.sagu = G.guID
			LEFT JOIN SaleTypes ST on ST.stID = S.sast
			left join MembershipTypes MT on MT.mtID = S.samt
			-- Vendedores
			left join Personnel SL1 on SL1.peID = S.saLiner1
			left join Personnel SL2 on SL2.peID = S.saLiner2
			left join Personnel SC1 on SC1.peID = S.saCloser1
			left join Personnel SC2 on SC2.peID = S.saCloser2
			left join Personnel SC3 on SC3.peID = S.saCloser3
			left join Personnel SE1 on SE1.peID = S.saExit1
			left join Personnel SE2 on SE2.peID = S.saExit2	
			-- Segment	
			left join Agencies A on A.agID = G.guag		
			left join SegmentsByAgency SA on SA.seID = A.agse
			left join LeadSources LS on LS.lsID = G.guls
			left join SegmentsByLeadSource SL on SL.soID = LS.lsso
			left join SegmentsCategories SCA on SCA.scID = SA.sesc
			left join SegmentsCategories SCL on SCL.scID = SL.sosc
		WHERE
			S.saD BETWEEN @DateFrom and @DateTo AND sagu = @gu
			AND S.sast NOT IN ('BUMP','REGEN')
			AND S.saProcD BETWEEN @DateFrom AND @DateTo
			AND (S.saCancel = 0 OR NOT S.saCancelD BETWEEN @DateFrom AND @DateTo)
			AND S.sasr = @SalesRoom				
		-- Segmento
		and (@Segments = 'ALL' or A.agse in (select item from split(@Segments, ',')))
		-- Programa
		and (@Programs = 'ALL' or LS.lspg in (select item from split(@Programs, ',')))
		INSERT INTO @Manifest (guID,own, saID,salesmanDate,saGrossAmount, MembershipGroup,
		Liner1,Liner1N,Liner1P,Liner2,Liner2N,Liner2P,Closer1,Closer1N,Closer1P,Closer2,Closer2N,Closer2p,Closer3,Closer3N,Closer3p,
		Exit1,Exit1N,Exit1P,Exit2,Exit2N,Exit2P,sold,procSales)
		SELECT * FROM #tbTemp;
		DROP TABLE #tbTemp;
	END		
--#endregion
	SET @cont = @cont +1;
END;
--#endregion

--===================================================================
--===================  DEPOSIT SALES    =============================
--===================================================================
--#region Deposit Sales
INSERT into @Manifest (guID, own, salesmanDate, guSelfGen, TeamSelfGen, 
procSales, MembershipGroup, Liner1, Liner1N, Liner1P, Liner2, Liner2N, Liner2P, 
Closer1, Closer1N, Closer1P, Closer2, Closer2N, Closer2p, Closer3, Closer3N, Closer3p, Exit1, Exit1N, Exit1P, Exit2, Exit2N, Exit2P)
select 
	--#region Campos de la tabla de huespedes
	G.guID,	
	dbo.UFN_IM_StringComparer(G.guLiner1,G.guLiner2,G.guCloser1,G.guCloser2,G.guCloser3,G.guExit1,G.guExit2),
	G.guShowD,
	G.guSelfGen,
	G.guts,
	0 procSales,
	MT.mtGroup,
	--#endregion	
	--#region PERSONAL DEL SHOW	
	-- Vendedores
	-- Liner 1
	G.guLiner1 as guLiner1,
	GL1.peN as guLiner1N,
	CASE WHEN G.guLiner1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guLiner1) ELSE NULL END [Liner1P],
	-- Liner 2
	G.guLiner2 as guLiner2,
	GL2.peN as guLiner2N,
	CASE WHEN G.guLiner2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guLiner2) ELSE NULL END [Liner2P],
	-- Closer 1
	G.guCloser1 as guCloser1,
	GC1.peN as guCloser1N,
	CASE WHEN G.guCloser1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser1) ELSE NULL END [Closer1P],
	-- Closer 2
	G.guCloser2 as guCloser2,
	GC2.peN as guCloser2N,
	CASE WHEN G.guCloser2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser2) ELSE NULL END [Closer2P],
	-- Closer 3
	G.guCloser3 as guCloser3,
	GC3.peN as guCloser3N,
	CASE WHEN G.guCloser3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser3) ELSE NULL END [Closer3P],
	-- Exit 1
	G.guExit1 as guExit1,
	GE1.peN as guExit1N,
	CASE WHEN G.guExit1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guExit1) ELSE NULL END [Exit1P],
	-- Exit 2
	G.guExit2 as guExit2,
	GE2.peN as guExit2N,
	CASE WHEN G.guExit2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guExit2) ELSE NULL END [Exit2P]
	--#endregion
from Guests G	
	left join Sales S on S.sagu = G.guID	
	left join MembershipTypes MT on MT.mtID = S.samt
--#region PERSONAL DEL SHOW	
	-- Vendedores
	left join Personnel GL1 on GL1.peID = G.guLiner1
	left join Personnel GL2 on GL2.peID = G.guLiner2
	left join Personnel GC1 on GC1.peID = G.guCloser1
	left join Personnel GC2 on GC2.peID = G.guCloser2
	left join Personnel GC3 on GC3.peID = G.guCloser3
	left join Personnel GE1 on GE1.peID = G.guExit1
	left join Personnel GE2 on GE2.peID = G.guExit2	
	--#endregion	
	-- Segment
	left join Agencies A on A.agID = G.guag
	left join SegmentsByAgency SA on SA.seID = A.agse
	left join LeadSources LS on LS.lsID = G.guls
	left join SegmentsByLeadSource SL on SL.soID = LS.lsso
	left join SegmentsCategories SCA on SCA.scID = SA.sesc
	left join SegmentsCategories SCL on SCL.scID = SL.sosc
where
	G.guDepositSaleD between @DateFrom and @DateTo
	-- Fecha de show diferente de la fecha de venta deposito
	and G.guShowD <> G.guDepositSaleD
	-- Sala de ventas
	and G.gusr = @SalesRoom
	-- Vendedor 
	and (@SalesmanID = 'ALL'		
		-- Rol de Liner
		or (@SalesmanID = G.guLiner1 or @SalesmanID = G.guLiner2 or @SalesmanID = S.saLiner1 or @SalesmanID = S.saLiner2)
		-- Rol de Closer
		or (@SalesmanID = G.guCloser1 or @SalesmanID = G.guCloser2 or @SalesmanID = G.guCloser3
			or @SalesmanID = S.saCloser1 or @SalesmanID = S.saCloser2 or @SalesmanID = S.saCloser3)
		-- Rol de Exit
		or (@SalesmanID = G.guExit1 or @SalesmanID = G.guExit2 or @SalesmanID = S.saExit1 or @SalesmanID = S.saExit2))
	-- Segmento
	and (@Segments = 'ALL' or A.agse in (select item from split(@Segments, ',')))
	-- Programa
	and (@Programs = 'ALL' or LS.lspg in (select item from split(@Programs, ',')))
order by G.guID;
--#endregion
--===================================================================
--======================  OTHER SALES    ============================
--===================================================================
--#region Other Sales
INSERT into @Manifest (guID, own, guSelfGen, salesmanDate,sold, Opp, saID, procSales, saGrossAmount, MembershipGroup,
Liner1, Liner1N, Liner1P, Liner2, Liner2N, Liner2P, Closer1, Closer1N, Closer1P, Closer2, Closer2N, 
Closer2p, Closer3, Closer3N, Closer3p, Exit1, Exit1N, Exit1P, Exit2, Exit2N, Exit2P)
SELECT 
--Datos de la venta
	S.sagu AS GUID,
	dbo.UFN_IM_StringComparer(S.saLiner1,S.saLiner2,S.saCloser1,S.saCloser2,S.saCloser3,S.saExit1,S.saExit2),
	S.saSelfGen,
	S.saD,
	1 Sold,
	CASE WHEN dbo.UFN_IM_GetSaleType(@DateFrom,@DateTo,S.sast,ST.ststc,G.guDepSale,S.saD,S.saProcD,S.saCancelD,G.gusr,S.sasr,S.saByPhone) IN(6,10) THEN 1 ELSE 0 END [Opp],
	S.saID,
	CASE WHEN dbo.UFN_IM_GetSaleType(@DateFrom,@DateTo,S.sast,ST.ststc,G.guDepSale,S.saD,S.saProcD,S.saCancelD,G.gusr,S.sasr,S.saByPhone) NOT IN(5,14) THEN 1 ELSE 0 END [procSales],
	S.saGrossAmount,
	MT.mtGroup,
--Datos de los vendedores	
	-- Liner 1
	S.saLiner1 as saLiner1,
	SL1.peN as saLiner1N,
	CASE WHEN S.saLiner1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saLiner1) ELSE NULL END [Liner1P],
	-- Liner 2
	S.saLiner2 as saLiner2,
	SL2.peN as saLiner2N,
	CASE WHEN S.saLiner2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saLiner2) ELSE NULL END [Liner2P],
	-- Closer 1
	S.saCloser1 as saCloser1,
	SC1.peN as saCloser1N,
	CASE WHEN S.saCloser1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saCloser1) ELSE NULL END [Closer1P],
	-- Closer 2
	S.saCloser2 as saCloser2,
	SC2.peN as saCloser2N,
	CASE WHEN S.saCloser2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saCloser2) ELSE NULL END [Closer2P],
	-- Closer 3
	S.saCloser3 as saCloser3,
	SC3.peN as saCloser3N,
	CASE WHEN S.saCloser3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saCloser3) ELSE NULL END [Closer3P],
	-- Exit 1
	S.saExit1 as saExit1,
	SE1.peN as saExit1N,
	CASE WHEN S.saExit1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saExit1) ELSE NULL END [Exit1P],
	-- Exit 2
	S.saExit2 as saExit2,
	SE2.peN as saExit2N,
	CASE WHEN S.saExit2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saExit2) ELSE NULL END [Exit2P]
FROM Guests G --para obtener la fecha del show
	LEFT JOIN Sales S on S.sagu = G.guID
	LEFT JOIN SaleTypes ST on ST.stID = S.sast
	left join MembershipTypes MT on MT.mtID = S.samt
	-- Vendedores
	left join Personnel SL1 on SL1.peID = S.saLiner1
	left join Personnel SL2 on SL2.peID = S.saLiner2
	left join Personnel SC1 on SC1.peID = S.saCloser1
	left join Personnel SC2 on SC2.peID = S.saCloser2
	left join Personnel SC3 on SC3.peID = S.saCloser3
	left join Personnel SE1 on SE1.peID = S.saExit1
	left join Personnel SE2 on SE2.peID = S.saExit2		
	-- Segment	
	left join Agencies A on A.agID = G.guag
	left join SegmentsByAgency SA on SA.seID = A.agse
	left join LeadSources LS on LS.lsID = S.sals
	left join SegmentsByLeadSource SL on Sl.soID = LS.lsso
	left join SegmentsCategories SCA on SCA.scID = SA.sesc
	left join SegmentsCategories SCL on SCL.scID = SL.sosc
WHERE
	((--NO SHOWS
	(G.guShowD IS NULL OR NOT G.guShowD BETWEEN @DateFrom AND @DateTo)
	--FECHA DE --- VENTA	-----------------	Venta Procesable	-----------------------
	AND (S.saD BETWEEN @DateFrom AND @DateTo OR S.saProcD BETWEEN @DateFrom AND @DateTo --OR s.saCancelD BETWEEN @DateFrom AND @DateTo
	))
	-- Incluye Bumps, Regen y ventas de otras salas
	OR ((S.sast IN ('BUMP','REGEN') OR G.gusr <> S.sasr) AND S.saD BETWEEN @DateFrom AND @DateTo))
	AND S.saProcD BETWEEN @DateFrom AND @DateTo
	AND (S.saCancel = 0 OR NOT S.saCancelD BETWEEN @DateFrom AND @DateTo)
	AND S.sasr = @SalesRoom
	-- Vendedor
	and (@SalesmanID = 'ALL'
		-- Rol de Liner
		or (@SalesmanID = S.saLiner1 or @SalesmanID = S.saLiner2)
		-- Rol de Closer
		or (@SalesmanID = S.saCloser1 or @SalesmanID = S.saCloser2 or @SalesmanID = S.saCloser3)
		-- Rol de Exit
		or (@SalesmanID = S.saExit1 or @SalesmanID = S.saExit2))
	-- Segmento
	and (@Segments = 'ALL' or A.agse in (select item from split(@Segments, ',')))
	-- Programa
	and (@Programs = 'ALL' or LS.lspg in (select item from split(@Programs, ',')))
ORDER BY S.sagu;
--#endregion


--=================== TABLA Salesman =============================
--#region Salesman Table
DECLARE @Salesman table (
	id int,	
	SalemanID varchar(10),
	SalemanName varchar(40),
	UPS money,
	Sales money,
	Amount money,
	Role varchar(50),
	SalemanType varchar(50)
	);
--#endregion
	
	--=================== Insert Salesman =============================
	--#region Insert Salesman

	--#region Liner1
INSERT into @Salesman (id, SalemanID,SalemanName, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Liner1 ,Liner1N, 'Liner',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Liner1,Liner2,default,default,default),
	dbo.UFN_IM_GetSalesmanTypesCloser('Liner',sold,own),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Liner1,Liner2,default,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Liner1,Liner2,default,default,default)
	from @Manifest WHERE Liner1 is NOT NULL AND Liner1P='CLOSER'
--#endregion
	
	--#region Liner2
INSERT into @Salesman (id, SalemanID,SalemanName, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Liner2 ,Liner2N, 'Liner',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Liner1,Liner2,default,default,default),
	dbo.UFN_IM_GetSalesmanTypesCloser('Liner',sold,own),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Liner1,Liner2,default,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Liner1,Liner2,default,default,default)
	from @Manifest m WHERE Liner2 is NOT NULL AND Liner2P='CLOSER' AND NOT EXISTS (SELECT * from @Salesman s WHERE s.id=m.id AND s.SalemanID =m.Liner2)
--#endregion
	
	--#region Closer1
	INSERT into @Salesman (id, SalemanID,SalemanName, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Closer1 ,Closer1N, 'Closer',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	dbo.UFN_IM_GetSalesmanTypesCloser('Closer',sold,own),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default)
	from @Manifest m WHERE Closer1 is NOT NULL AND Closer1P='CLOSER' AND NOT EXISTS (SELECT * from @Salesman s WHERE s.id=m.id AND s.SalemanID =m.Closer1);
	--#endregion
	
	--#region Closer2
	INSERT into @Salesman (id, SalemanID,SalemanName, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Closer2 ,Closer2N, 'Closer',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	dbo.UFN_IM_GetSalesmanTypesCloser('Closer',sold,own),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default)
	from @Manifest m WHERE Closer2 is NOT NULL AND Closer2P='CLOSER' AND NOT EXISTS (SELECT * from @Salesman s WHERE s.id=m.id AND s.SalemanID =m.Closer2)
	--#endregion
	
	--#region Closer3
	INSERT into @Salesman (id, SalemanID,SalemanName, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Closer3 ,Closer3N, 'Closer',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	dbo.UFN_IM_GetSalesmanTypesCloser('Closer',sold,own),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default)
	from @Manifest m WHERE Closer3 is NOT NULL AND Closer3P='CLOSER' AND NOT EXISTS (SELECT * from @Salesman s WHERE s.id=m.id AND s.SalemanID =m.Closer3)	
	--#endregion
	
	--#region Exit1
	INSERT into @Salesman (id, SalemanID,SalemanName, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Exit1 ,Exit1N,  'Exit',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default),
	dbo.UFN_IM_GetSalesmanTypesCloser('Exit',sold,own),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default)
	from @Manifest m WHERE Exit1 is NOT NULL AND Exit1P='CLOSER' AND NOT EXISTS (SELECT * from @Salesman s WHERE s.id=m.id AND s.SalemanID =m.Exit1)	
	--#endregion
	
	--#region Exit2
	INSERT into @Salesman (id, SalemanID,SalemanName, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Exit2 ,Exit2N, 'Exit',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default),
	dbo.UFN_IM_GetSalesmanTypesCloser('Exit',sold,own),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default)
	from @Manifest m WHERE Exit2 is NOT NULL AND Exit2P='CLOSER' AND NOT EXISTS (SELECT * from @Salesman s WHERE s.id=m.id AND s.SalemanID =m.Exit2)	
--#endregion
	
	--=================== TABLA StatsBySegment =============================
--#region StatsBySegment Table
DECLARE @StatsByCloser table (
	SalemanID varchar(10),
	SalemanName varchar(40),
	SalemanType varchar(5),
	SalemanTypeN varchar(50),
	TeamN varchar(30),
	TeamLeaderN varchar(40),
	SalesmanStatus VARCHAR(10) DEFAULT 'ACTIVE',
	SalesAmount money,
	OPP int,
	UPS money,
	SalesRegular money,
	SalesExit money,
	Sales money,	
	Efficiency money,
	ClosingFactor money,
	SaleAverage money
	);
--#endregion

	INSERT INTO @StatsByCloser
	SELECT SalemanID, SalemanName, SalemanType, 
	CASE SalemanType WHEN 'AS' THEN 'As Front To Back' WHEN 'WITH' THEN 'With Junior' WHEN 'OWN' THEN 'Closer' END AS SalemanTypeN,
	TeamN, TeamLeaderN, SalesmanStatus,
	sum(Amount) AS SalesAmount, sum(Opp) AS Opp, sum(UPS) AS UPS,
	SUM(Sales) AS SalesRegular, Sum([Exit]) AS SalesExit, SUM (Sales + [Exit]) as Sales,
	dbo.UFN_OR_SecureDivision( sum(Amount),sum(UPS)) Efficiency,
	dbo.UFN_OR_SecureDivision(SUM (Sales + [Exit]),sum(UPS)) ClosingFactor,
	dbo.UFN_OR_SecureDivision(SUM (Amount),SUM (Sales + [Exit])) SaleAverage
	FROM(
		SELECT DISTINCT m.guID, m.saID, s.SalemanID,s.SalemanName,s.SalemanType,
		s.UPS, s.Amount, m.Opp,
		CASE WHEN m.MembershipGroup='EXIT' THEN 0 ELSE s.Sales END AS Sales,
		CASE WHEN m.MembershipGroup='EXIT' THEN s.Sales ELSE 0 END AS [Exit],
		CASE WHEN t.SalesRoom = @SalesRoom AND ISNULL(CONVERT(varchar(1),tc.tsN),'') <> ISNULL(CONVERT(varchar(1),t.TeamN),'') THEN 'INACTIVE' ELSE 'ACTIVE'  END AS SalesmanStatus,
		CASE WHEN t.SalesRoom <> @SalesRoom THEN 'TEAMS OF OTHER SALES ROOMS' ELSE 
			CASE WHEN t.TeamN IS NULL THEN --si no tiene equipo
				CASE WHEN dbo.UFN_IM_IsSelfGen(s.SalemanID,s.Role,m.guSelfGen)=1 THEN -- si es un Self Gen obtenemos el equipo que asigno la Hostess
				ISNULL(CONVERT(varchar(40),m.TeamSelfGen),'NO TEAM')
				ELSE 'NO TEAM' END
			ELSE t.TeamN END
		END AS TeamN,
		CASE WHEN t.SalesRoom <> @SalesRoom THEN '' ELSE ISNULL(CONVERT(varchar(40),t.TeamLeaderN),'') END AS TeamLeaderN
		FROM @Salesman s	
		INNER JOIN @Manifest m ON m.id = s.id
		INNER JOIN Personnel p on p.peID= s.SalemanID
		left join TeamsSalesmen tc on peTeam = tsID	
		cross apply  dbo.UFN_IM_GetPersonnelTeamSalesmenByDate(s.SalemanID,m.salesmanDate) t
		WHERE SalemanType IS NOT NULL
		) AS x
		GROUP by SalemanID,SalemanName,SalemanType, TeamN, TeamLeaderN, x.SalesmanStatus


IF @IncludeAllSalesmen=1 AND  (SELECT COUNT(*) from @StatsByCloser)>0
BEGIN			
	INSERT INTO @StatsByCloser(SalemanID, SalemanName, SalemanTypeN, TeamN, TeamLeaderN)
	select P.peID, P.peN as SalesmanN, (SELECT top 1 SalemanTypeN from @StatsByCloser) , IsNull(tsN, 'NO TEAM'), IsNull(L.peN, '')
	from Personnel P 
	left join TeamsSalesmen on P.peTeamType = 'SA' and P.pePlaceID = tssr and P.peTeam = tsID 
	left join Personnel L on tsLeader = L.peID
	where
		-- Tipo de equipo
		P.peTeamType = 'SA'
		-- Clave del lugar
		and P.pePlaceID = @SalesRoom
		-- Personal activo
		and P.peA = 1
		-- Filtro
		and P.pepo = 'CLOSER'
		--si no se encuentra el vendedor
		AND NOT EXISTS (SELECT * from @StatsByCloser  WHERE SalemanID = p.peID)
END	
		
--===================================================================
--===================  SELECT  ===========================
--===================================================================
IF @GroupByTeams = 1
BEGIN
	SELECT DISTINCT 
		SalemanID,
		SalemanName,
		(TeamN + '  ' + TeamLeaderN) Team,
		SalesmanStatus,
		--TOTALS
		SUM(sbc.TSalesAmount) TSalesAmount,
		SUM(sbc.TOOP) TOOP,
		SUM(sbc.TUPS) TUPS,
		SUM(sbc.TSalesRegular) TSalesRegular, 
		SUM(sbc.TSalesExit) TSalesExit, 
		SUM(sbc.TSales) TSales, 
		SUM(sbc.TEfficiency) TEfficiency, 
		SUM(sbc.TClosingFactor) TClosingFactor, 
		SUM(sbc.TSaleAverage) TSaleAverage,
		--- Closers
		SUM(sbc.CSalesAmount) CSalesAmount, 
		SUM(sbc.COOP) COOP, 
		SUM(sbc.CUPS) CUPS, 
		sum(sbc.CSalesRegular) CSalesRegular, 
		SUM(sbc.CSalesExit) CSalesExit, 
		SUM(sbc.CSalesExit) CSales, 
		SUM(sbc.CEfficiency) CEfficiency, 
		SUM(sbc.CClosingFactor) CClosingFactor, 
		SUM(sbc.CSaleAverage) CSaleAverage,
		--- As FTB
		SUM(sbc.AsSalesAmount) AsSalesAmount, 
		SUM(sbc.AsOOP) AsOOP, 
		SUM(sbc.AsUPS) AsUPS, 
		SUM(sbc.AsSalesRegular) AsSalesRegular, 
		SUM(sbc.AsSalesExit) AsSalesExit, 
		SUM(sbc.AsSales) AsSales, 
		SUM(sbc.AsEfficiency) AsEfficiency, 
		SUM(sbc.AsClosingFactor) AsClosingFactor, 
		SUM(sbc.AsSaleAverage) AsSaleAverage,
		--- With Junior
		SUM(sbc.WSalesAmount) WSalesAmount, 
		SUM(sbc.WOOP) WOOP, 
		SUM(sbc.WUPS) WUPS, 
		SUM(sbc.WSalesRegular) WSalesRegular, 
		SUM(sbc.WSalesExit) WSalesExit, 
		SUM(sbc.WSales) WSales, 
		SUM(sbc.WEfficiency) WEfficiency, 
		SUM(sbc.WClosingFactor) WClosingFactor, 
		SUM(sbc.WSaleAverage) WSaleAverage,
		--- Tot As FTB and With Junior
		SUM(sbc.AWSalesAmount) AWSalesAmount, 
		SUM(sbc.AWOOP) AWOOP, 
		SUM(sbc.AWUPS) AWUPS, 
		SUM(sbc.AWSalesRegular) AWSalesRegular, 
		SUM(sbc.AWSalesExit) AWSalesExit, 
		SUM(sbc.AWSales) AWSales, 
		SUM(sbc.AWEfficiency) AWEfficiency, 
		SUM(sbc.AWClosingFactor) AWClosingFactor, 
		SUM(sbc.AWSaleAverage) AWSaleAverage
	FROM
	(
		---------------------------------------TOTALS
		SELECT SalemanID, SalemanName,
			TeamN, TeamLeaderN, SalesmanStatus,	
			-----Totales
			sum(SalesAmount) AS TSalesAmount, sum(OPP) as TOOP, sum(UPS) as TUPS, 
			sum(SalesRegular) as TSalesRegular, sum(SalesExit) as TSalesExit, sum(Sales) as TSales,		
			dbo.UFN_OR_SecureDivision( sum(SalesAmount),sum(UPS)) TEfficiency,
			dbo.UFN_OR_SecureDivision(SUM (Sales),sum(UPS)) TClosingFactor,
			dbo.UFN_OR_SecureDivision(SUM (SalesAmount),SUM (Sales)) TSaleAverage,    
			--- Closers
			0 CSalesAmount, 0 COOP, 0 CUPS, 0 CSalesRegular, 0 CSalesExit, 0 CSales, 0 CEfficiency, 0 CClosingFactor, 0 CSaleAverage,
			--- As FTB
			0 AsSalesAmount, 0 AsOOP, 0 AsUPS, 0 AsSalesRegular, 0 AsSalesExit, 0 AsSales, 0 AsEfficiency, 0 AsClosingFactor, 0 AsSaleAverage,
			--- With Junior
			0 WSalesAmount, 0 WOOP, 0 WUPS, 0 WSalesRegular, 0 WSalesExit, 0 WSales, 0 WEfficiency, 0 WClosingFactor, 0 WSaleAverage,
			--- Tot As FTB and With Junior
			0 AWSalesAmount, 0 AWOOP, 0 AWUPS, 0 AWSalesRegular, 0 AWSalesExit, 0 AWSales, 0 AWEfficiency, 0 AWClosingFactor, 0 AWSaleAverage		
		FROM @StatsByCloser  
		GROUP BY SalemanID,SalemanName,TeamN,TeamLeaderN,SalesmanStatus
		UNION ALL 
		--------------------------------------- CLOSERS
		SELECT
			SalemanID, SalemanName,	
			TeamN, TeamLeaderN, SalesmanStatus,	
			-- TOTALES
			0 TSalesAmount, 0 TOOP, 0 TUPS, 0 TSalesRegular, 0 TSalesExit, 0 TSales, 0 TEfficiency, 0 TClosingFactor, 0 TSaleAverage,
			-- CLOSERS
			SalesAmount CSalesAmount, OPP COOP, UPS CUPS, SalesRegular CSalesRegular, SalesExit CSalesExit, Sales CSales, Efficiency CEfficiency, ClosingFactor CClosingFactor, SaleAverage CSaleAverage,
			--- As FTB
			0 AsSalesAmount, 0 AsOOP, 0 AsUPS, 0 AsSalesRegular, 0 AsSalesExit, 0 AsSales, 0 AsEfficiency, 0 AsClosingFactor, 0 AsSaleAverage,
			--- With Junior
			0 WSalesAmount, 0 WOOP, 0 WUPS, 0 WSalesRegular, 0 WSalesExit, 0 WSales, 0 WEfficiency, 0 WClosingFactor, 0 WSaleAverage,
			--- Tot As FTB and With Junior
			0 AWSalesAmount, 0 AWOOP, 0 AWUPS, 0 AWSalesRegular, 0 AWSalesExit, 0 AWSales, 0 AWEfficiency, 0 AWClosingFactor, 0 AWSaleAverage
		from @StatsByCloser
		WHERE SalemanType = 'OWN'
		GROUP BY SalemanID,SalemanName,SalemanType, SalemanTypeN, TeamN,TeamLeaderN,SalesmanStatus,
			SalesAmount, OPP, UPS, SalesRegular, SalesExit, Sales, Efficiency, ClosingFactor, SaleAverage
		--------- As Front To Back
		UNION ALL
		SELECT 
			SalemanID, SalemanName,	
			TeamN, TeamLeaderN, SalesmanStatus,	
			-- TOTALES
			0 TSalesAmount, 0 TOOP, 0 TUPS, 0 TSalesRegular, 0 TSalesExit, 0 TSales, 0 TEfficiency, 0 TClosingFactor, 0 TSaleAverage,
			--- Closers
			0 CSalesAmount, 0 COOP, 0 CUPS, 0 CSalesRegular, 0 CSalesExit, 0 CSales, 0 CEfficiency, 0 CClosingFactor, 0 CSaleAverage,
			--- As FTB
			SalesAmount, OPP, UPS, SalesRegular, SalesExit, Sales, Efficiency, ClosingFactor, SaleAverage,
			--- With Junior
			0 WSalesAmount, 0 WOOP, 0 WUPS, 0 WSalesRegular, 0 WSalesExit, 0 WSales, 0 WEfficiency, 0 WClosingFactor, 0 WSaleAverage,
			--- Tot As FTB and With Junior
			0 AWSalesAmount, 0 AWOOP, 0 AWUPS, 0 AWSalesRegular, 0 AWSalesExit, 0 AWSales, 0 AWEfficiency, 0 AWClosingFactor, 0 AWSaleAverage
		FROM @StatsByCloser
		WHERE SalemanType = 'AS'
		GROUP BY SalemanID,SalemanName,SalemanType, SalemanTypeN, TeamN,TeamLeaderN,SalesmanStatus,
			SalesAmount, OPP, UPS, SalesRegular, SalesExit, Sales, Efficiency, ClosingFactor, SaleAverage
		--------- With Junior
		UNION ALL
		SELECT 
			SalemanID, SalemanName,
			TeamN, TeamLeaderN, SalesmanStatus,	
			-- TOTALES
			0 TSalesAmount, 0 TOOP, 0 TUPS, 0 TSalesRegular, 0 TSalesExit, 0 TSales, 0 TEfficiency, 0 TClosingFactor, 0 TSaleAverage,
			--- Closers
			0 CSalesAmount, 0 COOP, 0 CUPS, 0 CSalesRegular, 0 CSalesExit, 0 CSales, 0 CEfficiency, 0 CClosingFactor, 0 CSaleAverage,
			--- As FTB
			0 AsSalesAmount, 0 AsOOP, 0 AsUPS, 0 AsSalesRegular, 0 AsSalesExit, 0 AsSales, 0 AsEfficiency, 0 AsClosingFactor, 0 AsSaleAverage,
			--- With Junior
			SalesAmount, OPP, UPS, SalesRegular, SalesExit, Sales, Efficiency, ClosingFactor, SaleAverage,
			--- Tot As FTB and With Junior
			0 AWSalesAmount, 0 AWOOP, 0 AWUPS, 0 AWSalesRegular, 0 AWSalesExit, 0 AWSales, 0 AWEfficiency, 0 AWClosingFactor, 0 AWSaleAverage
		FROM @StatsByCloser
		WHERE SalemanType = 'WITH'
		GROUP BY SalemanID,SalemanName,SalemanType, SalemanTypeN, TeamN,TeamLeaderN,SalesmanStatus,
			SalesAmount, OPP, UPS, SalesRegular, SalesExit, Sales, Efficiency, ClosingFactor, SaleAverage
		--------- Total As Front To back and with Junior
		UNION ALL
		SELECT SalemanID, SalemanName,
			TeamN, TeamLeaderN, SalesmanStatus,	
			-- TOTALES
			0 TSalesAmount, 0 TOOP, 0 TUPS, 0 TSalesRegular, 0 TSalesExit, 0 TSales, 0 TEfficiency, 0 TClosingFactor, 0 TSaleAverage,
			--- Closers
			0 CSalesAmount, 0 COOP, 0 CUPS, 0 CSalesRegular, 0 CSalesExit, 0 CSales, 0 CEfficiency, 0 CClosingFactor, 0 CSaleAverage,
			--- As FTB
			0 AsSalesAmount, 0 AsOOP, 0 AsUPS, 0 AsSalesRegular, 0 AsSalesExit, 0 AsSales, 0 AsEfficiency, 0 AsClosingFactor, 0 AsSaleAverage,
			--- With Junior
			0 WSalesAmount, 0 WOOP, 0 WUPS, 0 WSalesRegular, 0 WSalesExit, 0 WSales, 0 WEfficiency, 0 WClosingFactor, 0 WSaleAverage,
			--- Tot As FTB and With Junior
			sum(SalesAmount) AS AWSalesAmount, sum(OPP) as AWOPP, sum(UPS) as AWUPS, 
			sum(SalesRegular) as AWSalesRegular, sum(SalesExit) as AWSalesExit, sum(Sales) as AWSales,		
			dbo.UFN_OR_SecureDivision( sum(SalesAmount),sum(UPS)) AWEfficiency,
			dbo.UFN_OR_SecureDivision(SUM (Sales),sum(UPS)) AWClosingFactor,
			dbo.UFN_OR_SecureDivision(SUM (SalesAmount),SUM (Sales)) AWSaleAverage    
		FROM @StatsByCloser  
		WHERE SalemanType in ('AS','WITH')
		GROUP BY SalemanID,SalemanName,TeamN,TeamLeaderN,SalesmanStatus
	)AS sbc
	GROUP BY SalemanID, SalemanName, TeamN, TeamLeaderN, SalesmanStatus
	ORDER BY TSalesAmount DESC, SalemanID
END
ELSE
BEGIN
	SELECT DISTINCT 
		SalemanID,
		SalemanName,
		(TeamN + '  ' + TeamLeaderN) Team,
		SalesmanStatus,
		--TOTALS
		SUM(sbc.TSalesAmount) TSalesAmount,
		SUM(sbc.TOOP) TOOP,
		SUM(sbc.TUPS) TUPS,
		SUM(sbc.TSalesRegular) TSalesRegular, 
		SUM(sbc.TSalesExit) TSalesExit, 
		SUM(sbc.TSales) TSales, 
		SUM(sbc.TEfficiency) TEfficiency, 
		SUM(sbc.TClosingFactor) TClosingFactor, 
		SUM(sbc.TSaleAverage) TSaleAverage,
		--- Closers
		SUM(sbc.CSalesAmount) CSalesAmount, 
		SUM(sbc.COOP) COOP, 
		SUM(sbc.CUPS) CUPS, 
		sum(sbc.CSalesRegular) CSalesRegular, 
		SUM(sbc.CSalesExit) CSalesExit, 
		SUM(sbc.CSalesExit) CSales, 
		SUM(sbc.CEfficiency) CEfficiency, 
		SUM(sbc.CClosingFactor) CClosingFactor, 
		SUM(sbc.CSaleAverage) CSaleAverage,
		--- As FTB
		SUM(sbc.AsSalesAmount) AsSalesAmount, 
		SUM(sbc.AsOOP) AsOOP, 
		SUM(sbc.AsUPS) AsUPS, 
		SUM(sbc.AsSalesRegular) AsSalesRegular, 
		SUM(sbc.AsSalesExit) AsSalesExit, 
		SUM(sbc.AsSales) AsSales, 
		SUM(sbc.AsEfficiency) AsEfficiency, 
		SUM(sbc.AsClosingFactor) AsClosingFactor, 
		SUM(sbc.AsSaleAverage) AsSaleAverage,
		--- With Junior
		SUM(sbc.WSalesAmount) WSalesAmount, 
		SUM(sbc.WOOP) WOOP, 
		SUM(sbc.WUPS) WUPS, 
		SUM(sbc.WSalesRegular) WSalesRegular, 
		SUM(sbc.WSalesExit) WSalesExit, 
		SUM(sbc.WSales) WSales, 
		SUM(sbc.WEfficiency) WEfficiency, 
		SUM(sbc.WClosingFactor) WClosingFactor, 
		SUM(sbc.WSaleAverage) WSaleAverage,
		--- Tot As FTB and With Junior
		SUM(sbc.AWSalesAmount) AWSalesAmount, 
		SUM(sbc.AWOOP) AWOOP, 
		SUM(sbc.AWUPS) AWUPS, 
		SUM(sbc.AWSalesRegular) AWSalesRegular, 
		SUM(sbc.AWSalesExit) AWSalesExit, 
		SUM(sbc.AWSales) AWSales, 
		SUM(sbc.AWEfficiency) AWEfficiency, 
		SUM(sbc.AWClosingFactor) AWClosingFactor, 
		SUM(sbc.AWSaleAverage) AWSaleAverage
	FROM
	(
		---------------------------------------TOTALS
		SELECT SalemanID, SalemanName,
			'' TeamN, '' TeamLeaderN, '' SalesmanStatus,
			-----Totales
			sum(SalesAmount) AS TSalesAmount, sum(OPP) as TOOP, sum(UPS) as TUPS, 
			sum(SalesRegular) as TSalesRegular, sum(SalesExit) as TSalesExit, sum(Sales) as TSales,		
			dbo.UFN_OR_SecureDivision( sum(SalesAmount),sum(UPS)) TEfficiency,
			dbo.UFN_OR_SecureDivision(SUM (Sales),sum(UPS)) TClosingFactor,
			dbo.UFN_OR_SecureDivision(SUM (SalesAmount),SUM (Sales)) TSaleAverage,    
			--- Closers
			0 CSalesAmount, 0 COOP, 0 CUPS, 0 CSalesRegular, 0 CSalesExit, 0 CSales, 0 CEfficiency, 0 CClosingFactor, 0 CSaleAverage,
			--- As FTB
			0 AsSalesAmount, 0 AsOOP, 0 AsUPS, 0 AsSalesRegular, 0 AsSalesExit, 0 AsSales, 0 AsEfficiency, 0 AsClosingFactor, 0 AsSaleAverage,
			--- With Junior
			0 WSalesAmount, 0 WOOP, 0 WUPS, 0 WSalesRegular, 0 WSalesExit, 0 WSales, 0 WEfficiency, 0 WClosingFactor, 0 WSaleAverage,
			--- Tot As FTB and With Junior
			0 AWSalesAmount, 0 AWOOP, 0 AWUPS, 0 AWSalesRegular, 0 AWSalesExit, 0 AWSales, 0 AWEfficiency, 0 AWClosingFactor, 0 AWSaleAverage		
		FROM @StatsByCloser  
		GROUP BY SalemanID,SalemanName,TeamN,TeamLeaderN,SalesmanStatus
		UNION ALL 
		--------------------------------------- CLOSERS
		SELECT
			SalemanID, SalemanName,	
			'' TeamN, '' TeamLeaderN, '' SalesmanStatus,
			-- TOTALES
			0 TSalesAmount, 0 TOOP, 0 TUPS, 0 TSalesRegular, 0 TSalesExit, 0 TSales, 0 TEfficiency, 0 TClosingFactor, 0 TSaleAverage,
			-- CLOSERS
			SalesAmount CSalesAmount, OPP COOP, UPS CUPS, SalesRegular CSalesRegular, SalesExit CSalesExit, Sales CSales, Efficiency CEfficiency, ClosingFactor CClosingFactor, SaleAverage CSaleAverage,
			--- As FTB
			0 AsSalesAmount, 0 AsOOP, 0 AsUPS, 0 AsSalesRegular, 0 AsSalesExit, 0 AsSales, 0 AsEfficiency, 0 AsClosingFactor, 0 AsSaleAverage,
			--- With Junior
			0 WSalesAmount, 0 WOOP, 0 WUPS, 0 WSalesRegular, 0 WSalesExit, 0 WSales, 0 WEfficiency, 0 WClosingFactor, 0 WSaleAverage,
			--- Tot As FTB and With Junior
			0 AWSalesAmount, 0 AWOOP, 0 AWUPS, 0 AWSalesRegular, 0 AWSalesExit, 0 AWSales, 0 AWEfficiency, 0 AWClosingFactor, 0 AWSaleAverage
		from @StatsByCloser
		WHERE SalemanType = 'OWN'
		GROUP BY SalemanID,SalemanName,SalemanType, SalemanTypeN, TeamN,TeamLeaderN,SalesmanStatus,
			SalesAmount, OPP, UPS, SalesRegular, SalesExit, Sales, Efficiency, ClosingFactor, SaleAverage
		--------- As Front To Back
		UNION ALL
		SELECT 
			SalemanID, SalemanName,	
			'' TeamN, '' TeamLeaderN, '' SalesmanStatus,
			-- TOTALES
			0 TSalesAmount, 0 TOOP, 0 TUPS, 0 TSalesRegular, 0 TSalesExit, 0 TSales, 0 TEfficiency, 0 TClosingFactor, 0 TSaleAverage,
			--- Closers
			0 CSalesAmount, 0 COOP, 0 CUPS, 0 CSalesRegular, 0 CSalesExit, 0 CSales, 0 CEfficiency, 0 CClosingFactor, 0 CSaleAverage,
			--- As FTB
			SalesAmount, OPP, UPS, SalesRegular, SalesExit, Sales, Efficiency, ClosingFactor, SaleAverage,
			--- With Junior
			0 WSalesAmount, 0 WOOP, 0 WUPS, 0 WSalesRegular, 0 WSalesExit, 0 WSales, 0 WEfficiency, 0 WClosingFactor, 0 WSaleAverage,
			--- Tot As FTB and With Junior
			0 AWSalesAmount, 0 AWOOP, 0 AWUPS, 0 AWSalesRegular, 0 AWSalesExit, 0 AWSales, 0 AWEfficiency, 0 AWClosingFactor, 0 AWSaleAverage
		FROM @StatsByCloser
		WHERE SalemanType = 'AS'
		GROUP BY SalemanID,SalemanName,SalemanType, SalemanTypeN, TeamN,TeamLeaderN,SalesmanStatus,
			SalesAmount, OPP, UPS, SalesRegular, SalesExit, Sales, Efficiency, ClosingFactor, SaleAverage
		--------- With Junior
		UNION ALL
		SELECT 
			SalemanID, SalemanName,
			'' TeamN, '' TeamLeaderN, '' SalesmanStatus,
			-- TOTALES
			0 TSalesAmount, 0 TOOP, 0 TUPS, 0 TSalesRegular, 0 TSalesExit, 0 TSales, 0 TEfficiency, 0 TClosingFactor, 0 TSaleAverage,
			--- Closers
			0 CSalesAmount, 0 COOP, 0 CUPS, 0 CSalesRegular, 0 CSalesExit, 0 CSales, 0 CEfficiency, 0 CClosingFactor, 0 CSaleAverage,
			--- As FTB
			0 AsSalesAmount, 0 AsOOP, 0 AsUPS, 0 AsSalesRegular, 0 AsSalesExit, 0 AsSales, 0 AsEfficiency, 0 AsClosingFactor, 0 AsSaleAverage,
			--- With Junior
			SalesAmount, OPP, UPS, SalesRegular, SalesExit, Sales, Efficiency, ClosingFactor, SaleAverage,
			--- Tot As FTB and With Junior
			0 AWSalesAmount, 0 AWOOP, 0 AWUPS, 0 AWSalesRegular, 0 AWSalesExit, 0 AWSales, 0 AWEfficiency, 0 AWClosingFactor, 0 AWSaleAverage
		FROM @StatsByCloser
		WHERE SalemanType = 'WITH'
		GROUP BY SalemanID,SalemanName,SalemanType, SalemanTypeN, TeamN,TeamLeaderN,SalesmanStatus,
			SalesAmount, OPP, UPS, SalesRegular, SalesExit, Sales, Efficiency, ClosingFactor, SaleAverage
		--------- Total As Front To back and with Junior
		UNION ALL
		SELECT SalemanID, SalemanName,
			'' TeamN, '' TeamLeaderN, '' SalesmanStatus,
			-- TOTALES
			0 TSalesAmount, 0 TOOP, 0 TUPS, 0 TSalesRegular, 0 TSalesExit, 0 TSales, 0 TEfficiency, 0 TClosingFactor, 0 TSaleAverage,
			--- Closers
			0 CSalesAmount, 0 COOP, 0 CUPS, 0 CSalesRegular, 0 CSalesExit, 0 CSales, 0 CEfficiency, 0 CClosingFactor, 0 CSaleAverage,
			--- As FTB
			0 AsSalesAmount, 0 AsOOP, 0 AsUPS, 0 AsSalesRegular, 0 AsSalesExit, 0 AsSales, 0 AsEfficiency, 0 AsClosingFactor, 0 AsSaleAverage,
			--- With Junior
			0 WSalesAmount, 0 WOOP, 0 WUPS, 0 WSalesRegular, 0 WSalesExit, 0 WSales, 0 WEfficiency, 0 WClosingFactor, 0 WSaleAverage,
			--- Tot As FTB and With Junior
			sum(SalesAmount) AS AWSalesAmount, sum(OPP) as AWOPP, sum(UPS) as AWUPS, 
			sum(SalesRegular) as AWSalesRegular, sum(SalesExit) as AWSalesExit, sum(Sales) as AWSales,		
			dbo.UFN_OR_SecureDivision( sum(SalesAmount),sum(UPS)) AWEfficiency,
			dbo.UFN_OR_SecureDivision(SUM (Sales),sum(UPS)) AWClosingFactor,
			dbo.UFN_OR_SecureDivision(SUM (SalesAmount),SUM (Sales)) AWSaleAverage    
		FROM @StatsByCloser  
		WHERE SalemanType in ('AS','WITH')
		GROUP BY SalemanID,SalemanName,TeamN,TeamLeaderN,SalesmanStatus
	)AS sbc
	GROUP BY SalemanID, SalemanName, TeamN, TeamLeaderN, SalesmanStatus
	ORDER BY TSalesAmount DESC, SalemanID
END
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptStatisticsByExitCloser]    Script Date: 09/22/2016 19:14:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de estadisticas por ExitCloser (Processor Sales)
**
** [aalcocer] 18/Jul/2016 Created
** [ecanul]	  18/08/2016 Modified. Apago los errores ANSI en el select final para que deje hacer los reportes
**				y no marque el error de conversion de tipos de datos 'String or binary data would be truncated.'
**
*/
CREATE procedure [dbo].[USP_IM_RptStatisticsByExitCloser]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@SalesRoom varchar(10),				-- Clave de la sala
	@SalesmanID varchar(10) = 'ALL',	-- Clave de un vendedor	
	@Segments varchar(8000) = 'ALL',	-- Claves de segmentos
	@Programs varchar(8000) = 'ALL',	-- Programs
	@IncludeAllSalesmen bit = 0		-- si se desea que esten todos los vendedores de la sala
as
--SET FMTONLY OFF
set nocount on
--=================== TABLA MANIFEST =============================
--#region ManifestTable
DECLARE @Manifest table (
--campos de huespedes
	id int identity(1,1),
	guID int,	
	guShow bit default 0,
	TeamSelfGen varchar(20),
	guSelfGen bit,
	salesmanDate DateTime,
	sold bit default 0,
	Opp int default 0,
		-- Campos de la tabla de ventas	
	saID int,
	procSales int,	
	saGrossAmount money default 0,
	-- Vendedores
	-- Liner 1
	Liner1 varchar(10),
	Liner1N varchar(40),
	Liner1P varchar(10),
	-- Liner 2
	Liner2 varchar(10),
	Liner2N varchar(40),
	Liner2P varchar(10),
	-- Closer 1
	Closer1 varchar(10),
	Closer1N varchar(40),
	Closer1P varchar(10),
	-- Closer 2
	Closer2 varchar(10),
	Closer2N varchar(40),
	Closer2p varchar(10),
	-- Closer 3
	Closer3 varchar(10),
	Closer3N varchar(40),
	Closer3p varchar(10),
	-- Exit 1
	Exit1 varchar(10),
	Exit1N varchar(40),
	Exit1P varchar(10),
	-- Exit 2
	Exit2 varchar(10),
	Exit2N varchar(40),
	Exit2P varchar(10));	
--#endregion

--#region Guest
--===================================================================
--=======================   MANIFEST    =============================
--===================================================================
INSERT into @Manifest (guID, guShow, salesmanDate, guSelfGen, TeamSelfGen,
saID, procSales, Liner1, Liner1N, Liner1P, Liner2, Liner2N, Liner2P,
Closer1, Closer1N, Closer1P, Closer2, Closer2N, Closer2p, Closer3, Closer3N, Closer3p, Exit1, Exit1N, Exit1P, Exit2, Exit2N, Exit2P)
select DISTINCT
	--Campos de la tabla de huespedes
	G.guID,	
	case WHEN (G.guTour=1 OR g.guWalkOut=1 OR ((G.guCTour=1 OR G.guSaveProgram=1) AND S.saID IS NOT NULL)) THEN 1 ELSE 0 END,
	G.guShowD,
	G.guSelfGen,
	G.guts,	
	S.sagu,
	0 procSales,
	-- PERSONAL DEL SHOW	
	-- Vendedores
	-- Liner 1
	G.guLiner1 as guLiner1,
	GL1.peN as guLiner1N,
	CASE WHEN G.guLiner1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guLiner1) ELSE NULL END [Liner1P],
	-- Liner 2
	G.guLiner2 as guLiner2,
	GL2.peN as guLiner2N,
	CASE WHEN G.guLiner2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guLiner2) ELSE NULL END [Liner2P],
	-- Closer 1
	G.guCloser1 as guCloser1,
	GC1.peN as guCloser1N,
	CASE WHEN G.guCloser1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser1) ELSE NULL END [Closer1P],
	-- Closer 2
	G.guCloser2 as guCloser2,
	GC2.peN as guCloser2N,
	CASE WHEN G.guCloser2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser2) ELSE NULL END [Closer2P],
	-- Closer 3
	G.guCloser3 as guCloser3,
	GC3.peN as guCloser3N,
	CASE WHEN G.guCloser3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser3) ELSE NULL END [Closer3P],
	-- Exit 1
	G.guExit1 as guExit1,
	GE1.peN as guExit1N,
	CASE WHEN G.guExit1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guExit1) ELSE NULL END [Exit1P],
	G.guExit2 as guExit2,
	GE2.peN as guExit2N,
	CASE WHEN G.guExit2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guExit2) ELSE NULL END [Exit2P]
from Guests G
	left join Sales S on sagu = G.guID	
	-- PERSONAL DEL SHOW	
	-- Vendedores
	left join Personnel GL1 on GL1.peID = G.guLiner1
	left join Personnel GL2 on GL2.peID = G.guLiner2
	left join Personnel GC1 on GC1.peID = G.guCloser1
	left join Personnel GC2 on GC2.peID = G.guCloser2
	left join Personnel GC3 on GC3.peID = G.guCloser3
	left join Personnel GE1 on GE1.peID = G.guExit1
	left join Personnel GE2 on GE2.peID = G.guExit2		
	--Segment 
	left join Agencies A on A.agID = G.guag
	left join SegmentsByAgency SA on SA.seID = A.agse
	left join LeadSources LS on LS.lsID = G.guls
	left join SegmentsByLeadSource SL on SL.soID = LS.lsso
	left join SegmentsCategories SCA on SCA.scID = SA.sesc
	left join SegmentsCategories SCL on SCL.scID = SL.sosc
where
	-- Fecha de show
	G.guShowD between @DateFrom and @DateTo
	-- Sala de ventas
	and G.gusr = @SalesRoom	
	-- Vendedor
	and (@SalesmanID = 'ALL'		
		-- Rol de Liner
		or (@SalesmanID = G.guLiner1 or @SalesmanID = G.guLiner2 or @SalesmanID = S.saLiner1 or @SalesmanID = S.saLiner2)
		-- Rol de Closer
		or (@SalesmanID = G.guCloser1 or @SalesmanID = G.guCloser2 or @SalesmanID = G.guCloser3 
			or @SalesmanID = S.saCloser1 or @SalesmanID = S.saCloser2 or @SalesmanID = S.saCloser3)
		-- Rol de Exit
		or (@SalesmanID = G.guExit1 or @SalesmanID = G.guExit2 or @SalesmanID = S.saExit1 or @SalesmanID = S.saExit2))
	-- Segmento
	and (@Segments = 'ALL' or A.agse in (select item from split(@Segments, ',')))
	-- Programa
	and (@Programs = 'ALL' or LS.lspg in (select item from split(@Programs, ',')))
order by G.guID;
--#endregion
--=================== Insert SALES =============================
--#region insertSales

DECLARE @nReg int; -- numero de regitros
SET @nReg=(SELECT COUNT(*) FROM @Manifest);
DECLARE @cont int; -- almacena la cantidad de veces que se recorre el while
SET @cont = 1;
DECLARE @gu int; -- si tiene ventas
WHILE @nReg >= @cont 
BEGIN
	SET @gu = (SELECT guid FROM @Manifest WHERE id = @cont); --obtiene el id del gest	
	DECLARE @sale int;
	SET @sale = (SELECT TOP 1 sagu FROM Sales WHERE sagu = @gu);	
	--#region IF @sale

	IF @sale IS NOT NULL		
	BEGIN		
		UPDATE @Manifest SET sold = 1 WHERE id = @cont;
		SELECT 
		--Datos de la venta
			S.sagu AS GUID,
			S.saID,
			S.saD,	
			S.saGrossAmount,
		--Datos de los vendedores			
			-- Liner 1
			S.saLiner1 as saLiner1,
			SL1.peN as saLiner1N,
			CASE WHEN S.saLiner1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saLiner1) ELSE NULL END [Liner1P],
			-- Liner 2
			S.saLiner2 as saLiner2,
			SL2.peN as saLiner2N,
			CASE WHEN S.saLiner2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saLiner2) ELSE NULL END [Liner2P],
			-- Closer 1
			S.saCloser1 as saCloser1,
			SC1.peN as saCloser1N,
			CASE WHEN S.saCloser1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saCloser1) ELSE NULL END [Closer1P],
			-- Closer 2
			S.saCloser2 as saCloser2,
			SC2.peN as saCloser2N,
			CASE WHEN S.saCloser2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saCloser2) ELSE NULL END [Closer2P],
			-- Closer 3
			S.saCloser3 as saCloser3,
			SC3.peN as saCloser3N,
			CASE WHEN S.saCloser3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saCloser3) ELSE NULL END [Closer3P],
			-- Exit 1
			S.saExit1 as saExit1,
			SE1.peN as saExit1N,
			CASE WHEN S.saExit1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saExit1) ELSE NULL END [Exit1P],
			-- Exit 2
			S.saExit2 as saExit2,
			SE2.peN as saExit2N,
			CASE WHEN S.saExit2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saExit2) ELSE NULL END [Exit2P],			
			1 Sold,
			CASE WHEN ST.ststc = 'DG' THEN 0 ELSE 1 END procSales
		INTO #tbTemp
		FROM Guests G
			LEFT JOIN Sales S on S.sagu = G.guID
			LEFT JOIN SaleTypes ST on ST.stID = S.sast
			-- Vendedores
			left join Personnel SL1 on SL1.peID = S.saLiner1
			left join Personnel SL2 on SL2.peID = S.saLiner2
			left join Personnel SC1 on SC1.peID = S.saCloser1
			left join Personnel SC2 on SC2.peID = S.saCloser2
			left join Personnel SC3 on SC3.peID = S.saCloser3
			left join Personnel SE1 on SE1.peID = S.saExit1
			left join Personnel SE2 on SE2.peID = S.saExit2	
			-- Segment	
			left join Agencies A on A.agID = G.guag		
			left join SegmentsByAgency SA on SA.seID = A.agse
			left join LeadSources LS on LS.lsID = G.guls
			left join SegmentsByLeadSource SL on SL.soID = LS.lsso
			left join SegmentsCategories SCA on SCA.scID = SA.sesc
			left join SegmentsCategories SCL on SCL.scID = SL.sosc
		WHERE
			S.saD BETWEEN @DateFrom and @DateTo AND sagu = @gu
			AND S.sast NOT IN ('BUMP','REGEN')
			AND S.saProcD BETWEEN @DateFrom AND @DateTo
			AND (S.saCancel = 0 OR NOT S.saCancelD BETWEEN @DateFrom AND @DateTo)
			AND S.sasr = @SalesRoom				
		-- Segmento
		and (@Segments = 'ALL' or A.agse in (select item from split(@Segments, ',')))
		-- Programa
		and (@Programs = 'ALL' or LS.lspg in (select item from split(@Programs, ',')))
		INSERT INTO @Manifest (guID,saID,salesmanDate,saGrossAmount, 
		Liner1,Liner1N,Liner1P,Liner2,Liner2N,Liner2P,Closer1,Closer1N,Closer1P,Closer2,Closer2N,Closer2p,Closer3,Closer3N,Closer3p,
		Exit1,Exit1N,Exit1P,Exit2,Exit2N,Exit2P,sold,procSales)
		SELECT * FROM #tbTemp;
		DROP TABLE #tbTemp;
	END		
--#endregion
	SET @cont = @cont +1;
END;
--#endregion

--===================================================================
--===================  DEPOSIT SALES    =============================
--===================================================================
--#region Deposit Sales
INSERT into @Manifest (guID, salesmanDate, guSelfGen, TeamSelfGen, 
procSales, Liner1, Liner1N, Liner1P, Liner2, Liner2N, Liner2P, 
Closer1, Closer1N, Closer1P, Closer2, Closer2N, Closer2p, Closer3, Closer3N, Closer3p, Exit1, Exit1N, Exit1P, Exit2, Exit2N, Exit2P)
select 
	--#region Campos de la tabla de huespedes
	G.guID,	
	G.guShowD,
	G.guSelfGen,
	G.guts,
	0 procSales,	
	--#endregion	
	--#region PERSONAL DEL SHOW	
	-- Vendedores
	-- Liner 1
	G.guLiner1 as guLiner1,
	GL1.peN as guLiner1N,
	CASE WHEN G.guLiner1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guLiner1) ELSE NULL END [Liner1P],
	-- Liner 2
	G.guLiner2 as guLiner2,
	GL2.peN as guLiner2N,
	CASE WHEN G.guLiner2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guLiner2) ELSE NULL END [Liner2P],
	-- Closer 1
	G.guCloser1 as guCloser1,
	GC1.peN as guCloser1N,
	CASE WHEN G.guCloser1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser1) ELSE NULL END [Closer1P],
	-- Closer 2
	G.guCloser2 as guCloser2,
	GC2.peN as guCloser2N,
	CASE WHEN G.guCloser2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser2) ELSE NULL END [Closer2P],
	-- Closer 3
	G.guCloser3 as guCloser3,
	GC3.peN as guCloser3N,
	CASE WHEN G.guCloser3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser3) ELSE NULL END [Closer3P],
	-- Exit 1
	G.guExit1 as guExit1,
	GE1.peN as guExit1N,
	CASE WHEN G.guExit1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guExit1) ELSE NULL END [Exit1P],
	-- Exit 2
	G.guExit2 as guExit2,
	GE2.peN as guExit2N,
	CASE WHEN G.guExit2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guExit2) ELSE NULL END [Exit2P]
	--#endregion
from Guests G	
	left join Sales S on S.sagu = G.guID	
--#region PERSONAL DEL SHOW	
	-- Vendedores
	left join Personnel GL1 on GL1.peID = G.guLiner1
	left join Personnel GL2 on GL2.peID = G.guLiner2
	left join Personnel GC1 on GC1.peID = G.guCloser1
	left join Personnel GC2 on GC2.peID = G.guCloser2
	left join Personnel GC3 on GC3.peID = G.guCloser3
	left join Personnel GE1 on GE1.peID = G.guExit1
	left join Personnel GE2 on GE2.peID = G.guExit2	
	--#endregion	
	-- Segment
	left join Agencies A on A.agID = G.guag
	left join SegmentsByAgency SA on SA.seID = A.agse
	left join LeadSources LS on LS.lsID = G.guls
	left join SegmentsByLeadSource SL on SL.soID = LS.lsso
	left join SegmentsCategories SCA on SCA.scID = SA.sesc
	left join SegmentsCategories SCL on SCL.scID = SL.sosc
where
	G.guDepositSaleD between @DateFrom and @DateTo
	-- Fecha de show diferente de la fecha de venta deposito
	and G.guShowD <> G.guDepositSaleD
	-- Sala de ventas
	and G.gusr = @SalesRoom
	-- Vendedor 
	and (@SalesmanID = 'ALL'		
		-- Rol de Liner
		or (@SalesmanID = G.guLiner1 or @SalesmanID = G.guLiner2 or @SalesmanID = S.saLiner1 or @SalesmanID = S.saLiner2)
		-- Rol de Closer
		or (@SalesmanID = G.guCloser1 or @SalesmanID = G.guCloser2 or @SalesmanID = G.guCloser3
			or @SalesmanID = S.saCloser1 or @SalesmanID = S.saCloser2 or @SalesmanID = S.saCloser3)
		-- Rol de Exit
		or (@SalesmanID = G.guExit1 or @SalesmanID = G.guExit2 or @SalesmanID = S.saExit1 or @SalesmanID = S.saExit2))
	-- Segmento
	and (@Segments = 'ALL' or A.agse in (select item from split(@Segments, ',')))
	-- Programa
	and (@Programs = 'ALL' or LS.lspg in (select item from split(@Programs, ',')))
order by G.guID;
--#endregion
--===================================================================
--======================  OTHER SALES    ============================
--===================================================================
--#region Other Sales
INSERT into @Manifest (guID, guSelfGen, salesmanDate,sold, Opp, saID, procSales, saGrossAmount,
Liner1, Liner1N, Liner1P, Liner2, Liner2N, Liner2P, Closer1, Closer1N, Closer1P, Closer2, Closer2N, 
Closer2p, Closer3, Closer3N, Closer3p, Exit1, Exit1N, Exit1P, Exit2, Exit2N, Exit2P)
SELECT 
--Datos de la venta
	S.sagu AS GUID,
	S.saSelfGen,
	S.saD,
	1 Sold,
	CASE WHEN dbo.UFN_IM_GetSaleType(@DateFrom,@DateTo,S.sast,ST.ststc,G.guDepSale,S.saD,S.saProcD,S.saCancelD,G.gusr,S.sasr,S.saByPhone) IN(6,10) THEN 1 ELSE 0 END [Opp],
	S.saID,
	CASE WHEN dbo.UFN_IM_GetSaleType(@DateFrom,@DateTo,S.sast,ST.ststc,G.guDepSale,S.saD,S.saProcD,S.saCancelD,G.gusr,S.sasr,S.saByPhone) NOT IN(5,14) THEN 1 ELSE 0 END [procSales],
	S.saGrossAmount,
--Datos de los vendedores	
	-- Liner 1
	S.saLiner1 as saLiner1,
	SL1.peN as saLiner1N,
	CASE WHEN S.saLiner1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saLiner1) ELSE NULL END [Liner1P],
	-- Liner 2
	S.saLiner2 as saLiner2,
	SL2.peN as saLiner2N,
	CASE WHEN S.saLiner2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saLiner2) ELSE NULL END [Liner2P],
	-- Closer 1
	S.saCloser1 as saCloser1,
	SC1.peN as saCloser1N,
	CASE WHEN S.saCloser1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saCloser1) ELSE NULL END [Closer1P],
	-- Closer 2
	S.saCloser2 as saCloser2,
	SC2.peN as saCloser2N,
	CASE WHEN S.saCloser2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saCloser2) ELSE NULL END [Closer2P],
	-- Closer 3
	S.saCloser3 as saCloser3,
	SC3.peN as saCloser3N,
	CASE WHEN S.saCloser3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saCloser3) ELSE NULL END [Closer3P],
	-- Exit 1
	S.saExit1 as saExit1,
	SE1.peN as saExit1N,
	CASE WHEN S.saExit1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saExit1) ELSE NULL END [Exit1P],
	-- Exit 2
	S.saExit2 as saExit2,
	SE2.peN as saExit2N,
	CASE WHEN S.saExit2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saExit2) ELSE NULL END [Exit2P]
FROM Guests G --para obtener la fecha del show
	LEFT JOIN Sales S on S.sagu = G.guID
	LEFT JOIN SaleTypes ST on ST.stID = S.sast
	left join MembershipTypes MT on MT.mtID = S.samt
	-- Vendedores
	left join Personnel SL1 on SL1.peID = S.saLiner1
	left join Personnel SL2 on SL2.peID = S.saLiner2
	left join Personnel SC1 on SC1.peID = S.saCloser1
	left join Personnel SC2 on SC2.peID = S.saCloser2
	left join Personnel SC3 on SC3.peID = S.saCloser3
	left join Personnel SE1 on SE1.peID = S.saExit1
	left join Personnel SE2 on SE2.peID = S.saExit2		
	-- Segment	
	left join Agencies A on A.agID = G.guag
	left join SegmentsByAgency SA on SA.seID = A.agse
	left join LeadSources LS on LS.lsID = S.sals
	left join SegmentsByLeadSource SL on Sl.soID = LS.lsso
	left join SegmentsCategories SCA on SCA.scID = SA.sesc
	left join SegmentsCategories SCL on SCL.scID = SL.sosc
WHERE
	((--NO SHOWS
	(G.guShowD IS NULL OR NOT G.guShowD BETWEEN @DateFrom AND @DateTo)
	--FECHA DE --- VENTA	-----------------	Venta Procesable	-----------------------
	AND (S.saD BETWEEN @DateFrom AND @DateTo OR S.saProcD BETWEEN @DateFrom AND @DateTo --OR s.saCancelD BETWEEN @DateFrom AND @DateTo
	))
	-- Incluye Bumps, Regen y ventas de otras salas
	OR ((S.sast IN ('BUMP','REGEN') OR G.gusr <> S.sasr) AND S.saD BETWEEN @DateFrom AND @DateTo))
	AND S.saProcD BETWEEN @DateFrom AND @DateTo
	AND (S.saCancel = 0 OR NOT S.saCancelD BETWEEN @DateFrom AND @DateTo)
	AND S.sasr = @SalesRoom
	-- Vendedor
	and (@SalesmanID = 'ALL'
		-- Rol de Liner
		or (@SalesmanID = S.saLiner1 or @SalesmanID = S.saLiner2)
		-- Rol de Closer
		or (@SalesmanID = S.saCloser1 or @SalesmanID = S.saCloser2 or @SalesmanID = S.saCloser3)
		-- Rol de Exit
		or (@SalesmanID = S.saExit1 or @SalesmanID = S.saExit2))
	-- Segmento
	and (@Segments = 'ALL' or A.agse in (select item from split(@Segments, ',')))
	-- Programa
	and (@Programs = 'ALL' or LS.lspg in (select item from split(@Programs, ',')))
ORDER BY S.sagu;
--#endregion


--=================== TABLA Salesman =============================
--#region Salesman Table
DECLARE @Salesman table (
	id int,	
	SalemanID varchar(10),
	SalemanName varchar(40),
	UPS money,
	Sales money,
	Amount money,
	Role varchar(50),
	SalemanType varchar(50)	
	);
--#endregion
	
	--=================== Insert Salesman =============================
	--#region Insert Salesman

	--#region Closer1
	INSERT into @Salesman (id, SalemanID,SalemanName, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Closer1 ,Closer1N, 'Closer',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	'Exit Closer',
	procSales * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default)
	from @Manifest m WHERE Closer1 is NOT NULL AND Closer1P='EXIT' 
	AND Closer1 NOT IN (IsNull(Liner1, ''), IsNull(Liner2, ''))
	--#endregion
	
	--#region Closer2
	INSERT into @Salesman (id, SalemanID,SalemanName, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Closer2 ,Closer2N, 'Closer',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	'Exit Closer',
	procSales * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default)
	from @Manifest m WHERE Closer2 is NOT NULL AND Closer2P='EXIT' 
	AND Closer2 NOT IN (IsNull(Liner1, ''), IsNull(Liner2, ''), IsNull(Closer1, ''))
	--#endregion
	
	--#region Closer3
	INSERT into @Salesman (id, SalemanID,SalemanName, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Closer3 ,Closer3N, 'Closer',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	'Exit Closer',
	procSales * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default)
	from @Manifest m WHERE Closer3 is NOT NULL AND Closer3P='EXIT' 
	AND Closer3 NOT IN (IsNull(Liner1, ''), IsNull(Liner2, ''), IsNull(Closer1, ''), IsNull(Closer2,''))
	--#endregion
	
	--#region Exit1
	INSERT into @Salesman (id, SalemanID,SalemanName, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Exit1 ,Exit1N,  'Exit',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default),
	CASE WHEN Exit1P='EXIT' THEN 'Exit Closer' ELSE 'Front To Back As Exit Closer' END,
	procSales * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default)
	from @Manifest m WHERE Exit1 is NOT NULL 
	AND Exit1 NOT IN (IsNull(Liner1, ''), IsNull(Liner2, ''), IsNull(Closer1, ''), IsNull(Closer2,''),IsNull(Closer3,''))
	--#endregion
	
	--#region Exit2
	INSERT into @Salesman (id, SalemanID,SalemanName, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Exit2 ,Exit2N, 'Exit',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default),
	CASE WHEN Exit2P='EXIT' THEN 'Exit Closer' ELSE 'Front To Back As Exit Closer' END,
	procSales * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default)
	from @Manifest m WHERE Exit2 is NOT NULL
	AND Exit2 NOT IN (IsNull(Liner1, ''), IsNull(Liner2, ''), IsNull(Closer1, ''), IsNull(Closer2,''),IsNull(Closer3,''),IsNull(Exit1,''))
--#endregion
	
	--=================== TABLA StatsByExitCloser =============================
--#region StatsByExitCloser Table
DECLARE @StatsByExitCloser table (
	SalemanID varchar(10),
	SalemanName varchar(40),
	SalemanType varchar(50),
	TeamN varchar(30),
	TeamLeaderN varchar(40),
	SalesmanStatus VARCHAR(10) DEFAULT 'ACTIVE',
	SalesAmount money,
	OPP int,
	UPS money,	
	SalesAmountRange VARCHAR(30),
	Sales money,
	SalesTotal money,	
	Efficiency money,
	ClosingFactor money,
	SaleAverage money
	);
--#endregion
--SET ansi_warnings OFF
	INSERT INTO @StatsByExitCloser
	SELECT SalemanID, SalemanName, SalemanType, 
	TeamN, TeamLeaderN, SalesmanStatus,
	sum(Amount) AS SalesAmount, sum(Opp) AS Opp, sum(UPS) AS UPS,
	SalesAmountRange, Sum(SalesRange),
	SUM (Sales) as SalesTotal,
	dbo.UFN_OR_SecureDivision( sum(Amount),sum(UPS)) Efficiency,
	dbo.UFN_OR_SecureDivision(SUM (Sales),sum(UPS)) ClosingFactor,
	dbo.UFN_OR_SecureDivision(SUM (Amount),SUM (Sales)) SaleAverage
	FROM(
		SELECT DISTINCT m.guID, m.saID, s.SalemanID,s.SalemanName,s.SalemanType,
		s.UPS, s.Amount, m.Opp,	s.Sales , snN AS SalesAmountRange, 
		CASE WHEN m.saGrossAmount BETWEEN snFrom AND snTo THEN S.Sales ELSE 0 END SalesRange,
		CASE WHEN t.SalesRoom = @SalesRoom AND ISNULL(CONVERT(varchar(1),tc.tsN),'') <> ISNULL(CONVERT(varchar(1),t.TeamN),'') THEN 'INACTIVE' ELSE 'ACTIVE'  END AS SalesmanStatus,
		CASE WHEN t.SalesRoom <> @SalesRoom THEN 'TEAMS OF OTHER SALES ROOMS' ELSE 
			CASE WHEN t.TeamN IS NULL THEN --si no tiene equipo
				CASE WHEN dbo.UFN_IM_IsSelfGen(s.SalemanID,s.Role,m.guSelfGen)=1 THEN -- si es un Self Gen obtenemos el equipo que asigno la Hostess
				ISNULL(CONVERT(varchar(40),m.TeamSelfGen),'NO TEAM')
				ELSE 'NO TEAM' END
			ELSE t.TeamN END
		END AS TeamN,
		CASE WHEN t.SalesRoom <> @SalesRoom THEN '' ELSE ISNULL(CONVERT(varchar(40),t.TeamLeaderN),'') END AS TeamLeaderN
		FROM @Salesman s	
		INNER JOIN @Manifest m ON m.id = s.id
		INNER JOIN Personnel p on p.peID= s.SalemanID
		left join TeamsSalesmen tc on peTeam = tsID	
		cross apply  dbo.UFN_IM_GetPersonnelTeamSalesmenByDate(s.SalemanID,m.salesmanDate) t
		CROSS JOIN SalesAmountRanges r
		WHERE SalemanType IS NOT NULL and snA = 1
		) AS x
		GROUP by SalemanID,SalemanName,SalemanType, TeamN, TeamLeaderN, x.SalesmanStatus, SalesAmountRange


IF @IncludeAllSalesmen=1 AND  (SELECT COUNT(*) from @StatsByExitCloser)>0
BEGIN			
	INSERT INTO @StatsByExitCloser(SalemanID, SalemanName, SalemanType, TeamN, TeamLeaderN)
	select P.peID, P.peN as SalesmanN, (SELECT top 1 SalemanType from @StatsByExitCloser) , IsNull(tsN, 'NO TEAM'), IsNull(L.peN, '')
	from Personnel P 
	left join TeamsSalesmen on P.peTeamType = 'SA' and P.pePlaceID = tssr and P.peTeam = tsID 
	left join Personnel L on tsLeader = L.peID
	where
		-- Tipo de equipo
		P.peTeamType = 'SA'
		-- Clave del lugar
		and P.pePlaceID = @SalesRoom
		-- Personal activo
		and P.peA = 1
		-- Filtro
		and P.pepo = 'EXIT'
		--si no se encuentra el vendedor
		AND NOT EXISTS (SELECT * from @StatsByExitCloser  WHERE SalemanID = p.peID)
END	
--SET ansi_warnings ON			
--===================================================================
--===================  SELECT  ===========================
--===================================================================
SELECT * FROM @StatsByExitCloser 
ORDER BY SalemanType, SalesAmount DESC, SalemanID  
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptStatisticsByFTB]    Script Date: 09/22/2016 19:14:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de estadisticas por FTB (Processor Sales)
**
** [michan] 21/Jul/2016 Created
**
*/
create procedure [dbo].[USP_IM_RptStatisticsByFTB]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@SalesRoom varchar(10),				-- Clave de la sala
	@SalesmanID varchar(10) = 'ALL',	-- Clave de un vendedor	
	@Segments varchar(8000) = 'ALL',	-- Claves de segmentos
	@Programs varchar(8000) = 'ALL',	-- Programs
	@GroupedByTeam bit = 0,			-- si se desea que esten agrupdo´por su equipo
	@IncludeAllSalesmen bit = 0			-- si se desea que esten todos los vendedores de la sala
as
--SET FMTONLY OFF
set nocount on
--=================== TABLA MANIFEST =============================
--#region ManifestTable
DECLARE @Manifest table (
--campos de huespedes
	id int identity(1,1),
	guID int,	
	guShow bit default 0,
	own bit,	
	TeamSelfGen varchar(20),
	guSelfGen bit,
	guOverflow bit default 0,
	salesmanDate DateTime,
	sold bit default 0,
	Opp int default 0,	
	MembershipGroup varchar(10),
	
		-- Campos de la tabla de ventas	
	saID int,
	procSales int,	
	saGrossAmount money default 0,
	-- Vendedores
	-- Liner 1
	Liner1 varchar(10),
	Liner1N varchar(40),
	Liner1P varchar(10),
	-- Liner 2
	Liner2 varchar(10),
	Liner2N varchar(40),
	Liner2P varchar(10),
	-- Closer 1
	Closer1 varchar(10),
	Closer1N varchar(40),
	Closer1P varchar(10),
	-- Closer 2
	Closer2 varchar(10),
	Closer2N varchar(40),
	Closer2p varchar(10),
	-- Closer 3
	Closer3 varchar(10),
	Closer3N varchar(40),
	Closer3p varchar(10),
	-- Exit 1
	Exit1 varchar(10),
	Exit1N varchar(40),
	Exit1P varchar(10),
	-- Exit 2
	Exit2 varchar(10),
	Exit2N varchar(40),
	Exit2P varchar(10));	
--#endregion

--#region Guest
--===================================================================
--=======================   MANIFEST    =============================
--===================================================================
INSERT into @Manifest (guID, guShow, own, salesmanDate, guSelfGen, TeamSelfGen, guOverflow,
saID, procSales, MembershipGroup,  Liner1, Liner1N, Liner1P, Liner2, Liner2N, Liner2P,
Closer1, Closer1N, Closer1P, Closer2, Closer2N, Closer2p, Closer3, Closer3N, Closer3p, Exit1, Exit1N, Exit1P, Exit2, Exit2N, Exit2P)
select DISTINCT
	--Campos de la tabla de huespedes
	G.guID,	
	case WHEN (G.guTour=1 OR g.guWalkOut=1 OR ((G.guCTour=1 OR G.guSaveProgram=1) AND S.saID IS NOT NULL)) THEN 1 ELSE 0 END,
	dbo.UFN_IM_StringComparer(G.guLiner1,G.guLiner2,G.guCloser1,G.guCloser2,G.guCloser3,G.guExit1,G.guExit2),
	G.guShowD,
	G.guSelfGen,	
	G.guts,	
	G.guOverflow,
	S.sagu,
	0 procSales,
	MT.mtGroup,
	
	-- PERSONAL DEL SHOW	
	-- Vendedores
	-- Liner 1
	G.guLiner1 as guLiner1,
	GL1.peN as guLiner1N,
	CASE WHEN G.guLiner1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guLiner1) ELSE NULL END [Liner1P],
	-- Liner 2
	G.guLiner2 as guLiner2,
	GL2.peN as guLiner2N,
	CASE WHEN G.guLiner2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guLiner2) ELSE NULL END [Liner2P],
	-- Closer 1
	G.guCloser1 as guCloser1,
	GC1.peN as guCloser1N,
	CASE WHEN G.guCloser1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser1) ELSE NULL END [Closer1P],
	-- Closer 2
	G.guCloser2 as guCloser2,
	GC2.peN as guCloser2N,
	CASE WHEN G.guCloser2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser2) ELSE NULL END [Closer2P],
	-- Closer 3
	G.guCloser3 as guCloser3,
	GC3.peN as guCloser3N,
	CASE WHEN G.guCloser3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser3) ELSE NULL END [Closer3P],
	-- Exit 1
	G.guExit1 as guExit1,
	GE1.peN as guExit1N,
	CASE WHEN G.guExit1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guExit1) ELSE NULL END [Exit1P],
	G.guExit2 as guExit2,
	GE2.peN as guExit2N,
	CASE WHEN G.guExit2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guExit2) ELSE NULL END [Exit2P]
from Guests G
	left join Sales S on sagu = G.guID	
	left join MembershipTypes MT on MT.mtID = S.samt
	-- PERSONAL DEL SHOW	
	-- Vendedores
	left join Personnel GL1 on GL1.peID = G.guLiner1
	left join Personnel GL2 on GL2.peID = G.guLiner2
	left join Personnel GC1 on GC1.peID = G.guCloser1
	left join Personnel GC2 on GC2.peID = G.guCloser2
	left join Personnel GC3 on GC3.peID = G.guCloser3
	left join Personnel GE1 on GE1.peID = G.guExit1
	left join Personnel GE2 on GE2.peID = G.guExit2		
	--Segment 
	left join Agencies A on A.agID = G.guag
	left join SegmentsByAgency SA on SA.seID = A.agse
	left join LeadSources LS on LS.lsID = G.guls
	
	left join SegmentsByLeadSource SL on SL.soID = LS.lsso
	left join SegmentsCategories SCA on SCA.scID = SA.sesc
	left join SegmentsCategories SCL on SCL.scID = SL.sosc
where
	-- Fecha de show
	G.guShowD between @DateFrom and @DateTo
	-- Sala de ventas
	and G.gusr = @SalesRoom	
	-- Vendedor
	and (@SalesmanID = 'ALL'		
		-- Rol de Liner
		or (@SalesmanID = G.guLiner1 or @SalesmanID = G.guLiner2 or @SalesmanID = S.saLiner1 or @SalesmanID = S.saLiner2)
		-- Rol de Closer
		or (@SalesmanID = G.guCloser1 or @SalesmanID = G.guCloser2 or @SalesmanID = G.guCloser3 
			or @SalesmanID = S.saCloser1 or @SalesmanID = S.saCloser2 or @SalesmanID = S.saCloser3)
		-- Rol de Exit
		or (@SalesmanID = G.guExit1 or @SalesmanID = G.guExit2 or @SalesmanID = S.saExit1 or @SalesmanID = S.saExit2))
	-- Segmento
	and (@Segments = 'ALL' or A.agse in (select item from split(@Segments, ',')))
	-- Programa
	and (@Programs = 'ALL' or LS.lspg in (select item from split(@Programs, ',')))
order by G.guID;
--#endregion
--=================== Insert SALES =============================
--#region insertSales

DECLARE @nReg int; -- numero de regitros
SET @nReg=(SELECT COUNT(*) FROM @Manifest);
DECLARE @cont int; -- almacena la cantidad de veces que se recorre el while
SET @cont = 1;
DECLARE @gu int; -- si tiene ventas
WHILE @nReg >= @cont 
BEGIN
	SET @gu = (SELECT guid FROM @Manifest WHERE id = @cont); --obtiene el id del gest	
	DECLARE @sale int;
	SET @sale = (SELECT TOP 1 sagu FROM Sales WHERE sagu = @gu);	
	--#region IF @sale

	IF @sale IS NOT NULL		
	BEGIN		
		UPDATE @Manifest SET sold = 1 WHERE id = @cont;
		SELECT 
		--Datos de la venta
			S.sagu AS GUID,
			dbo.UFN_IM_StringComparer(S.saLiner1,S.saLiner2,S.saCloser1,S.saCloser2,S.saCloser3,S.saExit1,S.saExit2) own,
			S.saID,
			S.saD,	
			S.saGrossAmount,						
			MT.mtGroup,
			
		--Datos de los vendedores			
			-- Liner 1
			S.saLiner1 as saLiner1,
			SL1.peN as saLiner1N,
			CASE WHEN S.saLiner1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saLiner1) ELSE NULL END [Liner1P],
			-- Liner 2
			S.saLiner2 as saLiner2,
			SL2.peN as saLiner2N,
			CASE WHEN S.saLiner2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saLiner2) ELSE NULL END [Liner2P],
			-- Closer 1
			S.saCloser1 as saCloser1,
			SC1.peN as saCloser1N,
			CASE WHEN S.saCloser1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saCloser1) ELSE NULL END [Closer1P],
			-- Closer 2
			S.saCloser2 as saCloser2,
			SC2.peN as saCloser2N,
			CASE WHEN S.saCloser2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saCloser2) ELSE NULL END [Closer2P],
			-- Closer 3
			S.saCloser3 as saCloser3,
			SC3.peN as saCloser3N,
			CASE WHEN S.saCloser3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saCloser3) ELSE NULL END [Closer3P],
			-- Exit 1
			S.saExit1 as saExit1,
			SE1.peN as saExit1N,
			CASE WHEN S.saExit1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saExit1) ELSE NULL END [Exit1P],
			-- Exit 2
			S.saExit2 as saExit2,
			SE2.peN as saExit2N,
			CASE WHEN S.saExit2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saExit2) ELSE NULL END [Exit2P],			
			1 Sold,
			CASE WHEN ST.ststc = 'DG' THEN 0 ELSE 1 END procSales
		INTO #tbTemp
		FROM Guests G
			LEFT JOIN Sales S on S.sagu = G.guID
			LEFT JOIN SaleTypes ST on ST.stID = S.sast
			left join MembershipTypes MT on MT.mtID = S.samt
			-- Vendedores
			left join Personnel SL1 on SL1.peID = S.saLiner1
			left join Personnel SL2 on SL2.peID = S.saLiner2
			left join Personnel SC1 on SC1.peID = S.saCloser1
			left join Personnel SC2 on SC2.peID = S.saCloser2
			left join Personnel SC3 on SC3.peID = S.saCloser3
			left join Personnel SE1 on SE1.peID = S.saExit1
			left join Personnel SE2 on SE2.peID = S.saExit2	
			-- Segment	
			left join Agencies A on A.agID = G.guag		
			left join SegmentsByAgency SA on SA.seID = A.agse
			left join LeadSources LS on LS.lsID = G.guls
			
			left join SegmentsByLeadSource SL on SL.soID = LS.lsso
			left join SegmentsCategories SCA on SCA.scID = SA.sesc
			left join SegmentsCategories SCL on SCL.scID = SL.sosc
		WHERE
			S.saD BETWEEN @DateFrom and @DateTo AND sagu = @gu
			AND S.sast NOT IN ('BUMP','REGEN')
			AND S.saProcD BETWEEN @DateFrom AND @DateTo
			AND (S.saCancel = 0 OR NOT S.saCancelD BETWEEN @DateFrom AND @DateTo)
			AND S.sasr = @SalesRoom		
			-- Vendedor 
		and (@SalesmanID = 'ALL'		
			-- Rol de Liner
			or (@SalesmanID = S.saLiner1 or @SalesmanID = S.saLiner2)
			-- Rol de Closer
			or (@SalesmanID = S.saCloser1 or @SalesmanID = S.saCloser2 or @SalesmanID = S.saCloser3)
			-- Rol de Exit
			or (@SalesmanID = S.saExit1 or @SalesmanID = S.saExit2))
		-- Segmento
		and (@Segments = 'ALL' or A.agse in (select item from split(@Segments, ',')))
		-- Programa
		and (@Programs = 'ALL' or LS.lspg in (select item from split(@Programs, ',')))
		INSERT INTO @Manifest (guID, own, saID,salesmanDate,saGrossAmount, MembershipGroup, 
		Liner1,Liner1N,Liner1P,Liner2,Liner2N,Liner2P,Closer1,Closer1N,Closer1P,Closer2,Closer2N,Closer2p,Closer3,Closer3N,Closer3p,
		Exit1,Exit1N,Exit1P,Exit2,Exit2N,Exit2P,sold,procSales)
		SELECT * FROM #tbTemp;
		DROP TABLE #tbTemp;
	END		
--#endregion
	SET @cont = @cont +1;
END;
--#endregion

--===================================================================
--===================  DEPOSIT SALES    =============================
--===================================================================
--#region Deposit Sales
INSERT into @Manifest (guID, own, salesmanDate, guSelfGen, TeamSelfGen, guOverflow, 
procSales, MembershipGroup, Liner1, Liner1N, Liner1P, Liner2, Liner2N, Liner2P, 
Closer1, Closer1N, Closer1P, Closer2, Closer2N, Closer2p, Closer3, Closer3N, Closer3p, Exit1, Exit1N, Exit1P, Exit2, Exit2N, Exit2P)
select 
	--#region Campos de la tabla de huespedes
	G.guID,	
	dbo.UFN_IM_StringComparer(G.guLiner1,G.guLiner2,G.guCloser1,G.guCloser2,G.guCloser3,G.guExit1,G.guExit2),	
	G.guShowD,
	G.guSelfGen,	
	G.guts,
	G.guOverflow,	
	0 procSales,
	MT.mtGroup,	
	
	--#endregion	
	--#region PERSONAL DEL SHOW	
	-- Vendedores
	-- Liner 1
	G.guLiner1 as guLiner1,
	GL1.peN as guLiner1N,
	CASE WHEN G.guLiner1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guLiner1) ELSE NULL END [Liner1P],
	-- Liner 2
	G.guLiner2 as guLiner2,
	GL2.peN as guLiner2N,
	CASE WHEN G.guLiner2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guLiner2) ELSE NULL END [Liner2P],
	-- Closer 1
	G.guCloser1 as guCloser1,
	GC1.peN as guCloser1N,
	CASE WHEN G.guCloser1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser1) ELSE NULL END [Closer1P],
	-- Closer 2
	G.guCloser2 as guCloser2,
	GC2.peN as guCloser2N,
	CASE WHEN G.guCloser2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser2) ELSE NULL END [Closer2P],
	-- Closer 3
	G.guCloser3 as guCloser3,
	GC3.peN as guCloser3N,
	CASE WHEN G.guCloser3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser3) ELSE NULL END [Closer3P],
	-- Exit 1
	G.guExit1 as guExit1,
	GE1.peN as guExit1N,
	CASE WHEN G.guExit1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guExit1) ELSE NULL END [Exit1P],
	-- Exit 2
	G.guExit2 as guExit2,
	GE2.peN as guExit2N,
	CASE WHEN G.guExit2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guExit2) ELSE NULL END [Exit2P]
	--#endregion
from Guests G	
	left join Sales S on S.sagu = G.guID
	left join MembershipTypes MT on MT.mtID = S.samt	
--#region PERSONAL DEL SHOW	
	-- Vendedores
	left join Personnel GL1 on GL1.peID = G.guLiner1
	left join Personnel GL2 on GL2.peID = G.guLiner2
	left join Personnel GC1 on GC1.peID = G.guCloser1
	left join Personnel GC2 on GC2.peID = G.guCloser2
	left join Personnel GC3 on GC3.peID = G.guCloser3
	left join Personnel GE1 on GE1.peID = G.guExit1
	left join Personnel GE2 on GE2.peID = G.guExit2	
	--#endregion	
	-- Segment
	left join Agencies A on A.agID = G.guag
	left join SegmentsByAgency SA on SA.seID = A.agse
	left join LeadSources LS on LS.lsID = G.guls
	
	left join SegmentsByLeadSource SL on SL.soID = LS.lsso
	left join SegmentsCategories SCA on SCA.scID = SA.sesc
	left join SegmentsCategories SCL on SCL.scID = SL.sosc
where
	G.guDepositSaleD between @DateFrom and @DateTo
	-- Fecha de show diferente de la fecha de venta deposito
	and G.guShowD <> G.guDepositSaleD
	-- Sala de ventas
	and G.gusr = @SalesRoom
	-- Vendedor 
	and (@SalesmanID = 'ALL'		
		-- Rol de Liner
		or (@SalesmanID = G.guLiner1 or @SalesmanID = G.guLiner2 or @SalesmanID = S.saLiner1 or @SalesmanID = S.saLiner2)
		-- Rol de Closer
		or (@SalesmanID = G.guCloser1 or @SalesmanID = G.guCloser2 or @SalesmanID = G.guCloser3
			or @SalesmanID = S.saCloser1 or @SalesmanID = S.saCloser2 or @SalesmanID = S.saCloser3)
		-- Rol de Exit
		or (@SalesmanID = G.guExit1 or @SalesmanID = G.guExit2 or @SalesmanID = S.saExit1 or @SalesmanID = S.saExit2))
	-- Segmento
	and (@Segments = 'ALL' or A.agse in (select item from split(@Segments, ',')))
	-- Programa
	and (@Programs = 'ALL' or LS.lspg in (select item from split(@Programs, ',')))
order by G.guID;
--#endregion
--===================================================================
--======================  OTHER SALES    ============================
--===================================================================
--#region Other Sales
INSERT into @Manifest (guID, own, guSelfGen, salesmanDate,sold, Opp, saID, procSales, saGrossAmount,  MembershipGroup, 
Liner1, Liner1N, Liner1P, Liner2, Liner2N, Liner2P, Closer1, Closer1N, Closer1P, Closer2, Closer2N, 
Closer2p, Closer3, Closer3N, Closer3p, Exit1, Exit1N, Exit1P, Exit2, Exit2N, Exit2P)
SELECT 
--Datos de la venta
	S.sagu AS GUID,
	dbo.UFN_IM_StringComparer(S.saLiner1,S.saLiner2,S.saCloser1,S.saCloser2,S.saCloser3,S.saExit1,S.saExit2),
	S.saSelfGen,
	S.saD,
	1 Sold,
	CASE WHEN dbo.UFN_IM_GetSaleType(@DateFrom,@DateTo,S.sast,ST.ststc,G.guDepSale,S.saD,S.saProcD,S.saCancelD,G.gusr,S.sasr,S.saByPhone) IN(6,10) THEN 1 ELSE 0 END [Opp],
	S.saID,
	CASE WHEN dbo.UFN_IM_GetSaleType(@DateFrom,@DateTo,S.sast,ST.ststc,G.guDepSale,S.saD,S.saProcD,S.saCancelD,G.gusr,S.sasr,S.saByPhone) NOT IN(5,14) THEN 1 ELSE 0 END [procSales],
	S.saGrossAmount,
	MT.mtGroup,
	
--Datos de los vendedores	
	-- Liner 1
	S.saLiner1 as saLiner1,
	SL1.peN as saLiner1N,
	CASE WHEN S.saLiner1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saLiner1) ELSE NULL END [Liner1P],
	-- Liner 2
	S.saLiner2 as saLiner2,
	SL2.peN as saLiner2N,
	CASE WHEN S.saLiner2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saLiner2) ELSE NULL END [Liner2P],
	-- Closer 1
	S.saCloser1 as saCloser1,
	SC1.peN as saCloser1N,
	CASE WHEN S.saCloser1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saCloser1) ELSE NULL END [Closer1P],
	-- Closer 2
	S.saCloser2 as saCloser2,
	SC2.peN as saCloser2N,
	CASE WHEN S.saCloser2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saCloser2) ELSE NULL END [Closer2P],
	-- Closer 3
	S.saCloser3 as saCloser3,
	SC3.peN as saCloser3N,
	CASE WHEN S.saCloser3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saCloser3) ELSE NULL END [Closer3P],
	-- Exit 1
	S.saExit1 as saExit1,
	SE1.peN as saExit1N,
	CASE WHEN S.saExit1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saExit1) ELSE NULL END [Exit1P],
	-- Exit 2
	S.saExit2 as saExit2,
	SE2.peN as saExit2N,
	CASE WHEN S.saExit2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saExit2) ELSE NULL END [Exit2P]
FROM Guests G --para obtener la fecha del show
	LEFT JOIN Sales S on S.sagu = G.guID
	LEFT JOIN SaleTypes ST on ST.stID = S.sast
	left join MembershipTypes MT on MT.mtID = S.samt
	-- Vendedores
	left join Personnel SL1 on SL1.peID = S.saLiner1
	left join Personnel SL2 on SL2.peID = S.saLiner2
	left join Personnel SC1 on SC1.peID = S.saCloser1
	left join Personnel SC2 on SC2.peID = S.saCloser2
	left join Personnel SC3 on SC3.peID = S.saCloser3
	left join Personnel SE1 on SE1.peID = S.saExit1
	left join Personnel SE2 on SE2.peID = S.saExit2		
	-- Segment	
	left join Agencies A on A.agID = G.guag
	left join SegmentsByAgency SA on SA.seID = A.agse
	left join LeadSources LS on LS.lsID = S.sals
	
	left join SegmentsByLeadSource SL on Sl.soID = LS.lsso
	left join SegmentsCategories SCA on SCA.scID = SA.sesc
	left join SegmentsCategories SCL on SCL.scID = SL.sosc
WHERE
	((--NO SHOWS
	(G.guShowD IS NULL OR NOT G.guShowD BETWEEN @DateFrom AND @DateTo)
	--FECHA DE --- VENTA	-----------------	Venta Procesable	-----------------------
	AND (S.saD BETWEEN @DateFrom AND @DateTo OR S.saProcD BETWEEN @DateFrom AND @DateTo --OR s.saCancelD BETWEEN @DateFrom AND @DateTo
	))
	-- Incluye Bumps, Regen y ventas de otras salas
	OR ((S.sast IN ('BUMP','REGEN') OR G.gusr <> S.sasr) AND S.saD BETWEEN @DateFrom AND @DateTo))
	AND S.saProcD BETWEEN @DateFrom AND @DateTo
	AND (S.saCancel = 0 OR NOT S.saCancelD BETWEEN @DateFrom AND @DateTo)
	AND S.sasr = @SalesRoom
	-- Vendedor
	and (@SalesmanID = 'ALL'
		-- Rol de Liner
		or (@SalesmanID = S.saLiner1 or @SalesmanID = S.saLiner2)
		-- Rol de Closer
		or (@SalesmanID = S.saCloser1 or @SalesmanID = S.saCloser2 or @SalesmanID = S.saCloser3)
		-- Rol de Exit
		or (@SalesmanID = S.saExit1 or @SalesmanID = S.saExit2))
	-- Segmento
	and (@Segments = 'ALL' or A.agse in (select item from split(@Segments, ',')))
	-- Programa
	and (@Programs = 'ALL' or LS.lspg in (select item from split(@Programs, ',')))
ORDER BY S.sagu;
--#endregion


--=================== TABLA Salesman =============================
--#region Salesman Table
DECLARE @Salesman table (
	id int,	
	SalemanID varchar(10),
	SalemanName varchar(40),
	UPS money,
	Sales money,
	Amount money,
	PostID varchar(10),
	Role varchar(50),
	SalemanType varchar(50)	
	);
--#endregion
	
	--=================== Insert Salesman =============================
	--#region Insert Salesman

	--#region Liner1
	INSERT into @Salesman (id, SalemanID,SalemanName, PostID, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Liner1 ,Liner1N, Liner1P, 'Liner',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Liner1,Liner2,default,default,default),
	dbo.UFN_IM_GetSalesmanTypesFTB(Liner1,'Liner',sold,Liner2, 'Liner', Liner2P, Closer1,'Closer',Closer1P, Closer2, 'Closer',Closer2p, Closer3, 'Closer',Closer3p, Exit1, 'Exit',Exit1P, Exit2, 'Exit',Exit2P),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Liner1,Liner2,default,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Liner1,Liner2,default,default,default)
	from @Manifest WHERE Liner1 is NOT NULL
	--#endregion
	
	--#region Liner2
	INSERT into @Salesman (id, SalemanID,SalemanName, PostID, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Liner2 ,Liner2N, Liner2P, 'Liner',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Liner1,Liner2,default,default,default),
	dbo.UFN_IM_GetSalesmanTypesFTB(Liner2,'Liner',sold,Liner1, 'Liner', Liner1P, Closer1,'Closer',Closer1P, Closer2, 'Closer',Closer2p, Closer3, 'Closer',Closer3p, Exit1, 'Exit',Exit1P, Exit2, 'Exit',Exit2P),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Liner1,Liner2,default,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Liner1,Liner2,default,default,default)
	from @Manifest WHERE Liner2 is NOT NULL 
	AND Liner2 NOT IN (IsNull(Liner1, ''))
--#endregion

	--#region Closer1
	INSERT into @Salesman (id, SalemanID,SalemanName, PostID, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Closer1 ,Closer1N, Closer1P, 'Closer',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	dbo.UFN_IM_GetSalesmanTypesFTB(Closer1,'Closer',sold,Liner1, 'Liner', Liner1P, Liner2,'Liner',Liner2P, Closer2, 'Closer',Closer2p, Closer3, 'Closer',Closer3p, Exit1, 'Exit',Exit1P, Exit2, 'Exit',Exit2P),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default)
	from @Manifest WHERE Closer1 is NOT NULL AND Closer1P IN ('LINER','FTB') 
	AND Closer1 NOT IN (IsNull(Liner1, ''), IsNull(Liner2, ''))
	--#endregion
	
	--#region Closer2
	INSERT into @Salesman (id, SalemanID,SalemanName, PostID, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Closer2 ,Closer2N, Closer2P, 'Closer',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	dbo.UFN_IM_GetSalesmanTypesFTB(Closer2,'Closer',sold,Liner1, 'Liner', Liner1P, Liner2,'Liner',Liner2P, Closer2, 'Closer',Closer1p, Closer3, 'Closer',Closer3p, Exit1, 'Exit',Exit1P, Exit2, 'Exit',Exit2P),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default)
	from @Manifest WHERE Closer2 is NOT NULL AND Closer2P IN ('LINER','FTB') 
	AND Closer2 NOT IN (IsNull(Liner1, ''), IsNull(Liner2, ''), IsNull(Closer1, ''))
	--#endregion
	
	--#region Closer3
	INSERT into @Salesman (id, SalemanID,SalemanName, PostID, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Closer3 ,Closer3N, Closer3P, 'Closer',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	dbo.UFN_IM_GetSalesmanTypesFTB(Closer3,'Closer',sold,Liner1, 'Liner', Liner1P, Liner2,'Liner',Liner2P, Closer1, 'Closer',Closer1p, Closer2, 'Closer',Closer2p, Exit1, 'Exit',Exit1P, Exit2, 'Exit',Exit2P),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default)
	from @Manifest WHERE Closer3 is NOT NULL AND Closer3P IN ('LINER','FTB') 
	AND Closer3 NOT IN (IsNull(Liner1, ''), IsNull(Liner2, ''), IsNull(Closer1, ''), IsNull(Closer2,''))
	--#endregion
	
	--#region Exit1
	INSERT into @Salesman (id, SalemanID,SalemanName, PostID, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Exit1 ,Exit1N, Exit1P, 'Exit',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default),
	dbo.UFN_IM_GetSalesmanTypesFTB(Exit1,'Exit',sold,Liner1, 'Liner', Liner1P, Liner2,'Liner',Liner2P, Closer1, 'Closer',Closer1p, Closer2, 'Closer',Closer2p, Closer3, 'Closer',Closer3p, Exit2, 'Exit',Exit2P),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default)
	from @Manifest WHERE Exit1 is NOT NULL AND Exit1P IN ('LINER','FTB') 
	AND Exit1 NOT IN (IsNull(Liner1, ''), IsNull(Liner2, ''), IsNull(Closer1, ''), IsNull(Closer2,''),IsNull(Closer3,''))
	--#endregion
	
	--#region Exit2
	INSERT into @Salesman (id, SalemanID,SalemanName, PostID, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Exit2 ,Exit2N, Exit2P, 'Exit',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default),
	dbo.UFN_IM_GetSalesmanTypesFTB(Exit2,'Exit',sold,Liner1, 'Liner', Liner1P, Liner2,'Liner',Liner2P, Closer1, 'Closer',Closer1p, Closer2, 'Closer',Closer2p, Closer3, 'Closer',Closer3p, Exit1, 'Exit',Exit1P),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default)
	from @Manifest WHERE Exit2 is NOT NULL AND Exit2P IN ('LINER','FTB') 
	AND Exit2 NOT IN (IsNull(Liner1, ''), IsNull(Liner2, ''), IsNull(Closer1, ''), IsNull(Closer2,''),IsNull(Closer3,''),IsNull(Exit1,''))
--#endregion
	
	--=================== TABLA StatsByExitCloser =============================
--#region StatsByTFB Table
--DECLARE #StatsByFTB table
CREATE TABLE #StatsByFTB (
	SalemanID varchar(10),
	SalemanName varchar(40),
	SalemanType varchar(5),
	SalemanTypeN varchar(50),
	PostName varchar(50),
	TeamN varchar(30),
	TeamLeaderN varchar(40),
	SalesmanStatus VARCHAR(10) DEFAULT 'ACTIVE',
	SalesAmount money,
	OPP int,
	UPS money,	
	SalesRegular money,
	SalesExit money,
	Sales money,	
	Efficiency money,
	ClosingFactor money,
	SaleAverage money
	);
--#endregion

	INSERT INTO #StatsByFTB
	SELECT SalemanID, SalemanName, SalemanType,
	CASE SalemanType WHEN 'AS' THEN 'As A Closer' WHEN 'WITH' THEN 'Front To Back (With Closer)' WHEN 'OWN' THEN 'Front To Back (Own)' END AS SalemanTypeN,
	PostName, 
	(case when @GroupedByTeam = 0 then '' else TeamN end ) as TeamN,
	(case when @GroupedByTeam = 0 then '' else TeamLeaderN end ) as TeamLeaderN,
	(case when @GroupedByTeam = 0 then '' else SalesmanStatus end ) as SalesmanStatus,
	sum(Amount) AS SalesAmount, sum(Opp) AS Opp, sum(UPS) AS UPS,
	SUM(Sales) AS SalesRegular, Sum([Exit]) AS SalesExit, SUM (Sales + [Exit]) as Sales,
	dbo.UFN_OR_SecureDivision( sum(Amount),sum(UPS)) Efficiency,
	dbo.UFN_OR_SecureDivision(SUM (Sales + [Exit]),sum(UPS)) ClosingFactor,
	dbo.UFN_OR_SecureDivision(SUM (Amount),SUM (Sales + [Exit])) SaleAverage
	FROM(
		SELECT DISTINCT m.guID, m.saID, s.SalemanID,s.SalemanName, s.SalemanType,
		CASE WHEN dbo.UFN_IM_IsSelfGen(s.SalemanID,s.Role,m.guSelfGen)=1 THEN 
			CASE WHEN m.guOverflow = 1 THEN 'Overflow' ELSE 'Front To Middle' END
		ELSE ISNULL(CONVERT(varchar(40),PO.poN ),'NO POST')  END AS PostName,
		s.UPS, s.Amount, m.Opp,	
		CASE WHEN m.MembershipGroup='EXIT' THEN 0 ELSE s.Sales END AS Sales,
		CASE WHEN m.MembershipGroup='EXIT' THEN s.Sales ELSE 0 END AS [Exit],
		CASE WHEN t.SalesRoom = @SalesRoom AND ISNULL(CONVERT(varchar(1),tc.tsN),'') <> ISNULL(CONVERT(varchar(1),t.TeamN),'') THEN 'INACTIVE' ELSE 'ACTIVE'  END AS SalesmanStatus,
		CASE WHEN t.SalesRoom <> @SalesRoom THEN 'TEAMS OF OTHER SALES ROOMS' ELSE 
			CASE WHEN t.TeamN IS NULL THEN --si no tiene equipo
				CASE WHEN dbo.UFN_IM_IsSelfGen(s.SalemanID,s.Role,m.guSelfGen)=1 THEN -- si es un Self Gen obtenemos el equipo que asigno la Hostess
				ISNULL(CONVERT(varchar(40),m.TeamSelfGen),'NO TEAM')
				ELSE 'NO TEAM' END
			ELSE t.TeamN END
		END AS TeamN,
		CASE WHEN t.SalesRoom <> @SalesRoom THEN '' ELSE ISNULL(CONVERT(varchar(40),t.TeamLeaderN),'') END AS TeamLeaderN
		FROM @Salesman s	
		INNER JOIN @Manifest m ON m.id = s.id
		INNER JOIN Personnel p on p.peID= s.SalemanID
		LEFT JOIN Posts PO on PO.poID = s.PostID
		left join TeamsSalesmen tc on peTeam = tsID	
		cross apply  dbo.UFN_IM_GetPersonnelTeamSalesmenByDate(s.SalemanID,m.salesmanDate) t		
		WHERE SalemanType IS NOT NULL
		) AS x
		GROUP by SalemanID,SalemanName, SalemanType, PostName, TeamN, TeamLeaderN, x.SalesmanStatus

		
		INSERT INTO #StatsByFTB 
		SELECT SalemanID, SalemanName, 'OWN', 'Front To Back (Own)', PostName, TeamN, TeamLeaderN, SalesmanStatus, 0, OPP, UPS, 0,
		0, 0, 0, 0, 0 FROM #StatsByFTB WHERE SalemanType = 'WITH'
		
		UPDATE  #StatsByFTB SET 
			#StatsByFTB.Efficiency = tempFTB.Efficiency,
			#StatsByFTB.ClosingFactor = tempFTB.ClosingFactor,
			#StatsByFTB.SaleAverage = tempFTB.SaleAverage
		FROM ( 
			SELECT DISTINCT
				SalemanID, SalemanType, PostName, TeamN, TeamLeaderN, SalesmanStatus, 
				dbo.UFN_OR_SecureDivision(sum(SalesAmount),sum(UPS)) Efficiency,
				dbo.UFN_OR_SecureDivision(SUM(Sales) ,SUM(UPS)) ClosingFactor,
				dbo.UFN_OR_SecureDivision(SUM(SalesAmount) ,SUM(Sales)) SaleAverage 
			FROM #StatsByFTB 
			WHERE SalemanType = 'OWN'
			GROUP BY SalemanID, SalemanType, PostName, TeamN, TeamLeaderN, SalesmanStatus
			) tempFTB
		 WHERE
			tempFTB.SalemanID = #StatsByFTB.SalemanID AND tempFTB.SalemanType = #StatsByFTB.SalemanType AND tempFTB.PostName = #StatsByFTB.PostName AND
			tempFTB.TeamN = #StatsByFTB.TeamN AND tempFTB.TeamLeaderN = #StatsByFTB.TeamLeaderN AND tempFTB.SalesmanStatus = #StatsByFTB.SalesmanStatus AND #StatsByFTB.SalesAmount > 0.00
		  
		
IF @IncludeAllSalesmen=1 AND  (SELECT COUNT(*) from #StatsByFTB)>0
BEGIN			
	INSERT INTO #StatsByFTB(SalemanID, SalemanName, SalemanType, TeamN, TeamLeaderN)
	select P.peID, P.peN as SalesmanN, (SELECT top 1 SalemanType from #StatsByFTB) ,
		(case when @GroupedByTeam = 0 then '' else IsNull(tsN, 'NO TEAM') end ) as TeamN,
		(case when @GroupedByTeam = 0 then '' else IsNull(L.peN, '') end ) as TeamLeaderN
	from Personnel P 
	left join TeamsSalesmen on P.peTeamType = 'SA' and P.pePlaceID = tssr and P.peTeam = tsID 
	left join Personnel L on tsLeader = L.peID
	where
		-- Tipo de equipo
		P.peTeamType = 'SA'
		-- Clave del lugar
		and P.pePlaceID = @SalesRoom
		-- Personal activo
		and P.peA = 1		
		--si no se encuentra el vendedor
		AND NOT EXISTS (SELECT * from #StatsByFTB  WHERE SalemanID = p.peID)
END	


--===================================================================
--===================  SELECT  ===========================
--===================================================================

SELECT DISTINCT
	SalemanID,
	SalemanName,
	PostName,
	(TeamN+' '+TeamLeaderN) Team,
	SalesmanStatus,
	SUM(OwnSalesAmount) OAmount,
	SUM(OwnOPP) OOPP,
	SUM(OwnUPS) OUPS,
	SUM(OwnSalesRegular) OSales,
	SUM (OwnSalesExit) OExit,
	SUM(OwnSales) OTotal,
	SUM(OwnEfficiency) OEfficiency,
	SUM(OwnClosingFactor) OClosingFactor,
	SUM (OwnSaleAverage) OSaleAverage,
	SUM(WithSalesAmount) WAmount,
	SUM(WithOPP) WOPP,
	SUM(WithUPS) WUPS,
	SUM(WithSalesRegular) WSales,
	SUM (WithSalesExit) WExit,
	SUM(WithSales) WTotal,
	SUM(WithEfficiency) WEfficiency,
	SUM(WithClosingFactor) WClosingFactor,
	SUM (WithSaleAverage) WSaleAverage,
	(SUM(OwnSalesAmount) + SUM(WithSalesAmount)) AS TAmount,
	(SUM(OwnOPP)) as TOPP,
	(SUM(OwnUPS)) TUPS,
	(SUM(OwnSalesRegular) + SUM(WithSalesRegular)) as TSales,
	(SUM(OwnSalesExit) + SUM(WithSalesExit)) as TExit,
	(SUM(OwnSales) + SUM(WithSales)) as TTotal,		
	dbo.UFN_OR_SecureDivision( (SUM(OwnSalesAmount) + SUM(WithSalesAmount)) ,SUM(OwnUPS)) TEfficiency,
	dbo.UFN_OR_SecureDivision((SUM(OwnSales) + SUM(WithSales)) ,SUM(OwnUPS)) TClosingFactor,
	dbo.UFN_OR_SecureDivision((SUM(OwnSalesAmount) + SUM(WithSalesAmount)) ,(SUM(OwnSales) + SUM(WithSales))) TSaleAverage,
	SUM(AsSalesAmount) AAmount,
	SUM(AsOPP) AOOP,
	SUM(AsUPS) AOUPS,
	SUM(AsSalesRegular) ASales,
	SUM (AsSalesExit) AExit,
	SUM(AsSales) ATotal,
	SUM(AsEfficiency) AEfficiency,
	SUM(AsClosingFactor) AClosingFactor,
	SUM (AsSaleAverage) ASaleAverage
FROM
(
		SELECT 
			SalemanID, SalemanName, PostName, TeamN, TeamLeaderN, SalesmanStatus, 
			SUM(SalesAmount) OwnSalesAmount, SUM(OPP) OwnOPP, SUM(UPS) OwnUPS, SUM(SalesRegular) OwnSalesRegular, SUM(SalesExit) OwnSalesExit, SUM(Sales) OwnSales, SUM(Efficiency) OwnEfficiency, SUM(ClosingFactor) OwnClosingFactor, SUM (SaleAverage) OwnSaleAverage, 
			0 WithSalesAmount, 0 WithOPP, 0 WithUPS, 0 WithSalesRegular, 0 WithSalesExit, 0 WithSales, 0 WithEfficiency,	0 WithClosingFactor, 0 WithSaleAverage, 
			0 AsSalesAmount, 0 AsOPP, 0 AsUPS, 0 AsSalesRegular, 0 AsSalesExit, 0 AsSales, 0 AsEfficiency,	0 AsClosingFactor, 0 AsSaleAverage 
		 
		FROM #StatsByFTB
		WHERE SalemanType = 'OWN'
		GROUP BY SalemanID,SalemanName, PostName, TeamN,TeamLeaderN,SalesmanStatus
		
	UNION ALL
		SELECT 
			SalemanID, SalemanName, PostName, TeamN, TeamLeaderN, SalesmanStatus,
			0 OwnSalesAmount, 0 OwnOPP, 0 OUPS, 0 OwnSalesRegular, 0 OwnSalesExit, 0 OwnSales, 0 OwnEfficiency,	0 OwnClosingFactor, 0 OwnSaleAverage, 
			SUM(SalesAmount) WithSalesAmount, SUM(OPP) WithOPP, SUM(UPS) WithUPS, SUM(SalesRegular) WithSalesRegular, SUM(SalesExit) WithSalesExit, SUM(Sales) WithSales, SUM(Efficiency) WithEfficiency,	SUM(ClosingFactor) WithClosingFactor, SUM (SaleAverage) WithSaleAverage, 
			0 AsSalesAmount, 0 AsOPP, 0 AsUPS, 0 AsSalesRegular, 0 AsSalesExit, 0 AsSales, 0 AsEfficiency,	0 AsClosingFactor, 0 AsSaleAverage
		 
		FROM #StatsByFTB
		WHERE SalemanType = 'WITH'
		GROUP BY SalemanID,SalemanName, PostName, TeamN,TeamLeaderN,SalesmanStatus 
		
	UNION ALL
		SELECT 
			SalemanID, SalemanName, PostName, TeamN, TeamLeaderN, SalesmanStatus,
			0 OwnSalesAmount, 0 OwnOPP, 0 OUPS, 0 OwnSalesRegular, 0 OwnSalesExit, 0 OwnSales, 0 OwnEfficiency,	0 OwnClosingFactor, 0 OwnSaleAverage, 
			0 WithSalesAmount, 0 WithOPP, 0 WUPS, 0 WithSalesRegular, 0 WithSalesExit, 0 WithSales, 0 WithEfficiency,0 WithClosingFactor, 0 WithSaleAverage, 
			SUM(SalesAmount) AsSalesAmount, SUM(OPP) AsOPP, SUM(UPS) AsUPS, SUM(SalesRegular) AsSalesRegular, SUM(SalesExit) AsSalesExit, SUM(Sales) AsSales, SUM(Efficiency) AsEfficiency,	SUM(ClosingFactor) AsClosingFactor, SUM (SaleAverage) AsSaleAverage
		 
		FROM #StatsByFTB
		WHERE SalemanType = 'AS'
		GROUP BY SalemanID,SalemanName, PostName, TeamN,TeamLeaderN,SalesmanStatus 
) AS LST
GROUP BY SalemanID,SalemanName, PostName, TeamN,TeamLeaderN,SalesmanStatus 
ORDER BY SalemanID

DROP TABLE #StatsByFTB;
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptStatisticsByFTBCategories]    Script Date: 09/22/2016 19:14:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de estadisticas por FTB (Processor Sales)
**
** [michan] 11/Agosto/2016 Created
**
*/
create procedure [dbo].[USP_IM_RptStatisticsByFTBCategories]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@SalesRoom varchar(10),				-- Clave de la sala
	@SalesmanID varchar(10) = 'ALL',	-- Clave de un vendedor
	@GroupedByTeam bit = 0,			-- si se desea que esten agrupdo´por su equipo
	@IncludeAllSalesmen bit = 0			-- si se desea que esten todos los vendedores de la sala
as
--SET FMTONLY OFF
set nocount on
--=================== TABLA MANIFEST =============================
--#region ManifestTable
DECLARE @Manifest table (
--campos de huespedes
	id int identity(1,1),
	guID int,	
	guShow bit default 0,
	own bit,	
	TeamSelfGen varchar(20),
	guSelfGen bit,
	guOverflow bit default 0,
	salesmanDate DateTime,
	sold bit default 0,
	Opp int default 0,	
	MembershipGroup varchar(10),
	Locations VARCHAR(30),
		-- Campos de la tabla de ventas	
	saID int,
	procSales int,	
	saGrossAmount money default 0,
	-- Vendedores
	-- Liner 1
	Liner1 varchar(10),
	Liner1N varchar(40),
	Liner1P varchar(10),
	-- Liner 2
	Liner2 varchar(10),
	Liner2N varchar(40),
	Liner2P varchar(10),
	-- Closer 1
	Closer1 varchar(10),
	Closer1N varchar(40),
	Closer1P varchar(10),
	-- Closer 2
	Closer2 varchar(10),
	Closer2N varchar(40),
	Closer2p varchar(10),
	-- Closer 3
	Closer3 varchar(10),
	Closer3N varchar(40),
	Closer3p varchar(10),
	-- Exit 1
	Exit1 varchar(10),
	Exit1N varchar(40),
	Exit1P varchar(10),
	-- Exit 2
	Exit2 varchar(10),
	Exit2N varchar(40),
	Exit2P varchar(10));	
--#endregion

--#region Guest
--===================================================================
--=======================   MANIFEST    =============================
--===================================================================
INSERT into @Manifest (guID, guShow, own, salesmanDate, guSelfGen, TeamSelfGen, guOverflow,
saID, procSales, MembershipGroup, Locations, Liner1, Liner1N, Liner1P, Liner2, Liner2N, Liner2P,
Closer1, Closer1N, Closer1P, Closer2, Closer2N, Closer2p, Closer3, Closer3N, Closer3p, Exit1, Exit1N, Exit1P, Exit2, Exit2N, Exit2P)
select DISTINCT
	--Campos de la tabla de huespedes
	G.guID,	
	case WHEN (G.guTour=1 OR g.guWalkOut=1 OR ((G.guCTour=1 OR G.guSaveProgram=1) AND S.saID IS NOT NULL)) THEN 1 ELSE 0 END,
	dbo.UFN_IM_StringComparer(G.guLiner1,G.guLiner2,G.guCloser1,G.guCloser2,G.guCloser3,G.guExit1,G.guExit2),
	G.guShowD,
	G.guSelfGen,	
	G.guts,	
	G.guOverflow,
	S.sagu,
	0 procSales,
	MT.mtGroup,
	-- Locacion
	IsNull(LC.lcN, 'NO LOCATION CATEGORY') as loN,
	-- PERSONAL DEL SHOW	
	-- Vendedores
	-- Liner 1
	G.guLiner1 as guLiner1,
	GL1.peN as guLiner1N,
	CASE WHEN G.guLiner1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guLiner1) ELSE NULL END [Liner1P],
	-- Liner 2
	G.guLiner2 as guLiner2,
	GL2.peN as guLiner2N,
	CASE WHEN G.guLiner2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guLiner2) ELSE NULL END [Liner2P],
	-- Closer 1
	G.guCloser1 as guCloser1,
	GC1.peN as guCloser1N,
	CASE WHEN G.guCloser1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser1) ELSE NULL END [Closer1P],
	-- Closer 2
	G.guCloser2 as guCloser2,
	GC2.peN as guCloser2N,
	CASE WHEN G.guCloser2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser2) ELSE NULL END [Closer2P],
	-- Closer 3
	G.guCloser3 as guCloser3,
	GC3.peN as guCloser3N,
	CASE WHEN G.guCloser3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser3) ELSE NULL END [Closer3P],
	-- Exit 1
	G.guExit1 as guExit1,
	GE1.peN as guExit1N,
	CASE WHEN G.guExit1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guExit1) ELSE NULL END [Exit1P],
	G.guExit2 as guExit2,
	GE2.peN as guExit2N,
	CASE WHEN G.guExit2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guExit2) ELSE NULL END [Exit2P]
from Guests G
	left join Sales S on sagu = G.guID	
	left join MembershipTypes MT on MT.mtID = S.samt
	-- PERSONAL DEL SHOW	
	-- Vendedores
	left join Personnel GL1 on GL1.peID = G.guLiner1
	left join Personnel GL2 on GL2.peID = G.guLiner2
	left join Personnel GC1 on GC1.peID = G.guCloser1
	left join Personnel GC2 on GC2.peID = G.guCloser2
	left join Personnel GC3 on GC3.peID = G.guCloser3
	left join Personnel GE1 on GE1.peID = G.guExit1
	left join Personnel GE2 on GE2.peID = G.guExit2		
	--Segment 
	left join Agencies A on A.agID = G.guag
	left join SegmentsByAgency SA on SA.seID = A.agse
	left join LeadSources LS on LS.lsID = G.guls
	left join Locations LO on LO.loID = G.guloInvit
	left join LocationsCategories LC on LC.lcID = LO.lolc
	left join SegmentsByLeadSource SL on SL.soID = LS.lsso
	left join SegmentsCategories SCA on SCA.scID = SA.sesc
	left join SegmentsCategories SCL on SCL.scID = SL.sosc
where
	-- Fecha de show
	G.guShowD between @DateFrom and @DateTo
	-- Sala de ventas
	and G.gusr = @SalesRoom	
	-- Vendedor
	and (@SalesmanID = 'ALL'		
		-- Rol de Liner
		or (@SalesmanID = G.guLiner1 or @SalesmanID = G.guLiner2 or @SalesmanID = S.saLiner1 or @SalesmanID = S.saLiner2)
		-- Rol de Closer
		or (@SalesmanID = G.guCloser1 or @SalesmanID = G.guCloser2 or @SalesmanID = G.guCloser3 
			or @SalesmanID = S.saCloser1 or @SalesmanID = S.saCloser2 or @SalesmanID = S.saCloser3)
		-- Rol de Exit
		or (@SalesmanID = G.guExit1 or @SalesmanID = G.guExit2 or @SalesmanID = S.saExit1 or @SalesmanID = S.saExit2))
	
order by G.guID;
--#endregion
--=================== Insert SALES =============================
--#region insertSales

DECLARE @nReg int; -- numero de regitros
SET @nReg=(SELECT COUNT(*) FROM @Manifest);
DECLARE @cont int; -- almacena la cantidad de veces que se recorre el while
SET @cont = 1;
DECLARE @gu int; -- si tiene ventas
WHILE @nReg >= @cont 
BEGIN
	SET @gu = (SELECT guid FROM @Manifest WHERE id = @cont); --obtiene el id del gest	
	DECLARE @sale int;
	SET @sale = (SELECT TOP 1 sagu FROM Sales WHERE sagu = @gu);	
	--#region IF @sale

	IF @sale IS NOT NULL		
	BEGIN		
		UPDATE @Manifest SET sold = 1 WHERE id = @cont;
		SELECT 
		--Datos de la venta
			S.sagu AS GUID,
			dbo.UFN_IM_StringComparer(S.saLiner1,S.saLiner2,S.saCloser1,S.saCloser2,S.saCloser3,S.saExit1,S.saExit2) own,
			S.saID,
			S.saD,	
			S.saGrossAmount,						
			MT.mtGroup,
			-- Locacion
			IsNull(LC.lcN, 'NO LOCATION CATEGORY') as loN,
		--Datos de los vendedores			
			-- Liner 1
			S.saLiner1 as saLiner1,
			SL1.peN as saLiner1N,
			CASE WHEN S.saLiner1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saLiner1) ELSE NULL END [Liner1P],
			-- Liner 2
			S.saLiner2 as saLiner2,
			SL2.peN as saLiner2N,
			CASE WHEN S.saLiner2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saLiner2) ELSE NULL END [Liner2P],
			-- Closer 1
			S.saCloser1 as saCloser1,
			SC1.peN as saCloser1N,
			CASE WHEN S.saCloser1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saCloser1) ELSE NULL END [Closer1P],
			-- Closer 2
			S.saCloser2 as saCloser2,
			SC2.peN as saCloser2N,
			CASE WHEN S.saCloser2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saCloser2) ELSE NULL END [Closer2P],
			-- Closer 3
			S.saCloser3 as saCloser3,
			SC3.peN as saCloser3N,
			CASE WHEN S.saCloser3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saCloser3) ELSE NULL END [Closer3P],
			-- Exit 1
			S.saExit1 as saExit1,
			SE1.peN as saExit1N,
			CASE WHEN S.saExit1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saExit1) ELSE NULL END [Exit1P],
			-- Exit 2
			S.saExit2 as saExit2,
			SE2.peN as saExit2N,
			CASE WHEN S.saExit2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saExit2) ELSE NULL END [Exit2P],			
			1 Sold,
			CASE WHEN ST.ststc = 'DG' THEN 0 ELSE 1 END procSales
		INTO #tbTemp
		FROM Guests G
			LEFT JOIN Sales S on S.sagu = G.guID
			LEFT JOIN SaleTypes ST on ST.stID = S.sast
			left join MembershipTypes MT on MT.mtID = S.samt
			-- Vendedores
			left join Personnel SL1 on SL1.peID = S.saLiner1
			left join Personnel SL2 on SL2.peID = S.saLiner2
			left join Personnel SC1 on SC1.peID = S.saCloser1
			left join Personnel SC2 on SC2.peID = S.saCloser2
			left join Personnel SC3 on SC3.peID = S.saCloser3
			left join Personnel SE1 on SE1.peID = S.saExit1
			left join Personnel SE2 on SE2.peID = S.saExit2	
			-- Segment	
			left join Agencies A on A.agID = G.guag		
			left join SegmentsByAgency SA on SA.seID = A.agse
			left join LeadSources LS on LS.lsID = G.guls
			left join Locations LO on LO.loID = G.guloInvit
			left join LocationsCategories LC on LC.lcID = LO.lolc
			left join SegmentsByLeadSource SL on SL.soID = LS.lsso
			left join SegmentsCategories SCA on SCA.scID = SA.sesc
			left join SegmentsCategories SCL on SCL.scID = SL.sosc
		WHERE
			S.saD BETWEEN @DateFrom and @DateTo AND sagu = @gu
			AND S.sast NOT IN ('BUMP','REGEN')
			AND S.saProcD BETWEEN @DateFrom AND @DateTo
			AND (S.saCancel = 0 OR NOT S.saCancelD BETWEEN @DateFrom AND @DateTo)
			AND S.sasr = @SalesRoom		
			-- Vendedor 
		and (@SalesmanID = 'ALL'		
			-- Rol de Liner
			or (@SalesmanID = S.saLiner1 or @SalesmanID = S.saLiner2)
			-- Rol de Closer
			or (@SalesmanID = S.saCloser1 or @SalesmanID = S.saCloser2 or @SalesmanID = S.saCloser3)
			-- Rol de Exit
			or (@SalesmanID = S.saExit1 or @SalesmanID = S.saExit2))
		
		INSERT INTO @Manifest (guID, own, saID,salesmanDate,saGrossAmount, MembershipGroup, Locations,
		Liner1,Liner1N,Liner1P,Liner2,Liner2N,Liner2P,Closer1,Closer1N,Closer1P,Closer2,Closer2N,Closer2p,Closer3,Closer3N,Closer3p,
		Exit1,Exit1N,Exit1P,Exit2,Exit2N,Exit2P,sold,procSales)
		SELECT * FROM #tbTemp;
		DROP TABLE #tbTemp;
	END		
--#endregion
	SET @cont = @cont +1;
END;
--#endregion

--===================================================================
--===================  DEPOSIT SALES    =============================
--===================================================================
--#region Deposit Sales
INSERT into @Manifest (guID, own, salesmanDate, guSelfGen, TeamSelfGen, guOverflow, 
procSales, MembershipGroup, Locations, Liner1, Liner1N, Liner1P, Liner2, Liner2N, Liner2P, 
Closer1, Closer1N, Closer1P, Closer2, Closer2N, Closer2p, Closer3, Closer3N, Closer3p, Exit1, Exit1N, Exit1P, Exit2, Exit2N, Exit2P)
select 
	--#region Campos de la tabla de huespedes
	G.guID,	
	dbo.UFN_IM_StringComparer(G.guLiner1,G.guLiner2,G.guCloser1,G.guCloser2,G.guCloser3,G.guExit1,G.guExit2),	
	G.guShowD,
	G.guSelfGen,	
	G.guts,
	G.guOverflow,	
	0 procSales,
	MT.mtGroup,	
	-- Locacion
	IsNull(LC.lcN, 'NO LOCATION CATEGORY') as loN,
	--#endregion	
	--#region PERSONAL DEL SHOW	
	-- Vendedores
	-- Liner 1
	G.guLiner1 as guLiner1,
	GL1.peN as guLiner1N,
	CASE WHEN G.guLiner1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guLiner1) ELSE NULL END [Liner1P],
	-- Liner 2
	G.guLiner2 as guLiner2,
	GL2.peN as guLiner2N,
	CASE WHEN G.guLiner2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guLiner2) ELSE NULL END [Liner2P],
	-- Closer 1
	G.guCloser1 as guCloser1,
	GC1.peN as guCloser1N,
	CASE WHEN G.guCloser1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser1) ELSE NULL END [Closer1P],
	-- Closer 2
	G.guCloser2 as guCloser2,
	GC2.peN as guCloser2N,
	CASE WHEN G.guCloser2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser2) ELSE NULL END [Closer2P],
	-- Closer 3
	G.guCloser3 as guCloser3,
	GC3.peN as guCloser3N,
	CASE WHEN G.guCloser3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser3) ELSE NULL END [Closer3P],
	-- Exit 1
	G.guExit1 as guExit1,
	GE1.peN as guExit1N,
	CASE WHEN G.guExit1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guExit1) ELSE NULL END [Exit1P],
	-- Exit 2
	G.guExit2 as guExit2,
	GE2.peN as guExit2N,
	CASE WHEN G.guExit2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guExit2) ELSE NULL END [Exit2P]
	--#endregion
from Guests G	
	left join Sales S on S.sagu = G.guID
	left join MembershipTypes MT on MT.mtID = S.samt	
--#region PERSONAL DEL SHOW	
	-- Vendedores
	left join Personnel GL1 on GL1.peID = G.guLiner1
	left join Personnel GL2 on GL2.peID = G.guLiner2
	left join Personnel GC1 on GC1.peID = G.guCloser1
	left join Personnel GC2 on GC2.peID = G.guCloser2
	left join Personnel GC3 on GC3.peID = G.guCloser3
	left join Personnel GE1 on GE1.peID = G.guExit1
	left join Personnel GE2 on GE2.peID = G.guExit2	
	--#endregion	
	-- Segment
	left join Agencies A on A.agID = G.guag
	left join SegmentsByAgency SA on SA.seID = A.agse
	left join LeadSources LS on LS.lsID = G.guls
	left join Locations LO on LO.loID = G.guloInvit
	left join LocationsCategories LC on LC.lcID = LO.lolc
	left join SegmentsByLeadSource SL on SL.soID = LS.lsso
	left join SegmentsCategories SCA on SCA.scID = SA.sesc
	left join SegmentsCategories SCL on SCL.scID = SL.sosc
where
	G.guDepositSaleD between @DateFrom and @DateTo
	-- Fecha de show diferente de la fecha de venta deposito
	and G.guShowD <> G.guDepositSaleD
	-- Sala de ventas
	and G.gusr = @SalesRoom
	-- Vendedor 
	and (@SalesmanID = 'ALL'		
		-- Rol de Liner
		or (@SalesmanID = G.guLiner1 or @SalesmanID = G.guLiner2 or @SalesmanID = S.saLiner1 or @SalesmanID = S.saLiner2)
		-- Rol de Closer
		or (@SalesmanID = G.guCloser1 or @SalesmanID = G.guCloser2 or @SalesmanID = G.guCloser3
			or @SalesmanID = S.saCloser1 or @SalesmanID = S.saCloser2 or @SalesmanID = S.saCloser3)
		-- Rol de Exit
		or (@SalesmanID = G.guExit1 or @SalesmanID = G.guExit2 or @SalesmanID = S.saExit1 or @SalesmanID = S.saExit2))
	
order by G.guID;
--#endregion
--===================================================================
--======================  OTHER SALES    ============================
--===================================================================
--#region Other Sales
INSERT into @Manifest (guID, own, guSelfGen, salesmanDate,sold, Opp, saID, procSales, saGrossAmount,  MembershipGroup, Locations,
Liner1, Liner1N, Liner1P, Liner2, Liner2N, Liner2P, Closer1, Closer1N, Closer1P, Closer2, Closer2N, 
Closer2p, Closer3, Closer3N, Closer3p, Exit1, Exit1N, Exit1P, Exit2, Exit2N, Exit2P)
SELECT 
--Datos de la venta
	S.sagu AS GUID,
	dbo.UFN_IM_StringComparer(S.saLiner1,S.saLiner2,S.saCloser1,S.saCloser2,S.saCloser3,S.saExit1,S.saExit2),
	S.saSelfGen,
	S.saD,
	1 Sold,
	CASE WHEN dbo.UFN_IM_GetSaleType(@DateFrom,@DateTo,S.sast,ST.ststc,G.guDepSale,S.saD,S.saProcD,S.saCancelD,G.gusr,S.sasr,S.saByPhone) IN(6,10) THEN 1 ELSE 0 END [Opp],
	S.saID,
	CASE WHEN dbo.UFN_IM_GetSaleType(@DateFrom,@DateTo,S.sast,ST.ststc,G.guDepSale,S.saD,S.saProcD,S.saCancelD,G.gusr,S.sasr,S.saByPhone) NOT IN(5,14) THEN 1 ELSE 0 END [procSales],
	S.saGrossAmount,
	MT.mtGroup,
	-- Locacion
	IsNull(LC.lcN, 'NO LOCATION CATEGORY') as loN,
--Datos de los vendedores	
	-- Liner 1
	S.saLiner1 as saLiner1,
	SL1.peN as saLiner1N,
	CASE WHEN S.saLiner1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saLiner1) ELSE NULL END [Liner1P],
	-- Liner 2
	S.saLiner2 as saLiner2,
	SL2.peN as saLiner2N,
	CASE WHEN S.saLiner2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saLiner2) ELSE NULL END [Liner2P],
	-- Closer 1
	S.saCloser1 as saCloser1,
	SC1.peN as saCloser1N,
	CASE WHEN S.saCloser1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saCloser1) ELSE NULL END [Closer1P],
	-- Closer 2
	S.saCloser2 as saCloser2,
	SC2.peN as saCloser2N,
	CASE WHEN S.saCloser2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saCloser2) ELSE NULL END [Closer2P],
	-- Closer 3
	S.saCloser3 as saCloser3,
	SC3.peN as saCloser3N,
	CASE WHEN S.saCloser3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saCloser3) ELSE NULL END [Closer3P],
	-- Exit 1
	S.saExit1 as saExit1,
	SE1.peN as saExit1N,
	CASE WHEN S.saExit1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saExit1) ELSE NULL END [Exit1P],
	-- Exit 2
	S.saExit2 as saExit2,
	SE2.peN as saExit2N,
	CASE WHEN S.saExit2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saExit2) ELSE NULL END [Exit2P]
FROM Guests G --para obtener la fecha del show
	LEFT JOIN Sales S on S.sagu = G.guID
	LEFT JOIN SaleTypes ST on ST.stID = S.sast
	left join MembershipTypes MT on MT.mtID = S.samt
	-- Vendedores
	left join Personnel SL1 on SL1.peID = S.saLiner1
	left join Personnel SL2 on SL2.peID = S.saLiner2
	left join Personnel SC1 on SC1.peID = S.saCloser1
	left join Personnel SC2 on SC2.peID = S.saCloser2
	left join Personnel SC3 on SC3.peID = S.saCloser3
	left join Personnel SE1 on SE1.peID = S.saExit1
	left join Personnel SE2 on SE2.peID = S.saExit2		
	-- Segment	
	left join Agencies A on A.agID = G.guag
	left join SegmentsByAgency SA on SA.seID = A.agse
	left join LeadSources LS on LS.lsID = S.sals
	left join Locations LO on Lo.loID = S.salo
	left join LocationsCategories LC on LC.lcID = LO.lolc
	left join SegmentsByLeadSource SL on Sl.soID = LS.lsso
	left join SegmentsCategories SCA on SCA.scID = SA.sesc
	left join SegmentsCategories SCL on SCL.scID = SL.sosc
WHERE
	((--NO SHOWS
	(G.guShowD IS NULL OR NOT G.guShowD BETWEEN @DateFrom AND @DateTo)
	--FECHA DE --- VENTA	-----------------	Venta Procesable	-----------------------
	AND (S.saD BETWEEN @DateFrom AND @DateTo OR S.saProcD BETWEEN @DateFrom AND @DateTo --OR s.saCancelD BETWEEN @DateFrom AND @DateTo
	))
	-- Incluye Bumps, Regen y ventas de otras salas
	OR ((S.sast IN ('BUMP','REGEN') OR G.gusr <> S.sasr) AND S.saD BETWEEN @DateFrom AND @DateTo))
	AND S.saProcD BETWEEN @DateFrom AND @DateTo
	AND (S.saCancel = 0 OR NOT S.saCancelD BETWEEN @DateFrom AND @DateTo)
	AND S.sasr = @SalesRoom
	-- Vendedor
	and (@SalesmanID = 'ALL'
		-- Rol de Liner
		or (@SalesmanID = S.saLiner1 or @SalesmanID = S.saLiner2)
		-- Rol de Closer
		or (@SalesmanID = S.saCloser1 or @SalesmanID = S.saCloser2 or @SalesmanID = S.saCloser3)
		-- Rol de Exit
		or (@SalesmanID = S.saExit1 or @SalesmanID = S.saExit2))
	
ORDER BY S.sagu;
--#endregion


--=================== TABLA Salesman =============================
--#region Salesman Table
DECLARE @Salesman table (
	id int,	
	SalemanID varchar(10),
	SalemanName varchar(40),
	UPS money,
	Sales money,
	Amount money,
	PostID varchar(10),
	Role varchar(50),
	SalemanType varchar(50)	
	);
--#endregion
	
--=================== Insert Salesman =============================
	--#region Insert Salesman

	--#region Liner1
	INSERT into @Salesman (id, SalemanID,SalemanName, PostID, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Liner1 ,Liner1N, Liner1P, 'Liner',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Liner1,Liner2,default,default,default),
	dbo.UFN_IM_GetSalesmanTypesFTB(Liner1,'Liner',sold,Liner2, 'Liner', Liner2P, Closer1,'Closer',Closer1P, Closer2, 'Closer',Closer2p, Closer3, 'Closer',Closer3p, Exit1, 'Exit',Exit1P, Exit2, 'Exit',Exit2P),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Liner1,Liner2,default,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Liner1,Liner2,default,default,default)
	from @Manifest WHERE Liner1 is NOT NULL
	--#endregion
	
	--#region Liner2
	INSERT into @Salesman (id, SalemanID,SalemanName, PostID, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Liner2 ,Liner2N, Liner2P, 'Liner',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Liner1,Liner2,default,default,default),
	dbo.UFN_IM_GetSalesmanTypesFTB(Liner2,'Liner',sold,Liner1, 'Liner', Liner1P, Closer1,'Closer',Closer1P, Closer2, 'Closer',Closer2p, Closer3, 'Closer',Closer3p, Exit1, 'Exit',Exit1P, Exit2, 'Exit',Exit2P),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Liner1,Liner2,default,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Liner1,Liner2,default,default,default)
	from @Manifest WHERE Liner2 is NOT NULL 
	AND Liner2 NOT IN (IsNull(Liner1, ''))
--#endregion

	--#region Closer1
	INSERT into @Salesman (id, SalemanID,SalemanName, PostID, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Closer1 ,Closer1N, Closer1P, 'Closer',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	dbo.UFN_IM_GetSalesmanTypesFTB(Closer1,'Closer',sold,Liner1, 'Liner', Liner1P, Liner2,'Liner',Liner2P, Closer2, 'Closer',Closer2p, Closer3, 'Closer',Closer3p, Exit1, 'Exit',Exit1P, Exit2, 'Exit',Exit2P),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default)
	from @Manifest WHERE Closer1 is NOT NULL AND Closer1P IN ('LINER','FTB') 
	AND Closer1 NOT IN (IsNull(Liner1, ''), IsNull(Liner2, ''))
	--#endregion
	
	--#region Closer2
	INSERT into @Salesman (id, SalemanID,SalemanName, PostID, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Closer2 ,Closer2N, Closer2P, 'Closer',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	dbo.UFN_IM_GetSalesmanTypesFTB(Closer2,'Closer',sold,Liner1, 'Liner', Liner1P, Liner2,'Liner',Liner2P, Closer2, 'Closer',Closer1p, Closer3, 'Closer',Closer3p, Exit1, 'Exit',Exit1P, Exit2, 'Exit',Exit2P),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default)
	from @Manifest WHERE Closer2 is NOT NULL AND Closer2P IN ('LINER','FTB') 
	AND Closer2 NOT IN (IsNull(Liner1, ''), IsNull(Liner2, ''), IsNull(Closer1, ''))
	--#endregion
	
	--#region Closer3
	INSERT into @Salesman (id, SalemanID,SalemanName, PostID, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Closer3 ,Closer3N, Closer3P, 'Closer',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	dbo.UFN_IM_GetSalesmanTypesFTB(Closer3,'Closer',sold,Liner1, 'Liner', Liner1P, Liner2,'Liner',Liner2P, Closer1, 'Closer',Closer1p, Closer2, 'Closer',Closer2p, Exit1, 'Exit',Exit1P, Exit2, 'Exit',Exit2P),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default)
	from @Manifest WHERE Closer3 is NOT NULL AND Closer3P IN ('LINER','FTB') 
	AND Closer3 NOT IN (IsNull(Liner1, ''), IsNull(Liner2, ''), IsNull(Closer1, ''), IsNull(Closer2,''))
	--#endregion
	
	--#region Exit1
	INSERT into @Salesman (id, SalemanID,SalemanName, PostID, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Exit1 ,Exit1N, Exit1P, 'Exit',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default),
	dbo.UFN_IM_GetSalesmanTypesFTB(Exit1,'Exit',sold,Liner1, 'Liner', Liner1P, Liner2,'Liner',Liner2P, Closer1, 'Closer',Closer1p, Closer2, 'Closer',Closer2p, Closer3, 'Closer',Closer3p, Exit2, 'Exit',Exit2P),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default)
	from @Manifest WHERE Exit1 is NOT NULL AND Exit1P IN ('LINER','FTB') 
	AND Exit1 NOT IN (IsNull(Liner1, ''), IsNull(Liner2, ''), IsNull(Closer1, ''), IsNull(Closer2,''),IsNull(Closer3,''))
	--#endregion
	
	--#region Exit2
	INSERT into @Salesman (id, SalemanID,SalemanName, PostID, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Exit2 ,Exit2N, Exit2P, 'Exit',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default),
	dbo.UFN_IM_GetSalesmanTypesFTB(Exit2,'Exit',sold,Liner1, 'Liner', Liner1P, Liner2,'Liner',Liner2P, Closer1, 'Closer',Closer1p, Closer2, 'Closer',Closer2p, Closer3, 'Closer',Closer3p, Exit1, 'Exit',Exit1P),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default)
	from @Manifest WHERE Exit2 is NOT NULL AND Exit2P IN ('LINER','FTB') 
	AND Exit2 NOT IN (IsNull(Liner1, ''), IsNull(Liner2, ''), IsNull(Closer1, ''), IsNull(Closer2,''),IsNull(Closer3,''),IsNull(Exit1,''))
--#endregion
	
	--=================== TABLA StatsByExitCloser =============================
--#region StatsByExitCloser Table
DECLARE @StatsByFTB table (
	SalemanID varchar(10),
	SalemanName varchar(40),
	SalemanType varchar(5),
	SalemanTypeN varchar(50),
	PostName varchar(50),
	Locations varchar(30),
	TeamN varchar(30),
	TeamLeaderN varchar(40),
	SalesmanStatus VARCHAR(10) DEFAULT 'ACTIVE',
	SalesAmount money,
	OPP int,
	UPS money,	
	SalesRegular money,
	SalesExit money,
	Sales money,	
	Efficiency money,
	ClosingFactor money,
	SaleAverage money
	);
--#endregion

	INSERT INTO @StatsByFTB
	SELECT SalemanID, SalemanName, SalemanType,
	CASE SalemanType WHEN 'AS' THEN 'As A Closer' WHEN 'WITH' THEN 'Front To Back (With Closer)' WHEN 'OWN' THEN 'Front To Back (Own)' END AS SalemanTypeN,
	
	PostName, --Locations, TeamN, TeamLeaderN, SalesmanStatus,
	Locations,
	(case when @GroupedByTeam = 0 then '' else TeamN end ) as TeamN,
	(case when @GroupedByTeam = 0 then '' else TeamLeaderN end ) as TeamLeaderN,
	(case when @GroupedByTeam = 0 then '' else SalesmanStatus end ) as SalesmanStatus,
	
	
	sum(Amount) AS SalesAmount, sum(Opp) AS Opp, sum(UPS) AS UPS,
	SUM(Sales) AS SalesRegular, Sum([Exit]) AS SalesExit, SUM (Sales + [Exit]) as Sales,
	dbo.UFN_OR_SecureDivision( sum(Amount),sum(UPS)) Efficiency,
	dbo.UFN_OR_SecureDivision(SUM (Sales + [Exit]),sum(UPS)) ClosingFactor,
	dbo.UFN_OR_SecureDivision(SUM (Amount),SUM (Sales + [Exit])) SaleAverage
	FROM(
		SELECT DISTINCT m.guID, m.saID, s.SalemanID,s.SalemanName,m.Locations, s.SalemanType,
		CASE WHEN dbo.UFN_IM_IsSelfGen(s.SalemanID,s.Role,m.guSelfGen)=1 THEN 
			CASE WHEN m.guOverflow = 1 THEN 'Overflow' ELSE 'Front To Middle' END
		ELSE ISNULL(CONVERT(varchar(40),PO.poN ),'NO POST')  END AS PostName,
		s.UPS, s.Amount, m.Opp,	
		CASE WHEN m.MembershipGroup='EXIT' THEN 0 ELSE s.Sales END AS Sales,
		CASE WHEN m.MembershipGroup='EXIT' THEN s.Sales ELSE 0 END AS [Exit],
		CASE WHEN t.SalesRoom = @SalesRoom AND ISNULL(CONVERT(varchar(1),tc.tsN),'') <> ISNULL(CONVERT(varchar(1),t.TeamN),'') THEN 'INACTIVE' ELSE 'ACTIVE'  END AS SalesmanStatus,
		CASE WHEN t.SalesRoom <> @SalesRoom THEN 'TEAMS OF OTHER SALES ROOMS' ELSE 
			CASE WHEN t.TeamN IS NULL THEN --si no tiene equipo
				CASE WHEN dbo.UFN_IM_IsSelfGen(s.SalemanID,s.Role,m.guSelfGen)=1 THEN -- si es un Self Gen obtenemos el equipo que asigno la Hostess
				ISNULL(CONVERT(varchar(40),m.TeamSelfGen),'NO TEAM')
				ELSE 'NO TEAM' END
			ELSE t.TeamN END
		END AS TeamN,
		CASE WHEN t.SalesRoom <> @SalesRoom THEN '' ELSE ISNULL(CONVERT(varchar(40),t.TeamLeaderN),'') END AS TeamLeaderN
		FROM @Salesman s	
		INNER JOIN @Manifest m ON m.id = s.id
		INNER JOIN Personnel p on p.peID= s.SalemanID
		LEFT JOIN Posts PO on PO.poID = s.PostID
		left join TeamsSalesmen tc on peTeam = tsID	
		cross apply  dbo.UFN_IM_GetPersonnelTeamSalesmenByDate(s.SalemanID,m.salesmanDate) t		
		WHERE SalemanType IS NOT NULL
		) AS x
		GROUP by SalemanID,SalemanName, Locations, SalemanType, PostName, TeamN, TeamLeaderN, x.SalesmanStatus
		
		
		
		

IF @IncludeAllSalesmen=1 AND  (SELECT COUNT(*) from @StatsByFTB)>0
BEGIN			
	INSERT INTO @StatsByFTB(SalemanID, SalemanName, SalemanType, TeamN, TeamLeaderN)
	select P.peID, P.peN as SalesmanN, (SELECT top 1 SalemanType from @StatsByFTB), 
		(case when @GroupedByTeam = 0 then '' else IsNull(tsN, 'NO TEAM') end ) as TeamN,
		(case when @GroupedByTeam = 0 then '' else IsNull(L.peN, '') end ) as TeamLeaderN
	from Personnel P 
	left join TeamsSalesmen on P.peTeamType = 'SA' and P.pePlaceID = tssr and P.peTeam = tsID 
	left join Personnel L on tsLeader = L.peID
	where
		-- Tipo de equipo
		P.peTeamType = 'SA'
		-- Clave del lugar
		and P.pePlaceID = @SalesRoom
		-- Personal activo
		and P.peA = 1		
		--si no se encuentra el vendedor
		AND NOT EXISTS (SELECT * from @StatsByFTB  WHERE SalemanID = p.peID)
END	
	
--===================================================================
--===================  SELECT  ===========================
--===================================================================

SELECT DISTINCT
	SalemanID,
	SalemanName,
	PostName,
    Locations,
	
	(TeamN+'   '+TeamLeaderN) Team,
	SalesmanStatus,
	
	(SUM(OwnUPS) + SUM(WithUPS) + SUM(AsUPS)) UPS,
	SUM(OwnSalesAmount) Own,
	SUM(WithSalesAmount) WithCloser,
	SUM(AsSalesAmount) AsCloser,
	SUM(OwnSalesAmount) Total,
	SUM(OwnSalesRegular) Sales,
	SUM (OwnSalesExit) 'Exit',
	SUM(OwnSales) TotalSales,
	dbo.UFN_OR_SecureDivision( (SUM(OwnSalesAmount) + SUM(WithSalesAmount) + SUM(AsSalesAmount)), (SUM(OwnUPS) + SUM(WithUPS) + SUM(AsUPS))) Efficiency,
	dbo.UFN_OR_SecureDivision(SUM(OwnSales), (SUM(OwnUPS) + SUM(WithUPS) + SUM(AsUPS))) ClosingFactor,
	dbo.UFN_OR_SecureDivision((SUM(OwnSalesAmount) + SUM(WithSalesAmount) + SUM(AsSalesAmount)), (SUM(OwnSales) + SUM(WithSales) + SUM(AsSales))) SaleAverage

FROM
(
		SELECT 
			SalemanID, SalemanName, Locations, PostName, TeamN, TeamLeaderN, SalesmanStatus, 
			SUM(SalesAmount) OwnSalesAmount, SUM(OPP) OwnOPP, SUM(UPS) OwnUPS, SUM(SalesRegular) OwnSalesRegular, SUM(SalesExit) OwnSalesExit, SUM(Sales) OwnSales, SUM(Efficiency) OwnEfficiency, SUM(ClosingFactor) OwnClosingFactor, SUM (SaleAverage) OwnSaleAverage, 
			0 WithSalesAmount, 0 WithOPP, 0 WithUPS, 0 WithSalesRegular, 0 WithSalesExit, 0 WithSales, 0 WithEfficiency,	0 WithClosingFactor, 0 WithSaleAverage, 
			0 AsSalesAmount, 0 AsOPP, 0 AsUPS, 0 AsSalesRegular, 0 AsSalesExit, 0 AsSales, 0 AsEfficiency,	0 AsClosingFactor, 0 AsSaleAverage 
		 
		FROM @StatsByFTB
		WHERE SalemanType = 'OWN'
		GROUP BY SalemanID,SalemanName, Locations, PostName, TeamN,TeamLeaderN,SalesmanStatus
		
	UNION ALL
		SELECT 
			SalemanID, SalemanName, Locations, PostName, TeamN, TeamLeaderN, SalesmanStatus,
			0 OwnSalesAmount, 0 OwnOPP, 0 OUPS, 0 OwnSalesRegular, 0 OwnSalesExit, 0 OwnSales, 0 OwnEfficiency,	0 OwnClosingFactor, 0 OwnSaleAverage, 
			SUM(SalesAmount) WithSalesAmount, SUM(OPP) WithOPP, SUM(UPS) WithUPS, SUM(SalesRegular) WithSalesRegular, SUM(SalesExit) WithSalesExit, SUM(Sales) WithSales, SUM(Efficiency) WithEfficiency,	SUM(ClosingFactor) WithClosingFactor, SUM (SaleAverage) WithSaleAverage, 
			0 AsSalesAmount, 0 AsOPP, 0 AsUPS, 0 AsSalesRegular, 0 AsSalesExit, 0 AsSales, 0 AsEfficiency,	0 AsClosingFactor, 0 AsSaleAverage
		 
		FROM @StatsByFTB
		WHERE SalemanType = 'WITH'
		GROUP BY SalemanID,SalemanName, Locations, PostName, TeamN,TeamLeaderN,SalesmanStatus 
		
	UNION ALL
		SELECT 
			SalemanID, SalemanName, Locations, PostName, TeamN, TeamLeaderN, SalesmanStatus,
			0 OwnSalesAmount, 0 OwnOPP, 0 OUPS, 0 OwnSalesRegular, 0 OwnSalesExit, 0 OwnSales, 0 OwnEfficiency,	0 OwnClosingFactor, 0 OwnSaleAverage, 
			0 WithSalesAmount, 0 WithOPP, 0 WUPS, 0 WithSalesRegular, 0 WithSalesExit, 0 WithSales, 0 WithEfficiency,0 WithClosingFactor, 0 WithSaleAverage, 
			SUM(SalesAmount) AsSalesAmount, SUM(OPP) AsOPP, SUM(UPS) AsUPS, SUM(SalesRegular) AsSalesRegular, SUM(SalesExit) AsSalesExit, SUM(Sales) AsSales, SUM(Efficiency) AsEfficiency,	SUM(ClosingFactor) AsClosingFactor, SUM (SaleAverage) AsSaleAverage
		 
		FROM @StatsByFTB
		WHERE SalemanType = 'AS'
		GROUP BY SalemanID,SalemanName, Locations, PostName, TeamN,TeamLeaderN,SalesmanStatus 
) AS LST
GROUP BY SalemanID,SalemanName, Locations, PostName, TeamN,TeamLeaderN,SalesmanStatus
ORDER BY Locations, Total DESC


GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptStatisticsByFTBLocations]    Script Date: 09/22/2016 19:14:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de estadisticas por FTB (Processor Sales)
**
** [michan] 21/Jul/2016 Created
**
*/
create procedure [dbo].[USP_IM_RptStatisticsByFTBLocations]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@SalesRoom varchar(10),				-- Clave de la sala
	@SalesmanID varchar(10) = 'ALL',	-- Clave de un vendedor
	@GroupedByTeam bit = 0,			-- si se desea que esten agrupdo´por su equipo	
	@IncludeAllSalesmen bit = 0			-- si se desea que esten todos los vendedores de la sala
as
--SET FMTONLY OFF
set nocount on
--=================== TABLA MANIFEST =============================
--#region ManifestTable
DECLARE @Manifest table (
--campos de huespedes
	id int identity(1,1),
	guID int,	
	guShow bit default 0,
	own bit,	
	TeamSelfGen varchar(20),
	guSelfGen bit,
	guOverflow bit default 0,
	salesmanDate DateTime,
	sold bit default 0,
	Opp int default 0,	
	MembershipGroup varchar(10),
	Locations VARCHAR(30),
		-- Campos de la tabla de ventas	
	saID int,
	procSales int,	
	saGrossAmount money default 0,
	-- Vendedores
	-- Liner 1
	Liner1 varchar(10),
	Liner1N varchar(40),
	Liner1P varchar(10),
	-- Liner 2
	Liner2 varchar(10),
	Liner2N varchar(40),
	Liner2P varchar(10),
	-- Closer 1
	Closer1 varchar(10),
	Closer1N varchar(40),
	Closer1P varchar(10),
	-- Closer 2
	Closer2 varchar(10),
	Closer2N varchar(40),
	Closer2p varchar(10),
	-- Closer 3
	Closer3 varchar(10),
	Closer3N varchar(40),
	Closer3p varchar(10),
	-- Exit 1
	Exit1 varchar(10),
	Exit1N varchar(40),
	Exit1P varchar(10),
	-- Exit 2
	Exit2 varchar(10),
	Exit2N varchar(40),
	Exit2P varchar(10));	
--#endregion

--#region Guest
--===================================================================
--=======================   MANIFEST    =============================
--===================================================================
INSERT into @Manifest (guID, guShow, own, salesmanDate, guSelfGen, TeamSelfGen, guOverflow,
saID, procSales, MembershipGroup, Locations, Liner1, Liner1N, Liner1P, Liner2, Liner2N, Liner2P,
Closer1, Closer1N, Closer1P, Closer2, Closer2N, Closer2p, Closer3, Closer3N, Closer3p, Exit1, Exit1N, Exit1P, Exit2, Exit2N, Exit2P)
select DISTINCT
	--Campos de la tabla de huespedes
	G.guID,	
	case WHEN (G.guTour=1 OR g.guWalkOut=1 OR ((G.guCTour=1 OR G.guSaveProgram=1) AND S.saID IS NOT NULL)) THEN 1 ELSE 0 END,
	dbo.UFN_IM_StringComparer(G.guLiner1,G.guLiner2,G.guCloser1,G.guCloser2,G.guCloser3,G.guExit1,G.guExit2),
	G.guShowD,
	G.guSelfGen,	
	G.guts,	
	G.guOverflow,
	S.sagu,
	0 procSales,
	MT.mtGroup,
	-- Locacion
	LO.loN,
	-- PERSONAL DEL SHOW	
	-- Vendedores
	-- Liner 1
	G.guLiner1 as guLiner1,
	GL1.peN as guLiner1N,
	CASE WHEN G.guLiner1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guLiner1) ELSE NULL END [Liner1P],
	-- Liner 2
	G.guLiner2 as guLiner2,
	GL2.peN as guLiner2N,
	CASE WHEN G.guLiner2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guLiner2) ELSE NULL END [Liner2P],
	-- Closer 1
	G.guCloser1 as guCloser1,
	GC1.peN as guCloser1N,
	CASE WHEN G.guCloser1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser1) ELSE NULL END [Closer1P],
	-- Closer 2
	G.guCloser2 as guCloser2,
	GC2.peN as guCloser2N,
	CASE WHEN G.guCloser2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser2) ELSE NULL END [Closer2P],
	-- Closer 3
	G.guCloser3 as guCloser3,
	GC3.peN as guCloser3N,
	CASE WHEN G.guCloser3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser3) ELSE NULL END [Closer3P],
	-- Exit 1
	G.guExit1 as guExit1,
	GE1.peN as guExit1N,
	CASE WHEN G.guExit1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guExit1) ELSE NULL END [Exit1P],
	G.guExit2 as guExit2,
	GE2.peN as guExit2N,
	CASE WHEN G.guExit2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guExit2) ELSE NULL END [Exit2P]
from Guests G
	left join Sales S on sagu = G.guID	
	left join MembershipTypes MT on MT.mtID = S.samt
	-- PERSONAL DEL SHOW	
	-- Vendedores
	left join Personnel GL1 on GL1.peID = G.guLiner1
	left join Personnel GL2 on GL2.peID = G.guLiner2
	left join Personnel GC1 on GC1.peID = G.guCloser1
	left join Personnel GC2 on GC2.peID = G.guCloser2
	left join Personnel GC3 on GC3.peID = G.guCloser3
	left join Personnel GE1 on GE1.peID = G.guExit1
	left join Personnel GE2 on GE2.peID = G.guExit2		
	--Segment 
	left join Agencies A on A.agID = G.guag
	left join SegmentsByAgency SA on SA.seID = A.agse
	left join LeadSources LS on LS.lsID = G.guls
	left join Locations LO on LO.loID = G.guloInvit
	left join LocationsCategories LC on LC.lcID = LO.lolc
	left join SegmentsByLeadSource SL on SL.soID = LS.lsso
	left join SegmentsCategories SCA on SCA.scID = SA.sesc
	left join SegmentsCategories SCL on SCL.scID = SL.sosc
where
	-- Fecha de show
	G.guShowD between @DateFrom and @DateTo
	-- Sala de ventas
	and G.gusr = @SalesRoom	
	-- Vendedor
	and (@SalesmanID = 'ALL'		
		-- Rol de Liner
		or (@SalesmanID = G.guLiner1 or @SalesmanID = G.guLiner2 or @SalesmanID = S.saLiner1 or @SalesmanID = S.saLiner2)
		-- Rol de Closer
		or (@SalesmanID = G.guCloser1 or @SalesmanID = G.guCloser2 or @SalesmanID = G.guCloser3 
			or @SalesmanID = S.saCloser1 or @SalesmanID = S.saCloser2 or @SalesmanID = S.saCloser3)
		-- Rol de Exit
		or (@SalesmanID = G.guExit1 or @SalesmanID = G.guExit2 or @SalesmanID = S.saExit1 or @SalesmanID = S.saExit2))
	
order by G.guID;
--#endregion
--=================== Insert SALES =============================
--#region insertSales

DECLARE @nReg int; -- numero de regitros
SET @nReg=(SELECT COUNT(*) FROM @Manifest);
DECLARE @cont int; -- almacena la cantidad de veces que se recorre el while
SET @cont = 1;
DECLARE @gu int; -- si tiene ventas
WHILE @nReg >= @cont 
BEGIN
	SET @gu = (SELECT guid FROM @Manifest WHERE id = @cont); --obtiene el id del gest	
	DECLARE @sale int;
	SET @sale = (SELECT TOP 1 sagu FROM Sales WHERE sagu = @gu);	
	--#region IF @sale

	IF @sale IS NOT NULL		
	BEGIN		
		UPDATE @Manifest SET sold = 1 WHERE id = @cont;
		SELECT 
		--Datos de la venta
			S.sagu AS GUID,
			dbo.UFN_IM_StringComparer(S.saLiner1,S.saLiner2,S.saCloser1,S.saCloser2,S.saCloser3,S.saExit1,S.saExit2) own,
			S.saID,
			S.saD,	
			S.saGrossAmount,						
			MT.mtGroup,
			-- Locacion
			LO.loN,
		--Datos de los vendedores			
			-- Liner 1
			S.saLiner1 as saLiner1,
			SL1.peN as saLiner1N,
			CASE WHEN S.saLiner1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saLiner1) ELSE NULL END [Liner1P],
			-- Liner 2
			S.saLiner2 as saLiner2,
			SL2.peN as saLiner2N,
			CASE WHEN S.saLiner2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saLiner2) ELSE NULL END [Liner2P],
			-- Closer 1
			S.saCloser1 as saCloser1,
			SC1.peN as saCloser1N,
			CASE WHEN S.saCloser1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saCloser1) ELSE NULL END [Closer1P],
			-- Closer 2
			S.saCloser2 as saCloser2,
			SC2.peN as saCloser2N,
			CASE WHEN S.saCloser2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saCloser2) ELSE NULL END [Closer2P],
			-- Closer 3
			S.saCloser3 as saCloser3,
			SC3.peN as saCloser3N,
			CASE WHEN S.saCloser3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saCloser3) ELSE NULL END [Closer3P],
			-- Exit 1
			S.saExit1 as saExit1,
			SE1.peN as saExit1N,
			CASE WHEN S.saExit1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saExit1) ELSE NULL END [Exit1P],
			-- Exit 2
			S.saExit2 as saExit2,
			SE2.peN as saExit2N,
			CASE WHEN S.saExit2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saExit2) ELSE NULL END [Exit2P],			
			1 Sold,
			CASE WHEN ST.ststc = 'DG' THEN 0 ELSE 1 END procSales
		INTO #tbTemp
		FROM Guests G
			LEFT JOIN Sales S on S.sagu = G.guID
			LEFT JOIN SaleTypes ST on ST.stID = S.sast
			left join MembershipTypes MT on MT.mtID = S.samt
			-- Vendedores
			left join Personnel SL1 on SL1.peID = S.saLiner1
			left join Personnel SL2 on SL2.peID = S.saLiner2
			left join Personnel SC1 on SC1.peID = S.saCloser1
			left join Personnel SC2 on SC2.peID = S.saCloser2
			left join Personnel SC3 on SC3.peID = S.saCloser3
			left join Personnel SE1 on SE1.peID = S.saExit1
			left join Personnel SE2 on SE2.peID = S.saExit2	
			-- Segment	
			left join Agencies A on A.agID = G.guag		
			left join SegmentsByAgency SA on SA.seID = A.agse
			left join LeadSources LS on LS.lsID = G.guls
			left join Locations LO on LO.loID = G.guloInvit
			left join LocationsCategories LC on LC.lcID = LO.lolc
			left join SegmentsByLeadSource SL on SL.soID = LS.lsso
			left join SegmentsCategories SCA on SCA.scID = SA.sesc
			left join SegmentsCategories SCL on SCL.scID = SL.sosc
		WHERE
			S.saD BETWEEN @DateFrom and @DateTo AND sagu = @gu
			AND S.sast NOT IN ('BUMP','REGEN')
			AND S.saProcD BETWEEN @DateFrom AND @DateTo
			AND (S.saCancel = 0 OR NOT S.saCancelD BETWEEN @DateFrom AND @DateTo)
			AND S.sasr = @SalesRoom		
			-- Vendedor 
		and (@SalesmanID = 'ALL'		
			-- Rol de Liner
			or (@SalesmanID = S.saLiner1 or @SalesmanID = S.saLiner2)
			-- Rol de Closer
			or (@SalesmanID = S.saCloser1 or @SalesmanID = S.saCloser2 or @SalesmanID = S.saCloser3)
			-- Rol de Exit
			or (@SalesmanID = S.saExit1 or @SalesmanID = S.saExit2))
		
		INSERT INTO @Manifest (guID, own, saID,salesmanDate,saGrossAmount, MembershipGroup, Locations,
		Liner1,Liner1N,Liner1P,Liner2,Liner2N,Liner2P,Closer1,Closer1N,Closer1P,Closer2,Closer2N,Closer2p,Closer3,Closer3N,Closer3p,
		Exit1,Exit1N,Exit1P,Exit2,Exit2N,Exit2P,sold,procSales)
		SELECT * FROM #tbTemp;
		DROP TABLE #tbTemp;
	END		
--#endregion
	SET @cont = @cont +1;
END;
--#endregion

--===================================================================
--===================  DEPOSIT SALES    =============================
--===================================================================
--#region Deposit Sales
INSERT into @Manifest (guID, own, salesmanDate, guSelfGen, TeamSelfGen, guOverflow, 
procSales, MembershipGroup, Locations, Liner1, Liner1N, Liner1P, Liner2, Liner2N, Liner2P, 
Closer1, Closer1N, Closer1P, Closer2, Closer2N, Closer2p, Closer3, Closer3N, Closer3p, Exit1, Exit1N, Exit1P, Exit2, Exit2N, Exit2P)
select 
	--#region Campos de la tabla de huespedes
	G.guID,	
	dbo.UFN_IM_StringComparer(G.guLiner1,G.guLiner2,G.guCloser1,G.guCloser2,G.guCloser3,G.guExit1,G.guExit2),	
	G.guShowD,
	G.guSelfGen,	
	G.guts,
	G.guOverflow,	
	0 procSales,
	MT.mtGroup,	
	-- Locacion
	LO.loN,
	--#endregion	
	--#region PERSONAL DEL SHOW	
	-- Vendedores
	-- Liner 1
	G.guLiner1 as guLiner1,
	GL1.peN as guLiner1N,
	CASE WHEN G.guLiner1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guLiner1) ELSE NULL END [Liner1P],
	-- Liner 2
	G.guLiner2 as guLiner2,
	GL2.peN as guLiner2N,
	CASE WHEN G.guLiner2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guLiner2) ELSE NULL END [Liner2P],
	-- Closer 1
	G.guCloser1 as guCloser1,
	GC1.peN as guCloser1N,
	CASE WHEN G.guCloser1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser1) ELSE NULL END [Closer1P],
	-- Closer 2
	G.guCloser2 as guCloser2,
	GC2.peN as guCloser2N,
	CASE WHEN G.guCloser2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser2) ELSE NULL END [Closer2P],
	-- Closer 3
	G.guCloser3 as guCloser3,
	GC3.peN as guCloser3N,
	CASE WHEN G.guCloser3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser3) ELSE NULL END [Closer3P],
	-- Exit 1
	G.guExit1 as guExit1,
	GE1.peN as guExit1N,
	CASE WHEN G.guExit1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guExit1) ELSE NULL END [Exit1P],
	-- Exit 2
	G.guExit2 as guExit2,
	GE2.peN as guExit2N,
	CASE WHEN G.guExit2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guExit2) ELSE NULL END [Exit2P]
	--#endregion
from Guests G	
	left join Sales S on S.sagu = G.guID
	left join MembershipTypes MT on MT.mtID = S.samt	
--#region PERSONAL DEL SHOW	
	-- Vendedores
	left join Personnel GL1 on GL1.peID = G.guLiner1
	left join Personnel GL2 on GL2.peID = G.guLiner2
	left join Personnel GC1 on GC1.peID = G.guCloser1
	left join Personnel GC2 on GC2.peID = G.guCloser2
	left join Personnel GC3 on GC3.peID = G.guCloser3
	left join Personnel GE1 on GE1.peID = G.guExit1
	left join Personnel GE2 on GE2.peID = G.guExit2	
	--#endregion	
	-- Segment
	left join Agencies A on A.agID = G.guag
	left join SegmentsByAgency SA on SA.seID = A.agse
	left join LeadSources LS on LS.lsID = G.guls
	left join Locations LO on LO.loID = G.guloInvit
	left join LocationsCategories LC on LC.lcID = LO.lolc
	left join SegmentsByLeadSource SL on SL.soID = LS.lsso
	left join SegmentsCategories SCA on SCA.scID = SA.sesc
	left join SegmentsCategories SCL on SCL.scID = SL.sosc
where
	G.guDepositSaleD between @DateFrom and @DateTo
	-- Fecha de show diferente de la fecha de venta deposito
	and G.guShowD <> G.guDepositSaleD
	-- Sala de ventas
	and G.gusr = @SalesRoom
	-- Vendedor 
	and (@SalesmanID = 'ALL'		
		-- Rol de Liner
		or (@SalesmanID = G.guLiner1 or @SalesmanID = G.guLiner2 or @SalesmanID = S.saLiner1 or @SalesmanID = S.saLiner2)
		-- Rol de Closer
		or (@SalesmanID = G.guCloser1 or @SalesmanID = G.guCloser2 or @SalesmanID = G.guCloser3
			or @SalesmanID = S.saCloser1 or @SalesmanID = S.saCloser2 or @SalesmanID = S.saCloser3)
		-- Rol de Exit
		or (@SalesmanID = G.guExit1 or @SalesmanID = G.guExit2 or @SalesmanID = S.saExit1 or @SalesmanID = S.saExit2))
	
order by G.guID;
--#endregion
--===================================================================
--======================  OTHER SALES    ============================
--===================================================================
--#region Other Sales
INSERT into @Manifest (guID, own, guSelfGen, salesmanDate,sold, Opp, saID, procSales, saGrossAmount,  MembershipGroup, Locations,
Liner1, Liner1N, Liner1P, Liner2, Liner2N, Liner2P, Closer1, Closer1N, Closer1P, Closer2, Closer2N, 
Closer2p, Closer3, Closer3N, Closer3p, Exit1, Exit1N, Exit1P, Exit2, Exit2N, Exit2P)
SELECT 
--Datos de la venta
	S.sagu AS GUID,
	dbo.UFN_IM_StringComparer(S.saLiner1,S.saLiner2,S.saCloser1,S.saCloser2,S.saCloser3,S.saExit1,S.saExit2),
	S.saSelfGen,
	S.saD,
	1 Sold,
	CASE WHEN dbo.UFN_IM_GetSaleType(@DateFrom,@DateTo,S.sast,ST.ststc,G.guDepSale,S.saD,S.saProcD,S.saCancelD,G.gusr,S.sasr,S.saByPhone) IN(6,10) THEN 1 ELSE 0 END [Opp],
	S.saID,
	CASE WHEN dbo.UFN_IM_GetSaleType(@DateFrom,@DateTo,S.sast,ST.ststc,G.guDepSale,S.saD,S.saProcD,S.saCancelD,G.gusr,S.sasr,S.saByPhone) NOT IN(5,14) THEN 1 ELSE 0 END [procSales],
	S.saGrossAmount,
	MT.mtGroup,
	-- Locacion
	LO.loN,
--Datos de los vendedores	
	-- Liner 1
	S.saLiner1 as saLiner1,
	SL1.peN as saLiner1N,
	CASE WHEN S.saLiner1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saLiner1) ELSE NULL END [Liner1P],
	-- Liner 2
	S.saLiner2 as saLiner2,
	SL2.peN as saLiner2N,
	CASE WHEN S.saLiner2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saLiner2) ELSE NULL END [Liner2P],
	-- Closer 1
	S.saCloser1 as saCloser1,
	SC1.peN as saCloser1N,
	CASE WHEN S.saCloser1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saCloser1) ELSE NULL END [Closer1P],
	-- Closer 2
	S.saCloser2 as saCloser2,
	SC2.peN as saCloser2N,
	CASE WHEN S.saCloser2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saCloser2) ELSE NULL END [Closer2P],
	-- Closer 3
	S.saCloser3 as saCloser3,
	SC3.peN as saCloser3N,
	CASE WHEN S.saCloser3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saCloser3) ELSE NULL END [Closer3P],
	-- Exit 1
	S.saExit1 as saExit1,
	SE1.peN as saExit1N,
	CASE WHEN S.saExit1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saExit1) ELSE NULL END [Exit1P],
	-- Exit 2
	S.saExit2 as saExit2,
	SE2.peN as saExit2N,
	CASE WHEN S.saExit2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saExit2) ELSE NULL END [Exit2P]
FROM Guests G --para obtener la fecha del show
	LEFT JOIN Sales S on S.sagu = G.guID
	LEFT JOIN SaleTypes ST on ST.stID = S.sast
	left join MembershipTypes MT on MT.mtID = S.samt
	-- Vendedores
	left join Personnel SL1 on SL1.peID = S.saLiner1
	left join Personnel SL2 on SL2.peID = S.saLiner2
	left join Personnel SC1 on SC1.peID = S.saCloser1
	left join Personnel SC2 on SC2.peID = S.saCloser2
	left join Personnel SC3 on SC3.peID = S.saCloser3
	left join Personnel SE1 on SE1.peID = S.saExit1
	left join Personnel SE2 on SE2.peID = S.saExit2		
	-- Segment	
	left join Agencies A on A.agID = G.guag
	left join SegmentsByAgency SA on SA.seID = A.agse
	left join LeadSources LS on LS.lsID = S.sals
	left join Locations LO on Lo.loID = S.salo
	left join LocationsCategories LC on LC.lcID = LO.lolc
	left join SegmentsByLeadSource SL on Sl.soID = LS.lsso
	left join SegmentsCategories SCA on SCA.scID = SA.sesc
	left join SegmentsCategories SCL on SCL.scID = SL.sosc
WHERE
	((--NO SHOWS
	(G.guShowD IS NULL OR NOT G.guShowD BETWEEN @DateFrom AND @DateTo)
	--FECHA DE --- VENTA	-----------------	Venta Procesable	-----------------------
	AND (S.saD BETWEEN @DateFrom AND @DateTo OR S.saProcD BETWEEN @DateFrom AND @DateTo --OR s.saCancelD BETWEEN @DateFrom AND @DateTo
	))
	-- Incluye Bumps, Regen y ventas de otras salas
	OR ((S.sast IN ('BUMP','REGEN') OR G.gusr <> S.sasr) AND S.saD BETWEEN @DateFrom AND @DateTo))
	AND S.saProcD BETWEEN @DateFrom AND @DateTo
	AND (S.saCancel = 0 OR NOT S.saCancelD BETWEEN @DateFrom AND @DateTo)
	AND S.sasr = @SalesRoom
	-- Vendedor
	and (@SalesmanID = 'ALL'
		-- Rol de Liner
		or (@SalesmanID = S.saLiner1 or @SalesmanID = S.saLiner2)
		-- Rol de Closer
		or (@SalesmanID = S.saCloser1 or @SalesmanID = S.saCloser2 or @SalesmanID = S.saCloser3)
		-- Rol de Exit
		or (@SalesmanID = S.saExit1 or @SalesmanID = S.saExit2))
	
ORDER BY S.sagu;
--#endregion


--=================== TABLA Salesman =============================
--#region Salesman Table
DECLARE @Salesman table (
	id int,	
	SalemanID varchar(10),
	SalemanName varchar(40),
	UPS money,
	Sales money,
	Amount money,
	PostID varchar(10),
	Role varchar(50),
	SalemanType varchar(50)	
	);
--#endregion
	
--=================== Insert Salesman =============================
	--#region Insert Salesman

	--#region Liner1
	INSERT into @Salesman (id, SalemanID,SalemanName, PostID, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Liner1 ,Liner1N, Liner1P, 'Liner',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Liner1,Liner2,default,default,default),
	dbo.UFN_IM_GetSalesmanTypesFTB(Liner1,'Liner',sold,Liner2, 'Liner', Liner2P, Closer1,'Closer',Closer1P, Closer2, 'Closer',Closer2p, Closer3, 'Closer',Closer3p, Exit1, 'Exit',Exit1P, Exit2, 'Exit',Exit2P),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Liner1,Liner2,default,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Liner1,Liner2,default,default,default)
	from @Manifest WHERE Liner1 is NOT NULL
	--#endregion
	
	--#region Liner2
	INSERT into @Salesman (id, SalemanID,SalemanName, PostID, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Liner2 ,Liner2N, Liner2P, 'Liner',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Liner1,Liner2,default,default,default),
	dbo.UFN_IM_GetSalesmanTypesFTB(Liner2,'Liner',sold,Liner1, 'Liner', Liner1P, Closer1,'Closer',Closer1P, Closer2, 'Closer',Closer2p, Closer3, 'Closer',Closer3p, Exit1, 'Exit',Exit1P, Exit2, 'Exit',Exit2P),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Liner1,Liner2,default,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Liner1,Liner2,default,default,default)
	from @Manifest WHERE Liner2 is NOT NULL 
	AND Liner2 NOT IN (IsNull(Liner1, ''))
--#endregion

	--#region Closer1
	INSERT into @Salesman (id, SalemanID,SalemanName, PostID, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Closer1 ,Closer1N, Closer1P, 'Closer',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	dbo.UFN_IM_GetSalesmanTypesFTB(Closer1,'Closer',sold,Liner1, 'Liner', Liner1P, Liner2,'Liner',Liner2P, Closer2, 'Closer',Closer2p, Closer3, 'Closer',Closer3p, Exit1, 'Exit',Exit1P, Exit2, 'Exit',Exit2P),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default)
	from @Manifest WHERE Closer1 is NOT NULL AND Closer1P IN ('LINER','FTB') 
	AND Closer1 NOT IN (IsNull(Liner1, ''), IsNull(Liner2, ''))
	--#endregion
	
	--#region Closer2
	INSERT into @Salesman (id, SalemanID,SalemanName, PostID, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Closer2 ,Closer2N, Closer2P, 'Closer',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	dbo.UFN_IM_GetSalesmanTypesFTB(Closer2,'Closer',sold,Liner1, 'Liner', Liner1P, Liner2,'Liner',Liner2P, Closer2, 'Closer',Closer1p, Closer3, 'Closer',Closer3p, Exit1, 'Exit',Exit1P, Exit2, 'Exit',Exit2P),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default)
	from @Manifest WHERE Closer2 is NOT NULL AND Closer2P IN ('LINER','FTB') 
	AND Closer2 NOT IN (IsNull(Liner1, ''), IsNull(Liner2, ''), IsNull(Closer1, ''))
	--#endregion
	
	--#region Closer3
	INSERT into @Salesman (id, SalemanID,SalemanName, PostID, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Closer3 ,Closer3N, Closer3P, 'Closer',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	dbo.UFN_IM_GetSalesmanTypesFTB(Closer3,'Closer',sold,Liner1, 'Liner', Liner1P, Liner2,'Liner',Liner2P, Closer1, 'Closer',Closer1p, Closer2, 'Closer',Closer2p, Exit1, 'Exit',Exit1P, Exit2, 'Exit',Exit2P),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default)
	from @Manifest WHERE Closer3 is NOT NULL AND Closer3P IN ('LINER','FTB') 
	AND Closer3 NOT IN (IsNull(Liner1, ''), IsNull(Liner2, ''), IsNull(Closer1, ''), IsNull(Closer2,''))
	--#endregion
	
	--#region Exit1
	INSERT into @Salesman (id, SalemanID,SalemanName, PostID, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Exit1 ,Exit1N, Exit1P, 'Exit',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default),
	dbo.UFN_IM_GetSalesmanTypesFTB(Exit1,'Exit',sold,Liner1, 'Liner', Liner1P, Liner2,'Liner',Liner2P, Closer1, 'Closer',Closer1p, Closer2, 'Closer',Closer2p, Closer3, 'Closer',Closer3p, Exit2, 'Exit',Exit2P),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default)
	from @Manifest WHERE Exit1 is NOT NULL AND Exit1P IN ('LINER','FTB') 
	AND Exit1 NOT IN (IsNull(Liner1, ''), IsNull(Liner2, ''), IsNull(Closer1, ''), IsNull(Closer2,''),IsNull(Closer3,''))
	--#endregion
	
	--#region Exit2
	INSERT into @Salesman (id, SalemanID,SalemanName, PostID, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Exit2 ,Exit2N, Exit2P, 'Exit',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default),
	dbo.UFN_IM_GetSalesmanTypesFTB(Exit2,'Exit',sold,Liner1, 'Liner', Liner1P, Liner2,'Liner',Liner2P, Closer1, 'Closer',Closer1p, Closer2, 'Closer',Closer2p, Closer3, 'Closer',Closer3p, Exit1, 'Exit',Exit1P),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default)
	from @Manifest WHERE Exit2 is NOT NULL AND Exit2P IN ('LINER','FTB') 
	AND Exit2 NOT IN (IsNull(Liner1, ''), IsNull(Liner2, ''), IsNull(Closer1, ''), IsNull(Closer2,''),IsNull(Closer3,''),IsNull(Exit1,''))
--#endregion
	
	--=================== TABLA StatsByExitCloser =============================
--#region StatsByExitCloser Table
DECLARE @StatsByFTB table (
	SalemanID varchar(10),
	SalemanName varchar(40),
	Locations varchar(30),
	SalemanType varchar(5),
	SalemanTypeN varchar(50),
	PostName varchar(50),
	TeamN varchar(30),
	TeamLeaderN varchar(40),
	SalesmanStatus VARCHAR(10) DEFAULT 'ACTIVE',
	SalesAmount money,
	OPP int,
	UPS money,	
	SalesRegular money,
	SalesExit money,
	Sales money,	
	Efficiency money,
	ClosingFactor money,
	SaleAverage money
	);
--#endregion

	INSERT INTO @StatsByFTB
	SELECT SalemanID, SalemanName, Locations, SalemanType,
	CASE SalemanType WHEN 'AS' THEN 'As A Closer' WHEN 'WITH' THEN 'Front To Back (With Closer)' WHEN 'OWN' THEN 'Front To Back (Own)' END AS SalemanTypeN,
	PostName, 
	(case when @GroupedByTeam = 0 then '' else TeamN end ) as TeamN,
	(case when @GroupedByTeam = 0 then '' else TeamLeaderN end ) as TeamLeaderN,
	(case when @GroupedByTeam = 0 then '' else SalesmanStatus end ) as SalesmanStatus,
	sum(Amount) AS SalesAmount, sum(Opp) AS Opp, sum(UPS) AS UPS,
	SUM(Sales) AS SalesRegular, Sum([Exit]) AS SalesExit, SUM (Sales + [Exit]) as Sales,
	dbo.UFN_OR_SecureDivision( sum(Amount),sum(UPS)) Efficiency,
	dbo.UFN_OR_SecureDivision(SUM (Sales + [Exit]),sum(UPS)) ClosingFactor,
	dbo.UFN_OR_SecureDivision(SUM (Amount),SUM (Sales + [Exit])) SaleAverage
	FROM(
		SELECT DISTINCT m.guID, m.saID, s.SalemanID,s.SalemanName,m.Locations, s.SalemanType,
		CASE WHEN dbo.UFN_IM_IsSelfGen(s.SalemanID,s.Role,m.guSelfGen)=1 THEN 
			CASE WHEN m.guOverflow = 1 THEN 'Overflow' ELSE 'Front To Middle' END
		ELSE ISNULL(CONVERT(varchar(40),PO.poN ),'NO POST')  END AS PostName,
		s.UPS, s.Amount, m.Opp,	
		CASE WHEN m.MembershipGroup='EXIT' THEN 0 ELSE s.Sales END AS Sales,
		CASE WHEN m.MembershipGroup='EXIT' THEN s.Sales ELSE 0 END AS [Exit],
		CASE WHEN t.SalesRoom = @SalesRoom AND ISNULL(CONVERT(varchar(1),tc.tsN),'') <> ISNULL(CONVERT(varchar(1),t.TeamN),'') THEN 'INACTIVE' ELSE 'ACTIVE'  END AS SalesmanStatus,
		CASE WHEN t.SalesRoom <> @SalesRoom THEN 'TEAMS OF OTHER SALES ROOMS' ELSE 
			CASE WHEN t.TeamN IS NULL THEN --si no tiene equipo
				CASE WHEN dbo.UFN_IM_IsSelfGen(s.SalemanID,s.Role,m.guSelfGen)=1 THEN -- si es un Self Gen obtenemos el equipo que asigno la Hostess
				ISNULL(CONVERT(varchar(40),m.TeamSelfGen),'NO TEAM')
				ELSE 'NO TEAM' END
			ELSE t.TeamN END
		END AS TeamN,
		CASE WHEN t.SalesRoom <> @SalesRoom THEN '' ELSE ISNULL(CONVERT(varchar(40),t.TeamLeaderN),'') END AS TeamLeaderN
		FROM @Salesman s	
		INNER JOIN @Manifest m ON m.id = s.id
		INNER JOIN Personnel p on p.peID= s.SalemanID
		LEFT JOIN Posts PO on PO.poID = s.PostID
		left join TeamsSalesmen tc on peTeam = tsID	
		cross apply  dbo.UFN_IM_GetPersonnelTeamSalesmenByDate(s.SalemanID,m.salesmanDate) t		
		WHERE SalemanType IS NOT NULL
		) AS x
		GROUP by SalemanID,SalemanName, Locations, SalemanType, PostName, TeamN, TeamLeaderN, x.SalesmanStatus

IF @IncludeAllSalesmen=1 AND  (SELECT COUNT(*) from @StatsByFTB)>0
BEGIN			
	INSERT INTO @StatsByFTB(SalemanID, SalemanName, SalemanType, TeamN, TeamLeaderN)
	select P.peID, P.peN as SalesmanN, (SELECT top 1 SalemanType from @StatsByFTB) ,
		(case when @GroupedByTeam = 0 then '' else IsNull(tsN, 'NO TEAM') end ) as TeamN,
		(case when @GroupedByTeam = 0 then '' else IsNull(L.peN, '') end ) as TeamLeaderN
	from Personnel P 
	left join TeamsSalesmen on P.peTeamType = 'SA' and P.pePlaceID = tssr and P.peTeam = tsID 
	left join Personnel L on tsLeader = L.peID
	where
		-- Tipo de equipo
		P.peTeamType = 'SA'
		-- Clave del lugar
		and P.pePlaceID = @SalesRoom
		-- Personal activo
		and P.peA = 1		
		--si no se encuentra el vendedor
		AND NOT EXISTS (SELECT * from @StatsByFTB  WHERE SalemanID = p.peID)
END	
		
--===================================================================
--===================  SELECT  ===========================
--===================================================================
	
SELECT DISTINCT
	SalemanID,
	SalemanName,
	PostName,
	Locations,
	(TeamN+'   '+TeamLeaderN) Team,
	SalesmanStatus,
	
	(SUM(OwnUPS) + SUM(WithUPS) + SUM(AsUPS)) UPS,
	SUM(OwnSalesAmount) Own,
	SUM(WithSalesAmount) WithCloser,
	SUM(AsSalesAmount) AsCloser,
	(SUM(OwnSalesAmount) + SUM(WithSalesAmount) + SUM(AsSalesAmount)) Total,
	(SUM(OwnSalesRegular) + SUM(WithSalesRegular) + SUM(AsSalesRegular)) Sales,
	(SUM (OwnSalesExit) + SUM (WithSalesExit) + SUM (AsSalesExit)) 'Exit',
	(SUM(OwnSales) + SUM(WithSales) + SUM(AsSales)) TotalSales,
	dbo.UFN_OR_SecureDivision( (SUM(OwnSalesAmount) + SUM(WithSalesAmount) + SUM(AsSalesAmount)), (SUM(OwnUPS) + SUM(WithUPS) + SUM(AsUPS))) Efficiency,
	dbo.UFN_OR_SecureDivision((SUM(OwnSales) + SUM(WithSales) + SUM(AsSales)), (SUM(OwnUPS) + SUM(WithUPS) + SUM(AsUPS))) ClosingFactor,
	dbo.UFN_OR_SecureDivision((SUM(OwnSalesAmount) + SUM(WithSalesAmount) + SUM(AsSalesAmount)), (SUM(OwnSales) + SUM(WithSales) + SUM(AsSales))) SaleAverage

FROM
(
		SELECT 
			SalemanID, SalemanName, Locations, PostName, TeamN, TeamLeaderN, SalesmanStatus, 
			SUM(SalesAmount) OwnSalesAmount, SUM(OPP) OwnOPP, SUM(UPS) OwnUPS, SUM(SalesRegular) OwnSalesRegular, SUM(SalesExit) OwnSalesExit, SUM(Sales) OwnSales, SUM(Efficiency) OwnEfficiency, SUM(ClosingFactor) OwnClosingFactor, SUM (SaleAverage) OwnSaleAverage, 
			0 WithSalesAmount,0 WithOPP, 0 WithUPS, 0 WithSalesRegular, 0 WithSalesExit, 0 WithSales, 0 WithEfficiency,	0 WithClosingFactor, 0 WithSaleAverage, 
			0 AsSalesAmount, 0 AsOPP, 0 AsUPS, 0 AsSalesRegular, 0 AsSalesExit, 0 AsSales, 0 AsEfficiency,	0 AsClosingFactor, 0 AsSaleAverage 
		 
		FROM @StatsByFTB
		WHERE SalemanType = 'OWN'
		GROUP BY SalemanID,SalemanName, Locations, PostName, TeamN,TeamLeaderN,SalesmanStatus
		
	UNION ALL
		SELECT 
			SalemanID, SalemanName, Locations, PostName, TeamN, TeamLeaderN, SalesmanStatus,
			0 OwnSalesAmount, 0 OwnOPP, 0 OUPS, 0 OwnSalesRegular, 0 OwnSalesExit, 0 OwnSales, 0 OwnEfficiency,	0 OwnClosingFactor, 0 OwnSaleAverage, 
			SUM(SalesAmount) WithSalesAmount, SUM(OPP) WithOPP, SUM(UPS) WithUPS, SUM(SalesRegular) WithSalesRegular, SUM(SalesExit) WithSalesExit, SUM(Sales) WithSales, SUM(Efficiency) WithEfficiency,	SUM(ClosingFactor) WithClosingFactor, SUM (SaleAverage) WithSaleAverage, 
			0 AsSalesAmount, 0 AsOPP, 0 AsUPS, 0 AsSalesRegular, 0 AsSalesExit, 0 AsSales, 0 AsEfficiency,	0 AsClosingFactor, 0 AsSaleAverage
		 
		FROM @StatsByFTB
		WHERE SalemanType = 'WITH'
		GROUP BY SalemanID,SalemanName, Locations,PostName, TeamN,TeamLeaderN,SalesmanStatus 
		
	UNION ALL
		SELECT 
			SalemanID, SalemanName,Locations, PostName, TeamN, TeamLeaderN, SalesmanStatus,
			0 OwnSalesAmount, 0 OwnOPP, 0 OUPS, 0 OwnSalesRegular, 0 OwnSalesExit, 0 OwnSales, 0 OwnEfficiency,	0 OwnClosingFactor, 0 OwnSaleAverage, 
			0 WithSalesAmount, 0 WithOPP, 0 WithUPS, 0 WithSalesRegular, 0 WithSalesExit, 0 WithSales, 0 WithEfficiency,0 WithClosingFactor, 0 WithSaleAverage, 
			SUM(SalesAmount) AsSalesAmount, SUM(OPP) AsOPP, SUM(UPS) AsUPS, SUM(SalesRegular) AsSalesRegular, SUM(SalesExit) AsSalesExit, SUM(Sales) AsSales, SUM(Efficiency) AsEfficiency,	SUM(ClosingFactor) AsClosingFactor, SUM (SaleAverage) AsSaleAverage
		 
		FROM @StatsByFTB
		WHERE SalemanType = 'AS'
		GROUP BY SalemanID,SalemanName, Locations, PostName, TeamN,TeamLeaderN,SalesmanStatus 
) AS LST
GROUP BY SalemanID,SalemanName, Locations, PostName, TeamN,TeamLeaderN,SalesmanStatus
ORDER BY Locations, Total DESC

GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptStatisticsBySegments]    Script Date: 09/22/2016 19:14:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de estadisticas por segmentos (Processor Sales)

**
** [aalcocer] 04/Jul/2016 Created
** [aalcocer] 07/Sep/2016 Modified. Se corrige error de resultados cambiando variables tipo tabla a tablas temporales
**
*/
create procedure [dbo].[USP_IM_RptStatisticsBySegments]
	@DatesFrom varchar(max),				-- Fechas desde
	@DatesTo varchar(max),					-- Fechas hasta
	@SalesRooms varchar(max),				-- Clave de las salas (El primero es la sala principal)
	@SalesmanID varchar(10) = 'ALL',		-- Clave de un vendedor
	@BySegmentsCategories bit = 0,			-- Indica si es por categorias de segmentos
	@Own bit = 0,							--  Indica si trabajo solo
	@IncludeAllSalesmen bit = 0			-- si se desea que esten todos los vendedores de la sala
as
--SET FMTONLY OFF
set nocount on

--=================== TABLA StatsBySegment =============================
--#region StatsBySegment Table
CREATE TABLE #StatsBySegment (
	SalemanID varchar(10),
	SalemanName varchar(40),
	SalemanType varchar(50),
	SegmentN varchar(30),
	TeamN varchar(30),
	TeamLeaderN varchar(40),
	[Status] VARCHAR(10) DEFAULT 'ACTIVE',
	UPS money,
	Sales money,
	Amount money,
	Efficiency money,
	ClosingFactor money
	);
--#endregion

DECLARE @DatesFromTable table(id int identity(1,1),DateFrom DateTime);
DECLARE @DatesToTable table(id int identity(1,1),DateTo DateTime);
DECLARE @SalesRoomsTable table(id int identity(1,1),SalesRoom varchar(10));
DECLARE @DateFrom DateTime, 
		@DateTo DateTime, 
		@SalesRoom varchar(10)

INSERT into @DatesFromTable(DateFrom) SELECT convert(datetime, item) from dbo.Split(@DatesFrom,',')
INSERT into @DatesToTable(DateTo) SELECT convert(datetime, item) from dbo.Split(@DatesTo,',')
INSERT into @SalesRoomsTable(SalesRoom) SELECT item from dbo.Split(@SalesRooms,',')

DECLARE @nSalesRoom int; --numero de salas
SELECT @nSalesRoom = COUNT(*) FROM @SalesRoomsTable sr
	INNER JOIN @DatesFromTable df on sr.id = df.id
	INNER Join @DatesToTable dt ON sr.id= dt.id

WHILE @nSalesRoom <> 0 
BEGIN


--=================== TABLA MANIFEST =============================
--#region ManifestTable
CREATE TABLE #Manifest (
--campos de huespedes
	id int identity(1,1),
	guID int,	
	guShow bit default 0,	
	TeamSelfGen varchar(20),
	guSelfGen bit,
	Segment varchar(10),
	SegmentN varchar(30),
	salesmanDate DateTime,
	-- Campos de la tabla de ventas	
	saID int,
	procSales int,	
	saGrossAmount money default 0,
	-- Vendedores
	-- Liner 1
	Liner1 varchar(10),
	Liner1N varchar(40),
	Liner1P varchar(10),
	-- Liner 2
	Liner2 varchar(10),
	Liner2N varchar(40),
	Liner2P varchar(10),
	-- Closer 1
	Closer1 varchar(10),
	Closer1N varchar(40),
	Closer1P varchar(10),
	-- Closer 2
	Closer2 varchar(10),
	Closer2N varchar(40),
	Closer2p varchar(10),
	-- Closer 3
	Closer3 varchar(10),
	Closer3N varchar(40),
	Closer3p varchar(10),
	-- Exit 1
	Exit1 varchar(10),
	Exit1N varchar(40),
	Exit1P varchar(10),
	-- Exit 2
	Exit2 varchar(10),
	Exit2N varchar(40),
	Exit2P varchar(10));	
--#endregion

SELECT @DateFrom = df.DateFrom, @DateTo = dt.DateTo, @SalesRoom = sr.SalesRoom  
FROM @SalesRoomsTable sr
INNER JOIN @DatesFromTable df on sr.id = df.id
INNER Join @DatesToTable dt ON sr.id= dt.id
WHERE sr.id = @nSalesRoom

--#region Guest
--===================================================================
--=======================   MANIFEST    =============================
--===================================================================
INSERT into #Manifest (guID, guShow, salesmanDate, guSelfGen, TeamSelfGen,
Segment,SegmentN, saID, procSales, Liner1, Liner1N, Liner1P, Liner2, Liner2N, Liner2P, 
Closer1, Closer1N, Closer1P, Closer2, Closer2N, Closer2p, Closer3, Closer3N, Closer3p, Exit1, Exit1N, Exit1P, Exit2, Exit2N, Exit2P)
select DISTINCT
	--Campos de la tabla de huespedes
	G.guID,	
	case WHEN (G.guTour=1 OR g.guWalkOut=1 OR ((G.guCTour=1 OR G.guSaveProgram=1) AND S.saID IS NOT NULL)) THEN 1 ELSE 0 END,
	G.guShowD,
	G.guSelfGen,
	G.guts,
	-- Segmento
	IsNull(case when @BySegmentsCategories = 0 then (case when LS.lspg = 'IH' then A.agse else LS.lsso end)
		else (case when LS.lspg = 'IH' then SA.sesc else SL.sosc end) end, 'NS') as Segment,
	IsNull(case when @BySegmentsCategories = 0 then (case when LS.lspg = 'IH' then SA.seN else SL.soN end)
		else (case when LS.lspg = 'IH' then SCA.scN else SCL.scN end) end, 'NO SEGMENT') as SegmentN,
	S.sagu,
	0 procSales,
	-- PERSONAL DEL SHOW	
	-- Vendedores
	-- Liner 1
	G.guLiner1 as guLiner1,
	GL1.peN as guLiner1N,
	CASE WHEN G.guLiner1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guLiner1) ELSE NULL END [Liner1P],
	-- Liner 2
	G.guLiner2 as guLiner2,
	GL2.peN as guLiner2N,
	CASE WHEN G.guLiner2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guLiner2) ELSE NULL END [Liner2P],
	-- Closer 1
	G.guCloser1 as guCloser1,
	GC1.peN as guCloser1N,
	CASE WHEN G.guCloser1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser1) ELSE NULL END [Closer1P],
	-- Closer 2
	G.guCloser2 as guCloser2,
	GC2.peN as guCloser2N,
	CASE WHEN G.guCloser2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser2) ELSE NULL END [Closer2P],
	-- Closer 3
	G.guCloser3 as guCloser3,
	GC3.peN as guCloser3N,
	CASE WHEN G.guCloser3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser3) ELSE NULL END [Closer3P],
	-- Exit 1
	G.guExit1 as guExit1,
	GE1.peN as guExit1N,
	CASE WHEN G.guExit1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guExit1) ELSE NULL END [Exit1P],
	-- Exit 2
	G.guExit2 as guExit2,
	GE2.peN as guExit2N,
	CASE WHEN G.guExit2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guExit2) ELSE NULL END [Exit2P]
from Guests G
	left join Sales S on sagu = G.guID
	-- PERSONAL DEL SHOW	
	-- Vendedores
	left join Personnel GL1 on GL1.peID = G.guLiner1
	left join Personnel GL2 on GL2.peID = G.guLiner2
	left join Personnel GC1 on GC1.peID = G.guCloser1
	left join Personnel GC2 on GC2.peID = G.guCloser2
	left join Personnel GC3 on GC3.peID = G.guCloser3
	left join Personnel GE1 on GE1.peID = G.guExit1
	left join Personnel GE2 on GE2.peID = G.guExit2		
	--Segment 
	left join Agencies A on A.agID = G.guag
	left join SegmentsByAgency SA on SA.seID = A.agse
	left join LeadSources LS on LS.lsID = G.guls
	left join SegmentsByLeadSource SL on SL.soID = LS.lsso
	left join SegmentsCategories SCA on SCA.scID = SA.sesc
	left join SegmentsCategories SCL on SCL.scID = SL.sosc
where
	-- Fecha de show
	G.guShowD between @DateFrom and @DateTo
	-- Sala de ventas
	and G.gusr = @SalesRoom	
	-- Vendedor
	and (@SalesmanID = 'ALL'		
		-- Rol de Liner
		or (@SalesmanID = G.guLiner1 or @SalesmanID = G.guLiner2 or @SalesmanID = S.saLiner1 or @SalesmanID = S.saLiner2)
		-- Rol de Closer
		or (@SalesmanID = G.guCloser1 or @SalesmanID = G.guCloser2 or @SalesmanID = G.guCloser3 
			or @SalesmanID = S.saCloser1 or @SalesmanID = S.saCloser2 or @SalesmanID = S.saCloser3)
		-- Rol de Exit
		or (@SalesmanID = G.guExit1 or @SalesmanID = G.guExit2 or @SalesmanID = S.saExit1 or @SalesmanID = S.saExit2))
	AND (@Own = 0 OR dbo.UFN_IM_StringComparer(G.guLiner1,G.guLiner2,G.guCloser1,G.guCloser2,G.guCloser3,G.guExit1,G.guExit2) = 1)
order by G.guID;
--#endregion
--=================== Insert SALES =============================
--#region insertSales

DECLARE @nReg int; -- numero de regitros
SELECT @nReg = COUNT(*) FROM #Manifest
DECLARE @cont int; -- almacena la cantidad de veces que se recorre el while
SET @cont = 1;
DECLARE @gu int; -- si tiene ventas
WHILE @nReg >= @cont 
BEGIN
	SELECT @gu = guid FROM #Manifest WHERE id = @cont --obtiene el id del gest	
	DECLARE @sale int;
	SELECT TOP 1 @sale = sagu FROM Sales WHERE sagu = @gu
	--#region IF @sale

	IF @sale IS NOT NULL		
	BEGIN		
		SELECT 
		--Datos de la venta
			S.sagu AS GUID,		
			-- Segmento
			IsNull(case when @BySegmentsCategories = 0 then (case when LS.lspg = 'IH' then A.agse else LS.lsso end)
			else (case when LS.lspg = 'IH' then SA.sesc else SL.sosc end) end, 'NS') as Segment,
			IsNull(case when @BySegmentsCategories = 0 then (case when LS.lspg = 'IH' then SA.seN else SL.soN end)
			else (case when LS.lspg = 'IH' then SCA.scN else SCL.scN end) end, 'NO SEGMENT') as SegmentN,
			S.saID,
			S.saD,	
			S.saGrossAmount,
		--Datos de los vendedores			
			-- Liner 1
			S.saLiner1 as saLiner1,
			SL1.peN as saLiner1N,
			CASE WHEN S.saLiner1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saLiner1) ELSE NULL END [Liner1P],
			-- Liner 2
			S.saLiner2 as saLiner2,
			SL2.peN as saLiner2N,
			CASE WHEN S.saLiner2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saLiner2) ELSE NULL END [Liner2P],
			-- Closer 1
			S.saCloser1 as saCloser1,
			SC1.peN as saCloser1N,
			CASE WHEN S.saCloser1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saCloser1) ELSE NULL END [Closer1P],
			-- Closer 2
			S.saCloser2 as saCloser2,
			SC2.peN as saCloser2N,
			CASE WHEN S.saCloser2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saCloser2) ELSE NULL END [Closer2P],
			-- Closer 3
			S.saCloser3 as saCloser3,
			SC3.peN as saCloser3N,
			CASE WHEN S.saCloser3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saCloser3) ELSE NULL END [Closer3P],
			-- Exit 1
			S.saExit1 as saExit1,
			SE1.peN as saExit1N,
			CASE WHEN S.saExit1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saExit1) ELSE NULL END [Exit1P],
			-- Exit 2
			S.saExit2 as saExit2,
			SE2.peN as saExit2N,
			CASE WHEN S.saExit2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,S.saExit2) ELSE NULL END [Exit2P],			
			CASE WHEN ST.ststc = 'DG' THEN 0 ELSE 1 END procSales
		INTO #tbTemp
		FROM Guests G
			LEFT JOIN Sales S on S.sagu = G.guID
			LEFT JOIN SaleTypes ST on ST.stID = S.sast		
			-- Vendedores
			left join Personnel SL1 on SL1.peID = S.saLiner1
			left join Personnel SL2 on SL2.peID = S.saLiner2
			left join Personnel SC1 on SC1.peID = S.saCloser1
			left join Personnel SC2 on SC2.peID = S.saCloser2
			left join Personnel SC3 on SC3.peID = S.saCloser3
			left join Personnel SE1 on SE1.peID = S.saExit1
			left join Personnel SE2 on SE2.peID = S.saExit2	
			-- Segment	
			left join Agencies A on A.agID = G.guag		
			left join SegmentsByAgency SA on SA.seID = A.agse
			left join LeadSources LS on LS.lsID = G.guls
			left join SegmentsByLeadSource SL on SL.soID = LS.lsso
			left join SegmentsCategories SCA on SCA.scID = SA.sesc
			left join SegmentsCategories SCL on SCL.scID = SL.sosc
		WHERE
			S.saD BETWEEN @DateFrom and @DateTo AND sagu = @gu
			AND S.sast NOT IN ('BUMP','REGEN')
			AND S.saProcD BETWEEN @DateFrom AND @DateTo
			AND (S.saCancel = 0 OR NOT S.saCancelD BETWEEN @DateFrom AND @DateTo)
			AND S.sasr = @SalesRoom	
			AND (@Own = 0 OR dbo.UFN_IM_StringComparer(S.saLiner1,S.saLiner2,S.saCloser1,S.saCloser2,S.saCloser3,S.saExit1,S.saExit2) = 1)	
		INSERT INTO #Manifest (guID,Segment,SegmentN, saID,salesmanDate,saGrossAmount,
		Liner1,Liner1N,Liner1P,Liner2,Liner2N,Liner2P,Closer1,Closer1N,Closer1P,Closer2,Closer2N,Closer2p,Closer3,Closer3N,Closer3p,
		Exit1,Exit1N,Exit1P,Exit2,Exit2N,Exit2P,procSales)
		SELECT * FROM #tbTemp;
		DROP TABLE #tbTemp;
	END		
--#endregion
	SET @cont = @cont +1;
END;
--#endregion

--===================================================================
--===================  DEPOSIT SALES    =============================
--===================================================================
--#region Deposit Sales
INSERT into #Manifest (guID, salesmanDate, guSelfGen, TeamSelfGen, 
Segment, SegmentN, procSales, Liner1, Liner1N, Liner1P, Liner2, Liner2N, Liner2P, 
Closer1, Closer1N, Closer1P, Closer2, Closer2N, Closer2p, Closer3, Closer3N, Closer3p, Exit1, Exit1N, Exit1P, Exit2, Exit2N, Exit2P)
select 
	--#region Campos de la tabla de huespedes
	G.guID,	
	G.guShowD,
	G.guSelfGen,
	G.guts,
	-- Segmento
	IsNull(case when @BySegmentsCategories = 0 then (case when LS.lspg = 'IH' then A.agse else LS.lsso end)
		else (case when LS.lspg = 'IH' then SA.sesc else SL.sosc end) end, 'NS') as Segment,
	IsNull(case when @BySegmentsCategories = 0 then (case when LS.lspg = 'IH' then SA.seN else SL.soN end)
		else (case when LS.lspg = 'IH' then SCA.scN else SCL.scN end) end, 'NO SEGMENT') as SegmentN,
	0 procSales,
	--#endregion	
	--#region PERSONAL DEL SHOW	
	-- Vendedores
	-- Liner 1
	G.guLiner1 as guLiner1,
	GL1.peN as guLiner1N,
	CASE WHEN G.guLiner1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guLiner1) ELSE NULL END [Liner1P],
	-- Liner 2
	G.guLiner2 as guLiner2,
	GL2.peN as guLiner2N,
	CASE WHEN G.guLiner2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guLiner2) ELSE NULL END [Liner2P],
	-- Closer 1
	G.guCloser1 as guCloser1,
	GC1.peN as guCloser1N,
	CASE WHEN G.guCloser1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser1) ELSE NULL END [Closer1P],
	-- Closer 2
	G.guCloser2 as guCloser2,
	GC2.peN as guCloser2N,
	CASE WHEN G.guCloser2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser2) ELSE NULL END [Closer2P],
	-- Closer 3
	G.guCloser3 as guCloser3,
	GC3.peN as guCloser3N,
	CASE WHEN G.guCloser3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guCloser3) ELSE NULL END [Closer3P],
	-- Exit 1
	G.guExit1 as guExit1,
	GE1.peN as guExit1N,
	CASE WHEN G.guExit1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guExit1) ELSE NULL END [Exit1P],
	-- Exit 2
	G.guExit2 as guExit2,
	GE2.peN as guExit2N,
	CASE WHEN G.guExit2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(G.guShowD,G.guExit2) ELSE NULL END [Exit2P]
	--#endregion
from Guests G	
	left join Sales S on S.sagu = G.guID	
--#region PERSONAL DEL SHOW	
	-- Vendedores
	left join Personnel GL1 on GL1.peID = G.guLiner1
	left join Personnel GL2 on GL2.peID = G.guLiner2
	left join Personnel GC1 on GC1.peID = G.guCloser1
	left join Personnel GC2 on GC2.peID = G.guCloser2
	left join Personnel GC3 on GC3.peID = G.guCloser3
	left join Personnel GE1 on GE1.peID = G.guExit1
	left join Personnel GE2 on GE2.peID = G.guExit2	
	--#endregion	
	-- Segment
	left join Agencies A on A.agID = G.guag
	left join SegmentsByAgency SA on SA.seID = A.agse
	left join LeadSources LS on LS.lsID = G.guls
	left join SegmentsByLeadSource SL on SL.soID = LS.lsso
	left join SegmentsCategories SCA on SCA.scID = SA.sesc
	left join SegmentsCategories SCL on SCL.scID = SL.sosc
where
	G.guDepositSaleD between @DateFrom and @DateTo
	-- Fecha de show diferente de la fecha de venta deposito
	and G.guShowD <> G.guDepositSaleD
	-- Sala de ventas
	and G.gusr = @SalesRoom
	-- Vendedor 
	and (@SalesmanID = 'ALL'		
		-- Rol de Liner
		or (@SalesmanID = G.guLiner1 or @SalesmanID = G.guLiner2 or @SalesmanID = S.saLiner1 or @SalesmanID = S.saLiner2)
		-- Rol de Closer
		or (@SalesmanID = G.guCloser1 or @SalesmanID = G.guCloser2 or @SalesmanID = G.guCloser3
			or @SalesmanID = S.saCloser1 or @SalesmanID = S.saCloser2 or @SalesmanID = S.saCloser3)
		-- Rol de Exit
		or (@SalesmanID = G.guExit1 or @SalesmanID = G.guExit2 or @SalesmanID = S.saExit1 or @SalesmanID = S.saExit2))
	AND (@Own = 0 OR dbo.UFN_IM_StringComparer(G.guLiner1,G.guLiner2,G.guCloser1,G.guCloser2,G.guCloser3,G.guExit1,G.guExit2) = 1)
order by G.guID;
--#endregion
--===================================================================
--======================  OTHER SALES    ============================
--===================================================================
--#region Other Sales
INSERT into #Manifest (guID, guSelfGen, salesmanDate, Segment, SegmentN, saID, procSales, saGrossAmount, 
Liner1, Liner1N, Liner1P, Liner2, Liner2N, Liner2P, Closer1, Closer1N, Closer1P, Closer2, Closer2N, 
Closer2p, Closer3, Closer3N, Closer3p, Exit1, Exit1N, Exit1P, Exit2, Exit2N, Exit2P)
SELECT 
--Datos de la venta
	S.sagu AS GUID,
	S.saSelfGen,
	S.saD,
	-- Segmento
	IsNull(case when @BySegmentsCategories = 0 then (case when LS.lspg = 'IH' then A.agse else LS.lsso end)
		else (case when LS.lspg = 'IH' then SA.sesc else SL.sosc end) end, 'NS') as Segment,
	IsNull(case when @BySegmentsCategories = 0 then (case when LS.lspg = 'IH' then SA.seN else SL.soN end)
		else (case when LS.lspg = 'IH' then SCA.scN else SCL.scN end) end, 'NO SEGMENT') as SegmentN,
	S.saID,
	CASE WHEN dbo.UFN_IM_GetSaleType(@DateFrom,@DateTo,S.sast,ST.ststc,G.guDepSale,S.saD,S.saProcD,S.saCancelD,G.gusr,S.sasr,S.saByPhone) NOT IN(5,14) THEN 1 ELSE 0 END [procSales],
	S.saGrossAmount,
--Datos de los vendedores	
	-- Liner 1
	S.saLiner1 as saLiner1,
	SL1.peN as saLiner1N,
	CASE WHEN S.saLiner1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saLiner1) ELSE NULL END [Liner1P],
	-- Liner 2
	S.saLiner2 as saLiner2,
	SL2.peN as saLiner2N,
	CASE WHEN S.saLiner2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saLiner2) ELSE NULL END [Liner2P],
	-- Closer 1
	S.saCloser1 as saCloser1,
	SC1.peN as saCloser1N,
	CASE WHEN S.saCloser1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saCloser1) ELSE NULL END [Closer1P],
	-- Closer 2
	S.saCloser2 as saCloser2,
	SC2.peN as saCloser2N,
	CASE WHEN S.saCloser2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saCloser2) ELSE NULL END [Closer2P],
	-- Closer 3
	S.saCloser3 as saCloser3,
	SC3.peN as saCloser3N,
	CASE WHEN S.saCloser3 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saCloser3) ELSE NULL END [Closer3P],
	-- Exit 1
	S.saExit1 as saExit1,
	SE1.peN as saExit1N,
	CASE WHEN S.saExit1 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saExit1) ELSE NULL END [Exit1P],
	-- Exit 2
	S.saExit2 as saExit2,
	SE2.peN as saExit2N,
	CASE WHEN S.saExit2 IS NOT NULL THEN dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saExit2) ELSE NULL END [Exit2P]
FROM Guests G --para obtener la fecha del show
	LEFT JOIN Sales S on S.sagu = G.guID
	LEFT JOIN SaleTypes ST on ST.stID = S.sast	
	-- Vendedores
	left join Personnel SL1 on SL1.peID = S.saLiner1
	left join Personnel SL2 on SL2.peID = S.saLiner2
	left join Personnel SC1 on SC1.peID = S.saCloser1
	left join Personnel SC2 on SC2.peID = S.saCloser2
	left join Personnel SC3 on SC3.peID = S.saCloser3
	left join Personnel SE1 on SE1.peID = S.saExit1
	left join Personnel SE2 on SE2.peID = S.saExit2		
	-- Segment	
	left join Agencies A on A.agID = G.guag
	left join SegmentsByAgency SA on SA.seID = A.agse
	left join LeadSources LS on LS.lsID = S.sals
	left join SegmentsByLeadSource SL on Sl.soID = LS.lsso
	left join SegmentsCategories SCA on SCA.scID = SA.sesc
	left join SegmentsCategories SCL on SCL.scID = SL.sosc
WHERE
	((--NO SHOWS
	(G.guShowD IS NULL OR NOT G.guShowD BETWEEN @DateFrom AND @DateTo)
	--FECHA DE --- VENTA	-----------------	Venta Procesable	-----------------------
	AND (S.saD BETWEEN @DateFrom AND @DateTo OR S.saProcD BETWEEN @DateFrom AND @DateTo --OR s.saCancelD BETWEEN @DateFrom AND @DateTo
	))
	-- Incluye Bumps, Regen y ventas de otras salas
	OR ((S.sast IN ('BUMP','REGEN') OR G.gusr <> S.sasr) AND S.saD BETWEEN @DateFrom AND @DateTo))
	AND S.saProcD BETWEEN @DateFrom AND @DateTo
	AND (S.saCancel = 0 OR NOT S.saCancelD BETWEEN @DateFrom AND @DateTo)
	AND S.sasr = @SalesRoom
	-- Vendedor
	and (@SalesmanID = 'ALL'
		-- Rol de Liner
		or (@SalesmanID = S.saLiner1 or @SalesmanID = S.saLiner2)
		-- Rol de Closer
		or (@SalesmanID = S.saCloser1 or @SalesmanID = S.saCloser2 or @SalesmanID = S.saCloser3)
		-- Rol de Exit
		or (@SalesmanID = S.saExit1 or @SalesmanID = S.saExit2))	
	AND (@Own = 0 OR dbo.UFN_IM_StringComparer(S.saLiner1,S.saLiner2,S.saCloser1,S.saCloser2,S.saCloser3,S.saExit1,S.saExit2) = 1)
ORDER BY S.sagu;
--#endregion



--=================== TABLA Salesman =============================
--#region Salesman Table
CREATE TABLE #Salesman (
	id int,	
	SalemanID varchar(10),
	SalemanName varchar(40),
	UPS money,
	Sales money,
	Amount money,
	Role varchar(50),
	SalemanType varchar(50)
	);
--#endregion
	--=================== Insert Salesman =============================
	--#region Insert Salesman

	--#region Liner1
INSERT into #Salesman (id, SalemanID,SalemanName, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Liner1 ,Liner1N, 'Liner',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Liner1,Liner2,default,default,default),
	dbo.UFN_IM_GetSalesmanTypeBySegments(Liner1,Liner1P,'Liner',guSelfGen),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Liner1,Liner2,default,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Liner1,Liner2,default,default,default)
	from #Manifest WHERE Liner1 is NOT NULL
--#endregion
	
	--#region Liner2
INSERT into #Salesman (id, SalemanID,SalemanName, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Liner2 ,Liner2N, 'Liner',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Liner1,Liner2,default,default,default),
	dbo.UFN_IM_GetSalesmanTypeBySegments(Liner2,Liner2P,'Liner',guSelfGen),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Liner1,Liner2,default,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Liner1,Liner2,default,default,default)
	from #Manifest m WHERE Liner2 is NOT NULL AND NOT EXISTS (SELECT * from #Salesman s WHERE s.id=m.id AND s.SalemanID =m.Liner2)
--#endregion
	
	--#region Closer1
	INSERT into #Salesman (id, SalemanID,SalemanName, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Closer1 ,Closer1N, 'Closer',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	dbo.UFN_IM_GetSalesmanTypeBySegments(Closer1,Closer1P,'Closer',guSelfGen),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default)
	from #Manifest m WHERE Closer1 is NOT NULL AND NOT EXISTS (SELECT * from #Salesman s WHERE s.id=m.id AND s.SalemanID =m.Closer1);
	--#endregion
	
	--#region Closer2
	INSERT into #Salesman (id, SalemanID,SalemanName, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Closer2 ,Closer2N, 'Closer',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	dbo.UFN_IM_GetSalesmanTypeBySegments(Closer2,Closer2P,'Closer',guSelfGen),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default)
	from #Manifest m WHERE Closer2 is NOT NULL AND NOT EXISTS (SELECT * from #Salesman s WHERE s.id=m.id AND s.SalemanID =m.Closer2)
	--#endregion
	
	--#region Closer3
	INSERT into #Salesman (id, SalemanID,SalemanName, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Closer3 ,Closer3N, 'Closer',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	dbo.UFN_IM_GetSalesmanTypeBySegments(Closer3,Closer3P,'Closer',guSelfGen),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Closer1,Closer2,Closer3,default,default)
	from #Manifest m WHERE Closer3 is NOT NULL AND NOT EXISTS (SELECT * from #Salesman s WHERE s.id=m.id AND s.SalemanID =m.Closer3)	
	--#endregion
	
	--#region Exit1
	INSERT into #Salesman (id, SalemanID,SalemanName, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Exit1 ,Exit1N,  'Exit',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default),
	dbo.UFN_IM_GetSalesmanTypeBySegments(Exit1,Exit1P,'Exit',guSelfGen),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default)
	from #Manifest m WHERE Exit1 is NOT NULL AND NOT EXISTS (SELECT * from #Salesman s WHERE s.id=m.id AND s.SalemanID =m.Exit1)	
	--#endregion
	
	--#region Exit2
	INSERT into #Salesman (id, SalemanID,SalemanName, Role, UPS, SalemanType, Sales, Amount)
	SELECT DISTINCT id, Exit2 ,Exit2N, 'Exit',
	guShow * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default),
	dbo.UFN_IM_GetSalesmanTypeBySegments(Exit2,Exit2P,'Exit',guSelfGen),
	procSales * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default),
	saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(Exit1,Exit2,DEFAULT,default,default)
	from #Manifest m WHERE Exit2 is NOT NULL AND NOT EXISTS (SELECT * from #Salesman s WHERE s.id=m.id AND s.SalemanID =m.Exit2)	
--#endregion


	INSERT INTO #StatsBySegment
	SELECT SalemanID, SalemanName, SalemanType, SegmentN , 
	TeamN, TeamLeaderN, [Status],
	sum(UPS) AS UPS ,SUM(Sales) AS Sales, sum(Amount) AS Amount,
	dbo.UFN_OR_SecureDivision( sum(Amount),sum(UPS)) Efficiency,
	dbo.UFN_OR_SecureDivision(SUM(Sales),sum(UPS)) ClosingFactor
	FROM(
		SELECT DISTINCT s.*,m.SegmentN,	
		CASE WHEN t.SalesRoom = @SalesRoom AND ISNULL(CONVERT(varchar(1),tc.tsN),'') <> ISNULL(CONVERT(varchar(1),t.TeamN),'') THEN 'INACTIVE' ELSE 'ACTIVE'  END AS [Status],
		CASE WHEN t.SalesRoom <> @SalesRoom THEN 'TEAMS OF OTHER SALES ROOMS' ELSE 
			CASE WHEN t.TeamN IS NULL THEN --si no tiene equipo
				CASE WHEN dbo.UFN_IM_IsSelfGen(s.SalemanID,s.Role,m.guSelfGen)=1 THEN -- si es un Self Gen obtenemos el equipo que asigno la Hostess
				ISNULL(CONVERT(varchar(40),m.TeamSelfGen),'NO TEAM')
				ELSE 'NO TEAM' END
			ELSE t.TeamN END
		END AS TeamN,
		CASE WHEN t.SalesRoom <> @SalesRoom THEN '' ELSE ISNULL(CONVERT(varchar(40),t.TeamLeaderN),'') END AS TeamLeaderN
		FROM #Salesman s	
		INNER JOIN #Manifest m ON m.id = s.id
		INNER JOIN Personnel p on p.peID= s.SalemanID
		left join TeamsSalesmen tc on peTeam = tsID	
		cross apply  dbo.UFN_IM_GetPersonnelTeamSalesmenByDate(s.SalemanID,m.salesmanDate) t
		) AS x
		GROUP by SalemanID,SalemanName,SalemanType,SegmentN, TeamN, TeamLeaderN, [Status]

SET @nSalesRoom = @nSalesRoom - 1
DROP TABLE #Manifest
DROP TABLE #Salesman
	
END


IF @IncludeAllSalesmen=1 AND  (SELECT COUNT(*) from #StatsBySegment)>0
BEGIN			
	INSERT INTO #StatsBySegment(SalemanID, SalemanName, SalemanType, SegmentN, TeamN, TeamLeaderN)
	select P.peID, P.peN as SalesmanN, poN , (SELECT top 1 SegmentN from #StatsBySegment), IsNull(tsN, 'NO TEAM'), IsNull(L.peN, '')
	from Personnel P 
	left join Posts on P.pepo = poID 
	left join TeamsSalesmen on P.peTeamType = 'SA' and P.pePlaceID = tssr and P.peTeam = tsID 
	left join Personnel L on tsLeader = L.peID
	where
		-- Tipo de equipo
		P.peTeamType = 'SA'
		-- Clave del lugar
		and P.pePlaceID = @SalesRoom
		-- Personal activo
		and P.peA = 1
		--si no se encuentra el vendedor
		AND NOT EXISTS (SELECT * from #StatsBySegment  WHERE SalemanID = p.peID)
END	
		
--===================================================================
--===================  SELECT  ===========================
--===================================================================
SELECT * from #StatsBySegment
		


GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptWeeklyGiftsItemsSimple]    Script Date: 09/22/2016 19:14:40 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_IM_RptWeeklyMonthlyHostess]    Script Date: 09/22/2016 19:14:42 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_OR_GetCxC]    Script Date: 09/22/2016 19:14:45 ******/
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
	R.grAmountToPay,
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

/****** Object:  StoredProcedure [dbo].[USP_OR_RptCxC]    Script Date: 09/22/2016 19:14:46 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_OR_RptCxCExcel]    Script Date: 09/22/2016 19:14:48 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_OR_RptDailySalesDetail]    Script Date: 09/22/2016 19:14:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el detalle del reporte de ventas diarias
** 
** [wtorres]	06/Nov/2008 Creado
** [wtorres]	07/Abr/2009 Dividi el reporte en encabezado y detalle
** [wtorres]	16/Nov/2013 Optimizado.
**							- Reemplace el uso de la funcion UFN_OR_ObtenerShowsReales por UFN_OR_GetShows
**							- Reemplace el uso de las funciones UFN_OR_ObtenerVentasPorTipoMembresia, UFN_OR_ObtenerOutOfPending
**							  y UFN_OR_ObtenerVentasCanceladas por UFN_OR_GetSales y UFN_OR_GetSalesAmount
** [LorMartinez] 18/Ene/2016, Modificado, Se agrega manejo de mas de un saleroom
**
*/
CREATE procedure [dbo].[USP_OR_RptDailySalesDetail]
	@DateFrom datetime,		-- Fecha inicial
	@DateTo	datetime,		-- Fecha final
	@SalesRoom varchar(MAX)= 'ALL'	-- Clave de la sala
as
set nocount on

-- Esta variable contendra la tabla de detalle
declare @TableDetail table(
	[Date] datetime,
	Shows int,
	SalesRegular int,
	SalesExit int,
	SalesVIP int,
	SalesAmount money,
	SalesAmountOOP money,
	SalesAmountCancel money,
  DownPact money,
  DownColl money,
  CnxSalesAmount money
)

declare @Date datetime

set @Date = @DateFrom

--Almacena el resultado de los colectados
declare @TableSale table(
	MembershipNum varchar(10),
  SaleRoom varchar(10),
  Down money,
  Down_Coll money
)

-- mientras haya mas fechas
while @Date <= @DateTo
begin
	
	-- agregamos el dia a la tabla detalle
	insert into @TableDetail--([Date],Shows,SalesRegular,SalesExit,SalesVIP,SalesAmount,SalesAmountOOP,SalesAmountCancel)
	values(
		@Date,
		-- Shows
		dbo.UFN_OR_GetShows(@Date, @Date, default, @SalesRoom, default, 4, default, default),
		-- Numero de ventas regulares
		dbo.UFN_OR_GetSales(@Date, @Date, default, @SalesRoom, 'REG', default, default, default),
		-- Numero de ventas exit
		dbo.UFN_OR_GetSales(@Date, @Date, default, @SalesRoom, 'EXIT', default, default, default),
		-- Numero de ventas VIP
		dbo.UFN_OR_GetSales(@Date, @Date, default, @SalesRoom, 'VIP,DIAMOND', default, default, default),
		-- Monto de ventas
		dbo.UFN_OR_GetSalesAmount(@Date, @Date, default, @SalesRoom, default, default, default, default),
		-- Monto de ventas Out Of Pending
		dbo.UFN_OR_GetSalesAmount(@Date, @Date, default, @SalesRoom, default, 1, default, default),
		-- Monto de ventas canceladas
		dbo.UFN_OR_GetSalesAmount(@Date, @Date, default, @SalesRoom, default, default, 1, default),
    --DownPayment
    dbo.UFN_OR_GetSalesDownPayment(@Date,@Date,@SalesRoom,0),
    --DownPayment Collected
    dbo.UFN_OR_GetSalesDownPayment(@Date,@Date,@SalesRoom,1),
    --Ventas canceladas
    dbo.UFN_OR_GetCnxSalesAmount(@Date,@Date,@SalesRoom)
	)
	/*
  INSERT INTO @TableSale
  exec USP_OR_ObtenerMembresiasPorSalaFecha @Date, @Date, @SalesRoom
  
  UPDATE @TableDetail
  SET downpact = (select sum(down) from @tablesale ),
      DownColl = (select sum(Down_Coll) from @tablesale),
      SHOWS = isnull(shows,0)
  WHERE [Date] = @Date
  
  delete @TableSale
  */  
	-- aumentamos la fecha temporal
	set @Date = DateAdd(day, 1, @Date)
end

-- devolvemos la tabla detalle
select [Date] ,
	     isnull(Shows,0) [Shows] ,
	     SalesRegular ,
	     SalesExit ,
	     SalesVIP ,
    	SalesAmount ,
    	SalesAmountOOP ,
    	SalesAmountCancel ,
      DownPact ,
      DownColl,
      CnxSalesAmount
from @TableDetail

GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptDailySalesHeader]    Script Date: 09/22/2016 19:14:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el encabezado del reporte de ventas diarias
** 
** [wtorres]	06/Nov/2008 Creado
** [wtorres]	07/Abr/2009 Dividi el reporte en encabezado y detalle
** [wtorres]	16/Nov/2013 Optimizado.
**							- Reemplace el uso de la funcion UFN_OR_ObtenerShowsReales por UFN_OR_GetShows
**							- Reemplace el uso de las funciones UFN_OR_ObtenerVentasPorTipoMembresia por UFN_OR_GetSales y UFN_OR_GetSalesAmount
** [LorMartinez] 18/Ene/2016 Modificado, Se aumenta el tamaño del campo de Salesroom ahora obtendra mas de un saleroom
*/
CREATE procedure [dbo].[USP_OR_RptDailySalesHeader]
	@DateFrom datetime,		-- Fecha inicial
	@DateTo	datetime,		-- Fecha final
	@SalesRoom varchar(MAX) ='ALL'	-- Clave de la sala
as
set nocount on

declare @DateFromPrevious datetime,
	@DateToPrevious datetime

set @DateFromPrevious = DateAdd(year, -1, @DateFrom)
set @DateToPrevious = DateAdd(year, -1, @DateTo)

-- devolvemos la tabla encabezado
select
	-- Shows
	dbo.UFN_OR_GetShows(@DateFrom, @DateTo, default, @SalesRoom, default, 4, default, default) as Shows,
	-- Monto de ventas
	dbo.UFN_OR_GetSalesAmount(@DateFrom, @DateTo, default, @SalesRoom, default, default, default, default) as SalesAmount,
	-- Shows del periodo anterior
	dbo.UFN_OR_GetShows(@DateFromPrevious, @DateToPrevious, default, @SalesRoom, default, 4, default, default) as ShowsPrevious,
	-- Monto de ventas del período anterior
	dbo.UFN_OR_GetSalesAmount(@DateFromPrevious, @DateToPrevious, default, @SalesRoom, default, default, default, default) as SalesAmountPrevious

GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptFoliosInvitationByDateFolio]    Script Date: 09/22/2016 19:14:55 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_OR_RptGifts]    Script Date: 09/22/2016 19:14:58 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_OR_RptManifestByLS]    Script Date: 09/22/2016 19:14:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de manifiesto (Host)
**		1. Bookings
**		2. Manifiesto
**		3. Deposit Sales
**		4. Ventas de otros dias, Be Backs, OOP, Cancellations, Regens, Deposit Before, etc.
**
** [wtorres]		05/Ene/2009 Modified. Depuracion
** [wtorres]		11/May/2009 Modified. Modifique la consulta de bookings:
**								1. Devolver el nombre de la locacion (loN)
**								2. Elimine los campos guDeposit, loFlyers
** [wtorres]		18/Jun/2010 Modified. Agregue el campo saProcRD
** [wtorres]		20/Jul/2011 Modified. Ahora no se traen los bookings cancelados
** [wtorres]		08/Ago/2011 Modified. Agregue los campos de programa de show
** [wtorres]		10/Ago/2011 Modified. Elimine los campos de calificado y no calificado y agregue el campo de programa de rescate
** [wtorres]		12/Oct/2011 Modified. Elimine el campo de categoria de programa de show y fusione los In & outs en Regular Tours
** [wtorres]		11/Ene/2012 Modified. Agregue el campo descripcion de la agencia
** [wtorres]		28/Ene/2012 Modified. Agregue el campo descripcion del pais
** [wtorres]		03/Jun/2013 Modified. Agregue los campos de porcentaje de enganche pactado y pagado
** [wtorres]		16/Nov/2013 Modified. Agregue el campo de categoria de tipo de venta
** [wtorres]		22/Feb/2014 Modified. Agregue el campo de Lead Source
** [LorMartinez]	21/Ene/2016 Modified. Se agrega validacion para GuestAdicionales
** [lchairez]		27/Abr/2016 Modified. Se agrega el campo saOriginalAmount al Select de ventas.
** [wtorres]		16/Jun/2016 Modified. Ahora se valida que no traiga las ventas que no son de la fecha indicada
**
*/
CREATE procedure [dbo].[USP_OR_RptManifestByLS]
	@Date Datetime,			-- Fecha
	@SalesRoom varchar(10)	-- Clave de la sala
as
set nocount on

-- ================================================
-- 					BOOKINGS
-- ================================================
select 
	case when G.guDeposit = 0 and L.loFlyers = 1 then G.guloInvit + 'F' else G.guloInvit end as guloInvit,
	case when G.guDeposit = 0 and L.loFlyers = 1 then loN + ' FLYERS' else loN end as LocationN,
	G.guBookT
from Guests G
	inner join Locations L on G.guloInvit = L.loID
where
	-- Fecha de booking
	G.guBookD = @Date
	-- Sala de ventas
	and G.gusr = @SalesRoom
	-- No directas
	and G.guDirect = 0
	-- No reschedules
	and (G.guReschD is null or G.guReschD <> @Date)
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- No bookings cancelados
	and G.guBookCanc = 0
order by G.guloInvit, G.guDeposit, G.guBookT

/*Se calculan los Additionals*/
DECLARE @AddGuest table(gagu integer,
                        gaShow datetime,
                        gaLoc varchar(50),
                        gaFlyers varchar(50),
                        gaLoN varchar(50)
                        )


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
WHERE g2.gusr= @SalesRoom
AND g.gusr = @SalesRoom
AND g2.guShowD= @Date

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
		G.guID,
		G.gusr,
		G.guls,
		G.guShowSeq,
		isnull(G.guloInvit,gadd.gaLoc) as guloInvit,
		isnull(L.loN,gadd.gaLoN) as loN,
		isnull(L.loFlyers,gadd.gaFlyers) as loFlyers,    
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
		G.guPRInvit1, 
		P1.peN as guPR1N,
		G.guPRInvit2, 
		P2.peN as guPR2N,
		G.guPRInvit3, 
		P3.peN as guPR3N,
		G.guLiner1, 
		L1.peN as guLiner1N,
		G.guLiner2, 
		L2.peN as guLiner2N,
		G.guCloser1, 
		C1.peN as guCloser1N,
		G.guCloser2, 
		C2.peN as guCloser2N,
		G.guCloser3,
		C3.peN as guCloser3N,
		G.guExit1, 
		E1.peN as guExit1N,
		G.guExit2, 
		E2.peN as guExit2N,
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
		S.saDownPaymentPaidPercentage / 100 as saDownPaymentPaidPercentage,
		dbo.UFN_OR_GetShowProgram(G.guSaveProgram, G.guCTour, 0, SR.srAppointment) as ShowProgram,
    gadd.gashow  
	from Guests G
		left join Sales S on G.guID = S.sagu AND S.sasr = @SalesRoom AND (S.saD = @Date OR S.saProcD = @Date OR S.saCancelD = @Date)
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
    LEFT JOIN @AddGuest gadd ON gadd.gagu= G.guID    
	where
		-- Fecha de show
  	ISNULL(gadd.gashow,G.guShowD) = @Date
		-- Sala de ventas
	and G.gusr = @SalesRoom
) as D
	inner join ShowPrograms SH on D.ShowProgram = SH.skID
order by D.guID


--Devolvemos el manifesto
SELECT * FROM #Manifest

-- ================================================
-- 					VENTAS DEPOSITO
-- ================================================
select
	D.*,
	-- Programa de show
	SH.skN as ShowProgramN
from (
	select
		G.guID,
		G.guShowSeq,
		G.gusr,
		G.guls,
		G.guloInvit,
		L.loN,
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
		G.guTour,
		G.guInOut, 
		G.guWalkOut,
		G.guCTour,
		G.guSaveProgram,
		G.guPRInvit1, 
		P1.peN as guPR1N,
		G.guPRInvit2, 
		P2.peN as guPR2N,
		G.guPRInvit3, 
		P3.peN as guPR3N,
		G.guLiner1, 
		L1.peN as guLiner1N,
		G.guLiner2, 
		L2.peN as guLiner2N,
		G.guCloser1, 
		C1.peN as guCloser1N,
		G.guCloser2, 
		C2.peN as guCloser2N,
		G.guCloser3,
		C3.peN as guCloser3N,
		G.guExit1, 
		E1.peN as guExit1N,
		G.guExit2, 
		E2.peN as guExit2N,
		G.guEntryHost, 
		G.guWcomments, 
		G.guBookD, 
		G.guShowD,
		G.guInvitD, 
		G.guDepSale,
		G.guDepositSaleNum,
		G.guDepositSaleD,
		dbo.UFN_OR_GetShowProgram(G.guSaveProgram, G.guCTour, 0, SR.srAppointment) as ShowProgram
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
		G.guDepositSaleD = @Date
		-- Fecha de show diferente de la fecha de venta deposito
		and G.guShowD <> G.guDepositSaleD
		-- Sala de ventas
		and G.gusr = @SalesRoom
) as D
	inner join ShowPrograms SH on D.ShowProgram = SH.skID
order by D.guID

-- ================================================
-- 					OTRAS VENTAS
-- ================================================
-- Ventas de otros dias, Be Backs, OOP, Cancellations, Regens, Deposit Before, etc.
select
	D.*,
	-- Programa de show
	SH.skN as ShowProgramN
from (
	select
		G.guShowD,
		G.guco,
		C.coN,
		G.guag,
		A.agN,
		G.guHotel, 
		G.guRoomNum, 
		G.guPax, 
		G.guDepSale,
		S.sagu,
		dbo.AddString(S.saLastName1, S.saLastName2, ' / ') as saLastName1,
		dbo.AddString(S.saFirstName1, S.saFirstName2, ' / ') as saFirstName1,
		S.sast,
		ST.ststc,
		S.saMembershipNum,
		S.saOriginalAmount,
		S.saGrossAmount, 
		S.saNewAmount, 
		S.sasr,
		G.gusr,
		G.guls,
		S.salo,
		L.loN,
		S.saD,
		S.saProcD,
		S.saCancelD,
		S.saPR1, 
		P1.peN as saPR1N,
		S.saPR2, 
		P2.peN as saPR2N,
		S.saPR3, 
		P3.peN as saPR3N,
		S.saLiner1, 
		L1.peN as saLiner1N,
		S.saLiner2, 
		L2.peN as saLiner2N,
		S.saCloser1, 
		C1.peN as saCloser1N,
		S.saCloser2, 
		C2.peN as saCloser2N,
		S.saCloser3,
		C3.peN as saCloser3N,
		S.saExit1, 
		E1.peN as saExit1N,
		S.saExit2, 
		E2.peN as saExit2N,
		S.saClosingCost,
		S.saByPhone,
		S.saComments,
		S.saProcRD,
		S.saDownPaymentPercentage / 100 as saDownPaymentPercentage,
		S.saDownPaymentPaidPercentage / 100 as saDownPaymentPaidPercentage,
		dbo.UFN_OR_GetShowProgram(G.guSaveProgram, G.guCTour, 0, SR.srAppointment) as ShowProgram,
    Case when s.sacancelD is not null and s.sacanceld  = @Date then
      -1 else 1 end as CnxSale
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
		(((G.guShowD is null or G.guShowD <> @Date)
		-- Fecha de venta
		and (S.saD = @Date 
		-- Fecha de venta procesable
		or S.saProcD = @Date
		-- Fecha de cancelacion
		or S.saCancelD = @Date))
		-- Incluye los Bumps, Regen y las ventas de otra sala del mismo dia
		or ((S.sast in ('BUMP', 'REGEN') or G.gusr <> S.sasr) and S.saD = @Date))
		-- Sala de ventas
		and S.sasr = @SalesRoom
    --Que no esten en el manifesto
    and m.guid is null
) as D
	inner join ShowPrograms SH on D.ShowProgram = SH.skID

GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptManifestByLSRange]    Script Date: 09/22/2016 19:15:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de manifiesto (ProcessorGeneral) permitiendo seleccionar varias salas de ventas y un rango de fechas
**		1. Bookings
**		2. Manifiesto
**		3. Deposit Sales
**		4. Ventas de otros dias, Be Backs, OOP, Cancellations, Regens, Deposit Before, etc.
**
** [axperez]	14/Nov/2013 Creado
** [wtorres]	02/Dic/2013 Agregue el campo de categoria de tipo de venta
** [wtorres]	22/Feb/2014 Agregue el campo de Lead Source
** [LorMartinez] 20/Ene/2016 Se agrega el campo CnxSale en el query de otras ventas
** [lchairez]	26/Abr/2016 Se agregó el campo saOriginalAmount al Manifiesto.
*/
CREATE procedure [dbo].[USP_OR_RptManifestByLSRange]
	@DateFrom Datetime,				-- Fecha desde
	@DateTo Datetime,				-- Fecha hasta
	@SalesRoom varchar(8000) = 'ALL'	-- Claves de las salas de ventas
as
set nocount on

-- ================================================
-- 					BOOKINGS
-- ================================================
select 
	case when G.guDeposit = 0 and L.loFlyers = 1 then G.guloInvit + 'F' else G.guloInvit end as guloInvit,
	case when G.guDeposit = 0 and L.loFlyers = 1 then loN + ' FLYERS' else loN end as LocationN,
	G.guBookT
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
order by G.guloInvit, G.guDeposit, G.guBookT


-- consideramos los shows que tienen ventas en otra sala
select 
	sagu,
	sast,
	saMembershipNum,
	saOriginalAmount,
	saGrossAmount, 
	saNewAmount, 
	saD,
	saProcD,
	saCancelD,
	saClosingCost,
	saComments,
	saProcRD,
	saDownPaymentPercentage,
	saDownPaymentPaidPercentage
into #Sales
from Sales
where (@SalesRoom = 'ALL' or sasr in (select item from split(@SalesRoom, ',')))

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
		G.guID,
		G.gusr,
		G.guls,
		G.guShowSeq,
		isnull(G.guloInvit,gadd.gaLoc) as guloInvit,
		isnull(L.loN,gadd.gaLoN) as loN,
		isnull(L.loFlyers,gadd.gaFlyers) as loFlyers,
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
		G.guPRInvit1, 
		P1.peN as guPR1N,
		G.guPRInvit2, 
		P2.peN as guPR2N,
		G.guPRInvit3, 
		P3.peN as guPR3N,
		G.guLiner1, 
		L1.peN as guLiner1N,
		G.guLiner2, 
		L2.peN as guLiner2N,
		G.guCloser1, 
		C1.peN as guCloser1N,
		G.guCloser2, 
		C2.peN as guCloser2N,
		G.guCloser3,
		C3.peN as guCloser3N,
		G.guExit1, 
		E1.peN as guExit1N,
		G.guExit2, 
		E2.peN as guExit2N,
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
		S.saDownPaymentPaidPercentage / 100 as saDownPaymentPaidPercentage,
		dbo.UFN_OR_GetShowProgram(G.guSaveProgram, G.guCTour, 0, SR.srAppointment) as ShowProgram,
    gadd.gashow  
	from Guests G
		left join #Sales S on G.guID = S.sagu
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
    OUTER APPLY (SELECT 
                  CASE WHEN isnull(ga.gagu,0) > 0 AND g.gushowD is null THEN 
                    g2.guShowD 
                  ELSE
                    NULL
                  END [gaShow],
                  g2.guloInvit [gaLoc],
                  l2.loFlyers [gaFlyers],
                  l2.loN [gaLoN]
               FROM dbo.GuestsAdditional ga
               INNER JOIN dbo.Guests g2 ON g2.guID = ga.gagu
               LEFT JOIN dbo.Locations l2 ON l2.loID= g2.guloInvit
               WHERE ga.gaAdditional = g.guid 
               AND (@SalesRoom = 'ALL' or g2.gusr in (select item from split(@SalesRoom, ',')))
               ) gadd
	where
		-- Fecha de show
		ISNULL(gadd.gashow,G.guShowD)  between @DateFrom and @DateTo
		-- Sala de ventas
		and (@SalesRoom = 'ALL' or G.gusr in (select item from split(@SalesRoom, ',')))
) as D
	inner join ShowPrograms SH on D.ShowProgram = SH.skID
order by D.guID

--Desplegamos el manifesto
Select * from #Manifest

-- ================================================
-- 					VENTAS DEPOSITO
-- ================================================
select
	D.*,
	-- Programa de show
	SH.skN as ShowProgramN
from (
	select
		G.guID,
		G.guShowSeq,
		G.gusr,
		G.guls,
		G.guloInvit,
		L.loN,
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
		G.guTour,
		G.guInOut, 
		G.guWalkOut,
		G.guCTour,
		G.guSaveProgram,
		G.guPRInvit1, 
		P1.peN as guPR1N,
		G.guPRInvit2, 
		P2.peN as guPR2N,
		G.guPRInvit3, 
		P3.peN as guPR3N,
		G.guLiner1, 
		L1.peN as guLiner1N,
		G.guLiner2, 
		L2.peN as guLiner2N,
		G.guCloser1, 
		C1.peN as guCloser1N,
		G.guCloser2, 
		C2.peN as guCloser2N,
		G.guCloser3,
		C3.peN as guCloser3N,
		G.guExit1, 
		E1.peN as guExit1N,
		G.guExit2, 
		E2.peN as guExit2N,
		G.guEntryHost, 
		G.guWcomments, 
		G.guBookD, 
		G.guShowD,
		G.guInvitD, 
		G.guDepSale,
		G.guDepositSaleNum,
		G.guDepositSaleD,
		dbo.UFN_OR_GetShowProgram(G.guSaveProgram, G.guCTour, 0, SR.srAppointment) as ShowProgram
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

-- ================================================
-- 					OTRAS VENTAS
-- ================================================
-- Ventas de otros dias, Be Backs, OOP, Cancellations, Regens, Deposit Before, etc.
select
	D.*,
	-- Programa de show
	SH.skN as ShowProgramN
from (
	select
		G.guShowD,
		G.guco,
		C.coN,
		G.guag,
		A.agN,
		G.guHotel, 
		G.guRoomNum, 
		G.guPax, 
		G.guDepSale,
		S.sagu,
		dbo.AddString(S.saLastName1, S.saLastName2, ' / ') as saLastName1,
		dbo.AddString(S.saFirstName1, S.saFirstName2, ' / ') as saFirstName1,
		S.sast,
		ST.ststc,
		S.saMembershipNum,
    Case when s.sacancelD is not null and s.sacanceld  between @DateFrom and @DateTo then
    S.saGrossAmount * -1 else S.saGrossAmount end saGrossAmount,
		--S.saGrossAmount,
		S.saOriginalAmount, 
		S.saNewAmount, 
		S.sasr,
		G.gusr,
		G.guls,
		S.salo,
		L.loN,
		S.saD,
		S.saProcD,
		S.saCancelD,
		S.saPR1, 
		P1.peN as saPR1N,
		S.saPR2, 
		P2.peN as saPR2N,
		S.saPR3, 
		P3.peN as saPR3N,
		S.saLiner1, 
		L1.peN as saLiner1N,
		S.saLiner2, 
		L2.peN as saLiner2N,
		S.saCloser1, 
		C1.peN as saCloser1N,
		S.saCloser2, 
		C2.peN as saCloser2N,
		S.saCloser3,
		C3.peN as saCloser3N,
		S.saExit1, 
		E1.peN as saExit1N,
		S.saExit2, 
		E2.peN as saExit2N,
		S.saClosingCost,
		S.saByPhone,
		S.saComments,
		S.saProcRD,
		S.saDownPaymentPercentage / 100 as saDownPaymentPercentage,
		S.saDownPaymentPaidPercentage / 100 as saDownPaymentPaidPercentage,
		dbo.UFN_OR_GetShowProgram(G.guSaveProgram, G.guCTour, 0, SR.srAppointment) as ShowProgram,
    Case when s.sacancelD is not null and s.sacanceld  between @DateFrom and @DateTo then
      -1 else 1 end as CnxSale
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

GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptManifestRange]    Script Date: 09/22/2016 19:15:03 ******/
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
** [wtorres]	07/May/2009 Modificado
**					- Solo se consideran ventas procesables o no canceladas
**					- Agregue los campos gusr y sasr para identificar las ventas de otras salas
**					- Agregue el campo saByPhone para identificar las ventas por telefono
** [wtorres]	27/Ago/2009 Agregue el parametro por programa
** [wtorres]	10/Ago/2011 Elimine los campos de calificado y no calificado y agregue los campos de tour y programa de rescate
** [wtorres]	28/Dic/2011 Agregue el campo descripcion de la agencia
** [wtorres]	28/Ene/2012 Agregue el campo descripcion del pais
** [axperez]	12/Nov/2013 Agregue que se puedan seleccionar varias salas de ventas
** [wtorres]	09/Dic/2013 Agregue el campo de categoria de tipo de venta
** [LorMartinez] 21/Ene/2016 Se agrega validacion para GuestAdicionales
** [lchairez]	26/Abr/2016 Se agregan los campos S.saOriginalAmount, S.saNewAmount al Select
*/
CREATE procedure [dbo].[USP_OR_RptManifestRange]
	@DateFrom Datetime,				-- Fecha desde
	@DateTo Datetime,				-- Fecha hasta
	@SalesRoom varchar(8000) = 'ALL',	-- Claves de las salas de ventas
	@Program varchar(10) = 'ALL'	-- Clave del programa
as
set nocount on

-- ================================================
-- 						SHOWS
-- ================================================
select
	G.guID,
	G.gusr,
	G.guLastName1 as LastName,
	G.guFirstName1 as FirstName,
	isnull(G.guloInvit,gadd.gaLoc) as Location,
	G.guHotel,
	G.guRoomNum,
	G.guPax,
	G.guag,
	A.agN,
	G.guco,
	C.coN,
	G.guTimeInT,
	G.guTimeOutT,
	G.guCheckInD,
	G.guCheckOutD,
	G.guTour,
	G.guInOut,
	G.guWalkOut,
	G.guCTour,
	G.guSaveProgram,
	G.guEntryHost,
	G.guWcomments as Comments, 
	G.guBookD,
	G.guShowD,
	G.guInvitD,
	-- Campos de la tabla de ventas
	S.sagu,
	S.sast,
	ST.ststc,
	S.saMembershipNum,
	S.saOriginalAmount,
	S.saNewAmount,
	S.saGrossAmount,
	S.saProcD,
	S.saCancelD,
	S.saClosingCost,
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
  gadd.gashow  
  INTO #Manifest
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
order by G.guShowD, S.sagu

--Mostramos la lista de valores de manifes
select * from #Manifest

-- ================================================
-- 						VENTAS
-- ================================================
select
	S.sagu,
	S.saLastName1 as LastName,
	S.saFirstName1 as FirstName,
	S.salo as Location,
	S.sast,
	ST.ststc,
	S.saMembershipNum,
	S.saOriginalAmount,
	S.saNewAmount,
	S.saGrossAmount,
	S.saD,
	S.saProcD,
	S.saCancelD,
	S.saClosingCost,
	S.saComments as Comments,
	S.sasr,
	S.saByPhone,
	-- Campos de la tabla de Guests
	G.guShowD,
	G.guDepSale,
	G.guHotel,
	G.guRoomNum,
	G.guPax,
	G.guag,
	A.agN,
	G.guco,
	C.coN,
	G.gusr,
	-- PRs
	S.saPR1 as PR1,
	P1.peN as PR1N,
	S.saPR2 as PR2,
	P2.peN as PR2N,
	S.saPR3 as PR3,
	P3.peN as PR3N,
	-- Liners
	S.saLiner1 as Liner1,
	L1.peN as Liner1N,
	S.saLiner2 as Liner2,
	L2.peN as Liner2N,
	-- Closers
	S.saCloser1 as Closer1,
	C1.peN as Closer1N,
	S.saCloser2 as Closer2,
	C2.peN as Closer2N,
	S.saCloser3 as Closer3,
	C3.peN as Closer3N,
	-- Exits
	S.saExit1 as Exit1,
	E1.peN as Exit1N,
	S.saExit2 as Exit2,
	E2.peN as Exit2N
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
	-- Fecha de venta
	(((G.guShowD is null or G.guShowD <> S.saD) and (S.saD between @DateFrom and @DateTo))
	-- Fecha de venta procesable
	or ((G.guShowD is null or G.guShowD <> S.saProcD) and (S.saProcD between @DateFrom and @DateTo))
	-- Fecha de cancelacion
	or ((G.guShowD is null or G.guShowD <> S.saCancelD) and (S.saCancelD between @DateFrom and @DateTo)))
	-- Solo se consideran ventas procesables o no canceladas
	and (not S.saProcD is null or S.saCancelD is null)
	-- Sala de ventas
	and (@SalesRoom = 'ALL' or S.sasr in (select item from split(@SalesRoom, ',')))
	-- Programa
	and (@Program = 'ALL' or lspg = @Program)
	and m.guid is null
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptProductionReferral]    Script Date: 09/22/2016 19:15:06 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_OR_RptPRStats]    Script Date: 09/22/2016 19:15:08 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_OR_SaveGiftsReceiptLog]    Script Date: 09/22/2016 19:15:11 ******/
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


