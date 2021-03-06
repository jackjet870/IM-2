USE [OrigosVCPalace]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte Semanal y Mensual de Hostess
** 
** [edgrodriguez]	20/May/2016 Created
**
*/
CREATE PROCEDURE [dbo].[USP_IM_RptWeeklyMonthlyHostess]
	@DateFrom as DateTime,			-- Fecha desde
	@DateTo as DateTime,			-- Fecha hasta
	@SalesRooms as varchar(8000)	-- Claves de las salas de ventas
as
set nocount on

Declare 
	@dtmFirsDate as datetime, --Primer dia del mes
	@dtmLastDate as datetime --Ultimo dia del mes
	set @dtmFirsDate = dateadd(d, day(@DateFrom) * (-1) + 1, @DateFrom)
	set @dtmLastDate = dateadd(m, 1, @dtmFirsDate) - 1
	-----------------------------------------------------------------------
	--Weekly & Monthly Report (Directs, InOut, WalkOuts
	-----------------------------------------------------------------------
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
	into #Guests 
	from 
		Guests
		inner join Personnel pe1 on guPRInvit1 = pe1.peID 
		left outer join Personnel pe2 on guPRInvit2 = pe2.peID 
		left outer join Personnel pe3 on guPRInvit3 = pe3.peID 
	where 
		(@SalesRooms = 'ALL' or gusr in (select item from Split(@SalesRooms, ',')))
		and (guShowD between @dtmFirsDate and @dtmLastDate)
	order by 
		guPRInvit1
		
SELECT [Type],guls,guloInvit,Sum(guDirect) guDirect,sum(guInOut) guInOut,sum(guWalkOut) guWalkOut, guPRInvit guPRInvit,guPRInvitN guPRInvitN,gusr  
FROM
(		
	Select 'Weekly' [Type],guls,guloInvit,Sum(Convert(int,guDirect)) guDirect,sum(Convert(int,guInOut)) guInOut,sum(Convert(int,guWalkOut)) guWalkOut, guPRInvit1 guPRInvit,guPRInvit1N guPRInvitN,gusr 
	from #Guests
	WHERE
	guPRInvit1 IS NOT NULL
	and (guShowD between @DateFrom and @DateTo)
	GROUP by guPRInvit1,guPRInvit1n,guls,guloInvit,guShowD,gusr
union ALL
	Select 'Weekly' [Type],guls,guloInvit,Sum(Convert(int,guDirect)) guDirect,sum(Convert(int,guInOut)) guInOut,sum(Convert(int,guWalkOut)) guWalkOut, guPRInvit2 guPRInvit,guPRInvit2N guPRInvitN,gusr 
	from #Guests
	WHERE
	guPRInvit2 IS NOT NULL
	and (guShowD between @DateFrom and @DateTo)
	GROUP by guPRInvit2,guPRInvit2n,guls,guloInvit,guShowD,gusr
union all
	Select 'Weekly' [Type],guls,guloInvit,Sum(Convert(int,guDirect)) guDirect,sum(Convert(int,guInOut)) guInOut,sum(Convert(int,guWalkOut)) guWalkOut, guPRInvit3 guPRInvit,guPRInvit3N guPRInvitN,gusr 
	from #Guests
	WHERE
	guPRInvit3 IS NOT NULL
	and (guShowD between @DateFrom and @DateTo)
	GROUP by guPRInvit3,guPRInvit3n,guls,guloInvit,guShowD,gusr
union all
	Select 'Monthly' [Type],guls,guloInvit,Sum(Convert(int,guDirect)) guDirect,sum(Convert(int,guInOut)) guInOut,sum(Convert(int,guWalkOut)) guWalkOut, guPRInvit1 guPRInvit,guPRInvit1N guPRInvitN,gusr 
	from #Guests
	WHERE
	guPRInvit1 IS NOT NULL
	GROUP by guPRInvit1,guPRInvit1n,guls,guloInvit,guShowD,gusr
union ALL
	Select 'Monthly' [Type],guls,guloInvit,Sum(Convert(int,guDirect)) guDirect,sum(Convert(int,guInOut)) guInOut,sum(Convert(int,guWalkOut)) guWalkOut, guPRInvit2 guPRInvit,guPRInvit2N guPRInvitN,gusr 
	from #Guests
	WHERE
	guPRInvit2 IS NOT NULL
	GROUP by guPRInvit2,guPRInvit2n,guls,guloInvit,guShowD,gusr
union all
	Select 'Monthly' [Type],guls,guloInvit,Sum(Convert(int,guDirect)) guDirect,sum(Convert(int,guInOut)) guInOut,sum(Convert(int,guWalkOut)) guWalkOut, guPRInvit3 guPRInvit,guPRInvit3N guPRInvitN,gusr 
	from #Guests
	WHERE
	guPRInvit3 IS NOT NULL
	GROUP by guPRInvit3,guPRInvit3n,guls,guloInvit,guShowD,gusr
) Gu
group by GU.guPRInvit,GU.guPRInvitN,GU.guls,[Type],GU.gusr, GU.guloInvit
order by Gu.guls

	-----------------------------------------------------------------------
	--Weekly Times (Bookings, Shows, Directs)
	-----------------------------------------------------------------------

	-- Bookings
	select 
		guls,
		guloInvit,
		guBookD,
		guDirect, 
		case
		 when CONVERT(VARCHAR(8),guBookT,108) <= '09:29:59' then '08:30'
		 when CONVERT(VARCHAR(8),guBookT,108) between '09:30:00' and '10:29:59' then '09:30'
		 when CONVERT(VARCHAR(8),guBookT,108) between '10:30:00' and '11:29:59' then '10:30'
		 when CONVERT(VARCHAR(8),guBookT,108) between '11:30:00' and '12:29:59' then '11:30'
		 when CONVERT(VARCHAR(8),guBookT,108) >= '12:30:00' then '12:30'
		 else NULL end guBookT
		into #Bookings 
	FROM 
		Guests  INNER JOIN Leadsources ls ON guls = lsID
	WHERE 
		(@SalesRooms = 'ALL' or gusr in (select item from Split(@SalesRooms, ',')))
		AND guBookD BETWEEN @DateFrom AND @DateTo
		AND (lspg = 'IH' OR (lspg = 'OUT' AND guDeposit <> 0)) --No considerar invit. outside sin depósito
		AND (guReschD IS NULL OR guReschD <> guBookD) --No considere Reschedules
		AND guAntesIO = 0

	-- Shows
	SELECT 
		guls,
		guloInvit,
		guShowD,
		case
		 when CONVERT(VARCHAR(8),guTimeInT,108) <= '09:29:59' then '08:30'
		 when CONVERT(VARCHAR(8),guTimeInT,108) between '09:30:00' and '10:29:59' then '09:30'
		 when CONVERT(VARCHAR(8),guTimeInT,108) between '10:30:00' and '11:29:59' then '10:30'
		 when CONVERT(VARCHAR(8),guTimeInT,108) between '11:30:00' and '12:29:59' then '11:30'
		 when CONVERT(VARCHAR(8),guTimeInT,108) >= '12:30:00' then '12:30'
		 else NULL end guTimeInT
		into #Shows 
	FROM 
		Guests INNER JOIN Leadsources ls ON guls = lsID
	WHERE 
		(@SalesRooms = 'ALL' or gusr in (select item from Split(@SalesRooms, ',')))
		AND guShowD BETWEEN @DateFrom AND @DateTo
		AND (guTour=1 OR guInOut=1 OR guWalkOut=1)


SELECT
 guls, guD, [Time], sum(guBook) guBook, sum(guShow) guShow, sum(guDirect)guDirect
FROM
(
-- Bookings
select guls, guBookD guD, guBookT [Time], Count(guBookD) - Sum(Cast(guDirect as int)) guBook, 0 guShow, Sum(Cast(guDirect as int)) guDirect
from #Bookings
GROUP BY guls, guBookD, guBookT
-- Shows
union all
select guls, guShowD, guTimeInT, 0, Count(guShowD), 0
from #Shows
GROUP BY guls, guShowD, guTimeInT
-- Total de Bookings
UNION ALL
select guls, guBookD, 'Total', Count(guBookD) - Sum(Cast(guDirect as int)), 0, Sum(Cast(guDirect as int))
from #Bookings
GROUP BY guls, guBookD
-- Total de Shows
union all
select guls, guShowD, 'Total', 0, Count(guShowD), 0
from #Shows
GROUP BY guls, guShowD
) HostessTime 
group by  guls, guD, [Time]
Order by guls,guD,[Time]