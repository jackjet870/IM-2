USE [OrigosVCPalace]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetGifts]    Script Date: 07/28/2016 14:08:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_GetGifts]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_GetGifts]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetLeadSourcesByUser]    Script Date: 07/28/2016 14:08:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_GetLeadSourcesByUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_GetLeadSourcesByUser]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetMarkets]    Script Date: 07/28/2016 14:08:19 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_GetMarkets]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_GetMarkets]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetPersonnel]    Script Date: 07/28/2016 14:08:19 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_GetPersonnel]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_GetPersonnel]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetSalesByPR]    Script Date: 07/28/2016 14:08:19 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_GetSalesByPR]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_GetSalesByPR]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetSalesRoomsByUser]    Script Date: 07/28/2016 14:08:20 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_GetSalesRoomsByUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_GetSalesRoomsByUser]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetWarehousesByUser]    Script Date: 07/28/2016 14:08:20 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_GetWarehousesByUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_GetWarehousesByUser]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetWhsMovs]    Script Date: 07/28/2016 14:08:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_GetWhsMovs]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_GetWhsMovs]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GraphProduction]    Script Date: 07/28/2016 14:08:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_GraphProduction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_GraphProduction]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GraphProductionByPR]    Script Date: 07/28/2016 14:08:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_GraphProductionByPR]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_GraphProductionByPR]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_ObtenerAsistenciasDePersonalPorSemana]    Script Date: 07/28/2016 14:08:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_ObtenerAsistenciasDePersonalPorSemana]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_ObtenerAsistenciasDePersonalPorSemana]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptCostByPR]    Script Date: 07/28/2016 14:08:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptCostByPR]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptCostByPR]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptCostByPRWithDetailGifts]    Script Date: 07/28/2016 14:08:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptCostByPRWithDetailGifts]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptCostByPRWithDetailGifts]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptDepositsPaymentByPR]    Script Date: 07/28/2016 14:08:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptDepositsPaymentByPR]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptDepositsPaymentByPR]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptFollowUpByPR]    Script Date: 07/28/2016 14:08:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptFollowUpByPR]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptFollowUpByPR]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptProductionByPRContactOutside]    Script Date: 07/28/2016 14:08:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptProductionByPRContactOutside]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptProductionByPRContactOutside]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptProductionByPRInhouse]    Script Date: 07/28/2016 14:08:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptProductionByPRInhouse]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptProductionByPRInhouse]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptProductionByPROutside]    Script Date: 07/28/2016 14:08:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptProductionByPROutside]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptProductionByPROutside]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptProductionByPRSalesRoomOutside]    Script Date: 07/28/2016 14:08:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptProductionByPRSalesRoomOutside]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptProductionByPRSalesRoomOutside]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptPRStats]    Script Date: 07/28/2016 14:08:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptPRStats]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptPRStats]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptScoreByPR]    Script Date: 07/28/2016 14:08:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptScoreByPR]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptScoreByPR]
GO

USE [OrigosVCPalace]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetGifts]    Script Date: 07/28/2016 14:08:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta el catalogo de regalos
** 
** [wtorres]	12/May/2014 Creado
** [edgrodriguez] 07/Mar/2016 Modificado --Se agregó la columna de categoria(gigc)
**
*/
create procedure [dbo].[USP_OR_GetGifts]
	@Locations varchar(8000) = 'ALL',	-- Claves de Locaciones
	@Status tinyint = 1					-- Filtro de estatus
										--		0. Sin filtro
										--		1. Activos
										--		2. Inactivos
as
set nocount on

select distinct G.giID, G.giN, G.gigc
from Gifts G
	left join GiftsByLoc L on L.glgi = G.giID
where
	-- Estatus
	(@Status = 0 or (@Status = 1 and G.giA = 1) or (@Status = 2 and G.giA = 0))
	-- Locaciones
	and (@Locations = 'ALL' or L.gllo in (select item from split(@Locations, ',')))
order by G.giN



GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetLeadSourcesByUser]    Script Date: 07/28/2016 14:08:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene los Lead Sources a los que tiene permiso un usuario
** 
** [wtorres]		07/Jun/2011 Created
** [wtorres]		12/Ago/2014 Modified. Agregue el parametro @Regions
** [edgrodriguez]	07/Mar/2016 Modified. Agregado a la consulta el campo lspg.
** [emoguel]		20/Jun/2016 Modified. Se agregó a la consulta el campo lsrg. 
**
*/
create procedure [dbo].[USP_OR_GetLeadSourcesByUser]
	@User varchar(10),				-- Clave del usuario
	@Programs varchar(8000) = 'ALL',	-- Clave de programas
	@Regions varchar(8000) = 'ALL'		-- Clave de regiones
as
set nocount on

select distinct L.lsID, L.lsN, L.lspg,L.lsrg
from PersLSSR P
	inner join LeadSources L on P.plLSSRID = L.lsID
	left join Areas A on A.arID = L.lsar
where
	-- Usuario
	P.plpe = @User
	-- Lugar de tipo Lead Source
	and P.plLSSR = 'LS'
	-- Activo
	and L.lsA = 1
	-- Programas
	and (@Programs = 'ALL' or L.lspg in (select item from split(@Programs, ',')))
	-- Regiones
	and (@Regions = 'ALL' or A.arrg in (select item from split(@Regions, ',')))
order by L.lsN



GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetMarkets]    Script Date: 07/28/2016 14:08:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta el catalogo de Markets
** 
** [erosado]	24/Febrero/2016 Created
**
*/
create procedure [dbo].[USP_OR_GetMarkets]
	@Status tinyint = 1	-- Filtro de estatus
						--		0. Sin filtro
						--		1. Activas
						--		2. Inactivas
as
set nocount on

select mkID, mkN
from Markets
where
	-- Estatus
	(@Status = 0 or (@Status = 1 and mkA = 1) or (@Status = 2 and mkA = 0))
order by mkN
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetPersonnel]    Script Date: 07/28/2016 14:08:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta el catalogo de personal
**
** [wtorres]	22/Ene/2014	Created
** [emoguel]	13/Jun/2016 Modified. Agregue el parametro @IdPersonnel
**
*/
create procedure [dbo].[USP_OR_GetPersonnel]
	@LeadSources varchar(8000) = 'ALL',		-- Claves de Lead Sources
	@SalesRooms varchar(8000) = 'ALL',		-- Claves de salas de ventas
	@Roles varchar(8000) = 'ALL',			-- Clave de rol
	@Status tinyint = 1,					-- Filtro de estatus
											--		0. Sin filtro
											--		1. Activos
											--		2. Inactivos
	@Permission varchar(10) = 'ALL',		-- Clave de permiso
	@RelationalOperator varchar(2) = '=',	-- Operador relacional
	@PermissionLevel int = 0,				-- Nivel de permiso
	@Dept varchar(10) = 'ALL',				-- Clave de departamento
	@IdPersonnel Varchar(10) ='ALL'			-- Id del personnel para devolver un unico registro
as
set nocount on

select distinct P.peID, P.peN, D.deN
from Personnel P
	left join PersLSSR PL on PL.plpe = P.peID
	left join Depts D on D.deID = P.pede
	left join PersonnelRoles PR on PR.prpe = P.peID
	left join PersonnelPermissions PP on PP.pppe = P.peID
where
	-- Lead Sources y Salas de ventas
	((@LeadSources = 'ALL' and @SalesRooms = 'ALL')
		-- Solo Lead Sources
		or (@LeadSources <> 'ALL' and @SalesRooms = 'ALL' and PL.plLSSR = 'LS' and PL.plLSSRID in (select item from split(@LeadSources, ',')))
		-- Solo Salas de ventas
		or (@LeadSources = 'ALL' and @SalesRooms <> 'ALL' and PL.plLSSR = 'SR' and PL.plLSSRID in (select item from split(@SalesRooms, ',')))
		-- Lead Sources o Salas de ventas
		or (@LeadSources <> 'ALL' and @SalesRooms <> 'ALL' and (PL.plLSSR = 'LS' and PL.plLSSRID in (select item from split(@LeadSources, ','))
		or PL.plLSSR = 'SR' and PL.plLSSRID in (select item from split(@SalesRooms, ',')))))
	-- Estatus
	and (@Status = 0 or (@Status = 1 and P.peA = 1) or (@Status = 2 and P.peA = 0))
	-- Roles
	and (@Roles = 'ALL' or PR.prro in (select item from split(@Roles, ',')))
	-- Permiso
	and (@Permission = 'ALL' or (PP.pppm = @Permission and (case @RelationalOperator
		when '=' then (case when PP.pppl = @PermissionLevel then 1 else 0 end)
		when '<>' then (case when PP.pppl <> @PermissionLevel then 1 else 0 end)
		when '>' then (case when PP.pppl > @PermissionLevel then 1 else 0 end)
		when '>=' then (case when PP.pppl >= @PermissionLevel then 1 else 0 end)
		when '<' then (case when PP.pppl < @PermissionLevel then 1 else 0 end)
		when '<=' then (case when PP.pppl <= @PermissionLevel then 1 else 0 end)
		end) = 1))
	-- Departamento
	and (@Dept = 'ALL' or P.pede = @Dept)
	AND (@IdPersonnel='ALL' OR P.peID = @IdPersonnel)
order by P.peN



GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetSalesByPR]    Script Date: 07/28/2016 14:08:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene las ventas de un PR
** 
** [wtorres]	14/Ago/2014 Created
** [wtorres]	05/Ene/2015 Modified. Desplegar los PR de la venta, no de la invitacion
** [lchairez]	19/Abr/2016 Modified. Se agrego el parametro @SearchBySalePR
** [wtorres]	26/Jul/2016 Modified. Simplifique la busqueda por PR de venta o PR de contacto
**
*/
create procedure [dbo].[USP_OR_GetSalesByPR] 
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSource varchar(10) = 'ALL',	-- Clave del Lead Source
	@PR varchar(10) = 'ALL',			-- Clave del PR
	@SearchBySalePR bit = 1				-- Busqueda por PR de venta o PR de contacto. 1 = Buscar por PR de venta, 0 = Buscar por PR de contacto
as
set nocount on

select
	S.saID,
	S.sals,
	S.sasr,
	S.saMembershipNum,
	ST.stN,
	S.saD,
	S.saProcD,
	S.saLastName1,
	G.guCheckOutD,
	G.guag,
	A.agN,
	S.saPR1,
	P1.peN as PR1N,
	S.saPR2,
	P2.peN as PR2N,
	S.saPR3,
	P3.peN as PR3N,
	G.guQ,
	S.saGrossAmount,
	S.saCancel,
	S.saCancelD
