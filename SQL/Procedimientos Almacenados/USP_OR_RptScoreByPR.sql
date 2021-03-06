if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptScoreByPR]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptScoreByPR]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de puntuacion por PR
**		1. Reporte
**		2. Conceptos de puntuacion
**		3. Reglas de puntuacion
**		4. Detalle de las reglas de puntuacion
**		5. Reglas de puntuacion por Lead Source
**		6. Detalle de las reglas de puntuacion por Lead Source
**		7. Tipos de reglas de puntuacion
** 
** [wtorres]	06/Dic/2010 Created
** [wtorres]	27/Dic/2010 Modified. Ahora ya no se basa en la fecha de llegada sino en la fecha de show
** [wtorres]	22/Ene/2011 Modified. Agregue el manejo de las reglas de puntuaciones por Lead Source y la regla 7 - Group of Members
** [wtorres]	03/Feb/2011 Modified. Ahora no se cuentan los In & Outs
** [wtorres]	22/Jun/2011 Modified. Agregue el uso de la funcion UFN_OR_GetGuestSales
** [wtorres]	17/Oct/2011 Modified. Ahora se toma la parte proporcional de un show para los casos de shows compartidos
** [lchairez]	20/Mar/2014	Modified. Se agrega reglas 8 (Regens), 9 (OTA 3 y 4 noches) y 10 (OTA 5 o mas noches)
** [wtorres]	23/Mar/2015 Modified. Ahora considera las ventas de huespedes adicionales
** [edgrodriguez]	05/May/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/
CREATE procedure [dbo].[USP_OR_RptScoreByPR]
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0	-- Indica si se debe considerar quinielas
as
set nocount on

-- Tabla de PRs (para ordenar)
declare @PRsToOrder table(
	PR varchar(10),
	Shows int,
	Score money
)

-- Tabla de PRs (ordenado)
declare @PRsOrdered table(
	[Order] int identity (1, 1),
	PR varchar(10)
)

