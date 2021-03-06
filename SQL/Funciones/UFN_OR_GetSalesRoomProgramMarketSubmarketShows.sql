if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetSalesRoomProgramMarketSubmarketShows]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetSalesRoomProgramMarketSubmarketShows]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por sala de ventas, programa, mercado y submercado
** 
** [wtorres]	06/Jul/2011 Creado
**
*/
create function [dbo].[UFN_OR_GetSalesRoomProgramMarketSubmarketShows](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@SalesRooms varchar(8000) = 'ALL',	-- Claves de salas de ventas
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@ConsiderDirectsAntesInOut bit = 0,	-- Indica si se debe considerar directas Antes In & Out
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	SalesRoom varchar(10),
	Program varchar(10),
	Market varchar(10),
	Submarket varchar(10),
	Shows int
)
as
begin

insert @Table
select
	D.SalesRoom,
	D.Program,
	D.Market,
	D.Submarket,
	Sum(D.Shows)
from (
	select
		G.gusr as SalesRoom,
		L.lspg as Program,
		G.gumk as Market,
		case when G.gumk in ('AGENCIES', 'DIRECTS') then (case when DateDiff(Day, G.guCheckInD, G.guCheckOutD) between 3 and 4 then '3-4 Nights' else 'Others' end)
			when G.gumk = 'EXCHANGES' then (case when G.guag like '%4X3%' then '4x3' else 'Others' end)
			else 'Total'
		end as Submarket,
		case when @ConsiderQuinellas = 0 then 1 else G.guShowsQty end as Shows
	from Guests G
		inner join LeadSources L on L.lsID = G.guls
	where
		-- Fecha de show
		((@BasedOnArrival = 0 and G.guShowD between @DateFrom and @DateTo)
		-- Fecha de llegada
		or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
		-- Con show
		and G.guShow = 1))
		-- Salas de ventas
		and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
		-- No Quiniela Split
		and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
		-- No Directas no Antes In & Out
		and (@ConsiderDirectsAntesInOut = 0 or not (G.guDirect = 1 and G.guAntesIO = 0))
) as D
group by D.SalesRoom, D.Program, D.Market, D.Submarket

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

