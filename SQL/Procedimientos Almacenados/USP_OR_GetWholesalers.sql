if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetWholesalers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetWholesalers]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta el catalogo de membresias mayoristas
** 
** [wtorres]	02/Mar/2015 Created
**
*/
create procedure [dbo].[USP_OR_GetWholesalers]
as
set nocount on

select
	W.wscl,
	C.clN,
	W.wsCompany,
	W.wsApplication,
	dbo.StringToTitle(dbo.Trim(case when W.wscl = 2 then M.name else MP.name end)) as Name,
	case when W.wscl = 2 then M.tour_date else MP.tour_date end as [Date],
	case when W.wscl = 2 then M.total else MP.total end as Total,
	dbo.StringToTitle(case when W.wscl = 2 then ST.descripcion else STP.descripcion end) as SaleType,
	dbo.StringToTitle(dbo.Trim(case when W.wscl = 2 then P.name else PP.name end)) as Program
from Wholesalers W
	left join Clubs C on C.clID = W.wscl
	
	-- Membresia Elite
	left join Hotel.analista_h.clmember M on W.wscl = 2 and M.company = W.wsCompany and M.application = W.wsApplication
	left join Hotel.analista_h.CLTipoVentas ST on ST.Codigo = M.sale_type
	left join Hotel.analista_h.clprogra P on P.code = M.program
	
	-- Membresia Premier
	left join [svr-mssql-test].Hotel.analista_h.clmember MP on W.wscl = 1 and MP.company = W.wsCompany and MP.application = W.wsApplication
	left join [svr-mssql-test].Hotel.analista_h.CLTipoVentas STP on STP.Codigo = MP.sale_type
	left join [svr-mssql-test].Hotel.analista_h.clprogra PP on PP.code = MP.program
order by W.wscl, M.name, M.tour_date desc

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

