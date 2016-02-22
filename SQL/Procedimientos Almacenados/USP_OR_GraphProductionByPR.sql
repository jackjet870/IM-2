USE [OrigosVCPalace];
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para la grafica de produccion de PR
** 
** [wtorres]		24/Abr/2009 Modified. Ahora se pasa la lista de Lead Sources como un solo parametro
** [wtorres]		23/Sep/2009 Modified. Ahora ya devuelve los datos agrupados
** [lormartinez]	22/Sep/2014 Modified. Se agrega parametro para filtro basado en fecha de booking
** [lormartinez]  16/Dic/2015 Modified. Se modifica llamado a funcion 
                  GetPRBooking y GetPRShows para agregar parametro opcional (paymenttypes)
**
*/
CREATE procedure [dbo].[USP_OR_GraphProductionByPR]
	@DateFrom datetime,		-- Fecha desde
	@DateTo datetime,		-- Fecha hasta
	@LeadSource varchar(10)	-- Clave de Lead Source
as
set nocount on

select
	-- Clave del PR
	D.PR,
	-- Bookings
	Sum(D.Books) as Books,
	-- Shows
	Sum(D.Shows) as Shows,
	-- Ventas
	Sum(D.Sales) as Sales
from (
	-- Bookings
	select PR, Books, 0 as Shows, 0 as Sales
	from UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSource, default, default, default, default, default, default, default,default)
	-- Shows
	union all
	select PR, 0, Shows, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSource, default, default, default, default, default, default, default, default, default, default,
		default, default,default)
	-- Ventas
	union all
	select PR, 0, 0, Sales
	from UFN_OR_GetPRSales(@DateFrom, @DateTo, @LeadSource, default, default, default, default, default, default, default, default, default)
) as D
group by D.PR
order by Sales desc, Shows desc, Books desc
GO