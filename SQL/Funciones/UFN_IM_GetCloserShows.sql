USE [OrigosVCPalace]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por Closer
** 
** [edgrodriguez]		09/May/2016 Created
**
*/
CREATE function [dbo].[UFN_IM_GetCloserShows](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@SalesRooms varchar(max) = 'ALL'	-- Clave de las salas de ventas
)
returns @Table table (
	Closer varchar(10),
	Shows money
)
as
begin

insert @Table

-- Shows (Closer 1)
-- =============================================
select
	G.guCloser1,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guCloser1, G.guCloser2, G.guCloser3, G.guExit1, G.guExit2))
from Guests G
where
	-- Closer 1
	G.guCloser1 is not null
	and G.guTour = 1
	-- Fecha de show
	and (G.guShowD between @DateFrom and @DateTo)
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
group by G.guCloser1
UNION ALL
-- Shows (Closer 2)
-- =============================================
select
	G.guCloser2,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guCloser1, G.guCloser2, G.guCloser3, G.guExit1, G.guExit2))
from Guests G
where
	-- Closer 1
	G.guCloser2 is not null
	and G.guTour = 1
	-- Fecha de show
	and (G.guShowD between @DateFrom and @DateTo)
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
group by G.guCloser2
UNION ALL
-- Shows (Closer 3)
-- =============================================
select
	G.guCloser3,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guCloser1, G.guCloser2, G.guCloser3, G.guExit1, G.guExit2))
from Guests G
where
	-- Closer 3
	G.guCloser3 is not null
	and G.guTour = 1
	-- Fecha de show
	and (G.guShowD between @DateFrom and @DateTo)
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
group by G.guCloser3
UNION ALL
-- Shows (Exit 1)
-- =============================================
select
	G.guExit1,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guCloser1, G.guCloser2, G.guCloser3, G.guExit1, G.guExit2))
from Guests G
where
	-- Exit 1
	G.guExit1 is not null
	and G.guTour = 1
	-- Fecha de show
	and (G.guShowD between @DateFrom and @DateTo)
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
group by G.guExit1
UNION ALL
-- Shows (Exit 2)
-- =============================================
select
	G.guExit2,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guCloser1, G.guCloser2, G.guCloser3, G.guExit1, G.guExit2))
from Guests G
where
	-- Exit 2
	G.guExit2 is not null
	and G.guTour = 1
	-- Fecha de show
	and (G.guShowD between @DateFrom and @DateTo)
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
group by G.guExit2
return
end
