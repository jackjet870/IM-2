if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetGiftWarehouseMovementsQuantity]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetGiftWarehouseMovementsQuantity]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve la cantidad de movimientos de almacen por regalo
** 
** [wtorres]	31/Dic/2010 Creado
** [wtorres]	07/Oct/2013 Agregue el parametro @Gifts
**
*/
create function [dbo].[UFN_OR_GetGiftWarehouseMovementsQuantity](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@Warehouse varchar(10),		-- Clave del almacen
	@Gifts varchar(max) = 'ALL'	-- Claves de regalos
)
returns @Table table (
	Gift varchar(10),
	Quantity int
)
as
begin

insert @Table
select 
	M.wmgi,
	Sum(M.wmQty)
from WhsMovs M
	inner join Gifts G on M.wmgi = G.giID
where
	-- Fecha de movimiento
	M.wmD between @DateFrom and @DateTo 
	-- Almacen
	and M.wmwh = @Warehouse
	-- Regalos
	and (@Gifts = 'ALL' or M.wmgi in (select item from split(@Gifts, ',')))
	-- Regalo inventariable
	and G.giInven = 1
group by M.wmgi

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
