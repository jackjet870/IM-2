if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetAgencySalesRoomShows]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetAgencySalesRoomShows]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por agencia y sala
** 
** [wtorres]	08/Mar/2010 Creado
** [wtorres]	20/Abr/2010 Ahora no se cuentan como show los In & Outs
** [wtorres]	19/Oct/2011 Agregue el parametro @InOut
** [alesanchez]	04/Sep/2013 Agregue el parametro @WalkOut, @TourType, @ConsiderTourSale
**
*/
create function [dbo].[UFN_OR_GetAgencySalesRoomShows](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@FilterDeposit tinyint = 0,			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
	@InOut int = -1,						-- Filtro de In & Outs:
										--		-1. Sin filtro
										--		 0. No In & Outs
										--		 1. In & Outs
	@WalkOut int = -1,					-- Filtro de Walk Outs:
										--		-1. Sin filtro
										--		 0. No Walk Outs
										--		 1. Walk & Outs
	@TourType int = 0,					-- Filtro de tipo de tour:
										--		0. Sin filtro
										--		1. Tours regulares
										--		2. Tours de cortesia
										--		3. Tours de rescate	
	@ConsiderTourSale bit = 0			-- Indica si se debe considerar los shows con tour o venta -UPS																													
)
returns @Table table (
	Agency varchar(25),
	SalesRoom varchar(10),
	Shows int
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
	-- Fecha de show
	G.guShowD between @DateFrom and @DateTo
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
	-- WalkOut
	and (@WalkOut = -1 or G.guWalkOut = @WalkOut)
	-- Filtro de tipo de tour
	and (@TourType = 0 or (@TourType = 1 and G.guTour = 1) or (@TourType = 2 and G.guCTour = 1) or (@TourType = 3 and G.guSaveProgram = 1))		
	-- Con tour o venta - UPS
	and (@ConsiderTourSale = 0 or (G.guTour = 1 or G.guSale = 1)) 	
group by G.guag, G.gusr

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

