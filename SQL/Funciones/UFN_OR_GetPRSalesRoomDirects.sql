if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetPRSalesRoomDirects]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetPRSalesRoomDirects]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Procedimiento:	Obtener directas de PR y sala
-- Descripción:		Devuelve el número de directas por PR y sala
-- Histórico:		[wtorres] 30/Oct/2009 Creado
--					[wtorres] 16/Abr/2010 Agregué los parámetros @PRs y @Program
-- =============================================
create function [dbo].[UFN_OR_GetPRSalesRoomDirects](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Clave de los Lead Sources
	@PRs varchar(8000) = 'ALL',		-- Claves de PRs
	@Program varchar(10) = 'ALL',	-- Clave de programa
	@ConsiderQuinellas bit = 0,		-- Indica si se debe considerar quinielas
	@FilterDeposit tinyint = 0		-- Filtro de depósitos:
									--		0. Sin filtro
									--		1. Con depósito (Deposits)
									--		2. Sin depósito (Flyers)
)
returns @Table table (
	PR varchar(10),
	SalesRoom varchar(10),
	Directs money
)
as
begin

insert @Table

-- Directas (PR 1)
-- =============================================
select
	G.guPRInvit1,
	G.gusr,
	Sum(case when @ConsiderQuinellas = 0 then [dbo].UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3)
		else G.guShowsQty * [dbo].UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3) end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- PR 1
	G.guPRInvit1 is not null
	-- Fecha de reservación
	and G.guBookD between @DateFrom and @DateTo
	-- Directas
	and G.guDirect = 1
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depósitos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
group by G.guPRInvit1, G.gusr

-- Directas (PR 2)
-- =============================================
union all
select
	G.guPRInvit2,
	G.gusr,
	Sum(case when @ConsiderQuinellas = 0 then [dbo].UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3)
		else G.guShowsQty * [dbo].UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3) end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- PR 2
	G.guPRInvit2 is not null
	-- Fecha de reservación
	and G.guBookD between @DateFrom and @DateTo
	-- Directas
	and G.guDirect = 1
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depósitos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
group by G.guPRInvit2, G.gusr

-- Directas (PR 3)
-- =============================================
union all
select
	G.guPRInvit3,
	G.gusr,
	Sum(case when @ConsiderQuinellas = 0 then [dbo].UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3)
		else G.guShowsQty * [dbo].UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3) end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- PR 3
	G.guPRInvit3 is not null
	-- Fecha de reservación
	and G.guBookD between @DateFrom and @DateTo
	-- Directas
	and G.guDirect = 1
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depósitos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
group by G.guPRInvit3, G.gusr

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

