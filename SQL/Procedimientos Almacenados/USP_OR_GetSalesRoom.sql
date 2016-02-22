if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetSalesRoom]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetSalesRoom]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta una sala de ventas dada su clave
** 
** [wtorres]	25/Ene/2014 Creado
**
*/
create procedure [dbo].[USP_OR_GetSalesRoom] 
	@SalesRoom varchar(10)	-- Clave de la sala de ventas
as
set nocount on

select srID, srN, srShowsCloseD, srMealTicketsCloseD, srSalesCloseD, srGiftsRcptCloseD
from SalesRooms
where srID = @SalesRoom

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

