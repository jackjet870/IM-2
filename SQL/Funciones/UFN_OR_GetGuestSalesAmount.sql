if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetGuestSalesAmount]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetGuestSalesAmount]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de ventas de un huesped
** 
** [wtorres]	22/Jun/2011 Creado
** [wtorres]	19/Nov/2013 Ahora los downgrades se cuentan en base a la categoria de tipo de venta
**
*/
create function [dbo].[UFN_OR_GetGuestSalesAmount](
	@Guest int	-- Clave del huesped
)
returns money
as
begin

declare @Result money

select @Result = IsNull(Sum(S.saGrossAmount), 0)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
where
	-- Huesped
	S.sagu = @Guest
	-- Procesable
	and S.saProc = 1
	-- No downgrades
	and ST.ststc <> 'DG'
	-- No cancelada
	and S.saCancel = 0

return @Result
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

