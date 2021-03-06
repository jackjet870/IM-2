if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptGiftsKardex]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptGiftsKardex]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de kardex de regalos
** 
** [wtorres]	22/Dic/2010 Modified. Depurado
** [wtorres]	07/Oct/2013 Modified. Agregue el parametro @Gifts
** [wtorres]	09/Abr/2014 Modified. Ahora agrega a la consulta de regalos, aquellos regalos de los movimientos al inventario y recibos de
**							regalos que no estan en el primer mes.
**							Elimine las variables tabla @tbl_WarehouseMovementsPrevious y @tbl_GiftsReceiptsPrevious
**
*/
create procedure [dbo].[USP_OR_RptGiftsKardex]
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@Warehouse varchar(10),		-- Clave del almacen
	@Gifts varchar(max) = 'ALL'	-- Claves de regalos
as
set nocount on

declare
	@FirstMonth datetime,		-- Sirve para ir al primer dia del mes de la fecha inicial
	@LastDayPrevious datetime,	-- Sirve para ir a un dia antes de la fecha inicial
	@TemporalInventory bit		-- indicamos que se genero el inventario temporal del primer mes

-- Tabla de inventario
declare @Inventory table (
	InvGi varchar(10),
	InvQty int
)

-- =============================================
--					Inventario
-- =============================================

--			Inventario del mes inicial
-- =============================================
-- generamos el inventario del mes inicial si no existe

-- obtenemos el primer dia del mes de la fecha inicial
set @FirstMonth = DateAdd(Day, Day(@DateFrom) * -1 + 1, @DateFrom)

-- si no existe el inventario del mes inicial
if (select Count(*) from GiftsInventory where gvD = @FirstMonth and gvwh = @Warehouse) = 0
begin

	-- indicamos que se genero el inventario temporal del primer mes
	set @TemporalInventory = 1

	-- generamos el inventario temporal del primer mes
	exec USP_OR_GetMonthInventory @Warehouse, @FirstMonth, @DateTo
end
else
	-- indicamos que no se genero el inventario temporal del primer mes
	set @TemporalInventory = 0

-- si la fecha inicial no es el primer dia del mes
if (Day(@DateFrom)) > 1
begin

	-- Inventario hasta un dia antes de la fecha inicial
	-- =============================================

	-- obtenemos la fecha de un dia antes de la fecha inicial
	set @LastDayPrevious = DateAdd(Day, -1 , @DateFrom)

	-- Inventario del primer mes
	insert into @Inventory
	select Gift, Quantity from UFN_OR_GetGiftsInventory(@FirstMonth, @Warehouse, @Gifts)

	-- Movimientos de almacen anteriores
	-- =============================================
	select Gift, Quantity
	into #WarehouseMovementsPrevious
	from UFN_OR_GetGiftWarehouseMovementsQuantity(@FirstMonth, @LastDayPrevious, @Warehouse, @Gifts)
	
	-- actualizamos el inventario, sumandole los movimientos de almacen anteriores
	update @Inventory
	set	InvQty = InvQty + M.Quantity
	from @Inventory I
		inner join #WarehouseMovementsPrevious M on M.Gift = I.InvGi

	-- agregamos los movimientos de almacen anteriores de los regalos inexistentes en el inventario
	insert into @Inventory
	select
		Gift,
		case when Quantity > 0 then Quantity else Quantity * -1 end
	from #WarehouseMovementsPrevious
	where
		-- Regalo que no existe en el inventario
		not exists (select InvGi from @Inventory)

	-- Recibos de regalos anteriores
	-- =============================================
	select Gift, Quantity
	into #GiftsReceiptsPrevious
	from UFN_OR_GetGiftGiftsReceiptsQuantity(@FirstMonth, @LastDayPrevious, @Warehouse, default, @Gifts)
	
	-- actualizamos el inventario, restandole los recibos anteriores
	update @Inventory
	set	InvQty = InvQty - R.Quantity
	from @Inventory I
		inner join #GiftsReceiptsPrevious R on R.Gift = I.InvGi

	-- agregamos los recibos anteriores de los regalos inexistentes en el inventario
	insert into @Inventory
	select 
		Gift, 
		case when Quantity > 0 then Quantity * -1 else Quantity end
	from #GiftsReceiptsPrevious
	where
		-- Regalo que no existe en el inventario
		not exists (select InvGi from @Inventory)
end

-- si la fecha inicial es el primer dia del mes
else
begin

	-- Inventario al primer dia del mes
	-- =============================================
	insert into @Inventory
	select Gift, Quantity from UFN_OR_GetGiftsInventory(@DateFrom, @Warehouse, @Gifts)
end

-- eliminamos los inventarios temporales, si se generaron
if @TemporalInventory = 1
	delete from GiftsInventory
	where gvwh = @Warehouse 
		and gvD >= (select srGiftsRcptCloseD from SalesRooms where srwh = @Warehouse)

-- Movimientos de almacen
-- =============================================
select * into #WarehouseMovements from (
	-- Entradas
	select [Date] as MovD, Gift as MovGi, Quantity as MovQty
	from UFN_OR_GetDateGiftWarehouseMovementsQuantity(@DateFrom, @DateTo, @Warehouse, @Gifts, 1)
	-- Salidas
	union all
	select [Date], Gift, Quantity
	from UFN_OR_GetDateGiftWarehouseMovementsQuantity(@DateFrom, @DateTo, @Warehouse, @Gifts, 0)
) as D

-- 5. Recibos de regalos
-- =============================================
select * into #GiftsReceipts from (
	-- Salidas
	select [Date] as RecD, Gift as RecGi, Quantity as RecQty
	from UFN_OR_GetDateGiftGiftsReceiptsQuantity(@DateFrom, @DateTo, @Warehouse, @Gifts, 1)
	-- Devoluciones
	union all
	select [Date], Gift, Quantity
	from UFN_OR_GetDateGiftGiftsReceiptsQuantity(@DateFrom, @DateTo, @Warehouse, @Gifts, 0)
) as D

-- 1. Indica si la sala de ventas ya cerro sus recibos de regalos hasta la fecha final
-- =============================================
select
	Cast(case when srGiftsRcptCloseD >= @DateTo then 1 else 0 end as bit) as GiftsClosed
from SalesRooms 
where srID = @Warehouse

-- 2. Regalos
-- =============================================
select G.giID, G.giN
from (
	select InvGi as Gift from @Inventory
	union select MovGi from #WarehouseMovements
	union select RecGi from #GiftsReceipts
) as D
	left join Gifts G on G.giID = D.Gift
order by G.giID

-- 3. Inventario
-- =============================================
select * from @Inventory order by InvGi

-- 4. Movimientos de almacen
-- =============================================
select * from #WarehouseMovements order by MovD, MovGi

-- 5. Recibos de regalos
-- =============================================
select * from #GiftsReceipts order by RecD, RecGi

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

