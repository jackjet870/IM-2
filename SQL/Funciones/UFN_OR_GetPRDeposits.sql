if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetPRDeposits]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetPRDeposits]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el número de depósitos por PR
** 
** [wtorres]	18/Sep/2009 Creado
** [wtorres]	23/Sep/2009 Convertido a función. Agregué el parámetro @LeadSources
** [wtorres]	18/Nov/2010 Agregué el parámetro @BasedOnArrival y reemplacé el parámetro @ConsiderDirects por @Direct
**
*/
create function [dbo].[UFN_OR_GetPRDeposits](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Clave de los Lead Sources
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	PR varchar(10),
	Deposits money
)
as
begin

insert @Table

-- Depósitos (PR 1)
-- =============================================
select
	guPRInvit1,
	Sum(guRoomsQty * [dbo].UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3))
from Guests
where
	-- PR 1
	guPRInvit1 is not null
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and guInvit = 1))
	-- Con Depósito
	and (guDeposit > 0 or guDepositTwisted > 0)
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
group by guPRInvit1

-- Depósitos (PR 2)
-- =============================================
union all
select
	guPRInvit2,
	Sum(guRoomsQty * [dbo].UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3))
from Guests
where
	-- PR 2
	guPRInvit2 is not null
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and guInvit = 1))
	-- Con Depósito
	and (guDeposit > 0 or guDepositTwisted > 0)
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
group by guPRInvit2

-- Depósitos (PR 3)
-- =============================================
union all
select
	guPRInvit3,
	Sum(guRoomsQty * [dbo].UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3))
from Guests
where
	-- PR 3
	guPRInvit3 is not null
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and guInvit = 1))
	-- Con Depósito
	and (guDeposit > 0 or guDepositTwisted > 0)
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
group by guPRInvit3

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