-- obtenemos los shows
-- =============================================
select *
into #DataRules
from (

	-- PR 1
	-- =============================================
	select
		G.guPRInvit1 as PR,
		G.guPRInfo as PRContact,
		G.guID,
		G.guLastName1 as LastName,
		G.guls as LeadSource,
		dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) * case when @ConsiderQuinellas = 0 then 1 else G.guShowsQty end as Shows,
		Cast(0 as money) as Sales,
		-- Agencies
		case when G.gumk  = 'AGENCIES' then 1 else 0 end as ScoreRule1,
		-- Members & Exchanges
		case when G.gumk in ('MEMBERS', 'EXCHANGES') then 1 else 0 end as ScoreRule2,
		-- External
		case when L.lspg = 'IH' and G.guHReservID is null then 1 else 0 end as ScoreRule3,
		-- Rebook
		case when G.guRef is not null then 1 else 0 end as ScoreRule4,
		-- Originally Unavailable
		case when G.guOriginAvail = 0 then 1 else 0 end as ScoreRule5,
		-- 3-4 Nights
		case when DateDiff(Day, guCheckInD, guCheckOutD) between 3 and 4 then 1 else 0 end as ScoreRule6,
		-- Group of Members
		case when (G.gumk = 'MEMBERS' and G.guAvail = 0 and G.guum = 30) or G.guag = 'PR WEDD' then 1 else 0 end as ScoreRule7,
		-- Regens
		0 as ScoreRule8,
		-- OTA 3-4 nights
		case when G.gumk = 'DIRECTS' AND DateDiff(Day, guCheckInD, guCheckOutD) between 3 and 4 then 1 else 0 end as ScoreRule9,
		-- OTA 5 or more nights
		case when G.gumk = 'DIRECTS' AND DateDiff(Day, guCheckInD, guCheckOutD) >= 5 then 1 else 0 end as ScoreRule10,
		-- Regla por Lead Source
		case when G.guls in (select sbls from ScoreRulesByLeadSource where sbA = 1) then 1 else 0 end as ScoreRuleByLeadSource
	from Guests G
		inner join LeadSources L on L.lsID = G.guls
	where
		-- Fecha de show
		G.guShowD between @DateFrom and @DateTo
		-- No Quiniela Split
		and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
		-- No In & Outs
		and G.guInOut = 0
		-- Lead Sources
		and G.guls in (select item from split(@LeadSources, ','))

	-- PR 2
	-- =============================================
	union all
	select
		G.guPRInvit2,
		G.guPRInfo,
		G.guID,
		G.guLastName1,
		G.guls,
		dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) * case when @ConsiderQuinellas = 0 then 1 else G.guShowsQty end,
		0,
		-- Agencies & Directs
		case when G.gumk = 'AGENCIES' then 1 else 0 end,
		-- Members & Exchanges
		case when G.gumk in ('MEMBERS', 'EXCHANGES') then 1 else 0 end,
		-- External
		case when L.lspg = 'IH' and G.guHReservID is null then 1 else 0 end,
		-- Rebook
		case when G.guRef is not null then 1 else 0 end,
		-- Originally Unavailable
		case when G.guOriginAvail = 0 then 1 else 0 end,
		-- 3-4 Nights
		case when DateDiff(Day, guCheckInD, guCheckOutD) between 3 and 4 then 1 else 0 end,
		-- Group of Members
		case when (G.gumk = 'MEMBERS' and G.guAvail = 0 and G.guum = 30) or G.guag = 'PR WEDD' then 1 else 0 end,
		-- Regens
		0,
		-- OTA 3-4 nights
		case when G.gumk = 'DIRECTS' AND DateDiff(Day, guCheckInD, guCheckOutD) between 3 and 4 then 1 else 0 end,
		-- OTA 5 or more nights
		case when G.gumk = 'DIRECTS' AND DateDiff(Day, guCheckInD, guCheckOutD) >= 5 then 1 else 0 end,
		-- Regla por Lead Source
		case when G.guls in (select sbls from ScoreRulesByLeadSource where sbA = 1) then 1 else 0 end
	from Guests G
		inner join LeadSources L on L.lsID = G.guls
	where
		-- PR 2
		G.guPRInvit2 is not null
		-- Fecha de show
		and G.guShowD between @DateFrom and @DateTo
		-- No Quiniela Split
		and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
		-- No In & Outs
		and G.guInOut = 0
		-- Lead Sources
		and G.guls in (select item from split(@LeadSources, ','))

	-- PR 3
	-- =============================================
	union all
	select
		G.guPRInvit3,
		G.guPRInfo,
		G.guID,
		G.guLastName1,
		G.guls,
		dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) * case when @ConsiderQuinellas = 0 then 1 else G.guShowsQty end,
		0,
		-- Agencies & Directs
		case when G.gumk ='AGENCIES' then 1 else 0 end,
		-- Members & Exchanges
		case when G.gumk in ('MEMBERS', 'EXCHANGES') then 1 else 0 end,
		-- External
		case when L.lspg = 'IH' and G.guHReservID is null then 1 else 0 end,
		-- Rebook
		case when G.guRef is not null then 1 else 0 end,
		-- Originally Unavailable
		case when G.guOriginAvail = 0 then 1 else 0 end,
		-- 3-4 Nights
		case when DateDiff(Day, guCheckInD, guCheckOutD) between 3 and 4 then 1 else 0 end,
		-- Group of Members
		case when (G.gumk = 'MEMBERS' and G.guAvail = 0 and G.guum = 30) or G.guag = 'PR WEDD' then 1 else 0 end,
		-- Regens
		0,
		-- OTA 3-4 nights
		case when G.gumk = 'DIRECTS' AND DateDiff(Day, guCheckInD, guCheckOutD) between 3 and 4 then 1 else 0 end,
		-- OTA 5 or more nights
		case when G.gumk = 'DIRECTS' AND DateDiff(Day, guCheckInD, guCheckOutD) >= 5 then 1 else 0 end,
		-- Regla por Lead Source
		case when G.guls in (select sbls from ScoreRulesByLeadSource where sbA = 1) then 1 else 0 end
	from Guests G
		inner join LeadSources L on L.lsID = G.guls
	where
		-- PR 3
		G.guPRInvit3 is not null
		-- Fecha de show
		and G.guShowD between @DateFrom and @DateTo
		-- No Quiniela Split
		and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
		-- No In & Outs
		and G.guInOut = 0
		-- Lead Sources
		and G.guls in (select item from split(@LeadSources, ','))
) as DR


