USE [OrigosVCPalace]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_IM_RptUpList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_IM_RptUpList]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte UpListStart o UpListEnd del modulo Host.
** 
** [edgrodriguez]	27/Jun/2016 Created
**
*/
CREATE PROCEDURE [dbo].[USP_IM_RptUpList]
	@DateFrom datetime,			-- Fecha desde
	@SalesRooms varchar(max),	-- Claves de las salas de ventas
	@UpListType int = 0			-- 0. UpListStart
								-- 1. UpListEnd
as
set nocount on
set FMTONLY OFF
declare @FirstMonthDate datetime, @PreviosDay datetime;

--Obtenemos el dia anterior.
set @PreviosDay = DATEADD(DAY, -1,@DateFrom)
--Si la fecha del dia anterior tiene un mes diferente
if(Month(@PreviosDay) = Month(@DateFrom) - 1)
	--Obtenemos el primer dia del mes.
	set @FirstMonthDate = dateadd(d, day(@PreviosDay) * (-1) + 1, @PreviosDay);
else
	--Obtenemos el primer dia del mes anterior.
	set @FirstMonthDate = dateadd(d, day(@DateFrom) * (-1) + 1, @DateFrom);


--------------------------------------------------------------------------
--		Ventas por Salesman
--------------------------------------------------------------------------
SELECT
	S.Salesman, 
	Sum(S.AmountYtd) AmountYtd,
	Sum(S.AmountM) AmountM,
	S.LastPost
	into #SalesmanAmount 
FROM (
	select Liner Salesman, 0 AmountYtd, SalesAmount AmountM, LastPost from dbo.UFN_IM_GetLinerSalesAmount(@FirstMonthDate,@PreviosDay,@SalesRooms,default,default,default,default,'LINER,FTM,FTB,CLOSER,REGEN',0, default)
	union all
	select Liner, SalesAmount, 0, LastPost from dbo.UFN_IM_GetLinerSalesAmount(@PreviosDay,@PreviosDay,@SalesRooms,default,default,default,default,'LINER,FTM,FTB,CLOSER,REGEN', 0,default)
	union all
	select Closer Salesman, 0 , SalesAmount, LastPost from dbo.UFN_IM_GetCloserSalesAmount(@FirstMonthDate,@PreviosDay,@SalesRooms,default,default,default,'LINER,FTB', 0, 'CLOSER')
	union all
	select Closer, SalesAmount, 0, LastPost from dbo.UFN_IM_GetCloserSalesAmount(@PreviosDay,@PreviosDay,@SalesRooms,default,default,default,'LINER,FTB', 0, 'CLOSER')
	union all
	select Closer Salesman, 0 , SalesAmount, LastPost from dbo.UFN_IM_GetCloserSalesAmount(@FirstMonthDate,@PreviosDay,@SalesRooms,default,default,default,'LINER,FTB', 0, 'EXIT')
	union all
	select Closer, SalesAmount, 0, LastPost from dbo.UFN_IM_GetCloserSalesAmount(@PreviosDay,@PreviosDay,@SalesRooms,default,default,default,'LINER,FTB', 0, 'EXIT')
	) as S
group by S.Salesman, S.LastPost

---------------------------------------------------------------
--		Uplist Start
---------------------------------------------------------------

-- Vendedores que tuvieron ventas
Select 
	S.Salesman,
	Po.SalesmanN,
	SalesmanPost = CASE S.LastPost
		when 'CLOSER' then UPPER(S.LastPost)
		when 'NP' then 'NP'
		else 'FTB' end,
	SalesmanPostN= CASE S.LastPost
		when 'CLOSER' then (SELECT UPPER(poN) FROM dbo.Posts WHERE poID=S.LastPost)
		when 'NP' then 'NP'
		ELSE 'FRONT TO BACKS' end,
	Po.DayOffList,
	'' [Language],
	'' Location,
	Cast(NULL as DATETIME) [Time],
	'' [TimeN], 
	S.AmountYtd,
	S.AmountM
