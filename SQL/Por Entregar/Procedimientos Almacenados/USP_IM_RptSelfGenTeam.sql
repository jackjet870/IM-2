/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte Self Gen & Self Gen Team (Processor Sales)
**		1.- SelfGen 2.- SelfGen Team
**
** [ecanul] 25/07/2016 Created
**
*/

CREATE PROCEDURE dbo.USP_IM_RptSelfGenTeam
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
LEFT JOIN dbo.Sales S on S.sagu = G.guID
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
LEFT JOIN dbo.Sales S on S.sagu = G.guID
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
	WHERE T.id = @count-- AND T.saID != @saID
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