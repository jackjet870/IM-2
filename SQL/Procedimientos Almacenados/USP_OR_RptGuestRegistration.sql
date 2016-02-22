if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptGuestRegistration]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptGuestRegistration]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos del reporte de registro de invitados:
**		1. Huesped
**		2. Huespedes
**		3. Depositos
**		4. Regalos
**		5. Vendedores
**		6. Tarjetas de credito
**		7. Comentarios
**
** [wtorres]		20/Dic/2008 Created
** [wtorres]		07/Abr/2009 Modified. Agregue los campos de numero de niños y sus edades
** [wtorres]		09/Abr/2009 Modified. Agregue los campos de pais y los comentarios de los regalos
** [wtorres]		22/Abr/2009 Modified. Cambio de tipo del parametro @GuestID
** [wtorres]		03/Jun/2009 Modified. Agregue los campos de fechas de llegada y salida
** [wtorres]		19/Oct/2009 Modified. Ahora maneja quinielas
** [wtorres]		11/Nov/2009 Modified. Agregue los campos de idioma, quiniela y moneda. No desplegar los estados civiles no especificados
** [wtorres]		23/Nov/2009 Modified. Agregue los campos de numero de habitacion y agencia de los invitados
** [wtorres]		17/Oct/2011 Modified. Aumente el tamaño de la descripcion del regalo a 50 y elimine la variable tipo tabla de comentarios
** [wtorres]		28/Dic/2011 Modified. Agregue el campo descripcion de la agencia
** [wtorres]		28/Ene/2012 Modified. Agregue el campo descripcion del pais
** [wtorres]		13/Mar/2014 Modified. Ahora el ancho de los campos se toman de la base de datos
** [lormartinez]	08/Sep/2014 Modified. Agregue los campos de reimpresion
** [LorMartinez] 28/Dic/2015 Modified. Se agrega campo de Area en el query principal
*/
CREATE procedure [dbo].[USP_OR_RptGuestRegistration]
	@GuestID int	-- Clave del huesped
as
set nocount on

declare
	@Quinella bit,		-- Indica si la invitacion es una quiniela
	@QuinellaSplit bit	-- Indica si la invitacion es una quiniela split

-- determinamos si la invitacion es una quiniela o una quiniela split
select @Quinella = guQuinella, @QuinellaSplit = guQuinellaSplit from Guests where guID = @GuestID

-- =============================================
--					HUESPED
-- =============================================
select
	G.guID,
	G.guState,
	G.guco,
	C.coN,
	G.guHotel,
	G.guRoomNum,
	L.loN,
	G.guag,
	A.agN,
	G.guCheckInD,
	G.guCheckOutD,
	G.guDepositTwisted,
	CU.cuN,
	G.guHotelB,
	G.guOutInvitNum,
	G.guPax,
	G.guChildren,
	G.guChildrenAges,
	G.guShowD,
	G.guTimeInT,
	G.guTaxiIn,
	G.guDirect,
	G.guInOut,
	G.guWalkOut,
	G.guIdentification,
	H.peN as EntryHostN,
	G.gula,
	G.guQuinella,
	G.guReimpresion,
	RM.rmN,
  ISNULL(s.srar,'') [Area]
from Guests G
	left join Locations L on G.guloInvit = L.loID
	left join Personnel H on G.guEntryHost = H.peID
	left join Currencies CU on G.gucu = CU.cuID
	left join Agencies A on G.guag = A.agID
	left join Countries C on G.guco = C.coID
	left join ReimpresionMotives RM on RM.rmID = G.gurm
  left join SalesRooms s ON s.srID = G.gusr
where G.guID = @GuestID

-- =============================================
--					HUESPEDES
-- =============================================
-- Huespedes principales
select
	G.guID,
	G.guLastName1 as LastName,
	G.guFirstName1 as FirstName,
	G.guAge1 as Age,
	case when G.gums1 = 'N' then '' else M.msN end as MaritalStatus,
	G.guOccup1 as Occupation,
	G.guEmail1 as Email,
	G.guRoomNum as RoomNum,
	G.guag as Agency,
	A.agN as AgencyN,
	1 as OrderInvit,
	1 as OrderPerson
into #Guests
from Guests G
	left join MStatus M on G.gums1 = M.msID
	left join Agencies A on G.guag = A.agID
where G.guID = @GuestID

-- Acompañantes principales
union
select G.guID, G.guLastName2, G.guFirstName2, G.guAge2, case when G.gums2 = 'N' then '' else M.msN end, G.guOccup2, G.guEmail2, G.guRoomNum,
	G.guag, A.agN, 1, 2
from Guests G
	left join MStatus M on G.gums2 = M.msID
	left join Agencies A on G.guag = A.agID
where G.guID = @GuestID and (G.guLastName2 <> '' or G.guFirstName2 <> '')

-- si es una quiniela
if @Quinella = 1
	
	-- Huespedes secundarios
	insert into #Guests
	select G.guID, G.guLastName1, G.guFirstName1, G.guAge1, case when G.gums1 = 'N' then '' else M.msN end, G.guOccup1, G.guEmail1, G.guRoomNum,
		G.guag, A.agN, G.guID, 1
	from Guests G
		left join MStatus M on G.gums1 = msID
		left join Agencies A on G.guag = A.agID
	where G.guID in (
		select gaAdditional from GuestsAdditional where gagu = @GuestID)
		
	-- Acompañantes secundarios
	union
	select G.guID, G.guLastName2, G.guFirstName2, G.guAge2, case when G.gums2 = 'N' then '' else M.msN end, G.guOccup2, G.guEmail2, G.guRoomNum,
		G.guag, A.agN, G.guID, 2
	from Guests G
		left join MStatus M on G.gums2 = M.msID
		left join Agencies A on G.guag = A.agID
	where G.guID in (
		select gaAdditional from GuestsAdditional where gagu = @GuestID)
	and (guLastName2 <> '' or guFirstName2 <> '')
	
-- si es una quiniela split
else if @QuinellaSplit = 1
	
	-- Huespedes secundarios
	insert into #Guests
	select G.guID, G.guLastName1, G.guFirstName1, G.guAge1, case when G.gums1 = 'N' then '' else M.msN end, G.guOccup1, G.guEmail1, G.guRoomNum,
		G.guag, A.agN, G.guID, 1
	from Guests G
		left join MStatus M on G.gums1 = M.msID
		left join Agencies A on G.guag = A.agID
	where G.guID in (
		select guID from Guests where guMainInvit = @GuestID)
	
	-- Acompañantes secundarios
	union
	select G.guID, G.guLastName2, G.guFirstName2, G.guAge2, case when G.gums2 = 'N' then '' else M.msN end, G.guOccup2, G.guEmail2, G.guRoomNum,
		G.guag, A.agN, G.guID, 2
	from Guests G
		left join MStatus M on G.gums2 = M.msID
		left join Agencies A on G.guag = A.agID
	where G.guID in (
		select guID from Guests where guMainInvit = @GuestID)
	and (guLastName2 <> '' or guFirstName2 <> '')

select * from #Guests order by OrderInvit, OrderPerson

-- =============================================
--					DEPOSITOS
-- =============================================
select D.bdAmount, D.bdReceived, C.cuN
from BookingDeposits D
	left join Currencies C on D.bdcu = C.cuID
where D.bdgu = @GuestID

-- =============================================
--					REGALOS
-- =============================================
select G.guID, dbo.UFN_OR_GetFullName(G.guLastName1, G.guFirstName1) as FullName, I.igQty, GI.giN, I.igAdults, I.igMinors, I.igComments,
	1 as OrderInvit
into #Gifts
from InvitsGifts I
	left join Guests G on I.iggu = G.guID
	left join Gifts GI on I.iggi = GI.giID
where I.iggu = @GuestID

-- si es una quiniela
if @Quinella = 1
	insert into #Gifts
	select G.guID, dbo.UFN_OR_GetFullName(G.guLastName1, G.guFirstName1), I.igQty, GI.giN, I.igAdults, I.igMinors, I.igComments, G.guID
	from InvitsGifts I
		left join Guests G on I.iggu = G.guID
		left join Gifts GI on I.iggi = GI.giID
	where I.iggu in (
		select gaAdditional from GuestsAdditional where gagu = @GuestID)
		
