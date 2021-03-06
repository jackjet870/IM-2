if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetPRDeposits]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetPRDeposits]
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de depositos por PR
** 
** [wtorres]		18/Sep/2009 Created
** [wtorres]		23/Sep/2009 Modified. Convertido a funcion. Agregue el parametro @LeadSources
** [wtorres]		18/Nov/2010 Modified. Agregue el parametro @BasedOnArrival y reemplace el parametro @ConsiderDirects por @Direct
** [edgrodriguez]	05/May/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/
CREATE function [dbo].[UFN_OR_GetPRDeposits](
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

-- Depositos (PR 1)
-- =============================================
select
	guPRInvit1,
	Sum(guRoomsQty * [dbo].UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT))
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
	-- Con Deposito
	and (guDeposit > 0 or guDepositTwisted > 0)
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
group by guPRInvit1

-- Depositos (PR 2)
-- =============================================
union all
select
	guPRInvit2,
	Sum(guRoomsQty * [dbo].UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT))
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
	-- Con Deposito
	and (guDeposit > 0 or guDepositTwisted > 0)
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
group by guPRInvit2

-- Depositos (PR 3)
-- =============================================
union all
select
	guPRInvit3,
	Sum(guRoomsQty * [dbo].UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT))
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
	-- Con Deposito
	and (guDeposit > 0 or guDepositTwisted > 0)
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
group by guPRInvit3

return
end