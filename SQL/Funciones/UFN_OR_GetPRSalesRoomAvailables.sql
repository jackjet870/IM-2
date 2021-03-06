if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetPRSalesRoomAvailables]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetPRSalesRoomAvailables]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de disponibles por PR y sala
** 
** [axperez]		03/Dic/2013 Created. Copiado de UFN_OR_GetPRAvailables
** [edgrodriguez]	05/May/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/
CREATE function [dbo].[UFN_OR_GetPRSalesRoomAvailables](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Clave de los Lead Sources
	@ConsiderQuinellas bit = 0,	-- Indica si se debe considerar quinielas
	@ConsiderFollow bit = 0,	-- Indica si se debe considerar seguimiento
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada	
)
returns @Table table (
	PR varchar(10),
	SalesRoom varchar(10),
	Availables money
)
as
begin

insert @Table

-- Disponibles (PR Info)
-- =============================================
select
	guPRInfo,
	gusr, 
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
	-- Lead Source
	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
group by guPRInfo, gusr

-- Disponibles (PR 1)
-- =============================================
union all
select
	guPRInvit1,
	gusr, 
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT)
		else guShowsQty * dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT) end)
from Guests
where
	-- PR 1
	guPRInvit1 is not null
	-- PR de contacto diferente de los PR de invitación
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
	-- Lead Source
	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
group by guPRInvit1, gusr

-- Disponibles (PR 2)
-- =============================================
union all
select
	guPRInvit2,
	gusr, 
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT)
		else guShowsQty * dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT) end)
from Guests
where
	-- PR 2
	guPRInvit2 is not null
	-- PR de contacto diferente de los PR de invitación
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
	-- Lead Source
	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
group by guPRInvit2, gusr

-- Disponibles (PR 3)
-- =============================================
union all
select
	guPRInvit3,
	gusr, 
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT)
		else guShowsQty * dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT) end)
from Guests
where
	-- PR 3
	guPRInvit3 is not null
	-- PR de contacto diferente de los PR de invitación
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
	-- Lead Source
	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
group by guPRInvit3, gusr

-- Disponibles (PR de seguimiento)
-- =============================================
if @ConsiderFollow = 1
	insert @Table
	select
		guPRFollow,
		gusr, 
		Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then 1 else guShowsQty end)
	from Guests
	where
		-- PR de seguimiento
		guPRFollow is not null
		-- PR de contacto diferente del PR de seguimiento
		and guPRInfo <> guPRFollow
		-- PR de seguimiento diferente de los PR de invitación
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
		-- Lead Source
		and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
	group by guPRFollow, gusr

return
end