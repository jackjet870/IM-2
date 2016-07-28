USE [OrigosVCPalace]
GO

/****** Object:  UserDefinedFunction [dbo].[Split]    Script Date: 07/28/2016 13:16:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Split]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[Split]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetDateLeadSourcePRCountryMarketAgeAvailables]    Script Date: 07/28/2016 13:16:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetDateLeadSourcePRCountryMarketAgeAvailables]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetDateLeadSourcePRCountryMarketAgeAvailables]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRAssigns]    Script Date: 07/28/2016 13:16:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetPRAssigns]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetPRAssigns]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRAvailables]    Script Date: 07/28/2016 13:16:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetPRAvailables]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetPRAvailables]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRBookings]    Script Date: 07/28/2016 13:16:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetPRBookings]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetPRBookings]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRContacts]    Script Date: 07/28/2016 13:16:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetPRContacts]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetPRContacts]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRDeposits]    Script Date: 07/28/2016 13:16:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetPRDeposits]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetPRDeposits]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRGroupAvailables]    Script Date: 07/28/2016 13:16:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetPRGroupAvailables]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetPRGroupAvailables]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRGroupBookings]    Script Date: 07/28/2016 13:16:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetPRGroupBookings]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetPRGroupBookings]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRGroupContacts]    Script Date: 07/28/2016 13:16:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetPRGroupContacts]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetPRGroupContacts]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRGroupSales]    Script Date: 07/28/2016 13:16:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetPRGroupSales]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetPRGroupSales]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRGroupSalesAmount]    Script Date: 07/28/2016 13:16:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetPRGroupSalesAmount]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetPRGroupSalesAmount]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRGroupShows]    Script Date: 07/28/2016 13:16:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetPRGroupShows]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetPRGroupShows]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRNotQualifieds]    Script Date: 07/28/2016 13:16:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetPRNotQualifieds]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetPRNotQualifieds]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRQualifieds]    Script Date: 07/28/2016 13:16:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetPRQualifieds]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetPRQualifieds]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRSales]    Script Date: 07/28/2016 13:16:37 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetPRSales]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetPRSales]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRSalesAmount]    Script Date: 07/28/2016 13:16:37 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetPRSalesAmount]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetPRSalesAmount]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRSalesRoomAvailables]    Script Date: 07/28/2016 13:16:38 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetPRSalesRoomAvailables]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetPRSalesRoomAvailables]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRSalesRoomBookings]    Script Date: 07/28/2016 13:16:38 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetPRSalesRoomBookings]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetPRSalesRoomBookings]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRSalesRoomContacts]    Script Date: 07/28/2016 13:16:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetPRSalesRoomContacts]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetPRSalesRoomContacts]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRSalesRoomSales]    Script Date: 07/28/2016 13:16:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetPRSalesRoomSales]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetPRSalesRoomSales]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRSalesRoomSalesAmount]    Script Date: 07/28/2016 13:16:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetPRSalesRoomSalesAmount]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetPRSalesRoomSalesAmount]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRSalesRoomShows]    Script Date: 07/28/2016 13:16:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetPRSalesRoomShows]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetPRSalesRoomShows]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRShows]    Script Date: 07/28/2016 13:16:41 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetPRShows]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetPRShows]
GO

USE [OrigosVCPalace]
GO

/****** Object:  UserDefinedFunction [dbo].[Split]    Script Date: 07/28/2016 13:16:41 ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER ON
GO




/*
** Palace Resorts
** Grupo de Desarrollo Merida
** 
** [wgonzalez]	12.05.2006
** [aalcocer]	10/Jun/2016 Se modifica el tipo de dato del campo @String a varchar(max)
**
** Implementación de la funcion SPLIT para devolver un arreglo o tabla
** con los valores recuperados.
*/

create FUNCTION [dbo].[Split]( @String varchar(max), @Delimiter char(1) )

RETURNS @Results TABLE (item varchar(8000))

AS

	BEGIN
		DECLARE @INDEX INT
		DECLARE @SLICE varchar(8000)

		SET @INDEX = 1

		IF @String IS NULL RETURN
	
		WHILE @INDEX !=0
		BEGIN	

			SET @INDEX = CHARINDEX( @Delimiter, @STRING )

			IF @INDEX !=0
				SET @SLICE = LEFT( @STRING, @INDEX - 1 )
			ELSE
				SET @SLICE = @STRING

			INSERT INTO @Results(item) VALUES( @SLICE )
			
			SET @STRING = RIGHT( @STRING, LEN(@STRING) - @INDEX )

			IF LEN( @STRING ) = 0 BREAK

		END -- WHILE
		
		RETURN

	END 




GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetDateLeadSourcePRCountryMarketAgeAvailables]    Script Date: 07/28/2016 13:16:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de disponibles por fecha, Lead Source, PR, pais, mercado y edad
** 
** [wtorres]		25/Jun/2010 Created
** [edgrodriguez]	05/May/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/

CREATE function [dbo].[UFN_OR_GetDateLeadSourcePRCountryMarketAgeAvailables](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@ConsiderQuinellas bit = 0	-- Indica si se debe considerar quinielas
)
returns @Table table (
	[Date] datetime,
	LeadSource varchar(10),
	PR varchar(10),
	Country varchar(25),
	Market varchar(10),
	Age tinyint,
	Availables money
)
as
begin

insert @Table

-- Disponibles
-- =============================================
select
	guCheckInD,
	guls,
	guPRInfo,
	guco,
	gumk,
	guAge1,
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then 1 else guShowsQty end)
from Guests
where
	-- Fecha de llegada
	guCheckInD between @DateFrom and @DateTo
	-- Con Check In
	and guCheckIn = 1
	-- Disponible
	and guAvail = 1
	-- No Rebook
	and guRef is null
	-- Contactado
	and guInfo = 1
	-- No Antes In & Out
	and guAntesIO = 0
group by guCheckInD, guls, guPRInfo, guco, gumk, guAge1

-- Reservaciones donde los PR de invitación 1 y contacto son diferentes
-- =============================================
union all
select
	guBookD,
	guls,
	guPRInvit1,
	guco,
	gumk,
	guAge1,
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT) else 
		guShowsQty * dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT) end)
from Guests
where
	-- PR 1
	guPRInvit1 is not null
	-- PR de contacto diferente de los PR de invitación
	and guPRInfo not in (IsNull(guPRInvit1, ''), IsNull(guPRInvit2, ''), IsNull(guPRInvit3, ''))
	-- Fecha de reservación
	and guBookD between @DateFrom and @DateTo
	-- No Antes In & Out
	and guAntesIO = 0
	-- Disponible
	and guAvail = 1
group by guBookD, guls, guPRInvit1, guco, gumk, guAge1

-- Reservaciones donde los PR de invitación 2 y contacto son diferentes
-- =============================================
union all
select
	guBookD,
	guls,
	guPRInvit2,
	guco,
	gumk,
	guAge1,
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT) else 
		guShowsQty * dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT) end)
from Guests
where
	-- PR 2
	guPRInvit2 is not null
	-- PR de contacto diferente de los PR de invitación
	and guPRInfo not in (IsNull(guPRInvit1, ''), IsNull(guPRInvit2, ''), IsNull(guPRInvit3, ''))
	-- Fecha de reservación
	and guBookD between @DateFrom and @DateTo
	-- No Antes In & Out
	and guAntesIO = 0
	-- Disponible
	and guAvail = 1
group by guBookD, guls, guPRInvit2, guco, gumk, guAge1

-- Reservaciones donde los PR de invitación 3 y contacto son diferentes
-- =============================================
union all
select
	guBookD,
	guls,
	guPRInvit3,
	guco,
	gumk,
	guAge1,
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT) else 
		guShowsQty * dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT) end)
from Guests
where
	-- PR 3
	guPRInvit3 is not null
	-- PR de contacto diferente de los PR de invitación
	and guPRInfo not in (IsNull(guPRInvit1, ''), IsNull(guPRInvit2, ''), IsNull(guPRInvit3, ''))
	-- Fecha de reservación
	and guBookD between @DateFrom and @DateTo
	-- No Antes In & Out
	and guAntesIO = 0
	-- Disponible
	and guAvail = 1
group by guBookD, guls, guPRInvit3, guco, gumk, guAge1

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRAssigns]    Script Date: 07/28/2016 13:16:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRAvailables]    Script Date: 07/28/2016 13:16:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de disponibles por PR
** 
** [wtorres]		18/Sep/2009 Created
** [wtorres]		23/Sep/2009 Modified. Convertido a funcion. Agregue el parametro @LeadSources
** [wtorres]		12/Jul/2010 Modified. Agregue el parametro @ConsiderFollow
** [wtorres]		12/Ago/2010 Modified. Agregue el parametro @ConsiderQuinellas
** [wtorres]		18/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [erosado]		04/Mar/2016 Modified. Agregue los parametros @SalesRooms, @Countries, @Agencies y @Markets
** [edgrodriguez]	05/May/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/
create function [dbo].[UFN_OR_GetPRAvailables](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(max)='ALL',	-- Clave de los Lead Sources
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@ConsiderFollow bit = 0,			-- Indica si se debe considerar seguimiento
	@BasedOnArrival bit = 0,			-- Indica si se debe basar en la fecha de llegada	
	@SalesRooms varchar(max) = 'ALL',	-- Clave de las salas de ventas
	@Countries varchar(max) = 'ALL',	-- Clave de los paises
	@Agencies varchar(max) = 'ALL',		-- Clave de las agencias
	@Markets varchar(max) = 'ALL'		-- Clave de los mercados
)
returns @Table table (
	PR varchar(10),
	Availables money
)
as
begin

insert @Table

-- Disponibles (PR de contacto)
-- =============================================
select
	guPRInfo,
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then 1 else guShowsQty end)
from Guests
where
	-- PR Info
	guPRInfo is not null
	-- Fecha de contacto
	and ((@BasedOnArrival = 0 and guInfoD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Contactado
	and guInfo = 1))
	-- No Antes In & Out
	and guAntesIO = 0
	-- Disponible
	and guAvail = 1
	-- Lead Source
	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or gusr in (select item from split(@SalesRooms, ',')))
	-- Countries
	and (@Countries = 'ALL' or guco in (select item from split(@Countries, ',')))
	-- Agencies
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
	-- Markets
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
group by guPRInfo

-- Disponibles (PR 1)
-- =============================================
union all
select
	guPRInvit1,
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT)
		else guShowsQty * dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT) end)
from Guests
where
	-- PR 1
	guPRInvit1 is not null
	-- PR de contacto diferente de los PR de invitacion
	and guPRInfo not in (IsNull(guPRInvit1, ''), IsNull(guPRInvit2, ''), IsNull(guPRInvit3, ''))
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and guInvit = 1))
	-- No Antes In & Out
	and guAntesIO = 0
	-- Disponible
	and guAvail = 1
	-- Lead Source
	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or gusr in (select item from split(@SalesRooms, ',')))
	-- Countries
	and (@Countries = 'ALL' or guco in (select item from split(@Countries, ',')))
	-- Agencies
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
	-- Markets
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
group by guPRInvit1

-- Disponibles (PR 2)
-- =============================================
union all
select
	guPRInvit2,
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT)
		else guShowsQty * dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT) end)
from Guests
where
	-- PR 2
	guPRInvit2 is not null
	-- PR de contacto diferente de los PR de invitacion
	and guPRInfo not in (IsNull(guPRInvit1, ''), IsNull(guPRInvit2, ''), IsNull(guPRInvit3, ''))
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and guInvit = 1))
	-- No Antes In & Out
	and guAntesIO = 0
	-- Disponible
	and guAvail = 1
	-- Lead Source
	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or gusr in (select item from split(@SalesRooms, ',')))
	-- Countries
	and (@Countries = 'ALL' or guco in (select item from split(@Countries, ',')))
	-- Agencies
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
	-- Markets
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
group by guPRInvit2

-- Disponibles (PR 3)
-- =============================================
union all
select
	guPRInvit3,
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT)
		else guShowsQty * dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT) end)
from Guests
where
	-- PR 3
	guPRInvit3 is not null
	-- PR de contacto diferente de los PR de invitacion
	and guPRInfo not in (IsNull(guPRInvit1, ''), IsNull(guPRInvit2, ''), IsNull(guPRInvit3, ''))
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and guInvit = 1))
	-- No Antes In & Out
	and guAntesIO = 0
	-- Disponible
	and guAvail = 1
	-- Lead Source
	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or gusr in (select item from split(@SalesRooms, ',')))
	-- Countries
	and (@Countries = 'ALL' or guco in (select item from split(@Countries, ',')))
	-- Agencies
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
	-- Markets
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
group by guPRInvit3

