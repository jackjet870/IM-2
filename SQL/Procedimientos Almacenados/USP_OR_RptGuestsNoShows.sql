if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptGuestsNoShows]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptGuestsNoShows]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de huespedes invitados que no hicieron show
** 
** [wtorres]	28/Dic/2011 Ahora se pasan las salas de ventas como un solo parametro y agregue el campo descripcion de la agencia
** [wtorres]	28/Ene/2012 Agregue el campo descripcion del pais
**
*/
create procedure [dbo].[USP_OR_RptGuestsNoShows]
	@DateFrom DateTime,			-- Fecha desde
	@DateTo DateTime,			-- Fecha hasta
	@SalesRooms varchar(8000)	-- Claves de las salas de ventas
as
set nocount on

select
	G.guID,
	G.guRoomNum,
	G.guHotel,
	G.guLastName1,
	G.guFirstName1,
	G.guloInvit,
	G.guag,
	A.agN,
	G.guco,
	C.coN,
	G.gupax,
	G.guCheckInD,
	G.guDeposit,
	G.guBookD,
	G.guBookCanc
from Guests G
	left join Agencies A on G.guag = A.agID
	left join Countries C on G.guco = C.coID
where
	-- Fecha de booking
	G.guBookD between @DateFrom and @DateTo
	-- No show
	and G.guShow = 0
	-- Sala de ventas
	and G.gusr in (select item from Split(@SalesRooms, ','))
order by G.guloInvit, G.guBookD, G.guLastName1

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

