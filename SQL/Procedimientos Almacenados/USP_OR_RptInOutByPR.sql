if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptInOutByPR]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptInOutByPR]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de In & Outs
**
** [wtorres]	28/Dic/2011 Elimine los campos de calificado y no calificado y agregue los campos de tour y programa de rescate.
**							Tambien elimine los campos no necesarios: guShowSeq, guBookD, guInvitD, loN, srN
**							Tambien agregue el campo descripcion de la agencia
** [wtorres]	28/Ene/2012 Agregue el campo descripcion del pais
**
*/
create procedure [dbo].[USP_OR_RptInOutByPR]
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000)	-- Claves de los Lead Source
as
set nocount on

select
	G.guPRInvit1 as PR1,
	P1.peN as PR1N,
	G.guID as GUID,
	G.guloInvit as Location,
	G.guHotel as Hotel,
	G.guRoomNum as Room,
	G.guPax as Pax,
	G.guLastName1 as LastName,
	G.guFirstName1 as FirstName,
	G.guag as Agency,
	A.agN as AgencyN,
	G.guco as Country,
	C.coN as CountryN,
	G.guShowD as ShowDate,
	G.guTimeInT as TimeIn,
	G.guTimeOutT as TimeOut,
	case when G.guBookD is not null and G.guInvitD is not null and G.guBookD = G.guInvitD then 1 else 0 end as Direct,
	G.guTour as Tour,
	G.guInOut as InOut,
	G.guWalkOut as WalkOut,
	G.guCTour as CourtesyTour,
	G.guSaveProgram as SaveProgram,
	G.guPRInvit2 as PR2,
	P2.peN as PR2N,
	G.guPRInvit3 as PR3,
	P3.peN as PR3N,
	G.guEntryHost as Host,
	G.guWcomments as Comments
from Guests G
	left join SalesRooms S on G.gusr = S.srID
	left join Personnel P1 on G.guPRInvit1 = P1.peID
	left join Personnel P2 ON G.guPRInvit2 = P2.peID
	left join Personnel P3 ON G.guPRInvit3 = P3.peID
	left join Agencies A on G.guag = A.agID
	left join Countries C on G.guco = C.coID
where
	-- Fecha de show
	G.guShowD between @DateFrom and @DateTo
	-- In & Out
	and G.guInOut = 1
	-- Lead Sources
	and G.guls in (select item from split(@LeadSources, ','))
order by G.guPRInvit1, G.guloInvit, G.guShowD, G.guID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

