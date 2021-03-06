if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetExchangeRateLog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetExchangeRateLog]
GO

/*** Palace Resorts**
** Grupo de Desarrollo Palace**** Consulta el registro historico de los tipos de cambio**
** [lchairez]	19/Nov/2013 Creado***/
CREATE PROCEDURE [dbo].[USP_OR_GetExchangeRateLog]
	@Currency AS VARCHAR(10)
AS
SET NOCOUNT ON
	
SELECT L.elChangedBy, P.peN as ChangedByN, L.elID, L.elcu ,elExchangeRate
FROM ExchangeRateLog L
	LEFT JOIN Personnel P on L.elChangedBy = P.peID
WHERE L.elcu = @Currency
ORDER BY L.elID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

