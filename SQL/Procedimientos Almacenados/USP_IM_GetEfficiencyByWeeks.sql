/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve un listado con fechas de Efficiency que esten dentro del mes seleccionado
**
** [ecanul] 25/07/2016 Created
**
*/

CREATE PROCEDURE dbo.USP_IM_GetEfficiencyByWeeks
		@SalesRoom varchar(5),			-- Sala de ventas
		@DateFrom datetime,				-- Fecha Inicio
		@DateTo datetime				-- Fecha Fin
AS
SELECT DISTINCT CAST(1 as BIT)AS Include, efDateFrom, efDateTo
FROM dbo.Efficiency
WHERE 
	--Sala de ventas
	efsr = @SalesRoom
	-- Año y mes inicial
	AND ( (YEAR(efDateFrom) = YEAR(@DateFrom) AND MONTH(efDateFrom) = MONTH(@DateFrom))
	-- Año y mes Final
	OR (YEAR(efDateTo) = YEAR(@DateTo) AND MONTH(efDateTo) = MONTH(@DateTo)))
	-- fecha final no sea mayor que la fecha final cerrada
	AND efDateTo < @DateTo
ORDER BY efDateFrom