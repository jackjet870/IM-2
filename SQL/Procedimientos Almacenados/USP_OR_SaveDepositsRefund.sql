if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_SaveDepositsRefund]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_SaveDepositsRefund]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Genera una devolucion de depositos y marca los depositos como devueltos
** 
** [LorMartinez]	23/Dic/2015 Created
** [wtorres]		21/Abr/2016 Modified. Documentado
**
*/
CREATE procedure [dbo].[USP_OR_SaveDepositsRefund]
	@GuestID integer,
	@Folio integer,
	@RefundType varchar(2),
	@Deposits varchar(max)
AS 
SET NOCOUNT ON
DECLARE @Refund INTEGER
 
-- creamos la devolucion de depositos
INSERT INTO DepositsRefund(drFolio, drD, drgu, drrf)
SELECT @Folio, GETDATE(), @GuestID, @RefundType
 
-- recuperamos la clave de la devolucion
SELECT @Refund = SCOPE_IDENTITY()  
  
-- marcamos los depositos como devueltos
UPDATE BookingDeposits
SET bddr = @Refund,
    bdRefund = 1
WHERE
	bdgu = @GuestID
	AND bdID IN (SELECT ITEM FROM dbo.Split(@Deposits, ','))
 
SELECT @Refund [RefunID], @Folio [Folio]