if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptFoliosInvitationsOuthouseByPR]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptFoliosInvitationsOuthouseByPR]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta los huespedes de un Lead Source
** 
** [lchairez]	08/Ene/2016 Created
** [wtorres]	04/Abr/2016	Modified. Renombrado. Antes se llamaba USP_OR_FoliosInvitationsOuthouseByPR
**
*/
CREATE PROCEDURE [dbo].[USP_OR_RptFoliosInvitationsOuthouseByPR]
	@DateFrom DATETIME,	-- Fecha desde
	@DateTo DATETIME,	-- Fecha hasta
	@Serie VARCHAR(5) = 'ALL',	-- Serie
	@FolioFrom INT = 0, --Desde que folio
	@FolioTo INT = 0, --Hasta que folio
	@LeadSource VARCHAR(MAX) = 'ALL',	-- Lista Lead Source
	@Promotors VARCHAR(MAX) = 'ALL'	-- Lista de promotores
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @tbUsedFolios AS TABLE --Folios Usados
	(
	  guID INT, 
	  guls VARCHAR(10), 
	  guFirstName1 VARCHAR(49),
	  guLastName1 VARCHAR(60),
	  guPRInvit1 VARCHAR(10),
	  guSerie VARCHAR(5),
	  guFrom INT,
	  guTo INT,
	  guBookD DATETIME
	)
	
	DECLARE @tbNotUseFolios AS TABLE --folios no usados
	(
	  fippe VARCHAR(10),
	  fipSerie VARCHAR(5),
	  fipFrom INT,
	  fipTo INT
	)

	DECLARE @tbCancelledFolios AS TABLE --folios cancelados
	(
	  ficID INT, 
	  ficpe VARCHAR(10), 
	  ficSerie VARCHAR(5), 
	  ficFrom INT, 
	  ficTo INT
	)

	DECLARE @tbDetailUsedFolios AS TABLE (Serie VARCHAR(5), Folio INT)
	
	--obtenemos los folios usados de acuerdo a las fechas establecidas.
	INSERT INTO @tbUsedFolios
	SELECT guID, guls, guFirstName1, guLastName1, guPRInvit1, guSerie, guFrom, guTo ,guBookD
	FROM(
			SELECT guID, guls, guFirstName1, guLastName1, guPRInvit1
				  ,Substring(G.guOutInvitNum, 0 , CharIndex('-', G.guOutInvitNum)) guSerie,
				CASE WHEN isNumeric(Substring(guOutInvitNum, CharIndex('-', guOutInvitNum) + 1, Len(guOutInvitNum) - CharIndex('-', guOutInvitNum))) = 1 THEN
					Substring(guOutInvitNum, CharIndex('-', guOutInvitNum) + 1, Len(guOutInvitNum) - CharIndex('-', guOutInvitNum))
				ELSE
					0
				END guFrom,
				CASE WHEN isNumeric(Substring(guOutInvitNum, CharIndex('-', guOutInvitNum) + 1, Len(guOutInvitNum) - CharIndex('-', guOutInvitNum))) = 1 then
					Substring(guOutInvitNum, CharIndex('-', guOutInvitNum) + 1, Len(guOutInvitNum) - CharIndex('-', guOutInvitNum))
				ELSE
					0
				END guTo
				  , guBookD
			FROM Guests G
			WHERE (@LeadSource = 'ALL' OR guls IN (SELECT item FROM dbo.Split(@LeadSource,',')))
				AND (@Promotors = 'ALL' OR guPRInvit1 IN (SELECT item FROM dbo.Split(@Promotors,',')))
				AND (@DateFrom is null or guBookD Between @DateFrom and @DateTo AND guOutInvitNum IS NOT NULL)
				AND guOutInvitNum IS NOT NULL
	) AS s
	WHERE @Serie = 'ALL' OR (guSerie = @Serie AND guFrom BETWEEN @FolioFrom AND @FolioTo)

	--obtenemos los rangos defolios usados
	INSERT INTO @tbNotUseFolios(fipserie,fipFrom,fipTo, fippe)
	SELECT serie, [from], [to], fippe
	FROM UFN_OR_GetRangeUsedFolios(@Serie, @FolioFrom, @FolioTo, @LeadSource ,@Promotors,@DateFrom,@DateTo)

	----obtenemos los folios cancelados por PR
	INSERT INTO @tbCancelledFolios
	SELECT FC.ficID, FC.ficpe, FC.ficSerie, FC.ficFrom, FC.ficTo
	FROM FolioInvitationsOutsidePRCancellation FC
	WHERE @Promotors = 'ALL' OR ficpe IN (SELECT item FROM dbo.Split(@Promotors,','))


	SELECT lsID, lsN, guSerie, guFrom, guTo, f.guPRInvit1, p.peN, guLastName1, guFirstName1, guBookD, 'USED' guStatus
	FROM @tbUsedFolios f
	JOIN Personnel p ON f.guPRInvit1 = p.peID
	JOIN LeadSources l on f.guls = lsID
	UNION 
	SELECT NULL, NULL, fipSerie, fipFrom, fipTo, f.fippe, p.peN, NULL, NULL, NULL, 'NOT USED'
	FROM @tbNotUseFolios f
	JOIN Personnel p ON f.fippe = p.peID
	UNION
	SELECT NULL, NULL, ficSerie, ficFrom, ficTo, f.ficpe, p.peN, NULL, NULL, NULL, 'CANCELLED'
	FROM @tbCancelledFolios f
	JOIN Personnel p ON f.ficpe = p.peID
	ORDER BY guSerie, guFrom,f.guPRInvit1

END
GO
