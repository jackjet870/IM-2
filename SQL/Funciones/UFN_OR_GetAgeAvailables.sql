if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetAgeAvailables]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetAgeAvailables]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el número de disponibles por edad
** 
** [wtorres]	18/Oct/2010 Creado
**
*/
create function [dbo].[UFN_OR_GetAgeAvailables](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0	-- Indica si se debe considerar quinielas
)
returns @Table table (
	Age tinyint,
	Availables int
)
as
begin

insert @Table
select
	guAge1,
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then 1 else guShowsQty end)
from Guests
where
	-- Fecha de llegada
	guCheckInD between @DateFrom and @DateTo
	-- Con Check In
	and guCheckIn = 1
	-- Disponible
	and guAvail = 1
	-- No Rebook
	and guRef is null
	-- Contactado
	and guInfo = 1
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
group by guAge1

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
