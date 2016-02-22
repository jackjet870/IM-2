/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Textos de los numeros en letras
** 
** [lormatinez]	18/Nov/2015 Created
**
*/
use Hotel

declare @Report varchar(50)
set @Report = 'Numbers'
delete from ReportsTexts where reReport = @Report
-- Unidades
exec USP_OR_AddReportText @Report, 'Zero', 'cero', 'zero', 'zero'
exec USP_OR_AddReportText @Report, 'One',  'un', 'one', 'um'
exec USP_OR_AddReportText @Report, 'Two',  'dos', 'two', 'dois'
exec USP_OR_AddReportText @Report, 'Three',  'tres', 'three', 'três'
exec USP_OR_AddReportText @Report, 'Four', 'cuatro', 'four', 'quatro'
exec USP_OR_AddReportText @Report, 'Five',  'cinco', 'five', 'cinco'
exec USP_OR_AddReportText @Report, 'Six',  'seis', 'six', 'seis'
exec USP_OR_AddReportText @Report, 'Seven', 'siete', 'seven', 'sete'
exec USP_OR_AddReportText @Report, 'Eight', 'ocho', 'eight', 'oito'
exec USP_OR_AddReportText @Report, 'Nine', 'nueve', 'nine', 'nove'
exec USP_OR_AddReportText @Report, 'Ten',  'diez', 'ten', 'dez'
exec USP_OR_AddReportText @Report, 'Eleven',  'once', 'eleven', 'onze'
exec USP_OR_AddReportText @Report, 'Twelve',  'doce', 'twelve', 'doze'
exec USP_OR_AddReportText @Report, 'Thirteen',  'trece', 'thirteen', 'treze'
exec USP_OR_AddReportText @Report, 'Fourteen',  'catorce', 'fourteen', 'catorze'
exec USP_OR_AddReportText @Report, 'Fifteen', 'quince', 'fifteen', 'quinze'
exec USP_OR_AddReportText @Report, 'Sixteen',  'dieciseis', 'sixteen', 'dezesseis'
exec USP_OR_AddReportText @Report, 'Seventeen', 'diecisiete', 'seventeen', 'dezessete'
exec USP_OR_AddReportText @Report, 'Eighteen',  'dieciocho', 'eighteen', 'dezoito'
exec USP_OR_AddReportText @Report, 'Nineteen',  'diecinueve', 'nineteen', 'dezenove'
exec USP_OR_AddReportText @Report, 'Twenty_',  'veinti', 'twenty-', 'vinte e '
-- Decenas
exec USP_OR_AddReportText @Report, 'Twenty', 'veinte', 'twenty', 'vinte'
exec USP_OR_AddReportText @Report, 'Thirty', 'treinta', 'thirty', 'trinta'
exec USP_OR_AddReportText @Report, 'Forty', 'cuarenta', 'forty', 'quarenta'
exec USP_OR_AddReportText @Report, 'Fifty', 'cincuenta', 'fifty', 'cinqüenta'
exec USP_OR_AddReportText @Report, 'Sixty',  'sesenta', 'sixty', 'sessenta'
exec USP_OR_AddReportText @Report, 'Seventy', 'setenta', 'seventy', 'setenta'
exec USP_OR_AddReportText @Report, 'Eighty',  'ochenta', 'eighty', 'oitenta'
exec USP_OR_AddReportText @Report, 'Ninety',  'noventa', 'ninety', 'noventa'
exec USP_OR_AddReportText @Report, 'Hundred', 'cien', 'hundred', 'cem'
-- Centenas
exec USP_OR_AddReportText @Report, 'OneHundred', 'ciento', 'one hundred', 'cem'
exec USP_OR_AddReportText @Report, 'TwoHundred', 'doscientos', 'two hundred', 'duzentos'
exec USP_OR_AddReportText @Report, 'ThreeHundred', 'trescientos', 'three hundred', 'trezentos'
exec USP_OR_AddReportText @Report, 'FourHundred', 'cuatrocientos', 'four hundred', 'quatrocentos'
exec USP_OR_AddReportText @Report, 'FiveHundred', 'quinientos', 'five hundred', 'quinhentos'
exec USP_OR_AddReportText @Report, 'SixHundred',  'seiscientos', 'six hundred', 'seiscentos'
exec USP_OR_AddReportText @Report, 'SevenHundred', 'setecientos', 'seven hundred', 'setecentos'
exec USP_OR_AddReportText @Report, 'EightHundred', 'ochocientos', 'eight hundred', 'oitocentos'
exec USP_OR_AddReportText @Report, 'NineHundred',  'novecientos', 'nine hundred', 'novecentos'
-- Mil, Millon
exec USP_OR_AddReportText @Report, 'Thousand', 'mil', 'thousand', 'mil'
exec USP_OR_AddReportText @Report, 'Million',  'millón', 'million', 'milhão'
exec USP_OR_AddReportText @Report, 'Millions',  'millones', 'million', 'milhão'