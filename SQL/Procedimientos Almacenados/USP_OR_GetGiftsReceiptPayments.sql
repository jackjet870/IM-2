if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetGiftsReceiptPayments]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetGiftsReceiptPayments]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta los pagos de un recibo de regalos
** 
** [wtorres]		29/Ago/2014 Created
** [lormartinez]	29/Jun/2015 Modified. Agregue el campo de banco
**
*/
create procedure [dbo].[USP_OR_GetGiftsReceiptPayments]
	@Receipt int	-- Clave del recibo de regalos
as
set nocount on

select
	P.gygr,
	P.gyAmount,
	P.gyRefund,
	P.gycu,
	P.gypt,
	P.gybk,
	P.gysb,
	P.gype,
	PE.peN as UserName
from GiftsReceiptsPayments P
	left join Personnel PE on PE.peID = P.gype
where
	-- Recibo de regalos
	P.gygr = @Receipt
order by P.gysb, P.gycu, P.gypt, P.gyAmount

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