from Sales S
	left join Guests G on G.guID = S.sagu
	left join SaleTypes ST on ST.stID = S.sast
	left join Agencies A on A.agID = G.guag
	left join Personnel P1 on P1.peID = S.saPR1
	left join Personnel P2 on P2.peID = S.saPR2
	left join Personnel P3 on P3.peID = S.saPR3	
where
	-- Lead Source
	(@LeadSource = 'ALL' or S.sals = @LeadSource)
	-- Fecha de procesable
	and S.saProcD between @DateFrom and @DateTo
	-- PR's de venta o PR de contacto
	and (@PR = 'ALL'
		-- PR's de venta
		or (@SearchBySalePR = 1 and (S.saPR1 = @PR or S.saPR2 = @PR or S.saPR3 = @PR))
		-- PR de contacto
		or (@SearchBySalePR = 0 and G.guPRInfo = @PR))
order by S.saMembershipNum
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetSalesRoomsByUser]    Script Date: 07/28/2016 14:08:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene las salas de ventas a las que tiene permiso un usuario
** 
** [wtorres]	07/Jun/2011 Created
** [wtorres]	21/Jun/2011 Modified. Agregue la posibilidad de obtener las salas de ventas asignadas a cualquier usuario
** [wtorres]	12/Ago/2014 Modified. Agregue el parametro @Regions
** [emoguel]	20/06/2016 Modified. Se Agregó el campo arrg.
**
*/
create procedure [dbo].[USP_OR_GetSalesRoomsByUser]
	@User varchar(10) = 'ALL',		-- Clave del usuario
	@Regions varchar(8000) = 'ALL'	-- Clave de regiones
as
set nocount on

select S.srID, S.srN, S.srwh, P.plpe, A.arrg
from SalesRooms S
	inner join PersLSSR P on P.plLSSRID = S.srID
	left join Areas A on A.arID = S.srar
where
	-- Usuario
	(@User = 'ALL' or P.plpe = @User)
	-- Lugar de tipo sala de ventas
	and P.plLSSR = 'SR'
	-- Activo
	and S.srA = 1
	-- Regiones
	and (@Regions = 'ALL' or A.arrg in (select item from split(@Regions, ',')))
order by S.srN



GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetWarehousesByUser]    Script Date: 07/28/2016 14:08:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene los almacenes a los que tiene permiso un usuario
** 
** [wtorres]	07/Jun/2011 Created
** [wtorres]	21/Jun/2011 Modified. Agregue la posibilidad de obtener los almacenes asignados a cualquier usuario
** [wtorres]	12/Ago/2014 Modified. Agregue el parametro @Regions
** [emoguel]	20/06/2016 Modified. Se agregó el campo arrg.
**
*/
create procedure [dbo].[USP_OR_GetWarehousesByUser]
	@User varchar(10) = 'ALL',		-- Clave del usuario
	@Regions varchar(8000) = 'ALL'	-- Clave de regiones
as
set nocount on

select W.whID, W.whN, P.plpe, A.arrg
from Warehouses W
	inner join PersLSSR P on P.plLSSRID = W.whID
	left join Areas A on A.arID = W.whar
where
	-- Usuario
	(@User = 'ALL' or P.plpe = @User)
	-- Lugar de tipo almacen
	and P.plLSSR = 'WH'
	-- Activo
	and W.whA = 1
	-- Regiones
	and (@Regions = 'ALL' or A.arrg in (select item from split(@Regions, ',')))
order by W.whN



GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetWhsMovs]    Script Date: 07/28/2016 14:08:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Script para obtener los  de un almacén 
** que ocurrieron en un día en específico.
** 
** [edgrodriguez]	22/feb/2016 Created
**
*/
CREATE procedure [dbo].[USP_OR_GetWhsMovs]
@wmwh varchar(10),	--Clave del almacén
@wmD datetime		--Fecha
as
Begin
	SELECT wmD, wmpe, peN, wmQty, giN, wmComments, wmwh FROM WhsMovs 
	INNER JOIN Personnel ON wmpe = peID 
	INNER JOIN Gifts ON wmgi=giID 
	WHERE wmwh = @wmwh AND wmD = @wmD
End

GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GraphProduction]    Script Date: 07/28/2016 14:08:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para la grafica de produccion
** 
** [wtorres]	06/May/2010 Modified. Ahora se pasa la lista de Lead Sources como un solo parametro
**							Ahora se utilizan funciones
** [wtorres]	30/Jul/2010 Modified. Eliminacion de las variables de fechas
** [wtorres]	24/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [wtorres]	19/Dic/2013 Modified. Agregue el parametro @ConsiderQuinellas
** [wtorres]	28/Jul/2016 Modified. Correccion de error cuando en una semana no habia bookings o shows, devolvia NULL, ahora devuelve cero
**
*/
create procedure [dbo].[USP_OR_GraphProduction]
	@DateFromWeek datetime,		-- Fecha desde de la semana
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0,	-- Indica si se debe considerar quinielas
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada
as
set nocount on

declare 
	@DateToWeek datetime,		-- Fecha hasta de la semana
	@DateFromMonth datetime,	-- Fecha desde del mes
	@DateToMonth datetime,		-- Fecha hasta del mes
	@Periods int,				-- Número de periodos
	@Period int,				-- Número de periodo
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime			-- Fecha hasta

-- Define las fechas semanales
set @DateToWeek = @DateFromWeek + 6

-- Define las fechas mensuales
set @DateFromMonth = DateAdd(d, (Day(@DateFromWeek) * -1) + 1, @DateFromWeek)
if (Month(@DateFromWeek) = Month(@DateToWeek))
 	set @DateToMonth = @DateToWeek
else
 	set @DateToMonth = DateAdd(m, 1, @DateFromMonth) - 1

-- Define el número de periodos
set @Periods = 3

--			Estadisticas semanales
-- =============================================
create table #tblWeek (
	Period tinyint,
	Arrivals int,
	Availables int,
	Contacts int,
	Books int,
	Shows int,
	Sales int
)
set @Period = 0
while @Period < @Periods
begin
	set @Period = @Period + 1
	set @DateFrom = @DateFromWeek - ((@Periods - @Period) * 7)
	set @DateTo = @DateToWeek - ((@Periods - @Period) * 7)
	
	insert into #tblWeek
	select @Period,
		IsNull(dbo.UFN_OR_GetArrivals(@DateFrom, @DateTo, @LeadSources), 0),
		IsNull(dbo.UFN_OR_GetAvailables(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas), 0),
		IsNull(dbo.UFN_OR_GetContacts(@DateFrom, @DateTo, @LeadSources, @BasedOnArrival), 0),
		IsNull(dbo.UFN_OR_GetBookings(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, @BasedOnArrival), 0),
		IsNull(dbo.UFN_OR_GetShows(@DateFrom, @DateTo, @LeadSources, default, @ConsiderQuinellas, default, 1, @BasedOnArrival), 0),
		IsNull(dbo.UFN_OR_GetSales(@DateFrom, @DateTo, @LeadSources, default, default, default, default, @BasedOnArrival), 0)
end

--			Estadisticas mensuales
-- =============================================
create table #tblMonth (
	Period tinyint,
	Arrivals int,
	Availables int,
	Contacts int,
	Books int,
	Shows int,
	Sales int,	
	SalesAmount money
)

set @Period = 0
while @Period < @Periods
begin
	set @Period = @Period + 1
	set @DateFrom = DateAdd(m, @Period - @Periods, @DateFromMonth)
	set @DateTo = case when @Period = @Periods then @DateToMonth
		else DateAdd(m, @Period - @Periods + 1, @DateFromMonth) - 1
	end
	
	insert into #tblMonth
	select @Period,
		IsNull(dbo.UFN_OR_GetArrivals(@DateFrom, @DateTo, @LeadSources), 0),
		IsNull(dbo.UFN_OR_GetAvailables(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas), 0),
		IsNull(dbo.UFN_OR_GetContacts(@DateFrom, @DateTo, @LeadSources, @BasedOnArrival), 0),
		IsNull(dbo.UFN_OR_GetBookings(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, @BasedOnArrival), 0),
		IsNull(dbo.UFN_OR_GetShows(@DateFrom, @DateTo, @LeadSources, default, @ConsiderQuinellas, default, 1, @BasedOnArrival), 0),
		IsNull(dbo.UFN_OR_GetSales(@DateFrom, @DateTo, @LeadSources, default, default, default, default, @BasedOnArrival), 0),
		IsNull(dbo.UFN_OR_GetSalesAmount(@DateFrom, @DateTo, @LeadSources, default, default, default, default, @BasedOnArrival), 0)
end

-- 1. Semanas
select
	Period,
	Arrivals,
	Availables,
	dbo.UFN_OR_SecureDivision(Availables, Arrivals) as AvailablesFactor,
	Contacts,
	dbo.UFN_OR_SecureDivision(Contacts, Availables) as ContactsFactor,
	Books,
	dbo.UFN_OR_SecureDivision(Books, Availables) as BooksFactor,
	Shows,
	dbo.UFN_OR_SecureDivision(Shows, Books) as ShowsFactor,
	Sales,
	dbo.UFN_OR_SecureDivision(Sales, Shows) as ClosingFactor
from #tblWeek

-- 2. Meses
select
	Period,
	Arrivals,
	Availables,
	dbo.UFN_OR_SecureDivision(Availables, Arrivals) as AvailablesFactor,
	Contacts,
	dbo.UFN_OR_SecureDivision(Contacts, Availables) as ContactsFactor,
	Books,
	dbo.UFN_OR_SecureDivision(Books, Availables) as BooksFactor,
	Shows,
	dbo.UFN_OR_SecureDivision(Shows, Books) as ShowsFactor,
	Sales,
	dbo.UFN_OR_SecureDivision(Sales, Shows) as ClosingFactor,
	SalesAmount,
	dbo.UFN_OR_SecureDivision(SalesAmount, Shows) as Efficiency
from #tblMonth

