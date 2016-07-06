/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de F.T.M. In & Out House
** [ecanul] 01/Jul/2016 Created
**
*/
ALTER PROCEDURE dbo.USP_IM_RptFTMInOutHouse
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