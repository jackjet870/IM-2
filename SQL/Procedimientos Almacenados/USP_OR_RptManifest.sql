if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptManifest]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptManifest]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de manifiesto (Processor Sales)
**		1. Manifiesto
**		2. Deposit Sales
**		3. Ventas de otros dias, Be Backs, OOP, Cancellations, Regens, Deposit Before, etc.
** 
** [wtorres]	06/Dic/2008 Created
** [wtorres]	12/Dic/2009 Modified. Ajuste por el renombramiento de la tabla de segmentos por agencia.
**							Ahora contempla los segmentos por Lead Source. Elimine el campo guag
** [wtorres]	21/Dic/2009 Modified. Agregue el parametro @BySegmentsCategories
** [wtorres]	22/Ene/2010 Modified. Agregue el parametro @ByLocationsCategories
** [wtorres]	25/Sep/2012 Modified. Agregue los campos de tour de cortesia y tour de rescate
** [wtorres]	16/Nov/2013 Modified. Agregue los campos de categoria de tipo de venta y grupo de tipo de membresia
** [wtorres]	20/Feb/2015 Modified. Correccion de los filtros por rol. No estaba devolviendo las ventas de los vendedores cuyos shows
**							tenian otro vendedor
** [wtorres]	09/Jun/2015 Modified. Agregue el campo saSelfGen
**
*/
create procedure [dbo].[USP_OR_RptManifest]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@SalesRoom varchar(10),				-- Clave de la sala
	@SalesmanID varchar(10) = 'ALL',	-- Clave de un vendedor
	@SalesmanRoles varchar(20) = 'ALL',	-- Roles del vendedor (PR, Liner, Closer, Exit)
	@Segments varchar(8000) = 'ALL',	-- Claves de segmentos
	@Programs varchar(8000) = 'ALL',	-- Programs
	@BySegmentsCategories bit = 0,		-- Indica si es por categorias de segmentos
	@ByLocationsCategories bit = 0		-- Indica si es por categorias de locaciones
AS
	SET NOCOUNT ON;

-- 1. Manifiesto
select 
	-- Campos de la tabla de huespedes
	G.guID,
	G.guLastName1,
	G.guFirstName1,
	G.guShowD,
	G.gusr,	
	G.guTour,
	G.guWalkOut,
	G.guCTour,
	G.guSaveProgram,
	G.guDepSale,
	G.guSelfGen,
	G.guts,
	-- Segmento
	IsNull(case when @BySegmentsCategories = 0 then (case when LS.lspg = 'IH' then A.agse else LS.lsso end)
		else (case when LS.lspg = 'IH' then SA.sesc else SL.sosc end) end, 'NS') as Segment,
	IsNull(case when @BySegmentsCategories = 0 then (case when LS.lspg = 'IH' then SA.seN else SL.soN end)
		else (case when LS.lspg = 'IH' then SCA.scN else SCL.scN end) end, 'NO SEGMENT') as SegmentN,
	-- Locacion
	case when @ByLocationsCategories = 0 then LO.loN else IsNull(LC.lcN, 'NO LOCATION CATEGORY') end as loN,
	G.guOverflow,
	-- =============================================
	--				PERSONAL DEL SHOW
	-- =============================================
	-- Guest Services
	-- PR 1
	G.guPRInvit1 as guPR1,
	GP1.peN as guPR1N,
	-- PR2
	G.guPRInvit2 as guPR2,
	GP2.peN as guPR2N,
	-- PR3
	G.guPRInvit3 as guPR3,
	GP3.peN as guPR3N,
	-- Vendedores
	-- Liner 1
	G.guLiner1 as guLiner1,
	GL1.peN as guLiner1N,
	-- Liner 2
	G.guLiner2 as guLiner2,
	GL2.peN as guLiner2N,
	-- Closer 1
	G.guCloser1 as guCloser1,
	GC1.peN as guCloser1N,
	-- Closer 2
	G.guCloser2 as guCloser2,
	GC2.peN as guCloser2N,
	-- Closer 3
	G.guCloser3 as guCloser3,
	GC3.peN as guCloser3N,
	-- Exit 1
	G.guExit1 as guExit1,
	GE1.peN as guExit1N,
	-- Exit 2
	G.guExit2 as guExit2,
	GE2.peN as guExit2N,
	-- =============================================
	--				PERSONAL DE LA VENTA
	-- =============================================
	-- Guest Services
	-- PR 1
	S.saPR1 as saPR1,
	SP1.peN as saPR1N,
	-- PR 2
	S.saPR2 as saPR2,
	SP2.peN as saPR2N,
	-- PR 3
	S.saPR3 as saPR3,
	SP3.peN as saPR3N,
	-- Vendedores
	-- Liner 1
	S.saLiner1 as saLiner1,
	SL1.peN as saLiner1N,
	-- Liner 2
	S.saLiner2 as saLiner2,
	SL2.peN as saLiner2N,
	-- Closer 1
	S.saCloser1 as saCloser1,
	SC1.peN as saCloser1N,
	-- Closer 2
	S.saCloser2 as saCloser2,
	SC2.peN as saCloser2N,
	-- Closer 3
	S.saCloser3 as saCloser3,
	SC3.peN as saCloser3N,
	-- Exit 1
	S.saExit1 as saExit1,
	SE1.peN as saExit1N,
	-- Exit 2
	S.saExit2 as saExit2,
	SE2.peN as saExit2N,
	-- Campos de la tabla de ventas	
	S.saID,
	S.saMembershipNum,
	S.sagu,
	S.saD,
	S.sast,
	ST.ststc,
	S.saProcD,
	S.saCancelD,
	S.sasr,
	S.saByPhone,
	S.samt,
	MT.mtGroup,
	S.saNewAmount,
	S.saGrossAmount,
	S.saSelfGen
