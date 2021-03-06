if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_ValidateShow]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_ValidateShow]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Valida que los datos de un show existan
** 
** [wtorres]	04/Jul/2013 Optimizado
**
*/
create procedure [dbo].[USP_OR_ValidateShow]
	@ChangedBy varchar(10),		-- Clave del usuario que esta haciendo el cambio
	@Password varchar(10),		-- Contraseña del usuario que esta haciendo el cambio
	@SalesRoom varchar(10),		-- Clave de la sala de ventas
	@Agency varchar(35),		-- Clave de la agencia
	@Country varchar(25),		-- Clave del pais
	@PR1 varchar(10),			-- Clave del PR 1
	@PR2 varchar(10),			-- Clave del PR 2
	@PR3 varchar(10),			-- Clave del PR 3
	@Liner1 varchar(10),		-- Clave del Liner 1
	@Liner2 varchar(10),		-- Clave del Liner 2
	@Closer1 varchar(10),		-- Clave del Closer 1
	@Closer2 varchar(10),		-- Clave del Closer 2
	@Closer3 varchar(10),		-- Clave del Closer 3
	@Exit1 varchar(10),			-- Clave del Exit Closer 1
	@Exit2 varchar(10),			-- Clave del Exit Closer 2
	@Podium varchar(10),		-- Clave del Podium
	@VLO varchar(10),			-- Clave del Verificador legal
	@EntryHost varchar(10),		-- Clave del Host de llegada
	@GiftsHost varchar(10),		-- Clave del Host de regalos
	@ExitHost varchar(10)		-- Clave del Host de llegada
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

	-- validamos la sala de ventas
	if (select Count(*) from SalesRooms where srID = @SalesRoom) = 0
		select @Focus = 'SalesRoom', @Message = 'Sales Room does not exist'

	-- validamos la agencia
	else if (select Count(*) from Agencies where agID = @Agency) = 0
		select @Focus = 'Agency', @Message = 'Agency does not exist'

	-- validamos el pais
	else if (select Count(*) from Countries where coID = @Country) = 0
		select @Focus = 'Country', @Message = 'Country does not exist'

	-- validamos el PR 1
	else if @PR1 <> '' and (select Count(*) from Personnel where peID = @PR1) = 0
		select @Focus = 'PR1', @Message = 'PR 1 does not exist'

	-- validamos el PR 2
	else if @PR2 <> '' and (select Count(*) from Personnel where peID = @PR2) = 0
		select @Focus = 'PR2', @Message = 'PR 2 does not exist'

	-- validamos el PR 3
	else if @PR3 <> '' and (select Count(*) from Personnel where peID = @PR3) = 0
		select @Focus = 'PR3', @Message = 'PR 3 does not exist'

	-- validamos el Liner 1
	else if @Liner1 <> '' and (select Count(*) from Personnel where peID = @Liner1) = 0
		select @Focus = 'Liner1', @Message = 'Liner 1 does not exist'

	-- validamos el Liner 2
	else if @Liner2 <> '' and (select Count(*) from Personnel where peID = @Liner2) = 0
		select @Focus = 'Liner2', @Message = 'Liner 2 does not exist'

	-- validamos el Closer 1
	else if @Closer1 <> '' and (select Count(*) from Personnel where peID = @Closer1) = 0
		select @Focus = 'Closer1', @Message = 'Closer 1 does not exist'

	-- validamos el Closer 2
	else if @Closer2 <> '' and (select Count(*) from Personnel where peID = @Closer2) = 0
		select @Focus = 'Closer2', @Message = 'Closer 2 does not exist'

	-- validamos el Closer 3
	else if @Closer3 <> '' and (select Count(*) from Personnel where peID = @Closer3) = 0
		select @Focus = 'Closer3', @Message = 'Closer 3 does not exist'

	-- validamos el Exit 1
	else if @Exit1 <> '' and (select Count(*) from Personnel where peID = @Exit1) = 0
		select @Focus = 'Exit1', @Message = 'Exit 1 does not exist'

	-- validamos el Exit 2
	else if @Exit2 <> '' and (select Count(*) from Personnel where peID = @Exit2) = 0
		select @Focus = 'Exit2', @Message = 'Exit 2 does not exist'

	-- validamos el Podium
	else if @Podium <> '' and (select Count(*) from Personnel where peID = @Podium) = 0
		select @Focus = 'Podium', @Message = 'Podium does not exist'

	-- validamos el Verificador legal
	else if @VLO <> '' and (select Count(*) from Personnel where peID = @VLO) = 0
		select @Focus = 'VLO', @Message = 'VLO does not exist'

	-- validamos el Host de llegada
	else if @EntryHost <> '' and (select Count(*) from Personnel where peID = @EntryHost) = 0
		select @Focus = 'EntryHost', @Message = 'Entry Host does not exist'

	-- validamos el Host de regalos
	else if @GiftsHost <> '' and (select Count(*) from Personnel where peID = @GiftsHost) = 0
		select @Focus = 'GiftsHost', @Message = 'Gifts Host does not exist'

	-- validamos el Host de salida
	else if @ExitHost <> '' and (select Count(*) from Personnel where peID = @ExitHost) = 0
		select @Focus = 'ExitHost', @Message = 'Exit Host does not exist'
end

select @Focus as Focus, @Message as Message

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

