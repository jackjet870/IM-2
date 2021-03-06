if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetCostByPRWithDetailGifts]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetCostByPRWithDetailGifts]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el PR con Detalle de Gifts
** 
** [gmaya]	07/Jul/2014 Creado
**
*/
create function [dbo].[UFN_OR_GetCostByPRWithDetailGifts](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@CourtesyTour int = -1,		-- Filtro de tours de cortesia:
								--		-1. Sin filtro
								--		 0. No tours de cortesia
								--		 1. Tours de cortesia
    @DetailGifts bit = 1	-- Indica si desea Detail Gifts
)


returns @Table table (		PR varchar(10),		Qty decimal,		GiftsID varchar(10),		GiftsName varchar(100)		)as
begin

insert @Table

	SELECT R.grpe, SUM(C.geQty) AS geQty, C.gegi, I.giN
	FROM   dbo.GiftsReceipts AS R INNER JOIN
		   dbo.GiftsReceiptsC AS C ON R.grID = C.gegr INNER JOIN
		   dbo.Guests AS G ON R.grgu = G.guID INNER JOIN
           dbo.Gifts AS I ON C.gegi = I.giID
	WHERE 
			-- Fecha del recibo
			(R.grD BETWEEN @DateFrom AND @DateTo) 
			-- Lead Source
			AND (R.grls IN (SELECT item FROM dbo.Split(@LeadSources, ',') AS Split_1))
			-- No considerar los recibos cancelados
			AND (R.grCancel = 0) 
			-- Tours de cortesia
			AND (@CourtesyTour = -1 or G.guCTour = @CourtesyTour)
	GROUP BY R.grpe, C.gegi, I.giN Order by R.grpe

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