-- Disponibles (PR de seguimiento)
-- =============================================
if @ConsiderFollow = 1
	insert @Table
	select
		guPRFollow,
		Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then 1 else guShowsQty end)
	from Guests
	where
		-- PR de seguimiento
		guPRFollow is not null
		-- PR de contacto diferente del PR de seguimiento
		and guPRInfo <> guPRFollow
		-- PR de seguimiento diferente de los PR de invitacion
		and guPRFollow not in (IsNull(guPRInvit1, ''), IsNull(guPRInvit2, ''), IsNull(guPRInvit3, ''))
		-- Fecha de seguimiento
		and ((@BasedOnArrival = 0 and guFollowD between @DateFrom and @DateTo)
		-- Fecha de llegada
		or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
		-- Con seguimiento
		and guFollow = 1))
		-- No Antes In & Out
		and guAntesIO = 0
		-- Disponible
		and guAvail = 1
		-- Lead Source
		and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
		-- Sales Rooms
		and (@SalesRooms = 'ALL' or gusr in (select item from split(@SalesRooms, ',')))
		-- Countries
		and (@Countries = 'ALL' or guco in (select item from split(@Countries, ',')))
		-- Agencies
		and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
		-- Markets
		and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
	group by guPRFollow

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRBookings]    Script Date: 07/28/2016 13:16:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de bookings por PR
** 
** [wtorres]		18/Sep/2009 Created
** [wtorres]		23/Sep/2009 Modified. Convertido a funcion. Agregue los parametros @LeadSources, @ConsiderAntesInOut, @ConsiderQuinellasSplit
** [wtorres]		27/Oct/2009 Modified. Agregue los parametros @ConsiderQuinellas y @FilterDeposit y elimine los parametros @ConsiderRoomsQty,
**								@ConsiderAntesInOut y @ConsiderQuinellasSplit
** [wtorres]		02/Ene/2010 Modified. Agregue los parametros @ConsiderDirects y @ConsiderInOuts
** [wtorres]		11/Mar/2010 Modified. Ahora ya no se restan los In & Outs que no estan en el rango de fechas definido
** [wtorres]		16/Abr/2010 Modified. Agregue los parametros @PRs y @Program
** [wtorres]		18/Nov/2010 Modified. Agregue el parametro @BasedOnArrival y reemplace el parametro @ConsiderDirects por @Direct
** [wtorres]		26/Nov/2010 Modified. Ahora no se cuentan los bookings cancelados
** [wtorres]		26/Nov/2013 Modified. Reemplace el parametro @ConsiderInOuts por @InOut para que el filtro de In & Outs sea mas simple
** [gmaya]			11/Ago/2014 Modified. Agregue el parametro de formas de pago
** [lormartinez]	22/Sep/2014 Modified. Se agrega parametro para filtro basado en fecha de booking
** [wtorres]		15/Jul/2015 Modified. Elimine el parametro @BasedOnBooking porque no es necesario
** [lormartinez]	15/Dic/2015 Modified. Se reimplementa filtro por tipos de pago 
** [erosado]		04/Mar/2016 Modified. Agregue los parametros @SalesRooms, @Countries, @Agencies y @Markets
** [edgrodriguez]	05/May/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/
create function [dbo].[UFN_OR_GetPRBookings](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(max)='ALL',-- Clave de los Lead Sources
	@PRs varchar(8000) = 'ALL',		-- Claves de PRs
	@Program varchar(10) = 'ALL',	-- Clave de programa
	@ConsiderQuinellas bit = 0,		-- Indica si se debe considerar quinielas
	@FilterDeposit tinyint = 0,		-- Filtro de depositos:
									--		0. Sin filtro
									--		1. Con deposito (Deposits)
									--		2. Sin deposito (Flyers)
									--		3. Con deposito y shows sin deposito (Deposits & Flyers Show)
	@Direct int = -1,				-- Filtro de directas:
									--		-1. Sin filtro
									--		 0. No directas
									--		 1. Directas
	@InOut int = -1,				-- Filtro de In & Outs:
									--		-1. Sin filtro
									--		 0. No In & Outs
									--		 1. In & Outs
	@BasedOnArrival bit = 0,		-- Indica si se debe basar en la fecha de llegada,
	@PaymentTypes varchar(MAX) = 'ALL', --Filtro para tipos de pagos
	@SalesRooms varchar(max) = 'ALL',	-- Clave de las salas de ventas
	@Countries varchar(max) = 'ALL',	-- Clave de los paises
	@Agencies varchar(max) = 'ALL',		-- Clave de las agencias
	@Markets varchar(max) = 'ALL'		-- Clave de los mercados
  
)
returns @Table table (
	PR varchar(10),
	Books money
)
as
begin

declare @tmpPaymentTypes table(item varchar(20))

--Si hay PaymentTypes se llena la lista
IF @PaymentTypes <> 'ALL'
BEGIN
 INSERT INTO @tmpPaymentTypes
 SELECT ITEM FROM dbo.Split(@PaymentTypes,',')
END

insert @Table

-- Bookings (PR 1)
-- =============================================
select
	G.guPRInvit1,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) * case when @ConsiderQuinellas = 0 then 1 else G.guRoomsQty end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
	OUTER APPLY (
		SELECT count(bd.bdID) bdCount
		FROM BookingDeposits bd
		WHERE bd.bdgu = g.guid
			and (@PaymentTypes ='ALL' or (@PaymentTypes <> 'ALL' and bd.bdpt IN (select item from @tmpPaymentTypes)) )
	) bk
where
	-- PR 1
	G.guPRInvit1 is not null
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and G.guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and G.guInvit = 1))
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- No Quiniela Split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0)
		or (@FilterDeposit = 3 and (G.guDeposit > 0 or (G.guShow = 1 and G.guDeposit = 0))))
	-- Directas
	and (@Direct = -1 or G.guDirect = @Direct)
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
	-- No bookings cancelados
	and G.guBookCanc = 0
	-- Filtro para tipos de pago
	and (@PaymentTypes ='ALL' or (@PaymentTypes <> 'ALL' and ISNULL(bk.bdCount, 0) > 0 ))
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
	-- Countries
	and (@Countries = 'ALL' or G.guco in (select item from split(@Countries, ',')))
	-- Agencies
	and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
	-- Markets
	and (@Markets = 'ALL' or G.gumk in (select item from split(@Markets, ',')))
group by G.guPRInvit1
union all

-- Bookings (PR 2)
-- =============================================
select
	G.guPRInvit2,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) * case when @ConsiderQuinellas = 0 then 1 else G.guRoomsQty end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
	OUTER APPLY (
		SELECT count(bd.bdID) bdCount
		FROM BookingDeposits bd
		WHERE bd.bdgu = g.guid
			and (@PaymentTypes ='ALL' or (@PaymentTypes <> 'ALL' and bd.bdpt IN (select item from @tmpPaymentTypes)) )
	) bk
where
	-- PR 2
	G.guPRInvit2 is not null
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and G.guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and G.guInvit = 1))
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- No Quiniela Split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0)
		or (@FilterDeposit = 3 and (G.guDeposit > 0 or (G.guShow = 1 and G.guDeposit = 0))))
	-- Directas
	and (@Direct = -1 or G.guDirect = @Direct)
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
	-- No bookings cancelados
	and G.guBookCanc = 0
	-- Filtro para tipos de pago
	and (@PaymentTypes ='ALL' or (@PaymentTypes <> 'ALL' and ISNULL(bk.bdCount, 0) > 0 ))
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
	-- Countries
	and (@Countries = 'ALL' or G.guco in (select item from split(@Countries, ',')))
	-- Agencies
	and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
	-- Markets
	and (@Markets = 'ALL' or G.gumk in (select item from split(@Markets, ',')))
group by G.guPRInvit2
union all

-- Bookings (PR 3)
-- =============================================
select
	G.guPRInvit3,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) * case when @ConsiderQuinellas = 0 then 1 else G.guRoomsQty end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
	OUTER APPLY (
		SELECT count(bd.bdID) bdCount
		FROM BookingDeposits bd
		WHERE bd.bdgu = g.guid
			and (@PaymentTypes ='ALL' or (@PaymentTypes <> 'ALL' and bd.bdpt IN (select item from @tmpPaymentTypes)) )
	) bk
where
	-- PR 3
	G.guPRInvit3 is not null
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and G.guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and G.guInvit = 1))
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- No Quiniela Split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0)
		or (@FilterDeposit = 3 and (G.guDeposit > 0 or (G.guShow = 1 and G.guDeposit = 0))))
	-- Directas
	and (@Direct = -1 or G.guDirect = @Direct)
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
	-- No bookings cancelados
	and G.guBookCanc = 0
	-- Filtro para tipos de pago
	and (@PaymentTypes ='ALL' or (@PaymentTypes <> 'ALL' and ISNULL(bk.bdCount, 0) > 0 ))
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
	-- Countries
	and (@Countries = 'ALL' or G.guco in (select item from split(@Countries, ',')))
	-- Agencies
	and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
	-- Markets
	and (@Markets = 'ALL' or G.gumk in (select item from split(@Markets, ',')))
group by G.guPRInvit3

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRContacts]    Script Date: 07/28/2016 13:16:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de contactos por PR
** 
** [wtorres]		18/Sep/2009 Created
** [wtorres]		23/Sep/2009 Modified. Convertido a funcion. Agregue el parametro @LeadSources
** [wtorres]		12/Jul/2010 Modified. Agregue el parametro @ConsiderFollow
** [wtorres]		18/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [wtorres]		20/Oct/2011 Modified. Ahora no se cuentan los rebooks
** [erosado]		04/Mar/2016 Modified. Agregue los parametros @SalesRooms, @Countries, @Agencies y @Markets
** [edgrodriguez]	05/May/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/
CREATE function [dbo].[UFN_OR_GetPRContacts](

	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(max)='ALL',	-- Clave de los Lead Sources
	@ConsiderFollow bit = 0,			-- Indica si se debe considerar seguimiento
	@BasedOnArrival bit = 0,			-- Indica si se debe basar en la fecha de llegada
	@SalesRooms varchar(max) = 'ALL',	-- Clave de las salas de ventas
	@Countries varchar(max) = 'ALL',	-- Clave de los paises
	@Agencies varchar(max) = 'ALL',		-- Clave de las agencias
	@Markets varchar(max) = 'ALL'		-- Clave de los mercados
)
returns @Table table (
	PR varchar(10),
	Contacts money
)
as
begin

insert @Table

-- Contactos (PR de contacto)
-- =============================================
select
	guPRInfo,
	Count(*)
from Guests
where
	-- PR de contacto
	guPRInfo is not null
	-- Fecha de contacto
	and ((@BasedOnArrival = 0 and guInfoD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Contactado
	and guInfo = 1))
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or gusr in (select item from split(@SalesRooms, ',')))
	-- Countries
	and (@Countries = 'ALL' or guco in (select item from split(@Countries, ',')))
	-- Agencies
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
	-- Markets
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
group by guPRInfo

-- Contactos (PR 1)
-- =============================================
union all
select
	guPRInvit1,
	Sum(dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT))
from Guests
where
	-- PR 1
	guPRInvit1 is not null
	-- PR de contacto diferente de los PR de invitacion
	and guPRInfo not in (IsNull(guPRInvit1, ''), IsNull(guPRInvit2, ''), IsNull(guPRInvit3, ''))
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and guInvit = 1))
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or gusr in (select item from split(@SalesRooms, ',')))
	-- Countries
	and (@Countries = 'ALL' or guco in (select item from split(@Countries, ',')))
	-- Agencies
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
	-- Markets
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
group by guPRInvit1

-- Contactos (PR 2)
-- =============================================
union all
select
	guPRInvit2,
	Sum(dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT))
from Guests
where
	-- PR 2
	guPRInvit2 is not null
	-- PR de contacto diferente de los PR de invitacion
	and guPRInfo not in (IsNull(guPRInvit1, ''), IsNull(guPRInvit2, ''), IsNull(guPRInvit3, ''))
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and guInvit = 1))
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or gusr in (select item from split(@SalesRooms, ',')))
	-- Countries
	and (@Countries = 'ALL' or guco in (select item from split(@Countries, ',')))
	-- Agencies
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
	-- Markets
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
group by guPRInvit2
union all

-- Contactos (PR 3)
-- =============================================
select
	guPRInvit3,
	Sum(dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT))
from Guests
where
	-- PR 3
	guPRInvit3 is not null
	-- PR de contacto diferente de los PR de invitacion
	and guPRInfo not in (IsNull(guPRInvit1, ''), IsNull(guPRInvit2, ''), IsNull(guPRInvit3, ''))
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and guInvit = 1))
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or gusr in (select item from split(@SalesRooms, ',')))
	-- Countries
	and (@Countries = 'ALL' or guco in (select item from split(@Countries, ',')))
	-- Agencies
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
	-- Markets
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
group by guPRInvit3

-- Contactos (PR de seguimiento)
-- =============================================
if @ConsiderFollow = 1
	insert @Table
	select
		guPRFollow,
		Count(*)
	from Guests
	where
		-- PR de seguimiento
		guPRFollow is not null
		-- PR de contacto diferente del PR de seguimiento
		and guPRInfo <> guPRFollow
		-- PR de seguimiento diferente de los PR de invitacion
		and guPRFollow not in (IsNull(guPRInvit1, ''), IsNull(guPRInvit2, ''), IsNull(guPRInvit3, ''))
		-- Fecha de seguimiento
		and ((@BasedOnArrival = 0 and guFollowD between @DateFrom and @DateTo)
		-- Fecha de llegada
		or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
		-- Con seguimiento
		and guFollow = 1))
		-- No Rebook
		and guRef is null
		-- No Antes In & Out
		and guAntesIO = 0
		-- Lead Source
		and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
		-- Sales Rooms
		and (@SalesRooms = 'ALL' or gusr in (select item from split(@SalesRooms, ',')))
		-- Countries
		and (@Countries = 'ALL' or guco in (select item from split(@Countries, ',')))
		-- Agencies
		and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
		-- Markets
		and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
	group by guPRFollow

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRDeposits]    Script Date: 07/28/2016 13:16:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de depositos por PR
** 
** [wtorres]		18/Sep/2009 Created
** [wtorres]		23/Sep/2009 Modified. Convertido a funcion. Agregue el parametro @LeadSources
** [wtorres]		18/Nov/2010 Modified. Agregue el parametro @BasedOnArrival y reemplace el parametro @ConsiderDirects por @Direct
** [edgrodriguez]	05/May/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/
CREATE function [dbo].[UFN_OR_GetPRDeposits](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Clave de los Lead Sources
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	PR varchar(10),
	Deposits money
)
as
begin

