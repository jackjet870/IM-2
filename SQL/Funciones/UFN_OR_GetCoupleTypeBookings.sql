if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetCoupleTypeBookings]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetCoupleTypeBookings]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de bookings por tipo de pareja
** 
** [wtorres]	26/Ago/2010 Creado
** [wtorres]	18/Oct/2010 Agregue el parámetro @BasedOnArrival y reemplace el parámetro @ConsiderDirects por @Direct
** [wtorres]	26/Nov/2010 Ahora no se cuentan los bookings cancelados
** [wtorres]	19/Dic/2013 Reemplace el parametro @ConsiderInOuts por @InOut para que el filtro de In & Outs sea mas simple
** [wtorres]	17/Ene/2014 Agregue los tipos de pareja: Couple with quinella y Quinella (Additional guests)
**
*/
create function [dbo].[UFN_OR_GetCoupleTypeBookings](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@FilterDeposit tinyint = 0,			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
										--		3. Con deposito y shows sin deposito (Deposits & Flyers Show)
	@Direct int = -1,					-- Filtro de directas:
										--		-1. Sin filtro
										--		 0. No directas
										--		 1. Directas
	@InOut int = -1,					-- Filtro de In & Outs:
										--		-1. Sin filtro
										--		 0. No In & Outs
										--		 1. In & Outs
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	CoupleType int,
	Books int
)
as
begin

insert @Table
select
	case
		-- Parejas quinielas con huespedes adicionales
		when G.guQuinella = 1 then 3
		-- Familias
		when G.guFamily = 1 then 2
		-- Parejas solitarias
		else 1 end,
	Sum(case when @ConsiderQuinellas = 0 then 1 else G.guRoomsQty end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- No Antes In & Out
	G.guAntesIO = 0
	-- No Quiniela Split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos y fechas de booking y show
	and ((@BasedOnArrival = 0 and (((@FilterDeposit <> 3 and G.guBookD between @DateFrom and @DateTo)
		and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0)))
		or (@FilterDeposit = 3 and ((G.guBookD between @DateFrom and @DateTo and G.guDeposit > 0)
		or (G.guShowD between @DateFrom and @DateTo and G.guDeposit = 0 and G.guInOut = 0)))))
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and G.guInvit = 1))
	-- Directas
	and (@Direct = -1 or G.guDirect = @Direct)
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
	-- No bookings cancelados
	and G.guBookCanc = 0
group by
	case
		-- Parejas quinielas con huespedes adicionales
		when G.guQuinella = 1 then 3
		-- Familias
		when G.guFamily = 1 then 2
		-- Parejas solitarias
		else 1 end

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

