USE [OrigosVCPalace]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetContractAgencyArrivals]    Script Date: 08/19/2016 10:29:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetContractAgencyArrivals]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetContractAgencyArrivals]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetContractAgencyAvailables]    Script Date: 08/19/2016 10:29:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetContractAgencyAvailables]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetContractAgencyAvailables]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetContractAgencyBookings]    Script Date: 08/19/2016 10:29:17 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetContractAgencyBookings]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetContractAgencyBookings]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetContractAgencyContacts]    Script Date: 08/19/2016 10:29:17 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetContractAgencyContacts]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetContractAgencyContacts]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableArrivals]    Script Date: 08/19/2016 10:29:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableArrivals]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableArrivals]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableAvailables]    Script Date: 08/19/2016 10:29:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableAvailables]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableAvailables]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableBookings]    Script Date: 08/19/2016 10:29:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableBookings]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableBookings]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableContacts]    Script Date: 08/19/2016 10:29:19 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableContacts]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableContacts]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableSales]    Script Date: 08/19/2016 10:29:19 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableSales]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableSales]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableSalesAmount]    Script Date: 08/19/2016 10:29:19 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableSalesAmount]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableSalesAmount]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableShows]    Script Date: 08/19/2016 10:29:20 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableShows]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableShows]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetContractAgencySales]    Script Date: 08/19/2016 10:29:20 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetContractAgencySales]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetContractAgencySales]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetContractAgencySalesAmount]    Script Date: 08/19/2016 10:29:20 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetContractAgencySalesAmount]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetContractAgencySalesAmount]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetContractAgencyShows]    Script Date: 08/19/2016 10:29:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetContractAgencyShows]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetContractAgencyShows]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMemberTypeAgencyArrivals]    Script Date: 08/19/2016 10:29:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetMemberTypeAgencyArrivals]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetMemberTypeAgencyArrivals]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMemberTypeAgencyAvailables]    Script Date: 08/19/2016 10:29:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetMemberTypeAgencyAvailables]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetMemberTypeAgencyAvailables]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMemberTypeAgencyBookings]    Script Date: 08/19/2016 10:29:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetMemberTypeAgencyBookings]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetMemberTypeAgencyBookings]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMemberTypeAgencyContacts]    Script Date: 08/19/2016 10:29:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetMemberTypeAgencyContacts]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetMemberTypeAgencyContacts]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableArrivals]    Script Date: 08/19/2016 10:29:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableArrivals]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableArrivals]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableAvailables]    Script Date: 08/19/2016 10:29:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableAvailables]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableAvailables]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableBookings]    Script Date: 08/19/2016 10:29:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableBookings]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableBookings]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableContacts]    Script Date: 08/19/2016 10:29:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableContacts]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableContacts]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableSales]    Script Date: 08/19/2016 10:29:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableSales]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableSales]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableSalesAmount]    Script Date: 08/19/2016 10:29:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableSalesAmount]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableSalesAmount]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableShows]    Script Date: 08/19/2016 10:29:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableShows]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableShows]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMemberTypeAgencySales]    Script Date: 08/19/2016 10:29:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetMemberTypeAgencySales]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetMemberTypeAgencySales]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMemberTypeAgencySalesAmount]    Script Date: 08/19/2016 10:29:26 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetMemberTypeAgencySalesAmount]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetMemberTypeAgencySalesAmount]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMemberTypeAgencyShows]    Script Date: 08/19/2016 10:29:26 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetMemberTypeAgencyShows]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetMemberTypeAgencyShows]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMonthAgencyLeadSourceArrivals]    Script Date: 08/19/2016 10:29:26 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetMonthAgencyLeadSourceArrivals]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetMonthAgencyLeadSourceArrivals]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMonthAgencyLeadSourceAvailables]    Script Date: 08/19/2016 10:29:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetMonthAgencyLeadSourceAvailables]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetMonthAgencyLeadSourceAvailables]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMonthAgencyLeadSourceBookings]    Script Date: 08/19/2016 10:29:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetMonthAgencyLeadSourceBookings]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetMonthAgencyLeadSourceBookings]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMonthAgencyLeadSourceContacts]    Script Date: 08/19/2016 10:29:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetMonthAgencyLeadSourceContacts]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetMonthAgencyLeadSourceContacts]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMonthAgencyLeadSourceSales]    Script Date: 08/19/2016 10:29:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetMonthAgencyLeadSourceSales]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetMonthAgencyLeadSourceSales]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMonthAgencyLeadSourceSalesAmount]    Script Date: 08/19/2016 10:29:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetMonthAgencyLeadSourceSalesAmount]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetMonthAgencyLeadSourceSalesAmount]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMonthAgencyLeadSourceShows]    Script Date: 08/19/2016 10:29:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetMonthAgencyLeadSourceShows]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetMonthAgencyLeadSourceShows]
GO

USE [OrigosVCPalace]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetContractAgencyArrivals]    Script Date: 08/19/2016 10:29:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de llegadas por contrato y agencia
** 
** [galcocer]	04/Feb/2012 Created
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetContractAgencyArrivals](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL'	-- Claves de agencias
)
returns @Table table (
	Contract varchar(20),
	Agency varchar(35),
	Arrivals int
)
as
begin

insert @Table
select
	guO1,
	guag,
	Count(*)
from Guests
where
	-- Fecha de llegada
	guCheckInD between @DateFrom and @DateTo
	-- Con Check In
	and guCheckIn = 1
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
group by guO1, guag

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetContractAgencyAvailables]    Script Date: 08/19/2016 10:29:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de disponibles por contrato y agencia
** 
** [galcocer]	04/Feb/2012 Created
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetContractAgencyAvailables](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@ConsiderQuinellas bit = 0		-- Indica si se debe considerar quinielas
)
returns @Table table (
	Contract varchar(20),
	Agency varchar(35),
	Availables int
)
as
begin

