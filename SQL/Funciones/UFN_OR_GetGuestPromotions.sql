if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetGuestPromotions]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetGuestPromotions]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene las promociones de un recibo de regalos
** 
** [wtorres]	19/Jun/2014 Created
**
*/
create function [dbo].[UFN_OR_GetGuestPromotions](
	@Receipt int	-- Clave del recibo de regalos
)
returns @Table table (
	Promotion varchar(20)
)
begin

insert @Table
select gpPromotion
from GuestsPromotions
where gpgr = @Receipt
order by gpPromotion

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

