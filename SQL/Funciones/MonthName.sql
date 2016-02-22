if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[MonthName]') and xtype in (N'FN', N'if', N'TF'))
drop function [dbo].[MonthName]
GO

set QUOTED_IDENTIFIER ON 
GO
set ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene el nombre del mes de una fecha en el idioma indicado
** 
** [wtorres]	12/Sep/2014 Created
**
*/
create function [dbo].[MonthName](
	@Date datetime,			-- Fecha
	@Language varchar(2)	-- Clave del idioma
) 
returns varchar(10)
as 
begin

declare @Month varchar(10)

set @Month = dbo.UFN_OR_GetReportText('Months', case Month(@Date)
	when 1 then 'January'    
	when 2 then 'February'  
	when 3 then 'March'    
	when 4 then 'April'    
	when 5 then 'May'     
	when 6 then 'June'    
	when 7 then 'July'    
	when 8 then 'August'   
	when 9 then 'September'
	when 10 then 'October'  
	when 11 then 'November'
	when 12 then 'December'
end, @Language)

return @Month
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO