if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptCxCPayments]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptCxCPayments]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de pagos de CxC
** 
** [lormartinez] 17 Jul 2015 Created
**
*/
create procedure [dbo].[USP_OR_RptCxCPayments] 
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
	R.grAmountToPay as AmountToPayUSD,
	R.grAmountToPay * R.grExchangeRate as AmountToPayMXN,
	R.grAmountPaid as AmountPaidUSD,
	R.grAmountPaid * R.grExchangeRate as AmountPaidMXN,
	R.grBalance as BalanceUSD,
	R.grBalance * R.grExchangeRate as BalanceMXN,
	R.grgu,
	G.guHReservID,
	G.guOutInvitNum,
	C.cxD, 
	IsNull(C.cxReceivedBy,'') as cxReceivedBy, 
	IsNull(rec.peN,'') as ReceivedByName,
	C.cxAmount as AmountUSD,
	C.cxAmountMXN as AmountMXN
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

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

