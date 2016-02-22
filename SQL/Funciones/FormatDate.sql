if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FormatDate]') and xtype in (N'FN', N'if', N'TF'))
drop function [dbo].[FormatDate]
GO

set QUOTED_IDENTIFIER ON 
GO
set ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Formatea una fecha en base a un idioma
** 
** [wtorres]	12/Sep/2014 Created
**
*/
create function [dbo].[FormatDate](
	@Date datetime,			-- Fecha a formatear
	@Language varchar(2),	-- Clave del idioma
	@Format varchar(20)		-- Formato de fecha
							--		Short. Formato corto (dia/mes/año o month/day/year)
							--		Medium. Formato medio (mes dia, año)
							--		Long. Formato largo (dia de mes de año)
							--		LongWeekDay. Formato largo con dia de la semana (dia de la semana, dia de mes de año)
) 
returns varchar(50)
as 
begin

declare
	@DateText varchar(50),
	@Day varchar(2),
	@Month varchar(10),
	@Year varchar(4)

if Convert(varchar, @Date, 112) = '17530101'
	set @DateText = ''

else begin

	-- obtenemos el dia, el mes y el año en formato texto
	set @Day = Cast(Day(@Date) as varchar)
	set @Month = dbo.MonthName(@Date, @Language)
	set @Year = Cast(Year(@Date) as varchar)
	
	-- obtenemos el mes en formato corto
	if @Format = 'Short'
		set @Month = Substring(@Month, 1, 3)
	
	-- obtenemos la fecha en formato texto
	set @DateText = case @Format
		-- FORMATO CORTO
		when 'Short' then case when @Language = 'E'
			-- Ingles
			then @Month + '/' + @Day + '/' + @Year
			-- Los demas idiomas
			else @Day + '/' + @Month + '/' + @Year
			end

		-- FORMATO MEDIO
		when 'Medium' then case when @Language = 'P'
			-- Portugues
			then @Day + ' ' + @Month + ', ' + @Year
			-- Los demas idiomas
			else @Month + ' ' + @Day + ', ' + @Year
			end

		-- FORMATO LARGO
		when 'Long' then @Day + ' ' + dbo.UFN_OR_GetReportText('General', 'Of', @Language) + ' ' + @Month
				+ ' ' + dbo.UFN_OR_GetReportText('General', 'Of', @Language) + ' ' + @Year

		-- FORMATO LARGO CON DIA DE LA SEMANA
		when 'LongWeekDay' then dbo.WeekDayName(@Date, @Language) + ', ' + @Day
				+ ' ' + dbo.UFN_OR_GetReportText('General', 'Of', @Language) + ' ' + @Month
				+ ' ' + dbo.UFN_OR_GetReportText('General', 'Of', @Language) + ' ' + @Year
		end
end

return @DateText
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO