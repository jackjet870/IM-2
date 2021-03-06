if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Separate]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[Separate]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Separa una cadena en tokens, en base a una lista de separadores
**
** [wtorres]	29/Dic/2008	Created
**
*/
create function [dbo].[Separate](
	@String varchar(max),		-- Cadena
	@Separators varchar(100),	-- Lista de sepadores
	@IncludeSeparators bit = 1	-- Indica si los separadores se deben incluir en la lista de tokens
)
returns @TableTokens table (
	token varchar(max)
)
as
begin

declare
	@StringList varchar(max),		-- Cadena auxiliar
	@SeparatorsList varchar(100),	-- Lista de separadores auxiliar
	@Separator varchar(1),			-- Separador
	@Pos int,						-- Posicion de un separador
	@PosMin int,					-- Posicion del primer separador en la cadena
	@Token varchar(max)				-- Token

-- Tabla de posiciones de los separadores dentro de la cadena
declare @TablePos table(
	Pos int
)

set @StringList = @String

-- recorremos los caracteres de la cadena
while @StringList <> '' begin

	-- recorremos los separadores
	set @SeparatorsList = @Separators
	while @SeparatorsList <> '' begin
	
		-- obtenemos el separador
		set @Separator = Left(@SeparatorsList, 1)
		
		-- eliminamos el separador de la lista de separadores
		set @SeparatorsList = Right(@SeparatorsList, Len(@SeparatorsList) - 1)
		
		-- obtenemos la posicion del separador
		set @Pos = CharIndex(@Separator, @StringList)
		
		-- si encontro el separador
		if @Pos > 0
		
			-- agregamos la posicion del separador a la tabla de posiciones
			insert into @TablePos values(@Pos)
	end
	
	-- obtenemos la posicion minima
	select @PosMin = IsNull(Min(Pos), 0) from @TablePos
	
	-- limpiamos la tabla de posiciones
	delete from @TablePos		
	
	-- si encontro un separador
	if @PosMin > 0 begin
	
		-- obtenemos el token
		set @Token = SubString(@StringList, 1, @PosMin - 1)
		
        -- agregamos el token a la tabla de tokens
		if @Token <> ''
			insert into @TableTokens values(@Token)
			
		-- si se deben incluir los separadores
		if @IncludeSeparators = 1 begin
		
			-- obtenemos el separador
			set @Separator = SubString(@StringList, @PosMin, 1)
			
			-- agregamos el separador a la tabla de tokens
			insert into @TableTokens values(@Separator)
		end
		
        -- eliminamos el token y el separador de la cadena
        set @StringList = Substring(@StringList, @PosMin + 1, Len(@StringList) - @PosMin)
	end
	
	-- si no encontro un separador
	else begin
	
		-- obtenemos el ultimo token
		set @Token = @StringList
		
		-- agregamos el ultimo token
		insert into @TableTokens values(@Token)
		
		-- limpiamos la cadena
		set @StringList = ''
	end
end	

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

