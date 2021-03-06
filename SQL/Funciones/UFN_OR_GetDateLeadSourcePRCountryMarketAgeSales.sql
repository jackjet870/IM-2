if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetDateLeadSourcePRCountryMarketAgeSales]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetDateLeadSourcePRCountryMarketAgeSales]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas por fecha, Lead Source, PR, pais, mercado y edad
** 
** [wtorres]	25/Jun/2010 Creado
** [wtorres]	19/Nov/2013 Ahora los downgrades se cuentan en base a la categoria de tipo de venta
**
*/
create function [dbo].[UFN_OR_GetDateLeadSourcePRCountryMarketAgeSales](
	@DateFrom datetime,	-- Fecha desde
	@DateTo datetime	-- Fecha hasta
)
returns @Table table (
	[Date] datetime,
	LeadSource varchar(10),
	PR varchar(10),
	Country varchar(25),
	Market varchar(10),
	Age tinyint,
	Sales money
)
as
begin

insert @Table
select
	S.saProcD,
	S.sals,
	S.saPR1,
	G.guco,
	G.gumk,
	G.guAge1,
	Count(*)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
where
	-- Fecha de procesable
	S.saProcD between @DateFrom and @DateTo
	-- No downgrades
	and ST.ststc <> 'DG'
	-- Fecha de cancelacion
	and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
group by S.saProcD, S.sals, S.saPR1, G.guco, G.gumk, G.guAge1

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

