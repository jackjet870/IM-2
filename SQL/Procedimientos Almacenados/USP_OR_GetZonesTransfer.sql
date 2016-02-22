if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetZonesTransfer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetZonesTransfer]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene las zonas de transferencia
**
** [wtorres]	30/Jun/2010 Created
** [wtorres]	12/Jul/2010 Modified. Ahora los Lead Sources de transferencia son los que tienen zona
** [wtorres]	26/Nov/2011 Modified. Agregue el campo lszn
** [wtorres]	02/Feb/2011 Modified. Ahora en el catalogo de zonas se definen las zonas de transferencia.
**							Renombre el procedimiento almacenado. Antes se llamaba USP_OR_GetLeadSourcesTransfer
** [wtorres]	28/Feb/2015 Modified. Ahora solo tomo las zonas activas
**
*/
create procedure [dbo].[USP_OR_GetZonesTransfer] 
as
set nocount on

select
	znID,
	znN,
	znZoneHotel
from Zones
where
	-- Configurado para transferir
	znZoneHotel is not null
	-- Corriendo la transferencia
	and znRunTrans = 1
	-- Zonas activas
	and znA = 1
order by znID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

