if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_IM_GetSalesmanTypesCloser]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_IM_GetSalesmanTypesCloser]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Determina los tipos de un vendedor Closer (Own, As Front To Back, With Junior)
** 
** [aalcocer]	12/Jul/2016 Creado
**
*/

create function [dbo].[UFN_IM_GetSalesmanTypesCloser](
	@Role varchar (10),			-- Clave de rol
	@Sold bit,				-- Indica si vendio	
	@Own bit					-- Indica si trabajo solo
)
returns varchar(5)
as
BEGIN
	DECLARE @Result varchar(50)
	
	SET @Result = CASE @Role
					WHEN 'Liner' THEN CASE 					
						WHEN @own=1 THEN 'AS'
						WHEN @sold=1 THEN 'WITH'						
						ELSE 'AS'
					END
					WHEN 'Closer' THEN 'OWN'						
					WHEN 'Exit' THEN CASE					
						WHEN @sold=1 THEN 'OWN'
					END
				END
	return @Result
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

	
	
	
	