insert @Table

-- Depositos (PR 1)
-- =============================================
select
	guPRInvit1,
	Sum(guRoomsQty * [dbo].UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT))
from Guests
where
	-- PR 1
	guPRInvit1 is not null
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and guInvit = 1))
	-- Con Deposito
	and (guDeposit > 0 or guDepositTwisted > 0)
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
group by guPRInvit1

-- Depositos (PR 2)
-- =============================================
union all
select
	guPRInvit2,
	Sum(guRoomsQty * [dbo].UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT))
from Guests
where
	-- PR 2
	guPRInvit2 is not null
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and guInvit = 1))
	-- Con Deposito
	and (guDeposit > 0 or guDepositTwisted > 0)
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
group by guPRInvit2

-- Depositos (PR 3)
-- =============================================
union all
select
	guPRInvit3,
	Sum(guRoomsQty * [dbo].UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT))
from Guests
where
	-- PR 3
	guPRInvit3 is not null
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and guInvit = 1))
	-- Con Deposito
	and (guDeposit > 0 or guDepositTwisted > 0)
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
group by guPRInvit3

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRGroupAvailables]    Script Date: 07/28/2016 13:16:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de disponibles por PR y grupo
** 
** [wtorres]		11/Ago/2010 Created
** [wtorres]		18/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [edgrodriguez]	05/may/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/
CREATE function [dbo].[UFN_OR_GetPRGroupAvailables](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0,	-- Indica si se debe considerar quinielas
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada	
)
returns @Table table (
	PR varchar(10),
	[Group] int,
	Availables money
)
as
begin

insert @Table

-- Disponibles (PR Info)
-- =============================================
select
	G.guPRInfo,
	I.gjgx,
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then 1 else guShowsQty end)
from Guests G
	left join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- PR Info
	G.guPRInfo is not null
	-- Fecha de contacto
	and ((@BasedOnArrival = 0 and G.guInfoD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Contactado
	and G.guInfo = 1))
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Disponible
	and G.guAvail = 1
	-- Lead Source
	and G.guls in (select item from split(@LeadSources, ','))
group by G.guPRInfo, I.gjgx

-- Disponibles (PR 1)
-- =============================================
union all
select
	G.guPRInvit1,
	I.gjgx,
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT)
		else guShowsQty * dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT) end)
from Guests G
	left join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- PR 1
	G.guPRInvit1 is not null
	-- PR de contacto diferente de los PR de invitacion
	and G.guPRInfo not in (IsNull(G.guPRInvit1, ''), IsNull(G.guPRInvit2, ''), IsNull(G.guPRInvit3, ''))
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and G.guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and G.guInvit = 1))
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Disponible
	and G.guAvail = 1
	-- Lead Source
	and G.guls in (select item from split(@LeadSources, ','))
group by G.guPRInvit1, I.gjgx

-- Disponibles (PR 2)
-- =============================================
union all
select
	G.guPRInvit2,
	I.gjgx,
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT)
		else guShowsQty * dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT) end)
from Guests G
	left join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- PR 2
	G.guPRInvit2 is not null
	-- PR de contacto diferente de los PR de invitacion
	and G.guPRInfo not in (IsNull(G.guPRInvit1, ''), IsNull(G.guPRInvit2, ''), IsNull(G.guPRInvit3, ''))
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and G.guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and G.guInvit = 1))
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Disponible
	and G.guAvail = 1
	-- Lead Source
	and G.guls in (select item from split(@LeadSources, ','))
group by G.guPRInvit2, I.gjgx

-- Disponibles (PR 3)
-- =============================================
union all
select
	G.guPRInvit3,
	I.gjgx,
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT)
		else guShowsQty * dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT) end)
from Guests G
	left join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- PR 3
	G.guPRInvit3 is not null
	-- PR de contacto diferente de los PR de invitacion
	and G.guPRInfo not in (IsNull(G.guPRInvit1, ''), IsNull(G.guPRInvit2, ''), IsNull(G.guPRInvit3, ''))
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and G.guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and G.guInvit = 1))
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Disponible
	and G.guAvail = 1
	-- Lead Source
	and G.guls in (select item from split(@LeadSources, ','))
group by G.guPRInvit3, I.gjgx

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRGroupBookings]    Script Date: 07/28/2016 13:16:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de bookings por PR y grupo
** 
** [wtorres]		11/Ago/2010 Created
** [wtorres]		18/Nov/2010 Modified. Agregue el parametro @BasedOnArrival y reemplace el parametro @ConsiderDirects por @Direct
** [wtorres]		26/Nov/2010 Modified. Ahora no se cuentan los bookings cancelados
** [edgrodriguez]	05/may/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/
CREATE function [dbo].[UFN_OR_GetPRGroupBookings](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0,	-- Indica si se debe considerar quinielas
	@Direct int = -1,			-- Filtro de directas:
								--		-1. Sin filtro
								--		 0. No directas
								--		 1. Directas
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	PR varchar(10),
	[Group] int,
	Books money
)
as
begin

insert @Table

-- Bookings (PR 1)
-- =============================================
select
	G.guPRInvit1,
	I.gjgx,
	Sum(case when @ConsiderQuinellas = 0 then dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT)
		else G.guRoomsQty * dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) end)
from Guests G
	left join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- PR 1
	G.guPRInvit1 is not null
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and G.guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and G.guInvit = 1))
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Lead Source
	and G.guls in (select item from split(@LeadSources, ','))
	-- No quinielas split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- Directas
	and (@Direct = -1 or G.guDirect = @Direct)
	-- No bookings cancelados
	and G.guBookCanc = 0
group by G.guPRInvit1, I.gjgx
union all

-- Bookings (PR 2)
-- =============================================
select
	G.guPRInvit2,
	I.gjgx,
	Sum(case when @ConsiderQuinellas = 0 then dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT)
		else G.guRoomsQty * dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) end)
from Guests G
	left join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- PR 2
	G.guPRInvit2 is not null
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and G.guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and G.guInvit = 1))
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Lead Source
	and G.guls in (select item from split(@LeadSources, ','))
	-- No quinielas split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- Directas
	and (@Direct = -1 or G.guDirect = @Direct)
	-- No bookings cancelados
	and G.guBookCanc = 0
group by G.guPRInvit2, I.gjgx
union all

-- Bookings (PR 3)
-- =============================================
select
	G.guPRInvit3,
	I.gjgx,
	Sum(case when @ConsiderQuinellas = 0 then dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT)
		else G.guRoomsQty * dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) end)
from Guests G
	left join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- PR 3
	G.guPRInvit3 is not null
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and G.guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and G.guInvit = 1))
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Lead Source
	and G.guls in (select item from split(@LeadSources, ','))
	-- No quinielas split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- Directas
	and (@Direct = -1 or G.guDirect = @Direct)
	-- No bookings cancelados
	and G.guBookCanc = 0
group by G.guPRInvit3, I.gjgx

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRGroupContacts]    Script Date: 07/28/2016 13:16:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de contactos por PR y grupo
** 
** [wtorres]		11/Ago/2010 Created
** [wtorres]		18/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [wtorres]		20/Oct/2011 Modified. Ahora no se cuentan los rebooks
** [edgrodriguez]	05/may/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/
CREATE function [dbo].[UFN_OR_GetPRGroupContacts](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	PR varchar(10),
	[Group] int,
	Contacts money
)
as
begin

insert @Table

-- Contactos (PR Info)
-- =============================================
select
	G.guPRInfo,
	I.gjgx,
	Count(*)
from Guests G
	left join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- PR de contacto
	G.guPRInfo is not null
	-- Fecha de contacto
	and ((@BasedOnArrival = 0 and G.guInfoD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Contactado
	and G.guInfo = 1))
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Lead Source
	and G.guls in (select item from split(@LeadSources, ','))
group by G.guPRInfo, I.gjgx
union all

-- Contactos (PR 1)
-- =============================================
select
	G.guPRInvit1,
	I.gjgx,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT))
from Guests G
	left join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- PR 1
	G.guPRInvit1 is not null
	-- PR de contacto diferente de los PR de invitacion
	and G.guPRInfo not in (IsNull(G.guPRInvit1, ''), IsNull(G.guPRInvit2, ''), IsNull(G.guPRInvit3, ''))
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and G.guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and G.guInvit = 1))
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Lead Source
	and G.guls in (select item from split(@LeadSources, ','))
group by G.guPRInvit1, I.gjgx
union all

-- Contactos (PR 2)
-- =============================================
select
	G.guPRInvit2,
	I.gjgx,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT))
from Guests G
	left join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- PR 2
	G.guPRInvit2 is not null
	-- PR de contacto diferente de los PR de invitacion
	and G.guPRInfo not in (IsNull(G.guPRInvit1, ''), IsNull(G.guPRInvit2, ''), IsNull(G.guPRInvit3, ''))
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and G.guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and G.guInvit = 1))
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Lead Source
	and G.guls in (select item from split(@LeadSources, ','))
group by G.guPRInvit2, I.gjgx
union all

-- Contactos (PR 3)
-- =============================================
select
	G.guPRInvit3,
	I.gjgx,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT))
from Guests G
	left join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- PR 3
	G.guPRInvit3 is not null
	-- PR de contacto diferente de los PR de invitacion
	and G.guPRInfo not in (IsNull(G.guPRInvit1, ''), IsNull(G.guPRInvit2, ''), IsNull(G.guPRInvit3, ''))
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and G.guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and G.guInvit = 1))
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Lead Source
	and G.guls in (select item from split(@LeadSources, ','))
group by G.guPRInvit3, I.gjgx

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRGroupSales]    Script Date: 07/28/2016 13:17:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas por PR y grupo
** 
** [wtorres]		11/Ago/2010 Created
** [wtorres]		18/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [wtorres]		19/Nov/2013 Modified. Ahora los downgrades se cuentan en base a la categoria de tipo de venta
** [edgrodriguez]	05/may/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/
CREATE function [dbo].[UFN_OR_GetPRGroupSales](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@ConsiderSelfGen bit = 0,			-- Indica si se debe considerar Self Gen
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	PR varchar(10),
	[Group] int,
	Sales money
)
as
begin

insert @Table

-- Numero de ventas (PR 1)
-- =============================================
select
	S.saPR1,
	I.gjgx,
	Sum(dbo.UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
	left join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- PR 1
	S.saPR1 is not null
	-- Fecha de procesable
	and (((@BasedOnArrival = 0 or (@BasedOnArrival = 1 and S.sagu is null)) and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1	and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo
	-- Procesable
	and S.saProc = 1))
	-- No downgrades
	and ST.ststc <> 'DG'
	-- Fecha de cancelacion
	and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ','))) 
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
group by S.saPR1, I.gjgx

-- Numero de ventas (PR 2)
-- =============================================
union all
select
	S.saPR2,
	I.gjgx,
	Sum(dbo.UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
	left join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- PR 2
	S.saPR2 is not null
	-- Fecha de procesable
	and (((@BasedOnArrival = 0 or (@BasedOnArrival = 1 and S.sagu is null)) and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1	and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo
	-- Procesable
	and S.saProc = 1))
	-- No downgrades
	and ST.ststc <> 'DG'
	-- Fecha de cancelacion
	and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ','))) 
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
group by S.saPR2, I.gjgx

-- Numero de ventas (PR 3)
-- =============================================
union all
select
	S.saPR3,
	I.gjgx,
	Sum(dbo.UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
	left join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- PR 3
	S.saPR3 is not null
	-- Fecha de procesable
	and (((@BasedOnArrival = 0 or (@BasedOnArrival = 1 and S.sagu is null)) and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1	and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo
	-- Procesable
	and S.saProc = 1))
	-- No downgrades
	and ST.ststc <> 'DG'
	-- Fecha de cancelacion
	and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ','))) 
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
group by S.saPR3, I.gjgx

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRGroupSalesAmount]    Script Date: 07/28/2016 13:17:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de ventas por PR y grupo
** 
** [wtorres]		11/Ago/2010 Created
** [wtorres]		18/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [wtorres]		19/Nov/2013 Modified. Ahora los downgrades se cuentan en base a la categoria de tipo de venta
** [edgrodriguez]	05/may/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/
CREATE function [dbo].[UFN_OR_GetPRGroupSalesAmount](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@ConsiderSelfGen bit = 0,			-- Indica si se debe considerar Self Gen
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	PR varchar(10),
	[Group] int,
	SalesAmount money
)
as
begin

insert @Table

-- Monto de ventas (PR 1)
-- =============================================
select
	S.saPR1,
	I.gjgx,
	Sum(S.saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
	left join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- PR 1
	S.saPR1 is not null
	-- Fecha de procesable
	and ((((@BasedOnArrival = 0 or (@BasedOnArrival = 1 and S.sagu is null)) and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo
	-- Procesable
	and S.saProc = 1))
	-- No downgrades
	and (ST.ststc <> 'DG'
	-- Downgrades cuya membresia de referencia esta dentro del periodo
	or (ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)))
	-- Fecha de cancelacion
	and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ','))) 
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
group by S.saPR1, I.gjgx

