USE ORIGOSVCPALACE

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Regresa una tabla con los rangos de folios no usados
** 
** [lchairez]	08/Ene/2016 Creado
**
*/
ALTER FUNCTION [dbo].[UFN_OR_GetRangeUsedFolios]
(
	@Series VARCHAR(5) = 'ALL', --Series
	@FolioFrom INT = 0, --desde que folio
	@FolioTo	INT = 0, --hasta que folio
	@LeadSource VARCHAR(MAX) = 'ALL',
	@Promotors VARCHAR(MAX) = 'ALL', --Lista Promotores
	@DateFrom DATETIME,	-- Fecha desde
	@DateTo DATETIME	-- Fecha hasta
)
RETURNS @RangeUsedFolios TABLE
(
	serie varchar(5),
	[from] int, 
	[to] int,
	fippe VARCHAR(10)
)
AS
BEGIN

	DECLARE @tbUsedCancelledFolios AS TABLE --Folios Usados
	(
	  guID INT, 
	  guls VARCHAR(10), 
	  guFirstName1 VARCHAR(49),
	  guLastName1 VARCHAR(60),
	  guPRInvit1 VARCHAR(10),
	  guSerie VARCHAR (8),
	  guFrom INT,
	  guTo INT,
	  guBookD DATETIME
	)
	
	--OBTENGO LOS FOLIOS USADOS 
	INSERT INTO @tbUsedCancelledFolios
	SELECT guID, guls, guFirstName1, guLastName1, guPRInvit1, guSerie, guFrom, guTo ,guBookD
	FROM(	
		SELECT guID, guls, guFirstName1, guLastName1, guPRInvit1, guOutInvitNum
				,Substring(guOutInvitNum, 0 , CharIndex('-', guOutInvitNum)) guSerie,
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
	WHERE @Series = 'ALL' OR (guSerie IN (@Series) AND guFrom BETWEEN @FolioFrom AND @FolioTo)
	
	
	
	--OBTENEMOS LOS FOLIOS CANCELADOS
	INSERT INTO @tbUsedCancelledFolios
	SELECT NULL, NULL, NULL, NULL, ficpe, ficSerie, ficFrom, ficTo, NULL
	FROM FolioInvitationsOutsidePRCancellation
	WHERE (@Series = 'ALL' OR (ficSerie = @Series AND ficFrom BETWEEN @FolioFrom AND @FolioTo))
		AND (@Promotors = 'ALL' OR ficpe IN (SELECT item FROM dbo.Split(@Promotors,',')))
	
			
	DECLARE @tbNewRangeFolios AS TABLE(serie varchar(5),[from] int,[to] int,fippe VARCHAR(10))

	DECLARE @serie varchar(5), @from int, @to int,@folio int, @fippe VARCHAR(10)
	DECLARE @newFrom int, @newTo int
	DECLARE @numFolio INT --numero de folio
	DECLARE @serieFol varchar(5), @fromFol int, @toFol int,@folioFol int, @fippeFol VARCHAR(10), @usedFol bit

	--Tabla para insertar cada folio de los rangos
	DECLARE @tbRegFoliosXRango AS TABLE (serie varchar(5), [from] int, [to] int,folio int, fippe VARCHAR(10),used bit)

	DECLARE range_cursor CURSOR FOR 

	  	--OBTENEMOS LOS RANGOS A LOS QUE PERTENECEN LOS FOLIOS USADOS
	  SELECT DISTINCT
		FPR.fipSerie, FPR.fipFrom, FPR.fipTo ,FPR.fippe
	  FROM FolioInvitationsOutsidePR FPR
	  JOIN @tbUsedCancelledFolios T ON FPR.fipSerie = T.guSerie AND T.guFrom BETWEEN FPR.fipFrom AND FPR.fipTo
	  
	  OPEN range_cursor

	  FETCH NEXT FROM range_cursor 
	  INTO @serie, @from, @to, @fippe
	  
	  WHILE @@FETCH_STATUS = 0
	  BEGIN
		  SET @numFolio = @from      
	      
		  WHILE @numFolio <= @to
		  BEGIN
			--INSERTAMOS FOLIO POR FOLIO DE CADA RANGO, SI EL RANGO ES A1 HASTA A5, INSERTAMOS A1,A2,A3,A4 Y A5
			INSERT INTO @tbRegFoliosXRango(serie,[from],[To],Folio,fippe,used) 
			  VALUES(@serie, @from, @to, @numFolio, @fippe, 0)
			SET @numFolio = @numFolio + 1
		  END
	      
		  --ACTUALIZAMOS LOS FOLIOS QUE YA SE USARON 
		  UPDATE FR SET FR.used = 1
		  FROM @tbRegFoliosXRango FR
		  JOIN @tbUsedCancelledFolios FU ON FR.serie = FU.guSerie AND FR.Folio = FU.guFrom
	      
		  DECLARE folio_cursor CURSOR FOR 
			  SELECT serie, [from], [to], folio, fippe ,used
			  FROM @tbRegFoliosXRango
	          
			  OPEN folio_cursor
			  FETCH NEXT FROM folio_cursor 
			  INTO @serieFol, @fromFol, @toFol, @folioFol, @fippeFol, @usedFol
	          
			  --variables que sirven para saber los bloques de rangos
			  DECLARE @fromGroup int, @ToGroup int, @serieGroup varchar(5)
			  SELECT @fromGroup = @fromFol,
					@ToGroup = @toFol,
					@newFrom = @fromFol,
					@newTo = @toFol,
					@numFolio = @folio,
					@serieGroup = @serieFol
	          
			  WHILE @@FETCH_STATUS = 0
			  BEGIN
				IF(@serieGroup <> @serieFol)
				BEGIN
				   SELECT @fromGroup = @fromFol,
					@ToGroup = @toFol,
					@newFrom = @fromFol,
					@newTo = @toFol,
					@numFolio = @folio,
					@serieGroup = @serieFol
				END
	            
				--REVISAMOS QUE EL FROM Y TO DE CADA BLOQUE DE RANGOS NO CAMBIE, SI ES ASI SE INSERTA UN NUEVO RANGO EN LA TABLA
				IF(@fromGroup <> @fromFol AND @ToGroup <> @toFol)
				BEGIN
				  INSERT INTO @tbNewRangeFolios(serie,[from],[To],fippe) 
					VALUES(@serieFol, @newFrom, @newTo, @fippe)
					SET @fromGroup = @fromFol
					SET @ToGroup = @toFol
					SET @newFrom  = @fromFol
					SET @serieGroup = @serieFol
				END
	            
				IF @usedFol = 1
				BEGIN
				  IF @folioFol <> @newFrom
				  BEGIN
					INSERT INTO @tbNewRangeFolios(serie,[from],[To],fippe) 
					  VALUES(@serieFol, @newFrom, @folioFol -1 , @fippe)
					  SET @newFrom = @folioFol + 1
					  SET @newTo = @folioFol + 1
				   END
				   ELSE
				   BEGIN
					  SET @newFrom = @folioFol + 1
					  SET @newTo = @folioFol + 1
				   END
				END
				ELSE
				  SET @newTo = @folioFol
	            
				FETCH NEXT FROM folio_cursor 
				INTO @serieFol, @fromFol, @toFol, @folioFol, @fippeFol, @usedFol
			  END
	        
			INSERT INTO @tbNewRangeFolios(serie,[from],[To],fippe) 
					VALUES(@serieFol, @newFrom, @newTo, @fippe)
			
			DELETE FROM @tbRegFoliosXRango
			
		CLOSE folio_cursor
		DEALLOCATE folio_cursor
	           
		-- Get the next vendor.
		FETCH NEXT FROM range_cursor 
		INTO @serie, @from, @to, @fippe
	  END 
	CLOSE range_cursor;
	DEALLOCATE range_cursor;
	
	INSERT INTO @RangeUsedFolios
	SELECT DISTINCT * 
	FROM @tbNewRangeFolios
	ORDER BY serie, [from]
	
	RETURN 
END
GO