from Guests G
	left join Sales S on sagu = G.guID
	left join SaleTypes ST on ST.stID = S.sast
	left join MembershipTypes MT on MT.mtID = S.samt
	-- =============================================
	--				PERSONAL DEL SHOW
	-- =============================================
	-- Guest Services
	left join Personnel GP1 on GP1.peID = G.guPRInvit1
	left join Personnel GP2 on GP2.peID = G.guPRInvit2
	left join Personnel GP3 on GP3.peID = G.guPRInvit3
	-- Vendedores
	left join Personnel GL1 on GL1.peID = G.guLiner1
	left join Personnel GL2 on GL2.peID = G.guLiner2
	left join Personnel GC1 on GC1.peID = G.guCloser1
	left join Personnel GC2 on GC2.peID = G.guCloser2
	left join Personnel GC3 on GC3.peID = G.guCloser3
	left join Personnel GE1 on GE1.peID = G.guExit1
	left join Personnel GE2 on GE2.peID = G.guExit2
	-- =============================================
	--				PERSONAL DE LA VENTA
	-- =============================================
	-- Guest Services
	left join Personnel SP1 on SP1.peID = S.saPR1
	left join Personnel SP2 on SP2.peID = S.saPR2
	left join Personnel SP3 on SP3.peID = S.saPR3
	-- Vendedores
	left join Personnel SL1 on SL1.peID = S.saLiner1
	left join Personnel SL2 on SL2.peID = S.saLiner2
	left join Personnel SC1 on SC1.peID = S.saCloser1
	left join Personnel SC2 on SC2.peID = S.saCloser2
	left join Personnel SC3 on SC3.peID = S.saCloser3
	left join Personnel SE1 on SE1.peID = S.saExit1
	left join Personnel SE2 on SE2.peID = S.saExit2
	left join Agencies A on A.agID = G.guag
	left join SegmentsByAgency SA on SA.seID = A.agse
	left join Locations LO on LO.loID = G.guloInvit
	left join LocationsCategories LC on LC.lcID = LO.lolc
	left join LeadSources LS on LS.lsID = G.guls
	left join SegmentsByLeadSource SL on SL.soID = LS.lsso
	left join SegmentsCategories SCA on SCA.scID = SA.sesc
	left join SegmentsCategories SCL on SCL.scID = SL.sosc