-- obtenemos las ventas
-- =============================================
select *
into #Sales
from (

	-- PR 1
	-- =============================================
	select
		S.saPR1 as PR,
		case when A.gaAdditional is null then S.sagu else A.gagu end as sagu,
		S.saLastName1,
		A.gaAdditional,
		GA.guLastName1,
		S.sals as LeadSource,
		S.sast,
		dbo.UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT) as Sales
	from Sales S
		left join SaleTypes ST on ST.stID = S.sast
		left join GuestsAdditional A on A.gaAdditional = S.sagu
		left join Guests GA on GA.guID = A.gagu
	where
		-- Guest ID
		S.sagu in (
			select guID from #DataRules
			union all select gaAdditional from GuestsAdditional where gagu in (select guID from #DataRules)
		)
		-- Procesable
		and S.saProc = 1
		-- No downgrades
		and ST.ststc <> 'DG'
		-- No cancelada
		and S.saCancel = 0

	-- PR 2
	-- =============================================
	union all
	select
		S.saPR2,
		case when A.gaAdditional is null then S.sagu else A.gagu end,
		S.saLastName1,
		A.gaAdditional,
		GA.guLastName1,
		S.sals,
		S.sast,
		dbo.UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT)
	from Sales S
		left join SaleTypes ST on ST.stID = S.sast
		left join GuestsAdditional A on A.gaAdditional = S.sagu
		left join Guests GA on GA.guID = A.gagu
	where
		-- PR 2
		S.saPR2 is not null
		-- Guest ID
		and S.sagu in (
			select guID from #DataRules
			union all select gaAdditional from GuestsAdditional where gagu in (select guID from #DataRules)
		)
		-- Procesable
		and S.saProc = 1
		-- No downgrades
		and ST.ststc <> 'DG'
		-- No cancelada
		and S.saCancel = 0

	-- PR 3
	-- =============================================
	union all
	select
		S.saPR3,
		case when A.gaAdditional is null then S.sagu else A.gagu end as sagu,
		S.saLastName1,
		A.gaAdditional,
		GA.guLastName1,
		S.sals,
		S.sast,
		dbo.UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT)
	from Sales S
		left join SaleTypes ST on ST.stID = S.sast
		left join GuestsAdditional A on A.gaAdditional = S.sagu
		left join Guests GA on GA.guID = A.gagu
	where
		-- PR 3
		S.saPR3 is not null
		-- Guest ID
		and S.sagu in (
			select guID from #DataRules
			union all select gaAdditional from GuestsAdditional where gagu in (select guID from #DataRules)
		)
		-- Procesable
		and S.saProc = 1
		-- No downgrades
		and ST.ststc <> 'DG'
		-- No cancelada
		and S.saCancel = 0
) as DR

-- disminuimos el numero de shows de los huespedes que hayan tenido ventas
-- =============================================
update #DataRules
set Shows = Shows - S.Sales
from #DataRules D
	inner join (
		select sagu, Sum(Sales) as Sales
		from #Sales S
		group by S.sagu
	) S on S.sagu = D.guID

-- agregamos las ventas
-- =============================================
insert into #DataRules (PR, PRContact, guID, LastName, LeadSource, Shows, Sales, ScoreRule1, ScoreRule2, ScoreRule3, ScoreRule4, ScoreRule5,
	ScoreRule6, ScoreRule7, ScoreRule8, ScoreRule9, ScoreRule10, ScoreRuleByLeadSource)
