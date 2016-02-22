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
** Obtiene el premanifiesto de una sala de ventas
** 
** [wtorres]	   04/Ago/2011 Creado
** [wtorres]	   27/Dic/2011 Agregue el campo descripcion de la agencia
** [wtorres]	   28/Ene/2012 Agregue el campo descripcion del pais
** [wtorres]	   13/Ene/2014 Agregue el campo folio de reservacion
** [wtorres]	   11/Feb/2014 Ahora los reschedules despliegan su horario de reschedule en lugar de su horario de booking
** [LorMartinez] 28/Dic/2015 Se agrega campo de refund cuando hay un refund asociado al guest.
**
*/
CREATE procedure  [dbo].[USP_OR_GetPremanifestHost]
	@Date datetime,			-- Fecha
	@SalesRoom varchar(10)	-- Clave de sala de ventas
as
set nocount on

-- =============================================
--					BOOKINGS
-- =============================================
select
	G.guShowSeq,
	G.guID,
	G.guls,
	G.guloInvit,
	G.guQuinella,
	G.guHReservID,
	G.guOutInvitNum,
	G.guBookT,
	G.guLastName1,
	G.guLastName2,
	G.guFirstName1,
	G.guFirstName2,
	G.guHotel,
	G.guRoomNum,
	G.guCheckInD,
	G.guCheckOutD,
	G.guco,
	C.coN,
	G.guag,
	A.agN,
	G.guDeposit,
	CU.cuN,
	G.guPRInvit1,
	G.guMembershipNum,
	G.guDepositReceived,
  --G.guDepositsRefund,
  CAST(CASE WHEN ISNULL(ref.drID,0) > 0 then 1 ELSE 0 END AS BIT) [refund],
	G.guTaxiIn,
	G.guShow,
	G.guTour,
	G.guInOut,
	G.guWalkOut,
	G.guCTour,
	G.guSaveProgram,
	G.guWithQuinella,
	G.guTimeInT,
	G.guTimeOutT,
	G.guMealTicket,
	G.guSale,
	G.guGiftsReceived,
	G.guWComments,
	G.guPax,
	Cast(0 as bit) as guResch,
	G.guBookCanc
 from Guests G
	inner join Currencies CU on G.gucu = CU.cuID
	left join Agencies A on G.guag = A.agID
	left join Countries C on G.guco = C.coID
  OUTER APPLY(SELECT TOP 1 dr.drID
              FROM dbo.DepositsRefund dr
              WHERE dr.drgu = g.guID
              ) Ref
where
	-- Fecha de booking
	G.guBookD = @Date
	-- Sala de ventas
	and G.gusr = @SalesRoom

-- =============================================
--					RESCHEDULES
-- =============================================
union all
select
	G.guShowSeq,
	G.guID,
	G.guls,
	G.guloInvit,
	G.guQuinella,
	G.guHReservID,
	G.guOutInvitNum,
	G.guReschT,
	G.guLastName1,
	G.guLastName2,
	G.guFirstName1,
	G.guFirstName2,
	G.guHotel,
	G.guRoomNum,
	G.guCheckInD,
	G.guCheckOutD,
	G.guco,
	C.coN,
	G.guag,
	A.agN,
	G.guDeposit,
	CU.cuN,
	G.guPRInvit1,
	G.guMembershipNum,
	G.guDepositReceived,
  CAST(CASE WHEN ISNULL(ref.drID,0) > 0 then 1 ELSE 0 END AS BIT) [refund],  
	G.guTaxiIn,
	G.guShow,
	G.guTour,
	G.guInOut,
	G.guWalkOut,
	G.guCTour,
	G.guSaveProgram,
	G.guWithQuinella,
	G.guTimeInT,
	G.guTimeOutT,
	G.guMealTicket,
	G.guSale,
	G.guGiftsReceived,
	G.guWComments,
	G.guPax,
	Cast(1 as bit) as guResch,
	G.guBookCanc
from Guests G
	inner join Currencies CU on G.gucu = CU.cuID
	left join Agencies A on G.guag = A.agID
	left join Countries C on G.guco = C.coID
  left join DepositsRefund DR on DR.drgu = G.guID
  OUTER APPLY(SELECT TOP 1 dr.drID
              FROM dbo.DepositsRefund dr
              WHERE dr.drgu = g.guID
              ) Ref
where
	-- Fecha de reschedule
	G.guReschD = @Date
	-- Fecha de reschedule no del mismo dia de booking
	and G.guBookD <> G.guReschD
	-- Sala de ventas
	and G.gusr = @SalesRoom

-- =============================================
--			PREMANIFESTADOS EN OTRO DIA
-- =============================================
union all
select
	G.guShowSeq,
	G.guID,
	G.guls,
	G.guloInvit,
	G.guQuinella,
	G.guHReservID,
	G.guOutInvitNum,
	G.guBookT,
	G.guLastName1,
	G.guLastName2,
	G.guFirstName1,
	G.guFirstName2,
	G.guHotel,
	G.guRoomNum,
	G.guCheckInD,
	G.guCheckOutD,
	G.guco,
	C.coN,
	G.guag,
	A.agN,
	G.guDeposit,
	CU.cuN,
	G.guPRInvit1,
	G.guMembershipNum,
	G.guDepositReceived,
  CAST(CASE WHEN ISNULL(ref.drID,0) > 0 then 1 ELSE 0 END AS BIT) [refund],
	G.guTaxiIn,
	G.guShow,
	G.guTour,
	G.guInOut,
	G.guWalkOut,
	G.guCTour,
	G.guSaveProgram,
	G.guWithQuinella,
	G.guTimeInT,
	G.guTimeOutT,
	G.guMealTicket,
	G.guSale,
	G.guGiftsReceived,
	G.guWComments,
	G.guPax,
	G.guResch,
	G.guBookCanc
from Guests G
	inner join Currencies CU on G.gucu = CU.cuID
	left join Agencies A on G.guag = A.agID
	left join Countries C on G.guco = C.coID
  OUTER APPLY(SELECT TOP 1 dr.drID
              FROM dbo.DepositsRefund dr
              WHERE dr.drgu = g.guID
              ) Ref
where
	-- Fecha de show
	G.guShowD = @Date
	-- Fecha de show no del mismo dia de booking
	and G.guShowD <> G.guBookD
	-- No reschedules o fecha de reschedule no del mismo dia del show
	and (G.guResch = 0 or G.guShowD <> G.guReschD)
	-- Sala de ventas
	and G.gusr = @SalesRoom
order by G.guBookT, G.guls
GO