if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_ValidateGiftsReceipt]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_ValidateGiftsReceipt]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Valida que los datos de un recibo de regalos existan
** 
** [wtorres]	04/Jul/2013 Optimizado
** [wtorres]	27/Ene/2014 Agregue el parametro de clave de huesped
**
*/
create procedure [dbo].[USP_OR_ValidateGiftsReceipt]
	@ChangedBy varchar(10),	-- Clave del usuario que esta haciendo el cambio
	@Password varchar(10),	-- Contraseña del usuario que esta haciendo el cambio
	@Guest int,				-- Clave del huesped
	@Location varchar(10),	-- Clave de la locacion
	@SalesRoom varchar(10),	-- Clave de la sala de ventas
	@GiftsHost varchar(10),	-- Clave del Host de regalos
	@Personnel varchar(10)	-- Clave del personal que ofrecio los regalos
as
set nocount on

declare
	@Focus varchar(20),		-- Control que tendra el foco en caso de error
	@Message varchar(100)	-- Mensaje de error

-- validamos que el usuario que esta haciendo el cambio tenga permiso
select @Focus = case when Focus = 'ID' then 'ChangedBy' else Focus end, @Message = Message
from UFN_OR_ValidateUser(@ChangedBy, @Password, 'SR', @SalesRoom, default, default)

-- si no hubo error
if @Focus = '' begin

	-- validamos el huesped
	if (select Count(*) from Guests where guID = @Guest) = 0
		select @Focus = 'Guest', @Message = 'Guest ID does not exist'
		
	-- validamos la locacion
	else if (select Count(*) from Locations where loID = @Location) = 0
		select @Focus = 'Location', @Message = 'Location does not exist'

	-- validamos la sala de ventas
	else if (select Count(*) from SalesRooms where srID = @SalesRoom) = 0
		select @Focus = 'SalesRoom', @Message = 'Sales Room does not exist'

	-- validamos el Host de regalos
	else if @GiftsHost <> '' and (select Count(*) from Personnel where peID = @GiftsHost) = 0
		select @Focus = 'GiftsHost', @Message = 'Gifts Host does not exist'

	-- validamos el personal que ofrecio los regalos
	else if @Personnel <> '' and (select Count(*) from Personnel where peID = @Personnel) = 0
		select @Focus = 'Personnel', @Message = 'Personnel does not exist'
end

select @Focus as Focus, @Message as Message

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