-- 3. Maximos
select
	MaxGraphW = (select Round(Max(Arrivals) + 5, -1) from #tblWeek),
	MaxGraphM = (select Round(Max(Arrivals) + 5, -1) from #tblMonth)

SET QUOTED_IDENTIFIER ON 


GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GraphProductionByPR]    Script Date: 07/28/2016 14:08:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para la grafica de produccion de PR
** 
** [wtorres]		24/Abr/2009 Modified. Ahora se pasa la lista de Lead Sources como un solo parametro
** [wtorres]		23/Sep/2009 Modified. Ahora ya devuelve los datos agrupados
** [lormartinez]	22/Sep/2014 Modified. Se agrega parametro para filtro basado en fecha de booking
** [lormartinez]	16/Dic/2015 Modified. Se modifica llamado a funcion GetPRBooking y GetPRShows para agregar parametro opcional (paymenttypes)
** [erosado]		04/Mar/2016 Modified. Se agrego parametro @SelfGen en la funcion UFN_OR_GetPRShows
**								Agregue los parametros @SalesRooms, @Countries, @Agencies y @Markets a las funciones UFN_OR_GetPRBookings,
**								UFN_OR_GetPRShows y UFN_OR_GetPRSales
** [lchairez]		15/Mar/2016 Modified. Agregue el parametro @BasedOnPRLocation en la función UFN_OR_GetPRSales
**
*/
CREATE procedure [dbo].[USP_OR_GraphProductionByPR]
	@DateFrom datetime,		-- Fecha desde
	@DateTo datetime,		-- Fecha hasta
	@LeadSource varchar(10)	-- Clave de Lead Source
as
set nocount on

select
	-- Clave del PR
	D.PR,
	-- Bookings
	Sum(D.Books) as Books,
	-- Shows
	Sum(D.Shows) as Shows,
	-- Ventas
	Sum(D.Sales) as Sales
from (
	-- Bookings
	select PR, Books, 0 as Shows, 0 as Sales
	from UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSource, default, default, default, default, default, default, default, default, default, default, default, default)
	-- Shows
	union all
	select PR, 0, Shows, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSource, default, default, default, default, default, default, default, default, default, default,
		default, default, default, default, default, default, default, default)
	-- Ventas
	union all
	select PR, 0, 0, Sales
	from UFN_OR_GetPRSales(@DateFrom, @DateTo, @LeadSource, default, default, default, default, default, default, default, default, default, default, default, default, default, default)
) as D
group by D.PR
order by Sales desc, Shows desc, Books desc

GO

/****** Object:  StoredProcedure [dbo].[USP_OR_ObtenerAsistenciasDePersonalPorSemana]    Script Date: 07/28/2016 14:08:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Autor:			William Jesús Torres Flota
-- Procedimiento:	Obtener asistencias de personal por semana
-- Fecha:			06/Mar/2009
-- Descripción:		Obtiene el número de asistencias de un personal en una semana.
--					Si no tiene definido las asistencias devuelve -1
-- =============================================
CREATE PROCEDURE [dbo].[USP_OR_ObtenerAsistenciasDePersonalPorSemana]
	@SalesRoom varchar(10),	-- Clave de la sala de ventas
	@Personnel varchar(10),	-- Clave del personal
	@DateFrom datetime,		-- Fecha de inicio de la semana
	@DateTo datetime		-- Fecha de fin de la semana
AS
	SET NOCOUNT ON;

declare
@Count int,
@NumAssistance int

-- Determina si hay asistencias
set @Count = (select count(aspe) from Assistance
	where asPlaceID = @SalesRoom and aspe = @Personnel and asStartD = @DateFrom and asEndD = @DateTo)
-- Si hay asistencias
if @Count = 1
begin
	select @NumAssistance = asNum
	from Assistance
	where asPlaceID = @SalesRoom and aspe = @Personnel and asStartD = @DateFrom and asEndD = @DateTo
end
-- Si NO hay asistencias
else
	set @NumAssistance = -1
-- Devuelve el número de asistencias
select @NumAssistance as NumAssistance


GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptCostByPR]    Script Date: 07/28/2016 14:08:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de costo por PR
** 
** [wtorres]		17/Oct/2011 Created
** [lormartinez]	22/Sep/2014 Modified. Se agrega parametro para filtro basado en fecha de booking
** [lormartinez]	16/Dic/2015 Modified. Se agrega paranetro default en las funciones de GetBooking y GetShows
** [erosado]		04/Mar/2016 Modified. Se agrego parametro @SelfGen en la funcion UFN_OR_GetPRShows
**								Agregue los parametros @SalesRooms, @Countries, @Agencies y @Markets a la funcion UFN_OR_GetPRShows
**
*/
CREATE procedure [dbo].[USP_OR_RptCostByPR]
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0	-- Indica si se debe considerar quinielas
as
set nocount on

select
	-- PR
	D.PR,
	-- Nombre del PR
	IsNull(P.peN, 'CODE NOT FOUND!') as PRN,
	-- Shows
	Sum(D.Shows) as Shows,
	-- Costo
	Sum(D.Cost) as TotalCost,
	-- Costo promedio
	dbo.UFN_OR_SecureDivision(Sum(D.Cost), Sum(D.Shows)) as AverageCost
from (
	-- Shows
	select PR, Shows, 0 as Cost
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, 0, 0, default, default, default,
		default, default, default, default, default, default, default, default)
	-- Monto de recibos de regalos
	union all
	select PR, 0, GiftsReceiptsAmount
	from UFN_OR_GetPRGiftsReceiptsAmount(@DateFrom, @DateTo, @LeadSources, 0)
) as D
	left join Personnel P on D.PR = P.peID
group by D.PR, P.peN
order by AverageCost, D.PR

GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptCostByPRWithDetailGifts]    Script Date: 07/28/2016 14:08:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el PR con Detalle de Gifts
** 
** [gmaya]			07/Jul/2014 Created
** [lormartinez]	22/Sep/2014 Modified. Se agrega parametro para filtro basado en fecha de booking
** [lormartinez]	16/Dic/2015 Modified. Se agrega paranetro default en las funciones de GetBooking y GetShows
** [erosado]		04/Mar/2016 Modified. Se agrego parametro @SelfGen en la funcion UFN_OR_GetPRShows
**								Agregue los parametros @SalesRooms, @Countries, @Agencies y @Markets a la funcion UFN_OR_GetPRShows
**
*/
CREATE procedure [dbo].[USP_OR_RptCostByPRWithDetailGifts]
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0,	-- Indica si se debe considerar quinielas
	@DetailGifts bit 	-- Indica si desea Detail Gifts
as
set nocount on
	
select
	-- PR
	D.PR,
	-- Nombre del PR
	IsNull(P.peN, 'CODE NOT FOUND!') as PRN,
	D.Qty,
	D.GiftsID, 
	D.GiftsName,
	-- Shows
	Sum(D.Shows) as Shows,
	-- Costo
	Sum(D.Cost) as TotalCost,
	-- Costo promedio
	dbo.UFN_OR_SecureDivision(Sum(D.Cost), Sum(D.Shows)) as AverageCost
from (
	-- Shows
	select PR, Shows, 0 as Cost, 0 as Qty, ''  as GiftsID, '' as GiftsName 
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, 0, 0, default, default, default,
		default, default, default, default, default, default, default, default)
	-- Monto de recibos de regalos
	union all
	select PR, 0 as Show, GiftsReceiptsAmount, 0 as Qty, ''  as GiftsID, '' as GiftsName 
	from UFN_OR_GetPRGiftsReceiptsAmount(@DateFrom, @DateTo, @LeadSources, 0)
	-- Detalles
	union all
	select PR, 0 as Show, 0 as Cost, Qty, GiftsID, GiftsName 
	from UFN_OR_GetCostByPRWithDetailGifts(@DateFrom, @DateTo, @LeadSources, 0, @DetailGifts)
) as D
	left join Personnel P on D.PR = P.peID
group by D.PR, P.peN, D.Qty, D.GiftsID, D.GiftsName
order by  AverageCost, D.PR
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptDepositsPaymentByPR]    Script Date: 07/28/2016 14:08:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de pago de depositos a los PRs (Comisiones de PRs de Outhouse)
**
** [alesanchez]		19/Nov/2013 Created
** [gmaya]			11/Ago/2014 Modified. Agregue el parametro de formas de pago
** [lormartinez]	22/Sep/2014 Modified. Se agrega parametro para filtro basado en fecha de booking
** [lormartinez]	15/Dic/2015 Modified. Se modifica parametro de formas de pago.
**								Se envia forma de pago en funciones para filtrado
** [erosado]		04/Mar/2016 Modified. Se agrego parametro @SelfGen en la funcion UFN_OR_GetPRShows
**								Agregue los parametros @SalesRooms, @Countries, @Agencies y @Markets a las funciones UFN_OR_GetPRContacts,
**								UFN_OR_GetPRAvailables, UFN_OR_GetPRBookings, UFN_OR_GetPRShows, UFN_OR_GetPRSales y UFN_OR_GetPRSalesAmount
** [lchairez]		18/Abr/2016 Modified. Agregue el parametro @BasesOnPRLocation en la función UFN_OR_GetPRSalesAmount
** [wtorres]		12/May/2016 Modified. Correccion de error. No estaba ordenando correctamente los datos del reporte. Elimine la consulta de "Lugares de Pago"
**								Elimine la columna tpShow
**										  
*/
create procedure [dbo].[USP_OR_RptDepositsPaymentByPR]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000),			-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@PaymentTypes varchar(MAX) = 'ALL',	-- Claves de formas de pago
	@FilterDeposit tinyint = 0			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
										--		3. Con deposito y shows sin deposito (Deposits & Flyers Show)
as
set nocount on

declare	@FilterDepositAlternate tinyint	-- Filtro de depositos alterno (Aplica para bookings)

set @FilterDepositAlternate = @FilterDeposit
-- si se desea el reporte con deposito y shows sin deposito (Deposits & Flyers Show)
if @FilterDeposit = 3
	set @FilterDeposit = 0	-- Sin filtro

select
	'TO RETAIN' AS Category,
	-- Esquema de pago
	Cast('' as varchar(50)) as PaymentSchema,
	-- Porcentaje de show del esquema de pago
	Cast(0 as money) as PaymentSchemaFactor,
	-- Clave del PR
	T.PR,
	-- Nombre del PR
	IsNull(P.peN, 'CODE NOT FOUND!') as PRN,
	-- Bookings
	Sum(T.Books) as Books,
	-- In & Outs
	Sum(T.InOuts) as InOuts,
	-- Bookings netos
	Sum(T.GrossBooks) as GrossBooks,
	-- Porcentaje de show factor
	[dbo].UFN_OR_SecureDivision(Sum(T.GrossShows), Sum(T.GrossBooks)) as ShowsFactor, 
	-- Shows netos
	Sum(T.GrossShows) as GrossShows,
	-- Sales Amount
	Sum(T.SalesAmount) SalesAmount,
	-- Eficiency
	[dbo].UFN_OR_SecureDivision(Sum(T.SalesAmount), Sum(T.GrossBooks)) as Efficiency
