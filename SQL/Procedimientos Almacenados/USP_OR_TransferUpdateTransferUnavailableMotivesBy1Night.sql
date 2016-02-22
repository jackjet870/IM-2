if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferUpdateTransferUnavailableMotivesBy1Night]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferUpdateTransferUnavailableMotivesBy1Night]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Procedimiento:	Actualizar motivo de indisponibilidad de reservaciones migradas por 1 noche en el proceso de transferencia
-- Descripción:		Actualiza el motivo de indisponibilidad de reservaciones migradas por 1 noche (1 - JUST ONE NIGHT)
-- Histórico:		[wtorres] 22/Abr/2009 Creado
-- =============================================
create procedure [dbo].[USP_OR_TransferUpdateTransferUnavailableMotivesBy1Night] 
as
set nocount on

update osTransfer
set tum = 1
where tCheckOutD <= DateAdd(day, 1, tCheckInD)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

