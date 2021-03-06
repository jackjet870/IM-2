if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetGuestType]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetGuestType]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene el tipo de un huesped
** 
** [wtorres]	19/Mar/2015 Created
**
*/
create function [dbo].[UFN_OR_GetGuestType](
	@Contract varchar(20),	-- Clave del contrato
	@MemberType varchar(12)	-- Clave del tipo de socio
)
returns varchar(1)
as
begin
declare @GuestType varchar(1)

select @GuestType = case
		when @Contract like 'REFREE%' or @Contract like 'CLRF%' or @Contract like 'CLRP%' or @Contract like 'RCIR%'
			or @Contract like 'REF99%' or @Contract like 'CLR9%' then 'R'
		when @MemberType = 'G' then 'G'
		else 'M' end

return @GuestType
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

