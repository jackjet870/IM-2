if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetLocationMonthShows]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetLocationMonthShows]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por locacion y mes
** 
** [wtorres]	22/Oct/2009 Creado
** [wtorres]	26/Sep/2012 Ahora cuenta a los Courtesy Tour y Save Tours si tienen venta
**
*/
create function [dbo].[UFN_OR_GetLocationMonthShows](
	@DateFrom datetime,		-- Fecha desde
	@DateTo datetime,		-- Fecha hasta
	@SalesRoom varchar(10)	-- Clave de la sala de ventas
)
returns @Table table (
	Location varchar(10),
	[Year] int,
	[Month] int,
	Shows int
)
as
begin

insert @Table
select
	guloInvit as Location,
	Year(guShowD),
	Month(guShowD),
	Count(*) as UPS
from Guests
where
	-- Sala de ventas
	gusr = @SalesRoom
	-- Fecha de show
	and guShowD between @DateFrom and @DateTo
	-- Tour, Walk Out o (Courtesy Tour, Save Tour con venta)
	and (guTour = 1 or guWalkOut = 1 or ((guCTour = 1 or guSaveProgram = 1) and guSale = 1))
group by guloInvit, Year(guShowD), Month(guShowD)

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

