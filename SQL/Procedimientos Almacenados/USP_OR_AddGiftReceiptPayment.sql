if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_AddGiftReceiptPayment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_AddGiftReceiptPayment]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Agrega un pago a una CxC de un recibo de regalos
**
** [lormartinez]	15/Jul/2015	Created
**
*/
create procedure [dbo].[USP_OR_AddGiftReceiptPayment]
	@GiftReceiptID integer,
	@ReceivedBy varchar(10),
	@ReceivedDate datetime,
	@USDAmount money,
	@RateAmount money,
	@MXNAmount money
as
set nocount on
 
declare
	@Balance money,
	@Seq integer

-- setea hora a ceros
select @ReceivedDate = DateAdd(dd, 0, DateDiff(dd, 0, @ReceivedDate))

select @Balance = ISNULL(gr.grBalance,0)
from dbo.GiftsReceipts gr
where grid = @GiftReceiptID

-- validamos el monto a pagar
if @USDAmount > @Balance begin
	select -1 as iRes, 'The amount of the payment is greater than the balance.' as sRes
	return
end

-- obtenemos el siguiente consecutivo
select @Seq = IsNull(Max(cxSeq), 0) + 1 from CxCPayments where cxgr = @GiftReceiptID

begin try

	-- iniciamos una transaccion
	begin transaction

	-- agregamos el pago de CxC
	insert into CxCPayments (cxgr, cxReceivedBy, cxAmount, cxExchangeRate, cxAmountMXN, cxD, cxSeq)
	values (@GiftReceiptID, @ReceivedBy, @USDAmount, @RateAmount, @MXNAmount, @ReceivedDate, @Seq)

	-- actualizamos el saldo y el monto pagado 
	update GiftsReceipts
	set grBalance = grAmountToPay - (IsNull(grAmountPaid, 0) + @USDAmount),
		grAmountPaid =  IsNull(grAmountPaid, 0) + @USDAmount
	where grID = @GiftReceiptID

	-- confirmamos la transaccion
	commit transaction

	select 1 as iRes, 'Success' as sRes
	return
end try

begin catch

	-- deshacemos la transaccion
	rollback transaction
	select ERROR_NUMBER() as iRes, ERROR_MESSAGE() as sRes
	return
end catch

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

