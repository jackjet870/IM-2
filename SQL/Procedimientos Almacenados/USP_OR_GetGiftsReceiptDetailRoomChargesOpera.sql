if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetGiftsReceiptDetailRoomChargesOpera]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetGiftsReceiptDetailRoomChargesOpera]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta los regalos de un recibo que tienen asociados cargos a habitacion de Opera
** 
** [wtorres]	17/Jun/2014 Creado
**
*/
create procedure [dbo].[USP_OR_GetGiftsReceiptDetailRoomChargesOpera]
	@Receipt int	-- Clave del recibo de regalos
as
set nocount on

select D.gegr, D.gegi, GI.giN, D.gePriceA, GI.giOperaTransactionType, IsNull(C.rhConsecutive, 0) + 1 as rhConsecutive, G.gulsOriginal,
	G.guHReservID
from GiftsReceiptsC D
	inner join GiftsReceipts R on R.grID = D.gegr
	inner join Guests G on G.guID = R.grgu
	inner join Gifts GI on GI.giID = D.gegi
	left join RoomCharges C on C.rhls = G.gulsOriginal and C.rhFolio = G.guHReservID
where
	-- Recibo de regalos
	R.grID = @Receipt
	-- Con reservacion
	and G.guHReservID is not null
	-- Regalo con tipo de transaccion de Opera
	and GI.giOperaTransactionType is not null
	-- El regalo aun no se ha dado en Opera
	and D.geInOpera = 0
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

