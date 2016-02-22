if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferAddRoomTypes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferAddRoomTypes]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Agrega los tipos de habitacion en el proceso de transferencia
**
** [wtorres] 	02/Feb/2012 Creado
**
*/
create procedure [dbo].[USP_OR_TransferAddRoomTypes]	
as
set nocount on

-- a los tipos de habitacion sin descripcion les pone como descripcion su clave
update osTransfer
set trtN = trt
where trtN is null or trtN = ''

-- agregamos los tipos de habitacion
insert into RoomTypes (rtID, rtN, rtA)
select distinct trt, trt, 1
from osTransfer
where trt <> '' and not exists
	(select rtID from RoomTypes where rtID = trt)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

