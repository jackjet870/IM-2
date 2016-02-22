if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GraphProduction]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GraphProduction]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para la grafica de produccion
** 
** [wtorres]	06/May/2010 Ahora se pasa la lista de Lead Sources como un solo parametro
**					- Ahora se utilizan funciones
** [wtorres]	30/Jul/2010 Eliminacion de las variables de fechas
** [wtorres]	24/Nov/2010 Agregue el parametro @BasedOnArrival
** [wtorres]	19/Dic/2013 Agregue el parametro @ConsiderQuinellas
**
*/
create procedure [dbo].[USP_OR_GraphProduction]
	@DateFromWeek datetime,		-- Fecha desde de la semana
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0,	-- Indica si se debe considerar quinielas
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada
as
set nocount on

declare 
	@DateToWeek datetime,		-- Fecha hasta de la semana
	@DateFromMonth datetime,	-- Fecha desde del mes
	@DateToMonth datetime,		-- Fecha hasta del mes
	@Periods int,				-- Número de periodos
	@Period int,				-- Número de periodo
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime			-- Fecha hasta

-- Define las fechas semanales
set @DateToWeek = @DateFromWeek + 6

-- Define las fechas mensuales
set @DateFromMonth = DateAdd(d, (Day(@DateFromWeek) * -1) + 1, @DateFromWeek)
if (Month(@DateFromWeek) = Month(@DateToWeek))
 	set @DateToMonth = @DateToWeek
else
 	set @DateToMonth = DateAdd(m, 1, @DateFromMonth) - 1

-- Define el número de periodos
set @Periods = 3

--			Estadisticas semanales
-- =============================================
create table #tblWeek (
	Period tinyint,
	Arrivals int,
	Availables int,
	Contacts int,
	Books int,
	Shows int,
	Sales int
)
set @Period = 0
while @Period < @Periods
begin
	set @Period = @Period + 1
	set @DateFrom = @DateFromWeek - ((@Periods - @Period) * 7)
	set @DateTo = @DateToWeek - ((@Periods - @Period) * 7)
	
	insert into #tblWeek
	select @Period,
		dbo.UFN_OR_GetArrivals(@DateFrom, @DateTo, @LeadSources),
		dbo.UFN_OR_GetAvailables(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas),
		dbo.UFN_OR_GetContacts(@DateFrom, @DateTo, @LeadSources, @BasedOnArrival),
		dbo.UFN_OR_GetBookings(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, @BasedOnArrival),
		dbo.UFN_OR_GetShows(@DateFrom, @DateTo, @LeadSources, default, @ConsiderQuinellas, default, 1, @BasedOnArrival),
		dbo.UFN_OR_GetSales(@DateFrom, @DateTo, @LeadSources, default, default, default, default, @BasedOnArrival)
end

--			Estadisticas mensuales
-- =============================================
create table #tblMonth (
	Period tinyint,
	Arrivals int,
	Availables int,
	Contacts int,
	Books int,
	Shows int,
	Sales int,	
	SalesAmount money
)

set @Period = 0
while @Period < @Periods
begin
	set @Period = @Period + 1
	set @DateFrom = DateAdd(m, @Period - @Periods, @DateFromMonth)
	set @DateTo = case when @Period = @Periods then @DateToMonth
		else DateAdd(m, @Period - @Periods + 1, @DateFromMonth) - 1
	end
	
	insert into #tblMonth
	select @Period,
		dbo.UFN_OR_GetArrivals(@DateFrom, @DateTo, @LeadSources),
		dbo.UFN_OR_GetAvailables(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas),		
		dbo.UFN_OR_GetContacts(@DateFrom, @DateTo, @LeadSources, @BasedOnArrival),		
		dbo.UFN_OR_GetBookings(@DateFrom, @DateTo, @LeadSources, @ConsiderQuinellas, @BasedOnArrival),
		dbo.UFN_OR_GetShows(@DateFrom, @DateTo, @LeadSources, default, @ConsiderQuinellas, default, 1, @BasedOnArrival),
		dbo.UFN_OR_GetSales(@DateFrom, @DateTo, @LeadSources, default, default, default, default, @BasedOnArrival),
		dbo.UFN_OR_GetSalesAmount(@DateFrom, @DateTo, @LeadSources, default, default, default, default, @BasedOnArrival)
end

-- 1. Semanas
select
	Period,
	Arrivals,
	Availables,
	dbo.UFN_OR_SecureDivision(Availables, Arrivals) as AvailablesFactor,
	Contacts,
	dbo.UFN_OR_SecureDivision(Contacts, Availables) as ContactsFactor,
	Books,
	dbo.UFN_OR_SecureDivision(Books, Availables) as BooksFactor,
	Shows,
	dbo.UFN_OR_SecureDivision(Shows, Books) as ShowsFactor,
	Sales,
	dbo.UFN_OR_SecureDivision(Sales, Shows) as ClosingFactor
from #tblWeek

-- 2. Meses
select
	Period,
	Arrivals,
	Availables,
	dbo.UFN_OR_SecureDivision(Availables, Arrivals) as AvailablesFactor,
	Contacts,
	dbo.UFN_OR_SecureDivision(Contacts, Availables) as ContactsFactor,
	Books,
	dbo.UFN_OR_SecureDivision(Books, Availables) as BooksFactor,
	Shows,
	dbo.UFN_OR_SecureDivision(Shows, Books) as ShowsFactor,
	Sales,
	dbo.UFN_OR_SecureDivision(Sales, Shows) as ClosingFactor,
	SalesAmount,
	dbo.UFN_OR_SecureDivision(SalesAmount, Shows) as Efficiency
from #tblMonth

-- 3. Maximos
select
	MaxGraphW = (select Round(Max(Arrivals) + 5, -1) from #tblWeek),
	MaxGraphM = (select Round(Max(Arrivals) + 5, -1) from #tblMonth)

SET QUOTED_IDENTIFIER ON 
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

