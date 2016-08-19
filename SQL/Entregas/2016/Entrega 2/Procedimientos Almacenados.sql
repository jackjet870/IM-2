USE [OrigosVCPalace]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptContactBookShowQuinellas]    Script Date: 08/19/2016 10:01:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_IM_RptContactBookShowQuinellas]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_IM_RptContactBookShowQuinellas]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetGuestsMailOuts]    Script Date: 08/19/2016 10:01:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_GetGuestsMailOuts]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_GetGuestsMailOuts]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptGiftsReceivedBySR]    Script Date: 08/19/2016 10:01:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptGiftsReceivedBySR]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptGiftsReceivedBySR]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptProductionByAgencyMonthly]    Script Date: 08/19/2016 10:01:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptProductionByAgencyMonthly]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptProductionByAgencyMonthly]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptProductionByContractAgencyInhouse]    Script Date: 08/19/2016 10:01:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptProductionByContractAgencyInhouse]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptProductionByContractAgencyInhouse]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptProductionByContractAgencyMarketOriginallyAvailableInhouse]    Script Date: 08/19/2016 10:01:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptProductionByContractAgencyMarketOriginallyAvailableInhouse]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptProductionByContractAgencyMarketOriginallyAvailableInhouse]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptProductionByMemberTypeAgencyInhouse]    Script Date: 08/19/2016 10:01:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptProductionByMemberTypeAgencyInhouse]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptProductionByMemberTypeAgencyInhouse]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouse]    Script Date: 08/19/2016 10:01:37 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouse]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouse]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptUnavailableMotivesByAgency]    Script Date: 08/19/2016 10:01:37 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_OR_RptUnavailableMotivesByAgency]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_OR_RptUnavailableMotivesByAgency]
GO

USE [OrigosVCPalace]
GO

/****** Object:  StoredProcedure [dbo].[USP_IM_RptContactBookShowQuinellas]    Script Date: 08/19/2016 10:01:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_GetGuestsMailOuts]    Script Date: 08/19/2016 10:01:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene los Mail Outs disponibles de un Lead Source
**
** [aalcocer]	24/Feb/2011 Created
** [wtorres]	12/Ago/2016 Modified. Documentado y actualizado a como estaba en la BD. Ahora ya no esta repetido el campo guPickUpT
**
*/
CREATE PROCEDURE [dbo].[USP_OR_GetGuestsMailOuts]
	@guls as varchar(10),		-- Clave del Lead Source
	@guCheckInD as datetime,	-- Fecha de llegada
	@guCheckOutD as datetime,	-- Fecha de salida
	@guBookD as datetime		-- Fecha de booking
AS 
SET NOCOUNT ON

SELECT CASE
	WHEN guCheckIn = 0 THEN 8  --Sin Check-In
	WHEN guCheckOutD < GETDATE() THEN 7 --Check Out
	WHEN guAvail = 0 THEN 6 --Invitacion
	WHEN guInvit=1 AND guBookCanc=1 THEN 5 --Inv. Cancelada
	WHEN guInvit=1 AND guBookD <  GETDATE() AND guShow=0 THEN 4 -- Inv. No Show
	WHEN guInvit=1 AND guShow=1 THEN 3 --Inv. Show
	WHEN guInvit=1 THEN 2 -- Invitado en stand-by
	WHEN guInfo=1 THEN 1 -- Info
	WHEN guAvail=1 THEN 0 --Disponible
	END AS guStatus,  
	guBookT, guPickUpT, guRoomNum, guLastName1, guFirstName1, guPax, 
	guCheckIn, guCheckInD, guCheckOutD, guco, coN, guag, agN, guInfo, guInvit, guBookCanc, 
	guInfoD, guBookD, guPRInvit1, peN, gumo, gula, gumoA, guComments 
FROM Guests 
	LEFT JOIN Personnel ON guPRInvit1 = peID 
	LEFT JOIN Agencies ON guag = agID 
	LEFT JOIN Countries ON guco = coID 
WHERE
	-- Lead Source
	guls = @guls
	-- Disponible
	AND ((guAvail = 1
	-- Fecha de llegada menor a igual a hoy
	AND (guCheckInD <= @guCheckInD
	-- Fecha de salida mayor a mañana
	AND guCheckOutD > @guCheckOutD)
	-- Sin show
	AND guShow = 0
	-- No invitado o Invitado con Fecha de booking menor a pasado mañana
	AND (guInvit = 0 OR (guInvit = 1 AND guBookD < @guBookD)))
	-- Con mail out
	OR (gumo IS NOT NULL AND gumo <> 'NOMAIL'))
