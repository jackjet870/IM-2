IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[UFN_OR_GetAgencyDifferenceInvitationBookingBookings]') AND xtype IN (N'FN', N'IF', N'TF'))
DROP FUNCTION [dbo].[UFN_OR_GetAgencyDifferenceInvitationBookingBookings]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de bookings por agencia
** 
** [lchairez]	19/Feb/2014 Creado
**
*/
CREATE FUNCTION [dbo].[UFN_OR_GetAgencyDifferenceInvitationBookingBookings]
(	
	@DateFrom DATETIME,					-- Fecha desde
	@DateTo DATETIME,					-- Fecha hasta
	@LeadSources VARCHAR(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs VARCHAR(8000) = 'ALL',			-- Claves de PRs
	@Program VARCHAR(10) = 'ALL',		-- Clave de programa
	@ConsiderQuinellas BIT = 0,			-- Indica si se debe considerar quinielas
	@FilterDeposit TINYINT = 0,			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
										--		3. Con deposito y shows sin deposito (Deposits & Flyers Show)
	@Direct INT = -1,					-- Filtro de directas:
										--		-1. Sin filtro
										--		 0. No directas
										--		 1. Directas
	@InOut INT = -1					-- Filtro de In & Outs:
										--		-1. Sin filtro
										--		 0. No In & Outs
										--		 1. In & Outs
)
RETURNS @Table TABLE (
	Agency VARCHAR(25),
	DaysDifference INT,
	Books INT
)
AS
BEGIN

INSERT @Table
SELECT
	D.guag,
	D.DaysDifference,
	SUM(D.Books) AS Books
FROM (
	SELECT
		G.guag,
		DateDiff(Day, G.guInvitD, G.guBookD) AS DaysDifference,
		CASE WHEN @ConsiderQuinellas = 0 THEN 1 ELSE G.guRoomsQty END AS Books
	FROM Guests G
		INNER JOIN LeadSources L on L.lsID = G.guls
	WHERE
		-- No Antes In & Out
		G.guAntesIO = 0
		-- No Quiniela Split
		AND (@ConsiderQuinellas = 0 OR G.guQuinellaSplit = 0)
		-- Lead Sources
		AND (@LeadSources = 'ALL' OR G.guls IN (SELECT item FROM split(@LeadSources, ','))) 
		-- PRs
		AND (@PRs = 'ALL' OR (G.guPRInvit1 IN (SELECT item FROM split(@PRs, ','))
			OR G.guPRInvit2 IN (SELECT item FROM split(@PRs, ','))
			OR G.guPRInvit3 in (SELECT item FROM split(@PRs, ','))))
		-- Programa
		AND (@Program = 'ALL' OR L.lspg = @Program)
		-- Filtro de depositos y fechas de booking y show
		AND (((@FilterDeposit <> 3 AND G.guBookD BETWEEN @DateFrom AND @DateTo)
			AND (@FilterDeposit = 0 OR (@FilterDeposit = 1 AND G.guDeposit > 0) OR (@FilterDeposit = 2 AND G.guDeposit = 0)))
			OR (@FilterDeposit = 3 AND ((G.guBookD BETWEEN @DateFrom AND @DateTo AND G.guDeposit > 0)
			OR (G.guShowD BETWEEN @DateFrom AND @DateTo AND G.guDeposit = 0 AND G.guInOut = 0))))
		-- Directas
		AND (@Direct = -1 OR G.guDirect = @Direct)
		-- In & Outs
		AND (@InOut = -1 OR G.guInOut = @InOut)
		-- No bookings cancelados
		AND G.guBookCanc = 0
) AS D
GROUP BY D.guag,D.DaysDifference

RETURN
END