into #PRs 
from (
	-- Bookings
	select PR, Books, 0 as GrossBooks, 0 as InOuts, 0 as GrossShows, 0 as SalesAmount
	from UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, 0, default, 0,@PaymentTypes, default, default, default, default)
	-- Bookings netos
	union all
	select PR, 0, Books, 0, 0, 0
	from UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, 0, 0, 0, @PaymentTypes, default, default, default, default)
	-- In & Outs
	union all
	select PR, 0, 0, Shows, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, 0, 1, default, default, default, default,
		default, 1, @PaymentTypes, default, default, default, default, default)
	-- Shows netos
	union all
	select PR, 0, 0, 0, Shows, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, 0, 0, default, default, default, default,
		default, 1, @PaymentTypes, default, default, default, default, default)
	-- Monto de ventas
	union all
	select PR, 0, 0, 0, 0, SalesAmount
	from dbo.UFN_OR_GetPRSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, @FilterDeposit, default, default, 1, default, default, default, default, default)
) AS T 
	left join Personnel P on T.PR = P.peID
group by T.PR, P.peN
order by ShowsFactor desc, GrossShows desc, Books desc

-- actualizamos la categoria (TO RETAIN o TO PAY) y el esquema de pago
;WITH cte AS
(SELECT P.*,
	IsNull(PAS.pasShowFactor, 0) as pasShowFactor, 
	IsNull(Pas.pasN, '') as pasN
FROM #PRs P
	OUTER APPLY (
		SELECT pasShowFactor / 100 as pasShowFactor, pasN
		FROM PaymentSchemas
		WHERE P.GrossShows between pasCoupleFrom AND pasCoupleTo
	) Pas
)
update cte
set PaymentSchemaFactor = pasShowFactor,
    PaymentSchema = pasN,
    Category = CASE WHEN ShowsFactor >= pasShowFactor THEN 'TO PAY' ELSE Category END

-- Depositos
select
	P.Category,
	C.pcN,
	P.PaymentSchema,
	P.PaymentSchemaFactor,
	P.PR, 
	P.PRN,
	G.guID,
	dbo.UFN_OR_GetFullName( G.guLastName1, g.gufirstname1) [guName],
	G.guAge1, 
	G.guBookD, 
	G.guShowD,
	G.guOutInvitNum,
	G.guHotel,
	G.guls, 
	G.gusr, 
	D.bdpt, 
	D.bdcu,
	D.bdAmount,
	D.bdReceived,
	case when D.bdpt = 'CC' and P.Category = 'TO PAY' then dbo.UFN_OR_GetTotalReceivedRound(D.bdAmount - D.bdReceived, 3.33) else D.bdReceived - D.bdAmount
	end as ToPay
into #Deposits
from #PRs P
	inner join Guests G on G.guPRInvit1 = P.PR
	inner join BookingDeposits D on D.bdgu = G.guID 
	left join PaymentPlaces C on C.pcID = D.bdpc
where
	-- Fecha de booking
	G.guBookD between @DateFrom and @DateTo
	-- No asisten al show
	and G.guShow = 0
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls  in (select item from split(@LeadSources, ','))) 
	-- Formas de pago
	and (@PaymentTypes = 'ALL' or D.bdpt  in (select item from split(@PaymentTypes, ',')))

-- 1. PRs
-- =============================================
select
	P.Category,
	P.PaymentSchema,
	P.PaymentSchemaFactor,
	P.PR,
	P.PRN,
	P.Books,
	P.InOuts,
	P.GrossBooks,
	P.ShowsFactor, 	     
	P.GrossShows,
	P.SalesAmount,
	P.Efficiency
from #PRs P
	inner join #Deposits D ON D.PR = P.PR
group by P.Category, P.PaymentSchema, P.PaymentSchemaFactor, P.PR, P.PRN, P.books, P.InOuts, P.GrossBooks,P.ShowsFactor, P.GrossShows, P.SalesAmount, P.Efficiency
order by P.Category, P.PaymentSchemaFactor, P.PRN

-- 2. Depositos
-- =============================================
select * from #Deposits
order by Category, pcN, PaymentSchemaFactor, PRN

-- 3. Monedas
-- =============================================
select distinct C.cuID, C.cuN
from #Deposits D
	left join Currencies C on D.bdcu = C.cuID

-- 4. Formas de Pago
-- =============================================
select distinct T.ptID, T.ptN 
from #Deposits D
	left join PaymentTypes T on D.bdpt = T.ptID
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptFollowUpByPR]    Script Date: 07/28/2016 14:08:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de seguimiento por PR
** 
** [wtorres]		26/May/2010 Created
** [wtorres]		18/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [lormartinez]	22/Sep/2014 Modified. Se agrega parametro para filtro basado en fecha de booking
** [lormartinez]	15/Dic/2015 Modified. Se agrega paranetro default en las funciones de GetBooking y GetShows
** [erosado]		04/Mar/2016 Modified. Se agrego parametro @SelfGen en la funcion UFN_OR_GetPRShows
**								Agregue los parametros @SalesRooms, @Countries, @Agencies y @Markets a las funciones UFN_OR_GetPRContacts,
**								UFN_OR_GetPRAvailables, UFN_OR_GetPRBookings, UFN_OR_GetPRShows, UFN_OR_GetPRSales y UFN_OR_GetPRSalesAmount
** [lchairez]		15/Abr/2016 Modified. Agregue el parametro @BasedOnPRLocation a las funciones UFN_OR_GetPRSales y UFN_OR_GetPRSalesAmount
**
*/
CREATE procedure [dbo].[USP_OR_RptFollowUpByPR]
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0,	-- Indica si se debe considerar quinielas
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada
as
set nocount on

select
	-- Clave del PR
	D.PR as PRID,
	-- Nombre del PR
	IsNull(P.peN, 'CODE NOT FOUND!') as PRN,
	-- Estatus del PR
	IsNull(P.peps, 'INACTIVE') as Status,	
	-- Contactos
	Sum(D.Contacts) as Contacts,
	-- Disponibles
	Sum(D.Availables) as Availables,
	-- Seguimientos
	Sum(D.FollowUps) as FollowUps,
	-- Porcentaje de seguimientos
	[dbo].UFN_OR_SecureDivision(Sum(D.FollowUps), Sum(D.Availables)) as FollowUpsFactor,
	-- Bookings
	Sum(D.Books) as Books,
	-- Porcentaje de bookings
	[dbo].UFN_OR_SecureDivision(Sum(D.Books), Sum(D.FollowUps)) as BooksFactor,
	-- Shows
	Sum(D.Shows) as Shows,
	-- Ventas
	Sum(D.Sales) as Sales,
	-- Monto de ventas
	Sum(D.SalesAmount) as SalesAmount,
	-- Porcentaje de cierre
	[dbo].UFN_OR_SecureDivision(Sum(D.Sales), Sum(D.Shows)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Shows)) as Efficiency,
	-- Venta promedio
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Sales)) as AverageSale
from (
	-- Contactos
	select PR, Contacts, 0 as Availables, 0 as FollowUps, 0 as Books, 0 as Shows, 0 as Sales, 0 as SalesAmount
	from UFN_OR_GetPRContacts(@DateFrom, @DateTo, @LeadSources, 1, @BasedOnArrival, default, default, default, default)
	-- Disponibles
	union all
	select PR, 0, Availables, 0, 0, 0, 0, 0
	from UFN_OR_GetPRAvailables(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, 1, @BasedOnArrival, default, default, default, default)
	-- Seguimientos
	union all
	select PR, 0, 0, FollowUps, 0, 0, 0, 0
	from UFN_OR_GetPRFollowUps(@DateFrom, @DateTo, @LeadSources, @BasedOnArrival)
	-- Bookings
	union all
	select PR, 0, 0, 0, Books, 0, 0, 0
	from UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, default, @BasedOnArrival, default, default, default, default, default)
	-- Shows
	union all
	select PR, 0, 0, 0, 0, Shows, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, default, default, default,
		default, default, @BasedOnArrival, default, default, default, default, default, default, default)
	-- Ventas
	union all
	select PR, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetPRSales(@DateFrom, @DateTo, @LeadSources, default, default, default, default, default, default, default, @BasedOnArrival, default, default, default, default, default, default)
	-- Monto de ventas
	union all
	select PR, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetPRSalesAmount(@DateFrom, @DateTo, @LeadSources, default, default, default, default, default, default, default, @BasedOnArrival, default, default, default, default, default, default)
) as D
	left join Personnel P on D.PR = P.peID
group by D.PR, P.peN, P.peps
order by Status, SalesAmount desc, Efficiency desc, Shows desc, Books desc, FollowUps desc, Availables desc, Contacts desc, D.PR
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptProductionByPRContactOutside]    Script Date: 07/28/2016 14:08:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por PR de contactos(Outside)
** 
** [lchairez]	10/Dic/2013 Created
** [erosado]	04/Mar/2016 Modified. Agregue los parametros @SalesRooms, @Countries, @Agencies y @Markets a la funcion UFN_OR_GetPRContacts
**
*/
CREATE procedure [dbo].[USP_OR_RptProductionByPRContactOutside]
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',		-- Claves de PRs
	@Program varchar(10) = 'ALL',	-- Clave de programa
	@FilterDeposit tinyint = 0		-- Filtro de depositos:
									--		0. Sin filtro
									--		1. Con deposito (Deposits)
									--		2. Sin deposito (Flyers)
									--		3. Con deposito y shows sin deposito (Deposits & Flyers Show)
as
set nocount on

declare	@FilterDepositAlternate tinyint	-- Filtro de depositos alterno (Aplica para bookings)

set @FilterDepositAlternate = @FilterDeposit
-- si se desea el reporte con deposito y shows sin deposito (Deposits & Flyers Show)
if @FilterDeposit = 3
	set @FilterDeposit = 0	-- Sin filtro