order by guCheckIn, guRoomNum
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptGiftsReceivedBySR]    Script Date: 08/19/2016 10:01:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene el reporte de regalos recibidos por sala de ventas
**
** [wtorres]	30/Jun/2009 Created
** [wtorres]	07/Jul/2009 Modified. Agregue el campo de clave del regalo
** [wtorres]	17/Sep/2009 Modified. Agregue el filtro por cargar a
** [wtorres]	03/Ago/2011 Modified. Desglose el campo de Pax en Adults y Minors y agregue los campos de Quantity y Couples
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave del regalo a varchar(max)
**
*/
create procedure [dbo].[USP_OR_RptGiftsReceivedBySR]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000),			-- Claves de Lead Sources
	@ChargeTo varchar(8000) = 'ALL',	-- Claves de cargar a
	@Gifts varchar(max) = 'ALL'			-- Claves de regalos
as
set nocount on

select
	S.srN as SalesRoom,
	G.giID as Gift,
	G.giN as GiftN,
	R.grcu as Currency,
	Sum(D.geQty) as Quantity,
	Sum(IsNull(GU.guShowsQty, 0)) as Couples,
	Sum(IsNull(Floor(R.grPax), 0)) as Adults,
	Sum(IsNull((R.grPax - Floor(R.grPax)) * 10, 0)) as Minors,
	Sum(IsNull(D.gePriceA + D.gePriceM, 0)) as Amount
into #tblData
from GiftsReceipts R
	inner join GiftsReceiptsC D on R.grID = D.gegr
	inner join SalesRooms S on R.grsr = S.srID
	inner join Gifts G on D.gegi = G.giID
	left join Guests GU on R.grgu = GU.guID
where
	-- Fecha del recibo
	R.grD between @DateFrom and @DateTo
	-- Lead Source
	and R.grls in (select item from split(@LeadSources, ','))
	-- Cargar a
	and (@ChargeTo = 'ALL' or R.grct in (select item from split(@ChargeTo, ',')))
	-- Regalo
	and (@Gifts = 'ALL' or D.gegi in (select item from split(@Gifts, ',')))
	-- No considerar los recibos cancelados
	and R.grCancel = 0
group by S.srN, G.giID, G.giN, R.grcu
order by S.srN, G.giID, R.grcu


-- 1. Regalos recibidos por sala
-- =============================================
select * from #tblData

-- 2. Monedas
-- =============================================
select distinct C.cuID, C.cuN
from #tblData D
	left join Currencies C on D.Currency = C.cuID
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptProductionByAgencyMonthly]    Script Date: 08/19/2016 10:01:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de produccion por agencia y mes
** 
** [wtorres]	09/Sep/2009 Created
** [wtorres]	14/May/2010 Modified. Agregue las columnas de bookings netos y shows netos
** [wtorres]	24/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [wtorres]	20/Dic/2013 Modified. Agregue el parametro @ConsiderQuinellas
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create procedure [dbo].[USP_OR_RptProductionByAgencyMonthly]
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@ConsiderQuinellas bit = 0,		-- Indica si se debe considerar quinielas
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
as
set nocount on

-- Datos
-- =============================================
select
	-- Año
	[Year],
	-- Total de agencia
	Cast(0 as bit) as AgencyTotal,
	-- Agencia
	Agency,
	IsNull(A.agN, 'Not Specified') as AgencyN,
	-- Total de Lead Source
	Cast(0 as bit) as LeadSourceTotal,
	-- Lead Source
	LeadSource,
	-- Mes
	[Month],
	-- Nombre del mes
	DateName(Month, dbo.DateSerial([Year], [Month], 1)) as MonthN,
	-- Llegadas
	Sum(Arrivals) as Arrivals,
	-- Contactos
	Sum(Contacts) as Contacts,
	-- Disponibles
	Sum(Availables) as Availables,
	-- Bookings netos
	Sum(GrossBooks) as GrossBooks,
	-- Bookings
	Sum(Books) as Books,
	-- Shows netos
	Sum(D.GrossShows) as GrossShows,
	-- Shows
	Sum(Shows) as Shows,
	-- Ventas
	Sum(Sales) as Sales,
	-- Monto de ventas
	Sum(SalesAmount) as SalesAmount,
	-- Porcentaje de cierre
	[dbo].UFN_OR_SecureDivision(Sum(Sales), Sum(Shows)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(SalesAmount), Sum(Shows)) as Efficiency
