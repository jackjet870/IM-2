USE [OrigosVCPalace];
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
/*
[LorMartinez] 17/11/2015 Created, Devuelve la informacion para las cartas de devolucion
*/
CREATE PROCEDURE dbo.USP_OR_RptRefundLetter(@RefundID integer)
AS
BEGIN

  SET NOCOUNT ON
  
  --Cabecera del reporte
  SELECT g.guID [GuestID],
         s.srID,
         s.srN [SaleRoom],
         g.gula,
         CASE 
           WHEN ISNULL(g.guFirstName2,'') = '' THEN
             ISNULL(g.guFirstName1,'') + ' ' + isnull(g.guLastName1,'') 
           ELSE
            CASE WHEN g.gula='EN' THEN 
              ISNULL(g.guFirstName1,'') + ' ' + isnull(g.guLastName1,'') + ' and ' +
              ISNULL(g.guFirstName2,'') + ' ' + isnull(g.guLastName2,'') 
             WHEN g.gula='ES' THEN
              ISNULL(g.guFirstName1,'') + ' ' + isnull(g.guLastName1,'') + ' y ' +
              ISNULL(g.guFirstName2,'') + ' ' + isnull(g.guLastName2,'') 
             WHEN g.gula='PO' THEN
              ISNULL(g.guFirstName1,'') + ' ' + isnull(g.guLastName1,'') + ' e ' +
              ISNULL(g.guFirstName2,'') + ' ' + isnull(g.guLastName2,'') 
           ELSE '' END
           END [GuestNames],        
        ISNULL(g.guFirstName1,'') + ' ' + isnull(g.guLastName1,'') [Guest1],
        ISNULL(g.guFirstName2,'') + ' ' + isnull(g.guLastName2,'') [Guest2],
        p.peN [PRName],
        ISNULL(g.guEmail1,'') [Email],
        dr.drID [RefundID],
        dr.drFolio [RefundFolio],
        ISNULL(booking.Tot,0) [TotalAmount],
        dr.drD [RefundDate],
        g.guOutInvitNum [OutInvt]
  FROM dbo.Guests g WITH(NOLOCK) 
  INNER JOIN dbo.SalesRooms s WITH(NOLOCK) ON s.srID= g.gusr  
  INNER JOIN dbo.DepositsRefund dr WITH(NOLOCK) ON  dr.drgu = g.guID  
  LEFT JOIN dbo.Personnel p WITH(NOLOCK) ON p.peID = g.guPRInvit1
  OUTER APPLY (SELECT sum(bd.bdAmount) [Tot]
               FROM dbo.BookingDeposits bd
               WHERE bd.bddr = dr.drID
                ) Booking
  WHERE dr.drID = @RefundID
  

  --SubReporte (Detalle de Bookings)  
  SELECT bd.bdAmount [Amount],
         dbo.NumberToLetters(bd.bdAmount,l.laID,'N',1) [AmountLetter],
         cu.cuN [Currency],
         ISNULL(c.ccN,'') [CCType],
         CASE WHEN isnull(bd.bdCardNum,'') = '' THEN ''
         ELSE bd.bdCardNum END [CardNum],
         CASE WHEN bd.bdExpD is null then ''
              WHEN bd.bdExpD <= '1/1/1900' THEN ''
              ELSE bd.bdExpD END [ExpDate],
         ISNULL(bd.bdAuth,0) [Auth]
  FROM dbo.BookingDeposits bd WITH(NOLOCK)
  INNER JOIN dbo.DepositsRefund dr WITH(NOLOCK) ON dr.drID = bd.bddr
  INNER JOIN dbo.Guests g WITH(NOLOCK) ON g.guID = bd.bdgu
  INNER JOIN dbo.Languages l WITH(NOLOCK) ON l.laID = g.gula
  INNER JOIN dbo.Currencies cu WITH(NOLOCK) ON cu.cuID = bd.bdcu
  LEFT JOIN dbo.CreditCardTypes c ON c.ccID = bd.bdcc  
  WHERE dr.drID=@RefundID
  



END
GO