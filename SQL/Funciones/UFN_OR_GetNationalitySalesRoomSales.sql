if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetNationalitySalesRoomSales]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetNationalitySalesRoomSales]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas por nacionalidad y sala
** 
** [wtorres]	23/Nov/2009 Creado
** [wtorres]	25/Jul/2013 Agregue el parametro @FilterSaveCourtesyTours
** [wtorres]	19/Nov/2013 Ahora los downgrades se cuentan en base a la categoria de tipo de venta
**
*/
create function [dbo].[UFN_OR_GetNationalitySalesRoomSales](
	@DateFrom datetime,						-- Fecha desde
	@DateTo datetime,						-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',		-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',				-- Claves de PRs
	@Program varchar(10) = 'ALL',			-- Clave de programa
	@ConsiderOutOfPending bit = 0,			-- Indica si se debe considerar Out Of Pending
	@ConsiderCancel bit = 0,				-- Indica si se debe considerar canceladas
	@FilterDeposit tinyint = 0,				-- Filtro de depositos:
											--		0. Sin filtro
											--		1. Con deposito (Deposits)
											--		2. Sin deposito (Flyers)
	@FilterSaveCourtesyTours tinyint = 0	-- Filtro de tours de rescate y cortesia
											--		0. Sin filtro
											--		1. Excluir tours de rescate y cortesia
											--		2. Excluir tours de rescate y cortesia sin venta

)
returns @Table table (
	Nationality varchar(25),
	SalesRoom varchar(10),
	Sales int
)
as
begin

insert @Table
select
	C.coNationality,
	S.sasr,
	Count(*)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
	left join Countries C on G.guco = C.coID
	inner join LeadSources L on L.lsID = S.sals
where
	-- Fecha de procesable
	S.saProcD between @DateFrom and @DateTo
	-- No downgrades
	and ST.ststc <> 'DG'
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (S.saPR1 in (select item from split(@PRs, ','))
		or S.saPR2 in (select item from split(@PRs, ','))
		or S.saPR3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
	-- Tours de rescate y cortesia
	and (@FilterSaveCourtesyTours = 0
		or (@FilterSaveCourtesyTours = 1 and G.guSaveProgram = 0 and G.guCTour = 0)
		or (@FilterSaveCourtesyTours = 2 and ((G.guSaveProgram = 0 and G.guCTour = 0) or dbo.UFN_OR_GetGuestSales(G.guID) > 0)))
group by C.coNationality, S.sasr

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

