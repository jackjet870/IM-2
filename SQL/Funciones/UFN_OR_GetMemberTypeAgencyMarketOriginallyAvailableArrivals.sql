if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableArrivals]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableArrivals]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de llegadas por tipo de socio, agencia, mercado y originalmente disponible
** 
** [wtorres]	26/Ene/2012 Created
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableArrivals](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL'	-- Claves de agencias
)
returns @Table table (
	MemberType varchar(12),
	Agency varchar(35),
	Market varchar(10),
	OriginallyAvailable varchar(30),
	Arrivals int
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
	-- Mercado
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
group by guGuestRef, guag, gumk, (guOriginAvail | guInvit)

return
end