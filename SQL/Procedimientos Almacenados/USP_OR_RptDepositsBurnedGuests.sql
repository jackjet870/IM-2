if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptDepositsBurnedGuests]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptDepositsBurnedGuests]
GO

set QUOTED_IDENTIFIER on 
GO
set ANSI_NULLS on 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de depositos quemados de los huespedes
**		1. Huespedes
**		2. Monedas
**		3. Formas de pago
** 
** [axperez]	28/Oct/2013 Creado
**
*/
create procedure [dbo].[USP_OR_RptDepositsBurnedGuests]
	@DateFrom as DateTime,			-- Fecha desde
	@DateTo as DateTime,			-- Fecha hasta
	@LeadSources as varchar(8000)	-- Claves de los Lead Sources
as

set nocount on

-- 1. Huespedes
-- =============================================
select 
	G.guID, 
	G.guInvitD,
	G.guBookD,
	dbo.UFN_OR_GetFullName(G.guLastname1, G.guFirstName1) as Guest, 
	G.guHotel, 
	G.guls, 
	G.gusr,
	G.guPRInvit1, 
	G.guOutInvitNum,
	P.peN,	
	G.gucu,
	G.gupt,
	G.guDepositTwisted
into #Guests
from Guests G
	left join Personnel P on G.guPRInvit1 = P.peID
where
	-- Lead Sources
	G.guls in (select item from split(@LeadSources, ','))
	-- Fecha de booking
	and G.guBookD between @DateFrom and @DateTo 
	-- Con depositos quemados
	and G.guDepositTwisted > 0
order by G.guID

select * from #Guests

-- 2. Monedas
-- =============================================
select distinct cuID, cuN
from #Guests
	left join Currencies on gucu = cuID

-- 3. Formas de Pago
-- =============================================
select distinct P.ptID, P.ptN 
from #Guests G
	left join PaymentTypes P on G.gupt = P.ptID
	
	
GO
set QUOTED_IDENTIFIER OFF 
GO
set ANSI_NULLS on 
GO