if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferUpdateTransferUnavailableMotivesByContract]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferUpdateTransferUnavailableMotivesByContract]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Actualiza el motivo de indisponibilidad de reservaciones migradas por contrato en el proceso de transferencia
** 
** [wtorres]	05/Sep/2011 Creado
**
*/
create procedure [dbo].[USP_OR_TransferUpdateTransferUnavailableMotivesByContract]	
as
set nocount on
  
update osTransfer
set tum = cnum
from osTransfer
	inner join Contracts on tO1 = cnID
where tum = 0 and cnum > 0

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

