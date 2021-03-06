if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprptHotelGroupProd]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprptHotelGroupProd]
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
** [wtorres]	20/Nov/2013 Ahora se pasa la lista de Lead Sources como un solo parametro
**							Ahora los downgrades se cuentan en base a la categoria de tipo de venta
** [wtorres]	16/Ene/2014 Ahora se devuelve la descripcion del grupo hotelero, debido a que cree el catalogo de grupos hoteleros
**
*/
create procedure [dbo].[sprptHotelGroupProd]
 	@DateFrom as DateTime,			-- Fecha desde
	@DateTo as DateTime,			-- Fecha hasta
	@LeadSources as varchar(8000)	-- Claves de los Lead Sources
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
where
	-- Lead Sources
	G.guls in (select item from split(@LeadSources, ','))
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
	left join Guests G on G.guID = S.sagu
	left join Hotels H on H.hoID = G.guHotel
	left join HotelGroups HG on HG.hgID = H.hoGroup
where
    -- Fecha de cancelacion
    ((S.saCancelD between @DateFrom and @DateTo
    -- Fecha de procesable
	and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de venta
	or ((S.saD between @DateFrom and @DateTo and S.saProc = 0)
	or (S.saProcD between @DateFrom and @DateTo and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)
	or (S.saProcD between @DateFrom and @DateTo and ST.ststc <> 'DG')))
	-- Lead Sources
	and S.sals in (select item from split(@LeadSources, ','))
order by S.sasr, HG.hgN, G.guHotel

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

