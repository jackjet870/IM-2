if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferUpdateGroupsNames]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferUpdateGroupsNames]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Actualiza las descripciones de los grupos en el proceso de transferencia
** 
** [lchairez]	23/Abr/2014 Created
**
*/
CREATE PROCEDURE [dbo].[USP_OR_TransferUpdateGroupsNames]
AS

UPDATE Groups
set gbN = tgroupN
from Groups
	inner join osTransfer on gbID = tgroupID
where gbN <> tgroupN
	and tgroupN <> ''
	and tgroupN <> gbID