-- Monto de ventas (PR 2)
-- =============================================
union all
select
	S.saPR2,
	I.gjgx,
	Sum(S.saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
	left join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- PR 2
	S.saPR2 is not null
	-- Fecha de procesable
	and ((((@BasedOnArrival = 0 or (@BasedOnArrival = 1 and S.sagu is null)) and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo
	-- Procesable
	and S.saProc = 1))
	-- No downgrades
	and (ST.ststc <> 'DG'
	-- Downgrades cuya membresia de referencia esta dentro del periodo
	or (ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)))
	-- Fecha de cancelacion
	and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ','))) 
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
group by S.saPR2, I.gjgx

-- Monto de ventas (PR 3)
-- =============================================
union all
select
	S.saPR3,
	I.gjgx,
	Sum(S.saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
	left join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- PR 3
	S.saPR3 is not null
	-- Fecha de procesable
	and ((((@BasedOnArrival = 0 or (@BasedOnArrival = 1 and S.sagu is null)) and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo
	-- Procesable
	and S.saProc = 1))
	-- No downgrades
	and (ST.ststc <> 'DG'
	-- Downgrades cuya membresia de referencia esta dentro del periodo
	or (ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)))
	-- Fecha de cancelacion
	and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ','))) 
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
group by S.saPR3, I.gjgx

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRGroupShows]    Script Date: 07/28/2016 13:17:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por PR y grupo
** 
** [wtorres]		11/Ago/2010 Created
** [wtorres]		18/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [edgrodriguez]	05/May/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/
CREATE function [dbo].[UFN_OR_GetPRGroupShows](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000),			-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@ConsiderDirectsAntesInOut bit = 0,	-- Indica si se debe considerar directas Antes In & Out
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	PR varchar(10),
	[Group] int,
	Shows money
)
as
begin

insert @Table

-- Shows (PR 1)
-- =============================================
select
	G.guPRInvit1,
	I.gjgx,
	Sum(case when @ConsiderQuinellas = 0 then dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT)
		else G.guShowsQty * dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) end)
from Guests G
	left join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- PR 1
	G.guPRInvit1 is not null
	-- Fecha de show
	and ((@BasedOnArrival = 0 and G.guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Con show
	and G.guShow = 1))
	-- Lead Source
	and G.guls in (select item from split(@LeadSources, ','))
	-- No quinielas split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- No Directas no Antes In & Out
	and (@ConsiderDirectsAntesInOut = 0 or not (G.guDirect = 1 and G.guAntesIO = 0))
group by G.guPRInvit1, I.gjgx

-- Shows (PR 2)
-- =============================================
union all
select
	G.guPRInvit2,
	I.gjgx,
	Sum(case when @ConsiderQuinellas = 0 then dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT)
		else G.guShowsQty * dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) end)
from Guests G
	left join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- PR 2
	G.guPRInvit2 is not null
	-- Fecha de show
	and ((@BasedOnArrival = 0 and G.guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Con show
	and G.guShow = 1))
	-- Lead Source
	and G.guls in (select item from split(@LeadSources, ','))
	-- No quinielas split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- No Directas no Antes In & Out
	and (@ConsiderDirectsAntesInOut = 0 or not (G.guDirect = 1 and G.guAntesIO = 0))
group by G.guPRInvit2, I.gjgx

-- Shows (PR 3)
-- =============================================
union all
select
	G.guPRInvit3,
	I.gjgx,
	Sum(case when @ConsiderQuinellas = 0 then dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT)
		else G.guShowsQty * dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) end)
from Guests G
	left join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- PR 3
	G.guPRInvit3 is not null
	-- Fecha de show
	and ((@BasedOnArrival = 0 and G.guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Con show
	and G.guShow = 1))
	-- Lead Source
	and G.guls in (select item from split(@LeadSources, ','))
	-- No quinielas split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- No Directas no Antes In & Out
	and (@ConsiderDirectsAntesInOut = 0 or not (G.guDirect = 1 and G.guAntesIO = 0))
group by G.guPRInvit3, I.gjgx

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRNotQualifieds]    Script Date: 07/28/2016 13:17:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de no calificados por PR
** 
** [wtorres]		21/May/2011 Created
** [edgrodriguez]	05/May/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/
CREATE function [dbo].[UFN_OR_GetPRNotQualifieds](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Clave de los Lead Sources
	@ConsiderQuinellas bit = 0,	-- Indica si se debe considerar quinielas
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	PR varchar(10),
	NotQualifieds money
)
as
begin

insert @Table

-- No calificados (PR 1)
-- =============================================
select
	G.guPRInvit1,
	Sum(case when @ConsiderQuinellas = 0 then [dbo].UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT)
		else G.guShowsQty * [dbo].UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- PR 1
	G.guPRInvit1 is not null
	-- Fecha de show
	and ((@BasedOnArrival = 0 and G.guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Con show
	and G.guShow = 1))
	-- No Quiniela Split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- No calificado
	and G.guNQ = 1
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ',')))
group by G.guPRInvit1

-- No calificados (PR 2)
-- =============================================
union all
select
	G.guPRInvit2,
	Sum(case when @ConsiderQuinellas = 0 then [dbo].UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT)
		else G.guShowsQty * [dbo].UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- PR 2
	G.guPRInvit2 is not null
	-- Fecha de show
	and ((@BasedOnArrival = 0 and G.guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Con show
	and G.guShow = 1))
	-- No Quiniela Split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- No calificado
	and G.guNQ = 1
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ',')))
group by G.guPRInvit2

-- No calificados (PR 3)
-- =============================================
union all
select
	G.guPRInvit3,
	Sum(case when @ConsiderQuinellas = 0 then [dbo].UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT)
		else G.guShowsQty * [dbo].UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- PR 3
	G.guPRInvit3 is not null
	-- Fecha de show
	and ((@BasedOnArrival = 0 and G.guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Con show
	and G.guShow = 1))
	-- No Quiniela Split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- No calificado
	and G.guNQ = 1
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ',')))
group by G.guPRInvit3

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRQualifieds]    Script Date: 07/28/2016 13:17:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de calificados por PR
** 
** [wtorres]		30/May/2011 Created
** [edgrodriguez]	05/May/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/
CREATE function [dbo].[UFN_OR_GetPRQualifieds](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Clave de los Lead Sources
	@ConsiderQuinellas bit = 0,	-- Indica si se debe considerar quinielas
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	PR varchar(10),
	Qualifieds money
)
as
begin

insert @Table

-- Calificados (PR 1)
-- =============================================
select
	G.guPRInvit1,
	Sum(case when @ConsiderQuinellas = 0 then [dbo].UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT)
		else G.guShowsQty * [dbo].UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- PR 1
	G.guPRInvit1 is not null
	-- Fecha de show
	and ((@BasedOnArrival = 0 and G.guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Con show
	and G.guShow = 1))
	-- No Quiniela Split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- Calificado
	and G.guQ = 1
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ',')))
group by G.guPRInvit1

-- Calificados (PR 2)
-- =============================================
union all
select
	G.guPRInvit2,
	Sum(case when @ConsiderQuinellas = 0 then [dbo].UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT)
		else G.guShowsQty * [dbo].UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- PR 2
	G.guPRInvit2 is not null
	-- Fecha de show
	and ((@BasedOnArrival = 0 and G.guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Con show
	and G.guShow = 1))
	-- No Quiniela Split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- Calificado
	and G.guQ = 1
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ',')))
group by G.guPRInvit2

-- Calificados (PR 3)
-- =============================================
union all
select
	G.guPRInvit3,
	Sum(case when @ConsiderQuinellas = 0 then [dbo].UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT)
		else G.guShowsQty * [dbo].UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- PR 3
	G.guPRInvit3 is not null
	-- Fecha de show
	and ((@BasedOnArrival = 0 and G.guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Con show
	and G.guShow = 1))
	-- No Quiniela Split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- Calificado
	and G.guQ = 1
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ',')))
group by G.guPRInvit3

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRSales]    Script Date: 07/28/2016 13:17:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas por PR
** 
** [wtorres]		18/Sep/2009 Created
** [wtorres]		23/Sep/2009 Modified. Convertido a funcion. Agregue el parametro @LeadSources
** [wtorres]		28/Oct/2009 Modified. Agregue los parametros @ConsiderOutOfPending y @FilterDeposit
** [wtorres]		24/Nov/2009 Modified. Agregue el parametro @ConsiderCancel
** [wtorres]		16/Abr/2010 Modified. Agregue los parametros @PRs y @Program
** [wtorres]		18/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [wtorres]		21/Sep/2011 Modified. Agregue el parametro @ConsiderPending
** [wtorres]		19/Nov/2013 Modified. Ahora los downgrades se cuentan en base a la categoria de tipo de venta
** [lormartinez]	22/Sep/2014 Modified. Se agrega parametro para filtro basado en fecha de booking
** [wtorres]		15/Jul/2015 Modified. Correccion de error. No estaba contando las ventas que no tuvieran Guest ID
** [wtorres]		09/Ene/2016 Modified. Correccion de error. No estaba contando las ventas pendientes (basado en la fecha de venta)
**								cuya fecha de procesable no estuviera en el rango del reporte
** [erosado]		05/Mar/2016 Modified. Agregue los parametros @SalesRooms, @Countries, @Agencies y @Markets
** [lchairez]		15/Abr/2016 Modified. Agregue el parametro @BasedOnPRLocation
** [edgrodriguez]	05/May/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/
CREATE function [dbo].[UFN_OR_GetPRSales](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(max)='ALL',-- Clave de los Lead Sources
	@PRs varchar(8000) = 'ALL',		-- Claves de PRs
	@Program varchar(10) = 'ALL',	-- Clave de programa
	@ConsiderSelfGen bit = 0,		-- Indica si se debe considerar Self Gen
	@ConsiderOutOfPending bit = 0,	-- Indica si se debe considerar Out Of Pending
	@ConsiderCancel bit = 0,		-- Indica si se debe considerar canceladas
	@FilterDeposit tinyint = 0,		-- Filtro de depositos:
									--		0. Sin filtro
									--		1. Con deposito (Deposits)
									--		2. Sin deposito (Flyers)
	@ConsiderPending bit = 0,		-- Indica si se debe considerar pendientes
	@BasedOnArrival bit = 0,		-- Indica si se debe basar en la fecha de llegada
	@BasedOnBooking bit = 0,		-- Indica si se debe basar en la fecha de booking
	@BasedOnPRLocation bit = 0,		-- Indica si se debe basar en la locacion por default del PR
	
	@SalesRooms varchar(max) = 'ALL',	-- Clave de las salas de ventas
	@Countries varchar(max) = 'ALL',	-- Clave de los paises
	@Agencies varchar(max) = 'ALL',		-- Clave de las agencias
	@Markets varchar(max) = 'ALL'		-- Clave de los mercados
)
returns @Table table (
	PR varchar(10),
	Sales money	
)
as
begin

insert @Table

-- Numero de ventas (PR 1)
-- =============================================
select
	S.saPR1,
	Sum([dbo].UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	inner join LeadSources L on L.lsID = S.sals
	left join Guests G on S.sagu = G.guID
where
	-- PR 1
	S.saPR1 is not null
	-- Fecha de procesable
	and ((((@BasedOnArrival = 0 and @BasedOnBooking = 0) or ((@BasedOnArrival = 1 or @BasedOnBooking = 1) and S.sagu is null))
	and ((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo))))
	-- Fecha de llegada
	or (((@BasedOnArrival = 1 and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo)
	-- Fecha de booking
	or (@BasedOnBooking = 1 and S.sagu is not null and G.guBookD between @DateFrom and @DateTo))
	-- Procesable
	and ((@ConsiderPending = 0 and S.saProc = 1) or (@ConsiderPending = 1 and S.saProc = 0))))
	-- No downgrades
	and ST.ststc <> 'DG'
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ',')))
	-- PRs
	and (@PRs = 'ALL' or (S.saPR1 in (select item from split(@PRs, ','))
		or S.saPR2 in (select item from split(@PRs, ','))
		or S.saPR3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
	-- Countries
	and (@Countries = 'ALL' or G.guco in (select item from split(@Countries, ',')))
	-- Agencies
	and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
	-- Markets
	and (@Markets = 'ALL' or G.gumk in (select item from split(@Markets, ',')))
group by S.saPR1

-- SI REQUIERE LAS VENTAS DEL PR DE SU LS POR DEFAULT
UNION ALL
select
	S.saPR1,
	Sum([dbo].UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT))
from Sales S
join Personnel p on s.saPR1 = p.peID AND p.peTeamType = 'GS' AND (@LeadSources = 'ALL' OR p.pePlaceID in (select item from split(@LeadSources, ',')))
where 
	@BasedOnPRLocation = 1 
	AND saProcD BETWEEN @DateFrom and @DateTo
	AND (saPR1 is not null)
	AND (s.saCancelD NOT BETWEEN  @DateFrom and @DateTo OR s.saCancel = 0)
	AND s.sals <> p.pePlaceID
group by S.saPR1

-- Numero de ventas (PR 2)
-- =============================================
union all
select
	S.saPR2,
	Sum([dbo].UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	inner join LeadSources L on L.lsID = S.sals
	left join Guests G on S.sagu = G.guID
where
	-- PR 2
	S.saPR2 is not null
	-- Fecha de procesable
	and ((((@BasedOnArrival = 0 and @BasedOnBooking = 0) or ((@BasedOnArrival = 1 or @BasedOnBooking = 1) and S.sagu is null))
	and ((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo))))
	-- Fecha de llegada
	or (((@BasedOnArrival = 1 and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo)
	-- Fecha de booking
	or (@BasedOnBooking = 1 and S.sagu is not null and G.guBookD between @DateFrom and @DateTo))
	-- Procesable
	and ((@ConsiderPending = 0 and S.saProc = 1) or (@ConsiderPending = 1 and S.saProc = 0))))
	-- No downgrades
	and ST.ststc <> 'DG'
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ',')))
	-- PRs
	and (@PRs = 'ALL' or (S.saPR1 in (select item from split(@PRs, ','))
		or S.saPR2 in (select item from split(@PRs, ','))
		or S.saPR3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
	-- Countries
	and (@Countries = 'ALL' or G.guco in (select item from split(@Countries, ',')))
	-- Agencies
	and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
	-- Markets
	and (@Markets = 'ALL' or G.gumk in (select item from split(@Markets, ',')))
group by S.saPR2

-- SI REQUIERE LAS VENTAS DEL PR DE SU LS POR DEFAULT
UNION ALL
select
	S.saPR2,
	Sum([dbo].UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT))
from Sales S
join Personnel p on s.saPR2 = p.peID AND p.peTeamType = 'GS' AND (@LeadSources = 'ALL' OR p.pePlaceID in (select item from split(@LeadSources, ',')))
where 
	@BasedOnPRLocation = 1 
	AND saProcD BETWEEN @DateFrom and @DateTo
	AND (saPR2 is not null)
	AND (s.saCancelD NOT BETWEEN  @DateFrom and @DateTo OR s.saCancel = 0)
	AND s.sals <> p.pePlaceID
group by S.saPR2

-- Numero de ventas (PR 3)
-- =============================================
union all
select
	S.saPR3,
	Sum([dbo].UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	inner join LeadSources L on L.lsID = S.sals
	left join Guests G on S.sagu = G.guID
where
	-- PR 3
	S.saPR3 is not null
	-- Fecha de procesable
	and ((((@BasedOnArrival = 0 and @BasedOnBooking = 0) or ((@BasedOnArrival = 1 or @BasedOnBooking = 1) and S.sagu is null))
	and ((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo))))
	-- Fecha de llegada
	or (((@BasedOnArrival = 1 and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo)
	-- Fecha de booking
	or (@BasedOnBooking = 1 and S.sagu is not null and G.guBookD between @DateFrom and @DateTo))
	-- Procesable
	and ((@ConsiderPending = 0 and S.saProc = 1) or (@ConsiderPending = 1 and S.saProc = 0))))
	-- No downgrades
	and ST.ststc <> 'DG'
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ',')))
	-- PRs
	and (@PRs = 'ALL' or (S.saPR1 in (select item from split(@PRs, ','))
		or S.saPR2 in (select item from split(@PRs, ','))
		or S.saPR3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
	-- Countries
	and (@Countries = 'ALL' or G.guco in (select item from split(@Countries, ',')))
	-- Agencies
	and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
	-- Markets
	and (@Markets = 'ALL' or G.gumk in (select item from split(@Markets, ',')))
group by S.saPR3

-- SI REQUIERE LAS VENTAS DEL PR DE SU LS POR DEFAULT
UNION ALL
select
	S.saPR3,
	Sum([dbo].UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT))
from Sales S
join Personnel p on s.saPR3 = p.peID AND p.peTeamType = 'GS' AND (@LeadSources = 'ALL' OR p.pePlaceID in (select item from split(@LeadSources, ',')))
where 
	@BasedOnPRLocation = 1 
	AND saProcD BETWEEN @DateFrom and @DateTo
	AND (saPR3 is not null)
	AND (s.saCancelD NOT BETWEEN  @DateFrom and @DateTo OR s.saCancel = 0)
	AND s.sals <> p.pePlaceID
group by S.saPR3

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRSalesAmount]    Script Date: 07/28/2016 13:17:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de ventas por PR
** 
** [wtorres]		18/Sep/2009 Created
** [wtorres]		23/Sep/2009 Modified. Convertido a funcion. Agregue el parametro @LeadSources
** [wtorres]		28/Oct/2009 Modified. Agregue los parametros @ConsiderOutOfPending y @FilterDeposit
** [wtorres]		24/Nov/2009 Modified. Agregue el parametro @ConsiderCancel
** [wtorres]		16/Abr/2010 Modified. Agregue los parametros @PRs y @Program
** [wtorres]		18/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [wtorres]		21/Sep/2011 Modified. Agregue el parametro @ConsiderPending
** [wtorres]		19/Nov/2013 Modified. Ahora los downgrades se cuentan en base a la categoria de tipo de venta
** [lormartinez]	22/Sep/2014 Modified. Se agrega parametro para filtro basado en fecha de booking
** [wtorres]		15/Jul/2015 Modified. Correccion de error. No estaba contando las ventas que no tuvieran Guest ID
** [wtorres]		09/Ene/2016 Modified. Correccion de error. No estaba contando las ventas pendientes (basado en la fecha de venta)
**								cuya fecha de procesable no estuviera en el rango del reporte
** [erosado]		05/Mar/2016 Modified. Agregue los parametros @SalesRooms, @Countries, @Agencies y @Markets
** [lchairez]		15/Abr/2016 Modified. Agregue el parametro @BasedOnPRLocation
** [edgrodriguez]	05/May/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/
CREATE function [dbo].[UFN_OR_GetPRSalesAmount](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(max)='ALL',-- Clave de los Lead Sources
	@PRs varchar(8000) = 'ALL',		-- Claves de PRs
	@Program varchar(10) = 'ALL',	-- Clave de programa
	@ConsiderSelfGen bit = 0,		-- Indica si se debe considerar Self Gen
	@ConsiderOutOfPending bit = 0,	-- Indica si se debe considerar Out Of Pending
	@ConsiderCancel bit = 0,		-- Indica si se debe considerar canceladas
	@FilterDeposit tinyint = 0,		-- Filtro de depositos:
									--		0. Sin filtro
									--		1. Con deposito (Deposits)
									--		2. Sin deposito (Flyers)
	@ConsiderPending bit = 0,		-- Indica si se debe considerar pendientes
	@BasedOnArrival bit = 0,		-- Indica si se debe basar en la fecha de llegada
	@BasedOnBooking bit = 0,		-- Indica si se debe basar en la fecha de booking
	@BasedOnPRLocation bit = 0,		-- Indica se se debe basar en la locación por default del PR
	
	@SalesRooms varchar(max) = 'ALL',	-- Clave de las salas de ventas
	@Countries varchar(max) = 'ALL',	-- Clave de los paises
	@Agencies varchar(max) = 'ALL',		-- Clave de las agencias
	@Markets varchar(max) = 'ALL'		-- Clave de los mercados
)
returns @Table table (
	PR varchar(10),
	SalesAmount money
)
as
begin

insert @Table

-- Monto de ventas (PR 1)
-- =============================================
select
	S.saPR1,
	Sum(S.saGrossAmount * [dbo].UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	inner join LeadSources L on L.lsID = S.sals
	left join Guests G on S.sagu = G.guID
where
	-- PR 1
	S.saPR1 is not null
	-- Fecha de procesable
	and (((((@BasedOnArrival = 0 and @BasedOnBooking = 0) or ((@BasedOnArrival = 1 or @BasedOnBooking = 1) and S.sagu is null))
	and ((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo))))
	-- Fecha de llegada
	or (((@BasedOnArrival = 1 and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo)
	-- Fecha de booking
	or (@BasedOnBooking = 1 and S.sagu is not null and G.guBookD between @DateFrom and @DateTo))
	-- Procesable
	and ((@ConsiderPending = 0 and S.saProc = 1) or (@ConsiderPending = 1 and S.saProc = 0))))
	-- No downgrades
	and (ST.ststc <> 'DG'
	-- Downgrades cuya membresia de referencia esta dentro del periodo
	or (@ConsiderCancel = 0 and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)))
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ',')))
	-- PRs
	and (@PRs = 'ALL' or (S.saPR1 in (select item from split(@PRs, ','))
		or S.saPR2 in (select item from split(@PRs, ','))
		or S.saPR3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
	-- Countries
	and (@Countries = 'ALL' or G.guco in (select item from split(@Countries, ',')))
	-- Agencies
	and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
	-- Markets
	and (@Markets = 'ALL' or G.gumk in (select item from split(@Markets, ',')))
group by S.saPR1

-- SI REQUIERE LAS VENTAS DEL PR DE SU LS POR DEFAULT
UNION ALL
select
	S.saPR1,
	Sum(S.saGrossAmount * [dbo].UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT))
from Sales S
join Personnel p on s.saPR1 = p.peID AND p.peTeamType = 'GS' AND (@LeadSources = 'ALL' OR p.pePlaceID in (select item from split(@LeadSources, ',')))
where 
	@BasedOnPRLocation = 1 
	AND saProcD BETWEEN @DateFrom and @DateTo
	AND (saPR1 is not null)
	AND (s.saCancelD NOT BETWEEN  @DateFrom and @DateTo OR s.saCancel = 0)
	AND s.sals <> p.pePlaceID
group by S.saPR1

-- Monto de ventas (PR 2)
-- =============================================
union all
select
	S.saPR2,
	Sum(S.saGrossAmount * [dbo].UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	inner join LeadSources L on L.lsID = S.sals
	left join Guests G on S.sagu = G.guID
where
	-- PR 2
	S.saPR2 is not null
	-- Fecha de procesable
	and (((((@BasedOnArrival = 0 and @BasedOnBooking = 0) or ((@BasedOnArrival = 1 or @BasedOnBooking = 1) and S.sagu is null))
	and ((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo))))
	-- Fecha de llegada
	or (((@BasedOnArrival = 1 and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo)
	-- Fecha de booking
	or (@BasedOnBooking = 1 and S.sagu is not null and G.guBookD between @DateFrom and @DateTo))
	-- Procesable
	and ((@ConsiderPending = 0 and S.saProc = 1) or (@ConsiderPending = 1 and S.saProc = 0))))
	-- No downgrades
	and (ST.ststc <> 'DG'
	-- Downgrades cuya membresia de referencia esta dentro del periodo
	or (@ConsiderCancel = 0 and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)))
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ',')))
	-- PRs
	and (@PRs = 'ALL' or (S.saPR1 in (select item from split(@PRs, ','))
		or S.saPR2 in (select item from split(@PRs, ','))
		or S.saPR3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
	-- Countries
	and (@Countries = 'ALL' or G.guco in (select item from split(@Countries, ',')))
	-- Agencies
	and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
	-- Markets
	and (@Markets = 'ALL' or G.gumk in (select item from split(@Markets, ',')))
group by S.saPR2

-- SI REQUIERE LAS VENTAS DEL PR DE SU LS POR DEFAULT
UNION ALL
select
	S.saPR2,
	Sum(S.saGrossAmount * [dbo].UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT))
from Sales S
join Personnel p on s.saPR2 = p.peID AND p.peTeamType = 'GS' AND (@LeadSources = 'ALL' OR p.pePlaceID in (select item from split(@LeadSources, ',')))
where 
	@BasedOnPRLocation = 1 
	AND saProcD BETWEEN @DateFrom and @DateTo
	AND (saPR2 is not null)
	AND (s.saCancelD NOT BETWEEN  @DateFrom and @DateTo OR s.saCancel = 0)
	AND s.sals <> p.pePlaceID
group by S.saPR2

-- Monto de ventas (PR 3)
-- =============================================
union all
select
	S.saPR3,
	Sum(S.saGrossAmount * [dbo].UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	inner join LeadSources L on L.lsID = S.sals
	left join Guests G on S.sagu = G.guID
where
	-- PR 3
	S.saPR3 is not null
	-- Fecha de procesable
	and (((((@BasedOnArrival = 0 and @BasedOnBooking = 0) or ((@BasedOnArrival = 1 or @BasedOnBooking = 1) and S.sagu is null))
	and ((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo))))
	-- Fecha de llegada
	or (((@BasedOnArrival = 1 and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo)
	-- Fecha de booking
	or (@BasedOnBooking = 1 and S.sagu is not null and G.guBookD between @DateFrom and @DateTo))
	-- Procesable
	and ((@ConsiderPending = 0 and S.saProc = 1) or (@ConsiderPending = 1 and S.saProc = 0))))
	-- No downgrades
	and (ST.ststc <> 'DG'
	-- Downgrades cuya membresia de referencia esta dentro del periodo
	or (@ConsiderCancel = 0 and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)))
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ',')))
	-- PRs
	and (@PRs = 'ALL' or (S.saPR1 in (select item from split(@PRs, ','))
		or S.saPR2 in (select item from split(@PRs, ','))
		or S.saPR3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
	-- Countries
	and (@Countries = 'ALL' or G.guco in (select item from split(@Countries, ',')))
	-- Agencies
	and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
	-- Markets
	and (@Markets = 'ALL' or G.gumk in (select item from split(@Markets, ',')))
group by S.saPR3

-- SI REQUIERE LAS VENTAS DEL PR DE SU LS POR DEFAULT
UNION ALL
select
	S.saPR3,
	Sum(S.saGrossAmount * [dbo].UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT))
from Sales S
join Personnel p on s.saPR3 = p.peID AND p.peTeamType = 'GS' AND (@LeadSources = 'ALL' OR p.pePlaceID in (select item from split(@LeadSources, ',')))
where 
	@BasedOnPRLocation = 1 
	AND saProcD BETWEEN @DateFrom and @DateTo
	AND (saPR3 is not null)
	AND (s.saCancelD NOT BETWEEN  @DateFrom and @DateTo OR s.saCancel = 0)
	AND s.sals <> p.pePlaceID
group by S.saPR3

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRSalesRoomAvailables]    Script Date: 07/28/2016 13:17:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de disponibles por PR y sala
** 
** [axperez]		03/Dic/2013 Created. Copiado de UFN_OR_GetPRAvailables
** [edgrodriguez]	05/May/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/
CREATE function [dbo].[UFN_OR_GetPRSalesRoomAvailables](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Clave de los Lead Sources
	@ConsiderQuinellas bit = 0,	-- Indica si se debe considerar quinielas
	@ConsiderFollow bit = 0,	-- Indica si se debe considerar seguimiento
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada	
)
returns @Table table (
	PR varchar(10),
	SalesRoom varchar(10),
	Availables money
)
as
begin

