if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprptMonthlyHostess]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprptMonthlyHostess]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE procedure sprptMonthlyHostess
	@StartDate datetime,
	@EndDate datetime,
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
	Declare 
	@dtmFirsDate as datetime, --Primer dia del mes
	@dtmLastDate as datetime --Ultimo dia del mes
	set @dtmFirsDate = dateadd(d, day(@StartDate) * (-1) + 1, @StartDate)
	set @dtmLastDate = dateadd(m, 1, @dtmFirsDate) - 1
	print cast(@dtmFirsDate as varchar(20)) + ' Primer dia'
	print cast(@dtmLastDate as varchar(20)) + 'ultimo dia'
	select 
		guls,
		guloInvit,
		guDirect,
		guPRInvit1,
		guPRInvit2,
		guPRInvit3,
		pe1.peN as guPRInvit1N, 
		pe2.peN as guPRInvit2N, 
		pe3.peN as guPRInvit3N, 
		guShowD,
		guInOut,
		guWalkOut,
		----------
		gusr,
		----------
		guTimeInT 
	from 
		Guests 
		inner  join Personnel pe1 on guPRInvit1 = pe1.peID 
		left outer join Personnel pe2 on guPRInvit2 = pe2.peID 
		left outer join Personnel pe3 on guPRInvit3 = pe3.peID 
	where 
		(gusr = @SR0 
		or gusr = @SR1 or gusr = @SR2 
		or gusr = @SR3 or gusr = @SR4 
		or gusr = @SR5 or gusr = @SR6 
		or gusr = @SR7 or gusr = @SR8 
		or gusr = @SR9) 
		and (guShowD between @StartDate and @EndDate) --semana 
	order by 
		guPRInvit1
	
	select 
		guls,
		guloInvit, 
		guDirect,
		guPRInvit1,
		guPRInvit2,
		guPRInvit3,
		pe1.peN as guPRInvit1N, 
		pe2.peN as guPRInvit2N, 
		pe3.peN as guPRInvit3N, 
		guShowD,
		guInOut,
		guWalkOut,
		----------
		gusr,
		----------
		guTimeInT
	from 
		Guests 
		inner  join Personnel pe1 on guPRInvit1 = pe1.peID 
		left outer join Personnel pe2 on guPRInvit2 = pe2.peID 
		left outer join Personnel pe3 on guPRInvit3 = pe3.peID 
	where 
		(gusr = @SR0 
		or gusr = @SR1 or gusr = @SR2 
		or gusr = @SR3 or gusr = @SR4 
		or gusr = @SR5 or gusr = @SR6 
		or gusr = @SR7 or gusr = @SR8 
		or gusr = @SR9) 
		and ( guShowD between @dtmFirsDate and @dtmLastDate) --mes
	order by 
		guPRInvit1
	


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