insert @Table
select
	guO1,
	guag,
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
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
group by guO1, guag

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetContractAgencyBookings]    Script Date: 08/19/2016 10:29:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de bookings por contrato y agencia
** 
** [galcocer]	04/Feb/2012 Created
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetContractAgencyBookings](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@ConsiderQuinellas bit = 0,		-- Indica si se debe considerar quinielas
	@Direct int = -1,				-- Filtro de directas:
									--		-1. Sin filtro
									--		 0. No directas
									--		 1. Directas
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	Contract varchar(20),
	Agency varchar(35),
	Books int
)
as
begin

insert @Table
select
	guO1,
	guag,
	Sum(case when @ConsiderQuinellas = 0 then 1 else guRoomsQty end)
from Guests
where
	-- Fecha de booking
	((@BasedOnArrival = 0 and guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and guInvit = 1))
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
	-- No quinielas split
	and (@ConsiderQuinellas = 0 or guQuinellaSplit = 0)
	-- Directas
	and (@Direct = -1 or guDirect = @Direct)
	-- No bookings cancelados
	and guBookCanc = 0
group by guO1, guag

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetContractAgencyContacts]    Script Date: 08/19/2016 10:29:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de contactos por contrato y agencia
** 
** [galcocer]	04/Feb/2012 Created
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetContractAgencyContacts](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	Contract varchar(20),
	Agency varchar(35),
	Contacts int
)
as
begin

insert @Table
select
	guO1,
	guag,
	Count(*)
from Guests
where
	-- Fecha de contacto
	((@BasedOnArrival = 0 and guInfoD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Contactado
	and guInfo = 1))
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
group by guO1, guag

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableArrivals]    Script Date: 08/19/2016 10:29:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de llegadas por contrato, agencia, mercado y originalmente disponible
** 
** [galcocer]	04/Feb/2012 Created
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableArrivals](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL'	-- Claves de agencias
)
returns @Table table (
	Contract varchar(20),
	Agency varchar(35),
	Market varchar(10),
	OriginallyAvailable varchar(30),
	Arrivals int
)
as
begin

insert @Table
select
	guO1,
	guag,
	gumk,
	-- Si tiene invitacion, considerarlo en originalmente disponible
	case when (guOriginAvail | guInvit) = 1 then 'ORIGINALLY AVAILABLES' else 'ORIGINALLY UNAVAILABLES' end,
	Count(*)
from Guests
where
	-- Fecha de llegada
	guCheckInD between @DateFrom and @DateTo
	-- Con Check In
	and guCheckIn = 1
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
group by guO1, guag, gumk, (guOriginAvail | guInvit)

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableAvailables]    Script Date: 08/19/2016 10:29:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de disponibles por contrato, agencia, mercado y originalmente disponible
** 
** [galcocer]	04/Feb/2012 Created
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableAvailables](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@ConsiderQuinellas bit = 0		-- Indica si se debe considerar quinielas
)
returns @Table table (
	Contract varchar(20),
	Agency varchar(35),
	Market varchar(10),
	OriginallyAvailable varchar(30),
	Availables int
)
as
begin

insert @Table
select
	guO1,
	guag,
	gumk,
	-- Si tiene invitacion, considerarlo en originalmente disponible
	case when (guOriginAvail | guInvit) = 1 then 'ORIGINALLY AVAILABLES' else 'ORIGINALLY UNAVAILABLES' end,
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
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
group by guO1, guag, gumk, (guOriginAvail | guInvit)

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableBookings]    Script Date: 08/19/2016 10:29:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de bookings por contrato, agencia, mercado y originalmente disponible
** 
** [galcocer]	04/Feb/2012 Created
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableBookings](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@ConsiderQuinellas bit = 0,		-- Indica si se debe considerar quinielas
	@Direct int = -1,				-- Filtro de directas:
									--		-1. Sin filtro
									--		 0. No directas
									--		 1. Directas
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	Contract varchar(20),
	Agency varchar(35),
	Market varchar(10),
	OriginallyAvailable varchar(30),
	Books int
)
as
begin

insert @Table
select
	guO1,
	guag,
	gumk,
	-- Si tiene invitacion, considerarlo en originalmente disponible
	case when (guOriginAvail | guInvit) = 1 then 'ORIGINALLY AVAILABLES' else 'ORIGINALLY UNAVAILABLES' end,
	Sum(case when @ConsiderQuinellas = 0 then 1 else guRoomsQty end)
from Guests
where
	-- Fecha de booking
	((@BasedOnArrival = 0 and guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and guInvit = 1))
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
	-- No quinielas split
	and (@ConsiderQuinellas = 0 or guQuinellaSplit = 0)
	-- Directas
	and (@Direct = -1 or guDirect = @Direct)
	-- No bookings cancelados
	and guBookCanc = 0
group by guO1, guag, gumk, (guOriginAvail | guInvit)

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableContacts]    Script Date: 08/19/2016 10:29:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de contactos por contrato, agencia, mercado y originalmente disponible
** 
** [galcocer]	04/Feb/2012 Created
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableContacts](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	Contract varchar(20),
	Agency varchar(35),
	Market varchar(10),
	OriginallyAvailable varchar(30),
	Contacts int
)
as
begin

insert @Table
select
	guO1,
	guag,
	gumk,
	-- Si tiene invitacion, considerarlo en originalmente disponible
	case when (guOriginAvail | guInvit) = 1 then 'ORIGINALLY AVAILABLES' else 'ORIGINALLY UNAVAILABLES' end,
	Count(*)
from Guests
where
	-- Fecha de contacto
	((@BasedOnArrival = 0 and guInfoD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Contactado
	and guInfo = 1))
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
group by guO1, guag, gumk, (guOriginAvail | guInvit)

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableSales]    Script Date: 08/19/2016 10:29:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas por contrato, agencia, mercado y originalmente disponible
** 
** [galcocer]	04/Feb/2012 Created
** [wtorres]	19/Nov/2013 Modified. Ahora los downgrades se cuentan en base a la categoria de tipo de venta
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableSales](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@ConsiderSelfGen bit = 0,		-- Indica si se debe considerar Self Gen
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	Contract varchar(20),
	Agency varchar(35),
	Market varchar(10),
	OriginallyAvailable varchar(30),
	Sales int
)
as
begin

