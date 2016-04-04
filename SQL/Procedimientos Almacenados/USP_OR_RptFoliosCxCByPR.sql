if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptFoliosCxCByPR]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptFoliosCxCByPR]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta los folios CxC
** 
** [lchairez]	18/Ene/2016 Created
** [wtorres]	04/Abr/2016	Modified. Renombrado. Antes se llamaba USP_OR_FoliosPRCxC
**
*/
CREATE PROCEDURE [dbo].[USP_OR_RptFoliosCxCByPR]
	@DateFrom DATETIME = NULL,
	@DateTo DATETIME = NULL,
	@allFolios BIT = 1,
	@FolioFrom INT = 0, 
	@FolioTo INT = 0,
	@LeadSource VARCHAR(MAX) = 'ALL',
	@Promotors VARCHAR(MAX) = 'ALL'
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @tbUsedFolios AS TABLE --Folios Usados
	(
	  lsID VARCHAR(10), 
	  firstName VARCHAR(49),
	  lastName VARCHAR(60),
	  prID VARCHAR(10),
	  [From] INT,
	  [To] INT,
	  bookD DATETIME
	)
	
	DECLARE @tbNotUseFolios AS TABLE --folios no usados
	(
	  prID VARCHAR(10),
	  [from] INT,
	  [to] INT
	)

	DECLARE @tbCancelledFolios AS TABLE --folios cancelados
	(
	  ficID INT, 
	  prID VARCHAR(10), 
	  [from] INT, 
	  [to] INT
	)

	--DECLARE @tbDetailUsedFolios AS TABLE (Serie VARCHAR(5), Folio INT)
	
	--obtenemos los folios usados de acuerdo a las fechas establecidas.
	INSERT INTO @tbUsedFolios
	SELECT g.guls AS lsID, g.guLastName1 AS lastName, g.guFirstName1 AS firstName,
		 g.guPRInvit1 AS prID, bd.bdFolioCxC AS [from], bd.bdFolioCxC AS [to],
		 g.guBookD AS bookD
	FROM FolioCxCPR fpr
	JOIN Guests g ON fpr.fcppe = g.guPRInvit1 
	LEFT JOIN BookingDeposits bd ON bd.bdgu = g.[guID] 
	WHERE bd.bdFolioCxC IS NOT NULL
		AND ((@allFolios = 1 AND bd.bdFolioCxC BETWEEN fpr.fcpFrom AND fpr.fcpTo) OR (@allFolios = 0 AND bdFolioCxC BETWEEN @FolioFrom AND @FolioTo))
		AND (@DateFrom is null or g.guBookD Between @DateFrom and @DateTo)
		AND (@LeadSource = 'ALL' OR g.guls IN (SELECT item FROM dbo.Split(@LeadSource,',')))
		AND (@Promotors = 'ALL' OR g.guPRInvit1 IN (SELECT item FROM dbo.Split(@Promotors,',')))


	--obtenemos los rangos defolios usados
	INSERT INTO @tbNotUseFolios([from], [to], prID)
	SELECT [from], [to], prID
	FROM UFN_OR_GetRangeUsedFoliosCxC(@allFolios, @FolioFrom, @FolioTo, @LeadSource ,@Promotors,@DateFrom,@DateTo)

	------obtenemos los folios cancelados por PR
	INSERT INTO @tbCancelledFolios
	SELECT FC.fccID, FC.fccpe, FC.fccFrom, FC.fccTo
	FROM FolioCxCCancellation FC
	WHERE @Promotors = 'ALL' OR fccpe IN (SELECT item FROM dbo.Split(@Promotors,','))


	SELECT f.lsID, lsN, [from] AS guFrom, [to] AS guTo, f.prID AS guPRInvit1, p.peN, lastName AS guLastName1
			, firstName AS guFirstName1, bookD AS guBookD, 'USED' guStatus
	FROM @tbUsedFolios f
	JOIN Personnel p ON f.prID = p.peID
	JOIN LeadSources l on f.lsID = l.lsID
	UNION 
	SELECT NULL, NULL, [from], [to], f.prID, p.peN, NULL, NULL, NULL, 'NOT USED'
	FROM @tbNotUseFolios f
	JOIN Personnel p ON f.prID = p.peID
	UNION
	SELECT NULL, NULL, [from], [to], f.prID, p.peN, NULL, NULL, NULL, 'CANCELLED'
	FROM @tbCancelledFolios f
	JOIN Personnel p ON f.prID = p.peID
	ORDER BY f.prID, f.[from]

END

GO


