if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetGiftsReceiptDetailPromotionsPVP]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetGiftsReceiptDetailPromotionsPVP]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta los regalos de un recibo que tienen asociados promociones de PVP
** 
** [wtorres]	17/Jun/2014 Created
** [wtorres]	16/Ago/2014 Modified. Agregue los campos de nombre y apellido
** [wtorres]	23/Sep/2014 Modified. Ahora se valida que la cantidad del regalo sea positiva
** [wtorres]	19/Nov/2014 Modified. Agregue los campos que indican si el huesped pertenece a un Lead Source o sala de ventas que usa Sistur
** [wtorres]	03/Jun/2015 Modified. Ahora contempla las invitaciones externas
**
*/
create procedure [dbo].[USP_OR_GetGiftsReceiptDetailPromotionsPVP]
	@Receipt int	-- Clave del recibo de regalos
as
set nocount on

select D.gegr, G.guFirstName1, G.guLastName1, D.gegi, GI.giN, GI.giPVPPromotion, G.gulsOriginal, G.guHReservID, L.lspg, L.lsUseSistur,
	L.lsPropertyOpera, S.srPropertyOpera, S.srUseSistur
from GiftsReceiptsC D
	inner join GiftsReceipts R on R.grID = D.gegr
	inner join Guests G on G.guID = R.grgu
	inner join Gifts GI on GI.giID = D.gegi
	inner join LeadSources L on G.gulsOriginal = L.lsID
	inner join SalesRooms S on G.gusr = S.srID
where
	-- Recibo de regalos
	R.grID = @Receipt
	-- Regalo con promocion de PVP
	and GI.giPVPPromotion is not null
	-- El regalo aun no se ha dado en PVP
	and D.geInPVPPromo = 0
	-- Regalos con cantidades positivas
	and D.geQty > 0

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

