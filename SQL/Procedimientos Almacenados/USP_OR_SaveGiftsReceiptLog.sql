if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_SaveGiftsReceiptLog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_SaveGiftsReceiptLog]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Agrega un registro en el historico de un recibo de regalos si su informacion relevante cambio
** 
** [wtorres]	01/Jun/2010 Optimizacion
** [alesanchez] 09/Nov/2013 Agregue campo de Forma de pago
** [axperez]	20/Dic/2013 Agregue campo de Motivo de Reimpresion y Contador de Reimpresiones
** [lchairez]	21/Dic/2013 Se omite parametro de @TotalGifts para que se calcule desde aquí
** [LoreMartinez] 13/Jul/2015 Se cambia la columna grAmountPaid por grAmountToPaid
**
*/
create procedure [dbo].[USP_OR_SaveGiftsReceiptLog]
	@Receipt int,			-- Clave del recibo de regalos
	@HoursDif smallint,		-- Horas de diferencia
	@ChangedBy varchar(10)	-- Clave del usuario que esta haciendo el cambio
as
set nocount on

declare
	@Count int,
	@TotalGifts money

-- determinamos si cambio algun campo relevante
select @Count = Count(*)
from GiftsReceiptsLog
	inner join GiftsReceipts on gogr = grID
where
	gogr = @Receipt
	and (goD = grD or (goD is null and grD is null))
	and (goHost = grHost or (goHost is null and grHost is null))
	and (goDeposit = grDeposit or (goDeposit is null and grDeposit is null))
	and (goBurned = grDepositTwisted or (goBurned is null and grDepositTwisted is null))
	and (gocu = grcu or (gocu is null and grcu is null))
	and (goCXCPRDeposit = grcxcPRDeposit or (goCXCPRDeposit is null and grcxcPRDeposit is null))
	and (goTaxiOut = grTaxiOut or (goTaxiOut is null and grTaxiOut is null))
	and (goct = grct or (goct is null and grct is null))
	and (gope = grpe or (gope is null and grpe is null))
	and (goCXCGifts = grcxcGifts or (goCXCGifts is null and grcxcGifts is null))
	and (goCXCAdj = grcxcAdj or (goCXCAdj is null and grcxcAdj is null))
	and (goTaxiOutDiff = grTaxiOutDiff or (goTaxiOutDiff is null and grTaxiOutDiff is null))
	and (gopt = grpt or (gopt is null and grpt is null))
	and (goReimpresion = grReimpresion or (goReimpresion is null and grReimpresion is null))
	and (gorm = grrm or (gorm is null and grrm is null))
	and (goAuthorizedBy = grAuthorizedBy or (goAuthorizedBy is null and grAuthorizedBy is null))
	and (goAmountPaid = grAmountToPay or (goAmountPaid is null and grAmountToPay is null))
	and (goup = grup or (goup is null and grup is null))
	and goID in (select Max(goID) from GiftsReceiptsLog where gogr = @Receipt)
	and (goCancelD = grCancelD or (goCancelD is null and grCancelD is null))

-- obtenemos el costo total de los regalos del recibo
select @TotalGifts = IsNull(Sum(gePriceA + gePriceM), 0) from GiftsReceiptsC where gegr = @Receipt

-- agregamos un registro en el historico, si cambio algun campo relevante
insert into GiftsReceiptsLog
select
	DateAdd(hh, @HoursDif, GetDate()),
	grID,
	grD,
	grHost,
	grDeposit,
	grDepositTwisted,
	grcu,
	grCXCPRDeposit,
	grTaxiOut,
	@TotalGifts,
	grct,
	grpe,
	grCXCGifts,
	grCXCAdj,
	grTaxiOutDiff,
	@ChangedBy,
	grpt,
	grReimpresion,
	grrm,
	grAuthorizedBy,
	grAmountToPay,
	grup,
	grCancelD
from GiftsReceipts
where grID = @Receipt and @Count = 0
