USE [OrigosVCPalace]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetGifts]    Script Date: 07/26/2016 13:36:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_GetGifts]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_GetGifts]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetLeadSourcesByUser]    Script Date: 07/26/2016 13:36:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_GetLeadSourcesByUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_GetLeadSourcesByUser]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetPersonnel]    Script Date: 07/26/2016 13:36:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_GetPersonnel]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_GetPersonnel]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetSalesByPR]    Script Date: 07/26/2016 13:36:37 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_GetSalesByPR]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_GetSalesByPR]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetSalesRoomsByUser]    Script Date: 07/26/2016 13:36:37 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_GetSalesRoomsByUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_GetSalesRoomsByUser]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetWarehousesByUser]    Script Date: 07/26/2016 13:36:38 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_GetWarehousesByUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_GetWarehousesByUser]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetWhsMovs]    Script Date: 07/26/2016 13:36:38 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_GetWhsMovs]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_GetWhsMovs]
GO

USE [OrigosVCPalace]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetGifts]    Script Date: 07/26/2016 13:36:38 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_OR_GetLeadSourcesByUser]    Script Date: 07/26/2016 13:36:40 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_OR_GetPersonnel]    Script Date: 07/26/2016 13:36:41 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_OR_GetSalesByPR]    Script Date: 07/26/2016 13:36:43 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_OR_GetSalesRoomsByUser]    Script Date: 07/26/2016 13:36:45 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_OR_GetWarehousesByUser]    Script Date: 07/26/2016 13:36:46 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_OR_GetWhsMovs]    Script Date: 07/26/2016 13:36:47 ******/
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


