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
** Devuelve los datos para el reporte de manifiesto por rango de fechas
**		1. Shows
**		2. Ventas
**
** [wtorres]	07/May/2009 Modificado
**					- Solo se consideran ventas procesables o no canceladas
**					- Agregue los campos gusr y sasr para identificar las ventas de otras salas
**					- Agregue el campo saByPhone para identificar las ventas por telefono
** [wtorres]	27/Ago/2009 Agregue el parametro por programa
** [wtorres]	10/Ago/2011 Elimine los campos de calificado y no calificado y agregue los campos de tour y programa de rescate
** [wtorres]	28/Dic/2011 Agregue el campo descripcion de la agencia
** [wtorres]	28/Ene/2012 Agregue el campo descripcion del pais
** [axperez]	12/Nov/2013 Agregue que se puedan seleccionar varias salas de ventas
** [wtorres]	09/Dic/2013 Agregue el campo de categoria de tipo de venta
** [LorMartinez] 21/Ene/2016 Se agrega validacion para GuestAdicionales
*/
CREATE procedure [dbo].[USP_OR_RptManifestRange]
	@DateFrom Datetime,				-- Fecha desde
	@DateTo Datetime,				-- Fecha hasta
	@SalesRoom varchar(8000) = 'ALL',	-- Claves de las salas de ventas
	@Program varchar(10) = 'ALL'	-- Clave del programa
as
set nocount on

-- ================================================
-- 						SHOWS
-- ================================================
select 
	G.guID,
	G.gusr,
	G.guLastName1 as LastName,
	G.guFirstName1 as FirstName,
	isnull(G.guloInvit,gadd.gaLoc) as Location,
	G.guHotel,
	G.guRoomNum,
	G.guPax,
	G.guag,
	A.agN,
	G.guco,
	C.coN,
	G.guTimeInT,
	G.guTimeOutT,
	G.guCheckInD,
	G.guCheckOutD,
	G.guTour,
	G.guInOut,
	G.guWalkOut,
	G.guCTour,
	G.guSaveProgram,
	G.guEntryHost,
	G.guWcomments as Comments, 
	G.guBookD,
	G.guShowD,
	G.guInvitD,
	-- Campos de la tabla de ventas
	S.sagu,
	S.sast,
	ST.ststc,
	S.saMembershipNum,
	S.saGrossAmount,
	S.saProcD,
	S.saCancelD,
	S.saClosingCost,
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
  gadd.gashow  
  INTO #Manifest
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
              ) gadd
  --left join dbo.GuestsAdditional ga ON ga.gaAdditional = G.guID
where 
	-- Fecha de show
	ISNULL(gadd.gashow,G.guShowD) between @DateFrom and @DateTo
	-- Sala de ventas
	and (@SalesRoom = 'ALL' or G.gusr in (select item from split(@SalesRoom, ',')))
	-- Programa
	and (@Program = 'ALL' or lspg = @Program)    
  --and G.guID in (7682136, 7682135)
order by G.guShowD, S.sagu

--Mostramos la lista de valores de manifes
select * from #Manifest


-- ================================================
-- 						VENTAS
-- ================================================
select
	S.sagu,
	S.saLastName1 as LastName,
	S.saFirstName1 as FirstName,
	S.salo as Location,
	S.sast,
	ST.ststc,
	S.saMembershipNum,
	S.saGrossAmount,
	S.saD,
	S.saProcD,
	S.saCancelD,
	S.saClosingCost,
	S.saComments as Comments,
	S.sasr,
	S.saByPhone,
	-- Campos de la tabla de Guests
	G.guShowD,
	G.guDepSale,
	G.guHotel,
	G.guRoomNum,
	G.guPax,
	G.guag,
	A.agN,
	G.guco,
	C.coN,
	G.gusr,
	-- PRs
	S.saPR1 as PR1,
	P1.peN as PR1N,
	S.saPR2 as PR2,
	P2.peN as PR2N,
	S.saPR3 as PR3,
	P3.peN as PR3N,
	-- Liners
	S.saLiner1 as Liner1,
	L1.peN as Liner1N,
	S.saLiner2 as Liner2,
	L2.peN as Liner2N,
	-- Closers
	S.saCloser1 as Closer1,
	C1.peN as Closer1N,
	S.saCloser2 as Closer2,
	C2.peN as Closer2N,
	S.saCloser3 as Closer3,
	C3.peN as Closer3N,
	-- Exits
	S.saExit1 as Exit1,
	E1.peN as Exit1N,
	S.saExit2 as Exit2,
	E2.peN as Exit2N
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
	-- Fecha de venta
	(((G.guShowD is null or G.guShowD <> S.saD) and (S.saD between @DateFrom and @DateTo))
	-- Fecha de venta procesable
	or ((G.guShowD is null or G.guShowD <> S.saProcD) and (S.saProcD between @DateFrom and @DateTo))
	-- Fecha de cancelacion
	or ((G.guShowD is null or G.guShowD <> S.saCancelD) and (S.saCancelD between @DateFrom and @DateTo)))
	-- Solo se consideran ventas procesables o no canceladas
	and (not S.saProcD is null or S.saCancelD is null)
	-- Sala de ventas
	and (@SalesRoom = 'ALL' or S.sasr in (select item from split(@SalesRoom, ',')))
	-- Programa
	and (@Program = 'ALL' or lspg = @Program)   
  --and G.guID in (7682136, 7682135)
  and m.guid is null
GO