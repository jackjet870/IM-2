if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetCxCPayments]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetCxCPayments]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene los pagos de CxC de un recibo de regalos
**
** [lormartinez]	15/Jul/2015	Created
**
*/
create procedure [dbo].[USP_OR_GetCxCPayments]
	@GiftReceiptID AS varchar(10)
as
set nocount on

select cxReceivedBy, cxAmount, cxExchangeRate, cxAmountMXN, cxSeq, cxD
from CxCPayments
where cxgr = @GiftReceiptID
order by cxSeq

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

