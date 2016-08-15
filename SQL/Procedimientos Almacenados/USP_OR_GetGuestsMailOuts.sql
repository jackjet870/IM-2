if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetGuestsMailOuts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetGuestsMailOuts]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene los Mail Outs disponibles de un Lead Source
**
** [aalcocer]	24/Feb/2011 Created
** [wtorres]	12/Ago/2016 Modified. Documentado y actualizado a como estaba en la BD. Ahora ya no esta repetido el campo guPickUpT
**
*/
CREATE PROCEDURE [dbo].[USP_OR_GetGuestsMailOuts]
	@guls as varchar(10),		-- Clave del Lead Source
	@guCheckInD as datetime,	-- Fecha de llegada
	@guCheckOutD as datetime,	-- Fecha de salida
	@guBookD as datetime		-- Fecha de booking
AS 
SET NOCOUNT ON

SELECT CASE
	WHEN guCheckIn = 0 THEN 8  --Sin Check-In
	WHEN guCheckOutD < GETDATE() THEN 7 --Check Out
	WHEN guAvail = 0 THEN 6 --Invitacion
	WHEN guInvit=1 AND guBookCanc=1 THEN 5 --Inv. Cancelada
	WHEN guInvit=1 AND guBookD <  GETDATE() AND guShow=0 THEN 4 -- Inv. No Show
	WHEN guInvit=1 AND guShow=1 THEN 3 --Inv. Show
	WHEN guInvit=1 THEN 2 -- Invitado en stand-by
	WHEN guInfo=1 THEN 1 -- Info
	WHEN guAvail=1 THEN 0 --Disponible
	END AS guStatus,  
	guBookT, guPickUpT, guRoomNum, guLastName1, guFirstName1, guPax, 
	guCheckIn, guCheckInD, guCheckOutD, guco, coN, guag, agN, guInfo, guInvit, guBookCanc, 
	guInfoD, guBookD, guPRInvit1, peN, gumo, gula, gumoA, guComments 
FROM Guests 
	LEFT JOIN Personnel ON guPRInvit1 = peID 
	LEFT JOIN Agencies ON guag = agID 
	LEFT JOIN Countries ON guco = coID 
WHERE
	-- Lead Source
	guls = @guls
	-- Disponible
	AND ((guAvail = 1
	-- Fecha de llegada menor a igual a hoy
	AND (guCheckInD <= @guCheckInD
	-- Fecha de salida mayor a mañana
	AND guCheckOutD > @guCheckOutD)
	-- Sin show
	AND guShow = 0
	-- No invitado o Invitado con Fecha de booking menor a pasado mañana
	AND (guInvit = 0 OR (guInvit = 1 AND guBookD < @guBookD)))
	-- Con mail out
	OR (gumo IS NOT NULL AND gumo <> 'NOMAIL'))
order by guCheckIn, guRoomNum