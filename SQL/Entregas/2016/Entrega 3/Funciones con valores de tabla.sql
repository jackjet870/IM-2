USE [OrigosVCPalace]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetFlightSalesRoomBookings]    Script Date: 09/10/2016 14:18:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetFlightSalesRoomBookings]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetFlightSalesRoomBookings]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetFlightSalesRoomSales]    Script Date: 09/10/2016 14:18:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetFlightSalesRoomSales]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetFlightSalesRoomSales]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetFlightSalesRoomSalesAmount]    Script Date: 09/10/2016 14:18:46 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetFlightSalesRoomSalesAmount]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetFlightSalesRoomSalesAmount]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetFlightSalesRoomShows]    Script Date: 09/10/2016 14:18:46 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetFlightSalesRoomShows]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetFlightSalesRoomShows]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelBookings]    Script Date: 09/10/2016 14:18:47 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetHotelBookings]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetHotelBookings]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelSales]    Script Date: 09/10/2016 14:18:48 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetHotelSales]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetHotelSales]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelSalesAmount]    Script Date: 09/10/2016 14:18:49 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetHotelSalesAmount]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetHotelSalesAmount]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelSalesRoomBookings]    Script Date: 09/10/2016 14:18:49 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetHotelSalesRoomBookings]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetHotelSalesRoomBookings]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelSalesRoomSales]    Script Date: 09/10/2016 14:18:50 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetHotelSalesRoomSales]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetHotelSalesRoomSales]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelSalesRoomSalesAmount]    Script Date: 09/10/2016 14:18:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetHotelSalesRoomSalesAmount]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetHotelSalesRoomSalesAmount]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelSalesRoomShows]    Script Date: 09/10/2016 14:18:52 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetHotelSalesRoomShows]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetHotelSalesRoomShows]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelShows]    Script Date: 09/10/2016 14:18:52 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetHotelShows]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetHotelShows]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveBookings]    Script Date: 09/10/2016 14:18:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetWaveBookings]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetWaveBookings]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveSales]    Script Date: 09/10/2016 14:18:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetWaveSales]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetWaveSales]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveSalesAmount]    Script Date: 09/10/2016 14:18:55 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetWaveSalesAmount]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetWaveSalesAmount]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveSalesRoomBookings]    Script Date: 09/10/2016 14:18:56 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetWaveSalesRoomBookings]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetWaveSalesRoomBookings]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveSalesRoomSales]    Script Date: 09/10/2016 14:18:56 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetWaveSalesRoomSales]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetWaveSalesRoomSales]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveSalesRoomSalesAmount]    Script Date: 09/10/2016 14:18:57 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetWaveSalesRoomSalesAmount]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetWaveSalesRoomSalesAmount]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveSalesRoomShows]    Script Date: 09/10/2016 14:18:58 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetWaveSalesRoomShows]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetWaveSalesRoomShows]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveShows]    Script Date: 09/10/2016 14:18:58 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetWaveShows]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetWaveShows]
GO

USE [OrigosVCPalace]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetFlightSalesRoomBookings]    Script Date: 09/10/2016 14:18:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de bookings por vuelo y sala
** 
** [VKU] 13/May/2016 Creado
**
*/
CREATE function [dbo].[UFN_IM_GetFlightSalesRoomBookings](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@FilterDeposit tinyint = 0,			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
										--		3. Con deposito y shows sin deposito (Deposits & Flyers Show)
	@InOut int = -1						-- Filtro de In & Outs:
										--		-1. Sin filtro
										--		 0. No In & Outs
										--		 1. In & Outs
)
returns @Table table (
	Flight varchar(10),
	SalesRoom varchar(10),
	Books int
)
as
begin

insert @Table
select
	G.guFlight,
	G.gusr,
	Count(*)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- Fecha de booking
	G.guBookD between @DateFrom and @DateTo
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ',')))
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos y fechas de reservacion y show
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0)
		or (@FilterDeposit = 3 and (G.guDeposit > 0 or (G.guShow = 1 and G.guDeposit = 0))))
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
	-- No bookings cancelados
	and G.guBookCanc = 0
group by G.guFlight, G.gusr

return
end














GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetFlightSalesRoomSales]    Script Date: 09/10/2016 14:19:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas por vuelo y sala
** 
**	[VKU] 13/May/2016 Creado
**	[VKU] 17/May/2016 Modified. Correccion de error. No estaba contando las ventas pendientes 
** 
*/
CREATE function [dbo].[UFN_IM_GetFlightSalesRoomSales](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@ConsiderOutOfPending bit = 0,		-- Indica si se debe considerar Out Of Pending
	@ConsiderCancel bit = 0,				-- Indica si se debe considerar canceladas
	@FilterDeposit tinyint = 0,			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
	@ConsiderPending bit = 0		-- Indica si se debe considerar pendientes

)
returns @Table table (
	Flight varchar(10),
	SalesRoom varchar(10),
	Sales int
)
as
begin

insert @Table
select
	G.guFlight,
	S.sasr,
	Count(*)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
	inner join LeadSources L on L.lsID = S.sals
where

	
	-- Fecha de procesable
	((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo)))))
	
	
	-- No downgrades
	and ST.ststc <> 'DG' 
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ',')))
	-- PRs
	and (@PRs = 'ALL' or (S.saPR1 in (select item from split(@PRs, ','))
		or S.saPR2 in (select item from split(@PRs, ','))
		or S.saPR3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program) 
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
group by G.guFlight, S.sasr

return
end








GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetFlightSalesRoomSalesAmount]    Script Date: 09/10/2016 14:19:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de ventas por vuelo y sala
** 
** [VKU] 13/May/2016 Creado
** [VKU] 17/May/2016 Modified. Correccion de error. No estaba contando las ventas pendientes 	
**
*/
CREATE function [dbo].[UFN_IM_GetFlightSalesRoomSalesAmount](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@ConsiderOutOfPending bit = 0,		-- Indica si se debe considerar Out Of Pending
	@ConsiderCancel bit = 0,			-- Indica si se debe considerar canceladas
	@FilterDeposit tinyint = 0,			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
	@ConsiderPending bit = 0		-- Indica si se debe considerar pendientes
)
returns @Table table (
	Flight varchar(10),
	SalesRoom varchar(10),
	SalesAmount money
)
as
begin

insert @Table
select
	G.guFlight,
	S.sasr,
	Sum(S.saGrossAmount)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
	inner join LeadSources L on L.lsID = S.sals
where

		-- Fecha de procesable
	((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo)))))
	
	and
	
	-- Procesables no downgrades
	((ST.ststc <> 'DG')
	-- Procesables downgrades cuya membresia de referencia esta dentro del periodo
	or (@ConsiderCancel = 0 and S.saProcD between @DateFrom and @DateTo and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo))
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ',')))
	-- PRs
	and (@PRs = 'ALL' or (S.saPR1 in (select item from split(@PRs, ','))
		or S.saPR2 in (select item from split(@PRs, ','))
		or S.saPR3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program) 
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
group by G.guFlight, S.sasr

return
end






GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetFlightSalesRoomShows]    Script Date: 09/10/2016 14:19:11 ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por vuelo y sala
** 
**	[VKU] 13/May/2016 Creado
**
*/

CREATE function [dbo].[UFN_IM_GetFlightSalesRoomShows](

	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@FilterDeposit tinyint = 0,			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
	@InOut int = -1						-- Filtro de In & Outs:
										--		-1. Sin filtro
										--		 0. No In & Outs
										--		 1. In & Outs
)
returns @Table table (
	Flight varchar(10),
	SalesRoom varchar(10),
	Shows int
)

as
begin

insert @Table
select
	G.guFlight,
	G.gusr,
	Count(*)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- Fecha de show
	G.guShowD between @DateFrom and @DateTo
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
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
group by G.guFlight, G.gusr

return
end





GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelBookings]    Script Date: 09/10/2016 14:19:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de bookings por hotel
** 
**	[VKU] 13/May/2016 Creado
**
*/
CREATE function [dbo].[UFN_IM_GetHotelBookings](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@FilterDeposit tinyint = 0,			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
										--		3. Con deposito y shows sin deposito (Deposits & Flyers Show)
	@InOut int = -1						-- Filtro de In & Outs:
										--		-1. Sin filtro
										--		 0. No In & Outs
										--		 1. In & Outs
)
returns @Table table (
	Hotel varchar(30),
	Books int
)
as
begin

insert @Table
select
	G.guHotel,
	Count(*)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- Fecha de booking
	G.guBookD between @DateFrom and @DateTo
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ',')))
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos y fechas de reservacion y show
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0)
		or (@FilterDeposit = 3 and (G.guDeposit > 0 or (G.guShow = 1 and G.guDeposit = 0))))
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
	-- No bookings cancelados
	and G.guBookCanc = 0
group by G.guHotel

return
end















GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelSales]    Script Date: 09/10/2016 14:19:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas por hotel
** 
**	[VKU] 13/May/2016 Creado
**	[VKU] 18/May/2016 Modified. Correccion de error. No estaba contando las ventas pendientes 
**
*/
CREATE function [dbo].[UFN_IM_GetHotelSales](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@ConsiderOutOfPending bit = 0,		-- Indica si se debe considerar Out Of Pending
	@ConsiderCancel bit = 0,				-- Indica si se debe considerar canceladas
	@FilterDeposit tinyint = 0,			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
	@ConsiderPending bit = 0		-- Indica si se debe considerar pendientes

)
returns @Table table (
	Hotel varchar(30),
	Sales int
)
as
begin

insert @Table
select
	G.guHotel,
	Count(*)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
	inner join LeadSources L on L.lsID = S.sals
where
	---- Fecha de procesable
	--S.saProcD between @DateFrom and @DateTo
	
		-- Fecha de procesable
	((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo)))))
	
	
	-- No downgrades
	and ST.ststc <> 'DG'
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ',')))
	-- PRs
	and (@PRs = 'ALL' or (S.saPR1 in (select item from split(@PRs, ','))
		or S.saPR2 in (select item from split(@PRs, ','))
		or S.saPR3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program) 
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
group by G.guHotel
return
end









GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelSalesAmount]    Script Date: 09/10/2016 14:19:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de ventas por hotel
** 
** [VKU] 13/May/2016 Creado
** [VKU] 18/May/2016 Modified. Correccion de error. No estaba contando las ventas pendientes 
**
*/
CREATE function [dbo].[UFN_IM_GetHotelSalesAmount](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@ConsiderOutOfPending bit = 0,		-- Indica si se debe considerar Out Of Pending
	@ConsiderCancel bit = 0,			-- Indica si se debe considerar canceladas
	@FilterDeposit tinyint = 0,			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
	@ConsiderPending bit = 0		-- Indica si se debe considerar pendientes

)
returns @Table table (
	Hotel varchar(30),
	SalesAmount money
)
as
begin

insert @Table
select
	G.guHotel,
	Sum(S.saGrossAmount)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
	inner join LeadSources L on L.lsID = S.sals
where

	-- Fecha de procesable
	((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo)))))
	
	and

	-- Procesables no downgrades
	((ST.ststc <> 'DG')
	-- Procesables downgrades cuya membresia de referencia esta dentro del periodo
	or (@ConsiderCancel = 0 and S.saProcD between @DateFrom and @DateTo and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo))
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ',')))
	-- PRs
	and (@PRs = 'ALL' or (S.saPR1 in (select item from split(@PRs, ','))
		or S.saPR2 in (select item from split(@PRs, ','))
		or S.saPR3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program) 
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
group by G.guHotel

return
end







GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelSalesRoomBookings]    Script Date: 09/10/2016 14:19:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de bookings por hotel y sala
** 
**	[VKU] 13/May/2016 Creado
**
*/
CREATE function [dbo].[UFN_IM_GetHotelSalesRoomBookings](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@FilterDeposit tinyint = 0,			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
										--		3. Con deposito y shows sin deposito (Deposits & Flyers Show)
	@InOut int = -1						-- Filtro de In & Outs:
										--		-1. Sin filtro
										--		 0. No In & Outs
										--		 1. In & Outs
)
returns @Table table (
	Hotel varchar(30),
	SalesRoom varchar(10),
	Books int
)
as
begin

insert @Table
select
	G.guHotel,
	G.gusr,
	Count(*)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- Fecha de booking
	G.guBookD between @DateFrom and @DateTo
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ',')))
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos y fechas de reservacion y show
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0)
		or (@FilterDeposit = 3 and (G.guDeposit > 0 or (G.guShow = 1 and G.guDeposit = 0))))
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
	-- No bookings cancelados
	and G.guBookCanc = 0
group by G.guHotel, G.gusr

return
end
















GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelSalesRoomSales]    Script Date: 09/10/2016 14:19:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas por hotel y sala
** 
**	[VKU] 13/May/2016 Creado
**	[VKU] 18/May/2016 Modified. Correccion de error. No estaba contando las ventas pendientes
**
*/
CREATE function [dbo].[UFN_IM_GetHotelSalesRoomSales](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@ConsiderOutOfPending bit = 0,		-- Indica si se debe considerar Out Of Pending
	@ConsiderCancel bit = 0,				-- Indica si se debe considerar canceladas
	@FilterDeposit tinyint = 0,			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
	@ConsiderPending bit = 0		-- Indica si se debe considerar pendientes

)
returns @Table table (
	Hotel varchar(30),
	SalesRoom varchar(10),
	Sales int
)
as
begin

