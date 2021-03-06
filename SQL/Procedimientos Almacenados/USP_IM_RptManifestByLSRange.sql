USE [OrigosVCPalace]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_IM_RptManifestByLSRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_IM_RptManifestByLSRange]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de manifiesto (ProcessorGeneral) permitiendo seleccionar varias salas de ventas y un rango de fechas
**		1. Reporte Manifiesto
**		2. Bookings
**
** [edgrodriguez]	01/Jun/2016 Created
** [edgrodriguez]	20/Jun/2016 Modified Se agregaron las columnas DownPayment,DownPaymentPercentage,DownPaymentPaid,DownPaymentPaidPercentage
** [edgrodriguez]	21/Sep/2016 Modified Se quito el OuterApply y se uso una tabla temporal para obtener los Guest Additionals.
** [edgrodriguez]	23/Sep/2016 Modified Se agrego la validacion de las ventas canceladas, ya que el ProcSale mostrará -1 y los montos seran negativos.
*/
ALTER procedure [dbo].[USP_IM_RptManifestByLSRange]
	@DateFrom Datetime,				-- Fecha desde
	@DateTo Datetime,				-- Fecha hasta
	@SalesRoom varchar(8000) = 'ALL'	-- Claves de las salas de ventas
as
--set fmtonly off
set nocount on

-- ================================================
-- 					GUEST ADDITIONAL
-- ================================================

DECLARE @AddGuest table(gagu integer,
                        gaShow datetime,
                        gaLoc varchar(50),
                        gaFlyers varchar(50),
                        gaLoN varchar(50))


INSERT INTO @AddGuest       
SELECT  ga.gaAdditional,      
    CASE WHEN isnull(ga.gagu,0) > 0 AND g.gushowD is null THEN 
      g2.guShowD 
    ELSE
      NULL
    END [gaShow],
    g2.guloInvit [gaLoc],
    l2.loFlyers [gaFlyers],
    l2.loN [gaLoN]    
FROM guests g
INNER JOIN dbo.GuestsAdditional ga ON ga.gaAdditional = g.guid                
INNER JOIN dbo.Guests g2 ON g2.guID = ga.gagu
LEFT JOIN dbo.Locations l2 ON l2.loID= g2.guloInvit
WHERE 
(@SalesRoom = 'ALL' or g2.gusr in (select item from split(@SalesRoom, ',')))
AND (@SalesRoom = 'ALL' or g.gusr in (select item from split(@SalesRoom, ',')))
AND g2.guShowD between @DateFrom and @DateTo


-- ================================================
-- 					MANIFIESTO
-- ================================================
select
	D.*,
	-- Programa de show
	SH.skN as ShowProgramN
  INTO #Manifest
