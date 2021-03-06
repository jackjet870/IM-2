if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferUpdateTransferUnavailableMotivesByGroups]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferUpdateTransferUnavailableMotivesByGroups]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Procedimiento:	Actualizar motivo de indisponibilidad de reservaciones migradas por grupos en el proceso de transferencia
-- Descripción:		Actualiza el motivo de indisponibilidad de reservaciones migradas por grupos (2 = WITH GROUP)
-- Histórico:		[wtorres] 22/Abr/2009 Creado
-- =============================================
create procedure [dbo].[USP_OR_TransferUpdateTransferUnavailableMotivesByGroups]	
as
set nocount on;

update osTransfer
set tum = 2
where tOnGroup = 1 and tum = 0

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

