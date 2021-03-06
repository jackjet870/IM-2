if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprptHotelProd]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprptHotelProd]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por hotel
** 
** [wtorres]	20/Nov/2013 Ahora se pasa la lista de Lead Sources como un solo parametro
**							Ahora los downgrades se cuentan en base a la categoria de tipo de venta
**
*/
create procedure [dbo].[sprptHotelProd]
 	@DateFrom as DateTime,			-- Fecha desde
	@DateTo as DateTime,			-- Fecha hasta
	@LeadSources as varchar(8000)	-- Claves de los Lead Sources
as
set nocount on

-- Huespedes
-- =============================================
select
	guHotel,
	gusr,
	guID,
	guRef,
	guInfo,
	guInfoD,
	guInvit,
	guInvitD,
	guPRInfo,
	guBookD,
	guInOut,
	guShow,
	guShowD,
	guAntesIO,
	guDeposit
from Guests
where
	-- Lead Sources
	guls in (select item from split(@LeadSources, ','))
	-- Fecha de contactacion
	and (guInfoD between @DateFrom and @DateTo
	-- Fecha de invitacion
	or guInvitD between @DateFrom and @DateTo
	-- Fecha de booking
	or ((guBookD between @DateFrom and @DateTo
	-- Fecha de show
	or guShowD between @DateFrom and @DateTo) and guDeposit > 0)
	or guShowD between @DateFrom and @DateTo and guDeposit = 0)
order by gusr, guHotel

-- Ventas
-- =============================================
select 
	S.sagu,
	S.sasr,
	G.guHotel,
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
	left join Guests G on G.guID = S.sagu
where
	-- Fecha de cancelacion
    ((S.saCancelD between @DateFrom and @DateTo
    -- Fecha de procesable
    and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de venta
	or ((S.saD between @DateFrom and @DateTo and S.saProc = 0)
	-- Fecha de procesable
	or (S.saProcD between @DateFrom and @DateTo and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)
	or (S.saProcD between @DateFrom and @DateTo and ST.ststc <> 'DG')))
	-- Lead Sources
	and S.sals in (select item from split(@LeadSources, ','))
order by
	S.sasr, G.guHotel

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

