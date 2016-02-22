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
** Obtiene los catalogos utilizados en el modulo Host:
**		1. Monedas
**		2. Estados civiles
**		3. Locaciones
**		4. Tipos de ventas
**		5. Tipos de membresias
**		6. Todo el personal
**		7. Personal activo
**		8. Personal de la sala
**		9. PRs
**		10. Liners
**		11. Closers
**		12. Exit Closers
**		13. Podiums
**		14. Host(ess) de llegada
**		15. Host(ess) de regalos
**		16. Host(ess) de salida
**		17. Verificadores legales
**		18. Capitanes de PRs
**		19. Capitanes de Liners
**		20. Capitanes de Closers
**		21. Regalos
**		22. Regalos de paquetes
**		23. Cargar A
**		24. Lead Sources
**		25. Salas
**		26. Agencias
**		27. Hoteles
**		28. Configuracion
**		29. Estatus de invitados
**		30. Idiomas
**		31. Equipos de vendedores
**		32. Tipos de pagos
**		33. Tipos de tarjetas de credito
**		34. Estatus de asistencia
**		35. Dias de descanso
**		36. Paises
**		37. Lugares de pago
**		38. Origenes de pago
**		39. Bancos
**		40. Roles
**    41. Tipos de devolucion
** [wtorres]	21/Ene/2009 Modified. Agregue:
**							1. Personal activo
**							2. Estatus de invitados
**							3. Idiomas
**							4. Equipos de vendedores
**							5. Tipos de pagos
**							6. Tipos de tarjetas de credito
**							7. Estatus de asistencia
** [wtorres]		07/Abr/2009 Modified. Agregue los dias de descanso
** [wtorres]		12/Jun/2009 Modified. Agregue el campo de cantidad maxima de regalos
** [wtorres]		20/Jun/2009 Modified. Agregue los siguientes campos de regalos:
**								1. Paquete. Indica si un regalo es paquete
**								2. Monetario. Indica si el regalo es monetario
**								3. Monto
**								4. Clave del producto de la tarjeta de regalos
** [wtorres]		25/Jun/2009 Modified. Agregue los regalos de paquetes
** [wtorres]		17/Jun/2011 Modified. Ahora los dias de descanso se obtienen mediante un procedimiento almacenado
** [wtorres]		26/Dic/2011 Modified. Agregue el campo descripcion de la agencia
** [wtorres]		27/Ene/2012 Modified. Agregue los paises
** [wtorres]		08/May/2012 Modified. Ahora las agencias se ordenan por la descripcion
** [wtorres]		23/Sep/2013 Modified. Agregue el campo de monto modificable a los regalos
** [lchairez]		27/Nov/2013 Modified. Ahora los dias de descanso se obtienen por tipo de lugar
** [wtorres]		19/Nov/2013 Modified. Agregue el campo de categoria a los tipos de venta, los campos de rango de montos a los tipos de membresia
**								y el campo de vendedor al personal
** [wtorres]		16/Ene/2014 Modified. Agregue el campo de hotel y los que indican si un Lead Source es Regen o de animacion
** [wtorres]		26/Feb/2014 Modified. Agregue el campo de tasa de IVA a la consulta de configuracion
** [gmaya]			14/Jul/2014 Modified. Agregue los lugares de pago
** [gmaya]			13/Ago/2014 Modified. Agregue los origenes de pago y precios de regalos
** [wtorres]		10/Sep/2014 Modified. Agregue el campo de tipo venta a la consulta de regalos
** [wtorres]		17/Sep/2014 Modified. Agregue el campo de promocion de PVP a la consulta de regalos
** [loremartinez]	29/Jun/2015 Modified. Se agrega catalogo de bancos por salesroom
** [wtorres]		13/Ago/2015 Modified. Agregue la consulta de Todo el personal
** [wtorres]		04/Sep/2015 Modified. Agregue la consulta de roles
** [LorMartinez]  10/Dic/2015 Modified. Se agrega catalogo de tipos de revolucion
**
*/
CREATE procedure [dbo].[USP_OR_GetCatalogsHost]
	@SalesRoom varchar(10)
as
set nocount on

declare @Area varchar(10)

-- Area de la sala
select @Area = srar from SalesRooms where srID = @SalesRoom

-- Monedas
select cuID, cuN from Currencies where cuA = 1 order by cuN

-- Estados civiles
select msID, msN from MStatus where msA = 1 order by msN

-- Locaciones
select loID, lols, losr,loN,lspg, loRegen, loAnimacion 
	from Locations
	inner join LeadSources on lols = lsID and loA = 1
	order by loN

-- Tipos de ventas
select stID, stN, stUpdate, ststc from SaleTypes where stA = 1 order by stN

