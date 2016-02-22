if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetGiftsInventory]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetGiftsInventory]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el inventario de regalos de un alamcen y un mes
** 
** [wtorres]	31/Dic/2010 Creado
** [wtorres]	07/Oct/2013 Agregue el parametro @Gifts
**
*/
create function [dbo].[UFN_OR_GetGiftsInventory](
	@Date datetime,				-- Fecha
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
	I.gvgi,
	I.gvQty
from GiftsInventory I
	inner join Gifts G on I.gvgi = G.giID
where
	-- Fecha del inventario
	I.gvD = @Date
	-- Almacen
	and I.gvwh = @Warehouse
	-- Regalos
	and (@Gifts = 'ALL' or I.gvgi in (select item from split(@Gifts, ',')))
order by I.gvgi

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
