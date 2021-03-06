if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetPersonnelByTeamType]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetPersonnelByTeamType]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene el personal de un lugar
** 
** [wtorres]	11/Mar/2009 Created
** [lchairez]	28/Nov/2013 Modified. Agregue el parametro @PlaceType
** [gmaya]		20/Jun/2014 Modified. Cambie el tipo del parametro @PlaceType
** [wtorres]	02/Jul/2014 Modified. Renombrado. Antes se llamaba USP_OR_ObtenerVendedoresPorSalaVentas
**
*/
create procedure [dbo].[USP_OR_GetPersonnelByTeamType]
	@TeamType varchar(10),	-- Tipo de equipo
	@Place varchar(10)		-- Clave del lugar
as
set nocount on
	
select peID, peN
from Personnel
where
	-- Tipo de equipo
	peTeamType = @TeamType
	-- Clave del lugar
	and pePlaceID = @Place
	-- Personal activo
	and peA = 1
order by peN

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

