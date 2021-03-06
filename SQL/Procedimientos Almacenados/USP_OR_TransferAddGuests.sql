if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferAddGuests]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferAddGuests]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Agrega las reservaciones en el proceso de transferencia
** 
** [wtorres]	13/Dic/2008 Modified. Agregue los campos de calificado y no calificado
** [wtorres]	29/Jun/2009 Modified. Agregue el campo de fecha de salida del sistema de Hotel
** [wtorres]	13/Jul/2010 Modified. Agregue los campos de tipo de reservacion y elimine la actualizacion de los campos de edad y fechas
**							de cumpleaños
** [wtorres]	21/Ago/2010 Modified. Agregue los campos de correo electronico, ciudad y estado
** [wtorres]	12/Ene/2011 Modified. Agregue el campo de compañia de la membresia
** [wtorres]	21/Sep/2011 Modified. Agregue el campo de id del perfil de Opera
** [wtorres] 	03/Nov/2011 Modified. Ahora tambien actualiza el campo de disponible por sistema
** [wtorres] 	01/Feb/2012 Modified. Ahora tambien actualiza los campos de fechas de cumpleaños, edades y tipo de habitacion
** [wtorres] 	25/Jun/2012 Modified. Ahora si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
** [wtorres] 	08/Abr/2013 Modified. Ahora tambien actualiza el campo de club de la membresia
** [wtorres] 	27/Ago/2015 Modified. Agregue el nombre y apellido de la reservacion
**
*/
create procedure [dbo].[USP_OR_TransferAddGuests]
as
set nocount on

insert into Guests (
	-- Lead Source (Hotel)
	guls,
	gulsOriginal,
	-- Folio de reservacion
	guHReservID,
	guHReservIDC,
	-- Sala de ventas
	gusr,
	-- Nombre
	guFirstName1,
	guFirstNameOriginal,
	-- Apellido
	guLastName1,
	guLastNameOriginal,
	-- Numero de habitacion
	guRoomNum,
	-- Tipo de habitacion
	gurt,
	-- Numero de huespedes
	guPax,
	-- Fecha de llegada
	guCheckInD,
	-- Fecha de salida
	guCheckOutD,
	guCheckOutHotelD,
	-- Correo electronico
	guEmail1,
	-- Ciudad
	guCity,
	-- Estado
	guState,
	-- Pais
	guco,
	-- Agencia
	guag,
	-- Mercado
	gumk,
	-- Club de la membresia
	gucl,
	-- Compañia de la membresia
	guCompany,
	-- Numero de membresia
	guMemberShipNum,
	-- Check In
	guCheckIn,
	-- En grupo
	guOnGroup,
	-- Contrato
	guO1,
	-- Tipo de huesped
	guO2,
	-- Motivo de indisponibilidad
	guum,
	-- Disponible
	guAvail,
	-- Originalmente disponible
	guOriginAvail,
	-- Disponible por sistema
	guAvailBySystem,
	-- Hotel
	guHotel,
	-- Idioma
	gula,
	-- Tarjeta de credito
	guCCType,
	-- Reservaciones enlazadas - Consecutivo (0 no dividida, 1 primera, 2 segunda, etc.)
	guDivResConsec,
	-- Reservaciones enlazdas - Hotel anterior
	guDivResLeadSource,
	-- Reservaciones enlazadas - Folio anterior
	guDivResResNum,
	-- Tipo de socio (G - Guest (Invitado) o M - Member (Socio))
	guGuestRef,
	-- Fechas de cumpleaños
	guBirthDate1,
	guBirthDate2,
	guBirthDate3,
	guBirthDate4,
	-- Edades
	guAge1,
	guAge2,
	-- Calificado
	guQ,
	-- No calificado
	guNQ,
	-- Tipo de reservacion
	guReservationType,
	-- Hotel anterior
	guHotelPrevious,
	-- Folio anterior
	guFolioPrevious,
	-- Id del perfil de Opera
	guIdProfileOpera
)
select
	-- Lead Source (Hotel)
	T.tls,
	T.tls,
	-- Folio reservacion
	T.tHReservID,
	T.tHReservID,
	-- Sala de ventas
	L.lssr,	
	-- Nombre
	T.tFirstName,
	T.tFirstName,
	-- Apellido
	T.tLastName,
	T.tLastName,
	-- Numero de habitacion
	T.tRoomNum,
	-- Tipo de habitacion
	T.trt,
	-- Numero de huespedes
	T.tPax,
	-- Fecha de llegada
	T.tCheckInD,
	-- Fecha de salida
	T.tCheckOutD,
	T.tCheckOutD,
	-- Correo electronico
	T.tEmail,
	-- Ciudad
	T.tCity,
	-- Estado
	T.tState,
	-- Pais
	T.tocoID,
	-- Agencia
	T.toagID,
	-- Mercado
	T.tmk,
	-- Club de la membresia
	T.tcl,
	-- Compañia de la membresia
	T.tCompany,
	-- Numero de membresia
	T.tMemberShipNum,
	-- Check In
	case when T.tGuestStatus = 'I' then 1 else 0 end,
	-- En grupo
	T.tOnGroup,
	-- Contrato
	T.tO1,
	-- Tipo de huesped
	T.tO2,
	-- Motivo de indisponibilidad
	T.tum,
	-- Disponible
	T.tAvail,
	-- Originalmente disponible
	case when T.tum = 0 then 1 else 0 end,
	-- Disponible por sistema
	case when T.tum = 0 then 1 else 0 end,
	-- Hotel
	L.lsN as Hotel,
	-- Idioma
	T.tla,
	-- Tarjeta de credito
	T.tCCType,
	-- Reservaciones enlazadas - Consecutivo (0 no dividida, 1 primera, 2 segunda, etc.)
	T.tDivResConsec,
	-- Reservaciones enlazadas - Hotel anterior
	T.tDivResLeadSource,
	-- Reservaciones enlazadas - Folio anterior
	T.tDivResResNum,
	-- Tipo de socio (G - Guest (Invitado) o M - Member (Socio))
	T.tGuestRef,
	-- Fechas de cumpleaños
	T.tBirthDate1,
	T.tBirthDate2,
	T.tBirthDate3,
	T.tBirthDate4,
	-- Edades
	T.tAge1,
	T.tAge2,
	-- Calificado
	1,
	-- No calificado
	0,
	-- Tipo de reservacion
	tReservationType,
	-- Hotel anterior
	tHotelPrevious,
	-- Folio anterior
	tFolioPrevious,
	-- Id del perfil de Opera
	tIdProfileOpera
from osTransfer T
	inner join LeadSources L on T.tls = L.lsID
where
	-- No reservaciones canceladas
	T.tGuestStatus <> 'X'
	-- No reservaciones check out
	and T.tGuestStatus <> 'O'
	-- No reservaciones que ya existan
	and not exists (
		select guHReservID
		from Guests G
			inner join LeadSources L on G.gulsOriginal = L.lsID
		where G.guHReservID = T.tHReservID and G.gulsOriginal = T.tls
			-- si el Lead Sorce usa Opera se valida que el huesped tenga perfil de Opera
			and ((L.lsUseOpera = 0) or (L.lsUseOpera = 1 and G.guIdProfileOpera <> ''))
	)

-- devolvemos el numero de registros agregados
select @@RowCount as TotalInserts

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

