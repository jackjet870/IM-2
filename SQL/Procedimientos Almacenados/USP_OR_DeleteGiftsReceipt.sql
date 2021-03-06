if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_DeleteGiftsReceipt]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_DeleteGiftsReceipt]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Elimina un recibo de regalos
** 
** [wtorres]	30/Nov/2013 Creado
**
*/
create procedure [dbo].[USP_OR_DeleteGiftsReceipt]
    @Receipt int	-- Clave del recibo de regalos
as
set nocount on

declare
	@Guest int,		-- Clave del huesped
	@Receipts int	-- Numero de recibos de regalos

-- obtenemos la clave del huesped
select @Guest = grgu from GiftsReceipts where grID = @Receipt

-- Regalos de recibos de regalos
delete from GiftsReceiptsC where gegr = @Receipt

-- Paquetes de regalos de recibos de regalos
delete from GiftsReceiptsPacks where gkgr = @Receipt

-- Historico de los recibos de regalos
delete from GiftsReceiptsLog where gogr = @Receipt

-- Recibo de regalos
delete from GiftsReceipts where grID = @Receipt

-- obtenemos el numero de recibos que le quedaron
select @Receipts = Count(*) from GiftsReceipts where grgu = @Guest

-- si ya no le quedan mas recibos
if @Receipts = 0

	-- indicamos que el huesped ya no tiene mas recibos
	update Guests set guGiftsReceived = 0 where guID = @Guest

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

