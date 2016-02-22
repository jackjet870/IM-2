USE [OrigosVCPalace];
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Convierte un numero a letras
**
** [wtorres]	08/Oct/2012	Created
** [wtorres]	08/Nov/2012	Agregue los parametros @Gender y @Original
** [LorMartinez] 18/Nov/2015 Modified, Se agrega a BD de Origos 
*/
CREATE function [dbo].[NumberToLetters] (
	@Number bigint,				-- Numero a convertir
	@Language varchar(2),		-- Clave del idioma
	@Gender varchar(1) = 'N',	-- Genero:
								--		N. Ninguno
								--		M. Masculino
								--		F. Femenino
	@Original bit = 1			-- Indica si es la invocacion original, es decir, si no es una invocacion recursiva
)
returns varchar(max)
as  
begin

declare
	@Letters varchar(max),
	@TextId varchar(50),	-- Clave del texto
	@Millions bigint,		-- Numero de millones
	@Thousands bigint,		-- Numero de millares
	@Hundreds bigint,		-- Numero de centenas
	@Tens bigint,			-- Numero de decenas
	@Units bigint			-- Numero de unidades

set @Letters = ''
set @Units = Abs(@Number)

-- determinamos si es cero
if @Units = 0
  set @Letters = dbo.UFN_OR_GetReportText('Numbers', 'Zero', @Language)

-- obtenemos los millones
set @Millions = Floor(@Units / 1000000)
set @Units = @Units - (@Millions * 1000000)

-- obtenemos los millares
set @Thousands = Floor(@Units / 1000)
set @Units = @Units - (@Thousands * 1000)

-- obtenemos las centenas
set @Hundreds = Floor(@Units / 100)
set @Units = @Units - (@Hundreds * 100)

-- obtenemos las decenas
set @Tens = Floor(@Units / 10)

-- obtenemos las unidades
if @Tens > 1
	set @Units = @Units - (@Tens * 10)

-- MILLONES
if @Millions > 0
	set @Letters = dbo.AddString(@Letters, dbo.NumberToLetters(@Millions, @Language, @Gender, 0) + ' '
		+ dbo.UFN_OR_GetReportText('Numbers', case when @Millions > 1 then 'Millions' else 'Million' end, @Language), ' ')

-- MILES
if @Thousands > 0
	set @Letters = dbo.AddString(@Letters, dbo.NumberToLetters(@Thousands, @Language, @Gender, 0) + ' '
		+ dbo.UFN_OR_GetReportText('Numbers', 'Thousand', @Language), ' ')

-- CENTENAS
if @Hundreds > 0 begin
  
  
	-- cien
	if @Hundreds = 1 and @Tens = 0 and @Units = 0
		set @Letters = @Letters + dbo.UFN_OR_GetReportText('Numbers', 'Hundred', @Language)
	else begin
		set @TextId = case @Hundreds
			when 1 then 'OneHundred'
			when 2 then 'TwoHundred'
			when 3 then 'ThreeHundred'
			when 4 then 'FourHundred'
			when 5 then 'FiveHundred'
			when 6 then 'SixHundred'
			when 7 then 'SevenHundred'
			when 8 then 'EightHundred'
			when 9 then 'NineHundred'
			end
		set @Letters = dbo.AddString(@Letters, IsNull(dbo.UFN_OR_GetReportText('Numbers', @TextId, @Language), ''), ' ')
	end
end


-- DECENAS
if @Tens > 1 begin

	-- veinte
	if @Tens = 2 and @Units = 0
		set @Letters = dbo.AddString(@Letters, dbo.UFN_OR_GetReportText('Numbers', 'Twenty', @Language), ' ')
	else begin
		set @TextId = case @Tens
			when 2 then 'Twenty_'
			when 3 then 'Thirty'
			when 4 then 'Forty'
			when 5 then 'Fifty'
			when 6 then 'Sixty'
			when 7 then 'Seventy'
			when 8 then 'Eighty'
			when 9 then 'Ninety'
			end
		set @Letters =dbo.AddString(@Letters, IsNull(dbo.UFN_OR_GetReportText('Numbers', @TextId, @Language), ''), ' ')

		-- treinta y tantos, cuarenta y tantos, etc
		if @Tens >= 3 and @Units > 0
			set @Letters = @Letters + case @Language
				when 'S' then ' y'
				when 'E' then '-'
				when 'P' then ' e'
			end
	end
end

-- UNIDADES
set @TextId = case @Units
	when 1 then 'One'
	when 2 then 'Two'
	when 3 then 'Three'
	when 4 then 'Four'
	when 5 then 'Five'
	when 6 then 'Six'
	when 7 then 'Seven'
	when 8 then 'Eight'
	when 9 then 'Nine'
	when 10 then 'Ten'
	when 11 then 'Eleven'
	when 12 then 'Twelve'
	when 13 then 'Thirteen'
	when 14 then 'Fourteen'
	when 15 then 'Fifteen'
	when 16 then 'Sixteen'
	when 17 then 'Seventeen'
	when 18 then 'Eighteen'
	when 19 then 'Nineteen'
	when 20 then 'Twenty'
	end

-- agregamos un espacio para separar las decenas de las unidades
set @Letters = dbo.AddString(@Letters, case when @TextId is not null then dbo.UFN_OR_GetReportText('Numbers', @TextId, @Language) else '' end,
	case when @Tens = 2 or (@Language = 'E') then '' else ' ' end)

-- definimos el genero
-- uno o una (solo aplica si es una palabra)
if @Original = 1 and @Language in ('S', 'P') and @Gender in ('M', 'F') and @Number = 1
	set @Letters = @Letters + case @Gender
		when 'M' then 'o'
		when 'F' then 'a'
	end
-- duas (solo aplica a la ultima palabra)
if @Original = 1 and @Language = 'P' and @Gender = 'F' and @Units = 2
	set @Letters = Substring(@Letters, 1, dbo.LastIndex('dois', @Letters) - 1) + 'duas'

-- establecemos respuesta de la funcion
return @Letters
end
GO