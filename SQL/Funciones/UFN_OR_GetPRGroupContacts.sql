if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetPRGroupContacts]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetPRGroupContacts]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de contactos por PR y grupo
** 
** [wtorres]	11/Ago/2010 Creado
** [wtorres]	18/Nov/2010 Agregue el parametro @BasedOnArrival
** [wtorres]	20/Oct/2011 Ahora no se cuentan los rebooks
**
*/
create function [dbo].[UFN_OR_GetPRGroupContacts](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	PR varchar(10),
	[Group] int,
	Contacts money
)
as
begin

insert @Table

-- Contactos (PR Info)
-- =============================================
select
	G.guPRInfo,
	I.gjgx,
	Count(*)
from Guests G
	left join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- PR de contacto
	G.guPRInfo is not null
	-- Fecha de contacto
	and ((@BasedOnArrival = 0 and G.guInfoD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Contactado
	and G.guInfo = 1))
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Lead Source
	and G.guls in (select item from split(@LeadSources, ','))
group by G.guPRInfo, I.gjgx
union all

-- Contactos (PR 1)
-- =============================================
select
	G.guPRInvit1,
	I.gjgx,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3))
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
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Lead Source
	and G.guls in (select item from split(@LeadSources, ','))
group by G.guPRInvit1, I.gjgx
union all

-- Contactos (PR 2)
-- =============================================
select
	G.guPRInvit2,
	I.gjgx,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3))
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
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Lead Source
	and G.guls in (select item from split(@LeadSources, ','))
group by G.guPRInvit2, I.gjgx
union all

-- Contactos (PR 3)
-- =============================================
select
	G.guPRInvit3,
	I.gjgx,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3))
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
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Lead Source
	and G.guls in (select item from split(@LeadSources, ','))
group by G.guPRInvit3, I.gjgx

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

