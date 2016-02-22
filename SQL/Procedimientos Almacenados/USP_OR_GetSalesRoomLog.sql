if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetSalesRoomLog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetSalesRoomLog]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta el registro historico de una sala de ventas
** 
** [wtorres]	09/Ene/2013 Creado
** [wtorres]	25/Ene/2014 Agregue los campos de fechas de cierre de Shows, Cupones de comida y Ventas
**
*/
create procedure [dbo].[USP_OR_GetSalesRoomLog] 
	@SalesRoom varchar(10)	-- Clave de la sala de ventas
as
set nocount on

select 
	L.sqChangedBy,
	P.peN as ChangedByN,
	L.sqID,
	L.sqShowsCloseD,
	L.sqMealTicketsCloseD,
	L.sqSalesCloseD,
	L.sqGiftsRcptCloseD,
	L.sqCxCCloseD
from SalesRoomsLog L
	left join Personnel P on P.peID = L.sqChangedBy
where L.sqsr = @SalesRoom
order by L.sqID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