where
	-- Fecha de show
	G.guShowD between @DateFrom and @DateTo
	-- Sala de ventas
	and G.gusr = @SalesRoom
	-- Vendedor y rol
	and (@SalesmanID = 'ALL'
		-- Rol de PR
		or ((@SalesmanRoles = 'ALL' or 'PR' in (select item from split(@SalesmanRoles, ',')))
			and (@SalesmanID = G.guPRInvit1 or @SalesmanID = G.guPRInvit2 or @SalesmanID = G.guPRInvit3
			or @SalesmanID = S.saPR1 or @SalesmanID = S.saPR2 or @SalesmanID = S.saPR3))
		-- Rol de Liner
		or ((@SalesmanRoles = 'ALL' or 'LINER' in (select item from split(@SalesmanRoles, ',')))
			and (@SalesmanID = G.guLiner1 or @SalesmanID = G.guLiner2 or @SalesmanID = S.saLiner1 or @SalesmanID = S.saLiner2))
		-- Rol de Closer
		or ((@SalesmanRoles = 'ALL' or 'CLOSER' in (select item from split(@SalesmanRoles, ',')))
			and (@SalesmanID = G.guCloser1 or @SalesmanID = G.guCloser2 or @SalesmanID = G.guCloser3
			or @SalesmanID = S.saCloser1 or @SalesmanID = S.saCloser2 or @SalesmanID = S.saCloser3))
		-- Rol de Exit
		or ((@SalesmanRoles = 'ALL' or 'EXIT' in (select item from split(@SalesmanRoles, ',')))
			and (@SalesmanID = G.guExit1 or @SalesmanID = G.guExit2 or @SalesmanID = S.saExit1 or @SalesmanID = S.saExit2)))
	-- Segmento
	and (@Segments = 'ALL' or A.agse in (select item from split(@Segments, ',')))
	-- Programa
	and (@Programs = 'ALL' or LS.lspg in (select item from split(@Programs, ',')))
order by G.guID

-- 2. Deposit Sales
select
	-- Campos de la tabla de huespedes
	G.guID,
	G.guLastName1,
	G.guFirstName1,
	G.guShowD,
	G.gusr,	
	G.guTour,
	G.guWalkOut,
	G.guCTour,
	G.guSaveProgram,
	G.guDepSale,
	G.guSelfGen,
	G.guts,
	-- Segmento
	IsNull(case when @BySegmentsCategories = 0 then (case when LS.lspg = 'IH' then A.agse else LS.lsso end)
		else (case when LS.lspg = 'IH' then SA.sesc else SL.sosc end) end, 'NS') as Segment,
	IsNull(case when @BySegmentsCategories = 0 then (case when LS.lspg = 'IH' then SA.seN else SL.soN end)
		else (case when LS.lspg = 'IH' then SCA.scN else SCL.scN end) end, 'NO SEGMENT') as SegmentN,
	-- Locacion
	case when @ByLocationsCategories = 0 then LO.loN else IsNull(LC.lcN, 'NO LOCATION CATEGORY') end as loN,
	G.guOverflow,
	-- =============================================
	--				PERSONAL DEL SHOW
	-- =============================================
	-- Guest Services
	-- PR 1
	G.guPRInvit1 as guPR1,
	GP1.peN as guPR1N,
	-- PR2
	G.guPRInvit2 as guPR2,
	GP2.peN as guPR2N,
	-- PR3
	G.guPRInvit3 as guPR3,
	GP3.peN as guPR3N,
	-- Vendedores
	-- Liner 1
	G.guLiner1 as guLiner1,
	GL1.peN as guLiner1N,
	-- Liner 2
	G.guLiner2 as guLiner2,
	GL2.peN as guLiner2N,
	-- Closer 1
	G.guCloser1 as guCloser1,
	GC1.peN as guCloser1N,
	-- Closer 2
	G.guCloser2 as guCloser2,
	GC2.peN as guCloser2N,
	-- Closer 3
	G.guCloser3 as guCloser3,
	GC3.peN as guCloser3N,
	-- Exit 1
	G.guExit1 as guExit1,
	GE1.peN as guExit1N,
	-- Exit 2
	G.guExit2 as guExit2,
	GE2.peN as guExit2N,
	-- =============================================
	--				PERSONAL DE LA VENTA
	-- =============================================
	-- Guest Services
	-- PR 1
	S.saPR1 as saPR1,
	SP1.peN as saPR1N,
	-- PR 2
	S.saPR2 as saPR2,
	SP2.peN as saPR2N,
	-- PR 3
	S.saPR3 as saPR3,
	SP3.peN as saPR3N,
	-- Vendedores
	-- Liner 1
	S.saLiner1 as saLiner1,
	SL1.peN as saLiner1N,
	-- Liner 2
	S.saLiner2 as saLiner2,
	SL2.peN as saLiner2N,
	-- Closer 1
	S.saCloser1 as saCloser1,
	SC1.peN as saCloser1N,
	-- Closer 2
	S.saCloser2 as saCloser2,
	SC2.peN as saCloser2N,
	-- Closer 3
	S.saCloser3 as saCloser3,
	SC3.peN as saCloser3N,
	-- Exit 1
	S.saExit1 as saExit1,
	SE1.peN as saExit1N,
	-- Exit 2
	S.saExit2 as saExit2,
	SE2.peN as saExit2N,
	-- Campos de la tabla de ventas	
	S.saID,
	S.saMembershipNum,
	S.sagu,
	S.saD,
	S.sast,
	ST.ststc,
	S.saProcD,
	S.saCancelD,
	S.sasr,
	S.saByPhone,
	S.samt,
	MT.mtGroup,
	S.saNewAmount,
	S.saGrossAmount,
	S.saSelfGen
