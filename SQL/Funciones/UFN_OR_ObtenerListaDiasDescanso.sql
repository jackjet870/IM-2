if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_ObtenerListaDiasDescanso]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_ObtenerListaDiasDescanso]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Autor:		William Jesús Torres Flota
-- Función:		Obtener lista de días de descanso
-- Fecha:		07/Mar/2009
-- Descripción:	Obtiene la lista de días de descanso de un personal
-- =============================================
CREATE FUNCTION [dbo].[UFN_OR_ObtenerListaDiasDescanso](
	@Monday bit,	-- Descanso en Lunes
	@Tuesday bit,	-- Descanso en Martes
	@Wednesday bit,	-- Descanso en Miércoles
	@Thursday bit,	-- Descanso en Jueves
	@Friday bit,	-- Descanso en Viernes
	@Saturday bit,	-- Descanso en Sábado
	@Sunday bit		-- Descanso en Domingo
)
RETURNS varchar(7)
AS
BEGIN
declare @List varchar(7)

set @List = ''

if @Monday = 1
	select @List = [dbo].[AddString](@List, 'MON', '/')
if @Tuesday = 1
	select @List = [dbo].[AddString](@List, 'TUE', '/')
if @Wednesday = 1
	select @List = [dbo].[AddString](@List, 'WED', '/')
if @Thursday = 1
	select @List = [dbo].[AddString](@List, 'THU', '/')
if @Friday = 1
	select @List = [dbo].[AddString](@List, 'FRI', '/')
if @Saturday = 1
	select @List = [dbo].[AddString](@List, 'SAT', '/')
if @Sunday = 1
	select @List = [dbo].[AddString](@List, 'SUN', '/')

RETURN @List
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

