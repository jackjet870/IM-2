if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetGuestsMailOuts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetGuestsMailOuts]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene los Mail Outs disponibles de un Lead Source
**
** [aalcocer]	24/Feb/2011 Created
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
guBookT, guPickUpT, guPickUpT, guRoomNum, guLastName1, guFirstName1, guPax, 
guCheckIn, guCheckInD, guCheckOutD, guco, coN, guag, agN, guInfo, guInvit, guBookCanc, 
guInfoD, guBookD, guPRInvit1, peN, gumo, gula, gumoA, guComments 
FROM Guests 
LEFT JOIN Personnel ON guPRInvit1 = peID 
LEFT JOIN Agencies ON guag = agID 
LEFT JOIN Countries ON guco = coID 
WHERE guls = @guls AND 
(
	(
		guAvail = 1 AND
		(guCheckInD <= @guCheckInD AND guCheckOutD > @guCheckOutD) AND 
		guShow = 0 AND 
		(guInvit = 0 OR (guInvit = 1 AND guBookD < @guBookD)
	)
) OR 
(gumo IS NOT NULL AND gumo <> 'NOMAIL'))
order by guCheckIn, guRoomNum

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO