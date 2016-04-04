if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptFoliosCXC]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptFoliosCXC]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de Folios de CxC
**
** [bcanche]	18/Ene/2015 Created
** [erosado]	04/Mar/2016 Modified. Agregue el uso del parametro @SelfGen = Default en la Funcion UFN_OR_GetPRShows
**							Agregue los parametroes @SalesRooms @Countries @Agencies @Markets a las funciones UFN_OR_GetPRBookings y UFN_OR_GetPRShows
** [wtorres]	04/Abr/2016	Modified. Renombrado. Antes se llamaba USP_OR_FoliosCXC
**
*/
CREATE procedure [dbo].[USP_OR_RptFoliosCXC]
	@DateFrom datetime = null,			-- Fecha desde
	@DateTo datetime = null,			-- Fecha hasta
	@FolioFrom integer = 0,				-- Folio desde
	@FolioTo integer = 0,				-- Folio hasta
	@FolioALL integer = 1,				-- Folio hasta
	@LeadSources varchar(MAX) ='ALL',	-- Lista de Lead Sources
	@PRs varchar(MAX) = 'ALL'			-- Lista de PRs
as
set nocount on

DECLARE @Program VARCHAR(10),
	@FilterDeposit tinyint,
	@PaymentTypes varchar(MAX),		-- Claves de formas de pago
	@FilterDepositAlternate tinyint	-- Filtro de depositos alterno (Aplica para bookings)

SET @Program = 'ALL'
SET	@FilterDeposit = 0	
SET @PaymentTypes = 'ALL'
set @FilterDepositAlternate = @FilterDeposit

IF  @DateFrom IS null OR @DateTo IS NULL 
BEGIN
	SET @DateFrom =  DATEADD(year,-30, GETDATE())
	SET @DateTo = GETDATE()
END	


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
	CASE when g.guShow = 1 THEN 'SHOWS' ELSE 'NO SHOWS' END  AS EsShow,
	SQ.GrossBooks,
	SQ.GrossShows,
	-- Porcentaje de show factor
	[dbo].UFN_OR_SecureDivision(SQ.GrossShows,SQ.GrossBooks) as ShowsFactor,
	CASE when [dbo].UFN_OR_SecureDivision(SQ.GrossShows,SQ.GrossBooks)  >= .70 THEN 'A PAGAR' ELSE 'A RETENER' END  AS Tipo
from Guests G
	left join LeadSources L on G.guls = L.lsID
	left join Personnel P on G.guPRInvit1 = P.peID
	INNER JOIN  bookingDeposits b ON  b.bdgu = g.guID
	inner JOIN  Personnel PP ON  PP.peID = b.bdUserCXC
	left JOIN
	(SELECT t.PR,SUM(t.GrossBooks) AS GrossBooks,SUM(t.GrossShows) AS GrossShows FROM (
		-- Bookings netos
		select PR, Books AS GrossBooks, 0 AS GrossShows
		from UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, 0, 0, 0, @PaymentTypes,default,default,default,default)
		UNION ALL
		-- Shows netos
		select PR, 0, Shows 
		from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, 0, 0, default, default, default, default,
			default, 1, @PaymentTypes,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT)
	) AS T GROUP BY t.pr
) AS SQ on SQ.pr = P.peID
WHERE
	-- Programa Outhouse
	L.lspg = 'OUT' and
	-- Rango de folios
	(@FolioAlL = 1 or (@FolioFrom = 0 and @FolioTo = 0) 
	or (@FolioFrom = 0 and (@FolioTo > 0 and b.bdFolioCXC<= @FolioTo))
	or (@FolioTo = 0 and  (@FolioFrom > 0 and b.bdFolioCXC >= @FolioFrom))
	or (@FolioTo > 0 and @FolioFrom > 0 and (b.bdFolioCXC between @FolioFrom and @FolioTo)))
	-- Fecha de booking
	AND (@DateFrom is null or G.guBookD Between @DateFrom and @DateTo)
	-- Lead Sources
	AND (@LeadSources = 'ALL' OR (@LeadSources <> 'ALL' AND L.lsID IN (select item from dbo.split(@LeadSources,','))))
	-- PR's
	AND (@PRs ='ALL' OR (@PRs <> 'ALL' AND P.peID IN (select item from dbo.split(@PRs,','))))
order BY g.guShow, Tipo,G.guOutInvitNum, b.bdFolioCXC