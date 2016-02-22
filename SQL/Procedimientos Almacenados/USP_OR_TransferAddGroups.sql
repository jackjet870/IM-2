if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferAddGroups]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferAddGroups]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Agrega los grupos en el proceso de transferencia
**
** [lchairez] 	23/Abr/2014 Creado 
**
*/
CREATE PROCEDURE [dbo].[USP_OR_TransferAddGroups]
AS
SET NOCOUNT ON

-- a los grupos sin descripcion les pone como descripcion su clave
update osTransfer
set tgroupN = tgroupID
where tgroupN is null or tgroupN = ''

-- agregamos los grupos
insert into Groups (gbID, gbN, gbgu)
select distinct t.tgroupID,t.tgroupN, g.guID
from osTransfer t
join Guests g on t.tHReservID = g.guHReservID
where tOnGroup = 1 and tgroupID <> '' and not exists
	(select gbID from Groups where gbID = tgroupID)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
