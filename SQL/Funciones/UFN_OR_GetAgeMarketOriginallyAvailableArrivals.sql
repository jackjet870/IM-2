if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetAgeMarketOriginallyAvailableArrivals]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetAgeMarketOriginallyAvailableArrivals]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el número de llegadas por edad, mercado y originalmente disponible
** 
** [wtorres]	26/Abr/2010 Creado
**
*/
create function [dbo].[UFN_OR_GetAgeMarketOriginallyAvailableArrivals](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000)	-- Claves de Lead Sources
)
returns @Table table (
	Age tinyint,
	Market varchar(10),
	OriginallyAvailable varchar(30),
	Arrivals int
)
as
begin

insert @Table
select
	guAge1,
	gumk,
	-- Si tiene invitación, considerarlo en originalmente disponible
	case when (guOriginAvail | guInvit) = 1 then 'ORIGINALLY AVAILABLES' else 'ORIGINALLY UNAVAILABLES' end,
	Count(*)
from Guests
where
	-- Fecha de llegada
	guCheckInD between @DateFrom and @DateTo
	-- Con Check In
	and guCheckIn = 1
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
group by guAge1, gumk, (guOriginAvail | guInvit)

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