into #tbl_Data
from (
	-- Llegadas
	select [Year], [Month], Agency, LeadSource, Arrivals, 0 as Contacts, 0 as Availables, 0 as Books, 0 as GrossBooks, 0 as Shows,
		0 as GrossShows, 0 as Sales, 0 as SalesAmount
	from UFN_OR_GetMonthAgencyLeadSourceArrivals(@DateFrom, @DateTo, @Agencies)
	-- Contactos
	union all
	select [Year], [Month], Agency, LeadSource, 0, Contacts, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetMonthAgencyLeadSourceContacts(@DateFrom, @DateTo, @Agencies, @BasedOnArrival)
	-- Disponibles
	union all
	select [Year], [Month], Agency, LeadSource, 0, 0, Availables, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetMonthAgencyLeadSourceAvailables(@DateFrom, @DateTo, @Agencies, @ConsiderQuinellas)
	-- Bookings
	union all
	select [Year], [Month], Agency, LeadSource, 0, 0, 0, Books, 0, 0, 0, 0, 0
	from UFN_OR_GetMonthAgencyLeadSourceBookings(@DateFrom, @DateTo, @Agencies, @ConsiderQuinellas, default, @BasedOnArrival)
	-- Bookings netos
	union all
	select [Year], [Month], Agency, LeadSource, 0, 0, 0, 0, Books,  0, 0, 0, 0
	from UFN_OR_GetMonthAgencyLeadSourceBookings(@DateFrom, @DateTo, @Agencies, @ConsiderQuinellas, 0, @BasedOnArrival)
	-- Shows
	union all
	select [Year], [Month], Agency, LeadSource, 0, 0, 0, 0, 0, Shows, 0, 0, 0
	from UFN_OR_GetMonthAgencyLeadSourceShows(@DateFrom, @DateTo, @Agencies, @ConsiderQuinellas, default, @BasedOnArrival)
	-- Shows netos
	union all
	select [Year], [Month], Agency, LeadSource, 0, 0, 0, 0, 0, 0, Shows, 0, 0
	from UFN_OR_GetMonthAgencyLeadSourceShows(@DateFrom, @DateTo, @Agencies, @ConsiderQuinellas, 1, @BasedOnArrival)
	-- Numero de ventas
	union all
	select [Year], [Month], Agency, LeadSource, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetMonthAgencyLeadSourceSales(@DateFrom, @DateTo, @Agencies, @BasedOnArrival)
	-- Monto de ventas
	union all
	select [Year], [Month], Agency, LeadSource, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetMonthAgencyLeadSourceSalesAmount(@DateFrom, @DateTo, @Agencies, @BasedOnArrival)
) as D
	left join Agencies A on A.agID = D.Agency
group by [Year], Agency, A.agN, LeadSource, [Month]

-- Datos con totales
-- =============================================
-- Datos
select * from #tbl_Data

-- Totales por agencia
union all
select
	-- Año
	[Year],
	-- Total de agencia
	Cast(0 as bit) as AgencyTotal,
	-- Agencia
	Agency,
	'' as AgencyN,
	-- Total de Lead Source
	Cast(1 as bit) as LeadSourceTotal,
	-- Lead Source
	'TOTAL ' + Agency as LeadSource,
	-- Mes
	[Month],
	-- Nombre del mes
	MonthN,
	-- Llegadas
	Sum(Arrivals) as Arrivals,
	-- Contactos
	Sum(Contacts) as Contacts,
	-- Disponibles
	Sum(Availables) as Availables,
	-- Bookings netos
	Sum(GrossBooks) as GrossBooks,
	-- Bookings
	Sum(Books) as Books,
	-- Shows netos
	Sum(GrossShows) as GrossShows,
	-- Shows
	Sum(Shows) as Shows,
	-- Ventas
	Sum(Sales) as Sales,
	-- Monto de ventas
	Sum(SalesAmount) as SalesAmount,
	-- Porcentaje de cierre
	[dbo].UFN_OR_SecureDivision(Sum(Sales), Sum(Shows)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(SalesAmount), Sum(Shows)) as Efficiency
from #tbl_Data
group by [Year], Agency, [Month], MonthN

-- Totales por año
union all
select
	-- Año
	[Year],
	-- Total de agencia
	Cast(1 as bit) as AgencyTotal,
	-- Agencia
	'TOTAL ' + Cast([Year] as varchar(4)) as Agency,
	'' as AgencyN,
	-- Total de Lead Source
	Cast(1 as bit) as LeadSourceTotal,
	-- Lead Source
	'TOTAL AGENCIES' as LeadSource,
	-- Mes
	[Month],
	-- Nombre del mes
	MonthN,
	-- Llegadas
	Sum(Arrivals) as Arrivals,
	-- Contactos
	Sum(Contacts) as Contacts,
	-- Disponibles
	Sum(Availables) as Availables,
	-- Bookings netos
	Sum(GrossBooks) as GrossBooks,
	-- Bookings
	Sum(Books) as Books,
	-- Shows netos
	Sum(GrossShows) as GrossShows,
	-- Shows
	Sum(Shows) as Shows,
	-- Ventas
	Sum(Sales) as Sales,
	-- Monto de ventas
	Sum(SalesAmount) as SalesAmount,
	-- Porcentaje de cierre
	[dbo].UFN_OR_SecureDivision(Sum(Sales), Sum(Shows)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(SalesAmount), Sum(Shows)) as Efficiency
from #tbl_Data
group by [Year], [Month], MonthN
order by [Year], AgencyTotal, Agency, LeadSourceTotal, LeadSource, [Month]
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptProductionByContractAgencyInhouse]    Script Date: 08/19/2016 10:01:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de producccion por contrato y agencia (Inhouse)
** 
** [galcocer]	04/Feb/2012 Created
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create procedure [dbo].[USP_OR_RptProductionByContractAgencyInhouse]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000),			-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',		-- Claves de mercados
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
as
set nocount on

