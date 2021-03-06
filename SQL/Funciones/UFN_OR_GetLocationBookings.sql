if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetLocationBookings]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetLocationBookings]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el número de bookings por locación
** 
** [wtorres]	28/Sep/2009 Creado
** [wtorres]	26/Nov/2010 Ahora no se cuentan los bookings cancelados
**
*/
create function [dbo].[UFN_OR_GetLocationBookings](
	@DateFrom datetime,		-- Fecha desde
	@DateTo datetime,		-- Fecha hasta
	@SalesRoom varchar(10)	-- Clave de la sala de ventas
)
returns @Table table (
	Location varchar(10),
	Books int
)
as
begin

insert @Table
select
	guloInvit as Location,
	Count(*) as Books
from Guests
where
	-- Sala de ventas
	gusr = @SalesRoom
	-- Fecha de booking
	and guBookD between @DateFrom and @DateTo
	-- No Antes In & Out
	and guAntesIO = 0
	-- No bookings cancelados
	and guBookCanc = 0
group by guloInvit

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

