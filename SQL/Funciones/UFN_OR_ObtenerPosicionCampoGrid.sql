if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_ObtenerPosicionCampoGrid]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_ObtenerPosicionCampoGrid]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Autor:		William Jesús Torres Flota
-- Función:		Obtener posición de un campo de un grid
-- Fecha:		20/Nov/2008
-- Descripción:	Obtiene la posición de un campo de un layout de grid. Si no lo encuentra devuelve cero.
-- =============================================
CREATE FUNCTION [dbo].[UFN_OR_ObtenerPosicionCampoGrid](
	@Grid varchar(20),		-- Nombre del grid
	@FieldName varchar(20))	-- Nombre del campo
RETURNS int
AS
BEGIN
declare @ColPosition smallint
set @ColPosition = 0

select @ColPosition = fgColPosition
from FieldsByGrid
where fgGrid = @Grid and fgFieldName = @FieldName

RETURN @ColPosition
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

