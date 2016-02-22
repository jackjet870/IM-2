/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Textos de los dias de la semana
** 
** [wtorres]	12/Sep/2014 Created
**
*/
use OrigosVCPalace

declare @Report varchar(50)
set @Report = 'WeekDays'
delete from ReportsTexts where reReport = @Report
exec USP_OR_AddReportText @Report, 'Sunday', 'Domingo', 'Sunday'
exec USP_OR_AddReportText @Report, 'Monday', 'Lunes', 'Monday'
exec USP_OR_AddReportText @Report, 'Tuesday', 'Martes', 'Tuesday'
exec USP_OR_AddReportText @Report, 'Wednesday', 'Miercoles', 'Wednesday'
exec USP_OR_AddReportText @Report, 'Thursday', 'Jueves', 'Thursday'
exec USP_OR_AddReportText @Report, 'Friday', 'Viernes', 'Friday'
exec USP_OR_AddReportText @Report, 'Saturday', 'Sabado', 'Saturday'