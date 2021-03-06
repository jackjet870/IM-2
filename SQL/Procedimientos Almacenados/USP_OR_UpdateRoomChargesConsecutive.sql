if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_UpdateRoomChargesConsecutive]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_UpdateRoomChargesConsecutive]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Actualiza el consecutivo de cargos a habitacion de una reservacion
**
** [wtorres]	21/Sep/2013	Created
**
*/
create procedure [dbo].[USP_OR_UpdateRoomChargesConsecutive]
	@Hotel varchar(10),
	@Folio varchar(15)
as
set nocount on

-- si no existe el registro, lo agregamos
if (select Count(*) from RoomCharges where rhls = @Hotel and rhFolio = @Folio) = 0
	insert into RoomCharges (rhls, rhFolio, rhConsecutive)
	values (@Hotel, @Folio, 1)

-- sino lo actualizamos
else
	update RoomCharges
	set rhConsecutive = rhConsecutive + 1
	where rhls = @Hotel and rhFolio = @Folio

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

