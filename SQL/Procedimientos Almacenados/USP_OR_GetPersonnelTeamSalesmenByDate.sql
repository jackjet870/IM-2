if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetPersonnelTeamSalesmenByDate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetPersonnelTeamSalesmenByDate]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene el equipo y la sala de un vendedor en una fecha determinada
** 
** [wtorres]	20/Ene/2009 Creado
** [wtorres]	12/Jun/2012 Ahora se valida que los registros del historico dde equipos no sean registros de equipos de guest services
**
*/
create procedure [dbo].[USP_OR_GetPersonnelTeamSalesmenByDate]	
	@PersonnelID varchar(10),	-- Clave del personal
	@Date datetime				-- Fecha desde
as
set nocount on

declare
	@TeamsLogCount int,	-- Numero de registros encontrados en el historico de equipos
	@PersonnelCount int	-- Numero de registros encontrados en el catalogo de personal

-- consultamos el historico de equipos
select @TeamsLogCount = Count(tlDT)
from TeamsLog
where
	-- Personal
	tlpe = @PersonnelID
	-- Con fecha anterior a la fecha enviada
	and DateDiff(Day, tlDT, @Date) >= 0
	-- No equipos de Guest Services
	and tlTeamType <> 'GS'

-- si se encontro registros en el historico de equipos
if @TeamsLogCount > 0
	select
		tlTeam as Team,
		tsN as TeamN,
		tsLeader as TeamLeader,
		peN as TeamLeaderN,
		tlPlaceID as SalesRoom, 
		srN as SalesRoomN
	from TeamsLog
	left join TeamsSalesmen on tlTeam = tsID and tlPlaceID = tssr
	left join Personnel on tsLeader = peID
	left join SalesRooms on tlPlaceID = srID
	where
		-- Personal
		tlpe = @PersonnelID
		-- Fecha
		and tlDT = (
			select Max(tlDT) 
			from TeamsLog 
			where
				-- Personal
				tlpe = @PersonnelID
				-- Con fecha anterior a la fecha enviada
				and DateDiff(Day, tlDT, @Date) >= 0
				-- No equipos de Guest Services
				and tlTeamType <> 'GS'
		)
		-- No equipos de Guest Services 
		and tlTeamType <> 'GS'
else
begin
	-- consultamos el catalogo de personal
	select @PersonnelCount = Count(peID)
	from Personnel
	where peID = @PersonnelID

	-- si el personal existe
	if @PersonnelCount > 0
		select
			P.peTeam as Team,
			tsN as TeamN,
			tsLeader as TeamLeader,
			L.peN as TeamLeaderN,
			P.pePlaceID as SalesRoom, 
			srN as SalesRoomN
		from Personnel P
		left join TeamsSalesmen on P.peTeam = tsID and P.pePlaceID = tssr
		left join Personnel L on tsLeader = L.peID
		left join SalesRooms on P.pePlaceID = srID
		where P.peID = @PersonnelID
	
	-- si el personal no existe
	else
		select
			'' as Team,
			'' as TeamN,
			'' as TeamLeader,
			'' as TeamLeaderN,
			'' as SalesRoom,
			'' as SalesRoomN
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

