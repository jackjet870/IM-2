USE [OrigosVCPalace]
GO
/****** Object:  StoredProcedure [dbo].[USP_OR_RptCxCPayments]    Script Date: 04/16/2016 10:35:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de pagos de CxC
** 
** [lormartinez] 17 Jul 2015 Created
** [edgrodriguez] 16 Abr 2016 Modified. Se validan los campos numericos para devolver 0 en vez de NULL
**
*/
ALTER procedure [dbo].[USP_OR_RptCxCPayments] 
	@DateFrom datetime,		-- Fecha desde  
	@DateTo datetime,		-- Fecha hasta  
	@SalesRoom varchar(10)	-- Clave de sala de ventas  
as
set nocount on

select
	R.grID,
	R.grD,
	R.grls,
	R.grpe,	 
	P.peN,
	ISNULL(R.grAmountToPay,0) as AmountToPayUSD,
	ISNULL(R.grAmountToPay * R.grExchangeRate,0) as AmountToPayMXN,
	ISNULL(R.grAmountPaid,0) as AmountPaidUSD,
	ISNULL(R.grAmountPaid * R.grExchangeRate,0) as AmountPaidMXN,
	ISNULL(R.grBalance,0) as BalanceUSD,
	ISNULL(R.grBalance * R.grExchangeRate,0) as BalanceMXN,
	R.grgu,
	G.guHReservID,
	G.guOutInvitNum,
	C.cxD, 
	IsNull(C.cxReceivedBy,'') as cxReceivedBy, 
	IsNull(rec.peN,'') as ReceivedByName,
	ISNULL(C.cxAmount,0) as AmountUSD,
	ISNULL(C.cxAmountMXN,0) as AmountMXN
from CxCPayments C 
	inner join GiftsReceipts R on C.cxgr = R.grID
	inner join Guests G on G.guID = R.grgu 
	left join Personnel P on P.peID = R.grpe
	left join Personnel rec on rec.peID = C.cxReceivedBy
WHERE
	-- Fecha de pago
	C.cxD between @DateFrom and @DateTo
	-- Sala de ventas
	and R.grsr = @SalesRoom

