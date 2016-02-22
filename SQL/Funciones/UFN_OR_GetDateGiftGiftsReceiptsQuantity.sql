if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetDateGiftGiftsReceiptsQuantity]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetDateGiftGiftsReceiptsQuantity]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve la cantidad de regalos de recibos de regalos por fecha y regalo
** 
** [wtorres]	29/Dic/2010 Creado
** [wtorres]	07/Oct/2013 Agregue el parametro @Gifts
**
*/
create function [dbo].[UFN_OR_GetDateGiftGiftsReceiptsQuantity](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@Warehouse varchar(10),			-- Clave del almacen
	@Gifts varchar(max) = 'ALL',	-- Claves de regalos
	@Out bit						-- Indica si se desean las salidas (1) o las devoluciones (0)
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
	[Date],
	Gift,
	Sum(Quantity) as Quantity
from (

	-- Regalos que no son paquetes de regalos
	-- =============================================
	select 
		R.grD as [Date],
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
		-- Salidas
		and ((@Out = 1 and D.geQty > 0)
		-- Devoluciones
		or (@Out = 0 and D.geQty < 0))
	group by R.grD, D.gegi

	-- Regalos que forman parte de paquetes de regalos
	-- =============================================
	union all
	select 
		R.grD,
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
		-- Salidas
		and ((@Out = 1 and D.geQty > 0)
		-- Devoluciones
		or (@Out = 0 and D.geQty < 0))
	group by R.grD, P.gpgi
) as D
group by [Date], Gift

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
