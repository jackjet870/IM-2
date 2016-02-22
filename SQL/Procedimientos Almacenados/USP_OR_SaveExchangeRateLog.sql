if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_SaveExchangeRateLog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_SaveExchangeRateLog]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desrrrollo Palace
**
** Agrega un registro en el historico de tipos de cambio
** 
** [lchairez]	19/Nov/2013 Creado
**
*/
CREATE PROCEDURE [dbo].[USP_OR_SaveExchangeRateLog] 
	@Currency AS VARCHAR(10),
	@Date AS DATETIME,
	@HoursDif AS SMALLINT,
	@ChangedBy AS VARCHAR(10)
AS
SET NOCOUNT ON
		
DECLARE @Count INT

-- determinamos si cambio algun campo relevante
SELECT  @Count = Count(*)
FROM ExchangeRateLog L
	INNER JOIN ExchangeRate E ON E.excu = L.elcu AND E.exD = L.elD
WHERE
	L.elcu = @Currency
	AND L.elD = @Date
	AND (L.elExchangeRate = E.exExchRate OR (L.elExchangeRate IS NULL AND E.exExchRate IS NULL))
	AND L.elID = (SELECT MAX(elID) FROM ExchangeRateLog WHERE elcu = @Currency and elD = @Date)
	
-- agregamos un registro en el historico, si cambio algun campo relevante
INSERT INTO ExchangeRateLog (elID, elcu, elD, elChangedBy, elExchangeRate)
SELECT 
	DATEADD(hh, @HoursDif, GETDATE()),
	excu,
	exD,
	@ChangedBy,
	exExchRate
FROM ExchangeRate
WHERE excu = @Currency AND exD = @Date AND @Count = 0 
    
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

