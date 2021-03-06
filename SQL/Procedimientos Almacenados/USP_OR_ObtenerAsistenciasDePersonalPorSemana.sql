if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_ObtenerAsistenciasDePersonalPorSemana]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_ObtenerAsistenciasDePersonalPorSemana]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Autor:			William Jesús Torres Flota
-- Procedimiento:	Obtener asistencias de personal por semana
-- Fecha:			06/Mar/2009
-- Descripción:		Obtiene el número de asistencias de un personal en una semana.
--					Si no tiene definido las asistencias devuelve -1
-- =============================================
CREATE PROCEDURE [dbo].[USP_OR_ObtenerAsistenciasDePersonalPorSemana]
	@SalesRoom varchar(10),	-- Clave de la sala de ventas
	@Personnel varchar(10),	-- Clave del personal
	@DateFrom datetime,		-- Fecha de inicio de la semana
	@DateTo datetime		-- Fecha de fin de la semana
AS
	SET NOCOUNT ON;

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
else
	set @NumAssistance = -1
-- Devuelve el número de asistencias
select @NumAssistance as NumAssistance

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

