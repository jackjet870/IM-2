if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GraphUnavailableArrivals]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GraphUnavailableArrivals]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para la grafica de llegadas no disponibles
** 
** [wtorres]	29/Jul/2010 Ahora se pasa la lista de Lead Sources como un solo parametro
** [wtorres]	03/Nov/2011 Agregue los campos de no disponibles por sistema y no disponibles por usuario
**
*/
create procedure [dbo].[USP_OR_GraphUnavailableArrivals]
	@DateFromWeek datetime,		-- Fecha desde de la semana
	@LeadSources varchar(8000)	-- Claves de Lead Sources
as
set nocount on

declare 
	@DateToWeek datetime,		-- Fecha hasta de la semana
	@DateFromMonth datetime,	-- Fecha desde del mes
	@DateToMonth datetime,		-- Fecha hasta del mes
	@TotalArrivalsWeek int,		-- Total de llegadas no disponibles en la semana
	@TotalArrivalsMonth int		-- Total de llegadas no disponibles en el mes

-- definimos las fechas semanales
set @DateToWeek = @DateFromWeek + 6

-- definimos las fechas mensuales
set @DateFromMonth = DateAdd(d, Day(@DateFromWeek) * -1 + 1, @DateFromWeek)
if (Month(@DateFromWeek) = Month(@DateToWeek))
	set @DateToMonth = @DateToWeek
else
	set @DateToMonth = DateAdd(m, 1, @DateFromMonth) - 1

-- =============================================
--     Llegadas no disponibles en la semana
-- =============================================
select
	U.umN as UnavailableMotiveN,
	Arrivals,
	Cast(0 as money) as Percentage,
	ByUser
into #tblWeek
from dbo.UFN_OR_GetUnavailableMotiveArrivals(@DateFromWeek, @DateToWeek, @LeadSources) A
	inner join UnAvailMots U on A.UnavailableMotive = U.umID

-- Total de llegadas no disponibles en la semana
set @TotalArrivalsWeek = (select Sum(Arrivals) from #tblWeek)

-- actualizamos el porcentaje de llegadas no disponibles en la semana
update #tblWeek set Percentage = dbo.UFN_OR_SecureDivision(Arrivals, @TotalArrivalsWeek)

-- =============================================
--		Llegadas no disponibles en el mes
-- =============================================
select
	U.umN as UnavailableMotiveN,
	Arrivals,
	Cast(0 as money) as Percentage,
	ByUser
into #tblMonth
from dbo.UFN_OR_GetUnavailableMotiveArrivals(@DateFromMonth, @DateToMonth, @LeadSources) A
	inner join UnAvailMots U on A.UnavailableMotive = U.umID

-- Total de llegadas no disponibles en el mes
set @TotalArrivalsMonth = (select Sum(Arrivals) from #tblMonth)

-- actualizamos el porcentaje de llegadas no disponibles en el mes
update #tblMonth set Percentage = dbo.UFN_OR_SecureDivision(Arrivals, @TotalArrivalsMonth)

-- 1. Maximos y total de llegadas
select
	IsNull((select Round(Max(Arrivals) + 5, -1) from #tblWeek), 0) as MaxGraphW,
	IsNull((select Round(Max(Arrivals) + 5, -1) from #tblMonth), 0) as MaxGraphM,
	@TotalArrivalsWeek as TotalW,
	@TotalArrivalsMonth as TotalM

-- 2. Llegadas no disponibles en la semana
select * from #tblWeek order by Arrivals desc

-- 3. Llegadas no disponibles en el mes
select * from #tblMonth order by Arrivals desc

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

