if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetAgencyMarketOriginallyAvailableFollowUps]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetAgencyMarketOriginallyAvailableFollowUps]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el número de seguimientos por agencia, mercado y originalmente disponible
** 
** [wtorres]	26/May/2010 Creado
** [wtorres]	15/Oct/2010 Agregué el parámetro @BasedOnArrival
**
*/
create function [dbo].[UFN_OR_GetAgencyMarketOriginallyAvailableFollowUps](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	Agency varchar(35),
	Market varchar(10),
	OriginallyAvailable varchar(30),
	FollowUps int
)
as
begin

insert @Table
select
	guag,
	gumk,
	-- Si tiene invitación, considerarlo en originalmente disponible
	case when (guOriginAvail | guInvit) = 1 then 'ORIGINALLY AVAILABLES' else 'ORIGINALLY UNAVAILABLES' end,
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
group by guag, gumk, (guOriginAvail | guInvit)

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

