if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_DeletePersonnel]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_DeletePersonnel]
GO

SET ANSI_NULLS ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Elimina un usuario
**
** [wtorres]	26/Mar/2015	Created
**
*/
create procedure [dbo].[USP_OR_DeletePersonnel]
	@User varchar(10)
as

-- Roles
delete from PersonnelRoles where prpe = @User

-- Permisos
delete from PersonnelPermissions where pppe = @User

-- Accesos a lugares
delete from PersLSSR where plpe = @User

-- Historico de puestos
delete from PostsLog where pppe = @User

-- Historico de equipos
delete from TeamsLog where tlpe = @User

-- Personal
delete from Personnel where peID = @User
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