insert @Table
select
	G.guO1,
	G.guag,
	gumk,
	-- Si tiene invitacion, considerarlo en originalmente disponible
	case when (guOriginAvail | guInvit) = 1 then 'ORIGINALLY AVAILABLES' else 'ORIGINALLY UNAVAILABLES' end,
	Count(*)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
where
	-- Fecha de procesable
	(((@BasedOnArrival = 0 or (@BasedOnArrival = 1 and S.sagu is null)) and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1	and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo
	-- Procesable
	and S.saProc = 1))
	-- No downgrades
	and ST.ststc <> 'DG'
	-- No canceladas o canceladas fuera del periodo
	and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
	-- Lead Source
	and S.sals in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or G.gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
group by G.guO1, G.guag, gumk, (guOriginAvail | guInvit)

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableSalesAmount]    Script Date: 08/19/2016 10:29:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de ventas por contrato, agencia, mercado y originalmente disponible
** 
** [galcocer]	04/Feb/2012 Created
** [wtorres]	19/Nov/2013 Modified. Ahora los downgrades se cuentan en base a la categoria de tipo de venta
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableSalesAmount](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@ConsiderSelfGen bit = 0,		-- Indica si se debe considerar Self Gen
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	Contract varchar(20),
	Agency varchar(35),
	Market varchar(10),
	OriginallyAvailable varchar(30),
	SalesAmount money
)
as
begin

insert @Table
select
	G.guO1,
	G.guag,
	gumk,
	-- Si tiene invitacion, considerarlo en originalmente disponible
	case when (guOriginAvail | guInvit) = 1 then 'ORIGINALLY AVAILABLES' else 'ORIGINALLY UNAVAILABLES' end,
	Sum(saGrossAmount)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
where
	-- Fecha de procesable
	((((@BasedOnArrival = 0 or (@BasedOnArrival = 1 and S.sagu is null)) and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo
	-- Procesable
	and S.saProc = 1))
	-- No downgrades
	and (ST.ststc <> 'DG'
	-- Downgrades cuya membresia de referencia esta dentro del periodo
	or (ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)))
	-- No canceladas o canceladas fuera del periodo
	and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
	-- Lead Source
	and S.sals in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or G.gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
group by G.guO1, G.guag, gumk, (guOriginAvail | guInvit)

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableShows]    Script Date: 08/19/2016 10:29:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por contrato, agencia, mercado y originalmente disponible
** 
** [galcocer]	04/Feb/2012 Created
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetContractAgencyMarketOriginallyAvailableShows](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000),			-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',		-- Claves de mercados
	@Agencies varchar(max) = 'ALL',		-- Claves de agencias
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@ConsiderDirectsAntesInOut bit = 0,	-- Indica si se debe considerar directas Antes In & Out
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	Contract varchar(20),
	Agency varchar(35),
	Market varchar(10),
	OriginallyAvailable varchar(30),
	Shows int
)
as
begin

insert @Table
select
	guO1,
	guag,
	gumk,
	-- Si tiene invitacion, considerarlo en originalmente disponible
	case when (guOriginAvail | guInvit) = 1 then 'ORIGINALLY AVAILABLES' else 'ORIGINALLY UNAVAILABLES' end,
	Sum(case when @ConsiderQuinellas = 0 then 1 else guShowsQty end)
from Guests
where
	-- Fecha de show
	((@BasedOnArrival = 0 and guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Con show
	and guShow = 1))
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
	-- No quinielas split
	and (@ConsiderQuinellas = 0 or guQuinellaSplit = 0)
	-- No Directas no Antes In & Out
	and (@ConsiderDirectsAntesInOut = 0 or not (guDirect = 1 and guAntesIO = 0))
group by guO1, guag, gumk, (guOriginAvail | guInvit)

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetContractAgencySales]    Script Date: 08/19/2016 10:29:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas por contrato y agencia
** 
** [galcocer]	04/Feb/2012 Created
** [wtorres]	19/Nov/2013 Modified. Ahora los downgrades se cuentan en base a la categoria de tipo de venta
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetContractAgencySales](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@ConsiderSelfGen bit = 0,		-- Indica si se debe considerar Self Gen
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	Contract varchar(20),
	Agency varchar(35),
	Sales int
)
as
begin

insert @Table
select
	G.guO1,
	G.guag,
	Count(*)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
where
	-- Fecha de procesable
	(((@BasedOnArrival = 0 or (@BasedOnArrival = 1 and S.sagu is null)) and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1	and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo
	-- Procesable
	and S.saProc = 1))
	-- No downgrades
	and ST.ststc <> 'DG'
	-- No canceladas o canceladas fuera del periodo
	and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
	-- Lead Source
	and S.sals in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or G.gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
group by G.guO1, G.guag

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetContractAgencySalesAmount]    Script Date: 08/19/2016 10:29:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de ventas por contrato y agencia
** 
** [galcocer]	04/Feb/2012 Created
** [wtorres]	19/Nov/2013 Modified. Ahora los downgrades se cuentan en base a la categoria de tipo de venta
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetContractAgencySalesAmount](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@ConsiderSelfGen bit = 0,		-- Indica si se debe considerar Self Gen
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	Contract varchar(20),
	Agency varchar(35),
	SalesAmount money
)
as
begin

insert @Table
select
	G.guO1,
	G.guag,
	Sum(saGrossAmount)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
where
	-- Fecha de procesable
	((((@BasedOnArrival = 0 or (@BasedOnArrival = 1 and S.sagu is null)) and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo
	-- Procesable
	and S.saProc = 1))
	-- No downgrades
	and (ST.ststc <> 'DG'
	-- Downgrades cuya membresia de referencia esta dentro del periodo
	or (ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)))
	-- No canceladas o canceladas fuera del periodo
	and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
	-- Lead Source
	and S.sals in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or G.gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