select
	-- Contrato
	D.Contract,
	C.cnN as ContractN,
	-- Agencia
	D.Agency,
	IsNull(A.agN, 'Not Specified') as AgencyN,
	-- Llegadas
	Sum(D.Arrivals) as Arrivals,
	-- Contactos
	Sum(D.Contacts) as Contacts,
	-- Porcentaje de contactacion
	[dbo].UFN_OR_SecureDivision(Sum(D.Contacts), Sum(D.Arrivals)) as ContactsFactor,
	-- Disponibles
	Sum(D.Availables) as Availables,
	-- Porcentaje de disponibles
	[dbo].UFN_OR_SecureDivision(Sum(D.Availables), Sum(D.Contacts)) as AvailablesFactor,
	-- Bookings netos
	Sum(D.GrossBooks) as GrossBooks,
	-- Directas
	Sum(D.Directs) as Directs,
	-- Bookings
	Sum(D.Books) as Books,
	-- Porcentaje de Bookings
	[dbo].UFN_OR_SecureDivision(Sum(D.Books), Sum(D.Availables)) as BooksFactor,
	-- Shows netos
	Sum(D.GrossShows) as GrossShows,
	-- Shows
	Sum(D.Shows) as Shows,
	-- Porcentaje de shows
	[dbo].UFN_OR_SecureDivision(Sum(D.GrossShows), Sum(D.GrossBooks)) as ShowsFactor,
	-- Ventas
	Sum(D.Sales) as Sales_TOTAL,
	-- Monto de ventas
	Sum(D.SalesAmount) as SalesAmount_TOTAL,
	-- Ventas PR
	Sum(D.Sales - D.SalesSelfGen) as Sales_PR,
	-- Monto de ventas PR
	Sum(D.SalesAmount - D.SalesAmountSelfGen) as SalesAmount_PR,
	-- Ventas Self Gen
	Sum(D.SalesSelfGen) as Sales_SELFGEN,
	-- Monto de ventas SelfGen
	Sum(D.SalesAmountSelfGen) as SalesAmount_SELFGEN,
	-- Porcentaje de cierre
	[dbo].UFN_OR_SecureDivision(Sum(D.Sales), Sum(D.Shows)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Shows)) as Efficiency,
	-- Venta promedio
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Sales)) as AverageSale
from (
	-- Llegadas
	select Contract, Agency, Arrivals, 0 as Contacts, 0 as Availables, 0 as Books, 0 as GrossBooks, 0 as Directs, 0 as Shows, 0 as GrossShows,
		0 as Sales, 0 as SalesAmount, 0 as SalesSelfGen, 0 as SalesAmountSelfGen
	from UFN_OR_GetContractAgencyArrivals(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies)
	-- Contactos
	union all
	select Contract, Agency, 0, Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetContractAgencyContacts(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @BasedOnArrival)
	-- Disponibles
	union all
	select Contract, Agency, 0, 0, Availables, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetContractAgencyAvailables(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @ConsiderQuinellas)
	-- Bookings
	union all
	select Contract, Agency, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetContractAgencyBookings(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @ConsiderQuinellas, default, @BasedOnArrival)
	-- Bookings netos
	union all
	select Contract, Agency, 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetContractAgencyBookings(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @ConsiderQuinellas, 0, @BasedOnArrival)
	-- Directas
	union all
	select Contract, Agency, 0, 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetContractAgencyBookings(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @ConsiderQuinellas, 1, @BasedOnArrival)
	-- Shows
	union all
	select Contract, Agency, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0
	from UFN_OR_GetContractAgencyShows(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @ConsiderQuinellas, default, @BasedOnArrival)
	-- Shows netos
	union all
	select Contract, Agency, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0
	from UFN_OR_GetContractAgencyShows(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @ConsiderQuinellas, 1, @BasedOnArrival)
	-- Ventas
	union all
	select Contract, Agency, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0
	from UFN_OR_GetContractAgencySales(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, default, @BasedOnArrival)
	-- Monto de ventas
	union all
	select Contract, Agency, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0
	from UFN_OR_GetContractAgencySalesAmount(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, default, @BasedOnArrival)
	-- Ventas Self Gen
	union all
	select Contract, Agency, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetContractAgencySales(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, 1, @BasedOnArrival)
	-- Monto de ventas Self Gen
	union all
	select Contract, Agency, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetContractAgencySalesAmount(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, 1, @BasedOnArrival)
) as D
	left join Agencies A on A.agID = D.Agency
	left join Contracts C on C.cnID = D.Contract
group by D.Contract, C.cnN, D.Agency, A.agN
order by D.Agency, SalesAmount_TOTAL desc, Efficiency desc, Shows desc, Books desc, Availables desc, Contacts desc, Arrivals desc, D.Contract



GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptProductionByContractAgencyMarketOriginallyAvailableInhouse]    Script Date: 08/19/2016 10:01:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de producccion por contrato, agencia, mercado y originalmente disponible (Inhouse)
** 
** [galcocer]	04/Feb/2012 Created
** [lchairez]	06/Mar/2014 Modified. Se agrega un left join con la tabla Markets
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
*/
create procedure [dbo].[USP_OR_RptProductionByContractAgencyMarketOriginallyAvailableInhouse]
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@ConsiderQuinellas bit = 0,		-- Indica si se debe considerar quinielas
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
as
set nocount on

