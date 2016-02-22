if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Pad]') and xtype in (N'FN', N'if', N'TF'))
drop function [dbo].[Pad]
GO

set QUOTED_IDENTIFIER ON 
GO
set ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Funcion para aplicar un formato de relleno a un texto con los parametros especificados.
** Parametros:
**		@String		Texto para aplicar formato
**		@Char		Caracter de relleno
**		@Length		Longitud final del texto
**		@Direction	Sentido o direccion de formato 0 = Izquierda, 1 = Derecha y 2 = Ambos sentidos.
**
** [wgonzalez]	13/08/2009	Created
*/

create function [dbo].[Pad] (
	@String varchar(100),
	@Char char(1),
	@Length int,
	@Direction int = 0
)
returns varchar(100)
as  
begin

-- Variables
declare @CurrentLength int, @CharLength int

-- Incializamos las variables
select @String = dbo.Trim(@String),
	@CurrentLength = Len(@String),
	@CharLength = @Length - @CurrentLength

-- Izquierda
if @Direction = 0

	-- Preparamos la cadena de respuesta
	set @String = Replicate(@Char, @CharLength) + @String

-- Derecha
else if @Direction = 1

	-- Preparamos la cadena de respuesta
	set @String = @String + Replicate(@Char, @CharLength)

-- Ambos
else
begin

	-- Variables
	declare @Complement varchar(100)

	-- Inicializamos las variables
	select @Complement = Replicate(@Char, @CharLength / 2)

	-- Preparamos la cadena de respuesta
	set @String = @Complement + @String + @Complement
	
	-- Si la longitud final no es igual a la especificada
	if Len(@String) != @Length
		
		-- Completamos una vez mas
		set @String = @Char + @String

end

-- Establecemos repuesta de la funcion
return @String
end