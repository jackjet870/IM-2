if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_UpdatePersonnelId]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_UpdatePersonnelId]
GO

SET ANSI_NULLS ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Cambia la clave de un usuario
**
** [wtorres]	26/Mar/2015	Created
**
*/
create procedure [dbo].[USP_OR_UpdatePersonnelId]
	@Old varchar(10),
	@New varchar(10)
as

-- =============================================
--				AGREGAMOS LA NUEVA CLAVE
-- =============================================

-- si no existe la nueva clave
if not exists (select top 1 null from Personnel where peID = @New) begin
	
	-- agregamos la nueva clave
	insert into Personnel (
		peID, peN, pePwd, peCaptain, peA,
		peps, pePwdD, pePwdDays, peTeamType, pePlaceID,
		peTeam, pepo, peLinerID, pede, peSalesmanID,
		peCollaboratorID)
	select @New, peN, pePwd, peCaptain, peA,
		peps, pePwdD, pePwdDays, peTeamType, pePlaceID,
		peTeam, pepo, peLinerID, pede, peSalesmanID,
		peCollaboratorID
	from Personnel where peID = @Old

	-- copiamos el perfil de la antigua clave
	
	-- Accesos a lugares
	update PersLSSR set plpe = @New where plpe = @Old

	-- Roles
	update PersonnelRoles set prpe = @New where prpe = @Old

	-- Permisos
	update PersonnelPermissions set pppe = @New where pppe = @Old

	-- Historico de puestos
	update PostsLog set pppe = @New where pppe = @Old

	-- Historico de equipos
	update TeamsLog set tlpe = @New where tlpe = @Old

-- si existe la nueva clave
end else begin
	
	-- eliminamos el perfil de la antigua clave para que no choque con el perfil de la nueva
	
	-- Accesos a lugares
	delete from PersLSSR where plpe = @Old

	-- Roles
	delete from PersonnelRoles where prpe = @Old

	-- Permisos
	delete from PersonnelPermissions where pppe = @Old
	
	-- Historico de puestos
	delete from PostsLog where pppe = @Old
	
	-- Historico de equipos
	delete from TeamsLog where tlpe = @Old
end

-- =============================================
--				TABLAS OPERATIVAS
-- =============================================
-- Huespedes
update Guests set guPRAssign = @New where guPRAssign = @Old
update Guests set guPRAvail = @New where guPRAvail = @Old
update Guests set guPRInfo = @New where guPRInfo = @Old
update Guests set guPRFollow = @New where guPRFollow = @Old
update Guests set guPRNoBook = @New where guPRNoBook = @Old
update Guests set guPRInvit1 = @New where guPRInvit1 = @Old
update Guests set guPRInvit2 = @New where guPRInvit2 = @Old
update Guests set guPRInvit3 = @New where guPRInvit3 = @Old
update Guests set guLiner1 = @New where guLiner1 = @Old
update Guests set guLiner2 = @New where guLiner2 = @Old
update Guests set guCloser1 = @New where guCloser1 = @Old
update Guests set guCloser2 = @New where guCloser2 = @Old
update Guests set guCloser3 = @New where guCloser3 = @Old
update Guests set guExit1 = @New where guExit1 = @Old
update Guests set guExit2 = @New where guExit2 = @Old
update Guests set guPodium = @New where guPodium = @Old
update Guests set guVLO = @New where guVLO = @Old
update Guests set guEntryHost = @New where guEntryHost = @Old
update Guests set guGiftsHost = @New where guGiftsHost = @Old
update Guests set guExitHost = @New where guExitHost = @Old
update Guests set guPRCaptain1 = @New where guPRCaptain1 = @Old
update Guests set guPRCaptain2 = @New where guPRCaptain2 = @Old
update Guests set guPRCaptain3 = @New where guPRCaptain3 = @Old
update Guests set guLinerCaptain1 = @New where guLinerCaptain1 = @Old
update Guests set guCloserCaptain1 = @New where guCloserCaptain1 = @Old

-- Shows de vendedores
update ShowsSalesmen set shpe = @New where shpe = @Old

-- Historico de huespedes
update GuestLog set glChangedBy = @New where glChangedBy = @Old
update GuestLog set glPRAvail = @New where glPRAvail = @Old
update GuestLog set glPRInfo = @New where glPRInfo = @Old
update GuestLog set glPRFollow = @New where glPRFollow = @Old
update GuestLog set glPRInvit1 = @New where glPRInvit1 = @Old
update GuestLog set glPRInvit2 = @New where glPRInvit2 = @Old
update GuestLog set glLiner1 = @New where glLiner1 = @Old
update GuestLog set glLiner2 = @New where glLiner2 = @Old
update GuestLog set glCloser1 = @New where glCloser1 = @Old
update GuestLog set glCloser2 = @New where glCloser2 = @Old
update GuestLog set glCloser3 = @New where glCloser3 = @Old
update GuestLog set glExit1 = @New where glExit1 = @Old
update GuestLog set glExit2 = @New where glExit2 = @Old
update GuestLog set glPodium = @New where glPodium = @Old
update GuestLog set glVLO = @New where glVLO = @Old

-- Movimientos de huespedes
update GuestsMovements set gmpe = @New where gmpe = @Old

