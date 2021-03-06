if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptCxCGifts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptCxCGifts]
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
**		1. Cierre de sala de ventas
**		2. Recibos de regalos
**		3. Regalos
** 
** [wtorres]	21/Feb/2014 Ahora se pasa la lista de salas de ventas como un solo parametro
**							Ahora devuelve el nombre del regalo
**
*/
create procedure dbo.USP_OR_RptCxCGifts
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
	R.grpe,
	P.peN,
	R.grlo,
	R.grHost,
	H.peN as HostN,
	R.grgu,
	R.grGuest,
	D.gegi as Gift,
	IsNull(D.geQty, 0) as Quantity,
	IsNull(D.geAdults, 0) as Adults,
	IsNull(D.geMinors, 0) as Minors,
	IsNull(D.geFolios, 0) as Folios,
	IsNull(D.gePriceA + D.gePriceM, 0) as Cost,
	R.grD,
	IsNull(D.gePriceA + D.gePriceM, 0) as CostUS,
	IsNull(E.exExchRate, 1) as exExchRate,
	IsNull(D.gePriceA + D.gePriceM, 0) / IsNull(E.exExchRate, 1) as CostMX,
	R.grMemberNum,
	R.grCxCComments
into #GiftsReceipts
from GiftsReceipts R
	left join GiftsReceiptsC D on D.gegr = R.grID
	left join Gifts G on G.giID = D.gegi
	left join Personnel P on P.peID = R.grpe
	left join Personnel H on H.peID = R.grHost
	left join ExchangeRate E on E.exD = R.grD and E.excu = 'MEX'
where
	-- Salas de ventas
	R.grsr in (select item from split(@SalesRooms, ','))
	-- Fecha de autorizacion de CxC
	and R.grCxCAppD between @DateFrom and @DateTo
	-- No considerar los recibos cancelados
	and R.grCancel = 0
	-- Tengan CxC de regalos
	and R.grCxCGifts + R.grCxCAdj > 0
order by R.grID

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
select distinct G.giID, G.giN, G.giShortN, G.giWFolio, G.giWPax, G.gigc, G.giO
from #GiftsReceipts R
	inner join Gifts G on G.giID = R.Gift
order by G.giO, G.gigc, G.giShortN

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

