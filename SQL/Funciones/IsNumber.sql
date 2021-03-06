if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IsNumber]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[IsNumber]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Autor:		William Jesús Torres Flota
-- Función:		IsNumber
-- Fecha:		30/Dic/2008
-- Descripción:	Determina si una cadena representa un número (sin comas ni puntos)
-- =============================================
CREATE FUNCTION [dbo].[IsNumber](
	@String varchar(4000))	-- Cadena
RETURNS bit
AS
BEGIN
declare @IsNumber bit				-- Indica si la cadena representa un número
declare @StringList varchar(4000)	-- Cadena auxiliar
declare @Char varchar(1)			-- Caracter

set @IsNumber = 1
set @StringList = [dbo].[Trim](@String)
if @StringList <> ''
begin
	-- Recorre los caracteres de la cadena
	while @StringList <> '' and @IsNumber = 1
	begin
		-- Obtiene el caracter
		select @Char = left(@StringList, 1)
		-- Elimina el caracter de la cadena
		select @StringList = Right(@StringList, Len(@StringList) - 1)
		select @IsNumber = 
			case when @Char in ('1', '2', '3', '4', '5', '6', '7', '8', '9', '0') then 1 else 0 end
	end
end
else
	set @IsNumber = 0
RETURN @IsNumber
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

