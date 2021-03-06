if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptCxCDeposits]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptCxCDeposits]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de CxC de regalos:
**		1. Recibos de regalos
**		2. Monedas
** 
** [wtorres]	21/Feb/2014 Ahora se pasa la lista de salas de ventas como un solo parametro
**							Ahora devuelve el nombre de la moneda
**
*/
create procedure USP_OR_RptCxCDeposits
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@SalesRooms varchar(8000)	-- Claves de salas de ventas
as 
set nocount on

-- Recibos de regalos
-- =============================================
select 
	R.grID,
	R.grNum,
	R.grD,
	R.grls,
	R.grgu,	
	R.grGuest,
	R.grHotel,
	R.grpe,
	P.peN,
	R.grHost,
	H.peN as HostN,
	R.grCxCPRDeposit,
	R.grcuCxCPRDeposit
into #GiftsReceipts
from GiftsReceipts R
	left join Personnel P on P.peID = R.grpe
	left join Personnel H on H.peID = R.grHost
where 
	-- Salas de ventas
	R.grsr in (select item from split(@SalesRooms, ','))
	-- Fecha de autorizacion de CxC
	and R.grCxCAppD between @DateFrom and @DateTo
	-- No considerar los recibos cancelados
	and R.grCancel = 0
	-- Tengan CxC de deposito
	and R.grCxCPRDeposit > 0
order by R.grID

-- 1. Recibos de regalos
-- =============================================
select * from #GiftsReceipts

-- 2. Monedas
-- =============================================
select distinct C.cuID, C.cuN
from #GiftsReceipts R
	inner join Currencies C on C.cuID = R.grcuCxCPRDeposit
order by C.cuID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

