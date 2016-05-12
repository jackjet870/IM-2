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