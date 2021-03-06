if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_SaveGiftLog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_SaveGiftLog]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Agrega un registro en el historico de un regalo si su informacion relevante cambio
** 
** [wtorres]	05/Nov/2015 Modified. Agregue los campos de precios
**
*/
create procedure [dbo].[USP_OR_SaveGiftLog]
	@Gift varchar(10),
	@HoursDif smallint,
	@ChangedBy varchar(10)
as
set nocount on

declare @Count int

-- determinamos si cambio algun campo relevante
select @Count = Count(*)
from GiftsLog
	inner join Gifts on gggi = giID
where
	gggi = @Gift
	and ggID in (select Max(ggID) from GiftsLog where gggi = @Gift)
	-- Costos
	and (ggPrice1 = giPrice1 or (ggPrice1 is null and giPrice1 is null))
	and (ggPrice2 = giPrice2 or (ggPrice2 is null and giPrice2 is null))
	and (ggPrice3 = giPrice3 or (ggPrice3 is null and giPrice3 is null))
	and (ggPrice4 = giPrice4 or (ggPrice4 is null and giPrice4 is null))
	-- Precios
	and (ggPriceAdults = giPublicPrice or (ggPriceAdults is null and giPublicPrice is null))
	and (ggPriceMinors = giPriceMinor or (ggPriceMinors is null and giPriceMinor is null))
	and (ggPriceExtraAdults = giPublicPrice or (ggPriceExtraAdults is null and giPublicPrice is null))
	and (ggPack = giPack or (ggPack is null and giPack is null))
	and (gggc = gigc or (gggc is null and gigc is null))
	and (ggInven  = giInven or (ggInven is null and giInven is null))
	and (ggA = giA or (ggA is null and giA is null))
	and (ggWFolio = giWFolio or (ggWFolio is null and giWFolio is null))
	and (ggWPax = giWPax or (ggWPax is null and giWPax is null))
	and (ggO = giO or (ggO is null and giO is null))

-- agregamos un registro en el historico, si cambio algun campo relevante
insert into GiftsLog (
	ggID,
	ggChangedBy,
	gggi,
	-- Costos
	ggPrice1,
	ggPrice2,
	ggPrice3,
	ggPrice4,
	-- Precios
	ggPriceAdults,
	ggPriceMinors,
	ggPriceExtraAdults,
	ggPack,
	gggc,
	ggInven,
	ggA,
	ggWFolio,
	ggWPax,
	ggO
)
select
    DateAdd(hh, @HoursDif, GetDate()),
	@ChangedBy,
	giID,
	-- Costos
	giPrice1,
	giPrice2,
	giPrice3,
	giPrice4,
	-- Precios
	giPublicPrice,
	giPriceMinor,
	giPriceExtraAdult,
	giPack,
	gigc,
	giInven,
	giA,
	giWFolio,
	giWPax,
	giO
from Gifts
where giID = @Gift and @Count = 0

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

