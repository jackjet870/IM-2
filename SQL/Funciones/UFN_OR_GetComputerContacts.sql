if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetComputerContacts]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetComputerContacts]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de contactos por computadora
** 
** [wtorres]	07/Jun/2010 Creado
** [wtorres]	18/Oct/2010 Agregue el parametro @BasedOnArrival
** [wtorres]	20/Oct/2011 Ahora no se cuentan los rebooks
**
*/
create function [dbo].[UFN_OR_GetComputerContacts](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada	
)
returns @Table table (
	Computer varchar(15),
	Contacts int
)
as
begin

insert @Table

-- Contactos
-- =============================================
select
	M.gmcp,
	Count(*)
from Guests G
	left join GuestsMovements M on G.guID = M.gmgu and M.gmgn = 'CN'
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
	and G.guls in (select item from split(@LeadSources, ','))
group by M.gmcp

-- Bookings donde las computadoras de booking y contacto son diferentes
-- =============================================
union all
select
	M.gmcp,
	Count(*)
from Guests G
	left join GuestsMovements M on G.guID = M.gmgu and M.gmgn = 'BK'
where
	-- Computadora de booking diferente de la computadora de contacto
	M.gmcp not in (
		select MC.gmcp
		from GuestsMovements MC
		where MC.gmgu = M.gmgu and MC.gmgn = 'CN'
	)
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and G.guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and G.guInvit = 1))
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Lead Source
	and G.guls in (select item from split(@LeadSources, ','))
group by M.gmcp

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

