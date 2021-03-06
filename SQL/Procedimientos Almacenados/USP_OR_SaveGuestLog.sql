if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_SaveGuestLog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_SaveGuestLog]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Agrega un registro en el historico de un huesped si su informacion relevante cambio
** 
** [wtorres]		20/May/2010 Modified. Agregue los campos de seguimiento
** [wtorres]		21/Sep/2010 Modified. Agregue los campos de disponibilidad
** [wtorres]		03/Nov/2011 Modified. Agregue el campo de disponible por sistema
** [wtorres]		27/Feb/2013 Modified. Agregue el campo de hora de reschedule
** [wtorres]		17/Ene/2014 Modified. Agregue el campo de folio de reservacion
** [lormartinez]	08/Sep/2014 Modified. Agregue los campos de reimpresion
** [wtorres]		23/Mar/2015 Modified. Agregue el campo de fecha de show
** [wtorres]		28/Ago/2015 Modified. Agregue los campos de los nombres y apellidos del huesped y su acompañante
** [emoguel]		10/Nov/2016 Modified. Agregue los campos Front To Back, Front To Middle, Liner 3, Closer 4 , Exit Closer 3
**
*/
create procedure [dbo].[USP_OR_SaveGuestLog]
    @Guest int,				-- Clave del huesped
    @HoursDif smallint,		-- Horas de diferencia
    @ChangedBy varchar(10)	-- Clave del usuario que esta haciendo el cambio
as
set nocount on

declare @Count int

-- determinamos si cambio algun campo relevante
select @Count = Count(*)
from GuestLog inner join Guests on glgu = guID
where
	glgu = @Guest
	-- Sala de ventas
	and (glsr = gusr or (glsr is null and gusr is null))
	-- Reservacion
	and (glHReservID = guHReservID or (glHReservID is null and guHReservID is null))
	-- Nombre y apellido del huesped
	and (glLastName1 = guLastName1 or (glLastName1 is null and guLastName1 is null))
	and (glFirstName1 = guFirstName1 or (glFirstName1 is null and guFirstName1 is null))
	-- Nombre y apellido del acompañante
	and (glLastName2 = guLastName2 or (glLastName2 is null and guLastName2 is null))
	and (glFirstName2 = guFirstName2 or (glFirstName2 is null and guFirstName2 is null))
	-- Disponibilidad
	and (glAvailBySystem = guAvailBySystem or (glAvailBySystem is null and guAvailBySystem is null))
	and (glOriginAvail = guOriginAvail or (glOriginAvail is null and guOriginAvail is null))
	and (glAvail = guAvail or (glAvail is null and guAvail is null))
	and (glum = guum or (glum is null and guum is null))
	and (glPRAvail = guPRAvail or (glPRAvail is null and guPRAvail is null))
	-- Contacto
	and (glInfoD = guInfoD or (glInfoD is null and guInfoD is null))
	and (glPRInfo = guPRInfo or (glPRInfo is null and guPRInfo is null))
	-- Seguimiento
	and (glFollowD = guFollowD or (glFollowD is null and guFollowD is null))
	and (glPRFollow = guPRFollow or (glPRFollow is null and guPRFollow is null))
	-- Invitacion
	and (glBookD = guBookD or (glBookD is null and guBookD is null))
	and (glBookT = guBookT or (glBookT is null and guBookT is null))
	and (glReschD = guReschD or (glReschD is null and guReschD is null))
	and (glReschT = guReschT or (glReschT is null and guReschT is null))
	and (glPRInvit1 = guPRInvit1 or (glPRInvit1 is null and guPRInvit1 is null))
	and (glPRInvit2 = guPRInvit2 or (glPRInvit2 is null and guPRInvit2 is null))
	and (glQuinella = guQuinella or (glQuinella is null and guQuinella is null))
	and (glBookCanc = guBookCanc or (glBookCanc is null and guBookCanc is null))
	-- Show
	and (glLiner1 = guLiner1 or (glLiner1 is null and guLiner1 is null))
	and (glLiner2 = guLiner2 or (glLiner2 is null and guLiner2 is null))
	and (glCloser1 = guCloser1 or (glCloser1 is null and guCloser1 is null))
	and (glCloser2 = guCloser2 or (glCloser2 is null and guCloser2 is null))
	and (glCloser3 = guCloser3 or (glCloser3 is null and guCloser3 is null))
	and (glExit1 = guExit1 or (glExit1 is null and guExit1 is null))
	and (glExit2 = guExit2 or (glExit2 is null and guExit2 is null))
	and (glPodium = guPodium or (glPodium is null and guPodium is null))
	and (glVLO = guVLO or (glVLO is null and guVLO is null))
	and (glShow = guShow or (glShow is null and guShow is null))
	and (glShowD = guShowD or (glShowD is null and guShowD is null))
	and (glQ = guQ or (glQ is null and guQ is null))
	and (glInOut = guInOut or (glInOut is null and guInOut is null))
	and (glWalkOut = guWalkOut or (glWalkOut is null and guWalkOut is null))
	and (glCTour = guCTour or (glCTour is null and guCTour is null))
	and (glTO = guTO or (glTO is null and guTO is null))
	and (glLW = guLW or (glLW is null and guLW is null))
	and (glNW = guNW or (glNW is null and guNW is null))
    and glID in (select Max(glID) from GuestLog where glgu = @Guest)
    and (glReimpresion = guReimpresion or (glReimpresion is null and guReimpresion is null))
	and (glrm = gurm or (glrm is null and glrm is null))
	and (glLiner3 = guLiner3 or (glLiner3 is NULL and guLiner3 is NULL))
	and (glCloser4 = guCloser4 or (glCloser4 is NULL and guCloser4 is NULL))
	and (glExit3 = guExit3 or (glExit3 is NULL and guExit3 is NULL))
	and (glFTB1 = guFTB1 or (glFTB1 is NULL and guFTB1 is NULL))
	and (glFTB2 = guFTB2 or (glFTB2 is NULL and guFTB2 is NULL))
	and (glFTM1 = guFTM1 or (glFTM1 is NULL and guFTM1 is NULL))
	and (glFTM2 = guFTM2 or (glFTM2 is NULL and guFTM2 is NULL))
    
