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
** Obtiene los datos para la pantalla de deposits refund
** 
** [LorMartinez] 21/Dic/2015, Created
*/
CREATE procedure [dbo].[USP_OR_GetDepositsRefund] (	  
  @Guest AS INTEGER =0 ,					-- Clave del huesped
  @RefundID as integer = 0, 			-- Refund Id
  @Folio varchar(10) ='' ,	  		-- Folio refund
	@Name varchar(20) ='',		    	-- Nombre   
  @Reservation varchar(15)='' ,	-- Folio de reservacion
 	@OutInv varchar (15) ='',       -- out invitation
  @PR varchar(10)='' ,	        	-- Clave PR
	@DateFrom datetime = null ,	-- Fecha desde
	@DateTo datetime  = null 		-- Fecha hasta
	)
as
  set nocount on 
   
  --select @DateFrom = null, @DateTo=null 
  
  select @DateFrom = dateadd(dd,0,datediff(dd,0,@DateFrom)),
         @DateTo = dateadd(dd,0,datediff(dd,0,@DateTo+1))
    
  select dr.drID , 
         dr.drFolio,
         g.guID
  from dbo.Guests g
  inner join dbo.DepositsRefund dr on dr.drgu = g.guID
  inner join dbo.SalesRooms s on s.srID= g.gusr
  inner join dbo.BookingDeposits bd on bd.bdgu= g.guID
  inner join dbo.LeadSources l on l.lsID = g.gulsOriginal  
  left join dbo.Personnel p ON p.peID = g.guPRInvit1
  where (@Guest = 0 OR (@Guest > 0 AND g.guID = @Guest))   
  and ((@DateFrom is null and @DateTo is null) OR (@DateFrom IS NOT NULL AND @DateTo IS NOT NULL AND dr.drD between @DateFrom and @DateTo))
  and (@RefundID = 0 OR (@RefundID > 0 AND dr.drID = @RefundID) )
  and (@Folio='' OR (@Folio <> '' AND dr.drfolio = @Folio))
  and (@Name ='' OR (@Name <> '' AND  (g.guFirstName1 like '%'+ @Name + '%'  OR g.guFirstName2 like '%'+ @Name + '%' OR g.guLastName1 like '%'+ @Name + '%' OR g.guLastname2 like '%'+ @Name + '%' )) )
  and (@Reservation = '' OR (@Reservation <> '' AND g.guHReservID = @Reservation))
  and (@OutInv = '' OR (@OutInv <> '' AND g.guOutInvitNum=@OutInv ))
  AND (@PR ='' OR (@PR <> '' AND p.peID =@PR)  )  
  group by dr.drID, dr.drFolio, g.guID
  
  
  /*
  select dr.drID , 
         dr.drFolio, 
         s.srN,
         dr.drD, 
         bd.bdAmount, 
         l.lsN,
         g.guID,
         dbo.UFN_OR_GetFullName(g.guLastName1, g.guFirstName1) GuestName,
         g.guHReservID,
         g.guOutInvitNum, 
         p.peID, 
         p.peN
  INTO #tmpDeposit       
  from dbo.Guests g
  join dbo.SalesRooms s on s.srID= g.gusr
  join dbo.BookingDeposits bd on bd.bdgu= g.guID
  join dbo.LeadSources l on l.lsID = g.gulsOriginal  
  left join dbo.Personnel p ON p.peID = g.guPRInvit1
  left join dbo.DepositsRefund dr on dr.drgu = g.guID
  where (@Guest = 0 OR (@Guest > 0 AND g.guID = @Guest))   
  and ((@DateFrom is null and @DateTo is null) OR (@DateFrom IS NOT NULL AND @DateTo IS NOT NULL AND dr.drD between @DateFrom and @DateTo))
  and (@RefundID = 0 OR (@RefundID > 0 AND dr.drID = @RefundID) )
  and (@Folio='' OR (@Folio <> '' AND dr.drfolio = @Folio))
  and (@Name ='' OR (@Name <> '' AND  (g.guFirstName1 like '%'+ @Name + '%'  OR g.guFirstName2 like '%'+ @Name + '%' OR g.guLastName1 like '%'+ @Name + '%' OR g.guLastname2 like '%'+ @Name + '%' )) )
  and (@Reservation = '' OR (@Reservation <> '' AND g.guHReservID = @Reservation))
  and (@OutInv = '' OR (@OutInv <> '' AND g.guOutInvitNum=@OutInv ))
  AND (@PR ='' OR (@PR <> '' AND p.peID =@PR)  )  
  --group by dr.drID, dr.drFolio
    
  --Lista de Deposit Refunds
  SELECT d.drID,
         d.drFolio,
         d.guid
  FROM #tmpDeposit d
  where d.drID is not null
  group by d.drID,d.drFolio, d.guid
  
  
  --Lista de Deposit Payments
  SELECT d.drID, 
         d.drFolio, 
         d.srN,
         d.drD, 
         d.bdAmount, 
         d.lsN,
         d.guID,
         d.GuestName,
         d.guHReservID,
         d.guOutInvitNum, 
         d.peID, 
         d.peN,
         case when isnull(d.drID,0) > 0 then 1 else 0 end [Refunded]
  FROM #tmpDeposit d
  */
  /*  
  i.	Refund ID. Clave del reembolso
  ii.	Refund Folio. Folio del reembolso por sala de ventas
  iii.	Sales Room. Sala de ventas de la invitación
  iv.	Date. Fecha del reembolso
  v.	Amount. Monto del reembolso
  vi.	Lead Source. Lead Source de la invitación
  vii.	Guest ID. Clave del invitado
  viii.	Guest Name. Nombre del invitado
  ix.	Reservation. Folio de reservación Inhouse (Opera)
  x.	Out Invit. Folio de invitación Outhouse
  xi.	PR. Clave del promotor
  xii.	PR Name. Nombre del promotor
  */
GO