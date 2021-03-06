if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetNationalityMarketSalesAmount]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetNationalityMarketSalesAmount]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de ventas por nacionalidad y mercado
** 
** [wtorres]	11/Oct/2010	Creado
** [wtorres]	18/Nov/2010 Agregue el parametro @BasedOnArrival
** [wtorres]	25/Jul/2013 Agregue el parametro @FilterSaveCourtesyTours
** [wtorres]	19/Nov/2013 Ahora los downgrades se cuentan en base a la categoria de tipo de venta
**
*/
create function [dbo].[UFN_OR_GetNationalityMarketSalesAmount](
	@DateFrom datetime,						-- Fecha desde
	@DateTo datetime,						-- Fecha hasta
	@LeadSources varchar(8000),				-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',				-- Claves de PRs
	@Program varchar(10) = 'ALL',			-- Clave de programa
	@ConsiderSelfGen bit = 0,				-- Indica si se debe considerar Self Gen
	@FilterSaveCourtesyTours tinyint = 0,	-- Filtro de tours de rescate y cortesia
											--		0. Sin filtro
											--		1. Excluir tours de rescate y cortesia
											--		2. Excluir tours de rescate y cortesia sin venta
	@BasedOnArrival bit = 0					-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	Nationality varchar(25),
	Market varchar(10),
	SalesAmount money
)
as
begin

insert @Table
select
	C.coNationality,
	G.gumk,
	Sum(S.saGrossAmount)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
	left join Countries C on G.guco = C.coID
	inner join LeadSources L on L.lsID = S.sals
where
	-- Fecha de procesable
	((((@BasedOnArrival = 0 or (@BasedOnArrival = 1 and S.sagu is null)) and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo
	-- Procesable
	and S.saProc = 1))
	-- No downgrades
	and (ST.ststc <> 'DG'
	-- Downgrades cuya membresia de referencia esta dentro del periodo
	or (ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)))
	-- No canceladas o canceladas fuera del periodo
	and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
	-- Lead Source
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ',')))
	-- PRs
	and (@PRs = 'ALL' or (S.saPR1 in (select item from split(@PRs, ','))
		or S.saPR2 in (select item from split(@PRs, ','))
		or S.saPR3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
	-- Tours de rescate y cortesia
	and (@FilterSaveCourtesyTours = 0
		or (@FilterSaveCourtesyTours = 1 and G.guSaveProgram = 0 and G.guCTour = 0)
		or (@FilterSaveCourtesyTours = 2 and ((G.guSaveProgram = 0 and G.guCTour = 0) or dbo.UFN_OR_GetGuestSales(G.guID) > 0)))
group by C.coNationality, G.gumk

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

