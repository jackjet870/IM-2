if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetAgencyDifferenceInvitationBookingDaysDifference]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetAgencyDifferenceInvitationBookingDaysDifference]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de dias de diferencia entre la fecha de invitacion y la fecha de booking por agencia y dias de diferencia
** 
** [lchairez]	20/Oct/2011 Creado
**
*/
CREATE FUNCTION [dbo].[UFN_OR_GetAgencyDifferenceInvitationBookingDaysDifference]
(	
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL'	-- Claves de Lead Sources
)

RETURNS @Table TABLE (
	Agency VARCHAR(25),
	DaysDifference INT,
	[Difference] INT
)

AS
BEGIN

INSERT @Table
SELECT
	D.guag,
	D.DaysDifference,
	Sum(D.DaysDifference)
FROM (
	SELECT
		G.guag,
		DateDiff(Day, G.guInvitD, G.guBookD) AS DaysDifference
	FROM Guests G
	WHERE
		-- Fecha de booking
		G.guBookD BETWEEN @DateFrom AND @DateTo
		-- Lead Sources
		AND (@LeadSources = 'ALL' OR G.guls IN (SELECT item FROM split(@LeadSources, ','))) 
		-- No bookings cancelados
		AND G.guBookCanc = 0
) AS D
GROUP BY D.guag, D.DaysDifference

RETURN
END

GO