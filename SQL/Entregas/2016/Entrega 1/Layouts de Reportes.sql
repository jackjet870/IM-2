/*
* Palace Resorts
** Grupo de Desarrollo Palace
**
** Script de layouts de reportes
** 
** [wtorres]	02/Jul/2016 Created
**
**/
use OrigosVCPalace

declare @Report varchar(50)
-- =============================================
-- I. LAYOUTS DE REPORTES
--		1. Layout del reporte para el pago de depositos a PRs

-- =============================================
--				LAYOUTS DE REPORTES
-- =============================================
-- Layout del reporte para el pago de depositos a PRs
-- =============================================
set @Report = 'rptDepositsPaymentByPR'
delete from FieldsByReport where frReport = @Report
exec USP_OR_AddFieldReport @Report, 'Category', 'Category', NULL, 1500, 2
exec USP_OR_AddFieldReport @Report, 'pcN', 'Payment Place', NULL, 1200, 2
exec USP_OR_AddFieldReport @Report, 'PaymentSchema', 'Payment Schema', NULL, 1500, 2
exec USP_OR_AddFieldReport @Report, 'PR', 'PR ID', NULL, 900
exec USP_OR_AddFieldReport @Report, 'PRN', 'PR Name', NULL, 2500
exec USP_OR_AddFieldReport @Report, 'Books', 'Books', 'Bookings', 700, 1, NULL, 'ejSum'
exec USP_OR_AddFieldReport @Report, 'InOuts', 'IO', 'In & Outs', 1300, 1, NULL, 'ejSum'
exec USP_OR_AddFieldReport @Report, 'GrossBooks', 'Total Book', 'Total Bookings', 900, 1, NULL, 'ejSum'
exec USP_OR_AddFieldReport @Report, 'ShowsFactor', 'Sh%', 'Shows Factor', 500, 1, '0%', 'ejFactor{GrossShows{GrossBooks'
exec USP_OR_AddFieldReport @Report, 'GrossShows', 'Total Shows', 'Total Shows', 1000, 1, NULL, 'ejSum'
exec USP_OR_AddFieldReport @Report, 'SalesAmount', 'Sales Amount', NULL, 1200
exec USP_OR_AddFieldReport @Report, 'Efficiency', 'Efficiency', 'Efficiency', 1000
exec USP_OR_AddFieldReport @Report, 'guID', 'GUID', 'Guest ID', 700, 1, '#'
exec USP_OR_AddFieldReport @Report, 'guName', 'Guest Name', NULL, 2000
exec USP_OR_AddFieldReport @Report, 'guBookD', 'Book Date', 'Booking Date',1000
exec USP_OR_AddFieldReport @Report, 'guOutInvitNum', 'Out. Inv.', 'Outside Invitation', 800
exec USP_OR_AddFieldReport @Report, 'guls', 'LS', 'Lead Source', 1000
exec USP_OR_AddFieldReport @Report, 'gusr', 'SR', 'Sales Room', 700
exec USP_OR_AddFieldReport @Report, 'guHotel', 'Hotel', NULL, 2000
exec USP_OR_AddFieldReport @Report, 'Deposited', 'Deposit', NULL, 800, 1, NULL, 'ejSum'
exec USP_OR_AddFieldReport @Report, 'Received', 'Received', NULL, 800, 1, NULL, 'ejSum'
exec USP_OR_AddFieldReport @Report, 'ToPay', 'To Pay', NULL, 800, 1, NULL, 'ejSum'