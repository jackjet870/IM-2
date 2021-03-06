USE [OrigosVCPalace]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptFoliosCXC]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptFoliosCXC]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de Folios de CxC
**
** [bcanche]		18/Ene/2015 Created
** [erosado]		04/Mar/2016 Modified. Agregue el uso del parametro @SelfGen = Default en la Funcion UFN_OR_GetPRShows
**								Agregue los parametroes @SalesRooms @Countries @Agencies @Markets a las funciones UFN_OR_GetPRBookings y UFN_OR_GetPRShows
** [wtorres]		04/Abr/2016	Modified. Renombrado. Antes se llamaba USP_OR_FoliosCXC
** [lchairez]		11/May/2016 Modified. Se modifica en la linea 70 "A PAGAR" por "TO PAY" Y "A RETENER" por "TO RETAIN"
** [edgrodriguez]	28/Sep/2016 Modified. Se aplican los esquemas de pago para determinar cuando se debe retener o pagar un No Show al PR.
**
*/
CREATE procedure [dbo].[USP_OR_RptFoliosCXC]
	@DateFrom datetime = null,			-- Fecha desde
	@DateTo datetime = null,			-- Fecha hasta
	@FolioFrom integer = 0,				-- Folio desde
	@FolioTo integer = 0,				-- Folio hasta
	@FolioALL integer = 1,				-- Folio hasta
	@LeadSources varchar(MAX) ='ALL',	-- Lista de Lead Sources
	@PRs varchar(MAX) = 'ALL',			-- Lista de PRs
	@Program varchar(100)='ALL'			-- Lista de Programas

as
set nocount on

DECLARE 
	@FilterDeposit tinyint,
	@FilterDepositAlternate tinyint	-- Filtro de depositos alterno (Aplica para bookings)

-- Filtro de depositos:
-- 0. Sin filtro
SET	@FilterDeposit = 0

-- Filtro de depositos alterno (Aplica para bookings)
-- 3. Con deposito y shows sin deposito (Deposits & Flyers Show)
set @FilterDepositAlternate = 3

/* no se utiliza el foliosCXC por que si esta capturado ya fue validado */ 
SELECT 
	b.bdFolioCXC,
	G.guPRInvit1 as PR,
	P.peN as PRN, 
	G.guOutInvitNum,
	L.lsN,
	G.guLastName1,
	G.guFirstName1,
	G.guBookD,
	b.bdUserCXC,
	pp.peN,
	b.bdEntryDCXC,
	G.guStatus,
	g.guShow,
	EsShow = CASE 
		when g.guShow = 1 THEN 'SHOWS' 
		ELSE 'NO SHOWS' END,
	SQ.Books,
	SQ.GrossBooks,
	SQ.InOuts,
	SQ.Shows,
	SQ.GrossShows,
	-- Porcentaje de show factor
	[dbo].UFN_OR_SecureDivision(SQ.GrossShows,SQ.GrossBooks) as ShowsFactor,
	(b.bdAmount - b.bdReceived) as CxC,
	'TO RETAIN' AS Tipo,
	-- Esquema de pago
	Cast('-' as varchar(50)) as PaymentSchema,
	-- Porcentaje de show del esquema de pago
	Cast(0 as money) as PaymentSchemaFactor
into #PRs
from Guests G
	left join LeadSources L on G.guls = L.lsID
	left join Personnel P on G.guPRInvit1 = P.peID
	INNER JOIN  bookingDeposits b ON  b.bdgu = g.guID
	inner JOIN  Personnel PP ON  PP.peID = b.bdUserCXC
	left JOIN
	(
		SELECT 
			t.PR,
			SUM(Books) as Books,
			SUM(t.GrossBooks) AS GrossBooks,
			SUM(InOuts) AS InOuts,
			SUM(Shows) AS Shows,
			SUM(t.GrossShows) AS GrossShows 
		FROM (
		--Bookings
		select PR, Books, 0 as GrossBooks, 0 as InOuts,0 as Shows, 0 as GrossShows
		from dbo.UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, 0, default, default, default, default, default, default, default)
		UNION ALL
		-- Bookings netos
		select PR, 0, Books, 0, 0, 0
		from dbo.UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, 0, 0, default, default, default, default, default, default)
		UNION ALL
		-- IN&Outs
		select PR, 0, 0, Shows, 0, 0
		from dbo.UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, 1, default, default, default, default,
			default, 1, default, default, default, default, default, default)
		UNION ALL
		-- Shows
		select PR, 0, 0, 0, Shows, 0
		from dbo.UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, default, default, default, default, 1,
			default, 1, default, default, default, default, default, default)
		UNION ALL
		-- Shows netos
		select PR, 0, 0, 0, 0, Shows
		from dbo.UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, 1, 0, default, default, default, default,
			default, 1, default, default, default, default, default, default)
	) AS T GROUP BY t.pr
) AS SQ on SQ.pr = P.peID
WHERE
	-- Programa Outhouse
	(@Program = 'ALL' or L.lspg in (select item from split(@Program, ',')))
	-- Rango de folios
	and (@FolioAlL = 1 or (@FolioFrom = 0 and @FolioTo = 0) 
	or (@FolioFrom = 0 and (@FolioTo > 0 and b.bdFolioCXC<= @FolioTo))
	or (@FolioTo = 0 and  (@FolioFrom > 0 and b.bdFolioCXC >= @FolioFrom))
	or (@FolioTo > 0 and @FolioFrom > 0 and (b.bdFolioCXC between @FolioFrom and @FolioTo)))
	-- Fecha de booking
	AND (@DateFrom is null or G.guBookD Between @DateFrom and @DateTo)
	-- Lead Sources
	AND (@LeadSources = 'ALL' OR (@LeadSources <> 'ALL' AND L.lsID IN (select item from dbo.split(@LeadSources,','))))
	-- PR's
	AND (@PRs ='ALL' OR (@PRs <> 'ALL' AND P.peID IN (select item from dbo.split(@PRs,','))))
	-- CxC > 0
	AND (b.bdAmount - b.bdReceived) > 0
order BY g.guShow, Tipo,G.guOutInvitNum, b.bdFolioCXC

;With cte as
(
	SELECT 
		P.*,
		ISNULL(Pas.pasShowFactor,0) pasShowFactor,
		ISNULL(Pas.pasN,'-') pasN
	FROM #PRs P
		OUTER APPLY
		(
			SELECT 
				pasShowFactor/100 pasShowFactor,
				pasN
			FROM dbo.PaymentSchemas
			WHERE P.GrossShows >= pasCoupleFrom and  P.GrossShows <= pasCoupleTo
		) Pas
)

Update cte
set PaymentSchemaFactor = pasShowFactor,
	PaymentSchema = pasN,
	Tipo = CASE
				WHEN ShowsFactor > 0 AND ShowsFactor >= pasShowFactor THEN 'TO PAY'
				ELSE Tipo END
				
SELECT
*
FROM
#PRs