select
	-- Clave del PR
	D.PR,
	-- Nombre del PR
	IsNull(P.peN, 'CODE NOT FOUND!') as PRN,
	-- Estatus del PR
	IsNull(P.peps, 'INACTIVE') as Status,
	-- Contact
	Sum(D.Contacts) as Contacts,
	-- Bookings
	Sum(D.Books) as Books,
	-- Porcentaje de Books
	[dbo].UFN_OR_SecureDivision(Sum(D.Books), Sum(D.Contacts)) as BooksFactor,
	-- In & Outs
	Sum(D.InOuts) as InOuts,
	-- Bookings netos
	Sum(D.GrossBooks) as GrossBooks,
	-- Shows netos
	Sum(D.GrossShows) as GrossShows,
	-- Porcentaje de shows
	[dbo].UFN_OR_SecureDivision(Sum(D.GrossShows), Sum(D.GrossBooks)) as ShowsFactor,
	-- Directas
	Sum(D.Directs) as Directs,
	-- Walk Outs
	Sum(D.WalkOuts) as WalkOuts,
	-- Tours de cortesia
	Sum(D.CourtesyTours) as CourtesyTours,
	-- Tours de rescate
	Sum(D.SaveTours) as SaveTours,
	-- Shows
	Sum(D.Shows) as Shows,
	-- Ventas
	Sum(D.Sales) as Sales_TOTAL,
	-- Monto de ventas
	Sum(D.SalesAmount) as SalesAmount_TOTAL,
	-- Ventas procesables
	Sum(D.Sales - D.SalesOOP + D.SalesCancel) as Sales_PROC,
	-- Monto de ventas procesables
	Sum(D.SalesAmount - D.SalesAmountOOP + D.SalesAmountCancel) as SalesAmount_PROC,
	-- Ventas Out Of Pending
	Sum(D.SalesOOP) as Sales_OOP,
	-- Monto de ventas Out Of Pending
	Sum(D.SalesAmountOOP) as SalesAmount_OOP,
	-- Ventas pendientes
	Sum(D.SalesPending) as Sales_PEND,
	-- Monto de ventas pendientes
	Sum(D.SalesAmountPending) as SalesAmount_PEND,
	-- Ventas canceladas
	Sum(D.SalesCancel) as Sales_CANCEL,
	-- Monto de ventas canceladas
	Sum(D.SalesAmountCancel) as SalesAmount_CANCEL,
	-- Subtotal
	Sum(D.SalesAmount) as Subtotal,
	-- Porcentaje de cancelacion
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmountCancel), Sum(D.SalesAmount + D.SalesAmountCancel)) as CancelFactor,
	-- Porcentaje de cierre
	[dbo].UFN_OR_SecureDivision(Sum(D.Sales), Sum(D.Shows)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Shows)) as Efficiency,
	-- Venta promedio
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Sales)) as AverageSale
from (
	-- Contact
	select PR, Contacts , 0 Books, 0 as GrossBooks, 0 as InOuts, 0 as WalkOuts, 0 as CourtesyTours, 0 as SaveTours, 0 as Shows, 0 as GrossShows, 0 as Directs,
		0 as Sales, 0 as SalesAmount, 0 as SalesOOP, 0 as SalesAmountOOP, 0 as SalesPending, 0 as SalesAmountPending, 0 as SalesCancel,
		0 as SalesAmountCancel
	from UFN_OR_GetPRContacts(@DateFrom, @DateTo, @LeadSources, default, default, default, default, default, default)
	-- Bookings
	union all
	select PR, 0 Contacts, Books, 0 as GrossBooks, 0 as InOuts, 0 as WalkOuts, 0 as CourtesyTours, 0 as SaveTours, 0 as Shows, 0 as GrossShows, 0 as Directs,
		0 as Sales, 0 as SalesAmount, 0 as SalesOOP, 0 as SalesAmountOOP, 0 as SalesPending, 0 as SalesAmountPending, 0 as SalesCancel,
		0 as SalesAmountCancel
	from UFN_OR_GetPRContactBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, 0, default, default)
	-- Bookings netos
	union all
	select PR, 0 Contacts, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRContactBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, 0, 0, default)
	-- In & Outs
	union all
	select PR, 0 Contacts, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRContactShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, 1, default, default,
		default, default, default)
	-- Walk Outs
	union all
	select PR, 0 Contacts, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRContactShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, default, 1, default,
		default, default, default)
	-- Tours de cortesia
	union all
	select PR, 0 Contacts, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRContactShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, default, default, 2,
		default, default, default)
	-- Tours de rescate
	union all
	select PR, 0 Contacts, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRContactShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, default, default, 3,
		default, default, default)
	-- Shows
	union all
	select PR, 0 Contacts, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRContactShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, default, default,
		default, default, 1, default)
	-- Shows netos
	union all
	select PR, 0 Contacts, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRContactShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, 1, 0, default, default, default,
		default, default)
	-- Directas
	union all
	select PR, 0 Contacts, 0, 0, 0, 0, 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRContactBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, 1, default, default)
	-- Ventas procesables
	union all
	select PR, 0 Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRContactSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, @FilterDeposit, default,
		default)
	-- Monto de ventas procesables
	union all
	select PR, 0 Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRContactSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, @FilterDeposit, default,
		default)
	-- Ventas Out Of Pending
	union all
	select PR, 0 Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0
	from UFN_OR_GetPRContactSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, default, @FilterDeposit, default, default)
	-- Monto de ventas Out Of Pending
	union all
	select PR, 0 Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0
	from UFN_OR_GetPRContactSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, default, @FilterDeposit, default,
		default)
	-- Ventas pendientes
	union all
	select PR, 0 Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0
	from UFN_OR_GetPRContactSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, @FilterDeposit, default, 1)
	-- Monto de ventas pendientes
	union all
	select PR, 0 Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0
	from UFN_OR_GetPRContactSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, @FilterDeposit, default,
		1)
	-- Ventas canceladas
	union all
	select PR, 0 Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetPRContactSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, 1, @FilterDeposit, default, default)
	-- Monto de ventas canceladas
	union all
	select PR, 0 Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetPRContactSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, 1, @FilterDeposit, default,
		default)
) as D
	left join Personnel P on D.PR = P.peID
group by D.PR, P.peN, P.peps
order by Status, SalesAmount_TOTAL desc, Shows desc, Books desc, D.PR

GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptProductionByPRInhouse]    Script Date: 07/28/2016 14:08:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por PR (Inhouse)
** 
** [wtorres]		14/Oct/2008 Modified. Agregue los campos de locacion y equipos
** [wtorres]		24/Abr/2009 Modified. Ahora se pasa la lista de Lead Sources como un solo parametro
** [wtorres]		08/Sep/2009 Modified. Ahora ya devuelve los datos agrupados
** [wtorres]		30/Oct/2009 Modified. Agregue el parametro @ConsiderQuinellas
** [wtorres]		02/Ene/2010 Modified. No se estaban contando bien los shows netos
** [wtorres]		15/Abr/2010 Modified. Ahora el numero de shows netos son todos los shows menos las directas no In & Outs
** [wtorres]		18/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [wtorres]		21/Ene/2011 Modified. Ahora el numero total de shows no incluye a los In & Outs cuando no se consideran quinielas
** [wtorres]		11/Mar/2011 Modified. Agregue los campos de tours y walk outs
** [wtorres]		21/May/2011 Modified. Agregue los campos de no calificados, tours de cortesia y tours netos
** [wtorres]		30/May/2011 Modified. Agregue el campo de calificados y cambie la forma de contar los tours netos: ahora son los tours calificados
** [wtorres]		10/Ago/2011 Modified. Elimine los campos de calificados, no calificados y tours netos y agregue el campo de shows del programa de rescate
** [wtorres]		21/Sep/2011 Modified. Elimine los campos de depositos y selfgens y agregue los campos de shows sin directas, total de tours y total de shows.
**								Elimine las columnas de ventas self gen y PR y agregue las ventas pendientes
** [wtorres]		26/Sep/2011 Modified. Ahora el numero de bookings sin directas se calcula mediante una diferencia en vez de hacer un select
**								Ahora el numero de total de tours se calcula mediante una suma en vez de hacer un select
** [wtorres]		12/Oct/2011 Modified. Ahora al numero de total de tours se le suman los Walk Outs y agregue las columnas de ventas out of pending
** [wtorres]		19/Oct/2011 Modified. Elimine el uso de las funciones UFN_OR_GetPRInOuts y UFN_OR_GetPRWalkOuts
** [wtorres]		01/Nov/2011 Modified. Elimine el uso de las funciones UFN_OR_GetPRCourtesyTours, UFN_OR_GetPRSaveTours y UFN_OR_GetPRTours
** [wtorres]		03/Dic/2013 Modified. Ahora la eficiencia se divide entre la columna UPS en lugar de Shows
** [wtorres]		26/May/2014 Modified. Ahora el porcentaje de cierre se divide entre la columna UPS en lugar de Shows
** [lormartinez]	22/Sep/2014 Modified. Se agrega parametro para filtro basado en fecha de booking
** [lormartinez]	15/Dic/2015 Modified. Se agrega paranetro default en las funciones de GetBooking y GetShows
** [erosado]		04/Mar/2016 Modified. Se agrego parametro @SelfGen en la funcion UFN_OR_GetPRShows
**								Agregue los parametros @SalesRooms, @Countries, @Agencies y @Markets a las funciones UFN_OR_GetPRAssigns,
**								UFN_OR_GetPRContacts, UFN_OR_GetPRAvailables, UFN_OR_GetPRBookings, UFN_OR_GetPRShows, UFN_OR_GetPRSales
**								y UFN_OR_GetPRSalesAmount
** [lchairez]		15/Abr/2016 Modified. Agregue el parametro @BasedOnPRLocation al procedimiento y a las funciones UFN_OR_GetPRSales y UFN_OR_GetPRSalesAmount
**
*/
CREATE procedure [dbo].[USP_OR_RptProductionByPRInhouse]
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0,	-- Indica si se debe considerar quinielas
	@BasedOnArrival bit = 0,	-- Indica si se debe basar en la fecha de llegada
	@BasedOnPRLocation bit = 0	-- Indica si se debe basar en la locacion por default del PR
as
SET NOCOUNT ON

