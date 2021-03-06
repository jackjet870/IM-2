if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptGiftsManifest]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptGiftsManifest]
GO

SET QUOTED_IDENTIFIER on 
GO
SET ANSI_NULLS on 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de manifiesto de regalos:
**		1. Recibos de regalos
**		2. Regalos
**		3. Monedas
**		4. Formas de pago
** 
** [wtorres]	09/May/2009 Modified. Ahora se pasa la lista de salas como un solo parametro
**							Elimine la consulta que determina si los recibos estan cerrados
** [wtorres]	26/Abr/2013 Modified. Ahora devuelve la descripcion de la agencia en lugar de su clave.
**							Elimine las consultas de numero de depositos y numero de depositos quemados
**							Elimine el campo Short Name de la consulta de recibos de regalos
** [wtorres]	28/May/2013 Modified. Agregue los campos de PR
** [alesanchez]	14/Oct/2013 Modified. Agregue las formas de pago
** [alesanchez]	29/Oct/2013 Modified. Elimine el campo ReceiptNumber, ya que no se utiliza
** [axperez]	09/Dic/2013 Modified. Agregue el campo hotel quemado
** [lchairez]	26/May/2014 Modified. Agregue el campo fecha de cancelacion
** [gmaya]	    28/Jul/2014 Modified. Se excluyen los recibos que no tengan regalo
** [wtorres]    21/Ago/2014 Modified. Agregue los parametros @Categories, @Gifts y @Status
** [wtorres]	27/Ago/2014 Modified. Agregue el campo de Show
**
*/
create procedure [dbo].[USP_OR_RptGiftsManifest]
	@DateFrom as datetime,				-- Fecha desde
	@DateTo as datetime,				-- Fecha hasta
	@SalesRooms as varchar(8000),		-- Claves de salas de ventas
	@Categories varchar(8000) = 'ALL',	-- Claves de categorias de regalos
	@Gifts varchar(8000) = 'ALL',		-- Claves de regalos
	@Status tinyint = 0					-- Estatus
										--		0. Sin filtro
										--		1. Solo incluye recibos activos
										--		2. Solo incluye recibos cancelados
as
	set noCount on

-- Recibos de regalos (Tabla temporal)
-- =============================================
select
	R.grgu as GuestID,
	GU.guHReservID as ReservID,
	R.grD as Date,
	R.grGuest as Guest,
	R.grGuest2 as Guest2,
	R.grPax as Pax,
	A.agN as Agency,
	R.grMemberNum as Membership,
	R.grlo as Location,
	R.grpe as PR,
	P.peN as PRN,
	R.grHost as Host,
	H.peN as HostN,
	R.grID as ReceiptID,
	GU.guShow as Show,
	GU.guHotelB as HotelBurned,
	R.grCancel as Cancel,
	R.grCancelD as CancelledDate,
	R.grDeposit as Deposited,
	R.grDepositTwisted as Burned,
	R.grcu as Currency,
	R.grpt as PaymentType,	
	R.grTaxiOut as TaxiOut,
	R.grTaxiOutDiff as TaxiOutDiff,
	D.gegi as Gift,
	IsNull(D.geQty, 0) as Quantity,
	IsNull(D.geAdults, 0) as Adults,
	IsNull(D.geMinors, 0) as Minors,
	IsNull(D.geFolios, 0) as Folios,
	IsNull(D.gePriceA, 0) as PriceAdults, 
	IsNull(D.gePriceM, 0) as PriceMinors,
	R.grComments as Comments
into #GiftsReceipts
from GiftsReceipts R
	-- Se excluyen los recibos que no tengan regalo
	inner join GiftsReceiptsC D on D.gegr = R.grID
	left join Guests GU on GU.guID = R.grgu
	left join Agencies A on A.agID = GU.guag
	left join Personnel P on P.peID = R.grpe
	left join Personnel H on H.peID = R.grHost
	left join Gifts G on G.giID = D.gegi
where
	-- Sala de ventas
	R.grsr in (select item from split(@SalesRooms, ','))
	-- Fecha del recibo de regalos
	and R.grD between @DateFrom and @DateTo
	-- Categorias de regalos
	and (@Categories = 'ALL' or G.gigc in (select item from split(@Categories, ',')))
	-- Regalos
	and (@Gifts = 'ALL' or D.gegi in (select item from split(@Gifts, ',')))
	-- Estatus
	and (@Status = 0
		or (@Status = 1 and R.grCancel = 0 and D.geCancelPVPPromo = 0)
		or (@Status = 2 and R.grCancel = 1 or D.geCancelPVPPromo = 1))
	-- No recibos de regalos cargados al personal (CxC)
	and R.grct not in ('PR', 'LINER', 'CLOSER')
order by R.grID

-- 1. Recibos de regalos
-- =============================================
-- desglosamos los paquetes de regalos que se deben desempaquetar

-- obtenemos los paquetes a desglosar
select * into #Packages
from #GiftsReceipts
where Gift in (select giID from Gifts where giPack = 1 and giUnPack = 1)
	
-- insertamos los registros de los componentes de paquete en la tabla #GiftsReceipts,
-- utilizando #Packages para generar la consulta respetando la estructura de #GiftsReceipts
insert into #GiftsReceipts
select
	P.GuestID,
	P.ReservID,
	P.Date,
	P.Guest,
	P.Guest2,
	P.Pax,
	P.Agency,
	P.Membership,
	P.Location,
	P.PR,
	P.PRN,
	P.Host,
	P.HostN,
	P.ReceiptID,
	P.Show,
	P.HotelBurned,
	P.Cancel,
	P.CancelledDate,
	P.Deposited,
	P.Burned,
	P.Currency,
	P.PaymentType,
	P.TaxiOut,
	P.TaxiOutDiff,
	-- cambiamos el ID del Paquete por el ID del Regalo
	I.gpgi,
	-- multiplicamos la cantidad de Paquetes por la cantidad de cada regalo que lo compone
	P.Quantity * I.gpQty,
	P.Adults,
	P.Minors,
	P.Folios,
	-- obtenemos el precio de Adultos (Basado en el precio actual)
	((P.Quantity * I.gpQty) * P.Adults * G.giPrice1),
	-- obtenemos el precio de Menores (Basado en el precio actual)
	((P.Quantity * I.gpQty) * P.Minors * G.giPrice2),
	P.Comments
from #Packages P
	inner join GiftsPacks I on I.gpPack = P.Gift
	inner join Gifts G on G.giID = I.gpgi

-- eliminamos los regalos de #GiftsReceipts que sean paquetes que se desglosan
delete from #GiftsReceipts where Gift in (select giID from Gifts where giPack = 1 and giUnPack = 1)

-- obtenemos los registros ordenados 
select * from #GiftsReceipts order by ReceiptID

-- 2. Regalos
-- =============================================
select distinct G.giID, G.giN, G.giShortN, G.giWFolio, G.giWPax, G.gigc, G.giO
from #GiftsReceipts R
	inner join Gifts G on G.giID = R.Gift
order by G.giO, G.gigc, G.giShortN

-- 3. Monedas
-- =============================================
select distinct C.cuID, C.cuN
from #GiftsReceipts R
	left join Currencies C on C.cuID = R.Currency
order by C.cuID

-- 4. Formas de pago
-- =============================================
select distinct P.ptID, P.ptN
from #GiftsReceipts R
	left join PaymentTypes P on P.ptID = R.PaymentType
order by P.ptID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS on 
GO

