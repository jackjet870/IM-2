if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferUpdateAgenciesHotelNames]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferUpdateAgenciesHotelNames]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Actualiza las descripciones de las agencias del sistema de Hotel en el proceso de transferencia
** 
** [wtorres]	26/Dic/2011 Creado
**
*/
create procedure [dbo].[USP_OR_TransferUpdateAgenciesHotelNames]
as

update HotelAgencies
set haN = tagN
from HotelAgencies
	inner join osTransfer on haID = tagID
where haN <> tagN
	and tagN <> ''
	and tagN <> haID
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

