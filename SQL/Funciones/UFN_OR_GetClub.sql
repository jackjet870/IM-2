if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetClub]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetClub]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene el club de un huesped
** 
** [wtorres]	26/Feb/2015 Created
**
*/
create function [dbo].[UFN_OR_GetClub](
	@AgencyClub int,	-- Club de la agencia
	@GuestClub int		-- Club del huesped
)
returns varchar(200)
as
begin
declare @Club varchar(200)

set @Club = Coalesce(@GuestClub, @AgencyClub, 1) -- 1 - Palace Premier

return @Club
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

