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
**
** Obtiene los catalogos utilizados en el modulo Outside:
**		1. Monedas
**		2. Idiomas
**		3. Estados civiles
**		4. Todos los regalos
**		5. Regalos de la locacion
**		6. Regalos de paquetes
**		7. Salas
**		8. Lead Sources
**		9. Personal de la locacion
**		10. Personal activo
**		11. Agencias
**		12. Hoteles
**		13. Paises
**		14. Estatus de invitados
**		15. Formas de pago
**		16. Estatus de asistencia
**		17. Dias de descanso
**		18. Lugares de pago
**    19. Tipos de tarjeta
**    20. Tipos de refund 
**
** [wtorres]	15/Ene/2009 Agregue los estatus de invitados
** [wtorres]	07/Abr/2009 Agregue todos lo regalos
** [wtorres]	12/Jun/2009 Agregue el campo de cantidad maxima de regalos
** [wtorres]	25/Jun/2009 Agregue los regalos de paquetes
** [wtorres]	23/Nov/2009 Agregue las agencias
** [wtorres]	26/Dic/2011 Agregue el campo descripcion de la agencia
** [wtorres]	27/Ene/2012 Agregue el campo descripcion del pais
** [wtorres]	08/May/2012 Ahora las agencias se ordenan por la descripcion
** [alesanchez] 25/Sep/2013 se agrega que se desplieguen todos las forma de pago
** [wtorres]	26/Sep/2013 Ahora se toman todos los paises, en lugar de solo los que tengan activado "Show in List"
** [wtorres]	22/Oct/2013 Agregue el campo de monto modificable a los regalos
** [wtorres]	02/Jul/2014 Agregue:
**							1. Personal activo
**							2. Estatus de asistencia
**							3. Dias de descanso
** [gmaya]		24/Jul/2014 Agregue los lugares de pago
** [wtorres]	27/Ago/2014 Agregue los precios de regalos
** [juogarcia]  14/12/2016  Agregue tipos de tarjetas de credito 
** [LorMartinez]  10/Dic/2015 Modified. Se agrega catalogo de tipos de revolucion
*/
CREATE procedure [dbo].[USP_OR_GetCatalogsOutside]
	@Location varchar(10),
	@LeadSource varchar(10),
	@PR varchar(10)
as
set nocount on

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
select distinct srID,srN 
from SalesRooms inner join PersLSSR on plLSSRID = srID
where plLSSR = 'SR' and plpe = @PR
order by srN

-- Lead Sources
select distinct lsID, lsN, lspg
from LeadSources
	inner join PersLSSR on plLSSRID = lsID
where plLSSR = 'LS' and plpe = @PR

-- Personal de la locacion
exec USP_OR_GetPersonnel @LeadSource, 'ALL', 'PR'

-- Personal activo
select peID, peN from Personnel where peA = 1 order by peN

-- Agencias
select agID, agN, agmk from Agencies where agA = 1 order by agN

-- Hoteles
select hoID from Hotels
	inner join Leadsources on hoar = lsar
	inner join Locations on lsID = lols
where lsID = @LeadSource and hoA = 1
order by hoID

-- Paises
Select coID, coN from Countries where coA = 1 order by coN

-- Estatus de invitados
select gsID, gsN, gsMaxAuthGifts from GuestStatus where gsA = 1 order by gsN

-- Formas de pago
select ptID, ptN from PaymentTypes where ptA = 1 order by ptN

-- Estatus de asistencia
select atID, atN from AssistanceStatus where atA = 1 order by atN

-- Dias de descanso
exec USP_OR_GetDaysOff 'GS', @LeadSource

-- Lugares de pago
select pcID, pcN from PaymentPlaces where pcA = 1 order by pcN

--Tarjetas de credito
select ccID,ccN,ccIsCreditCard from CreditCardTypes where ccA =1 order by ccN

--Tipos de devolcion
select rfID,rfN  from dbo.RefundTypes where rfA=1 order by rfID



GO