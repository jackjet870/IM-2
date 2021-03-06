if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetAvailables]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetAvailables]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de disponibles
** 
** [wtorres]	08/May/2010 Creado
** [wtorres]	12/Ago/2010 Agregue el parametro @ConsiderQuinellas
**
*/
create function [dbo].[UFN_OR_GetAvailables](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0	-- Indica si se debe considerar quinielas
)
returns int
as
begin

declare @Result int

select @Result = IsNull(Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then 1 else guShowsQty end), 0)
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

return @Result
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

