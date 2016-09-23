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
--		2. Layout del reporte de manifiesto por rango de fechas
--		3. Layout del reporte de cupones de comida

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

-- =============================================
-- Layout del reporte de manifiesto por rango de fechas
-- =============================================
set @Report = 'rptManifestRange'
delete from FieldsByReport where frReport = @Report
exec USP_OR_AddFieldReport @Report, 'DateManifest', 'Date', 'Date Manifest', 960
exec USP_OR_AddFieldReport @Report, 'SaleType', NULL, NULL, 10, 3
exec USP_OR_AddFieldReport @Report, 'SaleTypeN', 'Group', NULL, 2000
exec USP_OR_AddFieldReport @Report, 'sagu', 'GUID', 'Guest ID', 700, 1, '#'
exec USP_OR_AddFieldReport @Report, 'SalesRoom', 'SR', 'Sales Room', 500
exec USP_OR_AddFieldReport @Report, 'Location', 'Loc', 'Location', 500
exec USP_OR_AddFieldReport @Report, 'Hotel', 'Hotel', NULL, 1400
exec USP_OR_AddFieldReport @Report, 'Room', 'Room', 'Room Number', 660
exec USP_OR_AddFieldReport @Report, 'Pax', 'Pax', NULL, 375, 2
exec USP_OR_AddFieldReport @Report, 'LastName', 'Last Name', NULL, 1545
exec USP_OR_AddFieldReport @Report, 'FirstName', 'First Name', NULL, 1305
exec USP_OR_AddFieldReport @Report, 'Agency', 'Agency ID', 'Agency ID', 1000, 2
exec USP_OR_AddFieldReport @Report, 'AgencyN', 'Agency', 'Agency Name', 1500
exec USP_OR_AddFieldReport @Report, 'Country', 'Country ID', 'Country ID', 500, 2
exec USP_OR_AddFieldReport @Report, 'CountryN', 'Country', 'Country Name', 1000
exec USP_OR_AddFieldReport @Report, 'ShowD', 'Show D', NULL, 960
exec USP_OR_AddFieldReport @Report, 'TimeInT', 'T. In', 'Time In', 620
exec USP_OR_AddFieldReport @Report, 'TimeOutT', 'T. Out', 'Time Out', 620
exec USP_OR_AddFieldReport @Report, 'CheckIn', 'Chk-In D', 'Check-In Date', 960
exec USP_OR_AddFieldReport @Report, 'CheckOut', 'Chk-Out D', 'Check-Out Date', 960
exec USP_OR_AddFieldReport @Report, 'Direct', 'D', 'Direct', 380, 1, NULL, 'ejSum'
exec USP_OR_AddFieldReport @Report, 'Tour', 'Tr', 'Tour', 380, 1, NULL, 'ejSum'
exec USP_OR_AddFieldReport @Report, 'IO', 'IO', 'In & Out', 380, 1, NULL, 'ejSum'
exec USP_OR_AddFieldReport @Report, 'WO', 'WO', 'Walk Out', 380, 1, NULL, 'ejSum'
exec USP_OR_AddFieldReport @Report, 'CT', 'CT', 'Courtesy Tour', 380, 1, NULL, 'ejSum'
exec USP_OR_AddFieldReport @Report, 'SaveTour', 'Sve', 'Save Tour', 380, 1, NULL, 'ejSum'
exec USP_OR_AddFieldReport @Report, 'PR1', 'PR 1', 'PR who made the booking', 800, 2
exec USP_OR_AddFieldReport @Report, 'PR1N', 'PR 1 Name', 'PR who made the booking', 1000
exec USP_OR_AddFieldReport @Report, 'PR2', 'PR 2', 'PR who made the booking', 800, 2
exec USP_OR_AddFieldReport @Report, 'PR2N', 'PR 2 Name', 'PR who made the booking', 1000, 2
exec USP_OR_AddFieldReport @Report, 'PR3', 'PR 3', 'PR who made the booking', 800, 2
exec USP_OR_AddFieldReport @Report, 'PR3N', 'PR 3 Name', 'PR who made the booking', 1000, 2
exec USP_OR_AddFieldReport @Report, 'Liner1', 'Liner 1', NULL, 800, 2
exec USP_OR_AddFieldReport @Report, 'Liner1N', 'Liner 1 Name', 'PR who made the booking', 1000
exec USP_OR_AddFieldReport @Report, 'Liner2', 'Liner 2', NULL, 800, 2
exec USP_OR_AddFieldReport @Report, 'Liner2N', 'Liner 2  Name', 'PR who made the booking', 1000, 2
exec USP_OR_AddFieldReport @Report, 'Closer1', 'Closer 1', NULL, 800, 2
exec USP_OR_AddFieldReport @Report, 'Closer1N', 'Closer 1 Name', NULL, 1000
exec USP_OR_AddFieldReport @Report, 'Closer2', 'Closer 2', NULL, 800, 2
exec USP_OR_AddFieldReport @Report, 'Closer2N', 'Closer 2 Name', NULL, 1000, 2
exec USP_OR_AddFieldReport @Report, 'Closer3', 'Closer 3', NULL, 800, 2
exec USP_OR_AddFieldReport @Report, 'Closer3N', 'Closer 3 Name', NULL, 1000, 2
exec USP_OR_AddFieldReport @Report, 'Exit1', 'Exit 1', NULL, 800, 2
exec USP_OR_AddFieldReport @Report, 'Exit1N', 'Exit 1 Name', NULL, 1000
exec USP_OR_AddFieldReport @Report, 'Exit2', 'Exit 2', NULL, 800, 2
exec USP_OR_AddFieldReport @Report, 'Exit2N', 'Exit 2 Name', NULL, 1000, 2
exec USP_OR_AddFieldReport @Report, 'Hostess', 'Host', 'Host(ess)', 700
exec USP_OR_AddFieldReport @Report, 'ProcSales', NULL, 'Processable Sales', 255, 1, NULL, 'ejSum'
exec USP_OR_AddFieldReport @Report, 'ProcOriginal', 'Proc Orig', 'Processable Original Amount', 1000, 1, NULL, 'ejSum'
exec USP_OR_AddFieldReport @Report, 'ProcNew', 'Proc New', 'Processable New Amount', 1000, 1, NULL, 'ejSum'
exec USP_OR_AddFieldReport @Report, 'ProcGross', 'Proc Gross', 'Processable Gross Amount', 1000, 1, NULL, 'ejSum'
exec USP_OR_AddFieldReport @Report, 'PendSales', NULL, 'Pending  Sales', 255, 1, NULL, 'ejSum'
exec USP_OR_AddFieldReport @Report, 'PendOriginal', 'Pend. Orig.', 'Pending Original Amount', 1000, 1, NULL, 'ejSum'
exec USP_OR_AddFieldReport @Report, 'PendNew', 'Pend. New', 'Pending New Amount', 1000, 1, NULL, 'ejSum'
exec USP_OR_AddFieldReport @Report, 'PendGross', 'Pend. Gross', 'Pending Gross Amount', 1000, 1, NULL, 'ejSum'
exec USP_OR_AddFieldReport @Report, 'ClosingCost', 'C. Cost', 'Closing Cost', 900, 1, NULL, 'ejSum'
exec USP_OR_AddFieldReport @Report, 'Membership', 'Memb. #', 'Membership Number', 850
exec USP_OR_AddFieldReport @Report, 'Comments', 'Comments', NULL, 3600

