if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptGiftsReceipts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptGiftsReceipts]
GO

SET QUOTED_IDENTIFIER on 
GO
SET ANSI_NULLS on 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de recibos de regalos:
**		1. Recibos de regalos
**		2. Regalos
**		3. Monedas
**		4. Formas de pago
** 
** [alesanchez] 17/Sep/2013 Created. Basado en el manifiesto de Regalos. Se agrega filtro de regalos, filtro por huesped
** [alesanchez] 30/Sep/2013 Modified. Se agrega filtro de Categoría de Regalos 
** [alesanchez] 30/sep/2013 Modified. Se Elimina la columna "ReceiptNumber" a solicitud de Ana Margarita Aquino
** [alesanchez]	14/Oct/2013 Modified. Agregue las formas de pago
** [lchairez]	26/May/2014 Modified. Agregue el campo fecha de cancelacion
** [wtorres]	21/Ago/2014 Modified. Agregue el parametro de @Status
** [wtorres]	27/Ago/2014 Modified. Agregue el campo de hotel quemado y elimine el campo de categoria de regalo
**
*/
create procedure [dbo].[USP_OR_RptGiftsReceipts]
	@DateFrom as datetime,				-- Fecha desde
	@DateTo as datetime,				-- Fecha hasta
	@SalesRooms as varchar(8000),		-- Claves de salas de ventas
	@Categories varchar(8000) = 'ALL',	-- Claves de categorias de regalos
	@Gifts varchar(8000) = 'ALL',		-- Claves de regalos
	@Status tinyint = 0,				-- Estatus
										--		0. Sin filtro
										--		1. Solo incluye recibos activos
										--		2. Solo incluye recibos cancelados
	@GiftReceiptType tinyint = 0,		-- Tipo de recibo de regalos
										--		0. Sin filtro
										--		1. Solo incluye Recibos de Regalos
										--		2. Solo incluye Cargos a personal (CxC)
	@GuestId int = 0					-- Clave del huesped
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
	G.giWCost as WithCost, 
	G.giPublicPrice as PublicPrice,
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
	-- Permitir que traiga el recibo si no tiene regalos
	left join GiftsReceiptsC D on D.gegr = R.grID
	left join Guests GU on GU.guID = R.grgu
	left join Agencies A on A.agID = GU.guag
	left join Personnel P on P.peID = R.grpe
	left join Personnel H on H.peID = R.grHost
	left join Gifts G on G.giID = D.gegi
	left join ChargeTo C on C.ctId = R.grct 
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
	-- Tipo de recibo de regalos
	and (@GiftReceiptType = 0 
		or (@GiftReceiptType = 1 and C.ctIsCxC = 0)  
		or (@GiftReceiptType = 2 and C.ctIsCxC = 1))  
	-- Clave del huesped
	and (@GuestId = 0 or R.grgu = @GuestId)
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
	P.WithCost, 
	P.PublicPrice,
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

