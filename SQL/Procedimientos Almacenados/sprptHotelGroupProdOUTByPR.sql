if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprptHotelGroupProdOUTByPR]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprptHotelGroupProdOUTByPR]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por grupo hotelero
** 
** [wtorres]	22/Nov/2013 Ahora los downgrades se cuentan en base a la categoria de tipo de venta
** [wtorres]	16/Ene/2014 Ahora se devuelve la descripcion del grupo hotelero, debido a que cree el catalogo de grupos hoteleros
**
*/
create procedure [dbo].[sprptHotelGroupProdOUTByPR]
 	@DateFrom as DateTime,	-- Fecha desde
	@DateTo as DateTime,	-- Fecha hasta
	@PRs as varchar(8000)	-- Claves de PRs
as
set nocount on

-- Huespedes
-- =============================================
select
	G.guHotel,
	G.gusr,
	HG.hgN as hoGroup,
	G.guID,
	G.guRef,
	G.guInfo,
	G.guInfoD,
	G.guInvit,
	G.guInvitD,
	G.guPRInfo,
	G.guBookD,
	G.guInOut,
	G.guShow,
	G.guShowD,
	G.guAntesIO,
	G.guDeposit
from Guests G
	inner join Hotels H on H.hoID = G.guHotel
	left join HotelGroups HG on HG.hgID = H.hoGroup
	inner join LeadSources L on L.lsID = g.guls
where
	-- Programa
	L.lspg = 'OUT'
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Fecha de contactacion
	and (G.guInfoD between @DateFrom and @DateTo
	-- Fecha de invitacion
	or G.guInvitD between @DateFrom and @DateTo
	-- Fecha de booking
	or ((G.guBookD between @DateFrom and @DateTo
	-- Fecha de show
	or G.guShowD between @DateFrom and @DateTo) and G.guDeposit > 0)
	or G.guShowD between @DateFrom and @DateTo and G.guDeposit = 0)
order by G.gusr, HG.hgN, G.guHotel

-- Ventas
-- =============================================
select 
	S.sagu,
	S.sasr,
	G.guHotel,
	HG.hgN as hoGroup,
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
	left join Hotels H on H.hoID = G.guHotel
	left join HotelGroups HG on HG.hgID = H.hoGroup
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
order by S.sasr, HG.hgN, G.guHotel

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

