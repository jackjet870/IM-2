if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetLastMembershipType]') and xtype in (N'FN', N'if', N'TF'))
drop function [dbo].[UFN_OR_GetLastMembershipType]
GO

set QUOTED_IDENTIFIER ON 
GO
set ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene el ultimo tipo de membresia de un numero de membresia
**
** [wtorres]	29/Ene/2014	Created
*/

create function [dbo].[UFN_OR_GetLastMembershipType] (
	@MembershipNumber varchar(10)
)
returns varchar(50)
as  
begin

declare @MembershipType varchar(50)

select top 1 @MembershipType = M.mtN
from Sales S
	left join MembershipTypes M on M.mtID = S.samtGlobal
where S.saMembershipNum = @MembershipNumber
order by S.saD desc

-- establecemos respuesta de la funcion
return @MembershipType
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

