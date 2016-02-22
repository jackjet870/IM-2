if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_CopyTourTimesByLeadSourceSalesRoomToLeadSource]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_CopyTourTimesByLeadSourceSalesRoomToLeadSource]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Copia los horarios de tour por Lead Source y sala de ventas de un Lead Source a otro
** 
** [wtorres]	31/May/2011 Creado
**
*/
create procedure [dbo].[USP_OR_CopyTourTimesByLeadSourceSalesRoomToLeadSource]
	@LeadSourceFrom varchar(10),	-- Clave del Lead Source de donde se van a copiar los horarios
	@LeadSourceTo varchar(10)		-- Clave del Lead Source al que se le van a agregar los horarios
as
set nocount on

-- eliminamos los horarios de tour del Lead Source
delete from TourTimes where ttls = @LeadSourceTo

-- copiamos los horarios de tour del otro Lead Source
insert into TourTimes (ttls, ttsr, ttT, ttPickUpT, ttMaxBooks)
select @LeadSourceTo, ttsr, ttT, ttPickUpT, ttMaxBooks
from TourTimes
where ttls = @LeadSourceFrom
order by ttsr, ttT

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

