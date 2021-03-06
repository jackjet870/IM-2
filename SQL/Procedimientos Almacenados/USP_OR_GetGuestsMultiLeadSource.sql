if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetGuestsMultiLeadSource]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetGuestsMultiLeadSource]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta los huespedes de Multi Lead Sources
** 
** [wtorres]	29/Ene/2014 Creado
**
*/
create procedure  [dbo].[USP_OR_GetGuestsMultiLeadSource]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@Location varchar(10),				-- Clave de locacion
	@Name varchar(65) = 'ALL',			-- Nombre o apellido
	@RoomNumber varchar(15) = 'ALL',	-- Numero de habitacion
	@Reservation varchar(15) = 'ALL',	-- Folio de reservacion
	@GuestID int = 0,					-- Clave de huesped
	@Regen bit = 0,						-- Indica si es una locacion Regen
	@CurrentDate datetime = null		-- Fecha actual
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
	G.guIdProfileOpera,
	G.guls,
	G.guRoomNum,
	G.guLastName1,
	G.guFirstName1,
	G.guCheckInD,
	G.guCheckOutD,
	G.guco,
	C.coN,
	G.guag,
	A.agN,
	G.guPax,
	G.guPRAssign,
	G.guAvail,
	G.guPRAvail,
	G.guum,
	G.guInfo,
	G.guInfoD,
	G.guPRInfo,
	G.guInvit,
	G.guBookD,
	G.guPRInvit1,
	G.guQuinella,
	GA.gagu,
	G.guMembershipNum,
	G.guBookCanc,
	G.guShow,
	G.guSale,
	G.guLW,
	G.guComments
from Guests G
	left join Agencies A on A.agID = G.guag
	left join Countries C on C.coID = G.guco
	left join GuestsAdditional GA on GA.gaAdditional = G.guID
where
	-- Animacion (Multi Lead Source)
	((@Regen = 0 and G.guls in (select mlls from MultiLS where mllo = @Location))
	-- Regen (Invitaciones de la locacion Regen o Shows sin venta antes de la fecha actual de Multi Lead Source)
	or (@Regen = 1 and (G.guloInvit = @Location or (G.guls in (select mlls from MultiLS where mllo = @Location) and G.guShowD <= @CurrentDate and G.guSale = 0))))
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

