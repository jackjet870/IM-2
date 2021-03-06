if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetDateLeadSourcePRCountryMarketAgeAvailables]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetDateLeadSourcePRCountryMarketAgeAvailables]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de disponibles por fecha, Lead Source, PR, pais, mercado y edad
** 
** [wtorres]		25/Jun/2010 Created
** [edgrodriguez]	05/May/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/

CREATE function [dbo].[UFN_OR_GetDateLeadSourcePRCountryMarketAgeAvailables](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@ConsiderQuinellas bit = 0	-- Indica si se debe considerar quinielas
)
returns @Table table (
	[Date] datetime,
	LeadSource varchar(10),
	PR varchar(10),
	Country varchar(25),
	Market varchar(10),
	Age tinyint,
	Availables money
)
as
begin

insert @Table

-- Disponibles
-- =============================================
select
	guCheckInD,
	guls,
	guPRInfo,
	guco,
	gumk,
	guAge1,
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
group by guCheckInD, guls, guPRInfo, guco, gumk, guAge1

-- Reservaciones donde los PR de invitación 1 y contacto son diferentes
-- =============================================
union all
select
	guBookD,
	guls,
	guPRInvit1,
	guco,
	gumk,
	guAge1,
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT) else 
		guShowsQty * dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT) end)
from Guests
where
	-- PR 1
	guPRInvit1 is not null
	-- PR de contacto diferente de los PR de invitación
	and guPRInfo not in (IsNull(guPRInvit1, ''), IsNull(guPRInvit2, ''), IsNull(guPRInvit3, ''))
	-- Fecha de reservación
	and guBookD between @DateFrom and @DateTo
	-- No Antes In & Out
	and guAntesIO = 0
	-- Disponible
	and guAvail = 1
group by guBookD, guls, guPRInvit1, guco, gumk, guAge1

-- Reservaciones donde los PR de invitación 2 y contacto son diferentes
-- =============================================
union all
select
	guBookD,
	guls,
	guPRInvit2,
	guco,
	gumk,
	guAge1,
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT) else 
		guShowsQty * dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT) end)
from Guests
where
	-- PR 2
	guPRInvit2 is not null
	-- PR de contacto diferente de los PR de invitación
	and guPRInfo not in (IsNull(guPRInvit1, ''), IsNull(guPRInvit2, ''), IsNull(guPRInvit3, ''))
	-- Fecha de reservación
	and guBookD between @DateFrom and @DateTo
	-- No Antes In & Out
	and guAntesIO = 0
	-- Disponible
	and guAvail = 1
group by guBookD, guls, guPRInvit2, guco, gumk, guAge1

-- Reservaciones donde los PR de invitación 3 y contacto son diferentes
-- =============================================
union all
select
	guBookD,
	guls,
	guPRInvit3,
	guco,
	gumk,
	guAge1,
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT) else 
		guShowsQty * dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT) end)
from Guests
where
	-- PR 3
	guPRInvit3 is not null
	-- PR de contacto diferente de los PR de invitación
	and guPRInfo not in (IsNull(guPRInvit1, ''), IsNull(guPRInvit2, ''), IsNull(guPRInvit3, ''))
	-- Fecha de reservación
	and guBookD between @DateFrom and @DateTo
	-- No Antes In & Out
	and guAntesIO = 0
	-- Disponible
	and guAvail = 1
group by guBookD, guls, guPRInvit3, guco, gumk, guAge1

return
end