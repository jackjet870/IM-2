USE [OrigosVCPalace]
GO
/****** Object:  StoredProcedure [dbo].[USP_OR_RptDepositsByPR]    Script Date: 09/27/2016 09:54:37 ******/
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
** 
** [wtorres]	09/May/2009 Ahora se pasa la lista de Lead Sources como un solo parametro
**							Ahora se devuelve la descripcion de las monedas
** [wtorres]	10/Jul/2013 Renombrado de sprptPreManifDeps a USP_OR_RptDepositsByPR
** [erosado]	26/09/2016	Se´eliminó el campo Burned y se agregó el campo CxC dentro de Huespedes.
*/
ALTER procedure [dbo].[USP_OR_RptDepositsByPR]
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
	0 as CxC
	
	
into #Guests
from BookingDeposits D
	left join Guests G on G.guID = D.bdgu
	left join Personnel P on G.guPRInvit1 = P.peID 
where
	-- Lead Sources
	G.guls in (select item from split(@LeadSources, ','))
	-- Fecha de booking
	and G.guBookD between @DateFrom and @DateTo 
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
	SUM(guDeposit) - SUM(guDepositReceived) as CxC
	
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
	
	
