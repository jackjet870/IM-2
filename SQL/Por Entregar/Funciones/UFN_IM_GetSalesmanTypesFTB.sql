if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_IM_GetSalesmanTypesFTB]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_IM_GetSalesmanTypesFTB]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Determina los tipos de un vendedor FTB (Own, With Closer o As Closer)
** 
** [michan]	21/Jul/2016 Creado
**
*/

create function [dbo].[UFN_IM_GetSalesmanTypesFTB](
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
						WHEN @own=1 AND @Sold = 1 THEN 'OWN'
						ELSE 'WITH'	
					END
					WHEN 'Closer' THEN 'AS'						
					WHEN 'Exit' THEN 'AS'
				END
	return @Result
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

	
	
	
	