if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_SaveGuestMovement]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_SaveGuestMovement]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Agrega un movimiento a un huesped
** 
** [wtorres]	31/May/2010 Created
** [wtorres]	04/Dic/2014 Modified. Aumente el ancho del parametro @ComputerName para soportar los nombres de computadoras Mac
**
*/
create procedure [dbo].[USP_OR_SaveGuestMovement]
    @Guest int,						-- Clave del huesped
    @GuestMovementType varchar(10),	-- Tipo de movimiento del huesped
    @ChangedBy varchar(10),			-- Clave del usuario que está haciendo el cambio
	@ComputerName varchar(63),		-- Nombre de la computadora
	@IPAddress varchar(15)			-- Dirección IP de la computadora
as
set nocount on

-- agregamos la computadora si no existe
exec USP_OR_SaveComputer @ComputerName, @IPAddress

-- si no existe el tipo de movimiento del huesped
if (select Count(*) from GuestsMovements where gmgu = @Guest and gmgn = @GuestMovementType) = 0

	-- agregamos el movimiento
    insert into GuestsMovements (gmgu, gmgn, gmDT, gmpe, gmcp, gmIPAddress)
	values (@Guest, @GuestMovementType, GetDate(), @ChangedBy, @ComputerName, @IPAddress)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

