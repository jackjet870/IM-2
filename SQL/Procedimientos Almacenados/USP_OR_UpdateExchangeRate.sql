if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_UpdateExchangeRate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_UpdateExchangeRate]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Actualiza el tipo de cambio al mismo de la intranet
**
** [lchairez]	14/Nov/2013 Creado
** [caduran]    06/Oct/2014 Se cambio el tipo de dato del parametro ExchangeRate a decimal
*/
CREATE PROCEDURE [dbo].[USP_OR_UpdateExchangeRate]
	@Date as DATETIME,
	@Currency VARCHAR(10),
	@ExchangeRate AS DECIMAL(12,8)
AS
SET NOCOUNT ON

UPDATE ExchangeRate SET exExchRate = @ExchangeRate
WHERE exD = @Date AND excu = @Currency

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

