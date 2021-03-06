/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por Liner solo si son Overflow
** 
** [ecanul]		28/06/2016 Created 
**
*/
CREATE function [dbo].[UFN_IM_GetLinerOverflow](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@SalesRooms varchar(max) = 'ALL'	-- Clave de las salas de ventas
)
returns @Table table (
	Liner varchar(10),
	Overflow money
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
LEFT JOIN dbo.Sales S ON S.sagu = G.guID
where
	--Filtros de fechas
	G.guShowD BETWEEN @DateFrom AND @DateTo
	-- SalesRoom
	and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
	-- Que sean Overflow
	AND G.guOverflow = 1 
	--Que sean Liner
	AND (G.guLiner1 IS NOT NULL)
	--Selfen
	AND (G.guSelfGen = 1
	-- Liner 1 configurado como Front To Middle
	OR G.guLiner1 IN (SELECT peLinerID FROM dbo.Personnel WHERE peLinerID IS NOT NULL))
group by G.guLiner1
UNION ALL
-- Shows (Liner 2)
-- =============================================
select
	G.guLiner2,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guLiner1, G.guLiner2,default,default,default))
from dbo.Guests G
LEFT JOIN dbo.Sales S ON S.sagu = G.guID
where
	--Filtros de fechas
	G.guShowD BETWEEN @DateFrom AND @DateTo
	-- SalesRoom	
	and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
	-- Que sean Overflow
	AND G.guOverflow = 1 
	--Que sean Liner
	AND G.guLiner2 IS NOT NULL
	--Selfen
	AND (G.guSelfGen = 1
	-- Liner 2 configurado como Front To Middle
	OR G.guLiner2 IN (SELECT peLinerID FROM dbo.Personnel WHERE peLinerID IS NOT NULL))
group by G.guLiner2
return
end
