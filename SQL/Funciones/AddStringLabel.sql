if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddStringLabel]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[AddStringLabel]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Función:		AddStringLabel
-- Descripción:	Agrega una cadena a una lista separada por un caracter
-- Histórico:	[wtorres] 19/Nov/2009 Creado
-- =============================================
create function [dbo].[AddStringLabel](
	@StringList varchar(8000),		-- Lista de cadenas
	@String varchar(8000),			-- Cadena
	@Separator varchar(8000) = ',',	-- Separador
	@Label varchar(8000) = ''		-- Etiqueta inicial
)
returns varchar(8000)
as
begin
declare @List varchar(8000)

-- Inicializa la lista de cadenas
if @StringList <> ''
	set @List = @StringList
else
	set @List = ''
-- Si la cadena no es vacía
if @String <> ''
begin
	-- Si la lista está vacía
	if @List <> ''
		-- Agrega el separador
		set @List = @List + @Separator
	-- Si se envío una etiqueta de inicio
	if @Label <> ''
		-- Agrega la etiqueta
		set @List = @List + @Label
	-- Agrega la cadena
	set @List = @List + @String
end

return @List
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

