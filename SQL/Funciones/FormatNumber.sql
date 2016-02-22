if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FormatNumber]') and xtype in (N'FN', N'if', N'TF'))
drop function [dbo].[FormatNumber]
GO

set QUOTED_IDENTIFIER ON 
GO
set ANSI_NULLS ON 
GO

-- =============================================
-- Función:		Formatear número
-- Descripción:	Formatea un valor numérico
-- Histórico:	[wtorres] 13/Nov/2009 Creado
-- =============================================
create function [dbo].[FormatNumber](
	@Value sql_variant,	-- Valor a formatear
	@Decimals tinyint	-- Número de decimales
) 
returns varchar(8000)
as 
begin

-- Declaración de variables
declare
	@Result varchar(8000),	-- Valor formateado
	@Dot int,				-- Indica si tiene decimales
	@Length int,			-- Lonfitud del resultado
    @i int					-- Iterador

-- Si tiene decimales
if @Decimals > 0
	set @Dot = 1
else
	set @Dot = 0
-- Establece el número de decimales
if @Decimals = 0
	set @Result = Cast(@Value as decimal(26, 0))
else if @Decimals = 1
    set @Result = Cast(@Value as decimal(26, 1))
else if @Decimals = 2
    set @Result = Cast(@Value as decimal(26, 2))
else if @Decimals = 3
    set @Result = Cast(@Value as decimal(26, 3))
else
begin
    set @Result = Cast(@Value as decimal(26, 4))
	set @Decimals = 4
end

set @Length = Len(@Result) - @Decimals - @Dot
set @i  = 1
-- Agrega los separadores de millar
while 1 = 1
begin
	-- Si necesita agregar separador de millar
	if @Length > @i * 3
	begin
		set @Result = Stuff(@Result, @Length - 2 - ((@i - 1) * 3), 0, ',')
		set @i = @i + 1
	end
	-- Si no necesita agregar separador de millar
	else
		break
end

return @Result
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO