if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetPRAssigns]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetPRAssigns]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de asignaciones por PR
** 
** [wtorres]	18/Sep/2009 Created
** [wtorres]	23/Sep/2009 Modified. Convertido a funcion. Agregue el parametro @LeadSources
** [erosado]	24/Feb/2016 Modified. Agregue los parametros @SalesRooms, @Countries, @Agencies y @Markets
**
*/
create function [dbo].[UFN_OR_GetPRAssigns](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(max) = 'ALL',	-- Clave de los Lead Sources
	@SalesRooms varchar(max) = 'ALL',	-- Clave de las salas de ventas
	@Countries varchar(max) = 'ALL',	-- Clave de los paises
	@Agencies varchar(max) = 'ALL',		-- Clave de las agencias
	@Markets varchar(max) = 'ALL'		-- Clave de los mercados
)
returns @Table table (
	PR varchar(10),
	Assigns int
)
as
begin

insert @Table
select
	guPRAssign,
	Count(*)
from Guests
where
	-- PR asignado
	guPRAssign is not null
	-- Fecha de llegada
	and guCheckInD between @DateFrom and @DateTo
	-- No rebook
	and guRef is null
	-- Con Check In
	and guCheckIn = 1
	-- No antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
	-- Sala de ventas
	and (@SalesRooms = 'ALL' or gusr in (select item from split(@SalesRooms, ',')))
	-- Pais
	and (@Countries = 'ALL' or guco in (select item from split(@Countries, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
	-- Mercado
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
group by guPRAssign

return
end