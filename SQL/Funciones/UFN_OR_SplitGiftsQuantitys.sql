if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_SplitGiftsQuantitys]') and xtype in (N'FN', N'if', N'TF'))
drop function [dbo].[UFN_OR_SplitGiftsQuantitys]
GO

set QUOTED_IDENTIFIER ON 
GO
set ANSI_NULLS ON 
GO

-- =============================================
-- Función:			Split Gifts Quantity
-- Descripción:		Parsea una lista de regalos y una lista de cantidades y devuelve una tabla con los regalos y cantidades
-- Histórico:		[wtorres] 26/Nov/2009 Creado
-- =============================================
create function [dbo].[UFN_OR_SplitGiftsQuantitys](
	@List varchar(8000)	-- Lista de cantidades y claves de regalos
)
returns @Table table (
	Gift varchar(10),
	Quantity int
)
as
begin

declare
	@Pos int,				-- Posición del separador
	@Token varchar(8000),	-- Token
	@Gift varchar(10),		-- Clave del regalo
	@Quantity int			-- Cantidad de regalos

while Len(@List) > 0
begin
	-- Localiza el separador
	set @Pos = CharIndex(',', @List)
	-- Si encontró el separador
	if @Pos > 0
	begin
		set @Token = Left(@List, @Pos - 1)
		-- Elimina el token de la lista	
		set @List = Right(@List, Len(@List) - @Pos)
	end
	else
	begin
		set @Token = @List
		-- Limpia la lista	
		set @List = ''
	end
	-- Localiza el separador interno
	set @Pos = CharIndex('-', @Token)
	set @Quantity = Left(@Token, @Pos - 1)
	set @Gift = Right(@Token, Len(@Token) - @Pos)
	-- Agrega el regalo a la tabla
	insert into @Table values(@Gift, @Quantity)	
end

return
end

GO
set QUOTED_IDENTIFIER OFF 
GO
set ANSI_NULLS ON 
GO

