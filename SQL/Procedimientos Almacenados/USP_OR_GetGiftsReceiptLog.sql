if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetGiftsReceiptLog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetGiftsReceiptLog]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene el registro historico de un recibos de regalos
** 
** [wtorres]	17/Ago/2011 Created
** [alesanchez]	09/Nov/2013 Modified. Se le agrega el campo de Forma de pago 
** [axperez]	20/Dic/2013 Modified. Se le agrega el campo de Motivos de Reimpresion y Contador de reimpresiones
** [lchairez]	06/Ene/2014 Modified. Se agrega columnas de CxC autorizada por, monto pagado de la CxC y motivo de pago incompleto
** [wtorres]	28/May/2015 Modified. Agregue los nombres del personal
**
*/
create procedure [dbo].[USP_OR_GetGiftsReceiptLog] 
	@Receipt int -- Clave del recibo de regalos
as
set nocount on;

select 
	L.goChangedBy,
	C.peN as ChangedByN,
	L.goID,
	L.goD,
	L.gope,
	O.peN as OfferedByN,
	L.goHost,
	H.peN as HostN,
	L.goDeposit,
	L.goBurned,
	L.gocu,
	P.ptN,
	L.goCXCPRDeposit,
	L.goTaxiOut,
	L.goTotalGifts,
	L.goct,
	L.goCXCGifts,
	L.goCXCAdj,
	L.goReimpresion,
	RM.rmN,
	L.goAuthorizedBy,
	A.peN as AuthorizedByN,
	L.goAmountPaid,
	UM.upN,
	L.goCancelD
from GiftsReceiptsLog L
	left join Personnel C on C.peID = L.goChangedBy
	left join Personnel O on O.peID = L.gope
	left join Personnel H on H.peID = L.goHost
	left join Personnel A on A.peID = L.goAuthorizedBy
	left Join PaymentTypes P on P.ptID = L.gopt
	left join ReimpresionMotives RM on RM.rmID = L.gorm
	left join UnderPaymentMotives UM on UM.upID = L.goup
where L.gogr = @Receipt
order by L.goID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

