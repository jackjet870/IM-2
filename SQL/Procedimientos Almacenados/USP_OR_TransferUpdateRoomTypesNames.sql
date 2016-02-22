if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferUpdateRoomTypesNames]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferUpdateRoomTypesNames]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Actualiza las descripciones de los tipos de habitacion en el proceso de transferencia
** 
** [wtorres]	02/Feb/2012 Creado
**
*/
create procedure [dbo].[USP_OR_TransferUpdateRoomTypesNames]
as

update RoomTypes
set rtN = trtN
from RoomTypes
	inner join osTransfer on rtID = trt
where rtN <> trtN
	and trtN <> ''
	and trtN <> rtID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

