if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferAddAgenciesHotel]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferAddAgenciesHotel]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Agrega las agencias del sistema Hotel en el proceso de transferencia
**
** [wtorres] 	22/Abr/2009 Creado
** [wtorres]	26/Ene/2012 Agregue la actualizacion del pais de la agencia
** [wtorres]	09/Ene/2013 Ahora se establece el nombre de la agencia en lugar de establecerle como nombre su clave
**
*/
create procedure [dbo].[USP_OR_TransferAddAgenciesHotel]	
as
set nocount on

-- a las agencias sin nombre les pone como nombre su clave
update osTransfer
set tagN = tagID
where tagN is null or tagN = ''

-- agregamos las agencias del mercado de agencias (AGENCIES)
insert into HotelAgencies (haID, haN, haA, haag, haum, hamk, haco)
select distinct tagID, tagN, 1, tagID, 0, 'AGENCIES', tcoAID
from osTransfer
where tagID <> '' and not exists
	(select haID from HotelAgencies where haID = tagID)
	and (tGuestRef is null or (tGuestRef <> 'G' and tGuestRef <> 'M'))
	and tagID <> 'RCI/4X3'

-- agregamos las agencias del mercado de socios (MEMBERS)
insert into HotelAgencies (haID, haN, haA, haag, haum, hamk, haco)
select distinct tagID, tagN, 1, tagID, 0, 'MEMBERS', tcoAID
from osTransfer
where tagID <> '' and not exists
	(select haID from HotelAgencies where haID = tagID)
	and (tGuestRef = 'G' or tGuestRef = 'M')
            
-- agregamos las agencias del mercado de intercambios (EXCHANGES)
insert into HotelAgencies (haID, haN, haA, haag, haum, hamk, haco)
select distinct tagID, tagN, 1, tagID, 0, 'EXCHANGES', tcoAID
from osTransfer
where tagID = 'RCI/4X3' and not exists
	(select haID from HotelAgencies where haID = tagID)
	and (tGuestRef is null or (tGuestRef <> 'G' AND tGuestRef <> 'M'))

-- establecemos las agencias de grupos
update HotelAgencies
set haum = 2
from HotelAgencies
	inner join osTransfer on haID = tagID
where tum = 2

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

