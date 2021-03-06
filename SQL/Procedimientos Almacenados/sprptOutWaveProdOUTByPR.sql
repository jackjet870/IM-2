if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprptOutWaveProdOUTByPR]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprptOutWaveProdOUTByPR]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por horario
** 
** [wtorres]	22/Nov/2013 Ahora los downgrades se cuentan en base a la categoria de tipo de venta
**
*/
create procedure [dbo].[sprptOutWaveProdOUTByPR]
 	@DateFrom as DateTime,	-- Fecha desde
	@DateTo as DateTime,	-- Fecha hasta
	@PRs as varchar(8000)	-- Claves de PRs
as
set nocount on

-- Huespedes
-- =============================================
select
	G.gusr,
	G.guID,
	G.guRef,
	G.guInfo,
	G.guInfoD,
	G.guInvit,
	G.guInvitD,
	G.guPRInfo,
	G.guBookD,
	G.guBookT,
	G.guInOut,
	G.guShow,
	G.guShowD,
	G.guAntesIO, 
	G.guDeposit
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- Programa
	L.lspg = 'OUT'
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Fecha de booking
	and (G.guBookD between @DateFrom and @DateTo
	-- Fecha de show
	or G.guShowD between @DateFrom and @DateTo)
order by G.guBookT, G.gusr

-- Ventas
-- =============================================
select 
	S.sagu,
	S.sasr,
	G.guBookT,
	S.saD,
	S.saProc,
	S.saProcD,
	S.saCancel,
	S.saCancelD,
	S.saGrossAmount,
	S.samt,
	ST.ststc
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	inner join LeadSources L on L.lsID = S.sals
	left join Guests G on G.guID = S.sagu
where
	-- Programa
	L.lspg = 'OUT'
	-- Fecha de cancelacion
    and ((S.saCancelD between @DateFrom and @DateTo
	-- Fecha de procesable
    and S.saProcD between @DateFrom and @DateTo)
    -- Fecha de venta
	or ((S.saD between @DateFrom and @DateTo and S.saProc = 0)
	-- Fecha de procesable
	or (S.saProcD between @DateFrom and @DateTo and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)
	or (S.saProcD between @DateFrom and @DateTo and ST.ststc <> 'DG')))
	-- PRs
	and (@PRs = 'ALL' or (S.saPR1 in (select item from split(@PRs, ','))
		or S.saPR2 in (select item from split(@PRs, ','))
		or S.saPR3 in (select item from split(@PRs, ','))))
order by G.guBookT, S.sasr

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

