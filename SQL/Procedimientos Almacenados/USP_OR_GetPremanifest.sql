if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetPremanifest]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetPremanifest]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene el premanifiesto de un Lead Source
** 
** [wtorres]	18/Jul/2011 Creado
** [wtorres]	27/Dic/2011 Agregue los campos de id del perfil de Opera y descripcion de la agencia
** [wtorres]	28/Ene/2012 Agregue el campo descripcion del pais
** [wtorres]	06/Abr/2013 Agregue el campo club
** [wtorres]	03/Abr/2014 Ahora se obtiene el tipo de membresia de la venta mas reciente con la que viaja la reservacion
**
*/
create procedure  [dbo].[USP_OR_GetPremanifest]
	@Date datetime,					-- Fecha
	@LeadSource varchar(10),		-- Clave de Lead Source
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@OnGroup int = 2				-- Indica si se desean solo los huespedes que estan en grupo
									--		0. No en grupo
									--		1. En grupo
 									--		2. Sin filtro
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
	G.guRoomNum,
	G.guLastName1,
	G.guFirstName1,
	G.guCheckInD,
	G.guCheckOutD,
	G.guco,
	C.coN,
	G.guag,
	A.agN,
	A.agcl,
	G.guPax,
	G.guO2,
	G.guCCType,
	G.guPRAssign,
	G.guAvail,
	G.guInfo,
	G.guPRInfo,
	G.guFollow,
	G.guFollowD,
	G.guPRFollow,
	G.guInvit,
	G.guInvitD,
	G.guInvitT,
	Cast(0 as bit) as guResch,
	G.guBookD,
	G.guBookT,
	G.guPRInvit1,
	G.gunb,
	G.guNoBookD,
	G.guPRNoBook,
	G.guQuinella,
	GA.gagu,
	G.guGroup,
	'' as Equity,
	G.gucl,
	CL.clN,
	G.guCompany,
	G.guMembershipNum,
	dbo.UFN_OR_GetLastMembershipType(G.guMembershipNum) as mtN,
	G.guShow,
	G.guTour,
	G.guBookCanc,
	G.guSale,
	G.guLW,
	G.guPRNote,
	'' as Notes,
	G.guComments
from Guests G
	left join Agencies A on G.guag = A.agID
	left join Countries C on G.guco = C.coID
	left join GuestsAdditional GA on G.guID = GA.gaAdditional
	left join Clubs CL on CL.clID = G.gucl
where
	-- Fecha de booking
	G.guBookD = @Date
	-- Lead Source
	and G.guls = @LeadSource
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
	G.guRoomNum,
	G.guLastName1,
	G.guFirstName1,
	G.guCheckInD,
	G.guCheckOutD,
	G.guco,
	C.coN,
	G.guag,
	A.agN,
	A.agcl,
	G.guPax,
	G.guO2,
	G.guCCType,
	G.guPRAssign,
	G.guAvail,
	G.guInfo,
	G.guPRInfo,
	G.guFollow,
	G.guFollowD,
	G.guPRFollow,
	G.guInvit,
	G.guInvitD,
	G.guInvitT,
	Cast(1 as bit) as guResch,
	G.guReschD as guBookD,
	G.guReschT as guBookT,
	G.guPRInvit1,
	G.gunb,
	G.guNoBookD,
	G.guPRNoBook,
	G.guQuinella,
	GA.gagu,
	G.guGroup,
	'' as Equity,
	G.gucl,
	CL.clN,
	G.guCompany,
	G.guMembershipNum,
	dbo.UFN_OR_GetLastMembershipType(G.guMembershipNum) as mtN,
	G.guShow,
	G.guTour,
	G.guBookCanc,
	G.guSale,
	G.guLW,
	G.guPRNote,
	'' as Notes,
	G.guComments
from Guests G
	left join Agencies A on G.guag = A.agID
	left join Countries C on G.guco = C.coID
	left join GuestsAdditional GA on G.guID = GA.gaAdditional
	left join Clubs CL on CL.clID = G.gucl
where
	-- Fecha de reschedule
	G.guReschD = @Date
	-- Fecha de reschedule no del mismo dia de booking
	and G.guBookD <> G.guReschD
	-- Lead Source
	and G.guls = @LeadSource
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

