/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene el número de asistencias de un personal en una semana.
** Si no tiene definido las asistencias devuelve -1
**
** 
** [ecanul]  13/08/2016 Created
**
*/
CREATE FUNCTION [dbo].[UFN_IM_GetPersonnelAssistancesByWeek](
	@SalesRoom varchar(10),	-- Clave de la sala de ventas
	@Personnel varchar(10),	-- Clave del personal
	@DateFrom datetime,		-- Fecha de inicio de la semana
	@DateTo datetime		-- Fecha de fin de la semana
)
RETURNS int
AS
BEGIN
	declare
	@Count int,
	@NumAssistance int

	-- Determina si hay asistencias
	set @Count = (select count(aspe) from Assistance
		where asPlaceID = @SalesRoom and aspe = @Personnel and asStartD = @DateFrom and asEndD = @DateTo)
	-- Si hay asistencias
	if @Count = 1
	begin
		select @NumAssistance = asNum
		from Assistance
		where asPlaceID = @SalesRoom and aspe = @Personnel and asStartD = @DateFrom and asEndD = @DateTo
	end
	-- Si NO hay asistencias
	ELSE
		set @NumAssistance = -1
RETURN @NumAssistance
END