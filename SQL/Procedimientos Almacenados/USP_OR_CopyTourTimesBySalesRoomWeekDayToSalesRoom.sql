if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_CopyTourTimesBySalesRoomWeekDayToSalesRoom]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_CopyTourTimesBySalesRoomWeekDayToSalesRoom]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Copia los horarios de tour por sala de ventas y dia de la semana de una sala de ventas a otra
** 
** [wtorres]	31/May/2011 Creado
**
*/
create procedure [dbo].[USP_OR_CopyTourTimesBySalesRoomWeekDayToSalesRoom]
	@SalesRoomFrom varchar(10),	-- Clave de la sala de donde se van a copiar los horarios
	@SalesRoomTo varchar(10)	-- Clave de la sala a la que se le van a agregar los horarios
as
set nocount on

-- eliminamos los horarios de tour de la sala de ventas
delete from TourTimesBySalesRoomWeekDay where ttsr = @SalesRoomTo

-- copiamos los horarios de tour de la otra sala de ventas
insert into TourTimesBySalesRoomWeekDay (ttsr, ttDay, ttT, ttPickUpT, ttMaxBooks)
select @SalesRoomTo, ttDay, ttT, ttPickUpT, ttMaxBooks
from TourTimesBySalesRoomWeekDay
where ttsr = @SalesRoomFrom
order by ttDay, ttT

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

