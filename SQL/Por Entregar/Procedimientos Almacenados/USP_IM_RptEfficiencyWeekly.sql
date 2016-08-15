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