into #UpListStart 
from #SalesmanAmount S
	left join
	(
			select 
			peID Salesman, 
			peN SalesmanN,			
			ISNULL(doList,'') DayOffList
		from Personnel
			left join dbo.Posts on pepo=poID
			left join dbo.DaysOff on dope=peID
	) as Po on S.Salesman = Po.Salesman

-- Vendedores que NO tuvieron ventas
UNION ALL
select 
	peID Salesman, 
	peN SalesmanN,
	SalesmanPost = CASE pepo
			when 'CLOSER' then upper(pepo)
			else 'FTB' end,
	SalesmanPostN= CASE pepo
			when 'CLOSER' then upper(poN) 
			ELSE 'FRONT TO BACKS' end,
	ISNULL(doList,'') DayOffList,
	'' [Language],
	'' Location,
	Cast(NULL as DATETIME) [Time],
	'' [TimeN], 
	CAST(0 as Money),
	Cast(0 as Money)
from dbo.Personnel
	left join dbo.Posts on pepo=poID
	left join dbo.DaysOff on dope=peID
where 
	pepo IN ('CLOSER' ,'REGEN','FTM','FTB','LINER') and
	peID in (
			select 
				peID
			from Personnel
			where
				-- Tipo de equipo
				peTeamType = 'SA'--@TeamType
				-- Clave del lugar
				and pePlaceID = @SalesRooms--@Place
				-- Personal activo
				and peA = 1
			)
	and peID not in(
			SELECT S.Salesman from #SalesmanAmount S		
			)
Order by SalesmanPost, AmountYtd DESC, AmountM DESC, Salesman

IF @UpListType = 0 Begin
	SELECT * FROM #UpListStart
End

IF @UpListType = 1 Begin
	---------------------------------------------------------------
	--	UplistEnd
	---------------------------------------------------------------
	SELECT 
		S.Salesman,
		S.SalesmanN,
		S.SalesmanPost,
		S.SalesmanPostN,
		S.DayOffList, 
		G.Language,
		G.Location,
		G.Time, 
		TimeN= CASE 
			WHEN G.Time IS NULL THEN ''
			WHEN CONVERT(VARCHAR(8),G.Time,108) <= '09:59:59' THEN '08:00'
			WHEN CONVERT(VARCHAR(8),G.Time,108) BETWEEN '10:00:00' AND '11:59:59' THEN '10:00'
			ELSE
			'12:00' end,
		S.AmountYtd, 
		S.AmountM FROM #UpListStart S
	OUTER APPLY
	(
		select 
			G.gula [Language],
			G.guloInvit [Location],
			G.guTimeInT [Time]
		from Guests G	
			-- Segment and Location	
			left join Agencies A on A.agID = G.guag
			left join SegmentsByAgency SA on SA.seID = A.agse
			left join Locations LO on LO.loID = G.guloInvit
			left join LocationsCategories LC on LC.lcID = LO.lolc
			left join LeadSources LS on LS.lsID = G.guls
		where
			-- Fecha de show
			G.guShowD between @DateFrom and @DateFrom
			--Si es Tour o WalkOut
			and (guTour = 1 or guWalkOut = 1)
			-- Sala de ventas
			and G.gusr = @SalesRooms
			and (
				-- Rol de PR
					(S.Salesman = G.guPRInvit1 or S.Salesman = G.guPRInvit2 or S.Salesman = G.guPRInvit3)
				-- Rol de Liner
				or (S.Salesman = G.guLiner1 or S.Salesman = G.guLiner2)
				-- Rol de Closer
				or (S.Salesman = G.guCloser1 or S.Salesman = G.guCloser2 or S.Salesman = G.guCloser3)
				-- Rol de Exit
				or (S.Salesman = G.guExit1 or S.Salesman = G.guExit2))
	) AS G
	Order by S.SalesmanPost, S.AmountYtd DESC, S.AmountM DESC, S.Salesman, G.Time
END