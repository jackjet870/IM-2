if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].USP_OR_RptDepositsPaid') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].USP_OR_RptDepositsPaid
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
**		1. Recibos de regalos
**		2. Monedas
**		3. Tipos de pago
** 
** [alesanchez]	11/Oct/2013 Ahora se pasa la lista de Lead Sources como un solo parametro
**							Agregue el parametro de salas de ventas
**							Ahora se devuelve la descripcion de las monedas
**							Ahora se devuleve la lista de tipos de pago
**							Renombrado de sprptDepositsPaid a USP_OR_RptDepositsPaid
**
*/
create procedure [dbo].[USP_OR_RptDepositsPaid]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de las salas de ventas
	@SalesRooms varchar(8000) = 'ALL'	-- Claves de las salas de ventas
as
set nocount on

-- Recibos de regalos
-- =============================================
select
	R.grID,
	R.grNum,
	R.grD,
	R.grgu, 
	G.guBookD, 
	R.grGuest,
	R.grHotel, 
	R.grls,
	R.grsr, 
	R.grpe, 
	P.peN,	
	R.grcu,
	R.grpt, 
	R.grHost,
	R.grDeposit,
	R.grDepositTwisted
into #GiftsReceipts
from GiftsReceipts R
	inner join Guests G on G.guID = R.grgu
	left join Personnel P on P.peID = R.grpe
where
	-- Lead Sources
	(@LeadSources = 'ALL' or R.grls in (select item from split(@LeadSources, ',')))
	-- Salas de ventas
	and (@SalesRooms = 'ALL' or R.grsr in (select item from split(@SalesRooms, ',')))
	-- Fecha del recibo
	and R.grD between @DateFrom and @DateTo
	-- Con deposito 
	and (R.grDeposit > 0 or R.grDepositTwisted > 0)
	-- No cancelados
	and R.grCancel = 0
order by R.grID

-- 1. Recibos de regalos
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

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

