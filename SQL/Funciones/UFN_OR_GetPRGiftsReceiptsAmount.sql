if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetPRGiftsReceiptsAmount]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetPRGiftsReceiptsAmount]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de recibos de regalos por PR
** 
** [wtorres]	19/Oct/2011 Creado
** [wtorres]	27/Oct/2011 Agregue el parametro @CourtesyTour
**
*/
create function [dbo].[UFN_OR_GetPRGiftsReceiptsAmount](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Clave de los Lead Sources
	@CourtesyTour int = -1		-- Filtro de tours de cortesia:
								--		-1. Sin filtro
								--		 0. No tours de cortesia
								--		 1. Tours de cortesia
)
returns @Table table (
	PR varchar(10),
	GiftsReceiptsAmount money
)
as
begin

insert @Table
select
	R.grpe,
	Sum(IsNull(D.gePriceA + D.gePriceM, 0))
from GiftsReceipts R
	inner join GiftsReceiptsC D on R.grID = D.gegr
	inner join Guests G on R.grgu = G.guID
where
	-- Fecha del recibo
	R.grD between @DateFrom and @DateTo
	-- Lead Source
	and R.grls in (select item from split(@LeadSources, ','))
	-- No considerar los recibos cancelados
	and R.grCancel = 0
	-- Tours de cortesia
	and (@CourtesyTour = -1 or G.guCTour = @CourtesyTour)
group by R.grpe

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