insert @Table

-- Disponibles (PR Info)
-- =============================================
select
	guPRInfo,
	gusr, 
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then 1 else guShowsQty end)
from Guests
where
	-- PR Info
	guPRInfo is not null
	-- Fecha de contacto
	and ((@BasedOnArrival = 0 and guInfoD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Contactado
	and guInfo = 1))
	-- No Antes In & Out
	and guAntesIO = 0
	-- Disponible
	and guAvail = 1
	-- Lead Source
	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
group by guPRInfo, gusr

-- Disponibles (PR 1)
-- =============================================
union all
select
	guPRInvit1,
	gusr, 
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT)
		else guShowsQty * dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT) end)
from Guests
where
	-- PR 1
	guPRInvit1 is not null
	-- PR de contacto diferente de los PR de invitación
	and guPRInfo not in (IsNull(guPRInvit1, ''), IsNull(guPRInvit2, ''), IsNull(guPRInvit3, ''))
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and guInvit = 1))
	-- No Antes In & Out
	and guAntesIO = 0
	-- Disponible
	and guAvail = 1
	-- Lead Source
	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
group by guPRInvit1, gusr

-- Disponibles (PR 2)
-- =============================================
union all
select
	guPRInvit2,
	gusr, 
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT)
		else guShowsQty * dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT) end)
from Guests
where
	-- PR 2
	guPRInvit2 is not null
	-- PR de contacto diferente de los PR de invitación
	and guPRInfo not in (IsNull(guPRInvit1, ''), IsNull(guPRInvit2, ''), IsNull(guPRInvit3, ''))
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and guInvit = 1))
	-- No Antes In & Out
	and guAntesIO = 0
	-- Disponible
	and guAvail = 1
	-- Lead Source
	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
group by guPRInvit2, gusr

-- Disponibles (PR 3)
-- =============================================
union all
select
	guPRInvit3,
	gusr, 
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT)
		else guShowsQty * dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT) end)
from Guests
where
	-- PR 3
	guPRInvit3 is not null
	-- PR de contacto diferente de los PR de invitación
	and guPRInfo not in (IsNull(guPRInvit1, ''), IsNull(guPRInvit2, ''), IsNull(guPRInvit3, ''))
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and guInvit = 1))
	-- No Antes In & Out
	and guAntesIO = 0
	-- Disponible
	and guAvail = 1
	-- Lead Source
	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
group by guPRInvit3, gusr

-- Disponibles (PR de seguimiento)
-- =============================================
if @ConsiderFollow = 1
	insert @Table
	select
		guPRFollow,
		gusr, 
		Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then 1 else guShowsQty end)
	from Guests
	where
		-- PR de seguimiento
		guPRFollow is not null
		-- PR de contacto diferente del PR de seguimiento
		and guPRInfo <> guPRFollow
		-- PR de seguimiento diferente de los PR de invitación
		and guPRFollow not in (IsNull(guPRInvit1, ''), IsNull(guPRInvit2, ''), IsNull(guPRInvit3, ''))
		-- Fecha de seguimiento
		and ((@BasedOnArrival = 0 and guFollowD between @DateFrom and @DateTo)
		-- Fecha de llegada
		or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
		-- Con seguimiento
		and guFollow = 1))
		-- No Antes In & Out
		and guAntesIO = 0
		-- Disponible
		and guAvail = 1
		-- Lead Source
		and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
	group by guPRFollow, gusr

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRSalesRoomBookings]    Script Date: 07/28/2016 13:17:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de bookings por PR y sala
** 
** [wtorres]		30/Oct/2009 Created
** [wtorres]		11/Mar/2010 Modified. Agregue los parametros @ConsiderDirects y @ConsiderInOuts
** [wtorres]		16/Abr/2010 Modified. Agregue los parametros @PRs y @Program
** [wtorres]		26/Nov/2010 Modified. Ahora no se cuentan los bookings cancelados
** [wtorres]		03/Dic/2013 Modified. Agregue el parametro @BasedOnArrival
** [wtorres]		19/Dic/2013 Modified. Reemplace el parametro @ConsiderInOuts por @InOut para que el filtro de In & Outs sea mas simple
** [wtorres]		22/Feb/2014 Modified. Reemplace el parametro @ConsiderDirects por @Direct
** [lormartinez]	22/Sep/2014 Modified. Se agrega parametro para filtro basado en fecha de booking
** [wtorres]		15/Jul/2015 Modified. Elimine el parametro @BasedOnBooking porque no es necesario
** [edgrodriguez]	05/May/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/
CREATE function [dbo].[UFN_OR_GetPRSalesRoomBookings](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Clave de los Lead Sources
	@PRs varchar(8000) = 'ALL',		-- Claves de PRs
	@Program varchar(10) = 'ALL',	-- Clave de programa
	@ConsiderQuinellas bit = 0,		-- Indica si se debe considerar quinielas
	@FilterDeposit tinyint = 0,		-- Filtro de depositos:
									--		0. Sin filtro
									--		1. Con deposito (Deposits)
									--		2. Sin deposito (Flyers)
									--		3. Con deposito y shows sin deposito (Deposits & Flyers Show)
	@Direct int = -1,				-- Filtro de directas:
									--		-1. Sin filtro
									--		 0. No directas
									--		 1. Directas
	@InOut int = -1,				-- Filtro de In & Outs:
									--		-1. Sin filtro
									--		 0. No In & Outs
									--		 1. In & Outs
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	PR varchar(10),
	SalesRoom varchar(10),
	Books money
)
as
begin

insert @Table

-- Bookings (PR 1)
-- =============================================
select
	G.guPRInvit1,
	G.gusr,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) * case when @ConsiderQuinellas = 0 then 1 else G.guRoomsQty end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- PR 1
	G.guPRInvit1 is not null
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and G.guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and G.guInvit = 1))
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- No Quiniela Split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0)
		or (@FilterDeposit = 3 and (G.guDeposit > 0 or (G.guShow = 1 and G.guDeposit = 0))))
	-- Directas
	and (@Direct = -1 or G.guDirect = @Direct)
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
	-- No bookings cancelados
	and G.guBookCanc = 0
group by G.guPRInvit1, G.gusr
union all

-- Bookings (PR 2)
-- =============================================
select
	G.guPRInvit2,
	G.gusr,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) * case when @ConsiderQuinellas = 0 then 1 else G.guRoomsQty end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- PR 2
	G.guPRInvit2 is not null
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and G.guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and G.guInvit = 1))
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- No Quiniela Split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0)
		or (@FilterDeposit = 3 and (G.guDeposit > 0 or (G.guShow = 1 and G.guDeposit = 0))))
	-- Directas
	and (@Direct = -1 or G.guDirect = @Direct)
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
	-- No bookings cancelados
	and G.guBookCanc = 0
group by G.guPRInvit2, G.gusr
union all

-- Bookings (PR 3)
-- =============================================
select
	G.guPRInvit3,
	G.gusr,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) * case when @ConsiderQuinellas = 0 then 1 else G.guRoomsQty end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- PR 3
	G.guPRInvit3 is not null
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and G.guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and G.guInvit = 1))
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- No Quiniela Split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0)
		or (@FilterDeposit = 3 and (G.guDeposit > 0 or (G.guShow = 1 and G.guDeposit = 0))))
	-- Directas
	and (@Direct = -1 or G.guDirect = @Direct)
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
	-- No bookings cancelados
	and G.guBookCanc = 0
group by G.guPRInvit3, G.gusr

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRSalesRoomContacts]    Script Date: 07/28/2016 13:17:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de contactos por PR y sala
** 
** [axperez]	03/Dic/2013 Created. Copiado de UFN_OR_GetPRContacts
** [edgrodriguez]	05/May/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/
CREATE function [dbo].[UFN_OR_GetPRSalesRoomContacts](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Clave de los Lead Sources
	@ConsiderFollow bit = 0,	-- Indica si se debe considerar seguimiento
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	PR varchar(10),
	SalesRoom varchar(10), 
	Contacts money	
)
as
begin

insert @Table

-- Contactos (PR Info)
-- =============================================
select
	guPRInfo,
	gusr,
	Count(*)
from Guests
where
	-- PR de contacto
	guPRInfo is not null
	-- Fecha de contacto
	and ((@BasedOnArrival = 0 and guInfoD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Contactado
	and guInfo = 1))
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
group by guPRInfo, gusr
union all

-- Contactos (PR 1)
-- =============================================
select
	guPRInvit1,
	gusr, 
	Sum(dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT))
from Guests
where
	-- PR 1
	guPRInvit1 is not null
	-- PR de contacto diferente de los PR de invitacion
	and guPRInfo not in (IsNull(guPRInvit1, ''), IsNull(guPRInvit2, ''), IsNull(guPRInvit3, ''))
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and guInvit = 1))
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
group by guPRInvit1, gusr
union all

-- Contactos (PR 2)
-- =============================================
select
	guPRInvit2,
	gusr, 
	Sum(dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT))
from Guests
where
	-- PR 2
	guPRInvit2 is not null
	-- PR de contacto diferente de los PR de invitacion
	and guPRInfo not in (IsNull(guPRInvit1, ''), IsNull(guPRInvit2, ''), IsNull(guPRInvit3, ''))
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and guInvit = 1))
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
group by guPRInvit2, gusr
union all

-- Contactos (PR 3)
-- =============================================
select
	guPRInvit3,
	gusr, 
	Sum(dbo.UFN_OR_GetPercentageSalesman(guPRInvit1, guPRInvit2, guPRInvit3, DEFAULT, DEFAULT))	
from Guests
where
	-- PR 3
	guPRInvit3 is not null
	-- PR de contacto diferente de los PR de invitacion
	and guPRInfo not in (IsNull(guPRInvit1, ''), IsNull(guPRInvit2, ''), IsNull(guPRInvit3, ''))
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and guInvit = 1))
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
group by guPRInvit3, gusr

-- Contactos (PR de seguimiento)
-- =============================================
if @ConsiderFollow = 1
	insert @Table
	select
		guPRFollow,
		gusr,
		Count(*)
	from Guests
	where
		-- PR de seguimiento
		guPRFollow is not null
		-- PR de contacto diferente del PR de seguimiento
		and guPRInfo <> guPRFollow
		-- PR de seguimiento diferente de los PR de invitacion
		and guPRFollow not in (IsNull(guPRInvit1, ''), IsNull(guPRInvit2, ''), IsNull(guPRInvit3, ''))
		-- Fecha de seguimiento
		and ((@BasedOnArrival = 0 and guFollowD between @DateFrom and @DateTo)
		-- Fecha de llegada
		or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
		-- Con seguimiento
		and guFollow = 1))
		-- No Rebook
		and guRef is null
		-- No Antes In & Out
		and guAntesIO = 0
		-- Lead Source
		and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
	group by guPRFollow, gusr

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRSalesRoomSales]    Script Date: 07/28/2016 13:17:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas por PR y sala
** 
** [wtorres]		30/Oct/2009 Created
** [wtorres]		24/Nov/2009 Modified. Agregue el parametro @ConsiderCancel
** [wtorres]		16/Abr/2010 Modified. Agregue los parametros @PRs y @Program
** [wtorres]		16/Nov/2011 Modified. Agregue el parametro @ConsiderPending
** [wtorres]		19/Nov/2013 Modified. Ahora los downgrades se cuentan en base a la categoria de tipo de venta
** [axperez]		04/Dic/2013 Modified. Agregue el parametro @ConsiderCancel
** [lormartinez]	22/Sep/2014 Modified. Se agrega parametro para filtro basado en fecha de booking
** [wtorres]		15/Jul/2015 Modified. Correccion de error. No estaba contando las ventas que no tuvieran Guest ID
** [wtorres]		09/Ene/2016 Modified. Correccion de error. No estaba contando las ventas pendientes (basado en la fecha de venta)
**								cuya fecha de procesable no estuviera en el rango del reporte
** [edgrodriguez]	05/May/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/
CREATE function [dbo].[UFN_OR_GetPRSalesRoomSales](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Clave de los Lead Sources
	@PRs varchar(8000) = 'ALL',		-- Claves de PRs
	@Program varchar(10) = 'ALL',	-- Clave de programa
	@ConsiderSelfGen bit = 0,		-- Indica si se debe considerar Self Gen
	@ConsiderOutOfPending bit = 0,	-- Indica si se debe considerar Out Of Pending
	@ConsiderCancel bit = 0,		-- Indica si se debe considerar canceladas
	@FilterDeposit tinyint = 0,		-- Filtro de depositos:
									--		0. Sin filtro
									--		1. Con deposito (Deposits)
									--		2. Sin deposito (Flyers)
	@ConsiderPending bit = 0,		-- Indica si se debe considerar pendientes
	@BasedOnArrival bit = 0,		-- Indica si se debe basar en la fecha de llegada
	@BasedOnBooking bit = 0			-- Indica si se debe basar en la fecha de booking
)
returns @Table table (
	PR varchar(10),
	SalesRoom varchar(10),
	Sales money
)
as
begin

