if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptGiftsReceiptsPayments]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptGiftsReceiptsPayments]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de Gift Receipts Payments
**		1. Pagos
**		2. Origenes de pago
**		3. Monedas
**		4. Formas de pago
** 
** [caduran]	10/Oct/2014 Creado. Basado en USP_OR_RptGiftsSale
** [caduran]    16/Oct/2014 Modificado. Nuevo formato de reporte, se agrega una columna que servirá
							para agrupar los sources, por el momento será asignado por código, posteriormente
							puede crearse un catalogo que se encargue de definir esa agrupación.
**
*/
create procedure [dbo].[USP_OR_RptGiftsReceiptsPayments]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@SalesRooms varchar(8000) = 'ALL'	-- Claves de salas
as
set nocount on

-- Pagos (Tabla temporal)
-- =============================================
SELECT     
	P.gysb as [Source],
	P.gycu as [Currency],
	P.gypt as PaymentType,
	P.gyAmount * (case when R.grCancelD between @DateFrom and @DateTo then 
		case when R.grD between @DateFrom and @DateTo then 0 else -1 end else 1 end) as Amount,
	P.gyAmount * IsNull(E.exExchRate, 1) * 
		(case when R.grCancelD between @DateFrom and @DateTo then 
		case when R.grD between @DateFrom and @DateTo then 0 else -1 end else 1 end) as AmountUS
	
into #GiftsReceiptsPayments	
FROM dbo.GiftsReceiptsPayments AS P 
	INNER JOIN dbo.GiftsReceipts AS R ON P.gygr = R.grID 
	left join ExchangeRate E on E.exD = R.grD and E.excu = P.gycu
WHERE 	-- Fecha del recibo de regalos
	(R.grD between @DateFrom and @DateTo
	-- Fecha de cancelacion del recibo de regalos
	or R.grCancelD between @DateFrom and @DateTo)
	-- Salas de ventas
	and (@SalesRooms = 'ALL' or R.grsr in (select item from split(@SalesRooms, ',')))

-- 1. Pagos
-- =============================================
select [Source], [Currency], PaymentType, SUM(Amount) as Amount, SUM(AmountUS) as AmountUS 
from #GiftsReceiptsPayments
group by [Source], [Currency], PaymentType

-- 2. Origenes de pago
-- =============================================
select distinct S.sbID as SourceID, S.sbN as [Source], Cast(0 as money) as Amount, 
	Cast(0 as money) as AmountUS, Cast('' as varchar(20)) as GroupSource,
	Cast('' as varchar(20)) as GroupSource2, 0 as OrderSource
from #GiftsReceiptsPayments P
	inner join SourcePayments S on S.sbID = P.[Source]
order by S.sbN

-- 3. Monedas
-- =============================================
select distinct C.cuID, C.cuN
from #GiftsReceiptsPayments P
	inner join Currencies C on C.cuID = P.Currency
order by C.cuN

-- 4. Formas de pago
-- =============================================
select distinct PT.ptID, PT.ptN
from #GiftsReceiptsPayments P
	inner join PaymentTypes PT on PT.ptID = P.PaymentType
order by PT.ptN


GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO