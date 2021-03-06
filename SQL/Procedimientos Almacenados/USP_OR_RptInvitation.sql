if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptInvitation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptInvitation]
GO

set QUOTED_IDENTIFIER ON 
GO
set ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos del reporte de invitacion:
**		1. Huesped
**		2. Huespedes
**		3. Depositos
**		4. Regalos
**
** [wtorres]	11/Nov/2009 Created
** [wtorres]	17/Oct/2011 Modified. Aumente el tamaño de la descripcion del regalo a 50
** [wtorres]	28/Dic/2011 Modified. Agregue el campo descripcion de la agencia
** [wtorres]	28/Ene/2012 Modified. Agregue el campo descripcion del pais
** [wtorres]	13/Mar/2014 Modified. Ahora el ancho de los campos se toman de la base de datos
** [wtorres]	12/Sep/2014 Modified. Ahora se utiliza la funcion FormatDate
**
*/
create procedure [dbo].[USP_OR_RptInvitation]
	@GuestID int	-- Clave del huesped
as
set nocount on

declare @Quinella bit	-- Indica si la invitacion es una quiniela

-- obtenemos algunos datos del huesped
select
	@Quinella = guQuinella
from Guests
where guID = @GuestID

-- =============================================
--					HUESPED
-- =============================================
select
	G.guMembershipNum,
	I.itRTFHeader,
	dbo.FormatDate(case when guResch = 0 then guBookD else guReschD end, G.gula, 'LongWeekDay') as guBookD,
	case when guResch = 0 then guBookT else guReschT end as guBookT,
	G.guag,
	A.agN,
	G.guco,
	C.coN,
	G.guHotel,
	G.guRoomNum,
	G.guPax,
	L.lsN,
	G.guDepositTwisted,
	CU.cuN,
	P.peN + ' - ' + G.guPRInvit1 as PR,
	G.guExtraInfo,
	G.guResch,
	G.guID,
	G.guInvitT,
	G.guInvitD,
	I.itRTFFooter,
	G.gula,
	G.guQuinella
from Guests G
	left join LeadSources L on G.guls = L.lsID
	left join Currencies CU on G.gucu = CU.cuID
	left join Personnel P on G.guPRInvit1 = P.peID
	left join Agencies A on G.guag = A.agID
	left join Countries C on G.guco = C.coID
	left join InvitsTexts I on G.guls = I.itls and G.gula = I.itla
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
	1 as OrderInvit,
	1 as OrderPerson
into #Guests
from Guests G
	left join MStatus M on G.gums1 = M.msID
where G.guID = @GuestID

-- Acompañantes principales
union
select G.guID, G.guLastName2, G.guFirstName2, G.guAge2, case when G.gums2 = 'N' then '' else M.msN end, G.guOccup2, 1, 2
from Guests G
	left join MStatus M on G.gums2 = M.msID
where G.guID = @GuestID and (G.guLastName2 <> '' or G.guFirstName2 <> '')

-- si es una quiniela
if @Quinella = 1

	-- Huespedes secundarios
	insert into #Guests
	select G.guID, G.guLastName1, G.guFirstName1, G.guAge1, case when G.gums1 = 'N' then '' else M.msN end, G.guOccup1, G.guID, 1
	from Guests G
		left join MStatus M on G.gums1 = M.msID
	where G.guID in (
		select gaAdditional from GuestsAdditional where gagu = @GuestID)
		
	-- Acompañantes secundarios
	union
	select G.guID, G.guLastName2, G.guFirstName2, G.guAge2, case when G.gums2 = 'N' then '' else M.msN end, G.guOccup2, G.guID, 2
	from Guests G
		left join MStatus M on G.gums2 = M.msID
	where G.guID in (
		select gaAdditional from GuestsAdditional where gagu = @GuestID)
	and (G.guLastName2 <> '' or G.guFirstName2 <> '')

select * from #Guests order by OrderInvit, OrderPerson

-- =============================================
--					DEPOSITOS
-- =============================================
select D.bdAmount, C.cuN
from BookingDeposits D
	left join Currencies C on D.bdcu = C.cuID
where D.bdgu = @GuestID

-- =============================================
--					REGALOS
-- =============================================
select G.guID, dbo.UFN_OR_GetFullName(G.guLastName1, G.guFirstName1) as FullName, I.igQty, GI.giN, 1 as OrderInvit
into #Gifts
from InvitsGifts I
	left join Guests G on I.iggu = G.guID
	left join Gifts GI on I.iggi = GI.giID
where I.iggu = @GuestID

-- si es una quiniela
if @Quinella = 1
	insert into #Gifts
	select G.guID, dbo.UFN_OR_GetFullName(G.guLastName1, G.guFirstName1), I.igQty, GI.giN, guID
	from InvitsGifts I
		left join Guests G on I.iggu = G.guID
		left join Gifts GI on I.iggi = GI.giID
	where I.iggu in (
		select gaAdditional from GuestsAdditional where gagu = @GuestID)

select * from #Gifts order by OrderInvit, giN

GO
set QUOTED_IDENTIFIER OFF 
GO
set ANSI_NULLS ON 
GO