group by G.guO1, G.guag

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetContractAgencyShows]    Script Date: 08/19/2016 10:29:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por contrato y agencia
** 
** [galcocer]	04/Feb/2012 Created
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetContractAgencyShows](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000),			-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',		-- Claves de mercados
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@ConsiderDirectsAntesInOut bit = 0,	-- Indica si se debe considerar directas Antes In & Out
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	Contract varchar(20),
	Agency varchar(35),
	Shows int
)
as
begin

insert @Table
select
	guO1,
	guag,
	Sum(case when @ConsiderQuinellas = 0 then 1 else guShowsQty end)
from Guests
where
	-- Fecha de show
	((@BasedOnArrival = 0 and guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Con show
	and guShow = 1))
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
	-- No quinielas split
	and (@ConsiderQuinellas = 0 or guQuinellaSplit = 0)
	-- No Directas no Antes In & Out
	and (@ConsiderDirectsAntesInOut = 0 or not (guDirect = 1 and guAntesIO = 0))
group by guO1, guag

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMemberTypeAgencyArrivals]    Script Date: 08/19/2016 10:29:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de llegadas por tipo de socio y agencia
** 
** [wtorres]	26/Ene/2012 Created
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetMemberTypeAgencyArrivals](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL'	-- Claves de agencias
)
returns @Table table (
	MemberType varchar(12),
	Agency varchar(35),
	Arrivals int
)
as
begin

insert @Table
select
	guGuestRef,
	guag,
	Count(*)
from Guests
where
	-- Fecha de llegada
	guCheckInD between @DateFrom and @DateTo
	-- Con Check In
	and guCheckIn = 1
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
group by guGuestRef, guag

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMemberTypeAgencyAvailables]    Script Date: 08/19/2016 10:29:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de disponibles por tipo de socio y agencia
** 
** [wtorres]	26/Ene/2012 Created
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetMemberTypeAgencyAvailables](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@ConsiderQuinellas bit = 0		-- Indica si se debe considerar quinielas
)
returns @Table table (
	MemberType varchar(12),
	Agency varchar(35),
	Availables int
)
as
begin

insert @Table
select
	guGuestRef,
	guag,
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
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
group by guGuestRef, guag

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMemberTypeAgencyBookings]    Script Date: 08/19/2016 10:29:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de bookings por tipo de socio y agencia
** 
** [wtorres]	26/Ene/2012 Created
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetMemberTypeAgencyBookings](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@ConsiderQuinellas bit = 0,		-- Indica si se debe considerar quinielas
	@Direct int = -1,				-- Filtro de directas:
									--		-1. Sin filtro
									--		 0. No directas
									--		 1. Directas
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	MemberType varchar(12),
	Agency varchar(35),
	Books int
)
as
begin

insert @Table
select
	guGuestRef,
	guag,
	Sum(case when @ConsiderQuinellas = 0 then 1 else guRoomsQty end)
from Guests
where
	-- Fecha de booking
	((@BasedOnArrival = 0 and guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and guInvit = 1))
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
	-- No quinielas split
	and (@ConsiderQuinellas = 0 or guQuinellaSplit = 0)
	-- Directas
	and (@Direct = -1 or guDirect = @Direct)
	-- No bookings cancelados
	and guBookCanc = 0
group by guGuestRef, guag

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMemberTypeAgencyContacts]    Script Date: 08/19/2016 10:30:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de contactos por tipo de socio y agencia
** 
** [wtorres]	26/Ene/2012 Created
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetMemberTypeAgencyContacts](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	MemberType varchar(12),
	Agency varchar(35),
	Contacts int
)
as
begin

insert @Table
select
	guGuestRef,
	guag,
	Count(*)
from Guests
where
	-- Fecha de contacto
	((@BasedOnArrival = 0 and guInfoD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Contactado
	and guInfo = 1))
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
group by guGuestRef, guag

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableArrivals]    Script Date: 08/19/2016 10:30:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de llegadas por tipo de socio, agencia, mercado y originalmente disponible
** 
** [wtorres]	26/Ene/2012 Created
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableArrivals](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL'	-- Claves de agencias
)
returns @Table table (
	MemberType varchar(12),
	Agency varchar(35),
	Market varchar(10),
	OriginallyAvailable varchar(30),
	Arrivals int
)
as
begin

insert @Table
select
	guGuestRef,
	guag,
	gumk,
	-- Si tiene invitacion, considerarlo en originalmente disponible
	case when (guOriginAvail | guInvit) = 1 then 'ORIGINALLY AVAILABLES' else 'ORIGINALLY UNAVAILABLES' end,
	Count(*)
from Guests
where
	-- Fecha de llegada
	guCheckInD between @DateFrom and @DateTo
	-- Con Check In
	and guCheckIn = 1
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
group by guGuestRef, guag, gumk, (guOriginAvail | guInvit)

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableAvailables]    Script Date: 08/19/2016 10:30:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de disponibles por tipo de socio, agencia, mercado y originalmente disponible
** 
** [wtorres]	26/Ene/2012 Created
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableAvailables](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@ConsiderQuinellas bit = 0		-- Indica si se debe considerar quinielas
)
returns @Table table (
	MemberType varchar(12),
	Agency varchar(35),
	Market varchar(10),
	OriginallyAvailable varchar(30),
	Availables int
)
as
begin

insert @Table
select
	guGuestRef,
	guag,
	gumk,
	-- Si tiene invitacion, considerarlo en originalmente disponible
	case when (guOriginAvail | guInvit) = 1 then 'ORIGINALLY AVAILABLES' else 'ORIGINALLY UNAVAILABLES' end,
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
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
group by guGuestRef, guag, gumk, (guOriginAvail | guInvit)

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableBookings]    Script Date: 08/19/2016 10:30:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de bookings por tipo de socio, agencia, mercado y originalmente disponible
** 
** [wtorres]	26/Ene/2012 Created
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableBookings](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@ConsiderQuinellas bit = 0,		-- Indica si se debe considerar quinielas
	@Direct int = -1,				-- Filtro de directas:
									--		-1. Sin filtro
									--		 0. No directas
									--		 1. Directas
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	MemberType varchar(12),
	Agency varchar(35),
	Market varchar(10),
	OriginallyAvailable varchar(30),
	Books int
)
as
begin

