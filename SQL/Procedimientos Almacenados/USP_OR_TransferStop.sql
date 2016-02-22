if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferStop]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferStop]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Indica que la transferencia de reservaciones ha terminado
**
** [wtorres]	13/Jul/2010 Creado
**
*/
create procedure [dbo].[USP_OR_TransferStop]
as
set nocount on

update osConfig
set ocRunTrans = 0,
	ocTransDT = GetDate()

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

