if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetMemberSalesAmount]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetMemberSalesAmount]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de ventas por membresia
** 
** [wtorres]	25/Feb/2015 Created
** [wtorres]	19/Mar/2015 Modified. Ahora se agrupa por tipo de huesped
**
*/
create function [dbo].[UFN_OR_GetMemberSalesAmount](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@ConsiderSelfGen bit = 0,			-- Indica si se debe considerar Self Gen
	@ConsiderOutOfPending bit = 0,		-- Indica si se debe considerar Out Of Pending
	@ConsiderCancel bit = 0,			-- Indica si se debe considerar canceladas
	@FilterDeposit tinyint = 0,			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
	@Application varchar(15) = 'ALL',	-- Clave de membresia
	@Company decimal(2,0) = 0,			-- Clave de compania
	@Club int = 0,						-- Clave de club
	@OnlyWholesalers bit = 0,			-- Indica si se desean solo mayoristas
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	Club int,
	Company decimal(2,0),
	Application varchar(15),
	GuestType varchar(1),
	SalesAmount money
)
as
begin

insert @Table
select
	dbo.UFN_OR_GetClub(A.agcl, G.gucl),
	G.guCompany,
	G.guMembershipNum,
	dbo.UFN_OR_GetGuestType(G.guO1, G.guGuestRef),
	Sum(S.saGrossAmount)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
	left join Agencies A on A.agID = G.guag
	inner join LeadSources L on L.lsID = S.sals
	left join Wholesalers W on W.wscl = dbo.UFN_OR_GetClub(A.agcl, G.gucl) and W.wsCompany = G.guCompany and W.wsApplication = G.guMembershipNum
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
	or (@ConsiderCancel = 0 and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)))
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
	-- Con membresia
	and IsNull(G.guMembershipNum, '') <> ''
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
	-- Membresia
	and (@Application = 'ALL' or (G.guMembershipNum = @Application and G.guCompany = @Company and dbo.UFN_OR_GetClub(A.agcl, G.gucl) = @Club))
	-- Mayoristas
	and (@OnlyWholesalers = 0 or W.wsApplication is not null)
group by dbo.UFN_OR_GetClub(A.agcl, G.gucl), G.guCompany, G.guMembershipNum, dbo.UFN_OR_GetGuestType(G.guO1, G.guGuestRef)

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