select
	-- Contrato
	D.Contract,
	C.cnN as ContractN,
	-- Agencia
	D.Agency,
	IsNull(A.agN, 'Not Specified') as AgencyN,
	-- Mercado
	IsNull(Cast(M.mkN as varchar(13)), 'Not Specified') as Market,
	-- Originalmente disponibe
	D.OriginallyAvailable,
	-- Llegadas
	Sum(D.Arrivals) as Arrivals,
	-- Contactos
	Sum(D.Contacts) as Contacts,
	-- Porcentaje de contactacion
	[dbo].UFN_OR_SecureDivision(Sum(D.Contacts), Sum(D.Arrivals)) as ContactsFactor,
	-- Disponibles
	Sum(D.Availables) as Availables,
	-- Porcentaje de disponibles
	[dbo].UFN_OR_SecureDivision(Sum(D.Availables), Sum(D.Contacts)) as AvailablesFactor,
	-- Bookings netos
	Sum(D.GrossBooks) as GrossBooks,
	-- Directas
	Sum(D.Directs) as Directs,
	-- Bookings
	Sum(D.Books) as Books,
	-- Porcentaje de Bookings
	[dbo].UFN_OR_SecureDivision(Sum(D.Books), Sum(D.Availables)) as BooksFactor,
	-- Shows netos
	Sum(D.GrossShows) as GrossShows,
	-- Shows
	Sum(D.Shows) as Shows,
	-- Porcentaje de shows
	[dbo].UFN_OR_SecureDivision(Sum(D.GrossShows), Sum(D.GrossBooks)) as ShowsFactor,
	-- Ventas
	Sum(D.Sales) as Sales_TOTAL,
	-- Monto de ventas
	Sum(D.SalesAmount) as SalesAmount_TOTAL,
	-- Ventas PR
	Sum(D.Sales - D.SalesSelfGen) as Sales_PR,
	-- Monto de ventas PR
	Sum(D.SalesAmount - D.SalesAmountSelfGen) as SalesAmount_PR,
	-- Ventas Self Gen
	Sum(D.SalesSelfGen) as Sales_SELFGEN,
	-- Monto de ventas SelfGen
	Sum(D.SalesAmountSelfGen) as SalesAmount_SELFGEN,
	-- Porcentaje de cierre
	[dbo].UFN_OR_SecureDivision(Sum(D.Sales), Sum(D.Shows)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Shows)) as Efficiency,
	-- Venta promedio
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Sales)) as AverageSale
from (
	-- Llegadas
	select Contract, Agency, Market, OriginallyAvailable, Arrivals, 0 as Contacts, 0 as Availables, 0 as Books, 0 as GrossBooks, 0 as Directs,
		0 as Shows, 0 as GrossShows, 0 as Sales, 0 as SalesAmount, 0 as SalesSelfGen, 0 as SalesAmountSelfGen
	from UFN_OR_GetContractAgencyMarketOriginallyAvailableArrivals(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies)
	-- Contactos
	union all
	select Contract, Agency, Market, OriginallyAvailable, 0, Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetContractAgencyMarketOriginallyAvailableContacts(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @BasedOnArrival)
	-- Disponibles
	union all
	select Contract, Agency, Market, OriginallyAvailable, 0, 0, Availables, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetContractAgencyMarketOriginallyAvailableAvailables(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @ConsiderQuinellas)
	-- Bookings
	union all
	select Contract, Agency, Market, OriginallyAvailable, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetContractAgencyMarketOriginallyAvailableBookings(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @ConsiderQuinellas, default, @BasedOnArrival)
	-- Bookings netos
	union all
	select Contract, Agency, Market, OriginallyAvailable, 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetContractAgencyMarketOriginallyAvailableBookings(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @ConsiderQuinellas, 0, @BasedOnArrival)
	-- Directas
	union all
	select Contract, Agency, Market, OriginallyAvailable, 0, 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetContractAgencyMarketOriginallyAvailableBookings(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @ConsiderQuinellas, 1, @BasedOnArrival)
	-- Shows
	union all
	select Contract, Agency, Market, OriginallyAvailable, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0
	from UFN_OR_GetContractAgencyMarketOriginallyAvailableShows(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @ConsiderQuinellas, default, @BasedOnArrival)
	-- Shows netos
	union all
	select Contract, Agency, Market, OriginallyAvailable, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0
	from UFN_OR_GetContractAgencyMarketOriginallyAvailableShows(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @ConsiderQuinellas, 1, @BasedOnArrival)
	-- Ventas
	union all
	select Contract, Agency, Market, 'ORIGINALLY AVAILABLES', 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0
	from UFN_OR_GetContractAgencyMarketOriginallyAvailableSales(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, default, @BasedOnArrival)
	-- Monto de ventas
	union all
	select Contract, Agency, Market, 'ORIGINALLY AVAILABLES', 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0
	from UFN_OR_GetContractAgencyMarketOriginallyAvailableSalesAmount(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, default, @BasedOnArrival)
	-- Ventas Self Gen
	union all
	select Contract, Agency, Market, 'ORIGINALLY AVAILABLES', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetContractAgencyMarketOriginallyAvailableSales(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, 1, @BasedOnArrival)
	-- Monto de ventas Self Gen
	union all
	select Contract, Agency, Market, 'ORIGINALLY AVAILABLES', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetContractAgencyMarketOriginallyAvailableSalesAmount(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, 1, @BasedOnArrival)
) as D
	left join Agencies A on A.agID = D.Agency
	left join Contracts C on C.cnID = D.Contract
	left join Markets M on D.Market = M.mkID
