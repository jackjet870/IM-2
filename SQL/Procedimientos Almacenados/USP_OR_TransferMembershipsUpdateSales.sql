if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferMembershipsUpdateSales]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferMembershipsUpdateSales]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Actualiza las membresias en el proceso de transferencia de membresias
**
** [wtorres] 	29/May/2013 Creado
**
*/
create procedure [dbo].[USP_OR_TransferMembershipsUpdateSales] 
as
set nocount on

update Sales
set
	-- Compania
	saCompany = T.tmCompany,
	-- Enganche pactado
	saDownPayment = T.tmDownPayment,
	saDownPaymentPercentage = T.tmDownPaymentPercentage,
	-- Enganche pagado
	saDownPaymentPaid = T.tmDownPaymentPaid,
	saDownPaymentPaidPercentage = T.tmDownPaymentPaidPercentage
from Sales S
	inner join TransferMemberships T on S.saMembershipNum = T.tmApplication

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

