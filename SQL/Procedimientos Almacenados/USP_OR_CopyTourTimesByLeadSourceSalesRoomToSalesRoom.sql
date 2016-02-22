if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_CopyTourTimesByLeadSourceSalesRoomToSalesRoom]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_CopyTourTimesByLeadSourceSalesRoomToSalesRoom]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Copia los horarios de tour por Lead Source y sala de ventas de una sala de ventas a otra
** 
** [wtorres]	31/May/2011 Creado
**
*/
create procedure [dbo].[USP_OR_CopyTourTimesByLeadSourceSalesRoomToSalesRoom]
	@SalesRoomFrom varchar(10),	-- Clave de la sala de donde se van a copiar los horarios
	@SalesRoomTo varchar(10)	-- Clave de la sala a la que se le van a agregar los horarios
as
set nocount on

-- eliminamos los horarios de tour de la sala de ventas
delete from TourTimes where ttsr = @SalesRoomTo

-- copiamos los horarios de tour de la otra sala de ventas
insert into TourTimes (ttls, ttsr, ttT, ttPickUpT, ttMaxBooks)
select ttls, @SalesRoomTo, ttT, ttPickUpT, ttMaxBooks
from TourTimes
where ttsr = @SalesRoomFrom
order by ttls, ttT

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

