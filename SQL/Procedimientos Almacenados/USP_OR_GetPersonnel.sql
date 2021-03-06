if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetPersonnel]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetPersonnel]
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


