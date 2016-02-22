if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferAddCountries]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferAddCountries]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Agrega las paises en el proceso de transferencia
**
** [wtorres] 	22/Abr/2009 Creado
** [wtorres] 	25/Ene/2012 Agregue la actualizacion de la descripcion del pais
**
*/
create procedure [dbo].[USP_OR_TransferAddCountries]	
as
set nocount on

insert into Countries (coID, coN, coum, cola, coA)
select distinct hcco, hcN, 0, 'EN', 1
from HotelCountries
where hcco <> '' and not exists
	(select coID from Countries where coID = hcco)
    
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

