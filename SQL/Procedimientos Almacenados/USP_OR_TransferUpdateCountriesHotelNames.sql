if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferUpdateCountriesHotelNames]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferUpdateCountriesHotelNames]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Actualiza las descripciones de los paises del sistema de Hotel en el proceso de transferencia
** 
** [wtorres]	25/Ene/2012 Creado
**
*/
create procedure [dbo].[USP_OR_TransferUpdateCountriesHotelNames]
as

update HotelCountries
set hcN = tcoN
from HotelCountries
	inner join osTransfer on hcID = tcoID
where hcN <> tcoN
	and tcoN <> ''
	and tcoN <> hcID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

