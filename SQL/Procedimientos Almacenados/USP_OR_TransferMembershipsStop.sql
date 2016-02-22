if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferMembershipsStop]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferMembershipsStop]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Indica que la transferencia de membresias ha terminado
**
** [wtorres]	29/May/2013 Creado
**
*/
create procedure [dbo].[USP_OR_TransferMembershipsStop]
as
set nocount on

update osConfig
set ocRunTransferMemberships = 0,
	ocTransferMembershipsEndD = GetDate()

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

