if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Trim]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[Trim]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Autor:		William Jesús Torres Flota
-- Función:		Trim
-- Fecha:		29/Dic/2008
-- Descripción:	Función que recibe una cadena y la devuelve sin espacios en blanco, 
--				en caso de existir, al principio y al final.
-- =============================================
CREATE FUNCTION [dbo].[Trim](
	@StringList varchar(4000))	-- Cadena
RETURNS varchar(4000)
AS
BEGIN
	-- Eliminamos espacios en blanco por la derecha y por la izquierda
	RETURN RTRIM( LTRIM ( @StringList ) )
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

