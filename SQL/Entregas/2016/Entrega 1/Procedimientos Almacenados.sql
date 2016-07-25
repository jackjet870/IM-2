USE [OrigosVCPalace]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetGifts]    Script Date: 07/25/2016 10:58:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_GetGifts]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_GetGifts]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetLeadSourcesByUser]    Script Date: 07/25/2016 10:58:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_GetLeadSourcesByUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_GetLeadSourcesByUser]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetWarehousesByUser]    Script Date: 07/25/2016 10:58:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_GetWarehousesByUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_GetWarehousesByUser]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetWhsMovs]    Script Date: 07/25/2016 10:58:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_GetWhsMovs]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_GetWhsMovs]
GO

USE [OrigosVCPalace]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetGifts]    Script Date: 07/25/2016 10:58:25 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_OR_GetLeadSourcesByUser]    Script Date: 07/25/2016 10:58:26 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_OR_GetWarehousesByUser]    Script Date: 07/25/2016 10:58:27 ******/
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

/****** Object:  StoredProcedure [dbo].[USP_OR_GetWhsMovs]    Script Date: 07/25/2016 10:58:28 ******/
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


