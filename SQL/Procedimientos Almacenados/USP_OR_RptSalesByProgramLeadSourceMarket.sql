if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptSalesByProgramLeadSourceMarket]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptSalesByProgramLeadSourceMarket]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene los datos del reporte de ventas por programa, Lead Source y mercado
**
** [wtorres] 	03/Oct/2013 Creado
**
*/
create procedure [dbo].[USP_OR_RptSalesByProgramLeadSourceMarket]
	@DateFrom datetime,	-- Fecha desde
	@DateTo datetime	-- Fecha hasta
as
set nocount on

select
	P.pgN as Program,
	L.lsN as LeadSource,
	M.mkN as Market,
	S.sagu as GuestID,
	S.saLastName1 as LastName,
	S.saFirstName1 as FirstName,
	S.saMembershipNum as Membership,
	S.saD as SaleDate,
	S.saProc as Procesable,
	S.saProcD as SaleProcesableDate,
	S.saCancel as Cancel,
	S.saCancelD as CancelDate
from Sales S
	left join Guests G on G.guID = S.sagu
	left join LeadSources L on L.lsID = S.sals
	left join Programs P on P.pgID = L.lspg
	left join Markets M on M.mkID = G.gumk
where
	-- Fecha
	S.saD between @DateFrom and @DateTo
order by Program, LeadSource, Market, Membership

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

