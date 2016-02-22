if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetGuestPromotionsQuantity]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetGuestPromotionsQuantity]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene la cantidad de promociones de un recibo de regalos
**
** [wtorres]	19/Jun/2014 Creado
**
*/
create function [dbo].[UFN_OR_GetGuestPromotionsQuantity](
	@Receipt int	-- Clave del recibo de regalos
)
returns int
as
begin

declare @Quantity int

select @Quantity = Count(*)
from dbo.UFN_OR_GetGuestPromotions(@Receipt)

return @Quantity
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
