USE [OrigosVCPalace]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetRangeUsedFolios]    Script Date: 01/18/2016 09:43:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Regresa una tabla con los rangos de folios CxC no usados
** 
** [lchairezReload]	18/Ene/2016 Creado
**
*/
CREATE FUNCTION [dbo].[UFN_OR_GetRangeUsedFoliosCxC]
(
	@allFolios		BIT = 1,
	@FolioFrom		INT = 0,
	@FolioTo		INT = 0,
	@LeadSource		VARCHAR(MAX) = 'ALL',
	@Promotors		VARCHAR(MAX) = 'ALL',
	@DateFrom DATETIME = NULL,
	@DateTo DATETIME = NULL
)
RETURNS @RangeUsedFolios TABLE
(
	[from] int, 
	[to] int,
	prID VARCHAR(10)
)
AS
BEGIN

	DECLARE @tbUsedCancelledFolios AS TABLE --Folios Usados
	(
	  lsID VARCHAR(10), 
	  lastName VARCHAR(60),
	  firstName VARCHAR(49),
	  prID VARCHAR(10),
	  [from] INT,
	  [to] INT,
	  bookD DATETIME
	)
	
	--OBTENGO LOS FOLIOS USADOS 
	INSERT INTO @tbUsedCancelledFolios
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
	
	
	--OBTENEMOS LOS FOLIOS CANCELADOS
	INSERT INTO @tbUsedCancelledFolios
	SELECT NULL, NULL, NULL, fccpe, fccFrom, fccTo, NULL
	FROM FolioCxCCancellation
	WHERE (@allFolios = 1 OR ( fccFrom >= @FolioFrom AND fccTo <= @FolioTo))
		AND (@Promotors = 'ALL' OR fccpe IN (SELECT item FROM dbo.Split(@Promotors,',')))
	
	--Tabla para insertar cada folio de los rangos
	DECLARE @tbRegFoliosXRango AS TABLE ([from] int, [to] int,folio int, prID VARCHAR(10),used bit)
	--Tabla que guarda los rangos que mostraremos de folios no usados
	DECLARE @tbNewRangeFolios AS TABLE([from] int,[to] int,prID VARCHAR(10))

	DECLARE @from int, @to int,@folio int, @prID VARCHAR(10)
	DECLARE @newFrom int, @newTo int
	DECLARE @numFolio INT --numero de folio
	DECLARE @fromFol int, @toFol int,@folioFol int, @prIDFol VARCHAR(10), @usedFol bit

			
	DECLARE range_cursor CURSOR FOR 

	  	--OBTENEMOS LOS RANGOS A LOS QUE PERTENECEN LOS FOLIOS USADOS
		SELECT DISTINCT
			FPR.fcpFrom, FPR.fcpTo ,FPR.fcppe
		FROM FolioCxCPR fpr
		JOIN @tbUsedCancelledFolios t ON fpr.fcppe = t.prID AND T.[from] >= fpr.fcpFrom AND T.[to] <= fpr.fcpTo
	  
		OPEN range_cursor

		FETCH NEXT FROM range_cursor 
		INTO @from, @to, @prID
	  
		WHILE @@FETCH_STATUS = 0
		BEGIN
			SET @numFolio = @from      
	      
			WHILE @numFolio <= @to
			BEGIN
				--INSERTAMOS FOLIO POR FOLIO DE CADA RANGO, SI EL RANGO ES A1 HASTA A5, INSERTAMOS A1,A2,A3,A4 Y A5
				INSERT INTO @tbRegFoliosXRango([from],[To],Folio,prID,used) 
					VALUES(@from, @to, @numFolio, @prID, 0)
				SET @numFolio = @numFolio + 1
			END
	      
			--ACTUALIZAMOS LOS FOLIOS QUE YA SE USARON 
			UPDATE FR SET FR.used = 1
			FROM @tbRegFoliosXRango FR
			JOIN @tbUsedCancelledFolios FU ON FR.Folio = FU.[from]
				
			DECLARE folio_cursor CURSOR FOR 
				SELECT [from], [to], folio, prID ,used
				FROM @tbRegFoliosXRango
	          
				OPEN folio_cursor
				FETCH NEXT FROM folio_cursor 
				INTO @fromFol, @toFol, @folioFol, @prIDFol, @usedFol
	          
			  --variables que sirven para saber los bloques de rangos
				DECLARE @fromPreviously int, @toPreviously int, @prIDPreviously VARCHAR(5)
				SELECT @fromPreviously = @fromFol,
						@toPreviously = @toFol,
						@newFrom = @fromFol,
						@newTo = @toFol,
						@numFolio = @folio,
						@prIDPreviously = @prIDFol
	          
				WHILE @@FETCH_STATUS = 0
				BEGIN
				IF(@prIDPreviously <> @prIDFol)
				BEGIN
					SELECT @fromPreviously = @fromFol,
							@toPreviously = @toFol,
							@newFrom = @fromFol,
							@newTo = @toFol,
							@numFolio = @folio,
							@prIDPreviously = @prIDFol
				END
	            
				--REVISAMOS QUE EL FROM Y TO DE CADA BLOQUE DE RANGOS NO CAMBIE, SI ES ASI SE INSERTA UN NUEVO RANGO EN LA TABLA
				IF(@fromPreviously <> @fromFol AND @toPreviously <> @toFol)
				BEGIN
					INSERT INTO @tbNewRangeFolios([from],[To], prID) 
						VALUES(@newFrom, @newTo, @prID)
					SET @fromPreviously = @fromFol
					SET @toPreviously = @toFol
					SET @newFrom  = @fromFol
					SET @prIDPreviously = @prIDFol
				END
	            
				IF @usedFol = 1
				BEGIN
					IF @folioFol <> @newFrom
					BEGIN
						INSERT INTO @tbNewRangeFolios([from],[To],prID) 
							VALUES(@newFrom, @folioFol -1 , @prID)
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
				INTO @fromFol, @toFol, @folioFol, @prIDFol, @usedFol
			END
	        
	        IF @fromFol <> @folioFol AND @usedFol <> 1	
	        BEGIN        
	        	INSERT INTO @tbNewRangeFolios([from],[To],prID) 
					VALUES(@newFrom, @newTo, @prID)
	        END
						
			DELETE FROM @tbRegFoliosXRango
			
		CLOSE folio_cursor
		DEALLOCATE folio_cursor
	    
		
		FETCH NEXT FROM range_cursor 
		INTO @from, @to, @prID
	  END 
	CLOSE range_cursor;
	DEALLOCATE range_cursor;
	
	INSERT INTO @RangeUsedFolios
	SELECT DISTINCT * 
	FROM @tbNewRangeFolios
	ORDER BY prID,[from]
	
	RETURN 
END

GO


