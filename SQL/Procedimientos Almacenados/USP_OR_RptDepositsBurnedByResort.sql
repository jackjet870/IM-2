if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptDepositsBurnedByResort]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptDepositsBurnedByResort]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de depositos quemados por hotel
**		1. Recibos de regalos de huespedes
**		2. Monedas
**		3. Recibos de regalos de ventas
** 
** [wtorres]	04/Oct/2013 Ahora se pasa la lista de salas como un solo parametro
**							Ahora se devuelve la descripcion de las monedas
**							Renombrado de sprptDepositsBurntByDev a USP_OR_RptDepositsBurnedByResort
**
*/
create procedure [dbo].[USP_OR_RptDepositsBurnedByResort]
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@SalesRooms varchar(8000)	-- Claves de las salas de ventas
as
set nocount on

-- Recibos de regalos de huespedes
-- =============================================
select
	R.grID,
	R.grNum,
	R.grD,
	R.grgu,
	R.grGuest,
	R.grHotel,
	G.guHotelB,
	R.grlo,
	R.grsr,
	R.grpe,
	P.peN,
	R.grcu,
	R.grpt, 
	R.grHost,
	R.grDepositTwisted
into #GiftsReceipts
from Guests G
	inner join GiftsReceipts R on R.grgu = G.guID
	left join Personnel P on P.peID = R.grpe
where 
	-- Salas de ventas
	R.grsr in (select item from split(@SalesRooms, ','))
	-- Fecha del recibo
	and R.grD between @DateFrom and @DateTo
	-- Con deposito quemado u hotel quemado
	and (R.grDepositTwisted > 0 or G.guHotelB is not null)
	-- No cancelados
	and R.grCancel = 0
order by G.guHotelB, R.grID

-- 1. Recibos de regalos de huespedes
-- =============================================
select * from #GiftsReceipts

-- 2. Monedas
-- =============================================
select distinct C.cuID, C.cuN
from #GiftsReceipts R
	left join Currencies C on C.cuID = R.grcu

-- 3. Formas de Pago
-- =============================================
select distinct P.ptID, P.ptN
from #GiftsReceipts R
	left join PaymentTypes P on P.ptID = R.grpt

-- 4. Recibos de regalos de ventas
-- =============================================
select
	R.grID,
	R.grNum,
	R.grD,
	R.grgu,
	R.grGuest,
	R.grHotel,
	G.guHotelB,
	R.grlo,
	R.grsr,
	R.grpe,
	P.peN,
	R.grcu,
	R.grpt, 
	R.grHost,
	R.grDepositTwisted,
	S.saID,
	S.saMembershipNum,
	S.saGrossAmount,
	S.saD,
	S.saProcD
from Sales S
	left join Guests G on G.guID = S.sagu
	inner join GiftsReceipts R on R.grgu = G.guID
	left join Personnel P on P.peID = R.grpe
where
	-- Salas de ventas
	S.sasr in (select item from split(@SalesRooms, ',')) 
	-- Fecha de la venta
	and S.saD between @DateFrom and @DateTo
	-- Con deposito quemado u hotel quemado
	and (R.grDepositTwisted > 0 or G.guHotelB is not null)
order by G.guHotelB, R.grID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

