if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetLeadSourcesByZoneBoss]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetLeadSourcesByZoneBoss]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene los Lead Sources dada una zona y el patron configurado
** 
** [wtorres]	22/Nov/2011 Creado
**
*/
create procedure [dbo].[USP_OR_GetLeadSourcesByZoneBoss]
	@Zone varchar(10)	-- Clave de la zona
as
set nocount on

declare @Boss varchar(10)	-- Clave del patron

select @Boss = ocBoss from osConfig

-- si no tiene patron definido traemos todos los hoteles de la zona
if @Boss is null
	set @Boss = 'ALL'

select
	lsID,
	lsN
from LeadSources
where
	-- Zona
	lszn = @Zone
	-- Patron
	and (@Boss = 'ALL' or lsBoss = @Boss)
order by lsID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

