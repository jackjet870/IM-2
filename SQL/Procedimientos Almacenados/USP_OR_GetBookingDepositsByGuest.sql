USE [OrigosVCPalace];
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
/*
** Palace Resorts
** Grupo de Desarrollo Palace
** 7714491
** Obtiene los datos para la pantalla de deposits refund
** 
** [Omar Garcia]		19/NOV/2015  Created
** [LorMartinez]    22/Nov/2015  Modified Se repara select y se agregan joins de validacion
*/
CREATE PROCEDURE dbo.USP_OR_GetBookingDepositsByGuest(
 @GuestID integer,
 @RefundID integer = 0
)
AS
BEGIN
  set nocount on

  select  dr.drID , 
         dr.drFolio,
         cast(isnull(bd.bdRefund,0) as bit) [bdRefund],
         s.srN,
         dr.drD,          
         bd.bdAmount, 
         l.lsN,
         g.guID,
         dbo.UFN_OR_GetFullName(g.guLastName1, g.guFirstName1) GuestName,
         g.guHReservID,
         g.guOutInvitNum, 
         p.peID, 
         p.peN,
         bd.bdID
  from dbo.Guests g
  inner join dbo.BookingDeposits bd on bd.bdgu= g.guID
  left join dbo.SalesRooms s on s.srID= g.gusr
  left join dbo.DepositsRefund dr on dr.drgu = g.guID AND dr.drID = bd.bddr
  left join dbo.Personnel p ON p.peID = g.guPRInvit1
  left join dbo.LeadSources l on l.lsID = g.gulsOriginal
  where g.guID = @guestID
  AND (@RefundID= 0 OR (@RefundID > 0 and dr.drID= @RefundID))

END
GO