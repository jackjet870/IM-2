if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetCoupleTypeSalesRoomSalesAmount]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetCoupleTypeSalesRoomSalesAmount]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de ventas por regalo de tipo de pareja y sala
** 
** [wtorres]	26/Ago/2010 Creado
** [wtorres]	19/Nov/2013 Ahora los downgrades se cuentan en base a la categoria de tipo de venta
** [wtorres]	17/Ene/2014 Agregue los tipos de pareja: Couple with quinella y Quinella (Additional guests)
**
*/
create function [dbo].[UFN_OR_GetCoupleTypeSalesRoomSalesAmount](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@ConsiderOutOfPending bit = 0,		-- Indica si se debe considerar Out Of Pending
	@ConsiderCancel bit = 0,			-- Indica si se debe considerar canceladas
	@FilterDeposit tinyint = 0			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
)
returns @Table table (
	CoupleType int,
	SalesRoom varchar(10),
	SalesAmount money
)
as
begin

insert @Table
select
	case
		-- Huespedes adicionales
		when A.gaAdditional is not null then 4
		-- Parejas quinielas con huespedes adicionales
		when G.guQuinella = 1 then 3
		-- Familias
		when G.guFamily = 1 then 2
		-- Parejas solitarias
		else 1 end,
	S.sasr,
	Sum(S.saGrossAmount)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
	left join GuestsAdditional A on A.gaAdditional = G.guID
	inner join LeadSources L on L.lsID = S.sals
where
	-- Fecha de procesable
	S.saProcD between @DateFrom and @DateTo
	-- No downgrades
	and (ST.ststc <> 'DG'
	-- Downgrades cuya membresia de referencia esta dentro del periodo
	or (@ConsiderCancel = 0 and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo))
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
group by
	case
		-- Huespedes adicionales
		when A.gaAdditional is not null then 4
		-- Parejas quinielas con huespedes adicionales
		when G.guQuinella = 1 then 3
		-- Familias
		when G.guFamily = 1 then 2
		-- Parejas solitarias
		else 1 end,
	S.sasr

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

