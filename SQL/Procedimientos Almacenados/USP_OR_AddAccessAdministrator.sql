if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_AddAccessAdministrator]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_AddAccessAdministrator]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Agrega el acceso al usuario administrador a un nuevo Lead Source, sala de ventas o almacen
**
** [ejz]		24/Ene/2007 Created
** [wtorres]	12/Ago/2014 Modified. Renombrado. Antes se llamaba spAddAdminLSSR
**
*/
create procedure [dbo].[USP_OR_AddAccessAdministrator]
	@PlaceType varchar(2)	-- Clave del tipo de lugar
as
set nocount on

declare @User varchar(10)

-- obtenemos el usuario administrador
select @User = ocAdminUser from osConfig

if @User is not null
	
	-- Lead Sources
	if @PlaceType = 'LS'
		insert into PersLSSR (plpe, plLSSR, plLSSRID)
		select @User, @PlaceType, lsID
		from Leadsources
		where not exists (select plpe from PersLSSR where plpe = @User and plLSSR = @PlaceType and plLSSRID = lsID)
	
	-- Salas de ventas
	else if @PlaceType = 'SR'
		insert into PersLSSR (plpe, plLSSR, plLSSRID)
		select @User, @PlaceType, srID
		from SalesRooms
		where not exists (select plpe from PersLSSR where plpe = @User and plLSSR = @PlaceType and plLSSRID = srID)
	
	-- Almacenes
	else if @PlaceType = 'WH'
		insert into PersLSSR (plpe, plLSSR, plLSSRID)
		select @User, @PlaceType, whID
		from Warehouses
		where not exists (select plpe from PersLSSR where plpe = @User and plLSSR = @PlaceType and plLSSRID = whID)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

