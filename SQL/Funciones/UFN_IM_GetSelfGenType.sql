USE [OrigosVCPalace]
GO
/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetSelfGenType]    Script Date: 07/22/2016 11:28:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el tipo de SelfGen de un Personnel (para el reporte SelfGen & SelfGen Team) 
**						Retutn 1.- Selfgen	0.- Selfgen Team
** 
** [ecanul]	15/jul/2016 Created
**
*/
ALTER FUNCTION [dbo].[UFN_IM_GetSelfGenType](
	@PersonnelID varchar(10),	-- Clave del Personal
	@PR1 varchar(10) = NULL,	-- Clave del PR1
	@PR2 varchar(10) = NULL,	-- Clave del PR2
	@PR3 varchar(10) = NULL		-- Clave del P3
)
RETURNS int
AS
BEGIN
DECLARE @Result int
-- Si no se envio ningun PR
IF @PR1 IS NULL AND @PR2 IS NULL AND @PR3 IS NULL
	-- Se determina si es un Front To Middle en general
	SELECT
		@Result = count(peID)
	FROM Personnel
	WHERE peLinerID = @PersonnelID
ELSE
	-- Si se envio la clave del PR1
	IF @PR1 IS NOT NULL
	BEGIN
		BEGIN
			SELECT @Result = Count(peID)
			FROM dbo.Personnel
			WHERE peLinerID = @PersonnelID AND peID = @PR1
			-- Si no se encontro la clave del FTM y se envio PR2
			IF @Result = 0 AND @PR2 IS NOT NULL
				BEGIN
					SELECT @Result = COUNT(peID)
					FROM dbo.Personnel
					WHERE peLinerID = @PersonnelID AND peID = @PR2
					-- Si no se encontro el FTM y se mando PR3
					IF @Result = 0 AND @PR3 IS NOT NULL
						SELECT @Result = COUNT(peID)
						FROM dbo.Personnel
						WHERE peLinerID = @PersonnelID AND peID = @PR3
				END
			END
		END
RETURN @Result
END