insert @Table
select
	G.guHotel,
	S.sasr,
	Count(*)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
	inner join LeadSources L on L.lsID = S.sals
where
	---- Fecha de procesable
	--S.saProcD between @DateFrom and @DateTo
	
	
		-- Fecha de procesable
	((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo)))))
	
	
	-- No downgrades
	and ST.ststc <> 'DG'
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ',')))
	-- PRs
	and (@PRs = 'ALL' or (S.saPR1 in (select item from split(@PRs, ','))
		or S.saPR2 in (select item from split(@PRs, ','))
		or S.saPR3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program) 
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
group by G.guHotel, S.sasr
return
end










GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelSalesRoomSalesAmount]    Script Date: 09/10/2016 14:19:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de ventas por hotel y sala
** 
** [VKU] 13/May/2016 Creado
** [VKU] 18/May/2016 Modified. Correccion de error. No estaba contando las ventas pendientes 
**
*/
CREATE function [dbo].[UFN_IM_GetHotelSalesRoomSalesAmount](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@ConsiderOutOfPending bit = 0,		-- Indica si se debe considerar Out Of Pending
	@ConsiderCancel bit = 0,			-- Indica si se debe considerar canceladas
	@FilterDeposit tinyint = 0,			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
	@ConsiderPending bit = 0		-- Indica si se debe considerar pendientes
)
returns @Table table (
	Hotel varchar(30),
	SalesRoom varchar(10),
	SalesAmount money
)
as
begin

insert @Table
select
	G.guHotel,
	S.sasr,
	Sum(S.saGrossAmount)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
	inner join LeadSources L on L.lsID = S.sals
where

	-- Fecha de procesable
	((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo)))))
	
	and

	-- Procesables no downgrades
	((ST.ststc <> 'DG')
	-- Procesables downgrades cuya membresia de referencia esta dentro del periodo
	or (@ConsiderCancel = 0 and S.saProcD between @DateFrom and @DateTo and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo))
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ',')))
	-- PRs
	and (@PRs = 'ALL' or (S.saPR1 in (select item from split(@PRs, ','))
		or S.saPR2 in (select item from split(@PRs, ','))
		or S.saPR3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program) 
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
group by G.guHotel, S.sasr

return
end








GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelSalesRoomShows]    Script Date: 09/10/2016 14:19:44 ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER ON
GO



/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por hotel y sala
** 
**	[VKU] 13/May/2016 Creado
**
*/

CREATE function [dbo].[UFN_IM_GetHotelSalesRoomShows](

	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@FilterDeposit tinyint = 0,			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
	@InOut int = -1						-- Filtro de In & Outs:
										--		-1. Sin filtro
										--		 0. No In & Outs
										--		 1. In & Outs
)
returns @Table table (
	Hotel varchar(30),
	SalesRoom varchar(10),
	Shows int
)

as
begin

insert @Table
select
	G.guHotel,
	G.gusr,
	Count(*)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- Fecha de show
	G.guShowD between @DateFrom and @DateTo
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
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
group by G.guHotel, G.gusr

return
end







GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelShows]    Script Date: 09/10/2016 14:19:48 ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por hotel
** 
**	[VKU] 13/May/2016 Creado
**
*/

CREATE function [dbo].[UFN_IM_GetHotelShows](

	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@FilterDeposit tinyint = 0,			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
	@InOut int = -1						-- Filtro de In & Outs:
										--		-1. Sin filtro
										--		 0. No In & Outs
										--		 1. In & Outs
)
returns @Table table (
	Hotel varchar(30),
	Shows int
)

as
begin

insert @Table
select
	G.guHotel,
	Count(*)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- Fecha de show
	G.guShowD between @DateFrom and @DateTo
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
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
group by G.guHotel

return
end






GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveBookings]    Script Date: 09/10/2016 14:19:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de bookings por horario
** 
**	[VKU] 14/May/2016 Creado
**
*/
CREATE function [dbo].[UFN_IM_GetWaveBookings](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@FilterDeposit tinyint = 0,			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
										--		3. Con deposito y shows sin deposito (Deposits & Flyers Show)
	@InOut int = -1						-- Filtro de In & Outs:
										--		-1. Sin filtro
										--		 0. No In & Outs
										--		 1. In & Outs
)
returns @Table table (
	BookTime datetime,
	Books int
)
as
begin

