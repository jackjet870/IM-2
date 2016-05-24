USE [OrigosVCPalace]
GO
/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPercentageSalesman]    Script Date: 05/05/2016 17:11:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Autor:		William Jesús Torres Flota
-- Fecha:		07/Sep/2009
-- Descripción:	Obtiene el porcentaje que le corresponde a un vendedor
-- 
-- =============================================
/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene el porcentaje que le corresponde a un vendedor
** 
** [wtorres]	07/Sep/2009 Creado
** [edgrodriguez] 05/05/2016 Modificado. Ahora recibe 5 vendedores.
**
*/
ALTER function [dbo].[UFN_OR_GetPercentageSalesman](
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


