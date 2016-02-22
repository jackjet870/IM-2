USE [OrigosVCPalace];
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de manifiesto (Host)
**		1. Bookings
**		2. Manifiesto
**		3. Deposit Sales
**		4. Ventas de otros dias, Be Backs, OOP, Cancellations, Regens, Deposit Before, etc.
**
** [wtorres]	05/Ene/2009 Depuracion
** [wtorres]	11/May/2009 Modifique la consulta de bookings:
**								1. Devolver el nombre de la locacion (loN)
**								2. Elimine los campos guDeposit, loFlyers
** [wtorres]	18/Jun/2010 Agregue el campo saProcRD
** [wtorres]	20/Jul/2011 Ahora no se traen los bookings cancelados
** [wtorres]	08/Ago/2011 Agregue los campos de programa de show
** [wtorres]	10/Ago/2011 Elimine los campos de calificado y no calificado y agregue el campo de programa de rescate
** [wtorres]	12/Oct/2011 Elimine el campo de categoria de programa de show y fusione los In & outs en Regular Tours
** [wtorres]	11/Ene/2012 Agregue el campo descripcion de la agencia
** [wtorres]	28/Ene/2012 Agregue el campo descripcion del pais
** [wtorres]	03/Jun/2013 Agregue los campos de porcentaje de enganche pactado y pagado
** [wtorres]	16/Nov/2013 Agregue el campo de categoria de tipo de venta
** [wtorres]	22/Feb/2014 Agregue el campo de Lead Source
** [LorMartinez] 21/Ene/2016 Se agrega validacion para GuestAdicionales
*/
CREATE procedure [dbo].[USP_OR_RptManifestByLS]
	@Date Datetime,			-- Fecha
	@SalesRoom varchar(10)	-- Clave de la sala
as
set nocount on

-- ================================================
-- 					BOOKINGS
-- ================================================
select 
	case when G.guDeposit = 0 and L.loFlyers = 1 then G.guloInvit + 'F' else G.guloInvit end as guloInvit,
	case when G.guDeposit = 0 and L.loFlyers = 1 then loN + ' FLYERS' else loN end as LocationN,
	G.guBookT
from Guests G
	inner join Locations L on G.guloInvit = L.loID
where
	-- Fecha de booking
	G.guBookD = @Date
	-- Sala de ventas
	and G.gusr = @SalesRoom
	-- No directas
	and G.guDirect = 0
	-- No reschedules
	and (G.guReschD is null or G.guReschD <> @Date)
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- No bookings cancelados
	and G.guBookCanc = 0
order by G.guloInvit, G.guDeposit, G.guBookT


-- consideramos los shows que tienen ventas en otra sala
select 
	sagu,
	sast,
	saMembershipNum,
	saGrossAmount, 
	saNewAmount, 
	saD,
	saProcD,
	saCancelD,
	saClosingCost,
	saComments,
	saProcRD,
	saDownPaymentPercentage,
	saDownPaymentPaidPercentage
into #Sales
from Sales
where sasr = @SalesRoom

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
		G.guID,
		G.gusr,
		G.guls,
		G.guShowSeq,
		isnull(G.guloInvit,gadd.gaLoc) as guloInvit,
		isnull(L.loN,gadd.gaLoN) as loN,
		isnull(L.loFlyers,gadd.gaFlyers) as loFlyers,
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
		G.guPRInvit1, 
		P1.peN as guPR1N,
		G.guPRInvit2, 
		P2.peN as guPR2N,
		G.guPRInvit3, 
		P3.peN as guPR3N,
		G.guLiner1, 
		L1.peN as guLiner1N,
		G.guLiner2, 
		L2.peN as guLiner2N,
		G.guCloser1, 
		C1.peN as guCloser1N,
		G.guCloser2, 
		C2.peN as guCloser2N,
		G.guCloser3,
		C3.peN as guCloser3N,
		G.guExit1, 
		E1.peN as guExit1N,
		G.guExit2, 
		E2.peN as guExit2N,
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
		S.saGrossAmount, 
		S.saNewAmount, 
		S.saD,
		S.saProcD,
		S.saCancelD,
		S.saClosingCost,
		S.saComments,
		S.saProcRD,
		S.saDownPaymentPercentage / 100 as saDownPaymentPercentage,
		S.saDownPaymentPaidPercentage / 100 as saDownPaymentPaidPercentage,
		dbo.UFN_OR_GetShowProgram(G.guSaveProgram, G.guCTour, 0, SR.srAppointment) as ShowProgram,
    gadd.gashow  
	from Guests G
		left join #Sales S on G.guID = S.sagu
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
     OUTER APPLY (SELECT 
                  CASE WHEN isnull(ga.gagu,0) > 0 AND g.gushowD is null THEN 
                    g2.guShowD 
                  ELSE
                    NULL
                  END [gaShow],
                  g2.guloInvit [gaLoc],
                  l2.loFlyers [gaFlyers],
                  l2.loN [gaLoN]
               FROM dbo.GuestsAdditional ga
               INNER JOIN dbo.Guests g2 ON g2.guID = ga.gagu
               LEFT JOIN dbo.Locations l2 ON l2.loID= g2.guloInvit
               WHERE ga.gaAdditional = g.guid  ) gadd
	where
		-- Fecha de show
		ISNULL(gadd.gashow,G.guShowD) = @Date
		-- Sala de ventas
		and G.gusr = @SalesRoom
) as D
	inner join ShowPrograms SH on D.ShowProgram = SH.skID