select S.PR, D.PRContact, S.sagu, S.saLastName1, S.LeadSource, 0, S.Sales, D.ScoreRule1, D.ScoreRule2, D.ScoreRule3, D.ScoreRule4, D.ScoreRule5,
	D.ScoreRule6, D.ScoreRule7,
	case when S.sast = 'REGEN' then 1 else 0 end,
	D.ScoreRule9, D.ScoreRule10, D.ScoreRuleByLeadSource
from #Sales S
	inner join #DataRules D on D.guID = S.sagu

-- eliminamos los registros que no tengan shows ni ventas
-- =============================================
delete from #DataRules where Shows = 0 and Sales = 0

-- invalidamos casi todas las reglas generales si aplica la regla 7 - Group of Members
-- =============================================
update #DataRules
set ScoreRule1 = 0,
	ScoreRule2 = 0,
	ScoreRule3 = 0,
	ScoreRule4 = 0,
	ScoreRule5 = 0,
	ScoreRule6 = 0,
	ScoreRule8 = 0,
	ScoreRule9 = 0,
	ScoreRule10 = 0
where ScoreRule7 = 1

-- invalidamos todas las reglas generales si aplica una regla por Lead Source
-- =============================================
update #DataRules
set ScoreRule1 = 0,
	ScoreRule2 = 0,
	ScoreRule3 = 0,
	ScoreRule4 = 0,
	ScoreRule5 = 0,
	ScoreRule6 = 0,
	ScoreRule7 = 0,
	ScoreRule8 = 0,
	ScoreRule9 = 0,
	ScoreRule10 = 0
where ScoreRuleByLeadSource = 1

-- Datos
-- =============================================
select
	-- Clave del PR
	D.PR as PR,
	-- Tipo de regla
	D.ScoreRuleType,
	-- Regla
	D.ScoreRule,
	-- Concepto de la regla
	D.ScoreRuleConcept,
	-- Shows
	Sum(D.Shows) as Shows,
	-- Puntuacion
	Sum(D.Score) as Score
