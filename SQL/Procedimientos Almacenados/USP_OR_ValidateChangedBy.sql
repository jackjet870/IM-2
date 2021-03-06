if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_ValidateChangedBy]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_ValidateChangedBy]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Valida que una cuenta de usuario tenga permiso para hacer un cambio
** 
** [wtorres]	13/Oct/2009 Creado
** [wtorres]	02/Ago/2010 Agregue los parametros @UserType y @PR
**
*/
create procedure [dbo].[USP_OR_ValidateChangedBy]
	@ChangedBy varchar(10),	-- Clave del usuario que esta haciendo el cambio
	@Password varchar(10),	-- Contraseña
	@PlaceType varchar(2),	-- Tipo de lugar
	@PlaceID varchar(10),	-- Clave del lugar
	@UserType varchar(10),	-- Tipo de usuario
	@PR varchar(10) = ''	-- Clave del PR
as
set nocount on

declare
	@Focus varchar(20),		-- Control que tendra el foco
	@Message varchar(100)	-- Mensaje de error

-- determinamos si el usuario que esta haciendo el cambio tiene permiso
select @Focus = Focus, @Message = Message from UFN_OR_ValidateUser(@ChangedBy, @Password, @PlaceType, @PlaceID, @UserType, default)

-- si se desea validar el PR
if @PR <> '' begin

	-- si el usuario que esta haciendo el cambio tiene permiso
	if @Focus = '' begin

		-- si el que está haciendo el cambio no es el PR
		if @ChangedBy <> @PR begin

			-- determinamos si el PR tiene permiso
			select @Focus = Focus, @Message = Message from UFN_OR_ValidateUser(@PR, NULL, @PlaceType, @PlaceID, 'PR', 0)

			-- si el PR no tiene permiso
			if @Focus <> '' begin
				if @Focus = 'ID'
					set @Focus = 'PR'
			end		
		end
	end

	-- si el usuario que esta haciendo el cambio no tiene permiso
	else begin
		if @Focus = 'ID'
			set @Focus = 'ChangedBy'
	end
end

select @Focus as Focus, @Message as Message

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

