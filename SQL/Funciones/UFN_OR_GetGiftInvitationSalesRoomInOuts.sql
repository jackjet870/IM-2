if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetGiftInvitationSalesRoomInOuts]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetGiftInvitationSalesRoomInOuts]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Procedimiento:	Obtener In & Outs de regalo de invitación y sala
-- Descripción:		Devuelve el número de In & Outs por regalo de invitación y sala
-- Histórico:		[wtorres] 05/Nov/2009 Creado
--					[wtorres] 25/Nov/2009 Agregué el parámetro @FilterDeposit
--					[wtorres] 08/Mar/2010 Eliminé el parámetro @Gross
-- =============================================
create function [dbo].[UFN_OR_GetGiftInvitationSalesRoomInOuts](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@Gifts varchar(8000) = 'ALL',		-- Claves de regalos
	@FilterDeposit tinyint = 0			-- Filtro de depósitos:
										--		0. Sin filtro
										--		1. Con depósito (Deposits)
										--		2. Sin depósito (Flyers)
										--		3. Con depósito y shows sin depósito (Deposits & Flyers Show)
)
returns @Table table (
	Gift varchar(10),
	SalesRoom varchar(10),
	InOuts int
)
as
begin

insert @Table
select
	I.iggi,
	G.gusr,
	Count(*)
from Guests G
	inner join InvitsGifts I on I.iggu = G.guID
	inner join LeadSources L on L.lsID = G.guls
where
	-- Fecha de show
	G.guShowD between @DateFrom and @DateTo
	-- In & Out
	and G.guInOut = 1
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Regalos
	and (@Gifts = 'ALL' or I.iggi in (select item from split(@Gifts, ',')))
	-- Filtro de depósitos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0)
		or (@FilterDeposit = 3 and ((G.guBookD between @DateFrom and @DateTo and G.guDeposit > 0) 
		or (G.guShowD between @DateFrom and @DateTo and G.guDeposit = 0))))
group by I.iggi, G.gusr

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

