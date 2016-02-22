if exists (select * from sys.objects where object_id = object_id(N'[dbo].[USP_OR_GetSalesByMembership]') and type in (N'P', N'PC'))
drop procedure [dbo].[USP_OR_GetSalesByMembership]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
** 
** Obtiene las ventas dada su clave de membresia
**
** [wtorres]	31/01/2014 Created
**
*/
create procedure [dbo].[USP_OR_GetSalesByMembership](
	@saMembershipNum varchar(10)
)
as
SET NOCOUNT ON -- Deshabilitamos el conteo de registros actualizados

select saID, saMembershipNum, sagu, saD, sast,
	saReference, saRefMember, saUpdated, samt, saLastName1,
	saFirstName1, saLastName2, saFirstName2, saOriginalAmount, saNewAmount,
	saGrossAmount, saProc, saProcD, saCancel, saCancelD,
	salo, sals, sasr, saPR1, saSelfGen,
	saPR2, saPR3, saPRCaptain1, saPRCaptain2, saPRCaptain3,
	saLiner1Type, saLiner1, saLiner2, saLinerCaptain1, saCloser1,
	saCloser2, saCloser3, saExit1, saExit2, saCloserCaptain1,
	saPodium, saVLO, saLiner1P, saCloser1P, saCloser2P,
	saCloser3P, saExit1P, saExit2P, saClosingCost, saOverPack,
	saComments, saDeposit, saProcRD, saByPhone, samtGlobal,
	saCompany, saDownPayment, saDownPaymentPercentage, saDownPaymentPaid, saDownPaymentPaidPercentage,
	saOriginalAmountWithVAT, saNewAmountWithVAT, saGrossAmountWithVAT
from Sales
where saMembershipNum = @saMembershipNum
order by saD desc, saID desc

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


