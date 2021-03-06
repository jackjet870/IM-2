/****** Object:  StoredProcedure [dbo].[USP_IM_RptStatisticsByCloser]    Script Date: 08/15/2016 10:20:38 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptStatisticsByCloser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptStatisticsByCloser]
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