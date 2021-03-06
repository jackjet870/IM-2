USE [OrigosVCPalace]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de kardex de regalos
** 
** [edgrodriguez]	02/Jun/2016 Created.
**
*/
CREATE procedure [dbo].[USP_IM_RptGiftsKardex]
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@Warehouse varchar(10),		-- Clave del almacen
	@Gifts varchar(max) = 'ALL'	-- Claves de regalos
as

set fmtonly off
set nocount on

declare
	@FirstMonth datetime,		-- Sirve para ir al primer dia del mes de la fecha inicial
	@LastDayPrevious datetime,	-- Sirve para ir a un dia antes de la fecha inicial
	@TemporalInventory bit		-- indicamos que se genero el inventario temporal del primer mes

 --Tabla de inventario
declare @Inventory table (
	InvGi varchar(10),
	InvQty int
);

-- =============================================
--					Inventario
-- =============================================

--			Inventario del mes inicial
-- =============================================
-- generamos el inventario del mes inicial si no existe

-- obtenemos el primer dia del mes de la fecha inicial
set @FirstMonth = DATEADD(dd,-(DAY(@DateFrom)-1),@DateFrom)

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

 --5. Recibos de regalos
 --=============================================
select * into #GiftsReceipts from (
	-- Salidas
	select [Date] as RecD, Gift as RecGi, Quantity as RecQty
	from UFN_OR_GetDateGiftGiftsReceiptsQuantity(@DateFrom, @DateTo, @Warehouse, @Gifts, 1)
	-- Devoluciones
	union all
	select [Date], Gift, Quantity
	from UFN_OR_GetDateGiftGiftsReceiptsQuantity(@DateFrom, @DateTo, @Warehouse, @Gifts, 0)
) as D


-- Reporte GiftKardex
-- =============================================
declare @RptKardex table(
	MovD datetime,
	MovGi varchar(150),
	MovGiN varchar(400),
	[InQty] int,
	[OutQty] int,
	InvQty int
);

WHILE(@DateFrom <= @DateTo)
Begin
	Declare @Gift varchar(150), @GiftN varchar(500), @InvQty int;
		
	--Creamos un cursor con los regalores inventariables.
	DECLARE GiftsCursor Cursor Fast_Forward For select I.InvGi,G.giN, ISNULL(I.InvQty,0) from @Inventory I left join Gifts G on I.InvGi = G.giID;
	OPEN GiftsCursor;
	FETCH GiftsCursor INTO @Gift,@GiftN, @InvQty;
	WHILE (@@FETCH_STATUS = 0 )
	BEGIN
		--Declaramos las variables de WarehouseQty y ReceiptQty
		Declare @WQty int, @RQty int;
		set @WQty=0;
		set @RQty=0;
		
		--Obtenemos la cantidad de salida/entrada de gifts de Warehouse y Receipts.
		Select @WQty=ISNULL(MovQty , 0) from #WarehouseMovements WHERE MovD=@DateFrom and MovGi=@Gift;
		Select @RQty=ISNULL(RecQty , 0) from #GiftsReceipts WHERE RecD=@DateFrom and RecGi=@Gift
		
		--Entradas
		Select @InvQty = CASE
			--Si la cantidad de Gifts de Warehouse es Positiva y la cantidad de Gifts de Receipts es negativa
			--entonces sumamos los valores absolutos. (Entrada al Inventario.)
			When ISNULL(@WQty,0) >= 0 AND ISNULL(@RQty,0) <= 0 THEN (@InvQty +(ABS(@WQty)+ ABS(@RQty)))
		ELSE @InvQty END
		
		--Salidas
		Select @InvQty = CASE
			--Si la cantidad de Gifts de Warehouse es Negativa y la cantidad de Gifts de Receipts es Positiva
			--entonces sumamos los valores absolutos. (Entrada al Inventario.)
			When ISNULL(@WQty,0) <= 0 AND ISNULL(@RQty,0) >= 0 THEN (@InvQty - (ABS(@WQty)+ ABS(@RQty)))
		ELSE @InvQty END
		
		--Insertamos los datos a la tabla del reporte.
		INSERT INTO @RptKardex
		SELECT @DateFrom, @Gift, @GiftN,
		CASE
			When ISNULL(@WQty,0) >= 0 AND ISNULL(@RQty,0) <= 0 THEN (ABS(@WQty)+ ABS(@RQty))
		ELSE 0 END [In],
		CASE
			When ISNULL(@WQty,0) <= 0 AND ISNULL(@RQty,0) >= 0 THEN (ABS(@WQty)+ ABS(@RQty))
		ELSE 0 END [Out],		
		@InvQty;
		
		--Actualizamos el inventario.
		Update @Inventory set InvQty=@InvQty where InvGi = @Gift;
		
		FETCH GiftsCursor 
		INTO @Gift,@GiftN, @InvQty;
	END
	CLOSE GiftsCursor;
	DEALLOCATE GiftsCursor;
	
	SET @DateFrom = DATEADD(day,1,@DateFrom)
End

select * from @RptKardex
Order by MovGi,MovD