from Guests G
	left join Sales S on S.sagu = G.guID
	left join SaleTypes ST on ST.stID = S.sast
	left join MembershipTypes MT on MT.mtID = S.samt
	-- =============================================
	--				PERSONAL DEL SHOW
	-- =============================================
	-- Guest Services
	left join Personnel GP1 on GP1.peID = G.guPRInvit1
	left join Personnel GP2 on GP2.peID = G.guPRInvit2
	left join Personnel GP3 on GP3.peID = G.guPRInvit3
	-- Vendedores
	left join Personnel GL1 on GL1.peID = G.guLiner1
	left join Personnel GL2 on GL2.peID = G.guLiner2
	left join Personnel GC1 on GC1.peID = G.guCloser1
	left join Personnel GC2 on GC2.peID = G.guCloser2
	left join Personnel GC3 on GC3.peID = G.guCloser3
	left join Personnel GE1 on GE1.peID = G.guExit1
	left join Personnel GE2 on GE2.peID = G.guExit2
	-- =============================================
	--				PERSONAL DE LA VENTA
	-- =============================================
	-- Guest Services
	left join Personnel SP1 on SP1.peID = S.saPR1
	left join Personnel SP2 on SP2.peID = S.saPR2
	left join Personnel SP3 on SP3.peID = S.saPR3
	-- Vendedores
	left join Personnel SL1 on SL1.peID = S.saLiner1
	left join Personnel SL2 on SL2.peID = S.saLiner2
	left join Personnel SC1 on SC1.peID = S.saCloser1
	left join Personnel SC2 on SC2.peID = S.saCloser2
	left join Personnel SC3 on SC3.peID = S.saCloser3
	left join Personnel SE1 on SE1.peID = S.saExit1
	left join Personnel SE2 on SE2.peID = S.saExit2
	left join Agencies A on A.agID = G.guag
	left join SegmentsByAgency SA on SA.seID = A.agse
	left join Locations LO on LO.loID = G.guloInvit
	left join LocationsCategories LC on LC.lcID = LO.lolc
	left join LeadSources LS on LS.lsID = G.guls
	left join SegmentsByLeadSource SL on SL.soID = LS.lsso
	left join SegmentsCategories SCA on SCA.scID = SA.sesc
	left join SegmentsCategories SCL on SCL.scID = SL.sosc
