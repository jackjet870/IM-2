if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetGiftGiftsReceiptsQuantity]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetGiftGiftsReceiptsQuantity]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve la cantidad de regalos de recibos de regalos por regalo
** 
** [wtorres]	29/Dic/2010 Creado
** [wtorres]	07/Oct/2013 Agregue el parametro @Gifts
**
*/
create function [dbo].[UFN_OR_GetGiftGiftsReceiptsQuantity](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@Warehouse varchar(10),			-- Clave del almacen
	@SalesRoom varchar(10) = 'ALL',	-- Clave de la sala de ventas
	@Gifts varchar(max) = 'ALL'		-- Claves de regalos
)
returns @Table table (
	Gift varchar(10),
	Quantity int
)
as
begin

insert @Table

select
	Gift,
	Sum(Quantity) as Quantity
from (

	-- Regalos que no son paquetes de regalos
	-- =============================================
	select 
		D.gegi as Gift,
		Sum(D.geQty) as Quantity
	from GiftsReceipts R
		inner join GiftsReceiptsC D on R.grID = D.gegr 
		inner join Gifts G on D.gegi = G.giID
	where
		-- Fecha del recibo
		R.grD between @DateFrom and @DateTo
		-- Almacen
		and R.grwh = @Warehouse
		-- Sala de ventas
		and (@SalesRoom = 'ALL' or R.grsr = @SalesRoom)
		-- Regalos
		and (@Gifts = 'ALL' or D.gegi in (select item from split(@Gifts, ',')))
		-- Regalo inventariable
		and G.giInven = 1
		-- Regalo que se debe contar aunque este en un recibo cancelado
		and (G.giCountInCancelledReceipts = 1
		-- Regalo que no se debe contar si está en un recibo cancelado
		or (G.giCountInCancelledReceipts = 0 and R.grCancel = 0))
		-- Regalo que no es paquete de regalos
		and G.giPack = 0
	group by D.gegi

	-- Regalos que forman parte de paquetes de regalos
	-- =============================================
	union all
	select 
		P.gpgi,
		Sum(D.geQty * P.gpQty)
	from GiftsReceipts R
		inner join GiftsReceiptsC D on R.grID = D.gegr
		inner join GiftsPacks P on D.gegi = P.gpPack
		inner join Gifts G on P.gpgi = G.giID
	where
		-- Fecha del recibo
		R.grD between @DateFrom and @DateTo
		-- Almacen
		and R.grwh = @Warehouse
		-- Sala de ventas
		and (@SalesRoom = 'ALL' or R.grsr = @SalesRoom)
		-- Regalos
		and (@Gifts = 'ALL' or P.gpgi in (select item from split(@Gifts, ',')))
		-- Regalo inventariable
		and G.giInven = 1
		-- Regalo que se debe contar aunque este en un recibo cancelado
		and (G.giCountInCancelledReceipts = 1
		-- Regalo que no se debe contar si está en un recibo cancelado
		or (G.giCountInCancelledReceipts = 0 and R.grCancel = 0))
		-- Regalo que se debe contar si forma parte de un paquete de regalos
		and G.giCountInPackage = 1
	group by P.gpgi
) as D
group by Gift

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
