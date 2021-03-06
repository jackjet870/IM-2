if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprptWeeklyHostessTimesGS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprptWeeklyHostessTimesGS]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  StoredProcedure [dbo].[sprptWeeklyHostessTimesGS]    Script Date: 05/22/2007 13:26:57 ******/
--Punto 52, requerimiento de Palace, Golf y Sunrise se reportan juntos.
create PROCEDURE [dbo].[sprptWeeklyHostessTimesGS]
	@DateFrom DATETIME,
	@DateTo DATETIME
	AS
	SET NOCOUNT ON
	
	SELECT 
		guls,
		guloInvit,
		guBookD,
		guShowD,
		guDirect, 
		guBookT,
		guTimeInT,
		guAntesIO 
	INTO #t1
	FROM 
		Guests INNER JOIN Leadsources ON guls = lsID
	WHERE 
		(gusr = 'MPS' OR gusr = 'GMP') 
		AND (guShowD BETWEEN @DateFrom AND @DateTo
		OR guBookD BETWEEN @DateFrom AND @DateTo)
		AND (lspg = 'IH' OR (lspg = 'OUT' AND guDeposit <> 0)) AND guDirect = 0 --No considerar invit. outside sin depósito
		AND (guReschD IS NULL OR guReschD <> guBookD) --No considere Reschedules

	SELECT 
		guls,
		guloInvit,
		guBookD,
		guShowD,
		guDirect, 
		guBookT,
		guTimeInT,
		guAntesIO 
	INTO #t2
	FROM 
		Guests 
	WHERE 
		(gusr = 'MPS' OR gusr = 'GMP') 
		AND (guShowD BETWEEN @DateFrom AND @DateTo
		OR guBookD BETWEEN @DateFrom AND @DateTo)
		AND (guTour=1 OR guInOut=1 OR guWalkOut=1) --Contabilización de Shows Reales (Código en Host)

--20070521 HC Igual a Manifiesto de Host END

	select * from #t1 order by guloInvit, guBookD

	SELECT * FROM #t2 ORDER BY guloInvit, guBookD

	select distinct guloInvit, guls from #t1 order by guloInvit
	
	drop table #t1
	
	DROP TABLE #t2
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

