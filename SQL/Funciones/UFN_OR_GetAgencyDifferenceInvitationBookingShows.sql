IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[UFN_OR_GetAgencyDifferenceInvitationBookingShows]') AND xtype IN (N'FN', N'IF', N'TF'))
DROP FUNCTION [dbo].[UFN_OR_GetAgencyDifferenceInvitationBookingShows]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por agencia
** 
** [lchairez]	19/Feb/2014 Creado
**
*/
CREATE FUNCTION [dbo].[UFN_OR_GetAgencyDifferenceInvitationBookingShows]
(	
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@FilterDeposit tinyint = 0,			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
	@InOut int = -1,					-- Filtro de In & Outs:
										--		-1. Sin filtro
										--		 0. No In & Outs
										--		 1. In & Outs
	@WalkOut int = -1,					-- Filtro de Walk Outs:
										--		-1. Sin filtro
										--		 0. No Walk Outs
										--		 1. Walk & Outs
	@TourType int = 0,					-- Filtro de tipo de tour:
										--		0. Sin filtro
										--		1. Tours regulares
										--		2. Tours de cortesia
										--		3. Tours de rescate	
	@ConsiderTourSale bit = 0			-- Indica si se debe considerar los shows con tour o venta -UPS
)
RETURNS @Table TABLE (
	Agency VARCHAR(25),
	DaysDifference INT,
	Shows INT
)
AS
BEGIN

INSERT @Table
SELECT D.guag, D.DaysDifference, SUM(Shows) Shows
FROM (
	SELECT
		G.guag,
		DateDiff(Day, G.guInvitD, G.guBookD) AS DaysDifference,
		CASE WHEN @ConsiderQuinellas = 0 THEN 1 ELSE G.guShowsQty END Shows
	FROM Guests G
		INNER JOIN LeadSources L ON L.lsID = G.guls
	WHERE
		-- Fecha de show
		G.guShowD BETWEEN @DateFROM AND @DateTo
		-- No Quiniela Split
		AND (@ConsiderQuinellas = 0 OR G.guQuinellaSplit = 0)
		-- Lead Sources
		AND (@LeadSources = 'ALL' OR G.guls IN (SELECT item FROM split(@LeadSources, ','))) 
		-- PRs
		AND (@PRs = 'ALL' OR (G.guPRInvit1 IN (SELECT item FROM split(@PRs, ','))
			OR G.guPRInvit2 IN (SELECT item FROM split(@PRs, ','))
			OR G.guPRInvit3 IN (SELECT item FROM split(@PRs, ','))))
		-- Programa
		AND (@Program = 'ALL' OR L.lspg = @Program)
		-- Filtro de depositos
		AND (@FilterDeposit = 0 OR (@FilterDeposit = 1 AND G.guDeposit > 0) OR (@FilterDeposit = 2 AND G.guDeposit = 0))
		-- In & Outs
		AND (@InOut = -1 OR G.guInOut = @InOut)
		-- Walk Outs
		AND (@WalkOut = -1 OR G.guWalkOut = @WalkOut)
		-- Filtro de tipo de tour
		AND (@TourType = 0 OR (@TourType = 1 AND G.guTour = 1) OR (@TourType = 2 AND G.guCTour = 1) OR (@TourType = 3 AND G.guSaveProgram = 1))		
		-- Con tour o venta - UPS
		AND (@ConsiderTourSale = 0 OR (G.guTour = 1 OR G.guSale = 1))	
) AS D
GROUP BY D.guag, D.DaysDifference

RETURN
END
