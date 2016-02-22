if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetDateGiftWarehouseMovementsQuantity]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetDateGiftWarehouseMovementsQuantity]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve la cantidad de movimientos de almacen por fecha y regalo
** 
** [wtorres]	22/Dic/2010 Creado
** [wtorres]	07/Oct/2013 Agregue el parametro @Gifts
**
*/
create function [dbo].[UFN_OR_GetDateGiftWarehouseMovementsQuantity](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@Warehouse varchar(10),			-- Clave del almacen
	@Gifts varchar(max) = 'ALL',	-- Claves de regalos
	@In bit							-- Indica si se desean las entradas (1) o las salidas (0)
)
returns @Table table (
	[Date] datetime,
	Gift varchar(10),
	Quantity int
)
as
begin

insert @Table
select 
	M.wmD,
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
	-- Entradas
	and ((@In = 1 and M.wmQty > 0)
	-- Salidas
	or (@In = 0 and M.wmQty < 0))
group by M.wmD, M.wmgi

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
