if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Split]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[Split]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
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

create FUNCTION dbo.Split( @String varchar(max), @Delimiter char(1) )

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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

