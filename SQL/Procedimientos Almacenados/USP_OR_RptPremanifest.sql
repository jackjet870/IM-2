if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptPremanifest]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptPremanifest]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de premanifiesto
** 
** [wtorres]	03/Jun/2011 Creado
** [wtorres]	28/Dic/2011 Agregue el campo descripcion de la agencia
** [wtorres]	28/Ene/2012 Agregue el campo descripcion del pais
** [wtorres]	30/Ene/2014 Agregue los parametros @MultiLeadSource, @Regen y @CurrentDate
**
*/
create procedure  [dbo].[USP_OR_RptPremanifest]
	@Date datetime,					-- Fecha
	@PlaceID varchar(10) = 'ALL',	-- Clave de Lead Source o Locacion
	@SalesRoom varchar(10) = 'ALL',	-- Clave de sala de ventas
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@OnGroup int = 2,				-- Indica si se desean solo los huespedes que están en grupo
									--		0. No en grupo
									--		1. En grupo
 									--		2. Sin filtro
	@MultiLeadSource bit = 0,		-- Indica si es Multi Lead Source
	@Regen bit = 0,					-- Indica si es una locacion Regen
	@CurrentDate datetime = null	-- Fecha actual
as
set nocount on

-- =============================================
--					Bookings
-- =============================================
select
	S.srN,
	L.loN,
	G.guID,
	G.guHotel,
	G.guRoomNum,
	G.guLastName1,
	G.guPax,
	G.guCheckInD,
	G.guCheckOutD,
	G.guco,
	C.coN,
	G.guag,
	A.agN,
	G.guInvitD,
	G.guInvitT,
	G.guBookT,
	G.guPRInvit1,
	G.guBookCanc,
	Cast(0 as bit) as guResch,
	G.guShow,
	G.guSale,
	case when G.guResch = 1 then 'Rescheduled' else 'Active     ' end as InvitType,
	dbo.AddString(dbo.AddStringLabel(dbo.UFN_OR_GetGuestDepositsAsString(G.guID), 
		dbo.UFN_OR_FormatDeposit(G.guDepositTwisted, G.gucu), ', ', 'Burned: '),
		G.guComments, '  |||  ')
	as guComments
from Guests G
	inner join SalesRooms S on G.gusr = S.srID
	inner join Locations L on G.guloInvit = L.loID
	left join Agencies A on G.guag = A.agID
	left join Countries C on G.guco = C.coID
where
	-- Fecha de booking
	G.guBookD = @Date
	-- No Multi Lead Source
	and ((@MultiLeadSource = 0
		-- Lead Source
		and (@PlaceID = 'ALL' or G.guls = @PlaceID)
		-- Sala de ventas
		and (@SalesRoom = 'ALL' or G.gusr = @SalesRoom))
	-- Multi Lead Source
	or (@MultiLeadSource = 1
		-- Animacion (Multi Lead Source)
		and ((@Regen = 0 and G.guls in (select mlls from MultiLS where mllo = @PlaceID))
		-- Regen (Invitaciones de la locacion Regen o Shows sin venta antes de la fecha indicada de Multi Lead Source)
		or (@Regen = 1 and (G.guloInvit = @PlaceID or (G.guls in (select mlls from MultiLS where mllo = @PlaceID) and G.guShowD <= @CurrentDate and G.guSale = 0))))))
	-- Booking no cancelado
	and G.guBookCanc = 0 
	-- Mercados
	and (@Markets = 'ALL' or G.gumk in (select item from split(@Markets, ',')))
	-- En grupo
	and (@OnGroup = 2 or (@OnGroup = 0 and G.guOnGroup = 0 and G.guum <> 2) or (@OnGroup = 1 and (G.guOnGroup = 1 or G.guum = 2)))

-- =============================================
--					Reschedules
-- =============================================
union
select
	S.srN,
	L.loN,
	G.guID,
	G.guHotel,
	G.guRoomNum,
	G.guLastName1,
	G.guPax,
	G.guCheckInD,
	G.guCheckOutD,
	G.guco,
	C.coN,
	G.guag,
	A.agN,
	G.guInvitD,
	G.guInvitT,
	G.guReschT,
	G.guPRInvit1,
	G.guBookCanc,
	Cast(1 as bit),
	G.guShow,
	G.guSale,
	'Active     ', 
	dbo.AddString(dbo.AddStringLabel(dbo.UFN_OR_GetGuestDepositsAsString(G.guID), 
		dbo.UFN_OR_FormatDeposit(G.guDepositTwisted, G.gucu), ', ', 'Burned: '),
		G.guComments, '  |||  ')
	as guComments
from Guests G
	inner join SalesRooms S on G.gusr = S.srID
	inner join Locations L on G.guloInvit = L.loID
	left join Agencies A on G.guag = A.agID
	left join Countries C on G.guco = C.coID
where
	-- Fecha de reschedule
	G.guReschD = @Date
	-- Fecha de reschedule no del mismo dia de booking
	and G.guBookD <> G.guReschD
	-- No Multi Lead Source
	and ((@MultiLeadSource = 0
		-- Lead Source
		and (@PlaceID = 'ALL' or G.guls = @PlaceID)
		-- Sala de ventas
		and (@SalesRoom = 'ALL' or G.gusr = @SalesRoom))
	-- Multi Lead Source
	or (@MultiLeadSource = 1
		-- Animacion (Multi Lead Source)
		and ((@Regen = 0 and G.guls in (select mlls from MultiLS where mllo = @PlaceID))
		-- Regen (Invitaciones de la locacion Regen o Shows sin venta antes de la fecha indicada de Multi Lead Source)
		or (@Regen = 1 and (G.guloInvit = @PlaceID or (G.guls in (select mlls from MultiLS where mllo = @PlaceID) and G.guShowD <= @CurrentDate and G.guSale = 0))))))
	-- Booking no cancelado
	and G.guBookCanc = 0
	-- Mercados
	and (@Markets = 'ALL' or G.gumk in (select item from split(@Markets, ',')))
	-- En grupo
	and (@OnGroup = 2 or (@OnGroup = 0 and G.guOnGroup = 0 and G.guum <> 2) or (@OnGroup = 1 and (G.guOnGroup = 1 or G.guum = 2)))
order by S.srN, L.loN, InvitType, G.guBookT, G.guLastName1

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

