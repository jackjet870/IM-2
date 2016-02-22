if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_CopyTourTimesBySalesRoomWeekDayToWeekDaysOfSalesRoom]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_CopyTourTimesBySalesRoomWeekDayToWeekDaysOfSalesRoom]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Copia los horarios de tour por sala de ventas y dia de la semana de una sala de ventas y dia a todos los dias de la semana de la misma sala de
** ventas
** 
** [wtorres]	31/May/2011 Creado
**
*/
create procedure [dbo].[USP_OR_CopyTourTimesBySalesRoomWeekDayToWeekDaysOfSalesRoom]
	@SalesRoom varchar(10),		-- Clave de la sala de ventas
	@WeekDay tinyint			-- Dia de la semana
as
set nocount on

-- eliminamos los horarios de tour de todos los dias de las semana de la misma sala de ventas menos los del dia de la semana de donde se van a
-- copiar los horarios de tour
delete from TourTimesBySalesRoomWeekDay
where
	-- de la sala de ventas
	ttsr = @SalesRoom
	-- no del mismo dia de la semana
	and (ttDay <> @WeekDay
	-- o si es del mismo dia de la semana, que no tenga horarios
	or (ttDay = @WeekDay and ttMaxBooks <= 0))

-- copiamos los horarios de tour de la sala de ventas a todos los dias de la semana de la misma sala de ventas
insert into TourTimesBySalesRoomWeekDay (ttsr, ttDay, ttT, ttPickUpT, ttMaxBooks)
select @SalesRoom as SalesRoom, D.wdDay, T.ttT, T.ttPickUpT, T.ttMaxBooks
from TourTimesBySalesRoomWeekDay T, WeekDays D
where
	-- Horarios de tour de la sala de ventas y dia
	T.ttsr = @SalesRoom
	and T.ttDay = @WeekDay
	-- no del mismo dia
	and D.wdDay <> @WeekDay
	-- Dias
	and D.wdla = 'EN'
order by D.wdDay, T.ttT

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

