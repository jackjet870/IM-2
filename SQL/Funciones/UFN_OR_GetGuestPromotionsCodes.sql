if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetGuestPromotionsCodes]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetGuestPromotionsCodes]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene una cadena que contiene los codigos de promocion de un recibo de regalos
** 
** [wtorres]	19/Jun/2014 Created
**
*/
create function [dbo].[UFN_OR_GetGuestPromotionsCodes](
	@Receipt int	-- Clave del recibo de regalos
)
returns varchar(max)
as
begin

declare
	@List varchar(max),	-- Lista
	@Item varchar(20)	-- Elemento

-- declaramos el cursor
declare curPromotions cursor for
select Promotion from dbo.UFN_OR_GetGuestPromotions(@Receipt)

-- abrimos el cursor
open curPromotions

-- buscamos el primer registro
fetch next from curPromotions into @Item

-- mientras haya mas registros
while @@fetch_status = 0 begin

	-- agregamos el elemento a la lista
	set @List = dbo.AddString(@List, @Item, ', ')

	-- buscamos el siguiente registro
	fetch next from curPromotions into @Item
end

-- cerramos y liberamos el cursor
close curPromotions
deallocate curPromotions

return @List
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

