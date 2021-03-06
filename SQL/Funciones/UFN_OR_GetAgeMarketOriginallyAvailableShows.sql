if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetAgeMarketOriginallyAvailableShows]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetAgeMarketOriginallyAvailableShows]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el número de shows por edad, mercado y originalmente disponible
** 
** [wtorres]	26/Abr/2010 Creado
** [wtorres]	18/Oct/2010 Agregué el parámetro @BasedOnArrival
**
*/
create function [dbo].[UFN_OR_GetAgeMarketOriginallyAvailableShows](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000),			-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@ConsiderDirectsAntesInOut bit = 0,	-- Indica si se debe considerar directas Antes In & Out
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	Age tinyint,
	Market varchar(10),
	OriginallyAvailable varchar(30),
	Shows int
)
as
begin

insert @Table
select
	guAge1,
	gumk,
	-- Si tiene invitación, considerarlo en originalmente disponible
	case when (guOriginAvail | guInvit) = 1 then 'ORIGINALLY AVAILABLES' else 'ORIGINALLY UNAVAILABLES' end,
	Sum(case when @ConsiderQuinellas = 0 then 1 else guShowsQty end)
from Guests
where
	-- Fecha de show
	((@BasedOnArrival = 0 and guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Con show
	and guShow = 1))
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
	-- No quinielas split
	and (@ConsiderQuinellas = 0 or guQuinellaSplit = 0)
	-- No Directas no Antes In & Out
	and (@ConsiderDirectsAntesInOut = 0 or not (guDirect = 1 and guAntesIO = 0))
group by guAge1, gumk, (guOriginAvail | guInvit)

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

