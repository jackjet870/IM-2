if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptDepositRefund]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptDepositRefund]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene la informacion del reporte de deposit refund
** 
** [LorMartinez] 24/Nov/2015, Created, Obtiene la informacion del reporte de deposit refund
*/
CREATE PROCEDURE dbo.USP_OR_RptDepositRefund(
@DateFrom datetime,					-- Fecha desde
@DateTo datetime,					-- Fecha hasta
@LeadSources varchar(8000) = 'ALL'	-- Claves de Lead Sources
)
AS
BEGIN 

 SET NOCOUNT ON
 
 SELECT 
        s.srN ,
        ls.lsN ,
        dr.drID,
        dr.drFolio,
        dr.drD,
        g.guHReservID,
        g.guOutInvitNum,
        isnull(g.guFirstName1,'') + ' ' + isnull(g.guLastName1,'') [GuestName],
        bdep.Tot [Total],
        p.peID,
        p.peN,
        b.bdAmount,
        c.cuN,
        isnull(ct.ccN,'') [ccN],
        b.bdCardNum,
        b.bdExpD,
        b.bdAuth
 FROM dbo.DepositsRefund dr
 INNER JOIN dbo.BookingDeposits b ON b.bddr = dr.drID
 INNER JOIN dbo.Guests g ON dr.drgu = g.guID 
 INNER JOIN dbo.LeadSources ls ON ls.lsID= g.gulsOriginal
 INNER JOIN dbo.SalesRooms s ON s.srID = g.gusr 
 INNER JOIN dbo.Currencies c on c.cuID = b.bdcu
 LEFT JOIN dbo.CreditCardTypes ct ON ct.ccID = b.bdcc
 LEFT JOIN dbo.personnel p on p.peID = g.guPRInvit1
 OUTER APPLY (SELECT SUM(db.bdAmount) Tot
              FROM dbo.BookingDeposits db 
              WHERE db.bddr = dr.drID
              ) Bdep 
 WHERE  --Fechas de despositos
        Convert(varchar, dr.drD, 112) between @DateFrom and @DateTo
        --LeadSources      
        AND (@LeadSources = 'ALL' or ls.lsID in (select item from Split(@LeadSources, ',')))
END
GO