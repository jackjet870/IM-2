if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetArrivals]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetArrivals]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta las llegadas de un Lead Source
** 
** [wtorres]	28/Ene/2014 Creado
** [lchairez]	10/Jun/2014 Agregué el campo GroupN
*/
create procedure  [dbo].[USP_OR_GetArrivals]
	@Date datetime,					-- Fecha
	@LeadSource varchar(10),		-- Clave de Lead Source
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
	@OnGroup int = 2				-- Indica si se desean solo los huespedes que estan en grupo
									--		0. No en grupo
									--		1. En grupo
 									--		2. Sin filtro
as
set nocount on

select
	G.guStatus,
	G.guID,
	G.gulsOriginal,
	G.guHReservID,
	G.guIdProfileOpera,
	'' as Reservation,
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
	G.guO1,
	G.guO2,
	G.guCCType,
	G.gurt,
	G.guPRAssign,
	G.guAvail,
	G.guPRAvail,
	G.guum,
	G.guInfo,
	G.guInfoD,
	G.guPRInfo,
	G.guFollow,
	G.guFollowD,
	G.guPRFollow,
	G.guInvit,
	G.guBookD,
	G.guPRInvit1,
	G.gunb,
	G.guNoBookD,
	G.guPRNoBook,
	G.guQuinella,
	GA.gagu,
	G.guGroup,
	GR.gbN as GroupN,
	'' as Equity,
	G.gucl,
	CL.clN,
	G.guCompany,
	G.guMembershipNum,
	dbo.UFN_OR_GetLastMembershipType(G.guMembershipNum) as mtN,
	G.guBookCanc,
	G.guShow,
	G.guLW,
	G.guPRNote,
	'' as Notes,
	G.guComments
from Guests G
	left join Agencies A on A.agID = G.guag
	left join Countries C on C.coID = G.guco
	left join Clubs CL on CL.clID = G.gucl
	left join GuestsAdditional GA on GA.gaAdditional = G.guID
	left join Groups GR on G.guID = GR.gbgu
where
	-- Fecha de llegada
	G.guCheckInD = @Date
	-- Lead Source
	and G.guls = @LeadSource
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

