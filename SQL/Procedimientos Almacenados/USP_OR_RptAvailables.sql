if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptAvailables]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptAvailables]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de huespedes diponibles
** 
** [wtorres]	28/Ene/2014 Creado
**
*/
create procedure  [dbo].[USP_OR_RptAvailables]
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
	G.guID,
	G.guHReservID,
	G.guRoomNum,
	G.guLastName1,
	G.guPax,
	G.guCheckInD,
	G.guCheckOutD,
	G.guco,
	C.coN,
	G.guag,
	A.agN,
	G.guAvail,
	G.guInfo,
	G.guInvit,
	G.guPRInvit1,
	G.guComments
from Guests G
	left join Agencies A on A.agID = G.guag
	left join Countries C on C.coID = G.guco
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
	-- No rebooks
	and G.guRef is null
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

