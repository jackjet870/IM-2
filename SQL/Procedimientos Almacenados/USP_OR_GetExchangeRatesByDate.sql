if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetExchangeRatesByDate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetExchangeRatesByDate]
GO

/*
** Palace Resorts
**
** Grupo de Desarrollo Palace
**
** Obtiene los tipos de cambio dada una fecha
**
** [wtorres]		10/Sep/2014 Created
** [lormartinez]	14/Jul/2015 Modified. Se agrega parametro opcional ExchCode para filtrar por exchange rate
**
*/
create procedure [dbo].[USP_OR_GetExchangeRatesByDate]
	@Date datetime,
	@Currency varchar(5) = ''
as
set nocount on

-- seteamos la hora a ceros
select @Date = DateAdd(dd, 0, DateDiff(dd, 0, @Date))
	
select excu, exExchRate
from ExchangeRate
where exD = @Date
	and (@Currency = '' or (@Currency <> '' and excu = @Currency))

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

