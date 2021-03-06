if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetSalesRoomLocationShows]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetSalesRoomLocationShows]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por locacion y sales room
** [caduran]	29/Sep/2014 Creado, basado en UFN_OR_GetLocationShows del 25/07/2014
**
*/
create function [dbo].[UFN_OR_GetSalesRoomLocationShows](
	@DateFrom datetime,		-- Fecha desde
	@DateTo datetime,		-- Fecha hasta
	@SalesRoom varchar(8000) = 'ALL'	-- Clave de la sala de ventas
)
returns @Table table (
	SalesRoom varchar(10),
	Location varchar(10),
	Shows int
)
as
begin

insert @Table
select
	gusr as SalesRoom,
	guloInvit as Location,
	Count(*) as UPS
from Guests
where
	-- Sala de ventas
	(@SalesRoom = 'ALL' or gusr in (select item from split(@SalesRoom, ',')))
	-- Fecha de show
	and guShowD between @DateFrom and @DateTo
	-- Tour, Walk Out o (Courtesy Tour, Save Tour con venta)
	and (guTour = 1 or guWalkOut = 1 or ((guCTour = 1 or guSaveProgram = 1) and guSale = 1))
group by gusr, guloInvit

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

