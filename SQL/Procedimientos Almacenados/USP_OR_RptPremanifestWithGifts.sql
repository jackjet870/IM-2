if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptPremanifestWithGifts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptPremanifestWithGifts]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de premanifiesto con regalos
** 
** [wtorres]	12/Nov/2009 Ahora devuelve los huespedes adicionales
**							- Tambien devuelve los depositos, depositos quemados, regalos y comentarios concatenados en un solo campo
** [wtorres]	26/May/2010 Ahora el pax es la suma del pax del huesped principal y el pax de sus huespedes adicionales
** [wtorres]	03/Jun/2011 Agregue el parametro @SalesRoom y pase a funciones la obtencion de los huespedes adicionales, depositos, depositos
**							quemados y regalos
** [wtorres]	28/Dic/2011 Agregue el campo descripcion de la agencia
** [wtorres]	28/Ene/2012 Agregue el campo descripcion del pais
** [wtorres]	30/Ene/2014 Agregue los parametros @MultiLeadSource, @Regen y @CurrentDate
**
*/
create procedure  [dbo].[USP_OR_RptPremanifestWithGifts]
	@Date datetime,					-- Fecha
	@PlaceID varchar(10) = 'ALL',	-- Clave de Lead Source o Locacion
	@SalesRoom varchar(10) = 'ALL',	-- Clave de sala de ventas
	@MultiLeadSource bit = 0,		-- Indica si es Multi Lead Source
	@Regen bit = 0,					-- Indica si es una locacion Regen
	@CurrentDate datetime = null	-- Fecha actual
as
set nocount on

-- =============================================
--					Bookings
-- =============================================
select
	S.srN,
	L.loN,
	G.guID,
	G.guls, 
	G.guRoomNum,
	dbo.AddString(dbo.AddString(dbo.UFN_OR_GetFullName(G.guLastName1, G.guFirstName1), dbo.UFN_OR_GetFullName(G.guLastName2, G.guFirstName2), ' / '),
	dbo.UFN_OR_GetGuestGuestsAdditionalAsString(G.guID), Char(13)) as guLastName1,
	G.guco,
	C.coN,
	G.guHotel,
	G.guPax + dbo.UFN_OR_GetGuestGuestsAdditionalPax(G.guID) as guPax,
	G.guInvitD, 
	G.guInvitT,
	G.guReschDT,
	G.guCheckInD,
	G.guCheckOutD,
	G.guag,
	A.agN,
	G.guComments,
	G.guBookT,
	G.guPRInvit1,
	G.guO2,
	G.guCCType,
	case G.guShow when 0 then '' else 'ü' end as guShow,
	case G.guSale when 0 then '' else 'ü' end as guSale,
	G.guBookCanc,
	dbo.AddStringLabel(dbo.AddStringLabel(dbo.AddStringLabel(dbo.AddStringLabel('', dbo.UFN_OR_GetGuestDepositsAsString(G.guID), '', 'Deposits: '), 
		dbo.UFN_OR_FormatDeposit(G.guDepositTwisted, G.gucu), ', ', 'Burned: '),
		dbo.UFN_OR_GetGuestGiftsInvitationAsString(G.guID), '  |||  ', 'Gifts: '),
		G.guComments, '  |||  ', 'Comments: ') as Gifts
from Guests G
	inner join SalesRooms S on G.gusr = S.srID
	inner join Locations L on G.guloInvit = L.loID
	left join Agencies A on G.guag = A.agID
	left join Countries C on G.guco = C.coID
where
	-- Fecha de booking
	G.guBookD = @Date
	-- No Multi Lead Source
	and ((@MultiLeadSource = 0
		-- Lead Source
		and (@PlaceID = 'ALL' or G.guls = @PlaceID)
		-- Sala de ventas
		and (@SalesRoom = 'ALL' or G.gusr = @SalesRoom))
	-- Multi Lead Source
	or (@MultiLeadSource = 1
		-- Animacion (Multi Lead Source)
		and ((@Regen = 0 and G.guls in (select mlls from MultiLS where mllo = @PlaceID))
		-- Regen (Invitaciones de la locacion Regen o Shows sin venta antes de la fecha indicada de Multi Lead Source)
		or (@Regen = 1 and (G.guloInvit = @PlaceID or (G.guls in (select mlls from MultiLS where mllo = @PlaceID) and G.guShowD <= @CurrentDate and G.guSale = 0))))))

-- =============================================
--					Reschedules
-- =============================================
union
select
	S.srN,
	L.loN,
	G.guID,
	G.guls, 
	G.guRoomNum,
	dbo.AddString(dbo.AddString(dbo.UFN_OR_GetFullName(G.guLastName1, G.guFirstName1), dbo.UFN_OR_GetFullName(G.guLastName2, G.guFirstName2), ' / '),
	dbo.UFN_OR_GetGuestGuestsAdditionalAsString(G.guID), Char(13)) as guLastName1,
	G.guco,
	C.coN,
	G.guHotel,
	G.guPax + dbo.UFN_OR_GetGuestGuestsAdditionalPax(G.guID) as guPax,
	G.guInvitD, 
	G.guInvitT,
	G.guReschDT,
	G.guCheckInD,
	G.guCheckOutD,
	G.guag,
	A.agN,
	G.guComments,
	G.guReschT,
	G.guPRInvit1, 
	G.guO2,
	G.guCCType,
	case G.guShow when 0 then '' else 'ü' end,
	case G.guSale when 0 then '' else 'ü' end,
	G.guBookCanc,
	dbo.AddStringLabel(dbo.AddStringLabel(dbo.AddStringLabel(dbo.AddStringLabel('', dbo.UFN_OR_GetGuestDepositsAsString(G.guID), '', 'Deposits: '), 
		dbo.UFN_OR_FormatDeposit(G.guDepositTwisted, G.gucu), ', ', 'Burned: '),
		dbo.UFN_OR_GetGuestGiftsInvitationAsString(G.guID), '  |||  ', 'Gifts: '),
		G.guComments, '  |||  ', 'Comments: ') as Gifts
from Guests G
	inner join SalesRooms S on G.gusr = S.srID
	inner join Locations L on G.guloInvit = L.loID
	left join Agencies A on G.guag = A.agID
	left join Countries C on G.guco = C.coID
where
	-- Fecha de reschedule
	G.guReschD = @Date
	-- No Multi Lead Source
	and ((@MultiLeadSource = 0
		-- Lead Source
		and (@PlaceID = 'ALL' or G.guls = @PlaceID)
		-- Sala de ventas
		and (@SalesRoom = 'ALL' or G.gusr = @SalesRoom))
	-- Multi Lead Source
	or (@MultiLeadSource = 1
		-- Animacion (Multi Lead Source)
		and ((@Regen = 0 and G.guls in (select mlls from MultiLS where mllo = @PlaceID))
		-- Regen (Invitaciones de la locacion Regen o Shows sin venta antes de la fecha indicada de Multi Lead Source)
		or (@Regen = 1 and (G.guloInvit = @PlaceID or (G.guls in (select mlls from MultiLS where mllo = @PlaceID) and G.guShowD <= @CurrentDate and G.guSale = 0))))))
order by srN, loN, guBookT, guLastName1

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