group by D.Contract, C.cnN,D.Agency, A.agN, M.mkN, D.OriginallyAvailable
order by D.OriginallyAvailable, Market, D.Agency, SalesAmount_TOTAL desc, Efficiency desc, Shows desc, Books desc, Availables desc,
	Contacts desc, Arrivals desc, D.Contract
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptProductionByMemberTypeAgencyInhouse]    Script Date: 08/19/2016 10:01:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de producccion por tipo de socio y agencia (Inhouse)
** 
** [wtorres]	26/Ene/2012 Created
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create procedure [dbo].[USP_OR_RptProductionByMemberTypeAgencyInhouse]
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@ConsiderQuinellas bit = 0,		-- Indica si se debe considerar quinielas
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
as
set nocount on

select
	-- Tipo de socio
	D.MemberType,
	-- Agencia
	D.Agency,
	IsNull(A.agN, 'Not Specified') as AgencyN,
	-- Llegadas
	Sum(D.Arrivals) as Arrivals,
	-- Contactos
	Sum(D.Contacts) as Contacts,
	-- Porcentaje de contactacion
	[dbo].UFN_OR_SecureDivision(Sum(D.Contacts), Sum(D.Arrivals)) as ContactsFactor,
	-- Disponibles
	Sum(D.Availables) as Availables,
	-- Porcentaje de disponibles
	[dbo].UFN_OR_SecureDivision(Sum(D.Availables), Sum(D.Contacts)) as AvailablesFactor,
	-- Bookings netos
	Sum(D.GrossBooks) as GrossBooks,
	-- Directas
	Sum(D.Directs) as Directs,
	-- Bookings
	Sum(D.Books) as Books,
	-- Porcentaje de Bookings
	[dbo].UFN_OR_SecureDivision(Sum(D.Books), Sum(D.Availables)) as BooksFactor,
	-- Shows netos
	Sum(D.GrossShows) as GrossShows,
	-- Shows
	Sum(D.Shows) as Shows,
	-- Porcentaje de shows
	[dbo].UFN_OR_SecureDivision(Sum(D.GrossShows), Sum(D.GrossBooks)) as ShowsFactor,
	-- Ventas
	Sum(D.Sales) as Sales_TOTAL,
	-- Monto de ventas
	Sum(D.SalesAmount) as SalesAmount_TOTAL,
	-- Ventas PR
	Sum(D.Sales - D.SalesSelfGen) as Sales_PR,
	-- Monto de ventas PR
	Sum(D.SalesAmount - D.SalesAmountSelfGen) as SalesAmount_PR,
	-- Ventas Self Gen
	Sum(D.SalesSelfGen) as Sales_SELFGEN,
	-- Monto de ventas SelfGen
	Sum(D.SalesAmountSelfGen) as SalesAmount_SELFGEN,
	-- Porcentaje de cierre
	[dbo].UFN_OR_SecureDivision(Sum(D.Sales), Sum(D.Shows)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Shows)) as Efficiency,
	-- Venta promedio
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Sales)) as AverageSale
from (
	-- Llegadas
	select MemberType, Agency, Arrivals, 0 as Contacts, 0 as Availables, 0 as Books, 0 as GrossBooks, 0 as Directs, 0 as Shows, 0 as GrossShows,
		0 as Sales, 0 as SalesAmount, 0 as SalesSelfGen, 0 as SalesAmountSelfGen
	from UFN_OR_GetMemberTypeAgencyArrivals(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies)
	-- Contactos
	union all
	select MemberType, Agency, 0, Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetMemberTypeAgencyContacts(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @BasedOnArrival)
	-- Disponibles
	union all
	select MemberType, Agency, 0, 0, Availables, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetMemberTypeAgencyAvailables(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @ConsiderQuinellas)
	-- Bookings
	union all
	select MemberType, Agency, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetMemberTypeAgencyBookings(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @ConsiderQuinellas, default, @BasedOnArrival)
	-- Bookings netos
	union all
	select MemberType, Agency, 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetMemberTypeAgencyBookings(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @ConsiderQuinellas, 0, @BasedOnArrival)
	-- Directas
	union all
	select MemberType, Agency, 0, 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetMemberTypeAgencyBookings(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @ConsiderQuinellas, 1, @BasedOnArrival)
	-- Shows
	union all
	select MemberType, Agency, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0
	from UFN_OR_GetMemberTypeAgencyShows(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @ConsiderQuinellas, default, @BasedOnArrival)
	-- Shows netos
	union all
	select MemberType, Agency, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0
	from UFN_OR_GetMemberTypeAgencyShows(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @ConsiderQuinellas, 1, @BasedOnArrival)
	-- Ventas
	union all
	select MemberType, Agency, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0
	from UFN_OR_GetMemberTypeAgencySales(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, default, @BasedOnArrival)
	-- Monto de ventas
	union all
	select MemberType, Agency, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0
	from UFN_OR_GetMemberTypeAgencySalesAmount(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, default, @BasedOnArrival)
	-- Ventas Self Gen
	union all
	select MemberType, Agency, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetMemberTypeAgencySales(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, 1, @BasedOnArrival)
	-- Monto de ventas Self Gen
	union all
	select MemberType, Agency, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetMemberTypeAgencySalesAmount(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, 1, @BasedOnArrival)
) as D
	left join Agencies A on A.agID = D.Agency
