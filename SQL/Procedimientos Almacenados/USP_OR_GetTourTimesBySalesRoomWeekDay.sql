if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetTourTimesBySalesRoomWeekDay]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetTourTimesBySalesRoomWeekDay]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene los horarios de tour por sala de ventas y dia de la semana
** 
** [wtorres]	13/May/2014 Created
**
*/
create procedure [dbo].[USP_OR_GetTourTimesBySalesRoomWeekDay]
	@SalesRoom varchar(10) = 'ALL',	-- Clave de sala de venta
	@WeekDay int = 0,				-- Clave de dia de la semana: 1 - Domingo, 2 - Lunes, etc
	@Language varchar(10) = 'EN'	-- Clave de idioma
as
set nocount on

select T.ttsr, S.srN, T.ttDay, W.wdN, Convert(varchar(5), T.ttT, 114) as ttT, T.ttMaxBooks
from TourTimesBySalesRoomWeekDay T
	left join SalesRooms S on S.srID = T.ttsr
	left join WeekDays W on W.wdDay = T.ttDay
where
	-- Sala de ventas
	(@SalesRoom = 'ALL' or T.ttsr = @SalesRoom)
	-- Dia de la semana
	and (@WeekDay = 0 or T.ttDay = @WeekDay)
	-- Idioma
	and W.wdla = @Language

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

