if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferUpdateAgenciesNames]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferUpdateAgenciesNames]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Actualiza las descripciones de las agencias en el proceso de transferencia
** 
** [wtorres]	26/Dic/2011 Creado
**
*/
create procedure [dbo].[USP_OR_TransferUpdateAgenciesNames]
as

update Agencies
set agN = tagN
from Agencies
	inner join osTransfer on agID = tagID
where agN <> tagN
	and tagN <> ''
	and tagN <> agID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

