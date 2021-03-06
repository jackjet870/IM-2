if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_ValidateUser]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_ValidateUser]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Valida que una cuenta de usuario tenga permiso
** 
** [wtorres]	14/Oct/2009 Creado
** [wtorres]	20/May/2010 Agregue los parametros @UserType y @ValidatePassword
**
*/
create function [dbo].[UFN_OR_ValidateUser](
	@User varchar(10),				-- Clave del usuario
	@Password varchar(10),			-- Contraseña del usuario
	@PlaceType varchar(2) = NULL,	-- Tipo de lugar
	@PlaceID varchar(10) = NULL,	-- Clave del lugar
	@UserType varchar(10) = NULL,	-- Tipo de usuario
	@ValidatePassword bit = 1		-- Indica si se desea validar la contraseña
)
returns @Table table (
	Focus varchar(20),
	Message varchar(100)
)
as
begin

declare
	@Focus varchar(20),			-- Control que tendra el foco en caso de error
	@Message varchar(100),		-- Mensaje de error
	@PasswordSaved varchar(10),	-- Contraseña almacenada
	@Active	bit,				-- Status
	@PlaceTypeN	varchar(30),	-- Descripcion del tipo de lugar
	@UserTypeN varchar(20)		-- Descripcion del tipo de usuario

-- indicamos que no hay error
select @Focus = '', @Message = ''

-- establece la descripcion del tipo de usuario
if @UserType is null
	set @UserTypeN = 'Account'
else
	set @UserTypeN = @UserType + ' account'

-- si el usuario existe
if (select Count(*) from Personnel where peID = @User) > 0 begin

	-- si se desea autentificar a un tipo de lugar
	if @PlaceType is not null begin

		-- obtenemos la descripcion del lugar
		select @PlaceTypeN = pyN from PlaceTypes where pyID = @PlaceType

		select @PasswordSaved = pePwd, @Active = peA
		from Personnel
		where peID = @User
			and exists (
				select plpe from PersLSSR where plpe = peID and plLSSR = @PlaceType and plLSSRID = @PlaceID
			)

		-- si el usuario no tiene permiso para el lugar
		if @PasswordSaved is null
			select @Focus = 'ID', @Message = @UserTypeN + ' does not have access to this ' + Lower(IsNull(@PlaceTypeN, 'place'))
	end

	-- si desea autentificar sin tipo de lugar	
	else begin
		select @PasswordSaved = pePwd, @Active = peA
		from Personnel
		where peID = @User
	end

	-- si no hubo error
	if @Focus = '' begin

		-- si el usuario esta inactivo
		if @Active = 0
			select @Focus = 'ID', @Message = @UserTypeN + ' is inactive'

		-- si no coincide la contraseña y se desea validar la contraseña
		else if @Password <> @PasswordSaved and @ValidatePassword = 1
			select @Focus = 'Password', @Message = 'Invalid password'			
	end
end

-- si el usuario no existe
else
	select @Focus = 'ID', @Message = @UserTypeN + ' does not exist'

insert @Table
select @Focus, @Message

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

