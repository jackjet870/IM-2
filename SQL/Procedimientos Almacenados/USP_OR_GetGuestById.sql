if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetGuestById]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetGuestById]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta los datos de un huesped dada su clave
** 
** [wtorres]	09/Sep/2014 Created
** [wtorres]	12/Sep/2014 Modified. Agregue los campos de cantidad de monederos electronicos y propiedad de Opera asociada a la sala de
**							ventas
** [wtorres]	19/Nov/2014 Modified. Agregue los campos que indican si el huesped pertenece a un Lead Source o sala de ventas que usa Sistur
** [wtorres]	26/Ago/2015 Modified. Agregue el campo de la propiedad de Opera asociada al Lead Source
**
*/
create procedure [dbo].[USP_OR_GetGuestById]
	@Guest int	-- Clave del huesped
as
set nocount on

select
	G.guID,
	G.guLastName1,
	G.guFirstName1,
	dbo.UFN_OR_GetFullName(G.guLastName1, G.guFirstName1) as Name,
	-- Lead Source
	G.gulsOriginal,
	G.guls,
	L.lsPropertyOpera,
	L.lsUseSistur,
	-- Locacion
	G.guloInvit,
	-- Programa
	L.lspg,
	P.pgN,
	-- Sala de ventas
	G.gusr,
	S.srPropertyOpera,
	S.srUseSistur,
	-- Reservacion
	G.guHReservID,
	G.guRoomNum,
	G.guCheckInD,
	G.guCheckOutD,
	-- Agencia
	G.guag,
	A.agN,
	-- Monedero electronico
	G.guAccountGiftsCard,
	G.guQtyGiftsCard,
	-- PR's
	G.guSelfGen,
	G.guPRInvit1,
	G.guPRInvit2,
	G.guPRInvit3,
	-- Liners
	G.guLiner1Type,
	G.guLiner1,
	G.guLiner2,
	-- Closers
	G.guCloser1,
	G.guCloser2,
	G.guCloser3,
	-- Exit Closers
	G.guExit1,
	G.guExit2,
	-- Podium y Verificador legal
	G.guPodium,
	G.guVLO
from Guests G
	left join LeadSources L on L.lsID = G.gulsOriginal
	left join Programs P on P.pgID = L.lspg
	left join SalesRooms S on S.srID = G.gusr
	left join Agencies A on A.agID = G.guag
where G.guID = @Guest
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

