if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetGroupContacts]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetGroupContacts]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de contactos por grupo
** 
** [wtorres]	21/Jul/2010 Creado
** [wtorres]	17/Nov/2010 Agregue el parametro @BasedOnArrival
** [wtorres]	20/Oct/2011 Ahora no se cuentan los rebooks
**
*/
create function [dbo].[UFN_OR_GetGroupContacts](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada	
)
returns @Table table (
	[Group] int,
	Contacts int
)
as
begin

insert @Table
select
	I.gjgx,
	Count(*)
from Guests G
	inner join GuestsGroupsIntegrants I on I.gjgu = G.guID
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
group by I.gjgx

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

