if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetGiftsReceiptDetailPromotionsOpera]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetGiftsReceiptDetailPromotionsOpera]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta los regalos de un recibo que tienen asociados promociones de Opera
** 
** [wtorres]	18/Jun/2014 Created
** [wtorres]	25/Nov/2014 Modified. Ahora se valida que la cantidad del regalo sea positiva
** [wtorres]	24/Mar/2015 Modified. Ahora se valida que el regalo aun no se haya dado en Opera
**
*/
create procedure [dbo].[USP_OR_GetGiftsReceiptDetailPromotionsOpera]
	@Receipt int	-- Clave del recibo de regalos
as
set nocount on

select D.gegr, D.gegi, GI.giN, GI.giPromotionOpera, R.grgu, D.geQty
from GiftsReceiptsC D
	inner join GiftsReceipts R on R.grID = D.gegr
	inner join Gifts GI on GI.giID = D.gegi
where
	-- Recibo de regalos
	R.grID = @Receipt
	-- Regalo con promocion de Opera
	and GI.giPromotionOpera is not null
	-- El regalo aun no se ha dado en Opera
	and D.geAsPromotionOpera = 0
	-- Regalos con cantidades positivas
	and D.geQty > 0
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

