if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferUpdateTransferCountries]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferUpdateTransferCountries]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Procedimiento:	Actualizar pa�ses de reservaciones migradas en el proceso de transferencia
-- Descripci�n:		Actualiza las pa�ses de las reservaciones migradas
-- Hist�rico:		[wtorres] 22/Abr/2009 Creado
-- =============================================
create procedure [dbo].[USP_OR_TransferUpdateTransferCountries]	
as
set nocount on

update osTransfer
set tocoID = hcco
from osTransfer
	inner join HotelCountries on tcoID = hcID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

