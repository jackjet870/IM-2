if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetAvailables]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetAvailables]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta los huespedes disponibles de un Lead Source
** 
** [wtorres]	29/Ene/2014 Creado
**
*/
create procedure  [dbo].[USP_OR_GetAvailables]
	@CurrentDate datetime,			-- Fecha actual
	@LeadSource varchar(10),		-- Clave de Lead Source
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
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
	G.guCheckIn,
	G.guID,
	G.guIdProfileOpera,
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
	G.guCCType,
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
where
	-- Fecha de salida
	G.guCheckOutD >= @CurrentDate
	-- Lead Source
	and G.guls = @LeadSource
	-- Invitaciones no canceladas
	and G.guBookCanc = 0
	-- Disponibles no invitados
	and ((G.guAvail = 1 and G.guInvit = 0)
	-- Invitados no show antes de la fecha actual
	or (G.guBookD < @CurrentDate and G.guShow = 0))
	-- Mercados
	and (@Markets = 'ALL' or G.gumk in (select item from split(@Markets, ',')))
	-- Contactados
	and (@Contacted = 2 or guInfo = @Contacted)
	-- Invitados
	and (@Invited = 2 or guInvit = @Invited)
	-- En grupo
	and (@OnGroup = 2 or (@OnGroup = 0 and G.guOnGroup = 0 and G.guum <> 2) or (@OnGroup = 1 and (G.guOnGroup = 1 or G.guum = 2)))
order by G.guRoomNum

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

