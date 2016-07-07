if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_IM_GetSalesmanTypeBySegments]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_IM_GetSalesmanTypeBySegments]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene el tipo de un vendedor en el reporte de estadisticas por segmentos
** 
** [aalcocer]	17/Jun/2016 Creado
**
*/

create function [dbo].[UFN_IM_GetSalesmanTypeBySegments](
	@SalesmanID varchar(10),	-- Clave de un vendedor
	@PostID varchar(10),		-- Clave del puesto	
	@Role varchar (50),			-- Clave de rol
	@SelfGen bit				-- marcado como Self Gen
)
returns varchar(50)
as
BEGIN
	DECLARE @Result varchar(50)
	
	--si no tiene puesto
	IF (@PostID = 'NP') SET @Result = 'NO POST'
	ELSE BEGIN	
		IF(dbo.UFN_IM_IsSelfGen(@SalesmanID,@Role,@SelfGen) = 1) SET @Result = 'Front To Middle'
		ELSE BEGIN
			DECLARE @PostName varchar(50)
			SELECT @PostName = poN FROM Posts where poID = @PostID
			-- si es un regen
			IF (@PostID = 'REGEN') SET @Result = @PostName
			ELSE BEGIN			
				SET @Result = CASE @Role
					WHEN 'Liner' THEN CASE @PostID					
						WHEN 'LINER' THEN @PostName
						WHEN 'FTM' THEN @PostName
						WHEN 'CLOSER' THEN @PostName + ' As Front To Back'
						WHEN 'EXIT' THEN @PostName + ' As Front To Back'
						ELSE 'Front To Back'
					END
					WHEN 'Closer' THEN CASE @PostID					
						WHEN 'CLOSER' THEN @PostName
						WHEN 'EXIT' THEN @PostName
						ELSE @postName + ' As Closer'						
					END
					WHEN 'Exit' THEN CASE @PostID					
						WHEN 'EXIT' THEN @PostName					
						ELSE @PostName + ' As Exit Closer'
					END
				END
			END
		END
	END
	return @Result
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO