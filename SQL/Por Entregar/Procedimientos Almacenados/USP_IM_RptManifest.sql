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

CREATE PROCEDURE dbo.USP_IM_RptManifest
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