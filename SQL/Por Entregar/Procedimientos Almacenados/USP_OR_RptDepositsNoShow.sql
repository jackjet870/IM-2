USE [OrigosVCPalace]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptDepositsNoShow]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptDepositsNoShow]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de depositos por PR
**		1. Huespedes
**		2. Monedas
**		3. Formas de pago
** 
** [alesanchez]		14/Oct/2013 Ahora se pasa la lista de Lead Sources como un solo parametro
**								Ahora se devuelve la descripcion de las monedas
**								Renombrado de sprptDepositsNoShow a USP_OR_RptDepositsNoShow
**								Agregue las formas de pago
** [edgrodriguez]	26/Sep/2016 Se cambia el filtro guBookD por guInfoD
**
*/
ALTER procedure [dbo].[USP_OR_RptDepositsNoShow]
	@DateFrom as DateTime,			-- Fecha desde
	@DateTo as DateTime,			-- Fecha hasta
	@LeadSources as varchar(8000)	-- Claves de los Lead Sources
as
set nocount on

-- Huespedes
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
	D.bdcu as gucu, 
	D.bdpt as gupt, 
	D.bdAmount as guDeposit, 
	D.bdreceived as guDepositReceived,
	0 as guDepositTwisted
into #Guests
from BookingDeposits D
	left join Guests G on G.guID = D.bdgu
	left join Personnel P on G.guPRInvit1 = P.peID 
where
	-- Lead Sources
	G.guls in (select item from split(@LeadSources, ','))
	-- Fecha de booking
	and G.guInfoD between @DateFrom and @DateTo 
	and guShow = 0 
union all
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
	0 as guDeposit, 
	0 as guDepositReceived,
	G.guDepositTwisted
from Guests G
	left join Personnel P on G.guPRInvit1 = P.peID
where
	-- Lead Sources
	G.guls in (select item from split(@LeadSources, ','))
	-- Fecha de booking
	and G.guInfoD between @DateFrom and @DateTo 
	-- Con depositos quemados
	and G.guDepositTwisted > 0
	and guShow = 0 
-- 1. Huespedes
-- =============================================
select 
	guID, 
	guInvitD, 
	guBookD,
	Guest, 
	guHotel, 
	guls, 
	gusr,
	guPRInvit1, 
	guOutInvitNum,
	peN,	
    gucu,
	gupt, 
	Sum(guDeposit) as guDeposit, 
	Sum(guDepositReceived) as guDepositReceived,
	Sum(guDepositTwisted) as guDepositTwisted
from #Guests
group by guID, guInvitD, guBookD, Guest, guHotel, guls, gusr, guPRInvit1, guOutInvitNum, peN, gucu, gupt
order by guID, gucu, gupt

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
	
	
