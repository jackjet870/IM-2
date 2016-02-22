if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferStopZone]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferStopZone]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Indica que la transferencia de reservaciones de una zona ha terminado
**
** [wtorres]	13/Jul/2010 Creado
** [wtorres]	02/Feb/2011 Ahora en el catalogo de zonas se definen las zonas de transferencia
**
*/
create procedure [dbo].[USP_OR_TransferStopZone]
	@Zone varchar(10)	-- Clave de la zona
as
set nocount on

update Zones
set znRunTrans = 0
where znID = @Zone

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

