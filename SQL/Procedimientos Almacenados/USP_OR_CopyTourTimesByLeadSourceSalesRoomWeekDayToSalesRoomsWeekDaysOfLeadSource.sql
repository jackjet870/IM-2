if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_CopyTourTimesByLeadSourceSalesRoomWeekDayToSalesRoomsWeekDaysOfLeadSource]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_CopyTourTimesByLeadSourceSalesRoomWeekDayToSalesRoomsWeekDaysOfLeadSource]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Copia los horarios de tour por Lead Source, sala de ventas y dia de la semana de un Lead Source, sala de ventas y dia a todas las salas de
** ventas y dias de la semana del mismo Lead Source
** 
** [wtorres]	24/May/2011 Creado
**
*/
create procedure [dbo].[USP_OR_CopyTourTimesByLeadSourceSalesRoomWeekDayToSalesRoomsWeekDaysOfLeadSource]
	@LeadSource varchar(10),	-- Clave del Lead Source
	@SalesRoom varchar(10),		-- Clave de la sala de ventas
	@WeekDay tinyint			-- Dia de la semana
as
set nocount on

-- eliminamos los horarios de tour de todas las salas de ventas y dias de las semana del mismo Lead Source menos los de la sala de ventas y
-- dia de la semana de donde se van a copiar los horarios de tour
delete from TourTimesByDay
where
	-- del Lead Source
	ttls = @LeadSource
	-- no de la sala de ventas
	and (ttsr <> @SalesRoom
	-- o si es de la sala de ventas, que no sea del mismo dia de la semana
	or (ttsr = @SalesRoom and (ttDay <> @WeekDay
	-- o si es del mismo dia de la semana, que no tenga horarios
	or (ttDay = @WeekDay and ttMaxBooks <= 0))))

-- copiamos los horarios de tour del Lead Source a todas las salas de ventas y dias de la semana del mismo Lead Source
insert into TourTimesByDay (ttls, ttsr, ttDay, ttT, ttPickUpT, ttMaxBooks)
select @LeadSource as LeadSource, S.srID, D.wdDay, T.ttT, T.ttPickUpT, T.ttMaxBooks
from TourTimesByDay T, SalesRooms S, WeekDays D
where
	-- Horarios de tour del Lead Source, sala de ventas y dia
	T.ttls = @LeadSource
	and T.ttsr = @SalesRoom
	and T.ttDay = @WeekDay
	-- Salas de ventas
	-- no de la sala de ventas
	and (S.srID <> @SalesRoom
	-- o si es de la sala de ventas, que no sea del mismo dia
	or (S.srID = @SalesRoom and (D.wdDay <> @WeekDay)))
	and S.srA = 1
	-- Dias
	and D.wdla = 'EN'
order by LeadSource, S.srID, D.wdDay, T.ttT

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

