if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetFormatForExchangeRateInGiftSaleReport]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetFormatForExchangeRateInGiftSaleReport]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene el formato para el campo ExchangeRate del reporte Gifts Sales
** 
** [caduran]	09/Oct/2014 Creado
**
*/
create function [dbo].[UFN_OR_GetFormatForExchangeRateInGiftSaleReport](
	@MexicoRate decimal(12,8),		-- Tipo de cambio para peso mexico
	@CanadaRate decimal(12,8))	-- Tipo de cambio para dolar canadiense
returns varchar(200)
as
begin
declare @Resultado varchar(200)
declare @Rate1 varchar(20)
declare @Rate2 varchar(20)

set @Rate1 = CONVERT(varchar,cast ( round(1 / IsNull(@MexicoRate,1),8) as decimal(12,8)))
set @Rate2 = CONVERT(varchar,cast ( round(IsNull(@CanadaRate,1)/IsNull(@MexicoRate,1),8) as decimal(12,8)))
set @Resultado = 'USD: ' + @Rate1 + ' CAD: ' + @Rate2

return @Resultado
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