from (
	select
		DateManifest = CASE
			WHEN G.guShowD IS NOT NULL THEN G.guShowD
			ELSE
			(SELECT gaShow from @AddGuest where gagu=G.guID)
		END,
		SaleType = CASE
			WHEN G.guCTour = 1 THEN 1
			WHEN G.guSaveProgram = 1 THEN 2
			WHEN S.sagu IS NOT NULL THEN
				CASE 
					WHEN S.sast<>'BUMP' and S.sast<>'REGEN'
					AND (S.saD BETWEEN @DateFrom and @DateTo) 
					AND (S.sast='DNG' OR S.sast='DP') THEN 5
					ELSE
					0
				END
			ELSE
			0
		END ,
		SaleTypeN = CASE
			WHEN G.guCTour = 1 THEN 'COURTESY TOUR'
			WHEN G.guSaveProgram = 1 THEN 'SAVE TOUR'
			WHEN S.sagu IS NOT NULL THEN
				CASE 
					WHEN S.sast<>'BUMP' and S.sast<>'REGEN'
					AND (S.saD BETWEEN @DateFrom and @DateTo) 
					AND (S.sast='DNG' OR S.sast='DP') THEN 'DOWNGRADES'
					ELSE
					'MANIFEST'
				END
			ELSE
			'MANIFEST'
		END,
		G.guID,
		G.gusr,
		G.guls,
		G.guShowSeq,
		isnull(G.guloInvit,(SELECT gaLoc from @AddGuest where gagu=G.guID)) as guloInvit,
		isnull(L.loN,(SELECT gaLoN from @AddGuest where gagu=G.guID)) as loN,
		isnull(L.loFlyers,(SELECT gaFlyers from @AddGuest where gagu=G.guID)) as loFlyers,
		G.guDeposit,
		G.guHotel, 
		G.guRoomNum, 
		G.guPax,
		dbo.AddString(G.guLastName1, G.guLastName2, ' / ') as guLastName1,
		dbo.AddString(G.guFirstName1, G.guFirstName2, ' / ') as guFirstName1,
		G.guag,
		A.agN,
		G.guco,
		C.coN, 
		G.guDirect,
		G.guTimeInT,
		G.guTimeOutT, 
		G.guCheckInD,
		G.guCheckOutD,
		G.guResch,
		G.guReschD,
		G.guShow,
		G.guTour,
		G.guInOut, 
		G.guWalkOut,
		G.guCTour,
		G.guSaveProgram,
		G.guPRInvit1 PR1, 
		P1.peN as PR1N,
		G.guPRInvit2 PR2, 
		P2.peN as PR2N,
		G.guPRInvit3 PR3, 
		P3.peN as PR3N,
		G.guLiner1 Liner1, 
		L1.peN as Liner1N,
		G.guLiner2 Liner2, 
		L2.peN as Liner2N,
		G.guCloser1 Closer1, 
		C1.peN as Closer1N,
		G.guCloser2 Closer2, 
		C2.peN as Closer2N,
		G.guCloser3 Closer3,
		C3.peN as Closer3N,
		G.guExit1 Exit1, 
		E1.peN as Exit1N,
		G.guExit2 Exit2, 
		E2.peN as Exit2N,
		G.guEntryHost, 
		G.guWcomments, 
		G.guBookD, 
		G.guShowD,
		G.guInvitD, 
		G.guDepSale,
		G.guDepositSaleNum,
		G.guDepositSaleD,
		S.sagu,
		S.sast,
		ST.ststc,
		S.saMembershipNum,
		S.saOriginalAmount,
		S.saGrossAmount, 
		S.saNewAmount, 
		S.saD,
		S.saProcD,
		S.saCancelD,
		S.saClosingCost,
		S.saComments,
		S.saProcRD,
		S.saDownPaymentPercentage / 100 as saDownPaymentPercentage,
		(S.saDownPaymentPercentage / 100) * S.saGrossAmount as saDownPayment,
		S.saDownPaymentPaidPercentage / 100 as saDownPaymentPaidPercentage,
		(S.saDownPaymentPaidPercentage / 100)* S.saGrossAmount as saDownPaymentPaid,
		dbo.UFN_OR_GetShowProgram(G.guSaveProgram, G.guCTour, 0, SR.srAppointment) as ShowProgram,
		gashow = CASE
			WHEN G.guShowD IS NOT NULL THEN NULL
			ELSE
			(SELECT gaShow from @AddGuest where gagu=G.guID)
		END,
		NULL CnxSale,
		S.sasr,
		S.salo  
	from Guests G
		left outer join Sales S on G.guID = S.sagu and (S.saD between @DateFrom and @DateTo or S.saProcD between @DateFrom and @DateTo or S.saCancelD between @DateFrom and @DateTo) 
		left join SaleTypes ST on ST.stID = S.sast
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
		left join Locations L on L.loID = G.guloInvit
		left join SalesRooms SR on SR.srID = G.gusr
		left join Agencies A on A.agID = G.guag
		left join Countries C on C.coID = G.guco
	where
		-- Fecha de show
		(G.guShowD BETWEEN @DateFrom and @DateTo
		or G.guID in (select gagu from @AddGuest)
		)
		-- Sala de ventas
		and (@SalesRoom = 'ALL' or G.gusr in (select item from split(@SalesRoom, ',')))
) as D
	inner join ShowPrograms SH on D.ShowProgram = SH.skID
order by D.guID

--Select * from #Manifest

-- ================================================
-- 					Ventas Deposito
-- ================================================
select
	D.*,
	-- Programa de show
	SH.skN as ShowProgramN
	into #DepositSales
