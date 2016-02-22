if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferStart]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferStart]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Indica que la transferencia de reservaciones ha iniciado
**
** [wtorres]	13/Jul/2010 Created
** [wtorres]	02/Feb/2011 Modified. Ahora en el catalogo de zonas se definen las zonas de transferencia
** [wtorres]	10/May/2013 Modified. Elimine el campo ocStartD de la consulta porque no se utiliza e hice que se actualice para saber cuando
**							inicio la ultima transferencia de reservaciones
** [wtorres]	28/Feb/2015 Modified. Ahora solo tomo las zonas activas
**
*/
create procedure [dbo].[USP_OR_TransferStart]
as
set nocount on

-- obtenemos los parametros de configuracion
select ocOneNightV, ocTwoNightV from osConfig

-- indicamos que esta transfiriendo
update osConfig
set ocRunTrans = 1,
	ocStartD = GetDate()

-- indicamos que las zonas estan transfiriendo
update Zones
set znRunTrans = 1
where znZoneHotel is not null and znA = 1 and znRunTrans = 0

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

