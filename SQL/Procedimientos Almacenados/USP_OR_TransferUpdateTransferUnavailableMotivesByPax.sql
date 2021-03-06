if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferUpdateTransferUnavailableMotivesByPax]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferUpdateTransferUnavailableMotivesByPax]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Actualiza el motivo de indisponibilidad de reservaciones migradas por pax (35 - PAX)
**
** [wtorres]	15/May/2012	Created
**
*/
create procedure [dbo].[USP_OR_TransferUpdateTransferUnavailableMotivesByPax]
as
set nocount on

update osTransfer
set tum = 35
where tPax = 1

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

