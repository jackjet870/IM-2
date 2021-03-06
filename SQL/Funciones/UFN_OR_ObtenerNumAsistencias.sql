if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_ObtenerNumAsistencias]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_ObtenerNumAsistencias]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Autor:		William Jesús Torres Flota
-- Función:		Obtener número de asistencias
-- Fecha:		07/Mar/2009
-- Descripción:	Obtiene el número de asistencias de un personal
-- =============================================
CREATE FUNCTION [dbo].[UFN_OR_ObtenerNumAsistencias](
	@Monday varchar(10),	-- Asistencia en Lunes
	@Tuesday varchar(10),	-- Asistencia en Martes
	@Wednesday varchar(10),	-- Asistencia en Miércoles
	@Thursday varchar(10),	-- Asistencia en Jueves
	@Friday varchar(10),	-- Asistencia en Viernes
	@Saturday varchar(10),	-- Asistencia en Sábado
	@Sunday varchar(10)		-- Asistencia en Domingo
)
RETURNS tinyint
AS
BEGIN
declare @NumAssistance tinyint

set @NumAssistance = 0

if @Monday = 'A' or @Monday = 'L'
	set @NumAssistance = @NumAssistance + 1
if @Tuesday = 'A' or @Tuesday = 'L'
	set @NumAssistance = @NumAssistance + 1
if @Wednesday = 'A' or @Wednesday = 'L'
	set @NumAssistance = @NumAssistance + 1
if @Thursday = 'A' or @Thursday = 'L'
	set @NumAssistance = @NumAssistance + 1
if @Friday = 'A' or @Friday = 'L'
	set @NumAssistance = @NumAssistance + 1
if @Saturday = 'A' or @Saturday = 'L'
	set @NumAssistance = @NumAssistance + 1
if @Sunday = 'A' or @Sunday = 'L'
	set @NumAssistance = @NumAssistance + 1

RETURN @NumAssistance
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