select
	-- Locacion
	IsNull(L.loN, 'NO LOCATION') as Location,
	-- Equipo
	IsNull(T.tgN, 'NO TEAM') as Team,
	-- Lider del equipo
	PL.peN as Leader,
	-- Clave del PR
	D.PR as PRID,
	-- Nombre del PR
	IsNull(P.peN, 'CODE NOT FOUND!') as PRN,
	-- Estatus del PR
	IsNull(P.peps, 'INACTIVE') as Status,
	-- Asignaciones
	Sum(D.Assigns) as Assigns,
	-- Contactos
	Sum(D.Contacts) as Contacts,
	-- Porcentaje de contactacion
	[dbo].UFN_OR_SecureDivision(Sum(D.Contacts), Sum(D.Assigns)) as ContactsFactor,
	-- Disponibles
	Sum(D.Availables) as Availables,
	-- Porcentaje de disponibles
	[dbo].UFN_OR_SecureDivision(Sum(D.Availables), Sum(D.Contacts)) as AvailablesFactor,
	-- Bookings sin directas
	Sum(D.Books) - Sum(D.Directs) as BooksNoDirects,
	-- Directas
	Sum(D.Directs) as Directs,
	-- Bookings
	Sum(D.Books) as Books,
	-- Porcentaje de bookings
	[dbo].UFN_OR_SecureDivision(Sum(D.Books), Sum(D.Availables)) as BooksFactor,
	-- Shows sin directas no antes In & Out
	Sum(D.ShowsNoDirects) as ShowsNoDirects,
	-- Porcentaje de shows
	[dbo].UFN_OR_SecureDivision(Sum(D.ShowsNoDirects), Sum(D.Books) - Sum(D.Directs)) as ShowsFactor,
	-- Shows
	Sum(D.Shows) as Shows,
	-- In & Outs
	Sum(D.InOuts) as InOuts,
	-- Walk Outs
	Sum(D.WalkOuts) as WalkOuts,
	-- Tours regulares
	Sum(D.Tours) as Tours,
	-- Tours de cortesia
	Sum(D.CourtesyTours) as CourtesyTours,
	-- Tours de rescate
	Sum(D.SaveTours) as SaveTours,
	-- Total de tours
	Sum(D.Tours) + Sum(D.CourtesyTours) + Sum(D.SaveTours) + Sum(D.WalkOuts) as TotalTours,
	-- UPS (Unidades producidas)
	Sum(D.UPS) as UPS,
	-- Ventas
	Sum(D.Sales) as Sales_PROC,
	-- Monto de ventas
	Sum(D.SalesAmount) as SalesAmount_PROC,
	-- Ventas pendientes
	Sum(D.SalesPending) as Sales_PEND,
	-- Monto de ventas pendientes
	Sum(D.SalesAmountPending) as SalesAmount_PEND,
	-- Ventas Out Of Pending
	Sum(D.SalesOOP) as Sales_OOP,
	-- Monto de ventas Out Of Pending
	Sum(D.SalesAmountOOP) as SalesAmount_OOP,
	-- Porcentaje de cierre
	[dbo].UFN_OR_SecureDivision(Sum(D.Sales), Sum(D.UPS)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.UPS)) as Efficiency,
	-- Venta promedio
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Sales)) as AverageSale
from (
	-- Asignaciones
	select PR, Assigns, 0 as Contacts, 0 as Availables, 0 as Directs, 0 as Books, 0 as Shows, 0 as ShowsNoDirects, 0 as UPS, 0 as InOuts,
		0 as WalkOuts, 0 as Tours, 0 as CourtesyTours, 0 as SaveTours, 0 as Sales, 0 as SalesAmount, 0 as SalesPending, 0 as SalesAmountPending,
		0 as SalesOOP, 0 as SalesAmountOOP
	from UFN_OR_GetPRAssigns(@DateFrom, @DateTo, @LeadSources, default, default, default, default)
	-- Contactos
	union all
	select PR, 0, Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRContacts(@DateFrom, @DateTo, @LeadSources, default, @BasedOnArrival, default, default, default, default)
	-- Disponibles
	union all
	select PR, 0, 0, Availables, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRAvailables(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, default, @BasedOnArrival, default, default, default, default)
	-- Directas
	union all
	select PR, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, 1, default, @BasedOnArrival, default, default, default, default, default)
	-- Bookings
	union all
	select PR, 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, default, @BasedOnArrival, default, default, default, default, default)
	-- Shows
	union all
	select PR, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, default, default, default,
		default, default, @BasedOnArrival, default, default, default, default, default, default, default)
	-- Shows sin directas no antes In & Out
	union all
	select PR, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, default, default, default,
		1, default, @BasedOnArrival, default, default, default, default, default, default, default)
	-- UPS (son todos los shows con tour o venta)
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, default, default, default,
		default, 1, @BasedOnArrival, default, default, default, default, default, default, default)
	-- In & Outs
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, 1, default, default, default,
		default, @BasedOnArrival, default, default, default, default, default, default, default)
	-- Walk Outs
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, default, 1, default, default,
		default, @BasedOnArrival, default, default, default, default, default, default, default)
	-- Tours regulares
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, default, default, 1, default,
		default, @BasedOnArrival, default, default, default, default, default, default, default)
	-- Tours de cortesia
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, default, default, 2, default,
		default, @BasedOnArrival, default, default, default, default, default, default, default)
	-- Tours de rescate
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, default, default, 3, default,
		default, @BasedOnArrival, default, default, default, default, default, default, default)
	-- Ventas
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSales(@DateFrom, @DateTo, @LeadSources, default, default, default, default, default, default, default, @BasedOnArrival, default, @BasedOnPRLocation, default, default, default, default)
	-- Monto de ventas
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0
	from UFN_OR_GetPRSalesAmount(@DateFrom, @DateTo, @LeadSources, default, default, default, default, default, default, default, @BasedOnArrival, default, @BasedOnPRLocation, default, default, default, default)
	-- Ventas pendientes
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0
	from UFN_OR_GetPRSales(@DateFrom, @DateTo, @LeadSources, default, default, default, default, default, default, 1, @BasedOnArrival, default, @BasedOnPRLocation, default, default, default, default)
	-- Monto de ventas pendientes
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0
	from UFN_OR_GetPRSalesAmount(@DateFrom, @DateTo, @LeadSources, default, default, default, default, default, default, 1, @BasedOnArrival, default, @BasedOnPRLocation, default, default, default, default)
	-- Ventas Out Of Pending
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetPRSales(@DateFrom, @DateTo, @LeadSources, default, default, default, 1, default, default, default, @BasedOnArrival, default, @BasedOnPRLocation, default, default, default, default)
	-- Monto de ventas Out Of Pending
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetPRSalesAmount(@DateFrom, @DateTo, @LeadSources, default, default, default, 1, default, default, default, @BasedOnArrival, default, @BasedOnPRLocation, default, default, default, default)
) as D
	left join Personnel P on D.PR = P.peID
	left join TeamsGuestServices T on P.peTeam = T.tgID and P.pePlaceID = T.tglo and P.peTeamType = 'GS'
	left join Locations L on T.tglo = L.loID
	left join Personnel PL on T.tgLeader = PL.peID
group by D.PR, P.peN, P.peps, L.loN, T.tgN, PL.peN
order by Location, Team, Status, SalesAmount_PROC desc, Efficiency desc, Shows desc, Books desc, Contacts desc, Availables desc, Assigns desc, D.PR
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptProductionByPROutside]    Script Date: 07/28/2016 14:08:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por PR (Outhouse)
** 
** [wtorres]		27/Oct/2009 Modified. Ahora se pasa la lista de Lead Sources como un solo parametro y devuelve los datos agrupados
** [wtorres]		02/Ene/2010 Modified. No se estaban contando bien los bookings netos ni los shows netos
** [wtorres]		16/Abr/2010 Modified. Agregue los parametros @PRs y @Program
** [wtorres]		19/Oct/2011 Modified. Elimine el uso de la funcion UFN_OR_GetPRInOuts
** [wtorres]		01/Nov/2011 Modified. Agregue los campos de Walk Outs, Courtesy Tours y Save Tours
** [wtorres]		16/Nov/2011 Modified. Ahora el numero total de shows solo cuenta los shows con tour o venta y agregue las columnas de ventas pendientes
** [lormartinez]	22/Sep/2014 Modified. Se agrega parametro para filtro basado en fecha de booking
** [lormartinez]	15/Dic/2015 Modified. Se agrega paranetro default en las funciones de GetBooking y GetShows
** [erosado]		04/Mar/2016 Modified. Se agrego parametro @SelfGen en la funcion UFN_OR_GetPRShows
**								Agregue los parametros @SalesRooms, @Countries, @Agencies y @Markets a las funciones UFN_OR_GetPRBookings,
**								UFN_OR_GetPRShows, UFN_OR_GetPRSales y UFN_OR_GetPRSalesAmount
** [lchairez]		15/Abr/2016 Modified. Agregue el parametro @BasedOnPRLocation a las funciones UFN_OR_GetPRSales y UFN_OR_GetPRSalesAmount
**
*/
CREATE procedure [dbo].[USP_OR_RptProductionByPROutside]
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',		-- Claves de PRs
	@Program varchar(10) = 'ALL',	-- Clave de programa
	@FilterDeposit tinyint = 0,		-- Filtro de depositos:
									--		0. Sin filtro
									--		1. Con deposito (Deposits)
									--		2. Sin deposito (Flyers)
									--		3. Con deposito y shows sin deposito (Deposits & Flyers Show)
	@BasedOnBooking bit = 0			-- Indica si se debe basar en la fecha de booking
as
set nocount on

declare	@FilterDepositAlternate tinyint	-- Filtro de depositos alterno (Aplica para bookings)

set @FilterDepositAlternate = @FilterDeposit
-- si se desea el reporte con deposito y shows sin deposito (Deposits & Flyers Show)
if @FilterDeposit = 3
	set @FilterDeposit = 0	-- Sin filtro

