if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_SecureDivision]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_SecureDivision]
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
CREATE FUNCTION [dbo].[UFN_OR_SecureDivision](
	@Dividend money,	-- Dividendo
	@Divisor money)		-- Divisor
RETURNS money
AS
BEGIN
declare @Result money

if @Divisor <> 0
	set @Result = @Dividend / @Divisor
else
	set @Result = 0

RETURN @Result
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

