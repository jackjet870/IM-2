if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetPRGroupAvailables]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetPRGroupAvailables]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de disponibles por PR y grupo
** 
** [wtorres]		11/Ago/2010 Created
** [wtorres]		18/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [edgrodriguez]	05/may/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/
CREATE function [dbo].[UFN_OR_GetPRGroupAvailables](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0,	-- Indica si se debe considerar quinielas
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada	
)
returns @Table table (
	PR varchar(10),
	[Group] int,
	Availables money
)
as
begin

insert @Table

-- Disponibles (PR Info)
-- =============================================
select
	G.guPRInfo,
	I.gjgx,
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then 1 else guShowsQty end)
from Guests G
	left join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- PR Info
	G.guPRInfo is not null
	-- Fecha de contacto
	and ((@BasedOnArrival = 0 and G.guInfoD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Contactado
	and G.guInfo = 1))
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Disponible
	and G.guAvail = 1
	-- Lead Source
	and G.guls in (select item from split(@LeadSources, ','))
group by G.guPRInfo, I.gjgx

-- Disponibles (PR 1)
-- =============================================
union all
select
	G.guPRInvit1,
	I.gjgx,
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT)
		else guShowsQty * dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT) end)
from Guests G
	left join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- PR 1
	G.guPRInvit1 is not null
	-- PR de contacto diferente de los PR de invitacion
	and G.guPRInfo not in (IsNull(G.guPRInvit1, ''), IsNull(G.guPRInvit2, ''), IsNull(G.guPRInvit3, ''))
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and G.guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and G.guInvit = 1))
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Disponible
	and G.guAvail = 1
	-- Lead Source
	and G.guls in (select item from split(@LeadSources, ','))
group by G.guPRInvit1, I.gjgx

-- Disponibles (PR 2)
-- =============================================
union all
select
	G.guPRInvit2,
	I.gjgx,
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT)
		else guShowsQty * dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT) end)
from Guests G
	left join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- PR 2
	G.guPRInvit2 is not null
	-- PR de contacto diferente de los PR de invitacion
	and G.guPRInfo not in (IsNull(G.guPRInvit1, ''), IsNull(G.guPRInvit2, ''), IsNull(G.guPRInvit3, ''))
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and G.guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and G.guInvit = 1))
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Disponible
	and G.guAvail = 1
	-- Lead Source
	and G.guls in (select item from split(@LeadSources, ','))
group by G.guPRInvit2, I.gjgx

-- Disponibles (PR 3)
-- =============================================
union all
select
	G.guPRInvit3,
	I.gjgx,
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT)
		else guShowsQty * dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT) end)
from Guests G
	left join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- PR 3
	G.guPRInvit3 is not null
	-- PR de contacto diferente de los PR de invitacion
	and G.guPRInfo not in (IsNull(G.guPRInvit1, ''), IsNull(G.guPRInvit2, ''), IsNull(G.guPRInvit3, ''))
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and G.guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and G.guInvit = 1))
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Disponible
	and G.guAvail = 1
	-- Lead Source
	and G.guls in (select item from split(@LeadSources, ','))
group by G.guPRInvit3, I.gjgx

return
end