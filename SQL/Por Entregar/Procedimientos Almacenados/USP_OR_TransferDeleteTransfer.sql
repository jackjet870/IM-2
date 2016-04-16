if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferDeleteTransfer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferDeleteTransfer]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Limpia la tabla de Transferencia
** 
** [michan]	15/Abr/2016 Created
*/
CREATE PROCEDURE [dbo].[USP_OR_TransferDeleteTransfer]
AS
SET NOCOUNT ON

DELETE FROM osTransfer
	

