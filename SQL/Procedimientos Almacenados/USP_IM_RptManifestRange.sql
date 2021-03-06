USE [OrigosVCPalace]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_IM_RptManifestRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_IM_RptManifestRange]
GO
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