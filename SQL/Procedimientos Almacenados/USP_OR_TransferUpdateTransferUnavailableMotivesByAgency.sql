if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferUpdateTransferUnavailableMotivesByAgency]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferUpdateTransferUnavailableMotivesByAgency]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Procedimiento:	Actualizar motivo de indisponibilidad de reservaciones migradas por agencia en el proceso de transferencia
-- Descripción:		Actualiza el motivo de indisponibilidad de reservaciones migradas por agencia
-- Histórico:		[wtorres] 22/Abr/2009 Creado
-- =============================================
create procedure [dbo].[USP_OR_TransferUpdateTransferUnavailableMotivesByAgency]	
as
set nocount on
  
update osTransfer
set tum = agum
from osTransfer
	inner join Agencies on toagID = agID
where tum = 0 and agum > 0

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

