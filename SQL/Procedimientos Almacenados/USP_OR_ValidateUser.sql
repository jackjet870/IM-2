if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_ValidateUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_ValidateUser]
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
** [wtorres]	23/Ago/2011 Creado
**
*/
create procedure [dbo].[USP_OR_ValidateUser]
	@User varchar(10),			-- Clave del usuario
	@PlaceType varchar(2),		-- Tipo de lugar
	@PlaceID varchar(10),		-- Clave del lugar
	@UserType varchar(10),		-- Tipo de usuario
	@ValidatePassword bit = 0,	-- Indica si se desea validar la contraseña
	@Password varchar(10) = ''	-- Contraseña del usuario
as
set nocount on

if @UserType = ''
	set @UserType = NULL

-- determinamos si el usuario tiene permiso
select Focus, Message from UFN_OR_ValidateUser(@User, @Password, @PlaceType, @PlaceID, @UserType, @ValidatePassword)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

