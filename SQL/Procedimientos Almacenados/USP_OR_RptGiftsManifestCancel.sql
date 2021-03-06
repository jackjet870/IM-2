if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].USP_OR_RptGiftsManifestCancel') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].USP_OR_RptGiftsManifestCancel
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de depositos pagados
**		1. Cierre de sala de ventas
**		2. Recibos de regalos
**		3. Regalos
**		4. Monedas
**		5. Formas de pago
** 
** [wtorres]	10/Dic/2013 Ahora se pasa la lista de salas de ventas como un solo parametro
**							Elimine la consulta de numero de Taxis de salida y depositos
**							Ahora se devuelve la descripcion de las monedas
**							Ahora se devuelve la lista de tipos de pago
**							Agregue el campo de fecha del recibo y elimine el campo Guest 2
**							Renombrado de sprptGiftsManifCanc a USP_OR_RptGiftsManifestCancel
**
*/
create procedure [dbo].[USP_OR_RptGiftsManifestCancel]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@SalesRooms varchar(8000) = 'ALL'	-- Claves de las salas de ventas
as
set nocount on

-- Recibos de regalos
-- =============================================
select
	R.grgu,
	R.grGuest,
	R.grsr,
	R.grlo,
	R.grHost,
	P.peN,
	R.grID,
	R.grD,
	R.grDeposit,
	R.grDepositTwisted,
	R.grcu,
	R.grpt,
	R.grTaxiOut,
	D.gegi,
	D.geQty,
	D.geFolios,
	IsNull(D.gePriceA, 0) as gePriceA,
	IsNull(D.gePriceM, 0) as gePriceM,
	R.grComments
into #GiftsReceipts
from GiftsReceipts R
	-- Permitir que traiga el recibo si no tiene regalos
	left join GiftsReceiptsC D on D.gegr = R.grID
	left join Personnel P on P.peID = R.grHost
where
	-- Salas de ventas
	(@SalesRooms = 'ALL' or R.grsr in (select item from split(@SalesRooms, ',')))
	-- Fecha del recibo
	and R.grD between @DateFrom and @DateTo
	-- No cancelados
	and R.grCancel = 1
order by R.grlo, R.grID, R.grgu

-- 1. Cierre de sala de ventas
-- =============================================
select Cast(case when srGiftsRcptCloseD >= @DateTo then 1 else 0 end as bit) as GiftsClosed
from SalesRooms 
where srID in (select item from split(@SalesRooms, ','))

-- 2. Recibos de regalos
-- =============================================
select * from #GiftsReceipts

-- 3. Regalos
-- =============================================
select distinct
	G.giID,
	G.giN,
	G.giWFolio,
	G.gigc
from #GiftsReceipts R
	left join Gifts G on G.giID = R.gegi
where R.gegi is not null
order by G.gigc, G.giN

-- 4. Monedas
-- =============================================
select distinct C.cuID, C.cuN
from #GiftsReceipts R
	left join Currencies C on C.cuID = R.grcu
where R.grDeposit > 0 or R.grDepositTwisted > 0

-- 5. Formas de Pago
-- =============================================
select distinct P.ptID, P.ptN
from #GiftsReceipts R
	left join PaymentTypes P on P.ptID = R.grpt
where R.grDeposit > 0 or R.grDepositTwisted > 0

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
