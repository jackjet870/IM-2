if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_CalculateTotalsGiftsInvitation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_CalculateTotalsGiftsInvitation]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Procedimiento:	Calcular total de regalos de invitación
-- Descripción:		Calcula el total de los regalos de una invitación
-- Histórico:		[wtorres] 17/Jun/2009 Creado
-- =============================================
create procedure [dbo].[USP_OR_CalculateTotalsGiftsInvitation]
	@GuestID int	-- Clave de la invitación
as
set nocount on

select IsNull(Sum(gePriceA + gePriceM), 0) as Total
from GiftsReceiptsC
	inner join GiftsReceipts on gegr = grID
where grgu = @GuestID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

