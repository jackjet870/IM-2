if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetPRGroupSales]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetPRGroupSales]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas por PR y grupo
** 
** [wtorres]		11/Ago/2010 Created
** [wtorres]		18/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [wtorres]		19/Nov/2013 Modified. Ahora los downgrades se cuentan en base a la categoria de tipo de venta
** [edgrodriguez]	05/may/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/
CREATE function [dbo].[UFN_OR_GetPRGroupSales](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@ConsiderSelfGen bit = 0,			-- Indica si se debe considerar Self Gen
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	PR varchar(10),
	[Group] int,
	Sales money
)
as
begin

insert @Table

-- Numero de ventas (PR 1)
-- =============================================
select
	S.saPR1,
	I.gjgx,
	Sum(dbo.UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
	left join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- PR 1
	S.saPR1 is not null
	-- Fecha de procesable
	and (((@BasedOnArrival = 0 or (@BasedOnArrival = 1 and S.sagu is null)) and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1	and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo
	-- Procesable
	and S.saProc = 1))
	-- No downgrades
	and ST.ststc <> 'DG'
	-- Fecha de cancelacion
	and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ','))) 
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
group by S.saPR1, I.gjgx

-- Numero de ventas (PR 2)
-- =============================================
union all
select
	S.saPR2,
	I.gjgx,
	Sum(dbo.UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
	left join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- PR 2
	S.saPR2 is not null
	-- Fecha de procesable
	and (((@BasedOnArrival = 0 or (@BasedOnArrival = 1 and S.sagu is null)) and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1	and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo
	-- Procesable
	and S.saProc = 1))
	-- No downgrades
	and ST.ststc <> 'DG'
	-- Fecha de cancelacion
	and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ','))) 
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
group by S.saPR2, I.gjgx

-- Numero de ventas (PR 3)
-- =============================================
union all
select
	S.saPR3,
	I.gjgx,
	Sum(dbo.UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
	left join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- PR 3
	S.saPR3 is not null
	-- Fecha de procesable
	and (((@BasedOnArrival = 0 or (@BasedOnArrival = 1 and S.sagu is null)) and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1	and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo
	-- Procesable
	and S.saProc = 1))
	-- No downgrades
	and ST.ststc <> 'DG'
	-- Fecha de cancelacion
	and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ','))) 
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
group by S.saPR3, I.gjgx

return
end