insert @Table
select
	G.guBookT,
	Count(*)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- Fecha de booking
	G.guBookD between @DateFrom and @DateTo
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ',')))
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos y fechas de reservacion y show
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0)
		or (@FilterDeposit = 3 and (G.guDeposit > 0 or (G.guShow = 1 and G.guDeposit = 0))))
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
	-- No bookings cancelados
	and G.guBookCanc = 0
group by G.guBookT
return
end
















GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveSales]    Script Date: 09/10/2016 14:19:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas por horario
** 
**	[VKU] 14/May/2016 Creado
**	[VKU] 18/May/2016 Modified. Correccion de error. No estaba contando las ventas pendientes 
**
*/
CREATE function [dbo].[UFN_IM_GetWaveSales](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@ConsiderOutOfPending bit = 0,		-- Indica si se debe considerar Out Of Pending
	@ConsiderCancel bit = 0,				-- Indica si se debe considerar canceladas
	@FilterDeposit tinyint = 0,			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
	@ConsiderPending bit = 0		-- Indica si se debe considerar pendientes

)
returns @Table table (
	BookTime datetime,
	Sales int
)
as
begin

insert @Table
select
	G.guBookT,
	Count(*)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
	inner join LeadSources L on L.lsID = S.sals
where
	---- Fecha de procesable
	--S.saProcD between @DateFrom and @DateTo
	
	
		-- Fecha de procesable
	((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo)))))
	
	
	-- No downgrades
	and ST.ststc <> 'DG'
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ',')))
	-- PRs
	and (@PRs = 'ALL' or (S.saPR1 in (select item from split(@PRs, ','))
		or S.saPR2 in (select item from split(@PRs, ','))
		or S.saPR3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program) 
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
group by G.guBookT
return
end










GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveSalesAmount]    Script Date: 09/10/2016 14:19:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de ventas por horario
** 
** [VKU] 14/May/2016 Creado
** [VKU] 18/May/2016 Modified. Correccion de error. No estaba contando las ventas pendientes 
**
*/
CREATE function [dbo].[UFN_IM_GetWaveSalesAmount](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@ConsiderOutOfPending bit = 0,		-- Indica si se debe considerar Out Of Pending
	@ConsiderCancel bit = 0,			-- Indica si se debe considerar canceladas
	@FilterDeposit tinyint = 0,			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
	@ConsiderPending bit = 0		-- Indica si se debe considerar pendientes

)
returns @Table table (
	BookTime datetime,
	SalesAmount money
)
as
begin

insert @Table
select
	G.guBookT,
	Sum(S.saGrossAmount)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
	inner join LeadSources L on L.lsID = S.sals
where
	-- Fecha de procesable
	((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo)))))
	
	and

	-- Procesables no downgrades
	((ST.ststc <> 'DG')
	-- Procesables downgrades cuya membresia de referencia esta dentro del periodo
	or (@ConsiderCancel = 0 and S.saProcD between @DateFrom and @DateTo and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo))
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ',')))
	-- PRs
	and (@PRs = 'ALL' or (S.saPR1 in (select item from split(@PRs, ','))
		or S.saPR2 in (select item from split(@PRs, ','))
		or S.saPR3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program) 
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
group by G.guBookT

return
end








GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveSalesRoomBookings]    Script Date: 09/10/2016 14:20:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de bookings por horario y sala
** 
**	[VKU] 16/May/2016 Creado
**
*/
CREATE function [dbo].[UFN_IM_GetWaveSalesRoomBookings](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@FilterDeposit tinyint = 0,			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
										--		3. Con deposito y shows sin deposito (Deposits & Flyers Show)
	@InOut int = -1						-- Filtro de In & Outs:
										--		-1. Sin filtro
										--		 0. No In & Outs
										--		 1. In & Outs
)
returns @Table table (
	BookTime datetime,
	SalesRoom varchar(10),
	Books int
)
as
begin

insert @Table
select
	G.guBookT,
	G.gusr,
	Count(*)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- Fecha de booking
	G.guBookD between @DateFrom and @DateTo
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ',')))
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos y fechas de reservacion y show
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0)
		or (@FilterDeposit = 3 and (G.guDeposit > 0 or (G.guShow = 1 and G.guDeposit = 0))))
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
	-- No bookings cancelados
	and G.guBookCanc = 0
group by G.guBookT, G.gusr

return
end

















GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveSalesRoomSales]    Script Date: 09/10/2016 14:20:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas por horario y sala
** 
**	[VKU] 16/May/2016 Creado
**	[VKU] 18/May/2016 Modified. Correccion de error. No estaba contando las ventas pendientes 
**
*/
CREATE function [dbo].[UFN_IM_GetWaveSalesRoomSales](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@ConsiderOutOfPending bit = 0,		-- Indica si se debe considerar Out Of Pending
	@ConsiderCancel bit = 0,				-- Indica si se debe considerar canceladas
	@FilterDeposit tinyint = 0,			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
	@ConsiderPending bit = 0		-- Indica si se debe considerar pendientes

)
returns @Table table (
	BookTime datetime,
	SalesRoom varchar(10),
	Sales int
)
as
begin

insert @Table
select
	G.guBookT,
	S.sasr,
	Count(*)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
	inner join LeadSources L on L.lsID = S.sals
where
	---- Fecha de procesable
	--S.saProcD between @DateFrom and @DateTo
	
	
		-- Fecha de procesable
	((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo)))))
	
	
	-- No downgrades
	and ST.ststc <> 'DG'
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ',')))
	-- PRs
	and (@PRs = 'ALL' or (S.saPR1 in (select item from split(@PRs, ','))
		or S.saPR2 in (select item from split(@PRs, ','))
		or S.saPR3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program) 
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
group by G.guBookT, S.sasr
return
end











GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveSalesRoomSalesAmount]    Script Date: 09/10/2016 14:20:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de ventas por horario y sala
** 
** [VKU] 16/May/2016 Creado
** [VKU] 18/May/2016 Modified. Correccion de error. No estaba contando las ventas pendientes 
**
*/
CREATE function [dbo].[UFN_IM_GetWaveSalesRoomSalesAmount](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@ConsiderOutOfPending bit = 0,		-- Indica si se debe considerar Out Of Pending
	@ConsiderCancel bit = 0,			-- Indica si se debe considerar canceladas
	@FilterDeposit tinyint = 0,			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
	@ConsiderPending bit = 0		-- Indica si se debe considerar pendientes
)
returns @Table table (
	BookTime datetime,
	SalesRoom varchar(10),
	SalesAmount money
)
as
begin

insert @Table
select
	G.guBookT,
	S.sasr,
	Sum(S.saGrossAmount)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
	inner join LeadSources L on L.lsID = S.sals
where

	-- Fecha de procesable
	((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo)))))
	
	and

	-- Procesables no downgrades
	((ST.ststc <> 'DG')
	-- Procesables downgrades cuya membresia de referencia esta dentro del periodo
	or (@ConsiderCancel = 0 and S.saProcD between @DateFrom and @DateTo and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo))
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ',')))
	-- PRs
	and (@PRs = 'ALL' or (S.saPR1 in (select item from split(@PRs, ','))
		or S.saPR2 in (select item from split(@PRs, ','))
		or S.saPR3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program) 
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0))
group by G.guBookT, S.sasr

return
end









GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveSalesRoomShows]    Script Date: 09/10/2016 14:20:15 ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER ON
GO




/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por horario y sala
** 
**	[VKU] 16/May/2016 Creado
**
*/

CREATE function [dbo].[UFN_IM_GetWaveSalesRoomShows](

	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@FilterDeposit tinyint = 0,			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
	@InOut int = -1						-- Filtro de In & Outs:
										--		-1. Sin filtro
										--		 0. No In & Outs
										--		 1. In & Outs
)
returns @Table table (
	BookTime datetime,
	SalesRoom varchar(10),
	Shows int
)

as
begin

insert @Table
select
	G.guBookT,
	G.gusr,
	Count(*)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- Fecha de show
	G.guShowD between @DateFrom and @DateTo
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
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
group by G.guBookT, G.gusr

return
end








GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveShows]    Script Date: 09/10/2016 14:20:19 ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER ON
GO



/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por horario
** 
**	[VKU] 14/May/2016 Creado
**
*/

CREATE function [dbo].[UFN_IM_GetWaveShows](

	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@FilterDeposit tinyint = 0,			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
	@InOut int = -1						-- Filtro de In & Outs:
										--		-1. Sin filtro
										--		 0. No In & Outs
										--		 1. In & Outs
)
returns @Table table (
	BookTime datetime,
	Shows int
)

as
begin

insert @Table
select
	G.guBookT,
	Count(*)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
where
	-- Fecha de show
	G.guShowD between @DateFrom and @DateTo
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
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
group by G.guBookT

return
end







GO


