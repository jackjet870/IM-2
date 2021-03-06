if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptContactBookShowQuinellas]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptContactBookShowQuinellas]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de contactacion, book y show considerando quinielas
**		1. Agencias (3-4 noches)
**		2. Agencias
**		3. Directos (3-4 noches)
**		4. Directos
**		5. Intercambios 4x3
**		6. Intercambios
**		7. Grupos
**		8. Socios
**		9. Total
** 
** [wtorres]	19/Ago/2009 Created
** [wtorres]	03/Feb/2010 Modified. Agregue el mercado de directos
** [wtorres]	13/May/2010 Modified. Ahora acepta varios Lead Sources
** [wtorres]	24/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [caduran]	23/Sep/2014 Modified. Agregue el parametro @GroupByLeadSource
** [wtorres]	04/Jun/2015 Modified. Agregue el parametro @External
**
*/
create procedure [dbo].[USP_OR_RptContactBookShowQuinellas]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@External int = 0,					-- Filtro de invitaciones externas
										--		0. Sin filtro
										--		1. Excluir invitaciones externas
										--		2. Solo invitaciones externas
	@BasedOnArrival bit = 0,			-- Indica si se debe basar en la fecha de llegada
	@GroupByLeadSource bit = 0			-- Indica si el resultado debe ser agrupado por Lead Source
as
set nocount on

-- si no debe estar agrupado por Lead Source
if @GroupByLeadSource = 0  begin

	-- Agencias (3-4 noches)
	exec USP_OR_RptProductionByMonth @DateFrom, @DateTo, @LeadSources, 1, 'AGENCIES', 1, 3, 4, default, 1, @External, @BasedOnArrival

	-- Agencias
	exec USP_OR_RptProductionByMonth @DateFrom, @DateTo, @LeadSources, 1, 'AGENCIES', default, default, default, default, 1, @External, @BasedOnArrival

	-- Directos (3-4 noches)
	exec USP_OR_RptProductionByMonth @DateFrom, @DateTo, @LeadSources, 1, 'DIRECTS', 1, 3, 4, default, 1, @External, @BasedOnArrival

	-- Directos
	exec USP_OR_RptProductionByMonth @DateFrom, @DateTo, @LeadSources, 1, 'DIRECTS', default, default, default, default, 1, @External, @BasedOnArrival

	-- Intercambios 4x3
	exec USP_OR_RptProductionByMonth @DateFrom, @DateTo, @LeadSources, 1, 'EXCHANGES', 0, 0, 0, '%4X3%', 1, @External, @BasedOnArrival

	-- Intercambios
	exec USP_OR_RptProductionByMonth @DateFrom, @DateTo, @LeadSources, 1, 'EXCHANGES', default, default, default, default, 1, @External, @BasedOnArrival

	-- Grupos
	exec USP_OR_RptProductionByMonth @DateFrom, @DateTo, @LeadSources, 1, 'GROUPS', default, default, default, default, 1, @External, @BasedOnArrival

	-- Socios
	exec USP_OR_RptProductionByMonth @DateFrom, @DateTo, @LeadSources, 1, 'MEMBERS', default, default, default, default, 1, @External, @BasedOnArrival

	-- Total
	exec USP_OR_RptProductionByMonth @DateFrom, @DateTo, @LeadSources, 1, default, default, default, default, default, 1, @External, @BasedOnArrival

-- si debe estar agrupado por Lead Source
end else begin

	-- Agencias (3-4 noches)
	exec USP_OR_RptProductionByLeadSourceMonth @DateFrom, @DateTo, @LeadSources, 1, 'AGENCIES', 1, 3, 4, default, 1, @External, @BasedOnArrival

	-- Agencias
	exec USP_OR_RptProductionByLeadSourceMonth @DateFrom, @DateTo, @LeadSources, 1, 'AGENCIES', default, default, default, default, 1, @External, @BasedOnArrival

	-- Directos (3-4 noches)
	exec USP_OR_RptProductionByLeadSourceMonth @DateFrom, @DateTo, @LeadSources, 1, 'DIRECTS', 1, 3, 4, default, 1, @External, @BasedOnArrival

	-- Directos
	exec USP_OR_RptProductionByLeadSourceMonth @DateFrom, @DateTo, @LeadSources, 1, 'DIRECTS', default, default, default, default, 1, @External, @BasedOnArrival

	-- Intercambios 4x3
	exec USP_OR_RptProductionByLeadSourceMonth @DateFrom, @DateTo, @LeadSources, 1, 'EXCHANGES', 0, 0, 0, '%4X3%', 1, @External, @BasedOnArrival

	-- Intercambios
	exec USP_OR_RptProductionByLeadSourceMonth @DateFrom, @DateTo, @LeadSources, 1, 'EXCHANGES', default, default, default, default, 1, @External, @BasedOnArrival

	-- Grupos
	exec USP_OR_RptProductionByLeadSourceMonth @DateFrom, @DateTo, @LeadSources, 1, 'GROUPS', default, default, default, default, 1, @External, @BasedOnArrival

	-- Socios
	exec USP_OR_RptProductionByLeadSourceMonth @DateFrom, @DateTo, @LeadSources, 1, 'MEMBERS', default, default, default, default, 1, @External, @BasedOnArrival

	-- Total
	exec USP_OR_RptProductionByLeadSourceMonth @DateFrom, @DateTo, @LeadSources, 1, default, default, default, default, default, 1, @External, @BasedOnArrival		
	
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