from (
	select
		NULL DateManifest,
		12 SaleType,
		'DEPOSIT SALE' SaleTypeN,
		G.guID,
		G.gusr,
		G.guls,
		G.guShowSeq,
		G.guloInvit,
		L.loN,
		NULL loFlyers,
		G.guDeposit,
		G.guHotel, 
		G.guRoomNum, 
		G.guPax,
		dbo.AddString(G.guLastName1, G.guLastName2, ' / ') as guLastName1,
		dbo.AddString(G.guFirstName1, G.guFirstName2, ' / ') as guFirstName1,
		G.guag,
		A.agN, 
		G.guco,
		C.coN, 
		G.guDirect,
		G.guTimeInT,
		G.guTimeOutT, 
		G.guCheckInD,
		G.guCheckOutD,
		G.guResch,
		G.guReschD,
		G.guShow,
		G.guTour,
		G.guInOut, 
		G.guWalkOut,
		G.guCTour,
		G.guSaveProgram,
		G.guPRInvit1 PR1, 
		P1.peN as PR1N,
		G.guPRInvit2 PR2, 
		P2.peN as PR2N,
		G.guPRInvit3 PR3, 
		P3.peN as PR3N,
		G.guLiner1 Liner1, 
		L1.peN as Liner1N,
		G.guLiner2 Liner2, 
		L2.peN as Liner2N,
		G.guCloser1 Closer1, 
		C1.peN as Closer1N,
		G.guCloser2 Closer2, 
		C2.peN as Closer2N,
		G.guCloser3 Closer3,
		C3.peN as Closer3N,
		G.guExit1 Exit1, 
		E1.peN as Exit1N,
		G.guExit2 Exit2, 
		E2.peN as Exit2N,
		G.guEntryHost, 
		G.guWcomments, 
		G.guBookD, 
		G.guShowD,
		G.guInvitD, 
		G.guDepSale,
		G.guDepositSaleNum,
		G.guDepositSaleD,
		'' sagu,
		'' sast,
		'' ststc,
		'' saMemberShip,
		NULL saOriginalAmount,
		NULL saGrossAmount,
		NULL saNewAmount,
		NULL saD,
		NULL saProcD,
		NULL saCancelD,
		NULL saClosingCost,
		'' saComments,
		NULL saProcRD,
		NULL saDownPaymentPercentage,
		NULL saDownPayment,
		NULL saDownPaymentPaidPercentage,
		NULL saDownPaymentPaid,
		dbo.UFN_OR_GetShowProgram(G.guSaveProgram, G.guCTour, 0, SR.srAppointment) as ShowProgram,
		'' gaShow,
		NULL CnxSale,
		'' sasr,
		'' salo	
		from Guests G
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
		left join Locations L on L.loID = G.guloInvit
		left join SalesRooms SR on SR.srID = G.gusr
		left join Agencies A on A.agID = G.guag
		left join Countries C on C.coID = G.guco
	where
		-- Fecha de venta deposito
		G.guDepositSaleD between @DateFrom and @DateTo
		-- Fecha de show diferente de la fecha de venta deposito
		and G.guShowD <> G.guDepositSaleD
		-- Sala de ventas
		and (@SalesRoom = 'ALL' or G.gusr in (select item from split(@SalesRoom, ',')))
) as D
	inner join ShowPrograms SH on D.ShowProgram = SH.skID
order by D.guID

--Select * from #DepositSales

-- ================================================
-- 					OTRAS VENTAS
-- ================================================
-- Ventas de otros dias, Be Backs, OOP, Cancellations, Regens, Deposit Before, etc.
select
	D.*,
	-- Programa de show
	SH.skN as ShowProgramN
	into #OtherSales
