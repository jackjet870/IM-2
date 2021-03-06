if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetPRContactSales]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetPRContactSales]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas por PR de contactos
** 
** [lchairez]	10/Dic/2013 Created
** [wtorres]	09/Ene/2016 Modified. Correccion de error. No estaba contando las ventas pendientes (basado en la fecha de venta)
**							cuya fecha de procesable no estuviera en el rango del reporte
**
*/
create function [dbo].[UFN_OR_GetPRContactSales](
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
	@BasedOnArrival bit = 0,		-- Indica si se debe basar en la fecha de llegada
	@ConsiderPending bit = 0		-- Indica si se debe considerar pendientes
)
returns @Table table (
	PR varchar(10),
	Sales money
)
as
begin

insert @Table

select
	G.guPRInfo,
	Count(*)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	inner join LeadSources L on L.lsID = S.sals
	left join Guests G on S.sagu = G.guID
where
	-- PR 1
	 G.guPRInfo is not null
	-- Fecha de procesable
	and (((@BasedOnArrival = 0 or (@BasedOnArrival = 1 and S.sagu is null)) and ((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo))))
	-- Fecha de llegada
	or (@BasedOnArrival = 1	and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo
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
	and (@PRs = 'ALL' or ( G.guPRInfo in (select item from split(@PRs, ',')) ))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
group by  G.guPRInfo

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

