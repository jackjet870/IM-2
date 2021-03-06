if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetPRSalesRoomSales]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetPRSalesRoomSales]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas por PR y sala
** 
** [wtorres]		30/Oct/2009 Created
** [wtorres]		24/Nov/2009 Modified. Agregue el parametro @ConsiderCancel
** [wtorres]		16/Abr/2010 Modified. Agregue los parametros @PRs y @Program
** [wtorres]		16/Nov/2011 Modified. Agregue el parametro @ConsiderPending
** [wtorres]		19/Nov/2013 Modified. Ahora los downgrades se cuentan en base a la categoria de tipo de venta
** [axperez]		04/Dic/2013 Modified. Agregue el parametro @ConsiderCancel
** [lormartinez]	22/Sep/2014 Modified. Se agrega parametro para filtro basado en fecha de booking
** [wtorres]		15/Jul/2015 Modified. Correccion de error. No estaba contando las ventas que no tuvieran Guest ID
** [wtorres]		09/Ene/2016 Modified. Correccion de error. No estaba contando las ventas pendientes (basado en la fecha de venta)
**								cuya fecha de procesable no estuviera en el rango del reporte
** [edgrodriguez]	05/May/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/
CREATE function [dbo].[UFN_OR_GetPRSalesRoomSales](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Clave de los Lead Sources
	@PRs varchar(8000) = 'ALL',		-- Claves de PRs
	@Program varchar(10) = 'ALL',	-- Clave de programa
	@ConsiderSelfGen bit = 0,		-- Indica si se debe considerar Self Gen
	@ConsiderOutOfPending bit = 0,	-- Indica si se debe considerar Out Of Pending
	@ConsiderCancel bit = 0,		-- Indica si se debe considerar canceladas
	@FilterDeposit tinyint = 0,		-- Filtro de depositos:
									--		0. Sin filtro
									--		1. Con deposito (Deposits)
									--		2. Sin deposito (Flyers)
	@ConsiderPending bit = 0,		-- Indica si se debe considerar pendientes
	@BasedOnArrival bit = 0,		-- Indica si se debe basar en la fecha de llegada
	@BasedOnBooking bit = 0			-- Indica si se debe basar en la fecha de booking
)
returns @Table table (
	PR varchar(10),
	SalesRoom varchar(10),
	Sales money
)
as
begin

insert @Table

-- Numero de ventas (PR 1)
-- =============================================
select
	S.saPR1,
	S.sasr,
	Sum([dbo].UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	inner join LeadSources L on L.lsID = S.sals
	left join Guests G on S.sagu = G.guID
where
	-- PR 1
	S.saPR1 is not null
	-- Fecha de procesable
	and ((((@BasedOnArrival = 0 and @BasedOnBooking = 0) or ((@BasedOnArrival = 1 or @BasedOnBooking = 1) and S.sagu is null))
	and ((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo))))
	-- Fecha de llegada
	or (((@BasedOnArrival = 1 and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo)
	-- Fecha de booking
	or (@BasedOnBooking = 1 and S.sagu is not null and G.guBookD between @DateFrom and @DateTo))
	-- Procesable
	and ((@ConsiderPending = 0 and S.saProc = 1) or (@ConsiderPending = 1 and S.saProc = 0))))
	-- No downgrades
	and ST.ststc <> 'DG'
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (S.saPR1 in (select item from split(@PRs, ','))
		or S.saPR2 in (select item from split(@PRs, ','))
		or S.saPR3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
group by S.saPR1, S.sasr

-- Numero de ventas (PR 2)
-- =============================================
union all
select
	S.saPR2,
	S.sasr,
	Sum([dbo].UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	inner join LeadSources L on L.lsID = S.sals
	left join Guests G on S.sagu = G.guID
where
	-- PR 2
	S.saPR2 is not null
	-- Fecha de procesable
	and ((((@BasedOnArrival = 0 and @BasedOnBooking = 0) or ((@BasedOnArrival = 1 or @BasedOnBooking = 1) and S.sagu is null))
	and ((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo))))
	-- Fecha de llegada
	or (((@BasedOnArrival = 1 and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo)
	-- Fecha de booking
	or (@BasedOnBooking = 1 and S.sagu is not null and G.guBookD between @DateFrom and @DateTo))
	-- Procesable
	and ((@ConsiderPending = 0 and S.saProc = 1) or (@ConsiderPending = 1 and S.saProc = 0))))
	-- No downgrades
	and ST.ststc <> 'DG'
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (S.saPR1 in (select item from split(@PRs, ','))
		or S.saPR2 in (select item from split(@PRs, ','))
		or S.saPR3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
group by S.saPR2, S.sasr

-- Numero de ventas (PR 3)
-- =============================================
union all
select
	S.saPR3,
	S.sasr,
	Sum([dbo].UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	inner join LeadSources L on L.lsID = S.sals
	left join Guests G on S.sagu = G.guID
where
	-- PR 3
	S.saPR3 is not null
	-- Fecha de procesable
	and ((((@BasedOnArrival = 0 and @BasedOnBooking = 0) or ((@BasedOnArrival = 1 or @BasedOnBooking = 1) and S.sagu is null))
	and ((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo))))
	-- Fecha de llegada
	or (((@BasedOnArrival = 1 and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo)
	-- Fecha de booking
	or (@BasedOnBooking = 1 and S.sagu is not null and G.guBookD between @DateFrom and @DateTo))
	-- Procesable
	and ((@ConsiderPending = 0 and S.saProc = 1) or (@ConsiderPending = 1 and S.saProc = 0))))
	-- No downgrades
	and ST.ststc <> 'DG'
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (S.saPR1 in (select item from split(@PRs, ','))
		or S.saPR2 in (select item from split(@PRs, ','))
		or S.saPR3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
group by S.saPR3, S.sasr

return
end