insert @Table
select
	guGuestRef,
	guag,
	gumk,
	-- Si tiene invitacion, considerarlo en originalmente disponible
	case when (guOriginAvail | guInvit) = 1 then 'ORIGINALLY AVAILABLES' else 'ORIGINALLY UNAVAILABLES' end,
	Sum(case when @ConsiderQuinellas = 0 then 1 else guRoomsQty end)
from Guests
where
	-- Fecha de booking
	((@BasedOnArrival = 0 and guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and guInvit = 1))
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
	-- No quinielas split
	and (@ConsiderQuinellas = 0 or guQuinellaSplit = 0)
	-- Directas
	and (@Direct = -1 or guDirect = @Direct)
	-- No bookings cancelados
	and guBookCanc = 0
group by guGuestRef, guag, gumk, (guOriginAvail | guInvit)

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableContacts]    Script Date: 08/19/2016 10:30:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de contactos por tipo de socio, agencia, mercado y originalmente disponible
** 
** [wtorres]	26/Ene/2012 Created
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableContacts](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	MemberType varchar(12),
	Agency varchar(35),
	Market varchar(10),
	OriginallyAvailable varchar(30),
	Contacts int
)
as
begin

insert @Table
select
	guGuestRef,
	guag,
	gumk,
	-- Si tiene invitacion, considerarlo en originalmente disponible
	case when (guOriginAvail | guInvit) = 1 then 'ORIGINALLY AVAILABLES' else 'ORIGINALLY UNAVAILABLES' end,
	Count(*)
from Guests
where
	-- Fecha de contacto
	((@BasedOnArrival = 0 and guInfoD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Contactado
	and guInfo = 1))
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
group by guGuestRef, guag, gumk, (guOriginAvail | guInvit)

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableSales]    Script Date: 08/19/2016 10:30:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas por tipo de socio, agencia, mercado y originalmente disponible
** 
** [wtorres]	26/Ene/2012 Created
** [wtorres]	19/Nov/2013 Modified. Ahora los downgrades se cuentan en base a la categoria de tipo de venta
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableSales](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@ConsiderSelfGen bit = 0,		-- Indica si se debe considerar Self Gen
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	MemberType varchar(12),
	Agency varchar(35),
	Market varchar(10),
	OriginallyAvailable varchar(30),
	Sales int
)
as
begin

insert @Table
select
	G.guGuestRef,
	G.guag,
	gumk,
	-- Si tiene invitacion, considerarlo en originalmente disponible
	case when (guOriginAvail | guInvit) = 1 then 'ORIGINALLY AVAILABLES' else 'ORIGINALLY UNAVAILABLES' end,
	Count(*)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
where
	-- Fecha de procesable
	(((@BasedOnArrival = 0 or (@BasedOnArrival = 1 and S.sagu is null)) and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1	and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo
	-- Procesable
	and S.saProc = 1))
	-- No downgrades
	and ST.ststc <> 'DG'
	-- No canceladas o canceladas fuera del periodo
	and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
	-- Lead Source
	and S.sals in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or G.gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
group by G.guGuestRef, G.guag, gumk, (guOriginAvail | guInvit)

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableSalesAmount]    Script Date: 08/19/2016 10:30:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de ventas por tipo de socio, agencia, mercado y originalmente disponible
** 
** [wtorres]	26/Ene/2012 Created
** [wtorres]	19/Nov/2013 Modified. Ahora los downgrades se cuentan en base a la categoria de tipo de venta
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableSalesAmount](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@ConsiderSelfGen bit = 0,		-- Indica si se debe considerar Self Gen
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	MemberType varchar(12),
	Agency varchar(35),
	Market varchar(10),
	OriginallyAvailable varchar(30),
	SalesAmount money
)
as
begin

insert @Table
select
	G.guGuestRef,
	G.guag,
	gumk,
	-- Si tiene invitacion, considerarlo en originalmente disponible
	case when (guOriginAvail | guInvit) = 1 then 'ORIGINALLY AVAILABLES' else 'ORIGINALLY UNAVAILABLES' end,
	Sum(saGrossAmount)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
where
	-- Fecha de procesable
	((((@BasedOnArrival = 0 or (@BasedOnArrival = 1 and S.sagu is null)) and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo
	-- Procesable
	and S.saProc = 1))
	-- No downgrades
	and (ST.ststc <> 'DG'
	-- Downgrades cuya membresia de referencia esta dentro del periodo
	or (ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)))
	-- No canceladas o canceladas fuera del periodo
	and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
	-- Lead Source
	and S.sals in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or G.gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
group by G.guGuestRef, G.guag, gumk, (guOriginAvail | guInvit)

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableShows]    Script Date: 08/19/2016 10:30:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por tipo de socio, agencia, mercado y originalmente disponible
** 
** [wtorres]	26/Ene/2012 Created
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetMemberTypeAgencyMarketOriginallyAvailableShows](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000),			-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',		-- Claves de mercados
	@Agencies varchar(max) = 'ALL',		-- Claves de agencias
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@ConsiderDirectsAntesInOut bit = 0,	-- Indica si se debe considerar directas Antes In & Out
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	MemberType varchar(12),
	Agency varchar(35),
	Market varchar(10),
	OriginallyAvailable varchar(30),
	Shows int
)
as
begin

insert @Table
select
	guGuestRef,
	guag,
	gumk,
	-- Si tiene invitacion, considerarlo en originalmente disponible
	case when (guOriginAvail | guInvit) = 1 then 'ORIGINALLY AVAILABLES' else 'ORIGINALLY UNAVAILABLES' end,
	Sum(case when @ConsiderQuinellas = 0 then 1 else guShowsQty end)
