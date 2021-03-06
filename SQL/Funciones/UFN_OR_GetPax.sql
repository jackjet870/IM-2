if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetPax]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetPax]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene el pax dado un número de adultos y menores
** 
** [wtorres]	08/Feb/2010 Creado
** [wtorres]	16/Dic/2010 Aumenté el tamaño del valor devuelto
**
*/
create function [dbo].[UFN_OR_GetPax](
	@Adults int,	-- Número de adultos
	@Minors int		-- Número de menores
)
returns varchar(11)
as
begin
declare @Pax varchar(11)

set @Pax = Cast(@Adults as varchar(5)) + '.' + Cast(@Minors as varchar(5))

return @Pax
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

