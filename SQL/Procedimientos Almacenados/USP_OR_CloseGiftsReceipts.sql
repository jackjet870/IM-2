if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_CloseGiftsReceipts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_CloseGiftsReceipts]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Cierra los recibos de regalos de una sala de ventas hasta determinada fecha
** 
** [wtorres]	28/Dic/2010 Depurado
** [wtorres]	25/Ene/2014 Ahora se invoca directamente al procedimiento almacenado USP_OR_GetMonthInventory en lugar de invocarlo desde
**							Visual Basic
**
*/
create procedure [dbo].[USP_OR_CloseGiftsReceipts]
	@SalesRoom as varchar(10),	-- Clave de la sala de ventas
	@Date as datetime			-- Fecha
as 
set nocount on

declare @FirstDay as datetime

--  validamos que la fecha de cierre sea menor a la ultima fecha de cierre
if @Date <= (select srGiftsRcptCloseD from SalesRooms where srID = @SalesRoom)
	-- se elimina todo lo que se mayor e igual a la nueva fecha del primer mes
	Delete From GiftsInventory Where gvD > @Date and gvwh = @SalesRoom

-- cerramos los recibos de la sala de ventas
update SalesRooms
set srGiftsRcptCloseD = @Date,
	srCxCCloseD = @Date
from SalesRooms
where srID = @SalesRoom

-- obtenemos el primer dia del mes de la fecha indicada
set @FirstDay = DateAdd(Day, Day(@Date)* -1 + 1, @Date)

-- si no hay inventario del mes
if (select Count(*) as Inventory from GiftsInventory where gvD = @FirstDay and gvwh = @SalesRoom) = 0

	-- agregamos el inventario del mes
	exec USP_OR_GetMonthInventory @SalesRoom, @Date
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

