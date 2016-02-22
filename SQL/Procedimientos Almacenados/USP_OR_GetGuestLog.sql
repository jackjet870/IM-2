if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetGuestLog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetGuestLog]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene el registro historico de un huesped
** 
** [wtorres]		16/Jun/2010 Created
** [wtorres]		22/Sep/2010 Modified. Agregue los campos de disponibilidad
** [wtorres]		08/Dic/2010 Modified. Ahora se ordena por fecha y hora ascendentemente
** [wtorres]		03/Nov/2011 Modified. Agregue el campo de disponible por sistema
** [wtorres]		27/Feb/2013 Modified. Agregue los campos de hora de booking y hora de reschedule
** [wtorres]		17/Ene/2014 Modified. Agregue el campo de folio de reservacion y los nombres del personal
** [lormartinez]	08/Sep/2014 Modified. Agregue los campos de reimpresion
** [wtorres]		23/Mar/2015 Modified. Agregue el campo de fecha de show
** [wtorres]		28/Ago/2015 Modified. Agregue los campos de los nombres y apellidos del huesped y su acompañante
**
*/
create procedure [dbo].[USP_OR_GetGuestLog] 
	@Guest int -- Clave del huesped
as
set nocount on;

select 
	L.glChangedBy,
	C.peN as ChangedByN,
	L.glID,
	-- Sala de ventas
	L.glsr,
	-- Reservacion
	L.glHReservID,
	-- Nombre y apellido del huesped
	L.glLastName1,
	L.glFirstName1,
	-- Nombre y apellido del acompañante
	L.glLastName2,
	L.glFirstName2,
	-- Disponibilidad
	L.glAvailBySystem,
	L.glOriginAvail,
	L.glAvail,
	case when L.glAvail = 1 then '' else U.umN end as umN,
	L.glPRAvail,
	PA.peN as PRAvailN,
	-- Contacto
	L.glInfoD,
	L.glPRInfo,
	PI.peN as PRInfoN,
	-- Seguimiento
	L.glFollowD,
	L.glPRFollow,
	PF.peN as PRFollowN,
	-- Invitacion
	L.glBookD,
	L.glBookT,
	L.glReschD,
	L.glReschT,
	L.glPRInvit1,
	P1.peN as PRInvit1N,
	L.glPRInvit2,
	P2.peN as PRInvit2N,
	L.glBookCanc,
	-- Show
	L.glLiner1,
	L1.peN as Liner1N,
	L.glLiner2,
	L2.peN as Liner2N,
	L.glCloser1,
	C1.peN as Closer1N,
	L.glCloser2,
	C2.peN as Closer2N,
	L.glCloser3,
	C3.peN as Closer3N,
	L.glExit1,
	E1.peN as Exit1N,
	L.glExit2,
	E2.peN as Exit2N,
	L.glPodium,
	PO.peN as PodiumN,
	L.glVLO,
	VLO.peN as VLON,
	L.glShow,
	L.glShowD,
	L.glQ,
	L.glInOut,
	L.glWalkOut,
	L.glCTour,
	L.glReimpresion,
	RM.rmN
from GuestLog L
	left join Personnel C on C.peID = L.glChangedBy
	left join Personnel PA on PA.peID = L.glPRAvail
	left join Personnel PI on PI.peID = L.glPRInfo
	left join Personnel PF on PF.peID = L.glPRFollow
	left join Personnel P1 on P1.peID = L.glPRInvit1
	left join Personnel P2 on P2.peID = L.glPRInvit2
	left join Personnel L1 on L1.peID = L.glLiner1
	left join Personnel L2 on L2.peID = L.glLiner2
	left join Personnel C1 on C1.peID = L.glCloser1
	left join Personnel C2 on C2.peID = L.glCloser2
	left join Personnel C3 on C3.peID = L.glCloser3
	left join Personnel E1 on E1.peID = L.glExit1
	left join Personnel E2 on E2.peID = L.glExit2
	left join Personnel PO on PO.peID = L.glPodium
	left join Personnel VLO on VLO.peID = L.glVLO
	left join UnavailMots U on U.umID = L.glum
	left join ReimpresionMotives RM on RM.rmID = L.glrm
where L.glgu = @Guest
order by L.glID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

