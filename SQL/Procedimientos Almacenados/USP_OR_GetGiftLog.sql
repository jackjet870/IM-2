if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetGiftLog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetGiftLog]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene el registro historico de un regalo
** 
** [wtorres]	17/Ago/2011 Created
** [wtorres]	05/Nov/2015 Modified. Agregue los campos de precios y la descripcion de la categoria
**
*/
create procedure [dbo].[USP_OR_GetGiftLog] 
	@Gift varchar(10) -- Clave del regalo
as
set nocount on;

select 
	L.ggChangedBy,
	C.peN as ChangedByN,
	L.ggID,
	-- Costos
	L.ggPrice1,
	L.ggPrice2,
	L.ggPrice3,
	L.ggPrice4,
	-- Precios
	L.ggPriceAdults,
	L.ggPriceMinors,
	L.ggPriceExtraAdults,
	L.ggPack,
	GC.gcN,
	L.ggInven,
	L.ggA,
	L.ggWFolio,
	L.ggWPax,
	L.ggO
from GiftsLog L
	left join GiftsCategs GC on GC.gcID = L.gggc
	left join Personnel C on L.ggChangedBy = C.peID
where L.gggi = @Gift
order by L.ggID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

