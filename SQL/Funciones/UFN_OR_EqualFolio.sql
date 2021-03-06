if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_EqualFolio]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_EqualFolio]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Determina si un folio de invitacion outhouse corresponde a una serie y numero indicados
** 
** [wtorres]	26/Feb/2015 Created
**
*/
create function [dbo].[UFN_OR_EqualFolio](
	@Folio varchar(8),
	@Serie varchar(5),
	@Number int
)
returns bit
as
begin
declare
	@Equal bit,
	@Pos int,
	@SerieFolio varchar(8),
	@NumberFolio varchar(8)



set @Equal = 0

-- obtenemos la posicion del separador
set @Pos = CharIndex('-', @Folio)
	
-- si tiene separador
if @Pos > 0 begin
	
	-- obtenemos la serie
	set @SerieFolio = SubString(@Folio, 1, @Pos - 1)
	
	-- obtenemos el numero
	set @NumberFolio = SubString(@Folio, @Pos + 1, Len(@Folio) - @Pos)
	
	-- validamos que el numero en verdad sea numerico
	if dbo.IsNumber(@NumberFolio) = 1 begin
	
		-- determinamos si el folio coincide con la serie y numero indicados
		if @SerieFolio = @Serie and Cast(@NumberFolio as int) = @Number
			set @Equal = 1
	end
end /*else
	return Cast('The folio ' + @Folio +  ' is not in the correct format' as int)*/

return @Equal
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

