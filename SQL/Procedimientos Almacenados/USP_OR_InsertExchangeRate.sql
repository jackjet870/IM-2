if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_InsertExchangeRate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_InsertExchangeRate]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Hace una serie de inserciones de los tipos de cambio hasta la fecha actual
** 
** [wtorres]	07/Mar/2009 Creado
** 
*/
CREATE PROCEDURE [dbo].[USP_OR_InsertExchangeRate]
	@CurrentDate DateTime
AS 
SET NOCOUNT ON

DECLARE 
	@Date DATETIME,
	@LastDate DATETIME
	
IF (SELECT count(*) FROM ExchangeRate WHERE exD = @CurrentDate) = 0 
BEGIN
	SET @LastDate = (Select top 1 exD from ExchangeRate order by exD desc) 
	SET @Date = @LastDate + 1
	WHILE @Date <= @CurrentDate
	BEGIN
		INSERT INTO ExchangeRate(exD, excu, exExchRate)
		SELECT  @Date, excu,exExchRate FROM ExchangeRate WHERE exD = @LastDate 	
		SET @Date = @Date + 1
	END
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

