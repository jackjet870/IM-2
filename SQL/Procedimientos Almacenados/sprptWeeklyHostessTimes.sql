if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprptWeeklyHostessTimes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprptWeeklyHostessTimes]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  StoredProcedure [dbo].[sprptWeeklyHostessTimes]    Script Date: 05/22/2007 10:34:08 ******/
CREATE procedure [dbo].[sprptWeeklyHostessTimes]
	@DateFrom datetime,
	@DateTo datetime,
	@SR0 varchar(10),
	@SR1 varchar(10) = '',
	@SR2 varchar(10) = '',
	@SR3 varchar(10) = '',
	@SR4 varchar(10) = '',
	@SR5 varchar(10) = '',
	@SR6 varchar(10) = '',
	@SR7 varchar(10) = '',
	@SR8 varchar(10) = '',
	@SR9 varchar(10) = ''
	as
	set nocount on
	
	select 
		guls,
		guloInvit,
		guBookD,
		guShowD,
		guDirect, 
		guBookT,
		guTimeInT,
		guAntesIO --20070206 HC Antes IO no se contabiliza como booking. 
	into #t1
--20070521 HC Igual a Manifiesto de Host
/*	from 
		Guests 
	where 
		(gusr = @SR0 
		or gusr = @SR1 or gusr = @SR2 
		or gusr = @SR3 or gusr = @SR4 
		or gusr = @SR5 or gusr = @SR6 
		or gusr = @SR7 or gusr = @SR8 
		or gusr = @SR9) 
		and (guShowD between @DateFrom and @DateTo
		or guBookD between @DateFrom and @DateTo)
*/	
	FROM 
		Guests INNER JOIN Leadsources ON guls = lsID
	WHERE 
		(gusr = @SR0 
		OR gusr = @SR1 OR gusr = @SR2 
		OR gusr = @SR3 OR gusr = @SR4 
		OR gusr = @SR5 OR gusr = @SR6 
		OR gusr = @SR7 OR gusr = @SR8 
		OR gusr = @SR9) 
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
		(gusr = @SR0 
		OR gusr = @SR1 OR gusr = @SR2 
		OR gusr = @SR3 OR gusr = @SR4 
		OR gusr = @SR5 OR gusr = @SR6 
		OR gusr = @SR7 OR gusr = @SR8 
		OR gusr = @SR9) 
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

