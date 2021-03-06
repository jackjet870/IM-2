USE [OrigosVCPalace]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_IM_RptCloserStatistics]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_IM_RptCloserStatistics]
GO
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