USE [OrigosVCPalace]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_IM_RptLinerStatistics]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_IM_RptLinerStatistics]
GO
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

