if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableAvailables]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableAvailables]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de disponibles por contrato, agencia, mercado y originalmente disponible
** 
** [galcocer]	04/Feb/2012 Created
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableAvailables](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@ConsiderQuinellas bit = 0		-- Indica si se debe considerar quinielas
)
returns @Table table (
	Contract varchar(20),
	Agency varchar(35),
	Market varchar(10),
	OriginallyAvailable varchar(30),
	Availables int
)
as
begin

insert @Table
select
	guO1,
	guag,
	gumk,
	-- Si tiene invitacion, considerarlo en originalmente disponible
	case when (guOriginAvail | guInvit) = 1 then 'ORIGINALLY AVAILABLES' else 'ORIGINALLY UNAVAILABLES' end,
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then 1 else guShowsQty end)
from Guests
where
	-- Fecha de llegada
	guCheckInD between @DateFrom and @DateTo
	-- Con Check In
	and guCheckIn = 1
	-- Disponible
	and guAvail = 1
	-- No Rebook
	and guRef is null
	-- Contactado
	and guInfo = 1
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
group by guO1, guag, gumk, (guOriginAvail | guInvit)

return
end