if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_Login]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_Login]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Determina si un usuario puede autenticarse
** Devuelve los datos del usuario, sus roles , permisos y el lugar
**
** [wtorres]	24/Ene/2014	Created
** [wtorres]	24/Feb/2015 Modified. Ahora devuelve los datos del lugar en un SELECT aparte
**
*/
create procedure [dbo].[USP_OR_Login]
	@LoginType tinyint,		-- Tipo de autenticacion
							--		0. Normal
							--		1. Sala de ventas
							--		2. Locacion
							--		3. Alamacen
	@User varchar(10),		-- Clave de usuario
	@Place varchar(10) = ''	-- Clave del lugar
as
set nocount on

-- Usuario
select peID, peN, peA, pePwd, pePwdDays, pePwdD
from Personnel
where peID = @User

-- Roles
select PR.prro, R.roN
from PersonnelRoles PR
	left join Roles R on R.roID = PR.prro
where PR.prpe = @User
order by R.roN

-- Permisos
select
	-- Permiso
	PP.pppm, PM.pmN,
	-- Nivel de permiso
	PP.pppl, L.plN
from PersonnelPermissions PP
	left join Permissions PM on PM.pmID = PP.pppm
	left join PermissionsLevels L on L.plID = PP.pppl
where PP.pppe = @User
order by PM.pmN

-- Sala de ventas
select S.srID, S.srN, S.srcu, S.srHoursDif, S.srAddOut
from Personnel P
	inner join PersLSSR PL on PL.plpe = P.peID
	inner join SalesRooms S on S.srID = PL.plLSSRID
where
	@LoginType = 1
	and P.peID = @User
	and PL.plLSSR = 'SR' and S.srID = @Place
		
-- Locacion
select LO.loID, LO.loN, LO.losr
from Personnel P
	inner join PersLSSR PL on PL.plpe = P.peID
	inner join LeadSources L on L.lsID = PL.plLSSRID
	inner join Locations LO on Lo.lols = L.lsID
where
	@LoginType = 2
	and P.peID = @User
	and PL.plLSSR = 'LS' and LO.loID = @Place

-- Lead Source
select L.lsID, L.lsN, L.lspg, L.lsHoursDif
from Personnel P
	inner join PersLSSR PL on PL.plpe = P.peID
	inner join LeadSources L on L.lsID = PL.plLSSRID
	inner join Locations LO on Lo.lols = L.lsID
where
	@LoginType = 2
	and P.peID = @User
	and PL.plLSSR = 'LS' and LO.loID = @Place

-- Almacen
select W.whID, W.whN
from Personnel P
	inner join PersLSSR PL on PL.plpe = P.peID
	inner join Warehouses W on W.whID = PL.plLSSRID
where @LoginType = 3
	and P.peID = @User
	and PL.plLSSR = 'WH' and W.whID = @Place

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