from Guests
where
	-- Fecha de show
	((@BasedOnArrival = 0 and guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Con show
	and guShow = 1))
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
	-- No quinielas split
	and (@ConsiderQuinellas = 0 or guQuinellaSplit = 0)
	-- No Directas no Antes In & Out
	and (@ConsiderDirectsAntesInOut = 0 or not (guDirect = 1 and guAntesIO = 0))
group by guGuestRef, guag, gumk, (guOriginAvail | guInvit)

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMemberTypeAgencySales]    Script Date: 08/19/2016 10:30:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas por tipo de socio y agencia
** 
** [wtorres]	26/Ene/2012 Created
** [wtorres]	19/Nov/2013 Modified. Ahora los downgrades se cuentan en base a la categoria de tipo de venta
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetMemberTypeAgencySales](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@ConsiderSelfGen bit = 0,		-- Indica si se debe considerar Self Gen
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	MemberType varchar(12),
	Agency varchar(35),
	Sales int
)
as
begin

insert @Table
select
	G.guGuestRef,
	G.guag,
	Count(*)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
where
	-- Fecha de procesable
	(((@BasedOnArrival = 0 or (@BasedOnArrival = 1 and S.sagu is null)) and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1	and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo
	-- Procesable
	and S.saProc = 1))
	-- No downgrades
	and ST.ststc <> 'DG'
	-- No canceladas o canceladas fuera del periodo
	and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
	-- Lead Source
	and S.sals in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or G.gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
group by G.guGuestRef, G.guag

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMemberTypeAgencySalesAmount]    Script Date: 08/19/2016 10:30:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de ventas por tipo de socio y agencia
** 
** [wtorres]	26/Ene/2012 Created
** [wtorres]	19/Nov/2013 Modified. Ahora los downgrades se cuentan en base a la categoria de tipo de venta
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetMemberTypeAgencySalesAmount](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@ConsiderSelfGen bit = 0,		-- Indica si se debe considerar Self Gen
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	MemberType varchar(12),
	Agency varchar(35),
	SalesAmount money
)
as
begin

insert @Table
select
	G.guGuestRef,
	G.guag,
	Sum(saGrossAmount)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
where
	-- Fecha de procesable
	((((@BasedOnArrival = 0 or (@BasedOnArrival = 1 and S.sagu is null)) and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo
	-- Procesable
	and S.saProc = 1))
	-- No downgrades
	and (ST.ststc <> 'DG'
	-- Downgrades cuya membresia de referencia esta dentro del periodo
	or (ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)))
	-- No canceladas o canceladas fuera del periodo
	and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
	-- Lead Source
	and S.sals in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or G.gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
group by G.guGuestRef, G.guag

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMemberTypeAgencyShows]    Script Date: 08/19/2016 10:30:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por tipo de socio y agencia
** 
** [wtorres]	26/Ene/2012 Created
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetMemberTypeAgencyShows](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000),			-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',		-- Claves de mercados
	@Agencies varchar(max) = 'ALL',		-- Claves de agencias
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@ConsiderDirectsAntesInOut bit = 0,	-- Indica si se debe considerar directas Antes In & Out
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	MemberType varchar(12),
	Agency varchar(35),
	Shows int
)
as
begin

insert @Table
select
	guGuestRef,
	guag,
	Sum(case when @ConsiderQuinellas = 0 then 1 else guShowsQty end)
from Guests
where
	-- Fecha de show
	((@BasedOnArrival = 0 and guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Con show
	and guShow = 1))
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
	-- No quinielas split
	and (@ConsiderQuinellas = 0 or guQuinellaSplit = 0)
	-- No Directas no Antes In & Out
	and (@ConsiderDirectsAntesInOut = 0 or not (guDirect = 1 and guAntesIO = 0))
group by guGuestRef, guag

return
end


GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMonthAgencyLeadSourceArrivals]    Script Date: 08/19/2016 10:30:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de llegadas por mes, agencia y Lead Source
** 
** [wtorres]	22/Oct/2009 Created
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetMonthAgencyLeadSourceArrivals](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@Agencies varchar(max) = 'ALL'	-- Claves de agencias
)
returns @Table table (
	[Year] int,
	[Month] int,
	Agency varchar(35),
	LeadSource varchar(10),
	Arrivals int
)
as
begin

insert @Table
select
	Year(G.guCheckInD),
	Month(G.guCheckInD),
	G.guag,
	G.guls,
	Count(*)
from Guests G
	left join LeadSources L on G.guls = L.lsID
where
	-- Fecha de llegada
	G.guCheckInD between @DateFrom and @DateTo
	-- Con Check In
	and G.guCheckIn = 1
	-- No Rebook
	and G.guRef is null
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Agencias
	and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
	-- Programa
	and L.lspg = 'IH'
	-- Originalmente disponibles
	and (G.guOriginAvail = 1 or G.guInvit = 1)
group by Year(G.guCheckInD), Month(G.guCheckInD), G.guag, G.guls

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMonthAgencyLeadSourceAvailables]    Script Date: 08/19/2016 10:30:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de disponibles por mes, agencia y Lead Source
** 
** [wtorres]	22/Oct/2009 Created
** [wtorres]	12/Ago/2010 Modified. Agregue el parametro @ConsiderQuinellas
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetMonthAgencyLeadSourceAvailables](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@ConsiderQuinellas bit = 0		-- Indica si se debe considerar quinielas
)
returns @Table table (
	[Year] int,
	[Month] int,
	Agency varchar(35),
	LeadSource varchar(10),
	Availables int
)
as
begin

insert @Table
select
	Year(G.guCheckInD),
	Month(G.guCheckInD),
	G.guag,
	G.guls,
	Sum(case when @ConsiderQuinellas = 0 or G.guShow = 0 then 1 else G.guShowsQty end)
from Guests G
	left join LeadSources L on G.guls = L.lsID
where
	-- Fecha de llegada
	G.guCheckInD between @DateFrom and @DateTo
	-- Con Check In
	and G.guCheckIn = 1
	-- Disponible
	and G.guAvail = 1
	-- No Rebook
	and G.guRef is null
	-- Contactado
	and G.guInfo = 1
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Agencias
	and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
	-- Programa
	and L.lspg = 'IH'
	-- Originalmente disponibles
	and (G.guOriginAvail = 1 or G.guInvit = 1)
