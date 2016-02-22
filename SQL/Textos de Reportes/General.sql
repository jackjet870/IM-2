/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Textos generales de reportes
** 
** [wtorres]	12/Sep/2014 Created
**
*/
use OrigosVCPalace

declare @Report varchar(50)
set @Report = 'General'
delete from ReportsTexts where reReport = @Report
-- Textos
exec USP_OR_AddReportText @Report, 'Of', 'de', 'of'