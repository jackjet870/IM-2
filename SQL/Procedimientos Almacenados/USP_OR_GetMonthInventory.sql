if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetMonthInventory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetMonthInventory]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Genera el inventario mensual de un determinado mes
** 
** [wtorres]	27/Dic/2010 Depurado. Ahora usa variables tipo tabla en vez de variables temporales
**
*/
create procedure [dbo].[USP_OR_GetMonthInventory]
	@Warehouse as varchar(10),	-- Clave del almacen
	@Date datetime,				-- Fecha
	@DateTo datetime = null		-- Fecha hasta (solo se envia desde el reporte de Gifts Kardex)
as
set nocount on

declare
	@CurrentMonth datetime,			-- Mes actual
	@PreviousMonth datetime,		-- Mes anterior
	@NextMonth datetime,			-- Mes siguiente
	@LastDayPreviousMonth datetime,	-- Ultimo dia del mes anterior
	@LastDay datetime				-- Ultimo dia del mes actual

-- Tabla de inventario
declare @Inventory table (
	InvGi varchar(10),
	InvQty int
)

-- si no es el primer dia del mes, se obtiene el primer dia del mes
if Day(@Date) > 1
	set @CurrentMonth = DateAdd(Day, Day(@Date) * -1 + 1, @Date)
else
	set @CurrentMonth = @Date

-- si se envio la fecha hasta y coinciden el mes y el año, manejamos como fecha final la fecha hasta enviada
if @DateTo is not null and Year(@Date) = Year(@DateTo) and Month(@Date) = Month(@DateTo)
	set @LastDay = @DateTo

-- si no manejamos como fecha final el ultimo dia del mes
else begin
	set @NextMonth = DateAdd(m, 1, @CurrentMonth)
	set @LastDay = DateAdd(Day, -1 , @NextMonth)
end

-- obtenemos el mes anterior
set @PreviousMonth = DateAdd(m, -1, @CurrentMonth)

-- obtenemos el ultimo dia del mes anterior
set @LastDayPreviousMonth = DateAdd(Day, -1 , @CurrentMonth)

-- si no existe el inventario del mes anterior
if (select Count(*) from GiftsInventory where gvD = @PreviousMonth and gvwh = @Warehouse) = 0 begin

	-- si existe el inventario de meses anteriores al mes anterior
	if (select Count(*) from GiftsInventory where gvD < @PreviousMonth and gvwh = @Warehouse) > 0 begin

		-- generamos el inventario del mes anterior
		exec USP_OR_GetMonthInventory @Warehouse, @PreviousMonth, @DateTo
	end
end	

-- Inventario guardado del mes anterior
insert into @Inventory
select I.Gift, I.Quantity
from UFN_OR_GetGiftsInventory(@PreviousMonth, @Warehouse, default) I
	inner join Gifts G on G.giID = I.Gift
where
	-- Regalo inventariable
	G.giInven = 1
	-- Regalo activo
	and G.giA = 1

-- Regalos del catalogo de regalos que no estaban en el inventario del mes anterior
insert into @Inventory
select giID, 0
from Gifts
where
	-- Regalo inventariable
	giInven = 1
	-- Regalo activo
	and giA = 1
	-- Regalo que no existe en el inventario
	and giID not in (select InvGi from @Inventory)

-- Regalos de los movimientos al inventario que no estaban en el inventario del mes anterior
union
select Gift, 0
from UFN_OR_GetGiftWarehouseMovementsQuantity(@CurrentMonth, @LastDay, @Warehouse, default)
where
	-- Regalo que no existe en el inventario
	Gift not in (select InvGi from @Inventory)

-- Regalos de los recibos de regalos que no estaban en el inventario del mes anterior
union
select Gift, 0
from UFN_OR_GetGiftGiftsReceiptsQuantity(@CurrentMonth, @LastDay, @Warehouse, @Warehouse, default)
where
	-- Regalo que no existe en el inventario
	Gift not in (select InvGi from @Inventory)

-- actualizamos el inventario, sumandole los movimientos de almacen del mes
update @Inventory
set	InvQty = InvQty + M.Quantity
from @Inventory I
	inner join (
		select Gift, Quantity from UFN_OR_GetGiftWarehouseMovementsQuantity(@PreviousMonth, @LastDayPreviousMonth, @Warehouse, default)
	) M on I.InvGi = M.Gift

-- actualizamos el inventario, restandole los recibos de regalos del mes
update @Inventory
set	InvQty = InvQty - R.Quantity
from @Inventory I
	inner join (
		select Gift, Quantity from UFN_OR_GetGiftGiftsReceiptsQuantity(@PreviousMonth, @LastDayPreviousMonth, @Warehouse, @Warehouse, default)
	) R on I.InvGi = R.Gift

-- eliminamos los registros del inventario
delete from GiftsInventory where gvD = @CurrentMonth and gvwh = @Warehouse

-- agregamos los registros del inventario
insert into GiftsInventory   
select @CurrentMonth, @Warehouse, InvGi, InvQty
from @Inventory

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

