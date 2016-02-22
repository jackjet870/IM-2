if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferUpdateContractsNames]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferUpdateContractsNames]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Actualiza las descripciones de los tipos de habitacion en el proceso de transferencia
** 
** [wtorres]	02/Feb/2012 Creado
**
*/
create procedure [dbo].[USP_OR_TransferUpdateContractsNames]
as

update Contracts
set cnN = tcnN
from Contracts
	inner join osTransfer on cnID = tO1
where cnN <> tcnN
	and tcnN <> ''
	and tcnN <> cnID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

