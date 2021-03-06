if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_CloseShows]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_CloseShows]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Cierra los shows de una sala de ventas hasta determinada fecha
** 
** [wtorres]	25/Ene/2014 Creado
**
*/
create procedure [dbo].[USP_OR_CloseShows]
	@SalesRoom as varchar(10),	-- Clave de la sala de ventas
	@Date as datetime			-- Fecha
as 
set nocount on

update SalesRooms
set srShowsCloseD = @Date
from SalesRooms
where srID = @SalesRoom
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

