if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetPRAvailables]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetPRAvailables]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de disponibles por PR
** 
** [wtorres]		18/Sep/2009 Created
** [wtorres]		23/Sep/2009 Modified. Convertido a funcion. Agregue el parametro @LeadSources
** [wtorres]		12/Jul/2010 Modified. Agregue el parametro @ConsiderFollow
** [wtorres]		12/Ago/2010 Modified. Agregue el parametro @ConsiderQuinellas
** [wtorres]		18/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [erosado]		04/Mar/2016 Modified. Agregue los parametros @SalesRooms, @Countries, @Agencies y @Markets** [edgrodriguez]	05/May/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/
create function [dbo].[UFN_OR_GetPRAvailables](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(max)='ALL',	-- Clave de los Lead Sources
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@ConsiderFollow bit = 0,			-- Indica si se debe considerar seguimiento
	@BasedOnArrival bit = 0,			-- Indica si se debe basar en la fecha de llegada	
	@SalesRooms varchar(max) = 'ALL',	-- Clave de las salas de ventas
	@Countries varchar(max) = 'ALL',	-- Clave de los paises
	@Agencies varchar(max) = 'ALL',		-- Clave de las agencias
	@Markets varchar(max) = 'ALL'		-- Clave de los mercados
)
returns @Table table (
	PR varchar(10),
	Availables money
)
as
begin

insert @Table

-- Disponibles (PR de contacto)
-- =============================================
select
	guPRInfo,
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then 1 else guShowsQty end)
from Guests
where
	-- PR Info
	guPRInfo is not null
	-- Fecha de contacto
	and ((@BasedOnArrival = 0 and guInfoD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Contactado
	and guInfo = 1))
	-- No Antes In & Out
	and guAntesIO = 0
	-- Disponible
	and guAvail = 1
	-- Lead Source	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))	-- Sales Rooms	and (@SalesRooms = 'ALL' or gusr in (select item from split(@SalesRooms, ',')))	-- Countries	and (@Countries = 'ALL' or guco in (select item from split(@Countries, ',')))	-- Agencies	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))	-- Markets	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
group by guPRInfo

-- Disponibles (PR 1)
-- =============================================
union all
select
	guPRInvit1,
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT)
		else guShowsQty * dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT) end)
from Guests
where
	-- PR 1
	guPRInvit1 is not null
	-- PR de contacto diferente de los PR de invitacion
	and guPRInfo not in (IsNull(guPRInvit1, ''), IsNull(guPRInvit2, ''), IsNull(guPRInvit3, ''))
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and guInvit = 1))
	-- No Antes In & Out
	and guAntesIO = 0
	-- Disponible
	and guAvail = 1
	-- Lead Source	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))	-- Sales Rooms	and (@SalesRooms = 'ALL' or gusr in (select item from split(@SalesRooms, ',')))	-- Countries	and (@Countries = 'ALL' or guco in (select item from split(@Countries, ',')))	-- Agencies	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))	-- Markets	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
group by guPRInvit1

-- Disponibles (PR 2)
-- =============================================
union all
select
	guPRInvit2,
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT)
		else guShowsQty * dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT) end)
from Guests
where
	-- PR 2
	guPRInvit2 is not null
	-- PR de contacto diferente de los PR de invitacion
	and guPRInfo not in (IsNull(guPRInvit1, ''), IsNull(guPRInvit2, ''), IsNull(guPRInvit3, ''))
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and guInvit = 1))
	-- No Antes In & Out
	and guAntesIO = 0
	-- Disponible
	and guAvail = 1
	-- Lead Source	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))	-- Sales Rooms	and (@SalesRooms = 'ALL' or gusr in (select item from split(@SalesRooms, ',')))	-- Countries	and (@Countries = 'ALL' or guco in (select item from split(@Countries, ',')))	-- Agencies	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))	-- Markets	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
group by guPRInvit2

-- Disponibles (PR 3)
-- =============================================
union all
select
	guPRInvit3,
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT)
		else guShowsQty * dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT) end)
from Guests
where
	-- PR 3
	guPRInvit3 is not null
	-- PR de contacto diferente de los PR de invitacion
	and guPRInfo not in (IsNull(guPRInvit1, ''), IsNull(guPRInvit2, ''), IsNull(guPRInvit3, ''))
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and guInvit = 1))
	-- No Antes In & Out
	and guAntesIO = 0
	-- Disponible
	and guAvail = 1
	-- Lead Source	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))	-- Sales Rooms	and (@SalesRooms = 'ALL' or gusr in (select item from split(@SalesRooms, ',')))	-- Countries	and (@Countries = 'ALL' or guco in (select item from split(@Countries, ',')))	-- Agencies	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))	-- Markets	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
group by guPRInvit3

-- Disponibles (PR de seguimiento)
-- =============================================
if @ConsiderFollow = 1
	insert @Table
	select
		guPRFollow,
		Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then 1 else guShowsQty end)
	from Guests
	where
		-- PR de seguimiento
		guPRFollow is not null
		-- PR de contacto diferente del PR de seguimiento
		and guPRInfo <> guPRFollow
		-- PR de seguimiento diferente de los PR de invitacion
		and guPRFollow not in (IsNull(guPRInvit1, ''), IsNull(guPRInvit2, ''), IsNull(guPRInvit3, ''))
		-- Fecha de seguimiento
		and ((@BasedOnArrival = 0 and guFollowD between @DateFrom and @DateTo)
		-- Fecha de llegada
		or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
		-- Con seguimiento
		and guFollow = 1))
		-- No Antes In & Out
		and guAntesIO = 0
		-- Disponible
		and guAvail = 1
		-- Lead Source		and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))		-- Sales Rooms		and (@SalesRooms = 'ALL' or gusr in (select item from split(@SalesRooms, ',')))		-- Countries		and (@Countries = 'ALL' or guco in (select item from split(@Countries, ',')))		-- Agencies		and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))		-- Markets		and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
	group by guPRFollow

return
end