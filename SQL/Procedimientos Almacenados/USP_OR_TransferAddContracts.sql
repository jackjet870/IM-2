if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferAddContracts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferAddContracts]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Agrega los contratos en el proceso de transferencia
**
** [wtorres] 	02/Feb/2012 Creado
**
*/
create procedure [dbo].[USP_OR_TransferAddContracts]	
as
set nocount on

-- a los contratos sin descripcion les pone como descripcion su clave
update osTransfer
set tcnN = tO1
where tcnN is null or tcnN = ''

-- agregamos los contratos
insert into Contracts (cnID, cnN, cnA, cnum)
select distinct tO1, tO1, 1, 0
from osTransfer
where tO1 <> '' and not exists
	(select cnID from Contracts where cnID = tO1)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

