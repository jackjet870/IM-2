if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetPRNotQualifieds]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetPRNotQualifieds]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el número de no calificados por PR
** 
** [wtorres]	21/May/2011 Creado
**
*/
create function [dbo].[UFN_OR_GetPRNotQualifieds](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Clave de los Lead Sources
	@ConsiderQuinellas bit = 0,	-- Indica si se debe considerar quinielas
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	PR varchar(10),
	NotQualifieds money
)
as
begin

insert @Table

-- No calificados (PR 1)
-- =============================================
select
	G.guPRInvit1,
	Sum(case when @ConsiderQuinellas = 0 then [dbo].UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3)
		else G.guShowsQty * [dbo].UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3) end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- PR 1
	G.guPRInvit1 is not null
	-- Fecha de show
	and ((@BasedOnArrival = 0 and G.guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Con show
	and G.guShow = 1))
	-- No Quiniela Split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- No calificado
	and G.guNQ = 1
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ',')))
group by G.guPRInvit1

-- No calificados (PR 2)
-- =============================================
union all
select
	G.guPRInvit2,
	Sum(case when @ConsiderQuinellas = 0 then [dbo].UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3)
		else G.guShowsQty * [dbo].UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3) end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- PR 2
	G.guPRInvit2 is not null
	-- Fecha de show
	and ((@BasedOnArrival = 0 and G.guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Con show
	and G.guShow = 1))
	-- No Quiniela Split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- No calificado
	and G.guNQ = 1
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ',')))
group by G.guPRInvit2

-- No calificados (PR 3)
-- =============================================
union all
select
	G.guPRInvit3,
	Sum(case when @ConsiderQuinellas = 0 then [dbo].UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3)
		else G.guShowsQty * [dbo].UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3) end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- PR 3
	G.guPRInvit3 is not null
	-- Fecha de show
	and ((@BasedOnArrival = 0 and G.guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Con show
	and G.guShow = 1))
	-- No Quiniela Split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- No calificado
	and G.guNQ = 1
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ',')))
group by G.guPRInvit3

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

