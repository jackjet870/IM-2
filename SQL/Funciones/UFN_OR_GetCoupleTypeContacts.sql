if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetCoupleTypeContacts]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetCoupleTypeContacts]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de contactos por tipo de pareja
** 
** [wtorres]	26/Ago/2010 Creado
** [wtorres]	18/Oct/2010 Agregue el parametro @BasedOnArrival
** [wtorres]	20/Oct/2011 Ahora no se cuentan los rebooks
** [wtorres]	17/Ene/2014 Agregue los tipos de pareja: Couple with quinella y Quinella (Additional guests)
**
*/
create function [dbo].[UFN_OR_GetCoupleTypeContacts](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',		-- Claves de PRs
	@Program varchar(10) = 'ALL',	-- Clave de programa
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada	
)
returns @Table table (
	CoupleType int,
	Contacts int
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
	Count(*)
from Guests G
	left join GuestsAdditional A on A.gaAdditional = G.guID
	inner join LeadSources L on L.lsID = G.guls
where
	-- Fecha de contacto
	((@BasedOnArrival = 0 and G.guInfoD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Contactado
	and G.guInfo = 1))
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Lead Source
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ',')))
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
group by
	case
		-- Huespedes adicionales
		when A.gaAdditional is not null then 4
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

