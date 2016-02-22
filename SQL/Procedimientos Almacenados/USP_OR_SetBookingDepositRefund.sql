USE [OrigosVCPalace];
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Genera el Refund y actualiza los datos de los bookingdeposits
** 
** [LorMartinez] 23/Dic/2015, Created
*/
CREATE procedure [dbo].[USP_OR_SetBookingDepositRefund](
 @GuestID integer,
 @Folio integer,
 @RefundType varchar(2),
 @Bookings varchar(max)
)     
AS 

BEGIN       
 
 SET NOCOUNT ON
 DECLARE @Refund INTEGER
 
 --Creamos el refund
 INSERT INTO dbo.DepositsRefund(drFolio, drD, drgu,drrf)
 SELECT @Folio,GETDATE(),@GuestID,@RefundType
 
 --Guarda el refund creado
 SELECT @Refund = SCOPE_IDENTITY()  
  
 --Aactualiza los BookingDeposits 
 UPDATE dbo.BookingDeposits
 SET bddr = @Refund,
     bdRefund = 1
 WHERE bdgu = @GuestID
 AND bdID IN (SELECT ITEM FROM dbo.Split(@Bookings, ','))
 
 SELECT @Refund [RefunID],@Folio [Folio]
  
END
GO