if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetFullName]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetFullName]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene el nombre completo de un huésped
** 
** [wtorres]	12/Nov/2009 Creado
** [wtorres]	12/Oct/2010 Optimización
**
*/
create function [dbo].[UFN_OR_GetFullName](
	@LastName varchar(100),		-- Apellido
	@FirstName varchar(100))	-- Nombre
returns varchar(200)
as
begin
declare @FullName varchar(200)

set @FullName = dbo.AddString(@LastName, @FirstName, ' ')

return @FullName
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

