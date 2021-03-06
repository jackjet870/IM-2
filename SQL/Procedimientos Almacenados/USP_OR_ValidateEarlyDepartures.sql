if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_ValidateEarlyDepartures]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_ValidateEarlyDepartures]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Procedimiento:	Valida salidas anticipadas
-- Descripción:		Devuelve la lista de cuentas cuyas invitaciones tengan fecha de salida igual a la
--					fecha proporcionada
-- Histórico:		[wtorres] 29/Jun/2009 Creado
-- =============================================
create procedure [dbo].[USP_OR_ValidateEarlyDepartures]
	@Date datetime	-- Fecha del sistema
as
set nocount on

select guAccountGiftsCard
from Guests
where not guAccountGiftsCard is null
group by guAccountGiftsCard
having Max(guCheckOutHotelD) = @Date

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

