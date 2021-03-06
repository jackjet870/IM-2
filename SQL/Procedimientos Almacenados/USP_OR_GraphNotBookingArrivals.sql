if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GraphNotBookingArrivals]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GraphNotBookingArrivals]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Procedimiento:	Reporte de llegadas no booking
-- Descripción:		Devuelve los datos para el reporte
-- Histórico:		[wtorres] 30/Jul/2010 Creado
-- =============================================
create procedure [dbo].[USP_OR_GraphNotBookingArrivals]
	@DateFromWeek datetime,		-- Fecha desde de la semana
	@LeadSources varchar(8000)	-- Claves de Lead Sources
as
set nocount on

declare 
	@DateToWeek datetime,		-- Fecha hasta de la semana
	@DateFromMonth datetime,	-- Fecha desde del mes
	@DateToMonth datetime,		-- Fecha hasta del mes
	@TotalArrivalsWeek int,		-- Total de llegadas no booking en la semana
	@TotalArrivalsMonth int		-- Total de llegadas no booking en el mes

-- Define las fechas semanales
set @DateToWeek = @DateFromWeek + 6

-- Define las fechas mensuales
set @DateFromMonth = DateAdd(d, Day(@DateFromWeek) * -1 + 1, @DateFromWeek)
if (Month(@DateFromWeek) = Month(@DateToWeek))
	set @DateToMonth = @DateToWeek
else
	set @DateToMonth = DateAdd(m, 1, @DateFromMonth) - 1

-- Llegadas no booking en la semana
-- =============================================
select
	N.nbN as NotBookingMotiveN,
	Arrivals,
	Cast(0 as money) as Percentage
into #tblWeek
from dbo.UFN_OR_GetNotBookingMotiveArrivals(@DateFromWeek, @DateToWeek, @LeadSources) A
	inner join NotBookingMotives N on A.NotBookingMotive = N.nbID

-- Total de llegadas no booking en la semana
set @TotalArrivalsWeek = (select Sum(Arrivals) from #tblWeek)

-- Actualiza el porcentaje de llegadas no booking en la semana
update #tblWeek set Percentage = dbo.UFN_OR_SecureDivision(Arrivals, @TotalArrivalsWeek)

-- Llegadas no booking en el mes
-- =============================================
select
	N.nbN as NotBookingMotiveN,
	Arrivals,
	Cast(0 as money) as Percentage
into #tblMonth
from dbo.UFN_OR_GetNotBookingMotiveArrivals(@DateFromMonth, @DateToMonth, @LeadSources) A
	inner join NotBookingMotives N on A.NotBookingMotive = N.nbID

-- Total de llegadas no booking en el mes
set @TotalArrivalsMonth = (select Sum(Arrivals) from #tblMonth)

-- Actualiza el porcentaje de llegadas no booking en el mes
update #tblMonth set Percentage = dbo.UFN_OR_SecureDivision(Arrivals, @TotalArrivalsMonth)

-- 1. Máximos y total de llegadas
select
	MaxGraphW = IsNull((select Round(Max(Arrivals) + 5, -1) from #tblWeek), 0),
	MaxGraphM = IsNull((select Round(Max(Arrivals) + 5, -1) from #tblMonth), 0),
	TotalW = @TotalArrivalsWeek,
	TotalM = @TotalArrivalsMonth

-- 2. Llegadas no booking en la semana
select * from #tblWeek order by Arrivals desc

-- 3. Llegadas no booking en el mes
select * from #tblMonth order by Arrivals desc

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

