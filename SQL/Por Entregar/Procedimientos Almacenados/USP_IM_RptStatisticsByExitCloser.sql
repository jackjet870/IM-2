if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_IM_RptStatisticsByExitCloser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_IM_RptStatisticsByExitCloser]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de estadisticas por ExitCloser (Processor Sales)
**
** [aalcocer] 18/Jul/2016 Created
**
*/
create procedure [dbo].[USP_IM_RptStatisticsByExitCloser]
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
	SalesAmountRange VARCHAR(10),
	Sales money,
	SalesTotal money,	
	Efficiency money,
	ClosingFactor money,
	SaleAverage money
	);
--#endregion

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
		SELECT DISTINCT m.guID, s.SalemanID,s.SalemanName,s.SalemanType,
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
		
--===================================================================
--===================  SELECT  ===========================
--===================================================================
SELECT * FROM @StatsByExitCloser 
ORDER BY SalemanType, SalesAmount DESC, SalemanID  