where
	-- Fecha de venta deposito
	G.guDepositSaleD between @DateFrom and @DateTo
	-- Fecha de show diferente de la fecha de venta deposito
	and G.guShowD <> G.guDepositSaleD
	-- Sala de ventas
	and G.gusr = @SalesRoom
	-- Vendedor y rol
	and (@SalesmanID = 'ALL'
		-- Rol de PR
		or ((@SalesmanRoles = 'ALL' or 'PR' in (select item from split(@SalesmanRoles, ',')))
			and (@SalesmanID = G.guPRInvit1 or @SalesmanID = G.guPRInvit2 or @SalesmanID = G.guPRInvit3
			or @SalesmanID = S.saPR1 or @SalesmanID = S.saPR2 or @SalesmanID = S.saPR3))
		-- Rol de Liner
		or ((@SalesmanRoles = 'ALL' or 'LINER' in (select item from split(@SalesmanRoles, ',')))
			and (@SalesmanID = G.guLiner1 or @SalesmanID = G.guLiner2 or @SalesmanID = S.saLiner1 or @SalesmanID = S.saLiner2))
		-- Rol de Closer
		or ((@SalesmanRoles = 'ALL' or 'CLOSER' in (select item from split(@SalesmanRoles, ',')))
			and (@SalesmanID = G.guCloser1 or @SalesmanID = G.guCloser2 or @SalesmanID = G.guCloser3
			or @SalesmanID = S.saCloser1 or @SalesmanID = S.saCloser2 or @SalesmanID = S.saCloser3))
		-- Rol de Exit
		or ((@SalesmanRoles = 'ALL' or 'EXIT' in (select item from split(@SalesmanRoles, ',')))
			and (@SalesmanID = G.guExit1 or @SalesmanID = G.guExit2 or @SalesmanID = S.saExit1 or @SalesmanID = S.saExit2)))
	-- Segmento
	and (@Segments = 'ALL' or A.agse in (select item from split(@Segments, ',')))
	-- Programa
	and (@Programs = 'ALL' or LS.lspg in (select item from split(@Programs, ',')))
order by G.guID

-- 3. Ventas de otros dias, Be Backs, OOP, Cancellations, Regens, Deposit Before, etc.
select
	-- Campos de la tabla de invitados
	G.guID,
	G.guShowD,
	G.gusr,	
	G.guTour,
	G.guWalkOut,
	G.guCTour,
	G.guSaveProgram,
	G.guDepSale,
	G.guSelfGen,
	G.guts,
	-- Segmento
	IsNull(case when @BySegmentsCategories = 0 then (case when LS.lspg = 'IH' then A.agse else LS.lsso end)
		else (case when LS.lspg = 'IH' then SA.sesc else SL.sosc end) end, 'NS') as Segment,
	IsNull(case when @BySegmentsCategories = 0 then (case when LS.lspg = 'IH' then SA.seN else SL.soN end)
		else (case when LS.lspg = 'IH' then SCA.scN else SCL.scN end) end, 'NO SEGMENT') as SegmentN,
	-- Locacion
	case when @ByLocationsCategories = 0 then LO.loN else IsNull(LC.lcN, 'NO LOCATION CATEGORY') end as loN,
	G.guOverflow,
	-- =============================================
	--				PERSONAL DE LA VENTA
	-- =============================================
	-- Guest Services
	-- PR 1
	S.saPR1 as saPR1,
	SP1.peN as saPR1N,
	-- PR 2
	S.saPR2 as saPR2,
	SP2.peN as saPR2N,
	-- PR 3
	S.saPR3 as saPR3,
	SP3.peN as saPR3N,
	-- Vendedores
	-- Liner 1
	S.saLiner1 as saLiner1,
	SL1.peN as saLiner1N,
	-- Liner 2
	S.saLiner2 as saLiner2,
	SL2.peN as saLiner2N,
	-- Closer 1
	S.saCloser1 as saCloser1,
	SC1.peN as saCloser1N,
	-- Closer 2
	S.saCloser2 as saCloser2,
	SC2.peN as saCloser2N,
	-- Closer 3
	S.saCloser3 as saCloser3,
	SC3.peN as saCloser3N,
	-- Exit 1
	S.saExit1 as saExit1,
	SE1.peN as saExit1N,
	-- Exit 2
	S.saExit2 as saExit2,
	SE2.peN as saExit2N,
	-- Campos de la tabla de ventas
	S.saID,
	S.saLastName1,
	S.saFirstName1,
	S.saMembershipNum,
	S.sagu,
	S.saD,
	S.sast,
	ST.ststc,
	S.saProcD,
	S.saCancelD,
	S.sasr,
	S.saByPhone,
	S.samt,
	MT.mtGroup,
	S.saNewAmount,
	S.saGrossAmount,
	S.saSelfGen
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join MembershipTypes MT on MT.mtID = S.samt
	left join Guests G on G.guID = S.sagu
	-- =============================================
	--				PERSONAL DE LA VENTA
	-- =============================================
	-- Guest Services
	left join Personnel SP1 on SP1.peID = S.saPR1
	left join Personnel SP2 on SP2.peID = S.saPR2
	left join Personnel SP3 on SP3.peID = S.saPR3
	-- Vendedores
	left join Personnel SL1 on SL1.peID = S.saLiner1
	left join Personnel SL2 on SL2.peID = S.saLiner2
	left join Personnel SC1 on SC1.peID = S.saCloser1
	left join Personnel SC2 on SC2.peID = S.saCloser2
	left join Personnel SC3 on SC3.peID = S.saCloser3
	left join Personnel SE1 on SE1.peID = S.saExit1
	left join Personnel SE2 on SE2.peID = S.saExit2
	left join Agencies A on A.agID = G.guag
	left join SegmentsByAgency SA on SA.seID = A.agse
	left join Locations LO on Lo.loID = S.salo
	left join LocationsCategories LC on LC.lcID = LO.lolc
	left join LeadSources LS on LS.lsID = S.sals
	left join SegmentsByLeadSource SL on Sl.soID = LS.lsso
	left join SegmentsCategories SCA on SCA.scID = SA.sesc
	left join SegmentsCategories SCL on SCL.scID = SL.sosc