-- si es una quiniela split
else if @QuinellaSplit = 1
	insert into #Gifts
	select G.guID, dbo.UFN_OR_GetFullName(G.guLastName1, G.guFirstName1), I.igQty, GI.giN, I.igAdults, I.igMinors, I.igComments, G.guID
	from InvitsGifts I
		left join Guests G on I.iggu = G.guID
		left join Gifts GI on I.iggi = GI.giID
	where I.iggu in (
		select guID from Guests where guMainInvit = @GuestID)

select * from #Gifts order by OrderInvit, giN

-- =============================================
--					VENDEDORES
-- =============================================
select * from (
	-- PR 1
	select P.peID as ID, P.peN as Name, 'Guest Service' as Role, 1 as [Order]
	from Guests G
		left join Personnel P on G.guPRInvit1 = P.peID
	where G.guPRInvit1 <> '' and G.guID = @GuestID
	-- PR 2
	union
	select P.peID, P.peN, 'Guest Service 2', 2
	from Guests G
		left join Personnel P on G.guPRInvit2 = P.peID
	where G.guPRInvit2 <> '' and G.guID = @GuestID
	-- PR 3
	union
	select P.peID, P.peN, 'Guest Service 3', 3
	from Guests G
		left join Personnel P on G.guPRInvit3 = P.peID
	where G.guPRInvit3 <> '' and G.guID = @GuestID
	-- Liner 1
	union
	select P.peID, P.peN, 'Representative', 4
	from Guests G
		left join Personnel P on G.guLiner1 = P.peID
	where G.guLiner1 <> '' and G.guID = @GuestID
	-- Liner 2
	union
	select P.peID, P.peN, 'Representative 2', 5
	from Guests G
		left join Personnel P on G.guLiner2 = P.peID
	where G.guLiner2 <> '' and G.guID = @GuestID
	-- Closer 1
	union
	select P.peID, P.peN, 'Finance Manager', 6
	from Guests G
		left join Personnel P on G.guCloser1 = P.peID
	where G.guCloser1 <> '' and G.guID = @GuestID
	-- Closer 2
	union
	select P.peID, P.peN, 'Finance Manager 2', 7
	from Guests G
		left join Personnel P on G.guCloser2 = P.peID
	where G.guCloser2 <> '' and G.guID = @GuestID
	-- Closer 3
	union
	select P.peID, P.peN, 'Finance Manager 3', 8
	from Guests G
		left join Personnel P on G.guCloser3 = P.peID
	where G.guCloser3 <> '' and G.guID = @GuestID
	-- Exit 1
	union
	select P.peID, P.peN, 'Junior Finance Manager', 9
	from Guests G
		left join Personnel P on G.guExit1 = P.peID
	where G.guExit1 <> '' and G.guID = @GuestID
	-- Exit 2
	union
	select P.peID, P.peN, 'Junior Finance Manager 2', 10
	from Guests G
		left join Personnel P on G.guExit2 = P.peID
	where G.guExit2 <> '' and G.guID = @GuestID
) as Salesmen
order by [Order]

-- =============================================
--				TARJETAS DE CREDITO
-- =============================================
select G.gdQuantity, C.ccN
from GuestsCreditCards G
	left join CreditCardTypes C on G.gdcc = C.ccID
where G.gdgu = @GuestID

-- =============================================
--					COMENTARIOS
-- =============================================
-- Comentarios del PR
select
	'PR' as [By], guExtraInfo as Comments, 1 as [Order]
from Guests
where guExtraInfo <> '' and guID = @GuestID
-- Comentarios de la Hostess
union
select 'Hostess', guWComments, 2
from Guests
where guWComments <> '' and guID = @GuestID
-- Comentarios del Exit Host
union
select 'Exit Host', guEComments, 3
from Guests
where guEComments <> '' and guID = @GuestID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS on 
GO

