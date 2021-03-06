if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_ValidateGuestsGroupsSize]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_ValidateGuestsGroupsSize]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Procedimiento:	Validar tamaño de grupos de huéspedes
-- Descripción:		Valida el tamaño (número de integrantes) de los grupos de huéspedes, es decir, valida que no queden grupos de menos de
--					2 integrantes
--					Devuelve:
--						1. Los grupos que quedarían con menos de 2 integrantes
--						2. Los integrantes de esos grupos
-- Histórico:		[wtorres] 24/Jul/2010 Creado
-- =============================================
create procedure [dbo].[USP_OR_ValidateGuestsGroupsSize]
	@Group int,					-- Clave del grupo
	@Integrants varchar(8000)	-- Lista de integrantes
as
set nocount on

-- Tabla de integrantes
declare @TableIntegrants table (
	Integrant int
)

-- Tabla de grupos
declare @TableGroups table (
	[Group] int,
	GroupN varchar(50),
	IntegrantsRemaining int
)

-- Obtiene los integrantes
insert @TableIntegrants
select item from split(@Integrants, ',')

-- Grupos
-- =============================================
insert @TableGroups
select
	D.gjgx,
	D.gxN,
	Sum(D.Integrants - D.IntegrantsToDelete) as IntegrantsRemaining
from (
	-- Integrantes
	select
		I.gjgx,
		G.gxN,
		Count(*) as Integrants,
		0 as IntegrantsToDelete
	from GuestsGroups G
		inner join GuestsGroupsIntegrants I on I.gjgx = G.gxID 
	where
		-- Grupo
		I.gjgx in (
			select distinct gjgx
			from GuestsGroupsIntegrants
			where
				-- Integrantes
				gjgu in (select Integrant from @TableIntegrants)
				-- No sea el grupo que se va a guardar
				and gjgx <> @Group
		)
	group by I.gjgx, G.gxN

	-- Integrantes que ya no estarían en el grupo
	union all
	select
		I.gjgx,
		G.gxN,
		0,
		Count(*)
	from GuestsGroups G
		inner join GuestsGroupsIntegrants I on I.gjgx = G.gxID 
	where
		-- Grupo
		I.gjgx in (
			select distinct gjgx
			from GuestsGroupsIntegrants
			where
				-- Integrantes
				gjgu in (select Integrant from @TableIntegrants)
				-- No sea el grupo que se va a guardar
				and gjgx <> @Group
		)
		-- Integrantes del grupo que se va a guardar
		and I.gjgu in (select Integrant from @TableIntegrants)
	group by I.gjgx, G.gxN
) as D
group by D.gjgx, D.gxN
-- Que queden con menos de 2 integrantes
having Sum(D.Integrants - D.IntegrantsToDelete) < 2

-- 1. Grupos
-- =============================================
select * from @TableGroups

-- 1. Integrantes
-- =============================================
select
	I.gjgx as [Group],
	I.gjgu as [Integrant],
	dbo.UFN_OR_GetFullName(G.guLastName1, G.guFirstName1) as IntegrantN
from Guests G
	inner join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- Integrantes de los grupos con menos de 2 integrantes
	I.gjgx in (select [Group] from @TableGroups)
	-- Integrantes
	and I.gjgu in (select Integrant from @TableIntegrants)	

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

