if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetPRFollowUps]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetPRFollowUps]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el número de seguimientos por PR
** 
** [wtorres]	26/May/2010 Creado
** [wtorres]	18/Nov/2010 Agregué el parámetro @BasedOnArrival
**
*/
create function [dbo].[UFN_OR_GetPRFollowUps](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	PR varchar(10),
	FollowUps int
)
as
begin

insert @Table
select
	guPRFollow,
	Count(*)
from Guests
where
	-- Fecha de seguimiento
	((@BasedOnArrival = 0 and guFollowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Con seguimiento
	and guFollow = 1))
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
group by guPRFollow

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

