if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetTotalReceivedRound]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetTotalReceivedRound]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Autor:		Gabriel Maya Sanchez
-- Función:		Obtiene Total Received Round
-- Fecha:		22/Jul/2014
-- Descripción:	Obtiene Total Received Round
-- =============================================
CREATE FUNCTION [dbo].[UFN_OR_GetTotalReceivedRound](
	@Received money,    -- Received
	@ComBanPor money	-- Porcentaje de comisión Bancaria
	)		
RETURNS money
AS
BEGIN

declare @ComBan money
declare @TotalReceived money
declare @TotalReceivedR money

	set @ComBan = (@Received * @ComBanPor)/ 100
	set @TotalReceived = @Received - @ComBan
	set @TotalReceivedR = round(@TotalReceived, 0)
	
RETURN @TotalReceivedR
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

