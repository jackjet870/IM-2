if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferUpdateTransferLanguages]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferUpdateTransferLanguages]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Procedimiento:	Actualizar idiomas de reservaciones migradas en el proceso de transferencia
-- Descripción:		Actualiza los idiomas de las reservaciones migradas
-- Histórico:		[wtorres] 22/Abr/2009 Creado
-- =============================================
create procedure [dbo].[USP_OR_TransferUpdateTransferLanguages]	
as
set nocount on

update osTransfer
set tla = cola
from osTransfer
	inner join Countries on tocoID = coID
where tla <> cola

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