select
	-- Clave del PR
	D.PR,
	-- Nombre del PR
	IsNull(P.peN, 'CODE NOT FOUND!') as PRN,
	-- Estatus del PR
	IsNull(P.peps, 'INACTIVE') as Status,
	-- Bookings
	Sum(D.Books) as Books,
	-- In & Outs
	Sum(D.InOuts) as InOuts,
	-- Bookings netos
	Sum(D.GrossBooks) as GrossBooks,
	-- Shows netos
	Sum(D.GrossShows) as GrossShows,
	-- Porcentaje de shows
	[dbo].UFN_OR_SecureDivision(Sum(D.GrossShows), Sum(D.GrossBooks)) as ShowsFactor,
	-- Directas
	Sum(D.Directs) as Directs,
	-- Walk Outs
	Sum(D.WalkOuts) as WalkOuts,
	-- Tours de cortesia
	Sum(D.CourtesyTours) as CourtesyTours,
	-- Tours de rescate
	Sum(D.SaveTours) as SaveTours,
	-- Shows
	Sum(D.Shows) as Shows,
	-- Ventas
	Sum(D.Sales) as Sales_TOTAL,
	-- Monto de ventas
	Sum(D.SalesAmount) as SalesAmount_TOTAL,
	-- Ventas procesables
	Sum(D.Sales - D.SalesOOP + D.SalesCancel) as Sales_PROC,
	-- Monto de ventas procesables
	Sum(D.SalesAmount - D.SalesAmountOOP + D.SalesAmountCancel) as SalesAmount_PROC,
	-- Ventas Out Of Pending
	Sum(D.SalesOOP) as Sales_OOP,
	-- Monto de ventas Out Of Pending
	Sum(D.SalesAmountOOP) as SalesAmount_OOP,
	-- Ventas pendientes
	Sum(D.SalesPending) as Sales_PEND,
	-- Monto de ventas pendientes
	Sum(D.SalesAmountPending) as SalesAmount_PEND,
	-- Ventas canceladas
	Sum(D.SalesCancel) as Sales_CANCEL,
	-- Monto de ventas canceladas
	Sum(D.SalesAmountCancel) as SalesAmount_CANCEL,
	-- Subtotal
	Sum(D.SalesAmount) as Subtotal,
	-- Porcentaje de cancelacion
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmountCancel), Sum(D.SalesAmount + D.SalesAmountCancel)) as CancelFactor,
	-- Porcentaje de cierre
	[dbo].UFN_OR_SecureDivision(Sum(D.Sales), Sum(D.Shows)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Shows)) as Efficiency,
	-- Venta promedio
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Sales)) as AverageSale
from (
	-- Bookings
	select PR, Books, 0 as GrossBooks, 0 as InOuts, 0 as WalkOuts, 0 as CourtesyTours, 0 as SaveTours, 0 as Shows, 0 as GrossShows, 0 as Directs,
		0 as Sales, 0 as SalesAmount, 0 as SalesOOP, 0 as SalesAmountOOP, 0 as SalesPending, 0 as SalesAmountPending, 0 as SalesCancel,
		0 as SalesAmountCancel
	from UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, 0, default, default, default, default, default, default, default)
	-- Bookings netos
	union all
	select PR, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, 0, 0, default, default, default, default, default, default)
	-- In & Outs
	union all
	select PR, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, 1, default, default, default, default,
		default, @BasedOnBooking, default, default, default, default, default, default)
	-- Walk Outs
	union all
	select PR, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, default, 1, default, default, default,
		default, @BasedOnBooking, default, default, default, default, default, default)
	-- Tours de cortesia
	union all
	select PR, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, default, default, 2, default, default,
		default, @BasedOnBooking, default, default, default, default, default, default)
	-- Tours de rescate
	union all
	select PR, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, default, default, 3, default, default,
		default,@BasedOnBooking, default, default, default, default, default, default)
	-- Shows
	union all
	select PR, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, default, default, default, default, 1,
		default, @BasedOnBooking, default, default, default, default, default, default)
	-- Shows netos
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, 1, 0, default, default, default, default,
		default, @BasedOnBooking, default, default, default, default, default, default)
	-- Directas
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, 1, default, default, default, default, default, default, default)
	-- Ventas procesables
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, @FilterDeposit, default, default, @BasedOnBooking, default, default, default, default, default)
	-- Monto de ventas procesables
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, @FilterDeposit, default, default, @BasedOnBooking, default, default, default, default, default)
	-- Ventas Out Of Pending
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, default, @FilterDeposit, default, default, @BasedOnBooking, default, default, default, default, default)
	-- Monto de ventas Out Of Pending
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0
	from UFN_OR_GetPRSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, default, @FilterDeposit, default, default, @BasedOnBooking, default, default, default, default, default)
	-- Ventas pendientes
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0
	from UFN_OR_GetPRSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, @FilterDeposit, 1, default, @BasedOnBooking, default, default, default, default, default)
	-- Monto de ventas pendientes
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0
	from UFN_OR_GetPRSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, @FilterDeposit, 1, default, @BasedOnBooking, default, default, default, default, default)
	-- Ventas canceladas
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetPRSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, 1, @FilterDeposit, default, default, @BasedOnBooking, default, default, default, default, default)
	-- Monto de ventas canceladas
	union all
	select PR, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetPRSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, 1, @FilterDeposit, default, default, @BasedOnBooking, default, default, default, default, default)
) as D
	left join Personnel P on D.PR = P.peID
group by D.PR, P.peN, P.peps
order by Status, SalesAmount_TOTAL desc, Shows desc, Books desc, D.PR
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptProductionByPRSalesRoomOutside]    Script Date: 07/28/2016 14:08:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por PR y sala (Outhouse)
** 
** [wtorres]		30/Oct/2009 Created
** [wtorres]		01/Abr/2010 Modified. No se estaban contando bien las reservaciones netas ni los shows netos
** [wtorres]		16/Abr/2010 Modified. Agregue los parametros @PRs y @Program
** [wtorres]		19/Oct/2011 Modified. Elimine el uso de la funcion UFN_OR_GetPRSalesRoomInOuts
** [wtorres]		16/Nov/2011 Modified. Agregue los campos de Walk Outs, Courtesy Tours y Save Tours.
**								Ahora el numero total de shows solo cuenta los shows con tour o venta y agregue las columnas de ventas pendientes
** [axperez]		03/Dic/2013 Modified. Paso valor defualt para parametro @BasedOnArrival de la funcion UFN_OR_GetPRSalesRoomBookings
** [axperez]		04/Dic/2013 Modified. Paso valor defualt para parametros @ConsiderDirectsAntesInOut, @BasedOnArrival de la funcion UFN_OR_GetPRSalesRoomShows
** [lormartinez]	22/Sep/2014 Modified. Se agrega parametro para filtro basado en fecha de booking
** [wtorres]		28/Jul/2016 Modified. Elimine la funcion UFN_OR_GetPRSalesRoomDirects
**
*/
create procedure [dbo].[USP_OR_RptProductionByPRSalesRoomOutside]
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',		-- Claves de PRs
	@Program varchar(10) = 'ALL',	-- Clave de programa
	@FilterDeposit tinyint = 0,		-- Filtro de depositos:
									--		0. Sin filtro
									--		1. Con deposito (Deposits)
									--		2. Sin deposito (Flyers)
									--		3. Con deposito y shows sin deposito (Deposits & Flyers Show)
	@BasedOnBooking bit = 0			-- Indica si se debe basar en la fecha de booking
as
set nocount on

declare	@FilterDepositAlternate tinyint	-- Filtro de depositos alterno (Aplica para bookings)

set @FilterDepositAlternate = @FilterDeposit
-- si se desea el reporte con deposito y shows sin deposito (Deposits & Flyers Show)
if @FilterDeposit = 3
	set @FilterDeposit = 0	-- Sin filtro

select
	-- Clave del PR
	D.PR,
	-- Nombre del PR
	IsNull(P.peN, 'CODE NOT FOUND!') as PRN,
	-- Estatus del PR
	IsNull(P.peps, 'INACTIVE') as Status,
	-- Clave de la sala
	D.SalesRoom as SalesRoomID,
	-- Descripcion de la sala
	S.srN as SalesRoomN,
	-- Bookings
	Sum(D.Books) as Books,
	-- In & Outs
	Sum(D.InOuts) as InOuts,
	-- Bookings netos
	Sum(D.GrossBooks) as GrossBooks,
	-- Shows netos
	Sum(D.GrossShows) as GrossShows,
	-- Porcentaje de shows
	[dbo].UFN_OR_SecureDivision(Sum(D.GrossShows), Sum(D.GrossBooks)) as ShowsFactor,
	-- Directas
	Sum(D.Directs) as Directs,
	-- Walk Outs
	Sum(D.WalkOuts) as WalkOuts,
	-- Tours de cortesia
	Sum(D.CourtesyTours) as CourtesyTours,
	-- Tours de rescate
	Sum(D.SaveTours) as SaveTours,
	-- Shows
	Sum(D.Shows) as Shows,
	-- Ventas
	Sum(D.Sales) as Sales_TOTAL,
	-- Monto de ventas
	Sum(D.SalesAmount) as SalesAmount_TOTAL,
	-- Ventas procesables
	Sum(D.Sales - D.SalesOOP + D.SalesCancel) as Sales_PROC,
	-- Monto de ventas procesables
	Sum(D.SalesAmount - D.SalesAmountOOP + D.SalesAmountCancel) as SalesAmount_PROC,
	-- Ventas Out Of Pending
	Sum(D.SalesOOP) as Sales_OOP,
	-- Monto de ventas Out Of Pending
	Sum(D.SalesAmountOOP) as SalesAmount_OOP,
	-- Ventas pendientes
	Sum(D.SalesPending) as Sales_PEND,
	-- Monto de ventas pendientes
	Sum(D.SalesAmountPending) as SalesAmount_PEND,
	-- Ventas canceladas
	Sum(D.SalesCancel) as Sales_CANCEL,
	-- Monto de ventas canceladas
	Sum(D.SalesAmountCancel) as SalesAmount_CANCEL,
	-- Subtotal
	Sum(D.SalesAmount) as Subtotal,
	-- Porcentaje de cancelacion
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmountCancel), Sum(D.SalesAmount + D.SalesAmountCancel)) as CancelFactor,
	-- Porcentaje de cierre
	[dbo].UFN_OR_SecureDivision(Sum(D.Sales), Sum(D.Shows)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Shows)) as Efficiency,
	-- Venta promedio
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Sales)) as AverageSale
from (
	-- Bookings
	select PR, SalesRoom, Books, 0 as GrossBooks, 0 as InOuts, 0 as WalkOuts, 0 as CourtesyTours, 0 as SaveTours, 0 as Shows, 0 as GrossShows,
		0 as Directs, 0 as Sales, 0 as SalesAmount, 0 as SalesOOP, 0 as SalesAmountOOP, 0 as SalesPending, 0 as SalesAmountPending,
		0 as SalesCancel, 0 as SalesAmountCancel
	from UFN_OR_GetPRSalesRoomBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, 0, default, default)
	-- Bookings netos
	union all
	select PR, SalesRoom, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, 0, 0, default)
	-- In & Outs
	union all
	select PR, SalesRoom, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, 1, default, default, default, 
		default, default, @BasedOnBooking)
	-- Walk Outs
	union all
	select PR, SalesRoom, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, default, 1, default, default, 
		default, default, @BasedOnBooking)
	-- Tours de cortesia
	union all
	select PR, SalesRoom, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, default, default, 2, default, 
		default, default, @BasedOnBooking)
	-- Tours de rescate
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, default, default, 3, default, 
		default, default, @BasedOnBooking)
	-- Shows
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, default, default, default, default, default, 
		1, default, @BasedOnBooking)
	-- Shows netos
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, 1, 0, default, default, default, 
		default, default, @BasedOnBooking)
	-- Directas
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, 1, default, default)
	-- Ventas procesables
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, @FilterDeposit, default, default, @BasedOnBooking )
	-- Monto de ventas procesables
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, @FilterDeposit, default, 
		default, @BasedOnBooking)
	-- Ventas Out Of Pending
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, default, @FilterDeposit, default, default, @BasedOnBooking )
	-- Monto de ventas Out Of Pending
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0, 0, 0
	from UFN_OR_GetPRSalesRoomSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, 1, default, @FilterDeposit, default, 
		default, @BasedOnBooking)
	-- Ventas pendientes
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0
	from UFN_OR_GetPRSalesRoomSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, @FilterDeposit, 1, default, @BasedOnBooking)
	-- Monto de pendientes
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0
	from UFN_OR_GetPRSalesRoomSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, @FilterDeposit, 1, 
		default, @BasedOnBooking)
	-- Ventas canceladas
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetPRSalesRoomSales(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, 1, @FilterDeposit, default, default, @BasedOnBooking )
	-- Monto de ventas canceladas
	union all
	select PR, SalesRoom, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetPRSalesRoomSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, 1, @FilterDeposit, default, 
		default, @BasedOnBooking)
) as D
	left join Personnel P on D.PR = P.peID
	left join SalesRooms S on D.SalesRoom = S.srID
