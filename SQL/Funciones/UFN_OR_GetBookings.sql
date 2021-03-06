if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetBookings]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetBookings]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de bookings
** 
** [wtorres]	08/May/2010 Creado
** [wtorres]	24/Nov/2010 Agregue el parametro @BasedOnArrival
** [wtorres]	26/Nov/2010 Ahora no se cuentan los bookings cancelados
** [wtorres]	19/Dic/2013 Agregue el parametro @ConsiderQuinellas
**
*/
create function [dbo].[UFN_OR_GetBookings](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns int
as
begin

declare @Result int

select @Result = Sum(case when @ConsiderQuinellas = 0 then 1 else guRoomsQty end)
from Guests
where
	-- Fecha de booking
	((@BasedOnArrival = 0 and guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and guInvit = 1))
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Sources
	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ','))) 
	-- No bookings cancelados
	and guBookCanc = 0

return @Result
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

