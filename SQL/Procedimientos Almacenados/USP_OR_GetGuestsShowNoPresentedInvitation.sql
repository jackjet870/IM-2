USE [OrigosVCPalace]
GO
/****** Object:  StoredProcedure [dbo].[USP_OR_GetGuestsShowNoPresentedInvitation]    Script Date: 01/21/2016 16:09:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta los huespedes que hicieron show, pero que no presentaron invitacion al momento de presentarse en la sala de ventas
** 
** [wtorres]		12/May/2015 Created
** [lchairezReload]	19/Ene/2016 Se agrega filtro de lead source
** [lchairezReload]	21/Ene/2016 Se agregan las columnas Deposit, Received, Currency y PaymentType
**
*/
ALTER procedure [dbo].[USP_OR_GetGuestsShowNoPresentedInvitation]
	@DateFrom datetime,	-- Fecha desde
	@DateTo datetime,	-- Fecha hasta
	@LeadSources VARCHAR(MAX) = 'ALL' --lista lead sources
as
set nocount on

select
	G.guID as GuestID,
	L.lsN as LeadSource,
	G.guOutInvitNum as OutInvit,
	G.guLastName1 as LastName,
	G.guFirstName1 as FirstName,
	G.guShowD as ShowDate,
	bd.bdAmount as Deposit, 
	bd.bdReceived as Received, 
	c.cuN as Currency, 
	pt.ptN as PaymentType
from Guests G
	left join LeadSources L on L.lsID = G.gulsOriginal
	join BookingDeposits bd on G.guID = bd.bdgu
	join Currencies c on bd.bdcu = c.cuID
	join PaymentTypes pt on bd.bdpt = pt.ptID
where
	-- Fecha de show
	G.guShowD between @DateFrom and @DateTo
	-- No presentaron invitacion
	and G.guPresentedInvitation = 0
	-- Outhouse
	and L.lspg = 'OUT'
	-- lead source
	AND (@LeadSources = 'ALL' OR G.guls IN (SELECT item FROM dbo.Split(@LeadSources,',')))
order by G.guShowD, L.lsN, G.guLastName1, G.guFirstName1

