if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_IM_IsSelfGen]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_IM_IsSelfGen]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Determina si un personal es Self Gen
** 
** [aalcocer]	25/Jun/2016 Creado
**
*/
create function [dbo].[UFN_IM_IsSelfGen](
	@SalesmanID varchar(10),	-- Clave de un vendedor
	@Role varchar (50),			-- Clave de rol
	@SelfGen bit				-- marcado como Self Gen	
)
returns bit
AS
BEGIN
	DECLARE @IsSelfGen bit 
	-- si desempeña el rol de liner
	IF(@Role = 'Liner') BEGIN
		-- si la Hostess lo marco como Self Gen
		IF(@SelfGen = 1) SET @IsSelfGen = 1	
		-- si es un Front To Middle			
		ELSE SELECT @IsSelfGen = CAST(count(peID) AS bit) FROM Personnel where peLinerID = @SalesmanID 
	END	
RETURN @IsSelfGen
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO




				