group by D.MemberType, D.Agency, A.agN
order by D.Agency, SalesAmount_TOTAL desc, Efficiency desc, Shows desc, Books desc, Availables desc, Contacts desc, Arrivals desc, D.MemberType
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouse]    Script Date: 08/19/2016 10:01:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de producccion por tipo de socio, agencia, mercado y originalmente disponible (Inhouse)
** 
** [wtorres]	26/Ene/2012 Created
** [lchairez]	06/Mar/2014 Modified. Se agrega un left join con la tabla Markets
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
*/
create procedure [dbo].[USP_OR_RptProductionByMemberTypeAgencyMarketOriginallyAvailableInhouse]
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@ConsiderQuinellas bit = 0,		-- Indica si se debe considerar quinielas
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
as
set nocount on

select
	-- Tipo de socio
	D.MemberType,
	-- Agencia
	D.Agency,
	IsNull(A.agN, 'Not Specified') as AgencyN,
	-- Mercado
	IsNull(Cast(M.mkN as varchar(13)), 'Not Specified') as Market,
	-- Originalmente disponibe
	D.OriginallyAvailable,
	-- Llegadas
	Sum(D.Arrivals) as Arrivals,
	-- Contactos
	Sum(D.Contacts) as Contacts,
	-- Porcentaje de contactacion
	[dbo].UFN_OR_SecureDivision(Sum(D.Contacts), Sum(D.Arrivals)) as ContactsFactor,
	-- Disponibles
	Sum(D.Availables) as Availables,
	-- Porcentaje de disponibles
	[dbo].UFN_OR_SecureDivision(Sum(D.Availables), Sum(D.Contacts)) as AvailablesFactor,
	-- Bookings netos
	Sum(D.GrossBooks) as GrossBooks,
	-- Directas
	Sum(D.Directs) as Directs,
	-- Bookings
	Sum(D.Books) as Books,
	-- Porcentaje de Bookings
	[dbo].UFN_OR_SecureDivision(Sum(D.Books), Sum(D.Availables)) as BooksFactor,
	-- Shows netos
	Sum(D.GrossShows) as GrossShows,
	-- Shows
	Sum(D.Shows) as Shows,
	-- Porcentaje de shows
	[dbo].UFN_OR_SecureDivision(Sum(D.GrossShows), Sum(D.GrossBooks)) as ShowsFactor,
	-- Ventas
	Sum(D.Sales) as Sales_TOTAL,
	-- Monto de ventas
	Sum(D.SalesAmount) as SalesAmount_TOTAL,
	-- Ventas PR
	Sum(D.Sales - D.SalesSelfGen) as Sales_PR,
	-- Monto de ventas PR
	Sum(D.SalesAmount - D.SalesAmountSelfGen) as SalesAmount_PR,
	-- Ventas Self Gen
	Sum(D.SalesSelfGen) as Sales_SELFGEN,
	-- Monto de ventas SelfGen
	Sum(D.SalesAmountSelfGen) as SalesAmount_SELFGEN,
	-- Porcentaje de cierre
	[dbo].UFN_OR_SecureDivision(Sum(D.Sales), Sum(D.Shows)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Shows)) as Efficiency,
	-- Venta promedio
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Sales)) as AverageSale
from (
	-- Llegadas
	select MemberType, Agency, Market, OriginallyAvailable, Arrivals, 0 as Contacts, 0 as Availables, 0 as Books, 0 as GrossBooks, 0 as Directs,
		0 as Shows, 0 as GrossShows, 0 as Sales, 0 as SalesAmount, 0 as SalesSelfGen, 0 as SalesAmountSelfGen
	from UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableArrivals(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies)
	-- Contactos
	union all
	select MemberType, Agency, Market, OriginallyAvailable, 0, Contacts, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableContacts(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @BasedOnArrival)
	-- Disponibles
	union all
	select MemberType, Agency, Market, OriginallyAvailable, 0, 0, Availables, 0, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableAvailables(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @ConsiderQuinellas)
	-- Bookings
	union all
	select MemberType, Agency, Market, OriginallyAvailable, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableBookings(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @ConsiderQuinellas, default, @BasedOnArrival)
	-- Bookings netos
	union all
	select MemberType, Agency, Market, OriginallyAvailable, 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableBookings(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @ConsiderQuinellas, 0, @BasedOnArrival)
	-- Directas
	union all
	select MemberType, Agency, Market, OriginallyAvailable, 0, 0, 0, 0, 0, Books, 0, 0, 0, 0, 0, 0
	from UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableBookings(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @ConsiderQuinellas, 1, @BasedOnArrival)
	-- Shows
	union all
	select MemberType, Agency, Market, OriginallyAvailable, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0, 0
	from UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableShows(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @ConsiderQuinellas, default, @BasedOnArrival)
	-- Shows netos
	union all
	select MemberType, Agency, Market, OriginallyAvailable, 0, 0, 0, 0, 0, 0, 0, Shows, 0, 0, 0, 0
	from UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableShows(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, @ConsiderQuinellas, 1, @BasedOnArrival)
	-- Ventas
	union all
	select MemberType, Agency, Market, 'ORIGINALLY AVAILABLES', 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0, 0, 0
	from UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableSales(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, default, @BasedOnArrival)
	-- Monto de ventas
	union all
	select MemberType, Agency, Market, 'ORIGINALLY AVAILABLES', 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount, 0, 0
	from UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableSalesAmount(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, default, @BasedOnArrival)
	-- Ventas Self Gen
	union all
	select MemberType, Agency, Market, 'ORIGINALLY AVAILABLES', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Sales, 0
	from UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableSales(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, 1, @BasedOnArrival)
	-- Monto de ventas Self Gen
	union all
	select MemberType, Agency, Market, 'ORIGINALLY AVAILABLES', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SalesAmount
	from UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableSalesAmount(@DateFrom, @DateTo, @LeadSources, @Markets, @Agencies, 1, @BasedOnArrival)
) as D
	left join Agencies A on A.agID = D.Agency
	left join Markets M on D.Market = M.mkID