from (
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
		G.guls,
		G.guShowSeq,
		G.guloInvit,
		L.loN,
		'' loFlyers,
		G.guDeposit,
		G.guHotel, 
		G.guRoomNum, 
		G.guPax, 
		dbo.AddString(S.saLastName1, S.saLastName2, ' / ') as saLastName1,
		dbo.AddString(S.saFirstName1, S.saFirstName2, ' / ') as saFirstName1,
		G.guag,
		A.agN,
		G.guco,
		C.coN,
		Cast(0 as bit) guDirect,
		NULL guTimeInT,
		NULL guTimeOutT, 
		NULL guCheckInD,
		NULL guCheckOutD,
		Cast(0 as bit) guResch,
		NULL guReschD,
		Cast(0 as bit) guShow,
		Cast(0 as bit) guTour,
		Cast(0 as bit) guInOut, 
		Cast(0 as bit) guWalkOut,
		Cast(0 as bit) guCTour,
		Cast(0 as bit) guSaveProgram,
		S.saPR1 PR1, 
		P1.peN as PR1N,
		S.saPR2 PR2, 
		P2.peN as PR2N,
		S.saPR3 PR3, 
		P3.peN as PR3N,
		S.saLiner1 Liner1, 
		L1.peN as Liner1N,
		S.saLiner2 Liner2, 
		L2.peN as Liner2N,
		S.saCloser1 Closer1, 
		C1.peN as Closer1N,
		S.saCloser2 Closer2, 
		C2.peN as Closer2N,
		S.saCloser3 Closer3,
		C3.peN as Closer3N,
		S.saExit1 Exit1, 
		E1.peN as Exit1N,
		S.saExit2 Exit2, 
		E2.peN as Exit2N,
		'' guEntryHost,
		'' guWComments,
		NULL guBookD,		
		G.guShowD,
		NULL guInvitD,
		G.guDepSale,
		NULL guDepositSaleNum,
		NULL guDepositSaleD,
		S.sagu,
		S.sast,
		ST.ststc,
		S.saMembershipNum,
		S.saOriginalAmount, 
	    Case when s.sacancelD is not null and s.sacanceld  between @DateFrom and @DateTo then
		S.saGrossAmount * -1 else S.saGrossAmount end saGrossAmount,
		S.saNewAmount, 
		S.saD,
		S.saProcD,
		S.saCancelD,
		S.saClosingCost,
		S.saComments,
		S.saProcRD,
		S.saDownPaymentPercentage / 100 as saDownPaymentPercentage,
		(S.saDownPaymentPercentage / 100) * S.saGrossAmount as saDownPayment,
		S.saDownPaymentPaidPercentage / 100 as saDownPaymentPaidPercentage,
		(S.saDownPaymentPaidPercentage / 100)* S.saGrossAmount as saDownPaymentPaid,
		dbo.UFN_OR_GetShowProgram(G.guSaveProgram, G.guCTour, 0, SR.srAppointment) as ShowProgram,
		NULL gaShow,
		Case when s.sacancelD is not null and s.sacanceld  between @DateFrom and @DateTo then
		-1 else 1 end as CnxSale,
		S.sasr,
		S.salo
	from Sales S
		left join SaleTypes ST on ST.stID = S.sast
		left join Guests G on G.guID = S.sagu
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
		left join Locations L on L.loID = S.salo
		left join SalesRooms SR on SR.srID = S.sasr
		left join Agencies A on A.agID = G.guag
		left join Countries C on C.coID = G.guco
    left join #Manifest m ON m.guid = S.sagu AND m.gashow is not null
	where
		-- No shows
		(((G.guShowD is null or not(G.guShowD between @DateFrom and @DateTo))
		-- Fecha de venta
		and (S.saD between @DateFrom and @DateTo 
		-- Fecha de venta procesable
		or S.saProcD between @DateFrom and @DateTo
		-- Fecha de cancelacion
		or S.saCancelD between @DateFrom and @DateTo)) 
		--Considera los Bumps y Regen del dia de hoy
		--*20070702 EJZ Incluir ventas de otra sala del mismo dia
		or ((S.sast in ('BUMP', 'REGEN') or G.gusr <> S.sasr) and S.saD between @DateFrom and @DateTo))
		-- Sala de ventas
		and (@SalesRoom = 'ALL' or S.sasr in (select item from split(@SalesRoom, ',')))
    --Que no esten en el manifesto
    and m.guid is null
    
) as D
	inner join ShowPrograms SH on D.ShowProgram = SH.skID

-- ================================================
--			Reporte Manifest Range By LS
-- ================================================

