if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetLeadSourcesByUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetLeadSourcesByUser]
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