-- agregamos un registro en el historico, si cambio algun campo relevante
insert into GuestLog (
	glChangedBy,
	glID,
	-- Huesped
	glgu,
	-- Sala de ventas
	glsr,
	-- Reservacion
	glHReservID,
	-- Nombre y apellido del huesped
	glLastName1, glFirstName1,
	-- Nombre y apellido del acompañante
	glLastName2, glFirstName2,
	-- Disponibilidad
	glAvailBySystem, glOriginAvail, glAvail, glum, glPRAvail,
	-- Contacto
	glInfoD, glPRInfo,
	-- Seguimiento
	glFollowD, glPRFollow,
	-- Invitacion
	glBookD, glBookT, glReschD, glReschT, glPRInvit1, glPRInvit2, glQuinella, glBookCanc,
	-- Show
	glShow, glShowD, glQ, glInOut, glWalkOut, glCTour, glLiner1, glLiner2, glCloser1, glCloser2, glCloser3, glExit1, glExit2, glPodium, glVLO,
	glTO, glLW, glNW, glReimpresion, glrm,glLiner3,glCloser4,glExit3,glFTB1,glFTB2,glFTM1,glFTM2
)
select
	@ChangedBy,
	DateAdd(hh, @HoursDif, GetDate()),
	-- Huesped
	guID,
	-- Sala de ventas
	gusr,
	-- Reservacion
	guHReservID,
	-- Nombre y apellido del huesped
	guLastName1, guFirstName1,
	-- Nombre y apellido del acompañante
	guLastName2, guFirstName2,
	-- Disponibilidad
	guAvailBySystem, guOriginAvail, guAvail, guum, guPRAvail,
	-- Contacto
	guInfoD, guPRInfo,
	-- Seguimiento
	guFollowD, guPRFollow,
	-- Invitacion
	guBookD, guBookT, guReschD, guReschT, guPRInvit1, guPRInvit2, guQuinella, guBookCanc,
	-- Show
	guShow, guShowD, guQ, guInOut, guWalkOut, guCTour, guLiner1, guLiner2, guCloser1, guCloser2, guCloser3, guExit1, guExit2, guPodium, guVLO,
	guTO, guLW, guNW, guReimpresion, gurm, guLiner3, guCloser4, guExit3, guFTB1, guFTB2, guFTM1, guFTM2
from Guests
where guID = @Guest and @Count = 0

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

