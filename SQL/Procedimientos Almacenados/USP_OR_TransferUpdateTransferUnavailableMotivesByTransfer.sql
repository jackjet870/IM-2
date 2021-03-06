if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferUpdateTransferUnavailableMotivesByTransfer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferUpdateTransferUnavailableMotivesByTransfer]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Procedimiento:	Actualizar motivo de indisponibilidad de reservaciones migradas por transferencia en el proceso de transferencia
-- Descripción:		Actualiza el motivo de indisponibilidad de reservaciones migradas por transferencia (24 = TRANSFER)
-- Histórico:		[wtorres] 13/Jul/2009 Creado
-- =============================================
create procedure [dbo].[USP_OR_TransferUpdateTransferUnavailableMotivesByTransfer]	
as
set nocount on

update osTransfer
set tum = 24
where tReservationType = 'T'

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