into #Data
from (

	-- obtenemos la regla que le otorgue mas puntos al PR
	-- =============================================
	select
		DP.PR,
		DP.Shows,
		DP.ScoreRuleConcept,
		DP.Score,
		(case
			when DP.Score = 0 then null
			when DP.Score = DP.Score1 then '1'
			when DP.Score = DP.Score2 then '2'
			when DP.Score = DP.Score3 then '3'
			when DP.Score = DP.Score4 then '4'
			when DP.Score = DP.Score5 then '5'
			when DP.Score = DP.Score6 then '6'
			when DP.Score = DP.Score7 then '7'
			when DP.Score = DP.Score8 then '8'
			when DP.Score = DP.Score9 then '9'
			when DP.Score = DP.Score10 then '10'
			when DP.Score = DP.ScoreByLeadSource then DP.LeadSource
		end) as ScoreRule,
		(case
			when DP.Score = 0 then null
			when DP.Score in (DP.Score1, DP.Score2, DP.Score3, DP.Score4, DP.Score5, DP.Score6, DP.Score7, DP.Score8, DP.Score9,
				DP.Score10) then 'G'
			when DP.Score = DP.ScoreByLeadSource then 'LS'
		end) as ScoreRuleType
	from (

		-- obtenemos la puntuacion mas alta
		-- =============================================
		select
			DM.PR,
			DM.LeadSource,
			DM.Shows,
			DM.ScoreRuleConcept,
			DM.Score1,
			DM.Score2,
			DM.Score3,
			DM.Score4,
			DM.Score5,
			DM.Score6,
			DM.Score7,
			DM.Score8,
			DM.Score9,
			DM.Score10,
			DM.ScoreByLeadSource,
			dbo.UFN_OR_GetMaxScore(DM.Score1, DM.Score2, DM.Score3, DM.Score4, DM.Score5, DM.Score6, DM.Score7, DM.Score8, DM.Score9,
				DM.Score10, DM.ScoreByLeadSource) as Score
		from (

			-- obtenemos las puntuaciones de las reglas que aplican a cada huesped
			-- =============================================
			select
				DS.PR,
				DS.LeadSource,
				DS.Shows,
				DS.ScoreRuleConcept,
				-- Puntuaciones
				(case when DS.ScoreRule1 = 1 then DS.Shows * S1.siScore else 0 end) as Score1,
				(case when DS.ScoreRule2 = 1 then DS.Shows * S2.siScore else 0 end) as Score2,
				(case when DS.ScoreRule3 = 1 then DS.Shows * S3.siScore else 0 end) as Score3,
				(case when DS.ScoreRule4 = 1 then DS.Shows * S4.siScore else 0 end) as Score4,
				(case when DS.ScoreRule5 = 1 then DS.Shows * S5.siScore else 0 end) as Score5,
				(case when DS.ScoreRule6 = 1 then DS.Shows * S6.siScore else 0 end) as Score6,
				(case when DS.ScoreRule7 = 1 then DS.Shows * S7.siScore else 0 end) as Score7,
				(case when DS.ScoreRule8 = 1 then DS.Shows * S8.siScore else 0 end) as Score8,
				(case when DS.ScoreRule9 = 1 then DS.Shows * S9.siScore else 0 end) as Score9,
				(case when DS.ScoreRule10 = 1 then DS.Shows * S10.siScore else 0 end) as Score10,
				(case when DS.ScoreRuleByLeadSource = 1 then DS.Shows * SL.sjScore else 0 end) as ScoreByLeadSource
			from (
			
				-- obtenemos el numero de shows o ventas y los conceptos de las reglas que aplican a cada huesped
				-- =============================================
				select
					DR.PR,
					DR.LeadSource,
					(case when DR.Sales > 0 then DR.Sales else DR.Shows end) as Shows,
					(case
						-- Con venta
						when DR.Sales > 0 then 3
						-- Show ajeno
						when DR.PR <> DR.PRContact then 2
						-- Show propio
						else 1
					end) as ScoreRuleConcept,
					-- Reglas
					(case when R1.suA = 1 then DR.ScoreRule1 else 0 end) as ScoreRule1,
					(case when R2.suA = 1 then DR.ScoreRule2 else 0 end) as ScoreRule2,
					(case when R3.suA = 1 then DR.ScoreRule3 else 0 end) as ScoreRule3,
					(case when R4.suA = 1 then DR.ScoreRule4 else 0 end) as ScoreRule4,
					(case when R5.suA = 1 then DR.ScoreRule5 else 0 end) as ScoreRule5,
					(case when R6.suA = 1 then DR.ScoreRule6 else 0 end) as ScoreRule6,
					(case when R7.suA = 1 then DR.ScoreRule7 else 0 end) as ScoreRule7,
					(case when R8.suA = 1 then DR.ScoreRule8 else 0 end) as ScoreRule8,
					(case when R9.suA = 1 then DR.ScoreRule9 else 0 end) as ScoreRule9,
					(case when R10.suA = 1 then DR.ScoreRule10 else 0 end) as ScoreRule10,
					DR.ScoreRuleByLeadSource
				from #DataRules as DR
					left join ScoreRules R1 on R1.suID = 1
					left join ScoreRules R2 on R2.suID = 2
					left join ScoreRules R3 on R3.suID = 3
					left join ScoreRules R4 on R4.suID = 4
					left join ScoreRules R5 on R5.suID = 5
					left join ScoreRules R6 on R6.suID = 6
					left join ScoreRules R7 on R7.suID = 7
					left join ScoreRules R8 on R8.suID = 8
					left join ScoreRules R9 on R9.suID = 9
					left join ScoreRules R10 on R10.suID = 10
			) as DS
				left join ScoreRulesDetail S1 on S1.sisu = 1 and S1.sisp = DS.ScoreRuleConcept
				left join ScoreRulesDetail S2 on S2.sisu = 2 and S2.sisp = DS.ScoreRuleConcept
				left join ScoreRulesDetail S3 on S3.sisu = 3 and S3.sisp = DS.ScoreRuleConcept
				left join ScoreRulesDetail S4 on S4.sisu = 4 and S4.sisp = DS.ScoreRuleConcept
				left join ScoreRulesDetail S5 on S5.sisu = 5 and S5.sisp = DS.ScoreRuleConcept
				left join ScoreRulesDetail S6 on S6.sisu = 6 and S6.sisp = DS.ScoreRuleConcept
				left join ScoreRulesDetail S7 on S7.sisu = 7 and S7.sisp = DS.ScoreRuleConcept
				left join ScoreRulesDetail S8 on S8.sisu = 8 and S8.sisp = DS.ScoreRuleConcept
				left join ScoreRulesDetail S9 on S9.sisu = 9 and S9.sisp = DS.ScoreRuleConcept
				left join ScoreRulesDetail S10 on S10.sisu = 10 and S10.sisp = DS.ScoreRuleConcept
				left join ScoreRulesByLeadSourceDetail SL on SL.sjls = DS.LeadSource and SL.sjsp = DS.ScoreRuleConcept
		) as DM
	) as DP
) as D
group by D.PR, D.ScoreRuleType, D.ScoreRule, D.ScoreRuleConcept

