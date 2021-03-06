if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_SaveSalesRoomLog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_SaveSalesRoomLog]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desrrrollo Palace
**
** Agrega un registro en el historico de una srla de ventas si su informacion relevante cambio
** 
** [wtorres]	09/Ene/2013 Creado
** [wtorres]	25/Ene/2014 Agregue los campos de fechas de cierre de Shows, Cupones de comida y Ventas
**
*/
create procedure [dbo].[USP_OR_SaveSalesRoomLog]
	@SalesRoom varchar(10),	-- Clave de la srla de ventas
	@HoursDif smallint,		-- Horas de diferencia
	@ChangedBy varchar(10)	-- Clave del usuario que esta haciendo el cambio
as
set nocount on

declare @Count int

-- determinamos si cambio algun campo relevante
select @Count = Count(*)
from SalesRoomsLog L
	inner join SalesRooms S on S.srID = L.sqsr
where
	L.sqsr = S.srID
	and (L.sqGiftsRcptCloseD = S.srGiftsRcptCloseD or (L.sqGiftsRcptCloseD is null and S.srGiftsRcptCloseD is null))
	and (L.sqCxCCloseD = S.srCxCCloseD or (L.sqCxCCloseD is null and S.srCxCCloseD is null))
	and (L.sqShowsCloseD = S.srShowsCloseD or (L.sqShowsCloseD is null and S.srShowsCloseD is null))
	and (L.sqMealTicketsCloseD = S.srMealTicketsCloseD or (L.sqMealTicketsCloseD is null and S.srMealTicketsCloseD is null))
	and (L.sqSalesCloseD = S.srSalesCloseD or (L.sqSalesCloseD is null and S.srSalesCloseD is null))
	and L.sqID in (select Max(sqID) from SalesRoomsLog where sqsr = @SalesRoom)

-- agregamos un registro en el historico, si cambio algun campo relevante
insert into SalesRoomsLog
(sqID, sqChangedBy, sqsr, sqGiftsRcptCloseD, sqCxCCloseD, sqShowsCloseD, sqMealTicketsCloseD, sqSalesCloseD)
select
	DateAdd(hh, @HoursDif, GetDate()),
	@ChangedBy,
	srID,
	srGiftsRcptCloseD,
	srCxCCloseD,
	srShowsCloseD,
	srMealTicketsCloseD,
	srSalesCloseD
from SalesRooms
where srID = @SalesRoom and @Count = 0

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

