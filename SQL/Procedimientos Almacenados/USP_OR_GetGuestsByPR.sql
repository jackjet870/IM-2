if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetGuestsByPR]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetGuestsByPR]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene los huespedes atentidos por un PR
** 
** [wtorres]	22/Jun/2011 Created
** [wtorres]	27/Jun/2011 Modified. Agregue la columna de tour
** [wtorres]	25/Jul/2011 Modified. Agregue las columnas de fecha de show y quiniela split
** [wtorres]	17/Ago/2011 Modified. Ahora se permite la busqueda en todos los Lead Sources y agregue los campos de Lead Source y sala de ventas
** [wtorres]	27/Dic/2011 Modified. Agregue el campo descripcion de la agencia
** [wtorres]	14/Ago/2014 Modified. Agregue los campos de descripcion del mercado y nombres de los PRs
**
*/
create procedure [dbo].[USP_OR_GetGuestsByPR] 
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSource varchar(10) = 'ALL',	-- Clave del Lead Source
	@PR varchar(10) = 'ALL',			-- Clave del PR
	@ConsiderAssign bit = 1,			-- Indica si se va a considerar las asignaciones
	@ConsiderContact bit = 1,			-- Indica si se va a considerar los contactos
	@ConsiderFollowUp bit = 1,			-- Indica si se va a considerar los seguimientos
	@ConsiderInvit bit = 1,				-- Indica si se va a considerar las invitaciones
	@ConsiderShow bit = 1,				-- Indica si se va a considerar los shows
	@ConsiderSale bit = 1,				-- Indica si se va a considerar las ventas
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
as
set nocount on

select
	G.guID,
	G.guLastName1,
	G.guFirstName1,
	G.guls,
	G.gusr,
	G.guag,
	A.agN,
	G.gumk,
	M.mkN,
	Cast(case when L.lspg = 'IH' and G.guHReservID is null then 1 else 0 end as bit) as [External],
	Cast(case when L.lspg = 'IH' and G.guRef is not null then 1 else 0 end as bit) as [Rebook],
	G.guCheckInD,
	G.guPRAssign,
	G.guAvail,
	G.guOriginAvail,
	G.guInfo,
	G.guInfoD,
	G.guPRInfo,
	PI.peN as PRInfoN,
	G.guFollow,
	G.guFollowD,
	G.guPRFollow,
	PF.peN as PRFollowN,
	G.guInvit,
	G.guBookD,
	G.guPRInvit1,
	P1.peN as PR1N,
	G.guPRInvit2,
	P2.peN as PR2N,
	G.guPRInvit3,
	P3.peN as PR3N,
	G.guQuinella,
	G.guRoomsQty,
	G.guShow,
	G.guShowD,
	G.guTour,
	G.guInOut,
	G.guWalkOut,
	G.guQuinellaSplit,
	G.guShowsQty,
	G.guSale,
	case when G.guSale = 0 then 0 else dbo.UFN_OR_GetGuestSales(G.guID) end as Sales,
	case when G.guSale = 0 then 0 else dbo.UFN_OR_GetGuestSalesAmount(G.guID) end as SalesAmount
from Guests G
	left join LeadSources L on L.lsID = G.guls
	left join Agencies A on A.agID = G.guag
	left join Markets M on M.mkID = G.gumk
	left join Personnel PI on PI.peID = G.guPRInfo
	left join Personnel PF on PF.peID = G.guPRFollow
	left join Personnel P1 on P1.peID = G.guPRInvit1
	left join Personnel P2 on P2.peID = G.guPRInvit2
	left join Personnel P3 on P3.peID = G.guPRInvit3
where
	-- Lead Source
	(@LeadSource = 'ALL' or G.guls = @LeadSource)
	-- No basado en fecha de llegada
	and ((@BasedOnArrival = 0
		-- Fecha de llegada
		and ((@ConsiderAssign = 1 and G.guCheckInD between @DateFrom and @DateTo
		-- Asignado
		and G.guPRAssign is not null)
		-- Fecha de contacto
		or (@ConsiderContact = 1 and G.guInfoD between @DateFrom and @DateTo)
		-- Fecha de seguimiento
		or (@ConsiderFollowUp = 1 and G.guFollowD between @DateFrom and @DateTo)
		-- Fecha de booking
		or (@ConsiderInvit = 1 and G.guBookD between @DateFrom and @DateTo)
		-- Fecha de show
		or (@ConsiderShow = 1 and G.guShowD between @DateFrom and @DateTo)))
	-- Basado en fecha de llegada
	or (@BasedOnArrival = 1
		-- Fecha de llegada
		and G.guCheckInD between @DateFrom and @DateTo
		-- Asignado
		and ((@ConsiderAssign = 1 and G.guPRAssign is not null)
		-- Contactado
		or (@ConsiderContact = 1 and G.guInfo = 1)
		-- Con seguimiento
		or (@ConsiderFollowUp = 1 and G.guFollow = 1)
		-- Invitado
		or (@ConsiderInvit = 1 and G.guInvit = 1)
		-- Con show
		or (@ConsiderShow = 1 and G.guShow = 1))))
	-- Con venta
	and (@ConsiderSale = 0 or (@ConsiderSale = 1 and G.guSale = 1))
	-- Booking no cancelado
	and (@ConsiderInvit = 0 or (@ConsiderInvit = 1 and G.guBookCanc = 0))
	-- Todos los PR
	and (@PR = 'ALL'
	-- PR asignado
	or ((@ConsiderAssign = 1 and G.guPRAssign = @PR)
	-- PR de contacto
	or (@ConsiderContact = 1 and G.guPRInfo = @PR)
	-- PR de seguimiento
	or (@ConsiderFollowUp = 1 and G.guPRFollow = @PR)
	-- PR de invitacion
	or ((@ConsiderInvit = 1 or @ConsiderShow = 1) and (G.guPRInvit1 = @PR or G.guPRInvit2 = @PR or G.guPRInvit3 = @PR))))
order by G.guls, G.guCheckInD, G.guInfo desc, G.guInfoD, G.guInvit desc, G.guBookD, G.guShow desc, G.guShowD, G.guSale desc

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

