if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprptMonthlyHostessGS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprptMonthlyHostessGS]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

--Punto 52, requerimiento de Palace, Golf y Sunrise se reportan juntos.
CREATE PROCEDURE [dbo].[sprptMonthlyHostessGS]
	@StartDate datetime,
	@EndDate datetime
	AS
	SET NOCOUNT ON
	DECLARE 
	@dtmFirsDate AS DATETIME, --Primer dia del mes
	@dtmLastDate AS DATETIME --Ultimo dia del mes
	SET @dtmFirsDate = DATEADD(d, DAY(@StartDate) * (-1) + 1, @StartDate)
	SET @dtmLastDate = DATEADD(m, 1, @dtmFirsDate) - 1
	PRINT CAST(@dtmFirsDate AS VARCHAR(20)) + ' Primer dia'
	PRINT CAST(@dtmLastDate AS VARCHAR(20)) + 'ultimo dia'
	SELECT
		guls,
		guloInvit,
		guDirect,
		guPRInvit1,
		guPRInvit2,
		guPRInvit3,
		pe1.peN AS guPRInvit1N, 
		pe2.peN AS guPRInvit2N, 
		pe3.peN AS guPRInvit3N, 
		guShowD,
		guInOut,
		guWalkOut,
		----------
		'MPSyGolf' AS gusr,
		----------
		guTimeInT 
	FROM 
		Guests 
		INNER JOIN Personnel pe1 ON guPRInvit1 = pe1.peID 
		LEFT OUTER JOIN Personnel pe2 ON guPRInvit2 = pe2.peID 
		LEFT OUTER JOIN Personnel pe3 ON guPRInvit3 = pe3.peID 
	WHERE 
		(gusr = 'MPS' OR gusr = 'GMP') 
		AND (guShowD BETWEEN @StartDate AND @EndDate) --semana 
	ORDER BY 
		guPRInvit1
	
	SELECT 
		guls,
		guloInvit, 
		guDirect,
		guPRInvit1,
		guPRInvit2,
		guPRInvit3,
		pe1.peN AS guPRInvit1N, 
		pe2.peN AS guPRInvit2N, 
		pe3.peN AS guPRInvit3N, 
		guShowD,
		guInOut,
		guWalkOut,
		----------
		'MPSyGolf' AS gusr,
		----------
		guTimeInT
	FROM 
		Guests 
		INNER JOIN Personnel pe1 ON guPRInvit1 = pe1.peID 
		LEFT OUTER JOIN Personnel pe2 ON guPRInvit2 = pe2.peID 
		LEFT OUTER JOIN Personnel pe3 ON guPRInvit3 = pe3.peID 
	WHERE 
		(gusr = 'MPS' OR gusr = 'GMP') 
		AND ( guShowD BETWEEN @dtmFirsDate AND @dtmLastDate) --mes
	ORDER BY 
		guPRInvit1
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

