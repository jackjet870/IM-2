if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptShowFactorByBookingDate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptShowFactorByBookingDate]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de porcentaje de show por fecha de booking
** 
** [wtorres]	19/Oct/2011 Creado
** [wtorres]	28/Dic/2011 Agregue el campo descripcion de la agencia
** [alesanchez]	02/Sep/2013 aumentaron los parámetros de la función UFN_OR_GetAgencyShows
** [lchairez]	18/Feb/2013 Se agrega columna para agrupar por categoría.
*/
create procedure [dbo].[USP_OR_RptShowFactorByBookingDate]
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0	-- Indica si se debe considerar quinielas
as
set nocount on

	SELECT 
		-- Agencia
		D.Agency,
		ISNULL(A.agN, 'Not Specified') AS AgencyN,
		-- Dias de diferencia entre fecha de invitacion y fecha de booking
		D.DaysDifference,
		SUM(D.[Difference]) AS [Difference],
		-- Bookings
		SUM(D.Books) AS Books,
		-- Shows
		SUM(D.Shows) AS Shows,
		-- Porcentaje de shows
		dbo.UFN_OR_SecureDivision(SUM(D.Shows), SUM(D.Books)) AS ShowsFactor,
		-- In & Outs
		SUM(D.InOuts) AS InOuts,
		-- Walk Outs
		SUM(D.WalkOuts) AS WalkOuts
	INTO #tmp
	FROM (
		-- Dias de diferencia entre fecha de invitacion y fecha de booking
		SELECT Agency, DaysDifference, [Difference] , 0 AS Books, 0 AS Shows, 0 AS InOuts, 0 AS WalkOuts
		FROM UFN_OR_GetAgencyDifferenceInvitationBookingDaysDifference(@DateFrom, @DateTo, @LeadSources)
		-- Bookings
		UNION ALL
		SELECT Agency, DaysDifference, 0, Books,  0, 0, 0
		FROM UFN_OR_GetAgencyDifferenceInvitationBookingBookings(@DateFrom, @DateTo, @LeadSources, DEFAULT, DEFAULT, @ConsiderQuinellas, DEFAULT, 0, DEFAULT)
		-- Shows
		UNION ALL
		SELECT Agency, DaysDifference, 0, 0, Shows, 0, 0
		FROM UFN_OR_GetAgencyDifferenceInvitationBookingShows(@DateFrom, @DateTo, @LeadSources, DEFAULT, DEFAULT, @ConsiderQuinellas, DEFAULT, 0, 0, DEFAULT, DEFAULT)
		-- In & Outs
		UNION ALL
		SELECT Agency, DaysDifference, 0, 0, 0, Shows AS InOut, 0
		FROM UFN_OR_GetAgencyDifferenceInvitationBookingShows(@DateFrom, @DateTo, @LeadSources, DEFAULT, DEFAULT, @ConsiderQuinellas, DEFAULT, 1, DEFAULT, DEFAULT, DEFAULT)
		-- Walk Outs
		UNION ALL
		SELECT Agency, DaysDifference, 0, 0, 0, 0, Shows AS WalkOuts
		FROM UFN_OR_GetAgencyDifferenceInvitationBookingShows(@DateFrom, @DateTo, @LeadSources, DEFAULT, DEFAULT, @ConsiderQuinellas, DEFAULT, DEFAULT, 1, DEFAULT, DEFAULT)
	) AS D
		LEFT JOIN Agencies A ON D.Agency = A.agID
	GROUP BY D.Agency, A.agN, D.DaysDifference
	ORDER BY Shows DESC, Books DESC, Agency


	SELECT *,  DaysDifference [Group]
	FROM #tmp
	WHERE DaysDifference = 0

	UNION ALL

	SELECT *,  DaysDifference [Group]
	FROM #tmp
	WHERE DaysDifference = 1

	UNION ALL

	SELECT *,  DaysDifference [Group]
	FROM #tmp
	WHERE DaysDifference = 2

	UNION ALL

	SELECT *,  DaysDifference [Group]
	FROM #tmp
	WHERE DaysDifference = 3

	UNION ALL

	SELECT *,  DaysDifference [Group]
	FROM #tmp
	WHERE DaysDifference = 4

	UNION ALL

	SELECT *,  DaysDifference [Group]
	FROM #tmp
	WHERE DaysDifference >= 5

	DROP TABLE #tmp

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