group by Year(G.guCheckInD), Month(G.guCheckInD), G.guag, G.guls

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMonthAgencyLeadSourceBookings]    Script Date: 08/19/2016 10:30:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMonthAgencyLeadSourceContacts]    Script Date: 08/19/2016 10:30:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de contactos por mes, agencia y Lead Source
** 
** [wtorres]	22/Oct/2009 Created
** [wtorres]	24/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [wtorres]	20/Oct/2011 Modified. Ahora no se cuentan los rebooks
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetMonthAgencyLeadSourceContacts](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	[Year] int,
	[Month] int,
	Agency varchar(35),
	LeadSource varchar(10),
	Contacts int
)
as
begin

-- si no se debe basar en la fecha de llegada
if @BasedOnArrival = 0

	insert @Table
	select
		Year(G.guInfoD),
		Month(G.guInfoD),
		G.guag,
		G.guls,
		Count(*)
	from Guests G
		left join LeadSources L on G.guls = L.lsID
	where
		-- Fecha de contacto
		G.guInfoD between @DateFrom and @DateTo
		-- No Rebook
		and guRef is null
		-- No Antes In & Out
		and G.guAntesIO = 0
		-- Agencias
		and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
		-- Programa
		and L.lspg = 'IH'
		-- Originalmente disponibles
		and (G.guOriginAvail = 1 or G.guInvit = 1)
	group by Year(G.guInfoD), Month(G.guInfoD), G.guag, G.guls

-- si se debe basar en la fecha de llegada
else

	insert @Table
	select
		Year(G.guCheckInD),
		Month(G.guCheckInD),
		G.guag,
		G.guls,
		Count(*)
	from Guests G
		left join LeadSources L on G.guls = L.lsID
	where
		-- Fecha de llegada
		G.guCheckInD between @DateFrom and @DateTo
		-- Contactado
		and G.guInfo = 1
		-- No Rebook
		and guRef is null
		-- No Antes In & Out
		and G.guAntesIO = 0
		-- Agencias
		and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
		-- Programa
		and L.lspg = 'IH'
		-- Originalmente disponibles
		and (G.guOriginAvail = 1 or G.guInvit = 1)
	group by Year(G.guCheckInD), Month(G.guCheckInD), G.guag, G.guls

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMonthAgencyLeadSourceSales]    Script Date: 08/19/2016 10:30:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas por mes, agencia y Lead Source
** 
** [wtorres]	22/Oct/2009 Created
** [wtorres]	24/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [wtorres]	19/Nov/2013 Modified. Ahora los downgrades se cuentan en base a la categoria de tipo de venta
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetMonthAgencyLeadSourceSales](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	[Year] int,
	[Month] int,
	Agency varchar(35),
	LeadSource varchar(10),
	Sales int
)
as
begin

-- si no se debe basar en la fecha de llegada
if @BasedOnArrival = 0

	insert @Table
	select
		Year(S.saProcD),
		Month(S.saProcD),
		G.guag,
		S.sals,
		Count(*)
	from Sales S
		left join SaleTypes ST on ST.stID = S.sast
		left join Guests G on S.sagu = G.guID
		left join LeadSources L on S.sals = L.lsID
	where
		-- Fecha de procesable
		S.saProcD between @DateFrom and @DateTo
		-- No downgrades
		and ST.ststc <> 'DG'
		-- No canceladas o canceladas fuera del periodo
		and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
		-- Agencias
		and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
		-- Programa
		and L.lspg = 'IH'
		-- Originalmente disponibles
		and (G.guOriginAvail = 1 or G.guInvit = 1)
	group by Year(S.saProcD), Month(S.saProcD), G.guag, S.sals

-- si se debe basar en la fecha de llegada
else

	insert @Table

	-- Ventas con huesped
	select
		Year(G.guCheckInD),
		Month(G.guCheckInD),
		G.guag,
		S.sals,
		Count(*)
	from Sales S
		left join SaleTypes ST on ST.stID = S.sast
		left join Guests G on S.sagu = G.guID
		left join LeadSources L on S.sals = L.lsID
	where
		-- Con huesped
		S.sagu is not null
		-- Fecha de llegada
		and G.guCheckInD between @DateFrom and @DateTo
		-- Procesable
		and S.saProc = 1
		-- No downgrades
		and ST.ststc <> 'DG'
		-- No canceladas o canceladas fuera del periodo
		and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
		-- Agencias
		and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
		-- Programa
		and L.lspg = 'IH'
		-- Originalmente disponibles
		and (G.guOriginAvail = 1 or G.guInvit = 1)
	group by Year(G.guCheckInD), Month(G.guCheckInD), G.guag, S.sals

	-- Ventas sin huesped
	union all
	select
		Year(S.saProcD),
		Month(S.saProcD),
		G.guag,
		S.sals,
		Count(*)
	from Sales S
		left join SaleTypes ST on ST.stID = S.sast
		left join Guests G on S.sagu = G.guID
		left join LeadSources L on S.sals = L.lsID
	where
		-- Sin huesped
		S.sagu is null
		-- Fecha de procesable
		and S.saProcD between @DateFrom and @DateTo
		-- No downgrades
		and ST.ststc <> 'DG'
		-- No canceladas o canceladas fuera del periodo
		and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
		-- Agencias
		and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
		-- Programa
		and L.lspg = 'IH'
		-- Originalmente disponibles
		and (G.guOriginAvail = 1 or G.guInvit = 1)
	group by Year(S.saProcD), Month(S.saProcD), G.guag, S.sals

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMonthAgencyLeadSourceSalesAmount]    Script Date: 08/19/2016 10:30:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de ventas por mes, agencia y Lead Source
** 
** [wtorres]	22/Oct/2009 Created
** [wtorres]	23/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [wtorres]	19/Nov/2013 Modified. Ahora los downgrades se cuentan en base a la categoria de tipo de venta
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetMonthAgencyLeadSourceSalesAmount](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	[Year] int,
	[Month] int,
	Agency varchar(35),
	LeadSource varchar(10),
	SalesAmount money
)
as
begin

