/*
* Palace Resorts
** Grupo de Desarrollo Palace
**
** Script de layouts de reportes
** 
** [wtorres]	10/Sep/2016 Created
**
**/
use OrigosVCPalace

declare @Report varchar(50)
-- =============================================
-- I. LAYOUTS DE REPORTES
--		1. Layout del reporte Folios Invitations Outhouse

-- =============================================
--				LAYOUTS DE REPORTES
-- =============================================
-- Layout del reporte Folios Invitations Outhouse
-- =============================================
set @Report = 'rptFoliosInvitationByDateFolio'
delete from FieldsByReport where frReport = @Report
exec USP_OR_AddFieldReport @Report, 'guOutInvitNum', 'Out. Inv.', 'Outhouse Invitation', 800
exec USP_OR_AddFieldReport @Report, 'PR', 'PR ID', NULL, 700
exec USP_OR_AddFieldReport @Report, 'PRN', 'PR Name', NULL, 3000
exec USP_OR_AddFieldReport @Report, 'guLastName1', 'Last Name', null, 1500
exec USP_OR_AddFieldReport @Report, 'guBookD', 'Book D', 'Book Date', 960
exec USP_OR_AddFieldReport @Report, 'lsN', 'Lead Source', 'Lead Source', 1500
