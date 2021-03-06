if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_IM_RptContactBookShowQuinellas]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_IM_RptContactBookShowQuinellas]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de contactacion, book y show considerando quinielas
** 
** [aalcocer]	13/May/2016 Created
*/
create procedure [dbo].[USP_IM_RptContactBookShowQuinellas]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@External int = 0,					-- Filtro de invitaciones externas
										--		0. Sin filtro
										--		1. Excluir invitaciones externas
										--		2. Solo invitaciones externas
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada	
as
SET FMTONLY OFF;
set nocount on

CREATE TABLE #Report (
	Market varchar(10),
	Subgroup varchar(15),
	[Year] int,
	[Month] int,
	Arrivals int ,
	Contacts int,
	ContactsFactor money,
	Availables int,
	AvailablesFactor money,
	GrossBooks int,
	Books int,
	BooksFactor money,
	GrossShows  int,
	Shows  int,
	ShowsFactor money,
	ShowsArrivalsFactor money,
	Sales int,
	SalesAmount int,
	ClosingFactor money,
	Efficiency money,
	AverageSale money,
	LeadSource varchar(10)
)

CREATE TABLE #TempTable (
	[Year] int,
	[Month] int,
	Arrivals int ,
	Contacts int,
	ContactsFactor money,
	Availables int,
	AvailablesFactor money,
	GrossBooks int,
	Books int,
	BooksFactor money,
	GrossShows  int,
	Shows  int,
	ShowsFactor money,
	ShowsArrivalsFactor money,
	Sales int,
	SalesAmount int,
	ClosingFactor money,
	Efficiency money,
	AverageSale money,
	LeadSource varchar(10)
)

DECLARE @market varchar(10),
@subgroup varchar(15)

SET @market = 'AGENCIES'
-- Agencias (3-4 noches)	
SET @subgroup= '(3-4 Noches)'
INSERT INTO #TempTable
exec USP_OR_RptProductionByLeadSourceMonth @DateFrom, @DateTo, @LeadSources, 1, @market, 1, 3, 4, default, 1, @External, @BasedOnArrival
INSERT INTO #Report SELECT @market as Market, @subgroup as Subgroup, * FROM #TempTable


-- Agencias	
SET @subgroup= 'TOTAL'
DELETE #TempTable
INSERT INTO #TempTable
exec USP_OR_RptProductionByLeadSourceMonth @DateFrom, @DateTo, @LeadSources, 1, @market, default, default, default, default, 1, @External, @BasedOnArrival
INSERT INTO #Report SELECT @market as Market, @subgroup as Subgroup, * FROM #TempTable

SET @market = 'DIRECTS'
-- Directos (3-4 noches)
SET @subgroup= '(3-4 Noches)'
DELETE #TempTable
INSERT INTO #TempTable
exec USP_OR_RptProductionByLeadSourceMonth @DateFrom, @DateTo, @LeadSources, 1, @market, 1, 3, 4, default, 1, @External, @BasedOnArrival
INSERT INTO #Report SELECT @market as Market, @subgroup as Subgroup, * FROM #TempTable

-- Directos
SET @subgroup= 'TOTAL'
DELETE #TempTable
INSERT INTO #TempTable
exec USP_OR_RptProductionByLeadSourceMonth @DateFrom, @DateTo, @LeadSources, 1, @market, default, default, default, default, 1, @External, @BasedOnArrival
INSERT INTO #Report SELECT @market as Market, @subgroup as Subgroup, * FROM #TempTable

SET @market = 'EXCHANGES'
-- Intercambios 4x3
SET @subgroup= '4X3'
DELETE #TempTable
INSERT INTO #TempTable
exec USP_OR_RptProductionByLeadSourceMonth @DateFrom, @DateTo, @LeadSources, 1, @market, 0, 0, 0, '%4X3%', 1, @External, @BasedOnArrival
INSERT INTO #Report SELECT @market as Market, @subgroup as Subgroup, * FROM #TempTable

-- Intercambios
SET @subgroup= 'TOTAL'
DELETE #TempTable
INSERT INTO #TempTable
exec USP_OR_RptProductionByLeadSourceMonth @DateFrom, @DateTo, @LeadSources, 1, @market, default, default, default, default, 1, @External, @BasedOnArrival
INSERT INTO #Report SELECT @market as Market, @subgroup as Subgroup, * FROM #TempTable

SET @market = 'GROUPS'
-- Grupos
DELETE #TempTable
INSERT INTO #TempTable
exec USP_OR_RptProductionByLeadSourceMonth @DateFrom, @DateTo, @LeadSources, 1, @market, default, default, default, default, 1, @External, @BasedOnArrival
INSERT INTO #Report SELECT @market as Market, @subgroup as Subgroup, * FROM #TempTable

SET @market = 'MEMBERS'
-- Socios
DELETE #TempTable
INSERT INTO #TempTable
exec USP_OR_RptProductionByLeadSourceMonth @DateFrom, @DateTo, @LeadSources, 1, @market, default, default, default, default, 1, @External, @BasedOnArrival
INSERT INTO #Report SELECT @market as Market, @subgroup as Subgroup, * FROM #TempTable

Select 'IN HOUSE EXPRESS (Originally Availables)' as [Group], rpt.*, zn.znID,zn.znN
From #Report rpt
INNER JOIN LeadSources ls on ls.lsID = rpt.LeadSource
INNER Join Zones zn on ls.lszn=zn.znID