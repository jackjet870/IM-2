USE [OrigosVCPalace]
GO

/****** Object:  UserDefinedFunction [dbo].[Split]    Script Date: 07/23/2016 10:49:17 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Split]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[Split]
GO

USE [OrigosVCPalace]
GO

/****** Object:  UserDefinedFunction [dbo].[Split]    Script Date: 07/23/2016 10:49:17 ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER ON
GO




/*
** Palace Resorts
** Grupo de Desarrollo Merida
** 
** [wgonzalez]	12.05.2006
** [aalcocer]	10/Jun/2016 Se modifica el tipo de dato del campo @String a varchar(max)
**
** Implementación de la funcion SPLIT para devolver un arreglo o tabla
** con los valores recuperados.
*/

create FUNCTION [dbo].[Split]( @String varchar(max), @Delimiter char(1) )

RETURNS @Results TABLE (item varchar(8000))

AS

	BEGIN
		DECLARE @INDEX INT
		DECLARE @SLICE varchar(8000)

		SET @INDEX = 1

		IF @String IS NULL RETURN
	
		WHILE @INDEX !=0
		BEGIN	

			SET @INDEX = CHARINDEX( @Delimiter, @STRING )

			IF @INDEX !=0
				SET @SLICE = LEFT( @STRING, @INDEX - 1 )
			ELSE
				SET @SLICE = @STRING

			INSERT INTO @Results(item) VALUES( @SLICE )
			
			SET @STRING = RIGHT( @STRING, LEN(@STRING) - @INDEX )

			IF LEN( @STRING ) = 0 BREAK

		END -- WHILE
		
		RETURN

	END 




GO