-- Ventas
update Sales set saPR1 = @New where saPR1 = @Old
update Sales set saPR2 = @New where saPR2 = @Old
update Sales set saPR3 = @New where saPR3 = @Old
update Sales set saLiner1 = @New where saLiner1 = @Old
update Sales set saLiner2 = @New where saLiner2 = @Old
update Sales set saCloser1 = @New where saCloser1 = @Old
update Sales set saCloser2 = @New where saCloser2 = @Old
update Sales set saCloser3 = @New where saCloser3 = @Old
update Sales set saExit1 = @New where saExit1 = @Old
update Sales set saExit2 = @New where saExit2 = @Old
update Sales set saPodium = @New where saPodium = @Old
update Sales set saVLO = @New where saVLO = @Old
update Sales set saPRCaptain1 = @New where saPRCaptain1 = @Old
update Sales set saPRCaptain2 = @New where saPRCaptain2 = @Old
update Sales set saPRCaptain3 = @New where saPRCaptain3 = @Old
update Sales set saLinerCaptain1 = @New where saLinerCaptain1 = @Old
update Sales set saCloserCaptain1 = @New where saCloserCaptain1 = @Old

-- Ventas de vendedores
update SalesSalesmen set smpe = @New where smpe = @Old

-- Historico de ventas
update SalesLog set slChangedBy = @New where slChangedBy = @Old
update SalesLog set slPR1 = @New where slPR1 = @Old
update SalesLog set slPR2 = @New where slPR2 = @Old
update SalesLog set slPR3 = @New where slPR3 = @Old
update SalesLog set slLiner1 = @New where slLiner1 = @Old
update SalesLog set slLiner2 = @New where slLiner2 = @Old
update SalesLog set slCloser1 = @New where slCloser1 = @Old
update SalesLog set slCloser2 = @New where slCloser2 = @Old
update SalesLog set slCloser3 = @New where slCloser3 = @Old
update SalesLog set slExit1 = @New where slExit1 = @Old
update SalesLog set slExit2 = @New where slExit2 = @Old
update SalesLog set slPodium = @New where slPodium = @Old
update SalesLog set slVLO = @New where slVLO = @Old
update SalesLog set slPRCaptain1 = @New where slPRCaptain1 = @Old
update SalesLog set slPRCaptain2 = @New where slPRCaptain2 = @Old
update SalesLog set slLinerCaptain1 = @New where slLinerCaptain1 = @Old
update SalesLog set slCloserCaptain1 = @New where slCloserCaptain1 = @Old

-- Recibos de regalos
update GiftsReceipts set grAuthorizedBy = @New where grAuthorizedBy = @Old
update GiftsReceipts set grHost = @New where grHost = @Old
update GiftsReceipts set grpe = @New where grpe = @Old

-- Pagos de recibos de regalos
update GiftsReceiptsPayments set gype = @New where gype = @Old

-- Historico de recibos de regalos
update GiftsReceiptsLog set goAuthorizedBy = @New where goAuthorizedBy = @Old
update GiftsReceiptsLog set goChangedBy = @New where goChangedBy = @Old
update GiftsReceiptsLog set goHost = @New where goHost = @Old
update GiftsReceiptsLog set gope = @New where gope = @Old

-- Cupones de comida
update MealTickets set mepe = @New where mepe = @Old

-- Tipos de cambio
update ExchangeRateLog set elChangedBy = @New where elChangedBy = @Old

-- Movimientos al inventario
update WhsMovs set wmpe = @New where wmpe = @Old

-- Notas de PR
update PRNotes set pnPR = @New where pnPR = @Old

-- Sesiones
update LoginsLog set llpe = @New where llpe = @Old

-- Asistencia
update Assistance set aspe = @New where aspe = @Old

-- Dias de descanso
update DaysOff set dope = @New where dope = @Old

-- Eficiencia
update EfficiencySalesmen set espe = @New where espe = @Old

-- =============================================
--				TABLAS DE CATALOGOS
-- =============================================
-- Personal
update Personnel set peLinerID = @New where peLinerID = @Old

-- Historico de puestos
update PostsLog set ppChangedBy = @New where ppChangedBy = @Old

-- Historico de equipos
update TeamsLog set tlChangedBy = @New where tlChangedBy = @Old

-- Equipos de Guest Services
update TeamsGuestServices set tgLeader = @New where tgLeader = @Old

-- Equipos de vendedores
update TeamsSalesmen set tsLeader = @New where tsLeader = @Old

-- Regalos
update GiftsLog set ggChangedBy = @New where ggChangedBy = @Old

-- Lead Sources
update LeadSources set lsBoss = @New where lsBoss = @Old

-- Salas de ventas
update SalesRooms set srBoss = @New where srBoss = @Old

-- Historico de salas de ventas
update SalesRoomsLog set sqChangedBy = @New where sqChangedBy = @Old

-- Configuraciones
update osConfig set ocAdminUser = @New where ocAdminUser = @Old
update osConfig set ocBoss = @New where ocBoss = @Old

-- =============================================
--			ELIMINAMOS LA ANTIGUA CLAVE
-- =============================================
delete from Personnel where peID = @Old
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