where
	-- No shows
	(((G.guShowD is null or not G.guShowD between @DateFrom and @DateTo)
	-- Fecha de venta
	and (S.saD between @DateFrom and @DateTo 
	-- Fecha de venta procesable
	or S.saProcD between @DateFrom and @DateTo
	or S.saCancelD between @DateFrom and @DateTo) ) 
	-- Incluye los Bumps, Regen y las ventas de otra sala del rango definido
	or ( (S.sast in ('BUMP', 'REGEN') or G.gusr <> S.sasr) and S.saD between @DateFrom and @DateTo) )
	-- Sala de ventas
	and S.sasr = @SalesRoom
	-- Vendedor y rol
	and (@SalesmanID = 'ALL'
		-- Rol de PR
		or ((@SalesmanRoles = 'ALL' or 'PR' in (select item from split(@SalesmanRoles, ',')))
			and (@SalesmanID = S.saPR1 or @SalesmanID = S.saPR2 or @SalesmanID = S.saPR3))
		-- Rol de Liner
		or ((@SalesmanRoles = 'ALL' or 'LINER' in (select item from split(@SalesmanRoles, ',')))
			and (@SalesmanID = S.saLiner1 or @SalesmanID = S.saLiner2))
		-- Rol de Closer
		or ((@SalesmanRoles = 'ALL' or 'CLOSER' in (select item from split(@SalesmanRoles, ',')))
			and (@SalesmanID = S.saCloser1 or @SalesmanID = S.saCloser2 or @SalesmanID = S.saCloser3))
		-- Rol de Exit
		or ((@SalesmanRoles = 'ALL' or 'EXIT' in (select item from split(@SalesmanRoles, ',')))
			and (@SalesmanID = S.saExit1 or @SalesmanID = S.saExit2)))
	-- Segmento
	and (@Segments = 'ALL' or A.agse in (select item from split(@Segments, ',')))
	-- Programa
	and (@Programs = 'ALL' or LS.lspg in (select item from split(@Programs, ',')))
order by G.guID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

