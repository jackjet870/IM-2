if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetPercentageSalesman]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetPercentageSalesman]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Autor:		William Jesús Torres Flota
-- Fecha:		07/Sep/2009
-- Descripción:	Obtiene el porcentaje que le corresponde a un vendedor
-- =============================================
create function [dbo].[UFN_OR_GetPercentageSalesman](
	@Salesman1 varchar(10),	-- Clave del vendedor 1
	@Salesman2 varchar(10),	-- Clave del vendedor 2
	@Salesman3 varchar(10)	-- Clave del vendedor 3
)
returns money
as
begin
declare
	@Percentage money,	-- Porcentaje
	@NumSalesmen int	-- Número de vendedores

set @NumSalesmen = (case when @Salesman1 is not null then 1 else 0 end)
				 + (case when @Salesman2 is not null then 1 else 0 end)
				 + (case when @Salesman3 is not null then 1 else 0 end)
set @Percentage = dbo.UFN_OR_SecureDivision(1.0, @NumSalesmen)

return @Percentage
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

