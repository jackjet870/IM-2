USE [OrigosVCPalace]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetLinerShows]    Script Date: 09/05/2016 12:46:55 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetLinerShows]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetLinerShows]
GO

USE [OrigosVCPalace]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetLinerShows]    Script Date: 09/05/2016 12:46:55 ******/
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
** [edgrodriguez]		20/Jun/2016 Modified. Se agrego el parametro ShowType para filtrar los tipos de show.
** [ecanul]				27/jun/2016 Modified. Se agrego filtro para obtener los shows reales
** [ecanul]				05/Sep/2016 Modified. Eliminado Join con tabla Sales, Agregaba un Show mas al resultado 
**
*/
CREATE function [dbo].[UFN_IM_GetLinerShows](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@SalesRooms varchar(max) = 'ALL',	-- Clave de las salas de ventas
	@ShowType int = 0,					-- Tipo de Show
										-- 0. Sin Filtro
										-- 1. Regular Tour
										-- 2. In&Out
										-- 3. WalkOut
										-- 4. Courtesy Tour
										-- 5. Save Tour
										-- 6. With Quinellas
	@RealShows bit = 0					-- Real Shows
										-- 0 Shows = Total Shows - Walk Outs & CTours
										-- 1 Shows Reales 
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
from dbo.Guests G
where
	-- Liner 1
	G.guLiner1 is not null
	-- Fecha de show
	and G.guShowD between @DateFrom and @DateTo
	-- Filtro tipo de show
	and (@ShowType = 0 or (@ShowType = 1 and G.guTour = 1) or (@ShowType = 2 and G.guInOut = 1) or (@ShowType = 3 and G.guWalkOut = 1) 
	or (@ShowType = 4 and G.guCTour = 1) or (@ShowType = 5 and G.guSaveProgram = 1) or (@ShowType = 6 and G.guWithQuinella = 1))
	-- Real Shows			Shows Reales
	AND (@RealShows = 0 OR (G.guTour = 1 OR G.guWalkOut = 1 OR ((G.guCTour = 1 OR g.guSaveProgram = 1) AND g.guSale = 1)))
	-- Sales Rooms ---- 
	and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
group by G.guLiner1
UNION ALL
-- Shows (Liner 1)
-- =============================================
select
	G.guLiner2,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guLiner1, G.guLiner2,default,default,default))
from dbo.Guests G
where
	-- Liner 2
	G.guLiner2 is not null
	-- Fecha de show
	and G.guShowD between @DateFrom and @DateTo
	-- Filtro tipo de show
	and (@ShowType = 0 or (@ShowType = 1 and G.guTour = 1) or (@ShowType = 2 and G.guInOut = 1) or (@ShowType = 3 and G.guWalkOut = 1) 
	or (@ShowType = 4 and G.guCTour = 1) or (@ShowType = 5 and G.guSaveProgram = 1) or (@ShowType = 6 and G.guWithQuinella = 1))
	-- Real Shows			Shows Reales
	AND (@RealShows = 0 OR (G.guTour = 1 OR G.guWalkOut = 1 OR ((G.guCTour = 1 OR g.guSaveProgram = 1) AND g.guSale = 1)))
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
group by G.guLiner2
return
end

GO


