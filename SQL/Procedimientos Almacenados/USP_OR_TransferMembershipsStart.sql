if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferMembershipsStart]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferMembershipsStart]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Indica que la transferencia de membresias ha iniciado
**
** [wtorres]	10/May/2013 Creado
**
*/
create procedure [dbo].[USP_OR_TransferMembershipsStart]
as
set nocount on

update osConfig
	set ocRunTransferMemberships = 1,
	ocTransferMembershipsStartD = GetDate()

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

