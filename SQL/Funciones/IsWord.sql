if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IsWord]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[IsWord]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Autor:		William Jesús Torres Flota
-- Función:		IsWord
-- Fecha:		30/Dic/2008
-- Descripción:	Determina si una cadena representa una palabra
-- =============================================
CREATE FUNCTION [dbo].[IsWord](
	@String varchar(4000))	-- Cadena
RETURNS bit
AS
BEGIN
declare @IsWord bit					-- Indica si la cadena representa una palabra
declare @StringList varchar(4000)	-- Cadena auxiliar
declare @Char varchar(1)			-- Caracter

set @IsWord = 1
set @StringList = [dbo].[Trim](@String)
if @StringList <> ''
begin
	-- Obtiene el primer caracter
	select @Char = left(@StringList, 1)
	select @IsWord = 
		case when Lower(@Char) in (
			'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
			'k', 'l', 'm', 'n', 'ñ', 'o', 'p', 'q', 'r', 's',
			't', 'u', 'v', 'w', 'x', 'y', 'z') 
		then 1 else 0 end
end
else
	set @IsWord = 0
RETURN @IsWord
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

