if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetDateLeadSourcePRCountryMarketAgeContacts]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetDateLeadSourcePRCountryMarketAgeContacts]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de contactos por fecha, Lead Source, PR, pais, mercado y edad
** 
** [wtorres]	25/Jun/2010 Creado
** [wtorres]	20/Oct/2011 Ahora no se cuentan los rebooks
**
*/
create function [dbo].[UFN_OR_GetDateLeadSourcePRCountryMarketAgeContacts](
	@DateFrom datetime,	-- Fecha desde
	@DateTo datetime	-- Fecha hasta
)
returns @Table table (
	[Date] datetime,
	LeadSource varchar(10),
	PR varchar(10),
	Country varchar(25),
	Market varchar(10),
	Age tinyint,
	Contacts money
)
as
begin

insert @Table

-- Contactos
-- =============================================
select
	guInfoD,
	guls,
	guPRInfo,
	guco,
	gumk,
	guAge1,
	Count(*)
from Guests
where
	-- Fecha de contacto
	guInfoD between @DateFrom and @DateTo
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and guAntesIO = 0
group by guInfoD, guls, guPRInfo, guco, gumk, guAge1

-- Bookings donde los PR de invitacion 1 y contacto son diferentes
-- =============================================
union all
select
	guBookD,
	guls,
	guPRInvit1,
	guco,
	gumk,
	guAge1,
	Sum([dbo].UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3))
from Guests
where
	-- PR 1
	guPRInvit1 is not null
	-- PR de contacto diferente de los PR de invitacion
	and guPRInfo not in (IsNull(guPRInvit1, ''), IsNull(guPRInvit2, ''), IsNull(guPRInvit3, ''))
	-- Fecha de booking
	and guBookD between @DateFrom and @DateTo
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and guAntesIO = 0
group by guBookD, guls, guPRInvit1, guco, gumk, guAge1

-- Bookings donde los PR de invitacion 2 y contacto son diferentes
-- =============================================
union all
select
	guBookD,
	guls,
	guPRInvit2,
	guco,
	gumk,
	guAge1,
	Sum([dbo].UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3))
from Guests
where
	-- PR 2
	guPRInvit2 is not null
	-- PR de contacto diferente de los PR de invitacion
	and guPRInfo not in (IsNull(guPRInvit1, ''), IsNull(guPRInvit2, ''), IsNull(guPRInvit3, ''))
	-- Fecha de booking
	and guBookD between @DateFrom and @DateTo
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and guAntesIO = 0
group by guBookD, guls, guPRInvit2, guco, gumk, guAge1

-- Bookings donde los PR de invitacion 3 y contacto son diferentes
-- =============================================
union all
select
	guBookD,
	guls,
	guPRInvit3,
	guco,
	gumk,
	guAge1,
	Sum([dbo].UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3))
from Guests
where
	-- PR 3
	guPRInvit3 is not null
	-- PR de contacto diferente de los PR de invitacion
	and guPRInfo not in (IsNull(guPRInvit1, ''), IsNull(guPRInvit2, ''), IsNull(guPRInvit3, ''))
	-- Fecha de booking
	and guBookD between @DateFrom and @DateTo
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and guAntesIO = 0
group by guBookD, guls, guPRInvit3, guco, gumk, guAge1

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