SELECT
	SaleType,
	SaleTypeN,
	DateManifest,
	guID = CASE 
		WHEN sagu IS NOT NULL THEN sagu
		ELSE guID END,
	Location= CASE 
		WHEN SaleType=0 OR SaleType=1 OR SaleType=2 THEN
			CASE loFlyers
				WHEN 1 THEN 
				CASE
					WHEN guDeposit = 0 THEN guloInvit + 'F'
				ELSE
					guloInvit
				END
			ELSE
				guloInvit
			END
		WHEN SaleType=12 THEN
			guloInvit
		ELSE
			salo
		END,			
	LocationN = CASE 
		WHEN SaleType = 0 OR SaleType = 1 OR SaleType = 2 THEN
			CASE loFlyers
				WHEN 1 THEN 
				CASE
					WHEN guDeposit = 0 THEN loN + ' FLYERS'
					ELSE
						loN
					END
				ELSE
				loN
			END
		WHEN SaleType > 2 THEN
			loN
		END,
	ShowProgramN,
	SalesRoom = CASE 
		WHEN SaleType = 0 OR SaleType = 1 OR SaleType = 2 THEN gusr
		ELSE sasr END,
	guls LeadSource,
	Sequency = CASE 
		WHEN SaleType = 0 OR SaleType = 1 OR SaleType = 2 THEN guShowSeq end,
	guHotel Hotel,
	guRoomNum Room,
	Pax = CASE 
		WHEN SaleType = 0 OR SaleType = 1 OR SaleType = 2 THEN guPax end,
	guLastName1 LastName,
	guFirstName1 FirstName,
	guag Agency,
	agN AgencyN,
	guShowD ShowD,
	guco Country,
	coN CountryD,
	TimeInT = CASE 
		WHEN SaleType = 0 OR SaleType = 1 OR SaleType = 2 OR SaleType = 12 THEN guTimeInT
		else NULL end,
	TimeOutT = CASE 
		WHEN SaleType = 0 OR SaleType = 1 OR SaleType = 2 OR SaleType = 12 THEN guTimeOutT
		else NULL end,
	guTour Tour,
	guInOut IO,
	guWalkOut WO,
	guCTour CTour,
	guSaveProgram STour,
	guDirect Direct,
	Resch = Cast(CASE
		WHEN guResch=1 and (guReschD BETWEEN @DateFrom AND @DateTo) THEN 1
		ELSE 0 END as bit),
	--Tours = CASE 
	--	WHEN guTour=1 OR guWalkOut=1 OR (guCtour=1 OR guSaveProgram=1) AND sagu IS NOT NULL THEN 1
	--	ELSE 0 END,
	--guShow Shows,
	--RealShows = CASE
	--	WHEN guTour=1 OR guInOut=1 OR guWalkOut=1 or guCTour=1 OR guSaveProgram=1 THEN 1
	--	ELSE 0 END,
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
	Hostess = CASE 
		WHEN SaleType = 0 OR SaleType = 1 OR SaleType = 2 OR SaleType = 12 THEN guEntryHost
		ELSE NULL END,
	ProcSales=Cast(CASE
		WHEN sagu IS NOT NULL 
		AND saProcD IS NOT NULL
		AND (saProcD BETWEEN @DateFrom AND @DateTo) 
		AND (saCancelD IS NULL OR saCancelD NOT BETWEEN @DateFrom AND @DateTo) THEN 1
		WHEN sagu IS NOT NULL 
		AND saProcD IS NOT NULL
		AND (saProcD NOT BETWEEN @DateFrom AND @DateTo) 
		AND (saCancelD IS NOT NULL OR saCancelD BETWEEN @DateFrom AND @DateTo) THEN -1
		else 0
		end as int),
	ProcOriginal=CASE
		WHEN sagu IS NOT NULL 
		AND saProcD IS NOT NULL
		AND (saProcD BETWEEN @DateFrom AND @DateTo) 
		AND (saCancelD IS NULL OR saCancelD NOT BETWEEN @DateFrom AND @DateTo) THEN saOriginalAmount
		WHEN sagu IS NOT NULL 
		AND saProcD IS NOT NULL
		AND (saProcD NOT BETWEEN @DateFrom AND @DateTo) 
		AND (saCancelD IS NOT NULL OR saCancelD BETWEEN @DateFrom AND @DateTo) THEN saOriginalAmount*(-1)
		end,
	ProcNew=CASE
		WHEN sagu IS NOT NULL 
		AND saProcD IS NOT NULL
		AND (saProcD BETWEEN @DateFrom AND @DateTo) 
		AND (saCancelD IS NULL OR saCancelD NOT BETWEEN @DateFrom AND @DateTo) THEN saNewAmount
		WHEN sagu IS NOT NULL 
		AND saProcD IS NOT NULL
		AND (saProcD NOT BETWEEN @DateFrom AND @DateTo) 
		AND (saCancelD IS NOT NULL OR saCancelD BETWEEN @DateFrom AND @DateTo) THEN saNewAmount*(-1)
		end,
	ProcGross=CASE
		WHEN sagu IS NOT NULL 
		AND saProcD IS NOT NULL
		AND (saProcD BETWEEN @DateFrom AND @DateTo) 
		AND (saCancelD IS NULL OR saCancelD NOT BETWEEN @DateFrom AND @DateTo) THEN saGrossAmount
		WHEN sagu IS NOT NULL 
		AND saProcD IS NOT NULL
		AND (saProcD NOT BETWEEN @DateFrom AND @DateTo) 
		AND (saCancelD IS NOT NULL OR saCancelD BETWEEN @DateFrom AND @DateTo) THEN saGrossAmount
		end	,
	PendSales=CAST(CASE
		WHEN sagu IS NOT NULL 
		AND ((saProcD NOT BETWEEN @DateFrom AND @DateTo) OR (saProcD IS NULL))
		AND (saCancelD IS NULL OR saCancelD NOT BETWEEN @DateFrom AND @DateTo) THEN 1
		else 0
		end as int),
	PendOriginal=CASE
		WHEN sagu IS NOT NULL 
		AND ((saProcD NOT BETWEEN @DateFrom AND @DateTo) OR (saProcD IS NULL))
		AND (saCancelD IS NULL OR saCancelD NOT BETWEEN @DateFrom AND @DateTo) THEN saOriginalAmount
		end,
	PendNew=CASE
		WHEN sagu IS NOT NULL 
		AND ((saProcD NOT BETWEEN @DateFrom AND @DateTo) OR (saProcD IS NULL))
		AND (saCancelD IS NULL OR saCancelD NOT BETWEEN @DateFrom AND @DateTo) THEN saNewAmount
		end,
	PendGross=CASE
		WHEN sagu IS NOT NULL 
		AND ((saProcD NOT BETWEEN @DateFrom AND @DateTo) OR (saProcD IS NULL))
		AND (saCancelD IS NULL OR saCancelD NOT BETWEEN @DateFrom AND @DateTo) THEN saGrossAmount
		end,
	DepositSale = CASE
		WHEN guDepositSaleD BETWEEN @DateFrom AND @DateTo THEN guDepSale
		ELSE NULL END,
	DepositSaleNum = CASE
		WHEN guDepositSaleD BETWEEN @DateFrom AND @DateTo THEN guDepositSaleNum
		ELSE NULL END,
	--saMembership Membership,
	CC = CASE 
		WHEN SaleType = 0 OR SaleType = 1 OR SaleType = 2 OR SaleType = 12 THEN (select STUFF((
             SELECT ',' + (CASE 
              WHEN gdQuantity > 1 THEN Convert(varchar(10),gdQuantity) ELSE '' END)+ ' ' + gdcc
             FROM GuestsCreditCards
             WHERE gdgu=(CASE 
		WHEN sagu IS NOT NULL THEN sagu
		ELSE guID END)
             FOR XML PATH('')
       ),1,1,''))
		ELSE NULL END,
	Comments = CASE 
		WHEN SaleType = 0 OR SaleType = 1 OR SaleType = 2  OR SaleType = 12 THEN guWComments
		ELSE saComments END,
	saMembershipNum,
	guShow Show,
	saDownPaymentPercentage,
	saDownPayment,
	saDownPaymentPaidPercentage,
	saDownPaymentPaid
