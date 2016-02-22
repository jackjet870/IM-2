/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Textos de los meses
** 
** [wtorres]	12/Sep/2014 Created
**
*/
use OrigosVCPalace

declare @Report varchar(50)
set @Report = 'Months'
delete from ReportsTexts where reReport = @Report
exec USP_OR_AddReportText @Report, 'January', 'Enero', 'January'
exec USP_OR_AddReportText @Report, 'February', 'Febrero', 'February'
exec USP_OR_AddReportText @Report, 'March', 'Marzo', 'March'
exec USP_OR_AddReportText @Report, 'April', 'Abril', 'April'
exec USP_OR_AddReportText @Report, 'May', 'Mayo', 'May'
exec USP_OR_AddReportText @Report, 'June', 'Junio', 'June'
exec USP_OR_AddReportText @Report, 'July', 'Julio', 'July'
exec USP_OR_AddReportText @Report, 'August', 'Agosto', 'August'
exec USP_OR_AddReportText @Report, 'September', 'Septiembre', 'September'
exec USP_OR_AddReportText @Report, 'October', 'Octubre', 'October'
exec USP_OR_AddReportText @Report, 'November', 'Noviembre', 'November'
exec USP_OR_AddReportText @Report, 'December', 'Diciembre', 'December'