insert @Table

-- Numero de ventas (PR 1)
-- =============================================
select
	S.saPR1,
	S.sasr,
	Sum([dbo].UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	inner join LeadSources L on L.lsID = S.sals
	left join Guests G on S.sagu = G.guID
where
	-- PR 1
	S.saPR1 is not null
	-- Fecha de procesable
	and ((((@BasedOnArrival = 0 and @BasedOnBooking = 0) or ((@BasedOnArrival = 1 or @BasedOnBooking = 1) and S.sagu is null))
	and ((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo))))
	-- Fecha de llegada
	or (((@BasedOnArrival = 1 and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo)
	-- Fecha de booking
	or (@BasedOnBooking = 1 and S.sagu is not null and G.guBookD between @DateFrom and @DateTo))
	-- Procesable
	and ((@ConsiderPending = 0 and S.saProc = 1) or (@ConsiderPending = 1 and S.saProc = 0))))
	-- No downgrades
	and ST.ststc <> 'DG'
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (S.saPR1 in (select item from split(@PRs, ','))
		or S.saPR2 in (select item from split(@PRs, ','))
		or S.saPR3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
group by S.saPR1, S.sasr

-- Numero de ventas (PR 2)
-- =============================================
union all
select
	S.saPR2,
	S.sasr,
	Sum([dbo].UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	inner join LeadSources L on L.lsID = S.sals
	left join Guests G on S.sagu = G.guID
where
	-- PR 2
	S.saPR2 is not null
	-- Fecha de procesable
	and ((((@BasedOnArrival = 0 and @BasedOnBooking = 0) or ((@BasedOnArrival = 1 or @BasedOnBooking = 1) and S.sagu is null))
	and ((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo))))
	-- Fecha de llegada
	or (((@BasedOnArrival = 1 and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo)
	-- Fecha de booking
	or (@BasedOnBooking = 1 and S.sagu is not null and G.guBookD between @DateFrom and @DateTo))
	-- Procesable
	and ((@ConsiderPending = 0 and S.saProc = 1) or (@ConsiderPending = 1 and S.saProc = 0))))
	-- No downgrades
	and ST.ststc <> 'DG'
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (S.saPR1 in (select item from split(@PRs, ','))
		or S.saPR2 in (select item from split(@PRs, ','))
		or S.saPR3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
group by S.saPR2, S.sasr

-- Numero de ventas (PR 3)
-- =============================================
union all
select
	S.saPR3,
	S.sasr,
	Sum([dbo].UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	inner join LeadSources L on L.lsID = S.sals
	left join Guests G on S.sagu = G.guID
where
	-- PR 3
	S.saPR3 is not null
	-- Fecha de procesable
	and ((((@BasedOnArrival = 0 and @BasedOnBooking = 0) or ((@BasedOnArrival = 1 or @BasedOnBooking = 1) and S.sagu is null))
	and ((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo))))
	-- Fecha de llegada
	or (((@BasedOnArrival = 1 and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo)
	-- Fecha de booking
	or (@BasedOnBooking = 1 and S.sagu is not null and G.guBookD between @DateFrom and @DateTo))
	-- Procesable
	and ((@ConsiderPending = 0 and S.saProc = 1) or (@ConsiderPending = 1 and S.saProc = 0))))
	-- No downgrades
	and ST.ststc <> 'DG'
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (S.saPR1 in (select item from split(@PRs, ','))
		or S.saPR2 in (select item from split(@PRs, ','))
		or S.saPR3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
group by S.saPR3, S.sasr

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRSalesRoomSalesAmount]    Script Date: 07/28/2016 13:17:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de ventas por PR y sala
** 
** [wtorres]		30/Oct/2009 Created
** [wtorres]		24/Nov/2009 Modified. Agregue el parametro @ConsiderCancel
** [wtorres]		16/Abr/2010 Modified. Agregue los parametros @PRs y @Program
** [wtorres]		16/Nov/2011 Modified. Agregue el parametro @ConsiderPending
** [wtorres]		19/Nov/2013 Modified. Ahora los downgrades se cuentan en base a la categoria de tipo de venta
** [axperez]		05/Dic/2013 Modified. Agregue el parametro @BasedOnArrival
** [lormartinez]	22/Sep/2014 Modified. Se agrega parametro para filtro basado en fecha de booking
** [wtorres]		15/Jul/2015 Modified. Correccion de error. No estaba contando las ventas que no tuvieran Guest ID
** [wtorres]		09/Ene/2016 Modified. Correccion de error. No estaba contando las ventas pendientes (basado en la fecha de venta)
**								cuya fecha de procesable no estuviera en el rango del reporte
** [edgrodriguez]	05/May/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/
CREATE function [dbo].[UFN_OR_GetPRSalesRoomSalesAmount](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Clave de los Lead Sources
	@PRs varchar(8000) = 'ALL',		-- Claves de PRs
	@Program varchar(10) = 'ALL',	-- Clave de programa
	@ConsiderSelfGen bit = 0,		-- Indica si se debe considerar Self Gen
	@ConsiderOutOfPending bit = 0,	-- Indica si se debe considerar Out Of Pending
	@ConsiderCancel bit = 0,		-- Indica si se debe considerar canceladas
	@FilterDeposit tinyint = 0,		-- Filtro de depositos:
									--		0. Sin filtro
									--		1. Con deposito (Deposits)
									--		2. Sin deposito (Flyers)
	@ConsiderPending bit = 0,		-- Indica si se debe considerar pendientes
	@BasedOnArrival bit = 0,		-- Indica si se debe basar en la fecha de llegada
	@BasedOnBooking bit = 0			-- Indica si se debe basar en la fecha de booking
)
returns @Table table (
	PR varchar(10),
	SalesRoom varchar(10),
	SalesAmount money
)
as
begin

insert @Table

-- Monto de ventas (PR 1)
-- =============================================
select
	S.saPR1,
	S.sasr,
	Sum(S.saGrossAmount * [dbo].UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	inner join LeadSources L on L.lsID = S.sals
	left join Guests G on S.sagu = G.guID
where
	-- PR 1
	S.saPR1 is not null
	-- Fecha de procesable
	and (((((@BasedOnArrival = 0 and @BasedOnBooking = 0) or ((@BasedOnArrival = 1 or @BasedOnBooking = 1) and S.sagu is null))
	and ((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo))))
	-- Fecha de llegada
	or (((@BasedOnArrival = 1 and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo)
	-- Fecha de booking
	or (@BasedOnBooking = 1 and S.sagu is not null and G.guBookD between @DateFrom and @DateTo))
	-- Procesable
	and ((@ConsiderPending = 0 and S.saProc = 1) or (@ConsiderPending = 1 and S.saProc = 0))))
	-- No downgrades
	and (ST.ststc <> 'DG'
	-- Downgrades cuya membresia de referencia esta dentro del periodo
	or (@ConsiderCancel = 0 and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)))
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (S.saPR1 in (select item from split(@PRs, ','))
		or S.saPR2 in (select item from split(@PRs, ','))
		or S.saPR3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
group by S.saPR1, S.sasr

-- Monto de ventas (PR 2)
-- =============================================
union all
select
	S.saPR2,
	S.sasr,
	Sum(S.saGrossAmount * [dbo].UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	inner join LeadSources L on L.lsID = S.sals
	left join Guests G on S.sagu = G.guID
where
	-- PR 2
	S.saPR2 is not null
	-- Fecha de procesable
	and (((((@BasedOnArrival = 0 and @BasedOnBooking = 0) or ((@BasedOnArrival = 1 or @BasedOnBooking = 1) and S.sagu is null))
	and ((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo))))
	-- Fecha de llegada
	or (((@BasedOnArrival = 1 and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo)
	-- Fecha de booking
	or (@BasedOnBooking = 1 and S.sagu is not null and G.guBookD between @DateFrom and @DateTo))
	-- Procesable
	and ((@ConsiderPending = 0 and S.saProc = 1) or (@ConsiderPending = 1 and S.saProc = 0))))
	-- No downgrades
	and (ST.ststc <> 'DG'
	-- Downgrades cuya membresia de referencia esta dentro del periodo
	or (@ConsiderCancel = 0 and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)))
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (S.saPR1 in (select item from split(@PRs, ','))
		or S.saPR2 in (select item from split(@PRs, ','))
		or S.saPR3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
group by S.saPR2, S.sasr

-- Monto de ventas (PR 3)
-- =============================================
union all
select
	S.saPR3,
	S.sasr,
	Sum(S.saGrossAmount * [dbo].UFN_OR_GetPercentageSalesman(S.saPR1, S.saPR2, S.saPR3, DEFAULT, DEFAULT))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	inner join LeadSources L on L.lsID = S.sals
	left join Guests G on S.sagu = G.guID
where
	-- PR 3
	S.saPR3 is not null
	-- Fecha de procesable
	and (((((@BasedOnArrival = 0 and @BasedOnBooking = 0) or ((@BasedOnArrival = 1 or @BasedOnBooking = 1) and S.sagu is null))
	and ((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo))))
	-- Fecha de llegada
	or (((@BasedOnArrival = 1 and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo)
	-- Fecha de booking
	or (@BasedOnBooking = 1 and S.sagu is not null and G.guBookD between @DateFrom and @DateTo))
	-- Procesable
	and ((@ConsiderPending = 0 and S.saProc = 1) or (@ConsiderPending = 1 and S.saProc = 0))))
	-- No downgrades
	and (ST.ststc <> 'DG'
	-- Downgrades cuya membresia de referencia esta dentro del periodo
	or (@ConsiderCancel = 0 and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)))
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (S.saPR1 in (select item from split(@PRs, ','))
		or S.saPR2 in (select item from split(@PRs, ','))
		or S.saPR3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
group by S.saPR3, S.sasr

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRSalesRoomShows]    Script Date: 07/28/2016 13:17:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por PR y sala
** 
** [wtorres]		30/Oct/2009 Created
** [wtorres]		01/Abr/2010 Modified. Agregue los parametros @ConsiderDirects y @ConsiderInOuts
** [wtorres]		16/Abr/2010 Modified. Agregue los parametros @PRs y @Program
** [wtorres]		19/Oct/2011 Modified. Reemplace el parametro @ConsiderInOuts por @InOut
** [wtorres]		16/Nov/2011 Modified. Agregue los parametros @WalkOut, @TourType y @ConsiderTourSale
** [axperez]		04/Dic/2013 Modified. Agregue los parametros @ConsiderDirectsAntesInOut y @BasedOnArrival
** [lormartinez]	22/Sep/2014 Modified. Se agrega parametro para filtro basado en fecha de booking
** [wtorres]		15/Jul/2015 Modified. Correccion de error. No estaba contando solo los shows cuando se basaba en la fecha de booking
** [edgrodriguez]	05/May/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/
CREATE function [dbo].[UFN_OR_GetPRSalesRoomShows](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000),			-- Clave de los Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@FilterDeposit tinyint = 0,			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
	@ConsiderDirects bit = 0,			-- Indica si se debe considerar directas
	@InOut int = -1,					-- Filtro de In & Outs:
										--		-1. Sin filtro
										--		 0. No In & Outs
										--		 1. In & Outs
	@WalkOut int = -1,					-- Filtro de Walk Outs:
										--		-1. Sin filtro
										--		 0. No Walk Outs
										--		 1. Walk Outs
	@TourType int = 0,					-- Filtro de tipo de tour:
										--		0. Sin filtro
										--		1. Tours regulares
										--		2. Tours de cortesia
										--		3. Tours de rescate
	@ConsiderDirectsAntesInOut bit = 0,	-- Indica si se debe considerar directas Antes In & Out
	@ConsiderTourSale bit = 0,			-- Indica si se debe considerar los shows con tour o venta
	@BasedOnArrival bit = 0,			-- Indica si se debe basar en la fecha de llegada
	@BasedOnBooking bit = 0				-- Indica si se debe basar en la fecha de booking
)
returns @Table table (
	PR varchar(10),
	SalesRoom varchar(10),
	Shows money
)
as
begin

insert @Table

-- Shows (PR 1)
-- =============================================
select
	G.guPRInvit1,
	G.gusr,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) * case when @ConsiderQuinellas = 0 then 1 else G.guShowsQty end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- PR 1
	G.guPRInvit1 is not null
	-- Fecha de show
	and ((@BasedOnArrival = 0 and @BasedOnBooking = 0 and G.guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (((@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo)
	-- Fecha de booking
	or (@BasedOnBooking = 1 and G.guBookD between @DateFrom and @DateTo))
	-- Con show
	and G.guShow = 1))
	-- No Quiniela Split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
	-- No Directas
	and (@ConsiderDirects = 0 or G.guDirect = 0)
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
	-- Walk Outs
	and (@WalkOut = -1 or G.guWalkOut = @WalkOut)
	-- Filtro de tipo de tour
	and (@TourType = 0 or (@TourType = 1 and G.guTour = 1) or (@TourType = 2 and G.guCTour = 1) or (@TourType = 3 and G.guSaveProgram = 1))
	-- No Directas no Antes In & Out
	and (@ConsiderDirectsAntesInOut = 0 or not (G.guDirect = 1 and G.guAntesIO = 0))
	-- Con tour o venta
	and (@ConsiderTourSale = 0 or (G.guTour = 1 or G.guSale = 1))
group by G.guPRInvit1, G.gusr

-- Shows (PR 2)
-- =============================================
union all
select
	G.guPRInvit2,
	G.gusr,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) * case when @ConsiderQuinellas = 0 then 1 else G.guShowsQty end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- PR 2
	G.guPRInvit2 is not null
	-- Fecha de show
	and ((@BasedOnArrival = 0 and @BasedOnBooking = 0 and G.guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (((@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo)
	-- Fecha de booking
	or (@BasedOnBooking = 1 and G.guBookD between @DateFrom and @DateTo))
	-- Con show
	and G.guShow = 1))
	-- No Quiniela Split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
	-- No Directas
	and (@ConsiderDirects = 0 or G.guDirect = 0)
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
	-- Walk Outs
	and (@WalkOut = -1 or G.guWalkOut = @WalkOut)
	-- Filtro de tipo de tour
	and (@TourType = 0 or (@TourType = 1 and G.guTour = 1) or (@TourType = 2 and G.guCTour = 1) or (@TourType = 3 and G.guSaveProgram = 1))
	-- No Directas no Antes In & Out
	and (@ConsiderDirectsAntesInOut = 0 or not (G.guDirect = 1 and G.guAntesIO = 0))
	-- Con tour o venta
	and (@ConsiderTourSale = 0 or (G.guTour = 1 or G.guSale = 1))
group by G.guPRInvit2, G.gusr

-- Shows (PR 3)
-- =============================================
union all
select
	G.guPRInvit3,
	G.gusr,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) * case when @ConsiderQuinellas = 0 then 1 else G.guShowsQty end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- PR 3
	G.guPRInvit3 is not null
	-- Fecha de show
	and ((@BasedOnArrival = 0 and @BasedOnBooking = 0 and G.guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (((@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo)
	-- Fecha de booking
	or (@BasedOnBooking = 1 and G.guBookD between @DateFrom and @DateTo))
	-- Con show
	and G.guShow = 1))
	-- No Quiniela Split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
	-- No Directas
	and (@ConsiderDirects = 0 or G.guDirect = 0)
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
	-- Walk Outs
	and (@WalkOut = -1 or G.guWalkOut = @WalkOut)
	-- Filtro de tipo de tour
	and (@TourType = 0 or (@TourType = 1 and G.guTour = 1) or (@TourType = 2 and G.guCTour = 1) or (@TourType = 3 and G.guSaveProgram = 1))
	-- No Directas no Antes In & Out
	and (@ConsiderDirectsAntesInOut = 0 or not (G.guDirect = 1 and G.guAntesIO = 0))
	-- Con tour o venta
	and (@ConsiderTourSale = 0 or (G.guTour = 1 or G.guSale = 1))
group by G.guPRInvit3, G.gusr

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetPRShows]    Script Date: 07/28/2016 13:17:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por PR
** 
** [wtorres]		18/Sep/2009 Created
** [wtorres]		23/Sep/2009 Modified. Convertido a funcion. Agregue los parametros @LeadSources, @ConsiderShowsQty y @ConsiderQuinellasSplit
** [wtorres]		27/Oct/2009 Agregue los parametros @ConsiderQuinellas y @FilterDeposit y elimine los parametros @ConsiderShowsQty
								y @ConsiderQuinellasSplit
** [wtorres]		02/Ene/2010 Modified. Agregue los parametros @ConsiderDirects y @ConsiderInOuts
** [wtorres]		16/Abr/2010 Modified. Agregue los parametros @PRs, @Program y @ConsiderDirectsAntesInOut
** [wtorres]		18/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [wtorres]		19/Oct/2011 Modified. Reemplace el parametro @ConsiderInOuts por @InOut y agregue el parametro @WalkOut
** [wtorres]		01/Nov/2011 Modified. Agregue los parametros @TourType y @ConsiderTourSale
** [gmaya]			11/Ago/2014 Modified. Agregue el parametro de formas de pago
** [lormartinez]	22/Sep/2014 Modified. Se agrega parametro para filtro basado en fecha de booking
** [wtorres]		15/Jul/2015 Modified. Correccion de error. No estaba contando solo los shows cuando se basaba en la fecha de booking
** [lormartinez]	15/Dic/2015 Modified. Se reimplementa filtro por formas de pago
** [erosado]		02/Mar/2016 Modified. Se agrego parametro para  el filtro para SelfGen
** [erosado]		04/Mar/2016 Modified. Agregue los parametros @SalesRooms, @Countries, @Agencies y @Markets
** [edgrodriguez]	05/May/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/
create function [dbo].[UFN_OR_GetPRShows](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(max)='ALL',	-- Clave de los Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@FilterDeposit tinyint = 0,			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
	@ConsiderDirects bit = 0,			-- Indica si se debe considerar directas
	@InOut int = -1,					-- Filtro de In & Outs:
										--		-1. Sin filtro
										--		 0. No In & Outs
										--		 1. In & Outs
	@WalkOut int = -1,					-- Filtro de Walk Outs:
										--		-1. Sin filtro
										--		 0. No Walk Outs
										--		 1. Walk Outs
	@TourType int = 0,					-- Filtro de tipo de tour:
										--		0. Sin filtro
										--		1. Tours regulares
										--		2. Tours de cortesia
										--		3. Tours de rescate
	@ConsiderDirectsAntesInOut bit = 0,	-- Indica si se debe considerar directas Antes In & Out
	@ConsiderTourSale bit = 0,			-- Indica si se debe considerar los shows con tour o venta
	@BasedOnArrival bit = 0,			-- Indica si se debe basar en la fecha de llegada
	@BasedOnBooking bit = 0,				-- Indica si se debe basar en la fecha de booking,
										
	@PaymentTypes varchar(MAX) ='ALL',	--Indica los modos de pago
  
	@SelfGen int = -1,					-- Filtro de SelfGen:
										--		-1. Sin filtro
										--		 0. No SelfGen
										--		 1. SelfGen
	@SalesRooms varchar(max) = 'ALL',	-- Clave de las salas de ventas
	@Countries varchar(max) = 'ALL',	-- Clave de los paises
	@Agencies varchar(max) = 'ALL',		-- Clave de las agencias
	@Markets varchar(max) = 'ALL'		-- Clave de los mercados
)
returns @Table table (
	PR varchar(10),
	Shows money
)
as
begin

declare @tmpPaymentTypes table(item varchar(20))

--Si hay PaymentTypes se llena la lista
IF @PaymentTypes <> 'ALL'
BEGIN
 INSERT INTO @tmpPaymentTypes
 SELECT ITEM FROM dbo.Split(@PaymentTypes,',')
END

insert @Table

-- Shows (PR 1)
-- =============================================
select
	G.guPRInvit1,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) * case when @ConsiderQuinellas = 0 then 1 else G.guShowsQty end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls  
	OUTER APPLY (
		SELECT count(bd.bdID) bdCount
		FROM BookingDeposits bd
		WHERE bd.bdgu = g.guid
			and (@PaymentTypes ='ALL' or (@PaymentTypes <> 'ALL' and bd.bdpt IN (select item from @tmpPaymentTypes)) )
	) bk
