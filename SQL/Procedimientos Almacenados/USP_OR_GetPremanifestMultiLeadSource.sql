if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetPremanifestMultiLeadSource]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetPremanifestMultiLeadSource]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene el premanifiesto de un Multi Lead Source
** 
** [wtorres]	27/Dic/2011 Creado
** [wtorres]	28/Ene/2012 Agregue el campo descripcion del pais
** [wtorres]	30/Ene/2014 Reemplace el parametro de Lead Sources por el de Locacion
**
*/
create procedure  [dbo].[USP_OR_GetPremanifestMultiLeadSource]
	@Date datetime,					-- Fecha
	@Location varchar(10),			-- Clave de locacion
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@OnGroup int = 2,				-- Indica si se desean solo los huespedes que estan en grupo
									--		0. No en grupo
									--		1. En grupo
 									--		2. Sin filtro
	@Regen bit = 0,					-- Indica si es una locacion Regen
	@CurrentDate datetime = null	-- Fecha actual
as
set nocount on

-- =============================================
--					Bookings
-- =============================================
select
	G.guStatus,
	G.guID,
	G.guIdProfileOpera,
	G.guCheckIn,
	G.guls,
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
	G.guPRAssign,
	G.guAvail,
	G.guInfo,
	G.guPRInfo,
	G.guInvit,
	G.guInvitD,
	G.guInvitT,
	G.guPRInvit1,
	G.guQuinella,
	GA.gagu,
	G.guMembershipNum,
	G.guShow,
	G.guTour,
	G.guBookCanc,
	G.guSale,
	G.guLW,
	G.guComments,
	Cast(0 as bit) as guResch,
	G.guBookD,
	G.guBookT
from Guests G
	left join Agencies A on G.guag = A.agID
	left join Countries C on G.guco = C.coID
	left join GuestsAdditional GA on G.guID = GA.gaAdditional
where
	-- Fecha de booking
	G.guBookD = @Date
	-- Animacion (Multi Lead Source)
	and ((@Regen = 0 and G.guls in (select mlls from MultiLS where mllo = @Location))
	-- Regen (Invitaciones de la locacion Regen o Shows sin venta antes de la fecha actual de Multi Lead Source)
	or (@Regen = 1 and (G.guloInvit = @Location or (G.guls in (select mlls from MultiLS where mllo = @Location) and G.guShowD <= @CurrentDate and G.guSale = 0))))
	-- Mercados
	and (@Markets = 'ALL' or G.gumk in (select item from split(@Markets, ',')))
	-- En grupo
	and (@OnGroup = 2 or (@OnGroup = 0 and G.guOnGroup = 0 and G.guum <> 2) or (@OnGroup = 1 and (G.guOnGroup = 1 or G.guum = 2)))

-- =============================================
--					Reschedules
-- =============================================
union all
select
	G.guStatus,
	G.guID,
	G.guIdProfileOpera,
	G.guCheckIn,
	G.guls,
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
	G.guPRAssign,
	G.guAvail,
	G.guInfo,
	G.guPRInfo,
	G.guInvit,
	G.guInvitD,
	G.guInvitT,
	G.guPRInvit1,
	G.guQuinella,
	GA.gagu,
	G.guMembershipNum,
	G.guShow,
	G.guTour,
	G.guBookCanc,
	G.guSale,
	G.guLW,
	G.guComments,
	Cast(1 as bit) as guResch,
	G.guReschD as guBookD,
	G.guReschT as guBookT
from Guests G
	left join Agencies A on G.guag = A.agID
	left join Countries C on G.guco = C.coID
	left join GuestsAdditional GA on G.guID = GA.gaAdditional
where
	-- Fecha de reschedule
	G.guReschD = @Date
	-- Fecha de reschedule no del mismo dia de booking
	and G.guBookD <> G.guReschD
	-- Animacion (Multi Lead Source)
	and ((@Regen = 0 and G.guls in (select mlls from MultiLS where mllo = @Location))
	-- Regen (Invitaciones de la locacion Regen o Shows sin venta antes de la fecha actual de Multi Lead Source)
	or (@Regen = 1 and (G.guloInvit = @Location or (G.guls in (select mlls from MultiLS where mllo = @Location) and G.guShowD <= @CurrentDate and G.guSale = 0))))
	-- Mercados
	and (@Markets = 'ALL' or G.gumk in (select item from split(@Markets, ',')))
	-- En grupo
	and (@OnGroup = 2 or (@OnGroup = 0 and G.guOnGroup = 0 and G.guum <> 2) or (@OnGroup = 1 and (G.guOnGroup = 1 or G.guum = 2)))
order by G.guBookT, G.guLastName1

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

