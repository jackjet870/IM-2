if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_CopyTourTimesByLeadSourceSalesRoomWeekDayToSalesRoom]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_CopyTourTimesByLeadSourceSalesRoomWeekDayToSalesRoom]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Copia los horarios de tour por Lead Source, sala de ventas y dia de la semana de una sala de ventas a otra
** 
** [wtorres]	24/May/2011 Creado
**
*/
create procedure [dbo].[USP_OR_CopyTourTimesByLeadSourceSalesRoomWeekDayToSalesRoom]
	@SalesRoomFrom varchar(10),	-- Clave de la sala de donde se van a copiar los horarios
	@SalesRoomTo varchar(10)	-- Clave de la sala a la que se le van a agregar los horarios
as
set nocount on

-- eliminamos los horarios de tour de la sala de ventas
delete from TourTimesByDay where ttsr = @SalesRoomTo

-- copiamos los horarios de tour de la otra sala de ventas
insert into TourTimesByDay (ttls, ttsr, ttDay, ttT, ttPickUpT, ttMaxBooks)
select ttls, @SalesRoomTo, ttDay, ttT, ttPickUpT, ttMaxBooks
from TourTimesByDay
where ttsr = @SalesRoomFrom
order by ttls, ttDay, ttT

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

