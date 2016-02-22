if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[StringToTitle]') and xtype in (N'FN', N'if', N'TF'))
drop function [dbo].[StringToTitle]
GO

set QUOTED_IDENTIFIER ON 
GO
set ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Convierte una cadena a tipo titulo (la primera letra de cada palabra a mayuscula)
**
** [wtorres]	08/Oct/2012	Created
** [wtorres]	04/Jun/2014 Modified. Agregue los separadores: punto, coma y diagonal
**
*/
create function [dbo].[StringToTitle] (
	@String varchar(max)
)
returns varchar(max)
as  
begin

declare
	@Separators varchar(10),
	@NewString varchar(max),
	@Word varchar(max)

set @NewString = ''

-- validamos si tiene caracteres la cadena
if Len(@String) > 0 begin

	-- definimos la lista de separadores
	set @Separators = ' .,/'

	-- declaramos el cursor
	declare curWords cursor for
	select token from dbo.Separate(@String, @Separators, default)
	for read only
	
	-- abrimos el cursor
	open curWords

	-- buscamos el primer registro
	fetch next from curWords into @Word

	-- mientras haya mas registros
	while @@fetch_status = 0 begin
	
		-- ponemos en mayuscula el primer caracter de la palabra, si no es un separador
		if CharIndex(@Word, @Separators) = 0
			set @Word = dbo.StringToOration(@Word)
		
		set @NewString = @NewString + @Word

		-- buscamos el siguiente registro
		fetch next from curWords into @Word
	end

	-- cerramos y liberamos el cursor
	close curWords
	deallocate curWords
end

-- establecemos respuesta de la funcion
return @NewString
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

