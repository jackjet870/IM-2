if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetSalesRoomProgramMarketSubmarketSales]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetSalesRoomProgramMarketSubmarketSales]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas por sala de ventas, programa, mercado y submercado
** 
** [wtorres]	06/Jul/2011 Creado
** [wtorres]	19/Nov/2013 Ahora los downgrades se cuentan en base a la categoria de tipo de venta
**
*/
create function [dbo].[UFN_OR_GetSalesRoomProgramMarketSubmarketSales](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@SalesRooms varchar(8000) = 'ALL',	-- Claves de salas de ventas
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	SalesRoom varchar(10),
	Program varchar(10),
	Market varchar(10),
	Submarket varchar(10),
	Sales int
)
as
begin

insert @Table
select
	D.SalesRoom,
	D.Program,
	D.Market,
	D.Submarket,
	Count(*)
from (
	select
		S.sasr as SalesRoom,
		L.lspg as Program,
		G.gumk as Market,
		case when G.gumk in ('AGENCIES', 'DIRECTS') then (case when DateDiff(Day, G.guCheckInD, G.guCheckOutD) between 3 and 4 then '3-4 Nights' else 'Others' end)
			when G.gumk = 'EXCHANGES' then (case when G.guag like '%4X3%' then '4x3' else 'Others' end)
			else 'Total'
		end as Submarket
	from Sales S
		left join SaleTypes ST on ST.stID = S.sast
		inner join LeadSources L on L.lsID = S.sals
		left join Guests G on S.sagu = G.guID
	where
		-- Fecha de procesable
		(((@BasedOnArrival = 0 or (@BasedOnArrival = 1 and S.sagu is null)) and S.saProcD between @DateFrom and @DateTo)
		-- Fecha de llegada
		or (@BasedOnArrival = 1	and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo
		-- Procesable
		and S.saProc = 1))
		-- No downgrades
		and ST.ststc <> 'DG'
		-- Fecha de cancelacion
		and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
		-- Salas de ventas
		and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
) as D
group by D.SalesRoom, D.Program, D.Market, D.Submarket

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

