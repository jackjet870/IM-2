if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetExchangeRatesWithPesosByDate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetExchangeRatesWithPesosByDate]
GO

/*
** Palace Resorts
**
** Grupo de Desarrollo Palace
**
** Obtiene los tipos de cambio dada una fecha
**
** [caduran]	06/Oct/2014 Created
**
*/
create procedure [dbo].[USP_OR_GetExchangeRatesWithPesosByDate]
	@Date datetime
as
set nocount on
	
select exD, excu, exExchRate,
	exExchRate / (select exExchRate from ExchangeRate where exD=@Date and excu='MEX') as RatePesos
from ExchangeRate
where exD = @Date

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