-- Tipos de membresias
select mtID, mtN, mtLevel, mtFrom, mtTo from MembershipTypes where mtA = 1 order by mtN

-- Todo el personal
select peID, peN, peSalesmanID from Personnel order by peN

-- Personal activo
select peID, peN from Personnel where peA = 1 order by peN

-- Personal de la sala
exec USP_OR_GetPersonnel 'ALL', @SalesRoom

-- PRs
exec USP_OR_GetPersonnel 'ALL', @SalesRoom, 'PR'

-- Liners
exec USP_OR_GetPersonnel 'ALL', @SalesRoom, 'LINER'

-- Closers
exec USP_OR_GetPersonnel 'ALL', @SalesRoom, 'CLOSER'

-- Exit Closers
exec USP_OR_GetPersonnel 'ALL', @SalesRoom, 'EXIT'

-- Podiums
exec USP_OR_GetPersonnel 'ALL', @SalesRoom, 'PODIUM'

-- Host(ess) de llegada
exec USP_OR_GetPersonnel 'ALL', @SalesRoom, 'HOSTENTRY'

-- Host(ess) de regalos
exec USP_OR_GetPersonnel 'ALL', @SalesRoom, 'HOSTGIFTS'

-- Host(ess) de salida
exec USP_OR_GetPersonnel 'ALL', @SalesRoom, 'HOSTEXIT'

-- Verificadores legales
exec USP_OR_GetPersonnel 'ALL', @SalesRoom, 'VLO'

-- Capitanes de PRs
exec USP_OR_GetPersonnel 'ALL', @SalesRoom, 'PRCAPT'

-- Capitanes de Liners
exec USP_OR_GetPersonnel 'ALL', @SalesRoom, 'LINERCAPT'

-- Capitanes de Closers
exec USP_OR_GetPersonnel 'ALL', @SalesRoom, 'CLOSERCAPT'

-- Regalos
select giID, giN, giA, giWPax, giWFolio, giPrice1, giPrice2, giPrice3, giPrice4, giMaxQty, giPack,
	giMonetary, giAmount, giProductGiftsCard, giPVPPromotion, giAmountModifiable, giPublicPrice, giPriceMinor, giPriceExtraAdult, giSale
from Gifts order by giN

-- Regalos de paquetes
select gpPack, gpgi, gpQty, I.giPrice1, I.giPrice2, I.giPrice3, I.giPrice4
from GiftsPacks
	inner join Gifts P on gpPack = P.giID
	inner join Gifts I on gpgi = I.giID
where P.giA = 1 and I.giA = 1

-- Cargar A
select ctID, ctCalcType from ChargeTo order by ctID

-- Lead Sources
select lsID, lsN, lspg, lsMaxAuthGifts, lsho, lsRegen, lsAnimation from LeadSources where lsar = @Area and lsA = 1 order by lsN

-- Salas
select srID, srN, srwh from SalesRooms where srar = @Area order by srN

-- Agencias
select agID, agN, agmk from Agencies where agA = 1 order by agN

-- Hoteles
select H.hoID
from Hotels H
	inner join SalesRooms S on S.srar = H.hoar
where S.srID = @SalesRoom and H.hoA = 1 
order by H.hoID

-- Configuracion
select ocWelcomeCopies, ocVATRate from osConfig

-- Estatus de invitados
select gsID, gsN, gsMaxAuthGifts from GuestStatus where gsA = 1 order by gsN

-- Idiomas
select laID, laN from Languages where laA = 1 order by laN

-- Equipos de vendedores
select tsID, tsN from TeamsSalesmen where tssr = @SalesRoom and tsA = 1 order by tsN

-- Tipos de pagos
select ptID, ptN from PaymentTypes where ptA = 1 order by ptN

-- Tipos de tarjetas de credito
select ccID, ccN, ccIsCreditCard from CreditCardTypes where ccA = 1 order by ccN

-- Estatus de asistencia
select atID, atN from AssistanceStatus where atA = 1 order by atN

-- Dias de descanso
exec USP_OR_GetDaysOff 'SA', @SalesRoom

-- Paises
select coID, coN from Countries where coA = 1 order by coN

-- Lugares de pago
select pcID, pcN from PaymentPlaces where pcA = 1 order by pcN

-- Origenes de pago
select sbID, sbN from SourcePayments where sbA = 1 order by sbN

-- Bancos
select BK.bkID, BK.bkN
from banks BK
	inner join BanksBySalesRooms BS on BS.bsbk = bkID
where BS.bssr = @SalesRoom

-- Roles
select roID, roN from Roles order by roN

--Tipos de devolcion
select rfID,rfN  from dbo.RefundTypes where rfA=1 order by rfID
GO