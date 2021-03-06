if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_HasRole]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_HasRole]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Determina si un usuario tiene determinados roles
** 
** [wtorres]	24/Ene/2014 Creado
**
*/
create function [dbo].[UFN_OR_HasRole](
	@User varchar(10),				-- Clave de usuario
	@Roles varchar(8000) = 'ALL'	-- Claves de roles
)
returns bit
as
begin

declare @Has bit

select @Has = case when Count(*) > 0 then 1 else 0 end
from PersonnelRoles
where
	-- Usuario
	prpe = @User
	-- Roles
	and (@Roles = 'ALL' or prro in (select item from split(@Roles, ',')))

return @Has
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

