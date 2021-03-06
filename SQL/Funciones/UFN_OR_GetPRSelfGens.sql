if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetPRSelfGens]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetPRSelfGens]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el número de self gens por PR
** 
** [wtorres]	18/Sep/2009 Creado
** [wtorres]	23/Sep/2009 Convertido a función. Agregué el parámetro @LeadSources11/Ago/2010 Creado
** [wtorres]	18/Nov/2010 Agregué el parámetro @BasedOnArrival
**
*/
create function [dbo].[UFN_OR_GetPRSelfGens](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Clave de los Lead Sources
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	PR varchar(10),
	SelfGens money
)
as
begin

insert @Table
select
	guPRInvit1,
	Sum(guShowsQty)
from Guests
where
	-- PR 1
	guPRInvit1 is not null
	-- Fecha de show
	and ((@BasedOnArrival = 0 and guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Con show
	and guShow = 1))
	-- Self Gen
	and guSelfGen = 1
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
group by guPRInvit1

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

