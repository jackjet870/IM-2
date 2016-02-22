if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferAddCountriesHotel]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferAddCountriesHotel]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Agrega los paises del sistema Hotel en el proceso de transferencia
**
** [wtorres] 	22/Abr/2009 Creado
** [wtorres]	09/Ene/2013 Ahora agrega los paises de las agencias y se establece el nombre del pais en lugar de establecerle como nombre
**							su clave
**
*/
create procedure [dbo].[USP_OR_TransferAddCountriesHotel]	
as
set nocount on

-- a los paises sin nombre les pone como nombre su clave
update osTransfer
set tcoN = tcoID
where tcoN is null or tcoN = ''

-- agregamos los paises de los huespedes
insert into HotelCountries (hcID, hcN, hcA, hcco, hcum, hcla)
select distinct tcoID, tcoN, 1, tcoID, 0, 'EN'
from osTransfer
where tcoID <> '' and not exists
	(select hcID from HotelCountries where hcID = tcoID)
	
-- agregamos los paises de las agencias
insert into HotelCountries (hcID, hcN, hcA, hcco, hcum, hcla)
select distinct tcoAID, tcoAN, 1, tcoAID, 0, 'EN'
from osTransfer
where tcoAID <> '' and not exists
	(select hcID from HotelCountries where hcID = tcoAID)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

