if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetPersonnelStatistics]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetPersonnelStatistics]
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta las estadisticas de un usuario
**
** [wtorres]	25/Mar/2015	Created
**
*/
create procedure [dbo].[USP_OR_GetPersonnelStatistics]
	@User varchar(10)
as
set nocount on

select
	P.peID as Id,
	P.peN as Name,
	
	-- =============================================
	--				TABLAS OPERATIVAS
	-- =============================================
	-- Huespedes
	(select Count(*) from Guests where guPRAssign = P.peID or guPRAvail = P.peID or guPRInfo = P.peID or guPRFollow = P.peID
		or guPRNoBook = P.peID or guPRInvit1 = P.peID or guPRInvit2 = P.peID or guPRInvit3 = P.peID) as [Guests PR],
	(select Count(*) from Guests where guLiner1 = P.peID or guLiner2 = P.peID) as [Guests Liner],
	(select Count(*) from Guests where guCloser1 = P.peID or guCloser2 = P.peID or guCloser3 = P.peID) as [Guests Closer],
	(select Count(*) from Guests where guExit1 = P.peID or guExit2 = P.peID) as [Guests Exit],
	(select Count(*) from Guests where guPodium = P.peID) as [Guests Podium],
	(select Count(*) from Guests where guVLO = P.peID) as [Guests VLO],
	(select Count(*) from Guests where guEntryHost = P.peID or guGiftsHost = P.peID or guExitHost = P.peID) as [Guests Host],
	(select Count(*) from Guests where guPRCaptain1 = P.peID or guPRCaptain2 = P.peID or guPRCaptain3 = P.peID or guLinerCaptain1 = P.peID
		or guCloserCaptain1 = P.peID) as [Guests Captain],
	(select Count(*) from ShowsSalesmen where shpe = P.peID) as [Shows Salesmen],
	(select Count(*) from GuestLog where glChangedBy = P.peID or glPRAvail = P.peID or glPRInfo = P.peID or glPRFollow = P.peID
		or glPRInvit1 = P.peID or glPRInvit2 = P.peID or glLiner1 = P.peID or glLiner2 = P.peID or glCloser1 = P.peID or glCloser2 = P.peID
		or glCloser3 = P.peID or glExit1 = P.peID or glExit2 = P.peID or glPodium = P.peID or glVLO = P.peID) as [Guests Log],
	(select Count(*) from GuestsMovements where gmpe = P.peID) as [Guests Movements],

	-- Ventas
	(select Count(*) from Sales where saPR1 = P.peID or saPR2 = P.peID or saPR3 = P.peID) as [Sales PR],
	(select Count(*) from Sales where saLiner1 = P.peID or saLiner2 = P.peID) as [Sales Liner],
	(select Count(*) from Sales where saCloser1 = P.peID or saCloser2 = P.peID or saCloser3 = P.peID) as [Sales Closer],
	(select Count(*) from Sales where saExit1 = P.peID or saExit2 = P.peID) as [Sales Exit],
	(select Count(*) from Sales where saPodium = P.peID) as [Sales Podium],
	(select Count(*) from Sales where saVLO = P.peID) as [Sales VLO],
	(select Count(*) from Sales where saPRCaptain1 = P.peID or saPRCaptain2 = P.peID or saPRCaptain3 = P.peID or saLinerCaptain1 = P.peID
		or saCloserCaptain1 = P.peID) as [Sales Captain],
	(select Count(*) from SalesSalesmen where smpe = P.peID) as [Sales Salesmen],
	(select Count(*) from SalesLog where slChangedBy = P.peID or slPR1 = P.peID or slPR2 = P.peID or slPR3 = P.peID or slPRCaptain1 = P.peID
		or slPRCaptain2 = P.peID or slLiner1 = P.peID or slLiner2 = P.peID or slLinerCaptain1 = P.peID or slCloser1 = P.peID
		or slCloser2 = P.peID or slCloser3 = P.peID or slExit1 = P.peID or slExit2 = P.peID or slCloserCaptain1 = P.peID or slPodium = P.peID
		or slVLO = P.peID) as [Sales Log],

	-- Recibos de regalos
	(select Count(*) from GiftsReceipts where grAuthorizedBy = P.peID) as [Gifts Receipts Authorized],
	(select Count(*) from GiftsReceipts where grHost = P.peID) as [Gifts Receipts Host],
	(select Count(*) from GiftsReceipts where grpe = P.peID) as [Gifts Receipts PR],
	(select Count(*) from GiftsReceiptsPayments where gype = P.peID) as [Gifts Receipts Payments],
	(select Count(*) from GiftsReceiptsLog where goAuthorizedBy = P.peID or goChangedBy = P.peID or goHost = P.peID or gope = P.peID)
		as [Gifts Receipts Log],

	-- Cupones de comida
	(select Count(*) from MealTickets where mepe = P.peID) as [Meal Tickets],

	-- Tipos de cambio
	(select Count(*) from ExchangeRateLog where elChangedBy = P.peID) as [Exchange Rate Log],

	-- Movimientos al inventario
	(select Count(*) from WhsMovs where wmpe = P.peID) as [Warehouse Movements],

	-- Notas de PR
	(select Count(*) from PRNotes where pnPR = P.peID) as [PR Notes],

	-- Sesiones
	(select Count(*) from LoginsLog where llpe = P.peID) as [Logins Log],

	-- Asistencia
	(select Count(*) from Assistance where aspe = P.peID) as Assistance,
	(select Count(*) from DaysOff where dope = P.peID) as [Days Off],

	-- Eficiencia
	(select Count(*) from EfficiencySalesmen where espe = P.peID) as [Efficiency Salesmen],

	-- =============================================
	--				TABLAS DE CATALOGOS
	-- =============================================
	-- Personal
	(select Count(*) from PersLSSR where plpe = P.peID) as [Places Access],
	(select Count(*) from PersonnelRoles where prpe = P.peID) as Roles,
	(select Count(*) from PersonnelPermissions where pppe = P.peID) as Permissions,
	(select Count(*) from Personnel where peLinerID = P.peID) as [Personnel Liner],
	(select Count(*) from PostsLog where ppChangedBy = P.peID or pppe = P.peID) as [Posts Log],
	(select Count(*) from TeamsLog where tlChangedBy = P.peID or tlpe = P.peID) as [Teams Log],
	(select Count(*) from TeamsGuestServices where tgLeader = P.peID) as [Teams Guest Services],
	(select Count(*) from TeamsSalesmen where tsLeader = P.peID) as [Teams Salesmen],

	-- Regalos
	(select Count(*) from GiftsLog where ggChangedBy = P.peID) as [Gifts Log],

	-- Lead Sources
	(select Count(*) from LeadSources where lsBoss = P.peID) as [Lead Sources],

	-- Salas de ventas
	(select Count(*) from SalesRooms where srBoss = P.peID) as [Sales Rooms],
	(select Count(*) from SalesRoomsLog where sqChangedBy = P.peID) as [Sales Rooms Log],

	-- Configuraciones
	(select Count(*) from osConfig where ocAdminUser = P.peID or ocBoss = P.peID) as [Configurations]
from Personnel P
where P.peID = @User
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