-- Tabla de PRs para ordenar
-- =============================================
insert into @PRsToOrder (PR, Shows, Score)
select
	PR,
	Sum(Shows) as Shows,
	Sum(Score) as Score
from #Data
group by PR
order by Score desc, Shows desc, PR

-- Tabla de PRs ordenados
-- =============================================
insert into @PRsOrdered (PR)
select PR
from @PRsToOrder

-- Reporte
-- =============================================
select
	-- PR
	D.PR,
	-- Nombre del PR
	IsNull(P.peN, 'CODE NOT FOUND!') as PRN,
	-- Tipo de regla
	D.ScoreRuleType, T.syN as ScoreRuleTypeN,
	-- Regla
	D.ScoreRule, R.suN as ScoreRuleN,
	-- Concepto de la regla
	D.ScoreRuleConcept, C.spN as ScoreRuleConceptN,
	-- Shows
	D.Shows,
	-- Puntuacion
	D.Score
into #Report
from #Data as D
	inner join @PRsOrdered PO on PO.PR = D.PR
	left join Personnel P on P.peID = D.PR
	left join ScoreRulesTypes T on T.syID = D.ScoreRuleType
	left join ScoreRules R on R.suID = D.ScoreRule
	left join ScoreRulesConcepts C on C.spID = D.ScoreRuleConcept
order by PO.[Order]

-- 1. Reporte
-- =============================================
select * from #Report

-- 2. Conceptos de puntuacion
-- =============================================
select spID, spN from ScoreRulesConcepts

-- 3. Reglas de puntuacion
-- =============================================
select distinct suID, suN
into #ScoreRules
from #Report D
	inner join ScoreRules S on D.ScoreRule = S.suID
where D.ScoreRuleType = 'G'

select * from #ScoreRules

-- 4. Detalle de las reglas de puntuacion
-- =============================================
select Cast(sisu as varchar(10)) as sisu, sisp, siScore
from ScoreRulesDetail
where sisu in (select suID from #ScoreRules)

-- 5. Reglas de puntuacion por Lead Source
-- =============================================
select distinct lsID, lsN
into #ScoreRulesByLeadSource
from #Report D
	inner join LeadSources L on D.ScoreRule = L.lsID
where D.ScoreRuleType = 'LS'

select * from #ScoreRulesByLeadSource

-- 6. Detalle de las reglas de puntuacion por Lead Source
-- =============================================
select sjls, sjsp, sjScore
from ScoreRulesByLeadSourceDetail
where sjls in (select lsID from #ScoreRulesByLeadSource)

-- 7. Tipos de reglas de puntuacion
-- =============================================
select syID, syN
into #ScoreRulesTypes
from ScoreRulesTypes
where 1 = 2

if (select Count(*) from #ScoreRules) > 0
	insert into #ScoreRulesTypes
	select syID, syN from ScoreRulesTypes
	where syID = 'G'

if (select Count(*) from #ScoreRulesByLeadSource) > 0
	insert into #ScoreRulesTypes
	select syID, syN from ScoreRulesTypes
	where syID = 'LS'

select * from #ScoreRulesTypes