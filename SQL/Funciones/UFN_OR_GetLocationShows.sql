if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetLocationShows]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetLocationShows]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por locacion
** 
** [wtorres]	25/Sep/2009 Creado
** [wtorres]	26/Sep/2012 Ahora cuenta a los Courtesy Tour y Save Tours si tienen venta
** [gmaya]		25/07/2014 Se modifico el parametro @SalesRoom a varchar(8000)  = 'ALL'
** [gmaya]		25/07/2014 Se agrego (@SalesRoom = 'ALL' or gusr in (select item from split(@SalesRoom, ',')))
**
*/
create function [dbo].[UFN_OR_GetLocationShows](
	@DateFrom datetime,		-- Fecha desde
	@DateTo datetime,		-- Fecha hasta
	@SalesRoom varchar(8000) = 'ALL'	-- Clave de la sala de ventas
)
returns @Table table (
	Location varchar(10),
	Shows int
)
as
begin

insert @Table
select
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
group by guloInvit

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

