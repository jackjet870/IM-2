if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptInOut]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptInOut]
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
** [wtorres]	13/Ago/2011 Elimine los campos de calificado y no calificado y agregue los campos de tour y programa de rescate.
**							Tambien elimine los campos no necesarios: guShowSeq, guloInvit, guBookD, guInvitD
** [wtorres]	28/Dic/2011 Agregue el campo descripcion de la agencia
** [wtorres]	28/Ene/2012 Agregue el campo descripcion del pais
**
*/
create procedure [dbo].[USP_OR_RptInOut]
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@SalesRooms varchar(8000)	-- Claves de salas de ventas
as
set nocount on

select
	S.srN as SalesRoom,
	L.loN as Location,
	G.guID as GUID,
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
	G.guPRInvit1 as PR1,
	P1.peN as PR1N,
	G.guPRInvit2 as PR2,
	P2.peN as PR2N,
	G.guPRInvit3 as PR3,
	P3.peN as PR3N,
	G.guEntryHost as Host,
	G.guWcomments as Comments
from Guests G
	left join SalesRooms S on G.gusr = S.srID
	left join Locations L on G.guloInvit = L.loID
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
	-- Salas de ventas
	and G.gusr in (select item from split(@SalesRooms, ','))
order by S.srN, L.loN, G.guPRInvit1, G.guShowD

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