where
	-- PR 1
	G.guPRInvit1 is not null
	-- Fecha de show
	and ((@BasedOnArrival = 0 and @BasedOnBooking = 0 and G.guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (((@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo)
	-- Fecha de booking
	or (@BasedOnBooking = 1 and G.guBookD between @DateFrom and @DateTo))
	-- Con show
	and G.guShow = 1))
	-- No Quiniela Split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
	-- No Directas
	and (@ConsiderDirects = 0 or G.guDirect = 0)
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
	-- Walk Outs
	and (@WalkOut = -1 or G.guWalkOut = @WalkOut)
	-- Filtro de tipo de tour
	and (@TourType = 0 or (@TourType = 1 and G.guTour = 1) or (@TourType = 2 and G.guCTour = 1) or (@TourType = 3 and G.guSaveProgram = 1))
	-- No Directas no Antes In & Out
	and (@ConsiderDirectsAntesInOut = 0 or not (G.guDirect = 1 and G.guAntesIO = 0))
	-- Con tour o venta
	and (@ConsiderTourSale = 0 or (G.guTour = 1 or G.guSale = 1))
	-- Filtro de tipos de pago
	and (@PaymentTypes ='ALL' or (@PaymentTypes <> 'ALL' and ISNULL(bk.bdCount, 0) > 0 ))
	-- Filtro SelfGen
	and (@SelfGen = -1 or G.guSelfGen = @SelfGen)
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
	-- Countries
	and (@Countries = 'ALL' or G.guco in (select item from split(@Countries, ',')))
	-- Agencies
	and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
	-- Markets
	and (@Markets = 'ALL' or G.gumk in (select item from split(@Markets, ',')))
group by G.guPRInvit1

-- Shows (PR 2)
-- =============================================
union all
select
	G.guPRInvit2,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) * case when @ConsiderQuinellas = 0 then 1 else G.guShowsQty end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls  
	OUTER APPLY (
		SELECT count(bd.bdID) bdCount
		FROM BookingDeposits bd
		WHERE bd.bdgu = g.guid
			and (@PaymentTypes ='ALL' or (@PaymentTypes <> 'ALL' and bd.bdpt IN (select item from @tmpPaymentTypes)) )
	) bk
where
	-- PR 2
	G.guPRInvit2 is not null
	-- Fecha de show
	and ((@BasedOnArrival = 0 and @BasedOnBooking = 0 and G.guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (((@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo)
	-- Fecha de booking
	or (@BasedOnBooking = 1 and G.guBookD between @DateFrom and @DateTo))
	-- Con show
	and G.guShow = 1))
	-- No Quiniela Split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
	-- No Directas
	and (@ConsiderDirects = 0 or G.guDirect = 0)
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
	-- Walk Outs
	and (@WalkOut = -1 or G.guWalkOut = @WalkOut)
	-- Filtro de tipo de tour
	and (@TourType = 0 or (@TourType = 1 and G.guTour = 1) or (@TourType = 2 and G.guCTour = 1) or (@TourType = 3 and G.guSaveProgram = 1))
	-- No Directas no Antes In & Out
	and (@ConsiderDirectsAntesInOut = 0 or not (G.guDirect = 1 and G.guAntesIO = 0))
	-- Con tour o venta
	and (@ConsiderTourSale = 0 or (G.guTour = 1 or G.guSale = 1))
	 -- Filtro de tipos de pago
	and (@PaymentTypes ='ALL' or (@PaymentTypes <> 'ALL' and ISNULL(bk.bdCount,0) > 0 ))
	-- Filtro SelfGen
	and (@SelfGen = -1 or G.guSelfGen = @SelfGen)
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
	-- Countries
	and (@Countries = 'ALL' or G.guco in (select item from split(@Countries, ',')))
	-- Agencies
	and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
	-- Markets
	and (@Markets = 'ALL' or G.gumk in (select item from split(@Markets, ',')))
group by G.guPRInvit2

-- Shows (PR 3)
-- =============================================
union all
select
	G.guPRInvit3,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) * case when @ConsiderQuinellas = 0 then 1 else G.guShowsQty end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls  
	OUTER APPLY (
		SELECT count(bd.bdID) bdCount
		FROM BookingDeposits bd
		WHERE bd.bdgu = g.guid
			and (@PaymentTypes ='ALL' or (@PaymentTypes <> 'ALL' and bd.bdpt IN (select item from @tmpPaymentTypes)) )
	) bk
where
	-- PR 3
	G.guPRInvit3 is not null
	-- Fecha de show
	and ((@BasedOnArrival = 0 and @BasedOnBooking = 0 and G.guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (((@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo)
	-- Fecha de booking
	or (@BasedOnBooking = 1 and G.guBookD between @DateFrom and @DateTo))
	-- Con show
	and G.guShow = 1))
	-- No Quiniela Split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
	-- No Directas
	and (@ConsiderDirects = 0 or G.guDirect = 0)
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
	-- Walk Outs
	and (@WalkOut = -1 or G.guWalkOut = @WalkOut)
	-- Filtro de tipo de tour
	and (@TourType = 0 or (@TourType = 1 and G.guTour = 1) or (@TourType = 2 and G.guCTour = 1) or (@TourType = 3 and G.guSaveProgram = 1))
	-- No Directas no Antes In & Out
	and (@ConsiderDirectsAntesInOut = 0 or not (G.guDirect = 1 and G.guAntesIO = 0))
	-- Con tour o venta
	and (@ConsiderTourSale = 0 or (G.guTour = 1 or G.guSale = 1))
	-- Filtro de tipos de pago
	and (@PaymentTypes ='ALL' or (@PaymentTypes <> 'ALL' and ISNULL(bk.bdCount,0) > 0 ))
	-- Filtro SelfGen
	and (@SelfGen = -1 or G.guSelfGen = @SelfGen)
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
	-- Countries
	and (@Countries = 'ALL' or G.guco in (select item from split(@Countries, ',')))
	-- Agencies
	and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
	-- Markets
	and (@Markets = 'ALL' or G.gumk in (select item from split(@Markets, ',')))
group by G.guPRInvit3

return
end
GO


