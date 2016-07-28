USE [OrigosVCPalace]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPercentageSalesman]    Script Date: 07/28/2016 13:18:17 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetPercentageSalesman]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetPercentageSalesman]
GO

USE [OrigosVCPalace]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPercentageSalesman]    Script Date: 07/28/2016 13:18:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene el porcentaje que le corresponde a un vendedor
** 
** [wtorres]		07/Sep/2009 Created
** [edgrodriguez]	05/May/2016 Modificado. Ahora recibe 5 vendedores.
**
*/
create function [dbo].[UFN_OR_GetPercentageSalesman](
	@Salesman1 varchar(10),	-- Clave del vendedor 1
	@Salesman2 varchar(10),	-- Clave del vendedor 2
	@Salesman3 varchar(10),	-- Clave del vendedor 3
	@Salesman4 varchar(10), -- Clave del vendedor 4
	@Salesman5 varchar(10)  -- Clave del vendedor 5
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
				 + (case when @Salesman4 is not null then 1 else 0 end)
				 + (case when @Salesman5 is not null then 1 else 0 end)
set @Percentage = dbo.UFN_OR_SecureDivision(1.0, @NumSalesmen)

return @Percentage
end
GO


