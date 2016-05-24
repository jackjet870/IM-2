/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de Walkouts por Liner
** 
** [edgrodriguez]		11/May/2016 Created
**
*/
CREATE function [dbo].[UFN_IM_GetLinerWO](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@SalesRooms varchar(max) = 'ALL'	-- Clave de las salas de ventas
)
returns @Table table (
	Liner varchar(10),
	WalkOuts money
)
as
begin

insert @Table

-- Shows (Liner 1)
-- =============================================
select
	G.guLiner1,
	SUM(CONVERT(int,G.guWalkOut))
from Guests G
where
	-- Liner 1
	G.guLiner1 is not null
	-- Fecha de show
	and G.guShowD between @DateFrom and @DateTo
	-- Tour regular
	and (G.guTour = 1
	-- Walk Out
	or G.guWalkOut = 1)
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
group by G.guLiner1
UNION ALL
-- Shows (Liner 1)
-- =============================================
select
	G.guLiner2,
	SUM(CONVERT(int,G.guWalkOut))
from Guests G
where
	-- Liner 2
	G.guLiner2 is not null
	-- Fecha de show
	and G.guShowD between @DateFrom and @DateTo
	-- Tour regular
	and (G.guTour = 1
	-- Walk Out
	or G.guWalkOut = 1)
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
group by G.guLiner2
return
end