if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddString]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[AddString]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Agrega una cadena a una lista separada por un caracter
**
** [wtorres]	07/Mar/2009 Created
** [wtorres]	13/Nov/2009 Modified. Ahora ya no agrega cadenas vacias
**
*/
create function [dbo].[AddString](
	@StringList varchar(8000),		-- Lista de cadenas
	@String varchar(8000),			-- Cadena
	@Separator varchar(8000) = ','	-- Separador
)
returns varchar(8000)
as
begin
declare @List varchar(8000)

-- inicializamos la lista de cadenas
if @StringList <> ''
	set @List = @StringList
else
	set @List = ''
	
-- si la cadena no es vacia
if @String <> '' begin

	-- si la lista no esta vacia
	if @List <> ''
	
		-- agregamos el separador
		set @List = @List + @Separator
		
	-- agregamos la cadena
	set @List = @List + @String
end

return @List
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

