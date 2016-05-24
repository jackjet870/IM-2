USE [OrigosVCPalace]
GO
/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRSalesRoomContacts]    Script Date: 05/05/2016 17:32:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de contactos por PR
** 
** [axperez]	03/Dic/2013 Creado. copeado de UFN_OR_GetPRContacts
**
*/
ALTER function [dbo].[UFN_OR_GetPRSalesRoomContacts](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Clave de los Lead Sources
	@ConsiderFollow bit = 0,	-- Indica si se debe considerar seguimiento
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	PR varchar(10),
	SalesRoom varchar(10), 
	Contacts money	
)
as
begin

insert @Table

-- Contactos (PR Info)
-- =============================================
select
	guPRInfo,
	gusr,
	Count(*)
from Guests
where
	-- PR de contacto
	guPRInfo is not null
	-- Fecha de contacto
	and ((@BasedOnArrival = 0 and guInfoD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Contactado
	and guInfo = 1))
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
group by guPRInfo, gusr
union all

-- Contactos (PR 1)
-- =============================================
select
	guPRInvit1,
	gusr, 
	Sum(dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT))
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
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
group by guPRInvit1, gusr
union all

-- Contactos (PR 2)
-- =============================================
select
	guPRInvit2,
	gusr, 
	Sum(dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT))
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
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
group by guPRInvit2, gusr
union all

-- Contactos (PR 3)
-- =============================================
select
	guPRInvit3,
	gusr, 
	Sum(dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT))	
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
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
group by guPRInvit3, gusr

-- Contactos (PR de seguimiento)
-- =============================================
if @ConsiderFollow = 1
	insert @Table
	select
		guPRFollow,
		gusr,
		Count(*)
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
		-- No Rebook
		and guRef is null
		-- No Antes In & Out
		and guAntesIO = 0
		-- Lead Source
		and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
	group by guPRFollow, gusr

return
end



