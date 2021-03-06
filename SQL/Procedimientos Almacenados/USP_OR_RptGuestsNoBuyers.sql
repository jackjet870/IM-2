if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptGuestsNoBuyers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptGuestsNoBuyers]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de huespedes que no compraron membresia
** 
** [wtorres]	21/Ago/2010 Creado
** [wtorres]	28/Ene/2012 Agregue el campo descripcion del pais
**
*/
create procedure [dbo].[USP_OR_RptGuestsNoBuyers]
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@SalesRooms varchar(8000)	-- Claves de salas
as
set nocount on

select
	P.pgN as Program,
	L.lsN as LeadSource,
	G.guID as GuestID,
	G.guLastName1 as LastName,
	G.guFirstName1 as FirstName,
	G.guEmail1 as Email,
	G.guEmail2 as Email2,
	G.guCity as City,
	G.guState as State,
	G.guco as Country,
	C.coN as CountryN
from Guests G
	inner join LeadSources L on G.guls = L.lsID
	inner join Programs P on L.lspg = P.pgID
	left join Countries C on G.guco = C.coID
where
	-- Filtro por fecha
	G.guShowD between @DateFrom and @DateTo
	-- Filtro por sala
	and G.gusr in (select item from split(@SalesRooms, ','))
	-- No compraron
	and G.guSale = 0
order by P.pgN, L.lsN, G.guLastName1, G.guFirstname1

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

