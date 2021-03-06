if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetAgencySalesRoomBookings]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetAgencySalesRoomBookings]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de bookings por agencia y sala
** 
** [wtorres]	08/Mar/2010 Creado
** [wtorres]	26/Nov/2010 Ahora no se cuentan los bookings cancelados
** [wtorres]	19/Dic/2013 Reemplace el parametro @ConsiderInOuts por @InOut para que el filtro de In & Outs sea mas simple
**
*/
create function [dbo].[UFN_OR_GetAgencySalesRoomBookings](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@FilterDeposit tinyint = 0,			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
										--		3. Con deposito y shows sin deposito (Deposits & Flyers Show)
	@InOut int = -1						-- Filtro de In & Outs:
										--		-1. Sin filtro
										--		 0. No In & Outs
										--		 1. In & Outs
)
returns @Table table (
	Agency varchar(25),
	SalesRoom varchar(10),
	Books int
)
as
begin

insert @Table
select
	G.guag,
	G.gusr,
	Count(*)
from Guests G	
	inner join LeadSources L on L.lsID = G.guls
where
	-- No Antes In & Out
	G.guAntesIO = 0
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos y fechas de booking y show
	and (((@FilterDeposit <> 3 and G.guBookD between @DateFrom and @DateTo)
		and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0)))
		or (@FilterDeposit = 3 and ((G.guBookD between @DateFrom and @DateTo and G.guDeposit > 0)
		or (G.guShowD between @DateFrom and @DateTo and G.guDeposit = 0 and G.guInOut = 0))))
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
	-- No bookings cancelados
	and G.guBookCanc = 0
group by G.guag, G.gusr

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

