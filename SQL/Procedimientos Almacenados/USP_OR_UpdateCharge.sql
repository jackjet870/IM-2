if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_UpdateCharge]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_UpdateCharge]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Procedimiento:	Actualiza cargo
-- Descripción:		Actualiza el cargo de un recibo de regalos
-- Histórico:		[wtorres] 17/Jun/2009 Creado
-- =============================================
CREATE PROCEDURE [dbo].[USP_OR_UpdateCharge]
	@GuestID int,		-- Clave de la invitación
	@Charge money,		-- Monto del cargo
	@Adjustment money	-- Monto del ajuste del cargo
AS
	SET NOCOUNT ON;

declare @ReceiptID int

-- Obtiene el primer recibo de regalos
select @ReceiptID = Min(grID) from GiftsReceipts where grgu = @GuestID

-- Actualiza el cargo
update GiftsReceipts
set
	grCxCGifts = @Charge,
	grCxCAdj = @Adjustment
where grID = @ReceiptID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

