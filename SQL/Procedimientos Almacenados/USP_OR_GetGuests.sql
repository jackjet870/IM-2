if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetGuests]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetGuests]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta los huespedes de un Lead Source
** 
** [wtorres]	29/Ene/2014 Creado
**
*/
create procedure  [dbo].[USP_OR_GetGuests]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSource varchar(10),			-- Clave de Lead Source
	@Name varchar(65) = 'ALL',			-- Nombre o apellido
	@RoomNumber varchar(15) = 'ALL',	-- Numero de habitacion
	@Reservation varchar(15) = 'ALL',			-- Folio de reservacion
	@GuestID int = 0					-- Clave de huesped
as
set nocount on

declare @Criteria int

select @Criteria = case
	when @Reservation <> 'ALL' then 1
	when @GuestID <> 0 then 2
	else 3 end

select
	G.guStatus,
	G.guCheckIn,
	G.guID,
	G.guHReservID,
	G.guIdProfileOpera,
	G.guRoomNum,
	G.guLastName1,
	G.guFirstName1,
	G.guCheckInD,
	G.guCheckOutD,
	G.guco,
	C.coN,
	G.guag,
	A.agN,
	A.agcl,
	G.guPax,
	G.guO1,
	G.guCCType,
	G.guPRAssign,
	G.guAvail,
	G.guPRAvail,
	G.guum,
	G.guInfo,
	G.guInfoD,
	G.guPRInfo,
	G.guFollow,
	G.guFollowD,
	G.guPRFollow,
	G.guInvit,
	G.guBookD,
	G.guPRInvit1,
	G.gunb,
	G.guNoBookD,
	G.guPRNoBook,
	G.guQuinella,
	GA.gagu,
	G.guGroup,
	'' as Equity,
	G.gucl,
	CL.clN,
	G.guCompany,
	G.guMembershipNum,
	dbo.UFN_OR_GetLastMembershipType(G.guMembershipNum) as mtN,
	G.guBookCanc,
	G.guShow,
	G.guSale,
	G.guLW,
	G.guPRNote,
	'' as Notes,
	G.guComments
from Guests G
	left join Agencies A on A.agID = G.guag
	left join Countries C on C.coID = G.guco
	left join Clubs CL on CL.clID = G.gucl
	left join GuestsAdditional GA on GA.gaAdditional = G.guID
where
	-- Lead Source
	G.guls = @LeadSource
	-- Folio de reservacion
	and ((@Criteria = 1 and G.guHReservID = @Reservation)
	-- Clave de huesped
	or (@Criteria = 2 and G.guID = @GuestID)
	-- Fecha de llegada
	or (@Criteria = 3 and G.guCheckInD between @DateFrom and @DateTo
	-- Nombre o apellido
	and (@Name = 'ALL' or G.guFirstName1 like '%' + @Name + '%' or G.guLastName1 like '%' + @Name + '%')
	-- Numero de habitacion
	and (@RoomNumber = 'ALL' or G.guRoomNum like '%' + @RoomNumber + '%')))
order by G.guCheckInD desc, G.guLastName1

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

