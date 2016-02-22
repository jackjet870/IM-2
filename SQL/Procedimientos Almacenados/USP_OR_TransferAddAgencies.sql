if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferAddAgencies]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferAddAgencies]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Agrega las agencias en el proceso de transferencia
**
** [wtorres] 	22/Abr/2009 Creado
** [wtorres] 	23/Dic/2011 Agregue la actualizacion de la descripcion de la agencia
** [wtorres]	26/Ene/2012 Agregue la actualizacion del pais de la agencia
**
*/
create procedure [dbo].[USP_OR_TransferAddAgencies]	
as
set nocount on

-- agregamos las agencias del mercado de agencias (AGENCIES)
insert into Agencies (agID, agN, agum, agmk, agShowPay, agSalePay, agrp, agA, agco, agList, agVerified)
select distinct haag, haN, haum, 'AGENCIES', 0, 0, NULL, 1, haco, 0, 0
from HotelAgencies
	inner join osTransfer on haag = tagID
where haag <> '' and not exists
	(select agID from Agencies A where agID = haag)
	and (tGuestRef is null or (tGuestRef <> 'G' and tGuestRef <> 'M'))
	and tagID <> 'RCI/4X3'

-- agregamos las agencias del mercado de socios (MEMBERS)
insert into Agencies (agID, agN, agum, agmk, agShowPay, agSalePay, agrp, agA, agco, agList, agVerified)
select distinct haag, haN, haum, 'MEMBERS', 0, 0, NULL, 1, haco, 0, 0
from HotelAgencies
	inner join osTransfer on haag = tagID
where haag <> '' and not exists
	(select agID from Agencies A where agID = haag)
	and (tGuestRef = 'G' or tGuestRef = 'M')

-- agregamos las agencias del mercado de intercambios (EXCHANGES)
insert into Agencies (agID, agN, agum, agmk, agShowPay, agSalePay, agrp, agA, agco, agList, agVerified)
select distinct haag, haN, haum, 'EXCHANGES', 0, 0, NULL, 1, haco, 0, 0
from HotelAgencies
	inner join osTransfer on haag = tagID 
where haag <> '' and not exists
	(select agID from Agencies A where agID = haag)
	and (tGuestRef is null or (tGuestRef <> 'G' and tGuestRef <> 'M'))
    
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

