if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_ValidateSale]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_ValidateSale]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Valida que los datos de una venta existan
** 
** [wtorres]	04/Jul/2013 Optimizado
** [wtorres]	27/Ene/2014 Agregue el parametro de clave de huesped
**
*/
create procedure [dbo].[USP_OR_ValidateSale]
	@ChangedBy varchar(10),			-- Clave del usuario que esta haciendo el cambio
	@Password varchar(10),			-- Contraseña del usuario que esta haciendo el cambio
	@Sale int,						-- Clave de la venta
	@MembershipNumber varchar(10),	-- Numero de membresia
	@Guest int,						-- Clave del huesped
	@SaleType varchar(10),			-- Clave del tipo de venta
	@SalesRoom varchar(10),			-- Clave de la sala de ventas
	@Location varchar(10),			-- Clave de la locacion
	@PR1 varchar(10),				-- Clave del PR 1
	@PR2 varchar(10),				-- Clave del PR 2
	@PR3 varchar(10),				-- Clave del PR 3
	@PRCaptain1 varchar(10),		-- Clave del capitan del PR 1
	@PRCaptain2 varchar(10),		-- Clave del capitan del PR 2
	@PRCaptain3 varchar(10),		-- Clave del capitan del PR 3
	@Liner1 varchar(10),			-- Clave del Liner 1
	@Liner2 varchar(10),			-- Clave del Liner 2
	@LinerCaptain varchar(10),		-- Clave del capitan de Liners
	@Closer1 varchar(10),			-- Clave del Closer 1
	@Closer2 varchar(10),			-- Clave del Closer 2
	@Closer3 varchar(10),			-- Clave del Closer 3
	@CloserCaptain	varchar(10),	-- Clave del capitan de Closers
	@Exit1 varchar(10),				-- Clave del Exit Closer 1
	@Exit2 varchar(10),				-- Clave del Exit Closer 2
	@Podium varchar(10),			-- Clave del Podium
	@VLO varchar(10)				-- Clave del Verificador legal
as
set nocount on

declare
	@Focus varchar(20),		-- Control que tendra el foco en caso de error
	@Message varchar(100)	-- Mensaje de error

-- validamos que el usuario que esta haciendo el cambio tenga permiso
select @Focus = case when Focus = 'ID' then 'ChangedBy' else Focus end, @Message = Message
from UFN_OR_ValidateUser(@ChangedBy, @Password, default, default, default, default)

-- si no hubo error
if @Focus = '' begin

	-- validamos el numero de membresia
	if @SaleType = 'NS' and (select Count(*) from Sales where saMemberShipNum = @MembershipNumber and saCancel = 0
		and (@Sale = 0 or (sast = @SaleType and saID <> @Sale))) > 0 
		select @Focus = 'MembershipNumber', @Message = 'Membership number already exist'
		
	-- validamos el huesped
	else if (select Count(*) from Guests where guID = @Guest) = 0
		select @Focus = 'Guest', @Message = 'Guest ID does not exist'
		
	-- validamos la locacion
	else if (select Count(*) from Locations where loID = @Location) = 0
		select @Focus = 'Location', @Message = 'Location does not exist'

	-- validamos la sala de ventas
	else if (select Count(*) from SalesRooms where srID = @SalesRoom) = 0
		select @Focus = 'SalesRoom', @Message = 'Sales Room does not exist'

	-- validamos el PR 1
	else if @PR1 <> '' and (select Count(*) from Personnel where peID = @PR1) = 0
		select @Focus = 'PR1', @Message = 'PR 1 does not exist'

	-- validamos el PR 2
	else if @PR2 <> '' and (select Count(*) from Personnel where peID = @PR2) = 0
		select @Focus = 'PR2', @Message = 'PR 2 does not exist'

	-- validamos el PR 3
	else if @PR3 <> '' and (select Count(*) from Personnel where peID = @PR3) = 0
		select @Focus = 'PR3', @Message = 'PR 3 does not exist'

	-- validamos el capitan del PR 1
	else if @PRCaptain1 <> '' and (select Count(*) from Personnel where peID = @PRCaptain1) = 0
		select @Focus = 'PRCaptain1', @Message = 'PR Captain 1 does not exist'

	-- validamos el capitan del PR 2
	else if @PRCaptain2 <> '' and (select Count(*) from Personnel where peID = @PRCaptain2) = 0
		select @Focus = 'PRCaptain2', @Message = 'PR Captain 2 does not exist'

	-- validamos el capitan del PR 3
	else if @PRCaptain3 <> '' and (select Count(*) from Personnel where peID = @PRCaptain3) = 0
		select @Focus = 'PRCaptain3', @Message = 'PR Captain 3 does not exist'

	-- validamos el Liner 1
	else if @Liner1 <> '' and (select Count(*) from Personnel where peID = @Liner1) = 0
		select @Focus = 'Liner1', @Message = 'Liner 1 does not exist'

	-- validamos el Liner 2
	else if @Liner2 <> '' and (select Count(*) from Personnel where peID = @Liner2) = 0
		select @Focus = 'Liner2', @Message = 'Liner 2 does not exist'

	-- validamos el capitan de Liners
	else if @LinerCaptain <> '' and (select Count(*) from Personnel where peID = @LinerCaptain) = 0
		select @Focus = 'LinerCaptain', @Message = 'Liner Captain does not exist'

	-- validamos el Closer 1
	else if @Closer1 <> '' and (select Count(*) from Personnel where peID = @Closer1) = 0
		select @Focus = 'Closer1', @Message = 'Closer 1 does not exist'

	-- validamos el Closer 2
	else if @Closer2 <> '' and (select Count(*) from Personnel where peID = @Closer2) = 0
		select @Focus = 'Closer2', @Message = 'Closer 2 does not exist'

	-- validamos el Closer 3
	else if @Closer3 <> '' and (select Count(*) from Personnel where peID = @Closer3) = 0
		select @Focus = 'Closer3', @Message = 'Closer 3 does not exist'

	-- validamos el capitan de Closers
	else if @CloserCaptain <> '' and (select Count(*) from Personnel where peID = @CloserCaptain) = 0
		select @Focus = 'CloserCaptain', @Message = 'Closer Captain does not exist'

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
end

select @Focus as Focus, @Message as Message

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

