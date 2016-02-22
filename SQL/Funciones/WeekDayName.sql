if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[WeekDayName]') and xtype in (N'FN', N'if', N'TF'))
drop function [dbo].[WeekDayName]
GO

set QUOTED_IDENTIFIER ON 
GO
set ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene el nombre del dia de la semana de una fecha en el idioma indicado
** 
** [wtorres]	12/Sep/2014 Created
**
*/
create function [dbo].[WeekDayName](
	@Date datetime,			-- Fecha
	@Language varchar(2)	-- Clave del idioma
) 
returns varchar(20)
as 
begin

declare @WeekDay varchar(20)


set @WeekDay = dbo.UFN_OR_GetReportText('WeekDays', case DatePart(weekday, @Date)
	when 1 then 'Sunday'
	when 2 then 'Monday'    
	when 3 then 'Tuesday'   
	when 4 then 'Wednesday'
	when 5 then 'Thursday'
	when 6 then 'Friday'
	when 7 then 'Saturday'
end, @Language)

return @WeekDay
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO