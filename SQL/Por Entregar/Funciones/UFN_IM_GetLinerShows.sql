USE [OrigosVCPalace]
GO
/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetLinerShows]    Script Date: 05/12/2016 11:13:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por Liner
** 
** [edgrodriguez]		11/May/2016 Created
**
*/
ALTER function [dbo].[UFN_IM_GetLinerShows](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@SalesRooms varchar(max) = 'ALL'	-- Clave de las salas de ventas
)
returns @Table table (
	Liner varchar(10),
	Shows money
)
as
begin

insert @Table

-- Shows (Liner 1)
-- =============================================
select
	G.guLiner1,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guLiner1, G.guLiner2,default,default,default))
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
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guLiner1, G.guLiner2,default,default,default))
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