group by D.MemberType, D.Agency, A.agN, M.mkN, D.OriginallyAvailable
order by D.OriginallyAvailable, Market, D.Agency, SalesAmount_TOTAL desc, Efficiency desc, Shows desc, Books desc, Availables desc,
	Contacts desc, Arrivals desc, D.MemberType
GO

/****** Object:  StoredProcedure [dbo].[USP_OR_RptUnavailableMotivesByAgency]    Script Date: 08/19/2016 10:01:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de Unavailable Motives by Angency
** 
** [lchairez]	05/Nov/2013 Created
** [wtorres]	08/Nov/2013 Modified. Cambie el tipo de datos del porcentaje de llegadas
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
CREATE PROCEDURE [dbo].[USP_OR_RptUnavailableMotivesByAgency]
	@DateFrom AS DATETIME,			-- Fecha desde
	@DateTo AS DATETIME,			-- Fecha hasta
	@LeadSources AS VARCHAR(8000),	-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL'	-- Claves de agencias
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @ArrivalsTotal AS INT
	
	-- SACAMOS LOS DATOS QUE NECESITAMOS PARA EL REPORTE
	SELECT  um.umID, um.umN UnavailMot, mk.mkID, mk.mkN Market, ag.agID, ag.agN Agency
		,COUNT(*) Arrivals, CAST(0 AS DECIMAL(12,6)) as ArrivalsPercentage, SUM(CASE WHEN guAvail <> guAvailBySystem THEN 1 ELSE 0 END) ByUser
	INTO #Data
	FROM Guests gu
	LEFT JOIN dbo.UnavailMots um on gu.guum = um.umID
	LEFT JOIN Agencies ag on gu.guAg = ag.agID
	LEFT JOIN Markets mk on ag.agmk = mk.mkID
	WHERE 
	-- Fecha de llegada
	 gu.guCheckInD between @DateFrom and @DateTo
	 -- Con Check In
	 AND gu.guCheckIn = 1
	 -- No Rebook
	 AND gu.guRef is null
	 -- Lead Source
	 AND gu.guls in (select item from split(@LeadSources, ','))
	 -- No disponible
	 AND gu.guAvail = 0 and gu.guum > 0
	 -- Markets
	 AND (@Markets = 'ALL' or mk.mkID in (select item from Split(@Markets, ',')))
	 -- Agencies
	 AND (@Agencies = 'ALL' or gu.guAg in (select item from Split(@Agencies, ',')))
	GROUP BY um.umID, um.umN, mk.mkID, mk.mkN, ag.agID, ag.agN
	ORDER BY um.umN, mk.mkN, ag.agN
	
	-- OBTENEMOS LA CANTIDAD DE MERCADOS DE ACUERDO A SU MOTIVO
	SET @ArrivalsTotal = (SELECT SUM(Arrivals) FROM #Data)
	
	-- CALCULAMOS EL PORCENTAJE DE LAS LLEGADAS
	UPDATE #Data SET ArrivalsPercentage = CAST(Arrivals AS DECIMAL(12,6)) / @ArrivalsTotal

	-- DELVOLVEMOS LOS DATOS DEL REPORTE
	SELECT * FROM #Data
	
DROP TABLE #Data
	
END
GO


