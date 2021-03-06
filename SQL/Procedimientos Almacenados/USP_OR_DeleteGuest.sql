if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_DeleteGuest]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_DeleteGuest]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Elimina un huesped
** 
** [wtorres]	07/Jun/2010 Creado
** [wtorres]	28/Ene/2012 Ahora tambien elimina las notas de PR
** [wtorres]	03/Feb/2012 Ahora tambien elimina los integrantes de grupos de huespedes
**
*/
create procedure [dbo].[USP_OR_DeleteGuest]
    @Guest int	-- Clave del huesped
as
set nocount on

-- Historico del huesped
delete from GuestLog where glgu = @Guest

-- Movimientos del huesped
delete from GuestsMovements where gmgu = @Guest

-- Huespedes adicionales
delete from GuestsAdditional where gagu = @Guest
delete from GuestsAdditional where gaAdditional = @Guest

-- Tarjetas de credito
delete from GuestsCreditCards where gdgu = @Guest

-- Estatus de huespedes
delete from GuestsStatus where gtgu = @Guest

-- Depositos
delete from BookingDeposits where bdgu = @Guest

-- Shows especiales de los vendedores
delete from ShowsSalesmen where shgu = @Guest

-- Tickets de comida
delete from MealTickets where megu = @Guest

-- Notas de PR
delete from PRNotes where pngu = @Guest

-- Integrantes de grupos de huespedes
delete from GuestsGroupsIntegrants where gjgu = @Guest

/* =================================================*/
/*						Regalos						*/
/* =================================================*/
-- Regalos de invitacion
delete from InvitsGifts where iggu = @Guest

-- Regalos de recibos de regalos
delete from GiftsReceiptsC where gegr in (
	select grID from GiftsReceipts where grgu = @Guest
)

-- Paquetes de regalos de recibos de regalos
delete from GiftsReceiptsPacks where gkgr in (
	select grID from GiftsReceipts where grgu = @Guest
)

-- Historico de los recibos de regalos
delete from GiftsReceiptsLog where gogr in (
	select grID from GiftsReceipts where grgu = @Guest
)

-- Recibos de regalos
delete from GiftsReceipts where grgu = @Guest

/* =================================================*/
/*						Ventas						*/
/* =================================================*/
-- Pagos
delete from Payments where pasa in (
	select saID from Sales where sagu = @Guest
)

-- Ventas especiales de los vendedores
delete from SalesSalesmen where smsa in (
	select saID from Sales where sagu = @Guest
)

-- Historico de ventas
delete from SalesLog where slsa in (
	select saID from Sales where sagu = @Guest
)

-- Ventas
delete from Sales where sagu = @Guest

/* =================================================*/
/*						Huesped						*/
/* =================================================*/
-- Huesped
delete from Guests where guID = @Guest

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