group by D.PR, P.peN, P.peps, D.SalesRoom, S.srN
order by D.SalesRoom, Status, SalesAmount_TOTAL desc, Shows desc, Books desc, D.PR


GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptPRStats]    Script Date: 07/28/2016 14:08:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene las estadisticasdel modulo PRStats
** 
** [erosado]	01/Mar/2016 Created
** [lchairez]	18/Abr/2016 Modified. Agregué el parámetro @BasedOnPrLocation en la función UFN_OR_GetPRSales
** [lchairez]	18/Abr/2016 Modified. Agregué el parámetro @BasedOnPrLocation en la función UFN_OR_GetPRSalesAmount
**
*/
create procedure [dbo].[USP_OR_RptPRStats] 
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(max) = 'ALL',	-- Clave de los Lead Sources
	@SalesRooms varchar(max) = 'ALL',	-- Clave de las salas de ventas
	@Countries varchar(max) = 'ALL',	-- Clave de los paises
	@Agencies varchar(max) = 'ALL',		-- Clave de las agencias
	@Markets varchar(max) = 'ALL'		-- Clave de los mercados
as
set nocount on;

SELECT
	-- PR ID
	D.PR AS 'PR_ID',
	-- PR Names
	P.peN AS 'PR_NAME',
	-- Assigns
	Sum(Assigns) AS Assign,
	-- Contacts
	SUM(D.Contacts) AS Conts,
	-- Contacts Factor (Contacts / Assigns)
	dbo.UFN_OR_SecureDivision(SUM(D.Contacts),SUM(D.Assigns)) AS 'C_Factor',
	-- Availables
	SUM(D.Availables)AS Avails,
	-- Availables Factor (Availables / Contacts)
	dbo.UFN_OR_SecureDivision(SUM(D.Availables), SUM(D.Contacts))  AS 'A_Factor',
	-- Bookings Netos (Sin Directas)
	SUM(D.GrossBooks) AS Bk,
	-- Bookings Factor (Books / Availables)
	dbo.UFN_OR_SecureDivision(SUM(D.Books), SUM(D.Availables)) AS 'Bk_Factor',
	-- Deposits (Bookings)
	SUM(D.Deposits) AS Dep,
	-- 	Directs (Bookings)
	SUM(D.Directs) AS Dir,
	-- Shows Netos (Shows WithOut Directs Without In & Outs)
	SUM(D.GrossShows) AS Sh,
	--	In & Outs (Shows)
	SUM(D.InOuts) AS 'IO',
	-- Shows Factor (Shows / Bookings Netos)
	dbo.UFN_OR_SecureDivision(SUM(D.Shows), SUM(D.GrossBooks)) AS 'Sh_Factor',
	-- Total Shows
	SUM(D.Shows) AS 'TSh',
	-- Self Gen Tours (Guests)
	SUM(D.SelfGenShows) AS SG,
	-- Processable Number
	SUM(D.ProcessableNumber) AS	'Proc_Number',	
	-- Processable Amount
	SUM(D.ProcessableAmount) - SUM(D.OutPendingAmount) AS Processable,
	-- Out Pending Number 
	SUM(D.OutPendingNumber) AS	'OutP_Number',
	-- Out Pending Amount 
	SUM(D.OutPendingAmount) AS	'Out_Pending',
	-- Cancelled Number 
	SUM(D.CancelledNumber) AS	'C_Number',
	-- Cancelled Amount 
	SUM(D.CancelledAmount) AS	'Cancelled',
	-- Total Number
	SUM(D.ProcessableNumber) AS 'Total_Number',
	-- Total Amount
	SUM(D.ProcessableAmount) AS Total,
	-- Proc PR Number
	SUM(D.ProcessableNumber) - SUM(D.SelfGenNumber) AS 'Proc_PR_Number',
	-- Proc PR Amount
	SUM(D.ProcessableAmount) - SUM(D.SelfGenAmount) AS 'Proc_PR',
	-- Proc SG Number(ConsidererSelfGen=1)	
	SUM(D.SelfGenNumber)AS 'Proc_SG_Number',
	-- Proc SG Amount (ConsidererSelfgen=1)
	SUM(D.SelfGenAmount)AS 'Proc_SG',
	-- Efficient
	dbo.UFN_OR_SecureDivision(SUM(D.ProcessableAmount),SUM(D.Shows)) AS Eff,
	-- Clossing Factor
	dbo.UFN_OR_SecureDivision(SUM(D.ProcessableNumber),SUM(D.Shows)) AS 'Cl_Factor',
	-- Canceladas Factor
	dbo.UFN_OR_SecureDivision(SUM(D.CancelledAmount),SUM(D.ProcessableAmount)) AS 'Ca_Factor',
	-- Avg Sale
	dbo.UFN_OR_SecureDivision(SUM(D.ProcessableAmount),SUM(D.ProcessableNumber)) AS 'Avg_Sale'
FROM(
	-- Asignaciones
	SELECT 
	PR							/*1*/
	--	PR Name Join Personnel
	,Assigns					/*2*/
	,0 AS Contacts				/*3*/
	-- Contacts Factor	
	,0 AS Availables			/*4*/
	-- Availables Factor
	,0 AS GrossBooks			/*5*/
	-- Bookings Factor
	,0 AS Books					/*6*/
	,0 AS Deposits				/*7*/
	-- Shows Factor
	,0 AS Directs				/*8*/
	,0 AS GrossShows			/*9*/
	,0 AS InOuts				/*10*/
	,0 AS Shows					/*11*/
	,0 AS SelfGenShows			/*12*/
	,0 AS ProcessableNumber		/*13*/
	,0 AS ProcessableAmount		/*14*/
	,0 AS OutPendingNumber		/*15*/
	,0 AS OutPendingAmount		/*16*/
	,0 AS CancelledNumber		/*17*/
	,0 AS CancelledAmount		/*18*/
	-- Total Number
	-- Total Amount 	
	,0 AS SelfGenNumber			/*19*/
	,0 AS SelfGenAmount			/*20*/
	-- Efficient Factor
	-- Closing Factor
	-- Cancelled Factor
	-- Avg Sales
	
	FROM dbo.UFN_OR_GetPRAssigns(@DateFrom, @DateTo, @LeadSources, @SalesRooms, @Countries, @Agencies, @Markets)
	-- Contacts
	UNION ALL
	SELECT PR,0,Contacts,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	FROM UFN_OR_GetPRContacts(@DateFrom, @DateTo, @LeadSources, DEFAULT, DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Availables
	UNION ALL
	SELECT PR,0,0,Availables,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	FROM UFN_OR_GetPRAvailables(@DateFrom, @DateTo, @LeadSources, DEFAULT, DEFAULT, DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Bookings Netos (Sin Directas)
	UNION ALL
	SELECT PR,0,0,0,Books,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	FROM UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, DEFAULT, DEFAULT, DEFAULT, DEFAULT,0
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Bookings
	UNION ALL
	SELECT PR,0,0,0,0,Books,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	FROM UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, DEFAULT, DEFAULT, DEFAULT, DEFAULT,DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Depositos
	UNION ALL
	SELECT PR,0,0,0,0,0,Books,0,0,0,0,0,0,0,0,0,0,0,0,0
	FROM UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, DEFAULT, DEFAULT, DEFAULT,1,DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Directos
	UNION ALL
	SELECT PR,0,0,0,0,0,0,Books,0,0,0,0,0,0,0,0,0,0,0,0
	FROM UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, DEFAULT, DEFAULT, DEFAULT,DEFAULT,1
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Shows Netos
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,Shows,0,0,0,0,0,0,0,0,0,0,0
	FROM dbo.UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,DEFAULT,DEFAULT,1,0
	,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- In & Outs
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,Shows,0,0,0,0,0,0,0,0,0,0
	FROM UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,1
	,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Shows
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,0,Shows,0,0,0,0,0,0,0,0,0
	FROM dbo.UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT, DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Self Gen Shows
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,0,0,Shows,0,0,0,0,0,0,0,0
	FROM dbo.UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT, DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,1,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Number Processable 
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,0,0,0,Sales,0,0,0,0,0,0,0
	FROM dbo.UFN_OR_GetPRSales(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT, DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)	
	-- Amount Processable 
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,0,0,0,0,SalesAmount,0,0,0,0,0,0
	FROM dbo.UFN_OR_GetPRSalesAmount(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT, DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)	
	-- Number Out Of Pending 
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,0,0,0,0,0,Sales,0,0,0,0,0
	FROM dbo.UFN_OR_GetPRSales(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,DEFAULT,1,DEFAULT,DEFAULT,DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)	
	-- Amount Out Of Pending	
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,0,0,0,0,0,0,SalesAmount,0,0,0,0
	FROM dbo.UFN_OR_GetPRSalesAmount(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,DEFAULT,1,DEFAULT,DEFAULT,DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Number Cancelled
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,Sales,0,0,0
	FROM dbo.UFN_OR_GetPRSales(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,DEFAULT,DEFAULT,1,DEFAULT,DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Amount Cancelled
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,SalesAmount,0,0
	FROM dbo.UFN_OR_GetPRSalesAmount(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,DEFAULT,DEFAULT,1,DEFAULT,DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Number Self Gen
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,Sales,0
	FROM dbo.UFN_OR_GetPRSales(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,1,DEFAULT,DEFAULT,DEFAULT,DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Amount Self Gen
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,SalesAmount
	FROM dbo.UFN_OR_GetPRSalesAmount(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,1,DEFAULT,DEFAULT,DEFAULT,DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	
)AS D
	LEFT JOIN Personnel P ON D.PR = P.peID
GROUP BY PR, P.peN
ORDER BY P.peN

GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptScoreByPR]    Script Date: 07/28/2016 14:08:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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
GO


