if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferGetTransfer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferGetTransfer]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Selecciona todos los registros de la tabla OsTransfer
** 
** [michan]	15/Abr/2016 Created
*/
CREATE PROCEDURE [dbo].[USP_OR_TransferGetTransfer]
AS
SET NOCOUNT ON
SELECT * FROM osTransfer