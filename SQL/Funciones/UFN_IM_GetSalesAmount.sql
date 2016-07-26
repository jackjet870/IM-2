/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el Sales Amount del monto de venta enviado 
** 
** [ecanul]	18/jul/2016 Created
**
*/
CREATE FUNCTION [dbo].[UFN_IM_GetSalesAmount](
	@GrossAmount money,			-- Gross Amount
	@Salesman1 varchar(10),		-- Clave del vendedor 1
	@Salesman2 varchar(10),		-- Clave del vendedor 2
	@Salesman3 varchar(10),		-- Clave del vendedor 3
	@Salesman4 varchar(10),		-- Clave del vendedor 4
	@Salesman5 varchar(10)		-- Clave del vendedor 5
)
RETURNS money
AS
BEGIN
SET @GrossAmount = @GrossAmount * dbo.UFN_OR_GetPercentageSalesman(@Salesman1,@Salesman2,@Salesman3,@Salesman4,@Salesman5);
RETURN @GrossAmount
END