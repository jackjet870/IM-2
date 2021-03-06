if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_CloseSales]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_CloseSales]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Cierra las ventas de una sala de ventas hasta determinada fecha
** 
** [wtorres]	25/Ene/2014 Creado
**
*/
create procedure [dbo].[USP_OR_CloseSales]
	@SalesRoom as varchar(10),	-- Clave de la sala de ventas
	@Date as datetime			-- Fecha
as 
set nocount on

update SalesRooms
set srSalesCloseD = @Date
from SalesRooms
where srID = @SalesRoom
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

