if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetArrivalsMultiLeadSource]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetArrivalsMultiLeadSource]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta las llegadas de Multi Lead Source
** 
** [wtorres]	29/Ene/2014 Creado
**
*/
create procedure  [dbo].[USP_OR_GetArrivalsMultiLeadSource]
	@Date datetime,					-- Fecha
	@Location varchar(10),			-- Clave de locacion
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Available int = 2,				-- Filtro de disponibilidad
									--		0. No disponibles
									--		1. Disponibles
 									--		2. Sin filtro
 	@Contacted int = 2,				-- Filtro de contactacion
									--		0. No contactados
									--		1. Contactados
 									--		2. Sin filtro
 	@Invited int = 2,				-- Filtro de invitacion
									--		0. No invitados
									--		1. Invitados
 									--		2. Sin filtro
	@OnGroup int = 2,				-- Indica si se desean solo los huespedes que estan en grupo
									--		0. No en grupo
									--		1. En grupo
 									--		2. Sin filtro
 	@Regen bit = 0,					-- Indica si es una locacion Regen
 	@CurrentDate datetime = null	-- Fecha actual
as
set nocount on

select
	G.guStatus,
	G.guID,
	G.guls,
	G.guHReservID,
	G.guIdProfileOpera,
	G.guCheckIn,
	G.guRoomNum,
	G.guLastName1,
	G.guFirstName1,
	G.guCheckInD,
	G.guCheckOutD,
	G.guco,
	C.coN,
	G.guag,
	A.agN,
	G.guPax,
	G.guO1,
	G.guO2,
	G.gurt,
	G.guPRAssign,
	G.guAvail,
	G.guPRAvail,
	G.guum,
	G.guInfo,
	G.guInfoD,
	G.guPRInfo,
	G.guInvit,
	G.guBookD,
	G.guPRInvit1,
	G.guQuinella,
	GA.gagu,
	G.guMembershipNum,
	G.guBookCanc,
	G.guShow,
	G.guLW,
	G.guComments 
from Guests G
	left join Agencies A on A.agID = G.guag
	left join Countries C on C.coID = G.guco
	left join GuestsAdditional GA on GA.gaAdditional = G.guID
where
	-- Fecha de llegada
	G.guCheckInD = @Date
	-- Animacion (Multi Lead Source)
	and ((@Regen = 0 and G.guls in (select mlls from MultiLS where mllo = @Location))
	-- Regen (Invitaciones de la locacion Regen o Shows sin venta antes de la fecha actual de Multi Lead Source)
	or (@Regen = 1 and (G.guloInvit = @Location or (G.guls in (select mlls from MultiLS where mllo = @Location) and G.guShowD <= @CurrentDate and G.guSale = 0))))
	-- Mercados
	and (@Markets = 'ALL' or G.gumk in (select item from split(@Markets, ',')))
	-- Disponibles
	and (@Available = 2 or guAvail = @Available)
	-- Contactados
	and (@Contacted = 2 or guInfo = @Contacted)
	-- Invitados
	and (@Invited = 2 or guInvit = @Invited)
	-- En grupo
	and (@OnGroup = 2 or (@OnGroup = 0 and G.guOnGroup = 0 and G.guum <> 2) or (@OnGroup = 1 and (G.guOnGroup = 1 or G.guum = 2)))
order by G.guCheckIn desc, G.guLastName1

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

