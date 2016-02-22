if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[StringToOration]') and xtype in (N'FN', N'if', N'TF'))
drop function [dbo].[StringToOration]
GO

set QUOTED_IDENTIFIER ON 
GO
set ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Convierte una cadena a tipo oracion (la primera letra a mayuscula)
**
** [wtorres]	24/Ene/2014	Created
*/

create function [dbo].[StringToOration] (
	@String varchar(max)
)
returns varchar(max)
as  
begin

declare
	@NewString varchar(max),
	@Length int

set @Length = Len(@String)

if @Length = 0
	set @NewString = ''

else begin

	-- ponemos en mayuscula el primer caracter
	set @NewString = Upper(Substring(@String, 1, 1))

	if @Length > 1

		-- agregamos el resto de la cadena tal cual
		set @NewString = @NewString + Lower(Substring(@String, 2, @Length - 1))

end

-- establecemos respuesta de la funcion
return @NewString
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

