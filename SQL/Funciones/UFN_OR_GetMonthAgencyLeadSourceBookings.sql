if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetMonthAgencyLeadSourceBookings]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetMonthAgencyLeadSourceBookings]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de bookings por mes, agencia y Lead Source
** 
** [wtorres]	22/Oct/2009 Created
** [wtorres]	14/May/2010 Modified. Agregue el parametro @ConsiderDirects
** [wtorres]	24/Nov/2010 Modified. Agregue el parametro @BasedOnArrival y reemplace el parametro @ConsiderDirects por @Direct
** [wtorres]	26/Nov/2010 Modified. Ahora no se cuentan los bookings cancelados
** [wtorres]	20/Dic/2013 Modified. Agregue el parametro @ConsiderQuinellas
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetMonthAgencyLeadSourceBookings](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@ConsiderQuinellas bit = 0,		-- Indica si se debe considerar quinielas
	@Direct int = -1,				-- Filtro de directas:
									--		-1. Sin filtro
									--		 0. No directas
									--		 1. Directas
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	[Year] int,
	[Month] int,
	Agency varchar(35),
	LeadSource varchar(10),
	Books int
)
as
begin

-- si no se debe basar en la fecha de llegada
if @BasedOnArrival = 0

	insert @Table
	select
		Year(G.guBookD),
		Month(G.guBookD),
		G.guag,
		G.guls,
		Sum(case when @ConsiderQuinellas = 0 then 1 else G.guRoomsQty end)
	from Guests G
		left join LeadSources L on G.guls = L.lsID
	where
		-- Fecha de booking
		G.guBookD between @DateFrom and @DateTo
		-- No Antes In & Out
		and G.guAntesIO = 0
		-- No Quiniela Split
		and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
		-- Agencias
		and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
		-- Programa
		and L.lspg = 'IH'
		-- Originalmente disponibles
		and (G.guOriginAvail = 1 or G.guInvit = 1)
		-- Directas
		and (@Direct = -1 or G.guDirect = @Direct)
		-- No bookings cancelados
		and G.guBookCanc = 0
	group by Year(G.guBookD), Month(G.guBookD), G.guag, G.guls

-- si se debe basar en la fecha de llegada
else

	insert @Table
	select
		Year(G.guCheckInD),
		Month(G.guCheckInD),
		G.guag,
		G.guls,
		Sum(case when @ConsiderQuinellas = 0 then 1 else G.guRoomsQty end)
	from Guests G
		left join LeadSources L on G.guls = L.lsID
	where
		-- Fecha de llegada
		G.guCheckInD between @DateFrom and @DateTo
		-- Invitado
		and G.guInvit = 1
		-- No Antes In & Out
		and G.guAntesIO = 0
		-- No Quiniela Split
		and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
		-- Agencias
		and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
		-- Programa
		and L.lspg = 'IH'
		-- Originalmente disponibles
		and (G.guOriginAvail = 1 or G.guInvit = 1)
		-- Directas
		and (@Direct = -1 or G.guDirect = @Direct)
		-- No bookings cancelados
		and G.guBookCanc = 0
	group by Year(G.guCheckInD), Month(G.guCheckInD), G.guag, G.guls

return
end