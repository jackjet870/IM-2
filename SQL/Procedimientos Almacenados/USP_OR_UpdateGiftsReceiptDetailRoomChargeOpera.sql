if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_UpdateGiftsReceiptDetailRoomChargeOpera]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_UpdateGiftsReceiptDetailRoomChargeOpera]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Indica que a un regalo de un recibo se le guardo su cargo a habitacion en Opera
** 
** [wtorres]	17/Jun/2014 Creado
**
*/
create procedure [dbo].[USP_OR_UpdateGiftsReceiptDetailRoomChargeOpera]
	@Receipt int,						-- Clave del recibo de regalos
	@Gift varchar(10),					-- Clave del regalo
	@TransactionTypeOpera varchar(10)	-- Clave del tipo de transaccion en Opera
as
set nocount on

update GiftsReceiptsC
set geInOpera = 1,
	geOperaTransactionType = @TransactionTypeOpera
where gegr = @Receipt and gegi = @Gift

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