order by D.guID

--Devolvemos el manifesto
SELECT * FROM #Manifest

-- ================================================
-- 					VENTAS DEPOSITO
-- ================================================
select
	D.*,
	-- Programa de show
	SH.skN as ShowProgramN
from (
	select
		G.guID,
		G.guShowSeq,
		G.gusr,
		G.guls,
		G.guloInvit,
		L.loN,
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
		G.guTour,
		G.guInOut, 
		G.guWalkOut,
		G.guCTour,
		G.guSaveProgram,
		G.guPRInvit1, 
		P1.peN as guPR1N,
		G.guPRInvit2, 
		P2.peN as guPR2N,
		G.guPRInvit3, 
		P3.peN as guPR3N,
		G.guLiner1, 
		L1.peN as guLiner1N,
		G.guLiner2, 
		L2.peN as guLiner2N,
		G.guCloser1, 
		C1.peN as guCloser1N,
		G.guCloser2, 
		C2.peN as guCloser2N,
		G.guCloser3,
		C3.peN as guCloser3N,
		G.guExit1, 
		E1.peN as guExit1N,
		G.guExit2, 
		E2.peN as guExit2N,
		G.guEntryHost, 
		G.guWcomments, 
		G.guBookD, 
		G.guShowD,
		G.guInvitD, 
		G.guDepSale,
		G.guDepositSaleNum,
		G.guDepositSaleD,
		dbo.UFN_OR_GetShowProgram(G.guSaveProgram, G.guCTour, 0, SR.srAppointment) as ShowProgram
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
		G.guDepositSaleD = @Date
		-- Fecha de show diferente de la fecha de venta deposito
		and G.guShowD <> G.guDepositSaleD
		-- Sala de ventas
		and G.gusr = @SalesRoom
) as D
	inner join ShowPrograms SH on D.ShowProgram = SH.skID
order by D.guID

-- ================================================
-- 					OTRAS VENTAS
-- ================================================
-- Ventas de otros dias, Be Backs, OOP, Cancellations, Regens, Deposit Before, etc.
select
	D.*,
	-- Programa de show
	SH.skN as ShowProgramN
from (
	select
		G.guShowD,
		G.guco,
		C.coN,
		G.guag,
		A.agN,
		G.guHotel, 
		G.guRoomNum, 
		G.guPax, 
		G.guDepSale,
		S.sagu,
		dbo.AddString(S.saLastName1, S.saLastName2, ' / ') as saLastName1,
		dbo.AddString(S.saFirstName1, S.saFirstName2, ' / ') as saFirstName1,
		S.sast,
		ST.ststc,
		S.saMembershipNum,
		S.saGrossAmount, 
		S.saNewAmount, 
		S.sasr,
		G.gusr,
		G.guls,
		S.salo,
		L.loN,
		S.saD,
		S.saProcD,
		S.saCancelD,
		S.saPR1, 
		P1.peN as saPR1N,
		S.saPR2, 
		P2.peN as saPR2N,
		S.saPR3, 
		P3.peN as saPR3N,
		S.saLiner1, 
		L1.peN as saLiner1N,
		S.saLiner2, 
		L2.peN as saLiner2N,
		S.saCloser1, 
		C1.peN as saCloser1N,
		S.saCloser2, 
		C2.peN as saCloser2N,
		S.saCloser3,
		C3.peN as saCloser3N,
		S.saExit1, 
		E1.peN as saExit1N,
		S.saExit2, 
		E2.peN as saExit2N,
		S.saClosingCost,
		S.saByPhone,
		S.saComments,
		S.saProcRD,
		S.saDownPaymentPercentage / 100 as saDownPaymentPercentage,
		S.saDownPaymentPaidPercentage / 100 as saDownPaymentPaidPercentage,
		dbo.UFN_OR_GetShowProgram(G.guSaveProgram, G.guCTour, 0, SR.srAppointment) as ShowProgram,
    Case when s.sacancelD is not null and s.sacanceld  = @Date then
      -1 else 1 end as CnxSale
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
		(((G.guShowD is null or G.guShowD <> @Date)
		-- Fecha de venta
		and (S.saD = @Date 
		-- Fecha de venta procesable
		or S.saProcD = @Date
		-- Fecha de cancelacion
		or S.saCancelD = @Date))
		-- Incluye los Bumps, Regen y las ventas de otra sala del mismo dia
		or ((S.sast in ('BUMP', 'REGEN') or G.gusr <> S.sasr) and S.saD = @Date))
		-- Sala de ventas
		and S.sasr = @SalesRoom
    --Que no esten en el manifesto
    and m.guid is null
) as D
	inner join ShowPrograms SH on D.ShowProgram = SH.skID
GO