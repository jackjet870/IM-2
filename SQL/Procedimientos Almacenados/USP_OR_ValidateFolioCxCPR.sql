
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/** Palace Resorts
** Grupo de Desarrollo Palace
**
[LChairezReload] 13/Enero/2015 Created, Valida que los folios CxC no se sobreescriban
*/
CREATE PROCEDURE [dbo].[USP_OR_ValidateFolioCxCPR]
	@PR varchar(10),
	@From int,
	@To int,
	@isCancel bit = 0
AS
BEGIN
	SET NOCOUNT ON
	
	DECLARE @bRes bit
	DECLARE @sPR varchar(150)
	SELECT @bRes = 0,@SPR = ''
	 	 
	--Si no es de cancelacion solo se valida en FolioCxCPR
	IF @isCancel = 0
	BEGIN
	   
		SELECT @sPR = por.fcppe
		FROM dbo.FolioCxCPR por
		WHERE por.fcppe <> @PR
		AND por.fcpFrom between @From AND @To
	    
		IF ISNULL(@sPR,'') <> ''
			SELECT @sPR = p.peID + ' - ' + p.peN,
				@bRes= 1
			FROM dbo.Personnel p
			WHERE p.peID = @spr
		ELSE
			SELECT @bRes = 0,@sPR =''   
		END
	ELSE
	BEGIN
	   
		SELECT @sPR = fc.fccpe
		FROM dbo.FolioCxCCancellation fc
		WHERE fc.fccpe <> @PR
		and fc.fccFrom between @From and @To
	 
		IF ISNULL(@SPR,'') <> ''
			SELECT @sPR = p.peID + ' - ' + p.peN,
				  @bRes= 1
			FROM dbo.Personnel p
			WHERE p.peID = @spr
		ELSE
			SELECT @bRes = 0,@sPR=''
	 END
	 
	 SELECT @bRes [Result],@sPR [PR]
END
GO