-- =============================================
-- Layout del reporte de cupones de comida
-- =============================================
set @Report = 'rptMealTickets'
delete from FieldsByReport where frReport = @Report
exec USP_OR_AddFieldReport @Report, 'meID', 'No.', NULL, 700, 1, '#', NULL, 6
exec USP_OR_AddFieldReport @Report, 'meD', 'Date', NULL, 960
exec USP_OR_AddFieldReport @Report, 'megu', 'Guest ID', NULL, 700, 1, '#'
exec USP_OR_AddFieldReport @Report, 'guLastName1', 'Guest Name', NULL, 1800
exec USP_OR_AddFieldReport @Report, 'meQty', 'Qty', 'Quantity', 375, 1, NULL, 'ejSum'
exec USP_OR_AddFieldReport @Report, 'myN', 'Type', 'Ticket Type', 1100
exec USP_OR_AddFieldReport @Report, 'meFolios', 'Folios', NULL, 1000
exec USP_OR_AddFieldReport @Report, 'meAdults', 'Adults', NULL, 500, 1, NULL, 'ejSum'
exec USP_OR_AddFieldReport @Report, 'meMinors', 'Minors', NULL, 500, 1, NULL, 'ejSum'
exec USP_OR_AddFieldReport @Report, 'guShow', 'Show', NULL, 500
exec USP_OR_AddFieldReport @Report, 'Total', 'Total', NULL, 700, 1, NULL, 'ejSum'
exec USP_OR_AddFieldReport @Report, 'guloInfo', 'Loc', 'Location', 1100
exec USP_OR_AddFieldReport @Report, 'guPRInvit1', 'PR', 'PR ID', 700
exec USP_OR_AddFieldReport @Report, 'guPRInvit1N', 'PR Name', NULL, 2500
exec USP_OR_AddFieldReport @Report, 'guEntryHost', 'Host', NULL, 700
exec USP_OR_AddFieldReport @Report, 'guEntryHostN', 'Host Name', NULL, 2500
exec USP_OR_AddFieldReport @Report, 'guLiner1', 'Liner', 'Liner ID', 700
exec USP_OR_AddFieldReport @Report, 'guLiner1N', 'Liner Name', NULL, 2500
exec USP_OR_AddFieldReport @Report, 'meComments', 'Comments', NULL, 1800
exec USP_OR_AddFieldReport @Report, 'mera', '', NULL, 1800,3
exec USP_OR_AddFieldReport @Report, 'RateTypeN', 'Rate Type', NULL, 3500
exec USP_OR_AddFieldReport @Report, 'mepe', 'Personnel ID', NULL, 1000
exec USP_OR_AddFieldReport @Report, 'peN', 'Personnel Name', NULL, 2600
exec USP_OR_AddFieldReport @Report, 'peCollaboratorID', '# Collaborator', NULL, 1200
exec USP_OR_AddFieldReport @Report, 'agN', 'Agency', NULL, 1800
exec USP_OR_AddFieldReport @Report, 'merep', 'Representative', NULL, 3500