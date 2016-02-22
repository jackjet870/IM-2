if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferUpdateCountriesNames]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferUpdateCountriesNames]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Actualiza las descripciones de los paises en el proceso de transferencia
** 
** [wtorres]	25/Ene/2012 Creado
**
*/
create procedure [dbo].[USP_OR_TransferUpdateCountriesNames]
as

update Countries
set coN = tcoN
from Countries
	inner join osTransfer on coID = tcoID
where coN <> tcoN
	and tcoN <> ''
	and tcoN <> coID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