FROM( 
SELECT * FROM #Manifest
UNION ALL
SELECT * FROM #DepositSales
UNION ALL 
SELECT * FROM #OtherSales
) RptManifest
ORDER BY SaleType, Location, ShowProgramN, Sequency, TimeInt, LastName

-- ================================================
-- 					BOOKINGS
-- ================================================
SELECT
 guloInvit,
 LocationN,
 guBookT,
 Count(guBookT) Bookings
 FROM(
select 
	case when G.guDeposit = 0 and L.loFlyers = 1 then G.guloInvit + 'F' else G.guloInvit end as guloInvit,
	case when G.guDeposit = 0 and L.loFlyers = 1 then loN + ' FLYERS' else loN end as LocationN,
	case
		 when CONVERT(VARCHAR(8),G.guBookT,108) <= '09:29:59' then '08:30'
		 when CONVERT(VARCHAR(8),G.guBookT,108) between '09:30:00' and '10:29:59' then '09:30'
		 when CONVERT(VARCHAR(8),G.guBookT,108) between '10:30:00' and '11:29:59' then '10:30'
		 when CONVERT(VARCHAR(8),G.guBookT,108) between '11:30:00' and '12:29:59' then '11:30'
		 when CONVERT(VARCHAR(8),G.guBookT,108) >= '12:30:00' then '12:30'
		 else NULL 
	end guBookT
from Guests G
	inner join Locations L on G.guloInvit = L.loID
where
	-- Fecha de booking
	G.guBookD between @DateFrom and @DateTo
	-- Sala de ventas
	and (@SalesRoom = 'ALL' or G.gusr in (select item from split(@SalesRoom, ',')))
	-- No directas
	and G.guDirect = 0
	-- No reschedules
	and (G.guReschD is null or not(G.guReschD between @DateFrom and @DateTo))
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- No bookings cancelados
	and G.guBookCanc = 0
	) Bookings
GROUP BY guloInvit,LocationN,guBookT
order by guloInvit, guBookT
