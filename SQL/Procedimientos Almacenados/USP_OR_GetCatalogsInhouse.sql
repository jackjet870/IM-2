USE [OrigosVCPalace];
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene los catalogos utilizados en los modulos inhouse (Register, Regen y Animacion)
**		1. Mercados
**		2. Motivos de indisponibilidad
**		3. Monedas
**		4. Idiomas
**		5. Estados civiles
**		6. Todos los regalos
**		7. Regalos de la locacion
**		8. Regalos de paquetes
**		9. Salas
**		10. Lead Sources
**		11. Personal de la locacion
**		12. Personal activo
**		13. Agencias
**		14. Hoteles
**		15. Estatus de invitados
**		16. Tipos de tarjetas de credito
**		17. Motivos no booking
**		18. Paises
**		19. Estatus de asistencia
**		20. Dias de descanso
**		21. Formas de pago
**    22. Tipos de devolucion
**
** [wtorres]	14/Ene/2009 Agregue los estatus de invitados
** [wtorres]	07/Abr/2009 Agregue:
**							1. Todos los regalos
**							2. Tipos de tarjetas de credito
** [wtorres]	12/Jun/2009 Agregue:
**							1. El campo de cantidad maxima de regalos
**							2. Lead Sources
** [wtorres]	25/Jun/2009 Agregue los regalos de paquetes
** [wtorres]	29/Jul/2010 Agregue los motivos no booking
** [wtorres]	26/Dic/2011 Agregue el campo descripcion de la agencia
** [wtorres]	24/Ene/2012 Ahora ya no se trae el motivo de indisponibilidad 0 - AVAILABLE
** [wtorres]	27/Ene/2012 Agregue los paises
** [wtorres]	08/May/2012 Ahora las agencias se ordenan por la descripcion
** [wtorres]	22/Oct/2013 Agregue el campo de monto modificable a los regalos
** [lchairez]	27/Nov/2013 Agregue:
**							1. Personal activo
**							2. Estatus de asistencia
**							3. Dias de descanso
** [wtorres]	16/Ene/2014 Agregue el campo de hotel y los que indican si un Lead Source es Regen o de animacion
** [wtorres]	27/Ago/2014 Agregue los precios de regalos
** [LorMartinez] 10/Dic/2015 Se agrega tipos de devolucion						
**
*/
CREATE procedure [dbo].[USP_OR_GetCatalogsInhouse]
	@Location varchar(10),
	@PR varchar(10),
	@LeadSource varchar(10)
as
set nocount on

-- Mercados
select mkID, mkN from Markets where mkA = 1 order by mkN

-- Motivos de indisponibilidad
select umID, umN, umA from UnavailMots where umID > 0 order by umN

-- Monedas
select cuID, cuN from Currencies where cuA = 1 order by cuN

-- Idiomas
select laID, laN from Languages where laA = 1 order by laN

-- Estados civiles
select msID, msN from MStatus where msA = 1 order by msN

-- Todos los regalos
select giID, giN, giWPax, giPrice1, giPrice2, giPrice3, giPrice4, giMaxQty, giAmountModifiable, giPublicPrice, giPriceMinor, giPriceExtraAdult
from Gifts where giA = 1 order by giN

-- Regalos de la locacion
select giID, giN, giWPax
from Gifts
	inner join GiftsByLoc on giID = glgi 
where gllo = @Location and giA = 1
order by giN

-- Regalos de paquetes
select gpPack, gpgi, gpQty, I.giPrice1, I.giPrice2, I.giPrice3, I.giPrice4
from GiftsPacks
	inner join Gifts P on gpPack = P.giID
	inner join Gifts I on gpgi = I.giID
	inner join GiftsByLoc on glgi = P.giID
where gllo = @Location and P.giA = 1 and I.giA = 1

-- Salas
select distinct srID, srN 
from SalesRooms
	inner join PersLSSR on plLSSRID = srID
where plLSSR = 'SR' and plpe = @PR

-- Lead Sources
select distinct lsID, lsN, lspg, lsho, lsRegen, lsAnimation
from LeadSources
	inner join PersLSSR on plLSSRID = lsID
where plLSSR = 'LS' and plpe = @PR

-- Personal de la locacion
exec USP_OR_GetPersonnel  @LeadSource, 'ALL', 'PR'

-- Personal activo
select peID, peN from Personnel where peA = 1 order by peN

-- Agencias
select agID, agN, agmk from Agencies where agA = 1 order by agN

-- Hoteles
select distinct H.hoID
from Hotels H
	inner join LeadSources L on L.lsar = H.hoar
	inner join Locations LO on LO.lols = L.lsID
where L.lsID = @LeadSource and H.hoA = 1
order by H.hoID

-- Estatus de invitados
select gsID, gsN, gsMaxAuthGifts from GuestStatus where gsA = 1 order by gsN

-- Tipos de tarjetas de credito
select ccID, ccN ,ccIsCreditCard from CreditCardTypes where ccA = 1 order by ccN

-- Motivos no booking
select nbID, nbN, nbA from NotBookingMotives order by nbN

-- Paises
select coID, coN from Countries where coA = 1 order by coN

-- Estatus de asistencia
select atID, atN from AssistanceStatus where atA = 1 order by atN

-- Dias de descanso
exec USP_OR_GetDaysOff 'GS', @LeadSource

-- Formas de Pago
select ptID, ptN from PaymentTypes where ptA = 1 order by ptN

-- Tipos de devolucion
select rfID, rfN from dbo.RefundTypes where rfA = 1 order by rfID
GO