-- si no se debe basar en la fecha de llegada
if @BasedOnArrival = 0

	insert @Table
	select
		Year(S.saProcD),
		Month(S.saProcD),
		G.guag,
		S.sals,
		Sum(saGrossAmount)
	from Sales S
		left join SaleTypes ST on ST.stID = S.sast
		left join Guests G on S.sagu = G.guID
		left join LeadSources L on S.sals = L.lsID
	where
		-- Fecha de procesable
		S.saProcD between @DateFrom and @DateTo
		-- No downgrades
		and (ST.ststc <> 'DG'
		-- Downgrades cuya membresia de referencia esta dentro del periodo
		or (ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo))
		-- No canceladas o canceladas fuera del periodo
		and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
		-- Agencias
		and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
		-- Programa
		and L.lspg = 'IH'
		-- Originalmente disponibles
		and (G.guOriginAvail = 1 or G.guInvit = 1)
	group by Year(S.saProcD), Month(S.saProcD), G.guag, S.sals

-- si se debe basar en la fecha de llegada
else

	insert @Table

	-- Ventas con huesped
	select
		Year(G.guCheckInD),
		Month(G.guCheckInD),
		G.guag,
		S.sals,
		Sum(saGrossAmount)
	from Sales S
		left join SaleTypes ST on ST.stID = S.sast
		left join Guests G on S.sagu = G.guID
		left join LeadSources L on S.sals = L.lsID
	where
		-- Con huesped
		S.sagu is not null
		-- Fecha de llegada
		and G.guCheckInD between @DateFrom and @DateTo
		-- Procesable
		and S.saProc = 1
		-- No downgrades
		and (ST.ststc <> 'DG'
		-- Downgrades cuya membresia de referencia esta dentro del periodo
		or (ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo))
		-- No canceladas o canceladas fuera del periodo
		and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
		-- Agencias
		and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
		-- Programa
		and L.lspg = 'IH'
		-- Originalmente disponibles
		and (G.guOriginAvail = 1 or G.guInvit = 1)
	group by Year(G.guCheckInD), Month(G.guCheckInD), G.guag, S.sals

	-- Ventas sin huesped
	union all
	select
		Year(S.saProcD),
		Month(S.saProcD),
		G.guag,
		S.sals,
		Sum(saGrossAmount)
	from Sales S
		left join SaleTypes ST on ST.stID = S.sast
		left join Guests G on S.sagu = G.guID
		left join LeadSources L on S.sals = L.lsID
	where
		-- Sin huesped
		S.sagu is null
		-- Fecha de procesable
		and S.saProcD between @DateFrom and @DateTo
		-- No downgrades
		and (ST.ststc <> 'DG'
		-- Downgrades cuya membresia de referencia esta dentro del periodo
		or (ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo))
		-- No canceladas o canceladas fuera del periodo
		and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
		-- Agencias
		and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
		-- Programa
		and L.lspg = 'IH'
		-- Originalmente disponibles
		and (G.guOriginAvail = 1 or G.guInvit = 1)
	group by Year(S.saProcD), Month(S.saProcD), G.guag, S.sals

return
end
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetMonthAgencyLeadSourceShows]    Script Date: 08/19/2016 10:30:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por mes, agencia y Lead Source
** 
** [wtorres]	22/Oct/2009 Created
** [wtorres]	14/May/2010 Modified. Agregue el parametro @ConsiderDirectsAntesInOut
** [wtorres]	24/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [wtorres]	20/Dic/2013 Modified. Agregue el parametro @ConsiderQuinellas
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetMonthAgencyLeadSourceShows](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@Agencies varchar(max) = 'ALL',		-- Claves de agencias
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@ConsiderDirectsAntesInOut bit = 0,	-- Indica si se debe considerar directas Antes In & Out
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	[Year] int,
	[Month] int,
	Agency varchar(35),
	LeadSource varchar(10),
	Shows int
)
as
begin

-- si no se debe basar en la fecha de llegada
if @BasedOnArrival = 0

	insert @Table
	select
		Year(G.guShowD),
		Month(G.guShowD),
		G.guag,
		G.guls,
		Sum(case when @ConsiderQuinellas = 0 then 1 else G.guShowsQty end)
	from Guests G
		left join LeadSources L on G.guls = L.lsID
	where
		-- Fecha de show
		G.guShowD between @DateFrom and @DateTo
		-- No Quiniela Split
		and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
		-- Agencias
		and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
		-- Programa
		and L.lspg = 'IH'
		-- Originalmente disponibles
		and (G.guOriginAvail = 1 or G.guInvit = 1)
		-- No Directas no Antes In & Out
		and (@ConsiderDirectsAntesInOut = 0 or not (guDirect = 1 and guAntesIO = 0))
	group by Year(G.guShowD), Month(G.guShowD), G.guag, G.guls

-- si se debe basar en la fecha de llegada
else

	insert @Table
	select
		Year(G.guCheckInD),
		Month(G.guCheckInD),
		G.guag,
		G.guls,
		Sum(case when @ConsiderQuinellas = 0 then 1 else G.guShowsQty end)
	from Guests G
		left join LeadSources L on G.guls = L.lsID
	where
		-- Fecha de llegada
		G.guCheckInD between @DateFrom and @DateTo
		-- Con show
		and G.guShow = 1
		-- No Quiniela Split
		and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
		-- Agencias
		and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
		-- Programa
		and L.lspg = 'IH'
		-- Originalmente disponibles
		and (G.guOriginAvail = 1 or G.guInvit = 1)
		-- No Directas no Antes In & Out
		and (@ConsiderDirectsAntesInOut = 0 or not (guDirect = 1 and guAntesIO = 0))
	group by Year(G.guCheckInD), Month(G.guCheckInD), G.guag, G.guls

return
end
GO


