if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferUpdateTransferMarkets]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferUpdateTransferMarkets]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Procedimiento:	Actualizar mercado de reservaciones migradas en el proceso de transferencia
-- Descripción:		Actualiza el mercado de reservaciones migradas
-- Histórico:		[wtorres] 05/Jul/2010 Creado
-- =============================================
create procedure [dbo].[USP_OR_TransferUpdateTransferMarkets]
as
set nocount on

update osTransfer
set tmk = agmk
from osTransfer
	inner join Agencies on toagID = agID
where tmk = 'AGENCIES' and agmk <> 'AGENCIES'

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

