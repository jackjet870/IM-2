if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprptSelfGen]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprptSelfGen]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de Self Gen
** 
** [wtorres]	19/Nov/2013 Ahora se pasa la lista de Lead Sources como un solo parametro
**							Ahora los downgrades se cuentan en base a la categoria de tipo de venta
**
*/
create procedure [dbo].[sprptSelfGen]
	@DateFrom as DateTime,			-- Fecha desde
	@DateTo as DateTime,			-- Fecha hasta
	@LeadSources as varchar(8000)	-- Claves de los Lead Sources
as
set nocount on

-- Huespedes
-- =============================================
select 
	G.guPRInvit1,
	P.peN,
	P.peps,
	G.guShowD,
	G.guShow, 
	G.guInOut,
	G.guWalkOut
from Guests G
	left join Personnel P on P.peID = G.guPRInvit1
where
	-- Fecha de show
	G.guShowD between @DateFrom and @DateTo
	-- Lead Sources
	and G.guls in (select item from split(@LeadSources, ','))
	-- Self Gen
	and G.guSelfGen = 1
order by G.guPRInvit1 

-- Ventas
-- =============================================
select 
	S.saD, 
	S.saProcD, 
	S.saCancelD, 
	S.saGrossAmount,
	ST.ststc,
	S.saPR1, 
	P.peN, 
	P.peps 
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Personnel P on P.peID = S.saPR1
where
	-- Fecha de venta
	(S.saD between @DateFrom and @DateTo
	-- Fecha de procesable
	or S.saProcD between @DateFrom and @DateTo
	-- Fecha de cancelacion
	or S.saCancelD between @DateFrom and @DateTo)
	-- Lead Sources
	and S.sals in (select item from split(@LeadSources, ','))
	-- Self Gen
	and S.saSelfGen = 1
order by S.sapr1

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

