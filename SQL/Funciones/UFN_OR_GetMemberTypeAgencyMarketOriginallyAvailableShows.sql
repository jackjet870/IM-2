if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableShows]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableShows]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por tipo de socio, agencia, mercado y originalmente disponible
** 
** [wtorres]	26/Ene/2012 Creado
** [aalcocer]	10/Jun/2016 Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableShows](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000),			-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',		-- Claves de mercados
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@ConsiderDirectsAntesInOut bit = 0,	-- Indica si se debe considerar directas Antes In & Out
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	MemberType varchar(12),
	Agency varchar(35),
	Market varchar(10),
	OriginallyAvailable varchar(30),
	Shows int
)
as
begin

insert @Table
select
	guGuestRef,
	guag,
	gumk,
	-- Si tiene invitacion, considerarlo en originalmente disponible
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
	-- Mercado
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
	-- No quinielas split
	and (@ConsiderQuinellas = 0 or guQuinellaSplit = 0)
	-- No Directas no Antes In & Out
	and (@ConsiderDirectsAntesInOut = 0 or not (guDirect = 1 and guAntesIO = 0))
group by guGuestRef, guag, gumk, (guOriginAvail | guInvit)

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

