USE [OrigosVCPalace]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetCloserSales]    Script Date: 09/21/2016 17:07:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetCloserSales]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetCloserSales]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetCloserSalesAmount]    Script Date: 09/21/2016 17:07:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetCloserSalesAmount]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetCloserSalesAmount]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetCloserShows]    Script Date: 09/21/2016 17:07:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetCloserShows]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetCloserShows]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetFlightSalesRoomBookings]    Script Date: 09/21/2016 17:07:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetFlightSalesRoomBookings]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetFlightSalesRoomBookings]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetFlightSalesRoomSales]    Script Date: 09/21/2016 17:07:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetFlightSalesRoomSales]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetFlightSalesRoomSales]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetFlightSalesRoomSalesAmount]    Script Date: 09/21/2016 17:07:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetFlightSalesRoomSalesAmount]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetFlightSalesRoomSalesAmount]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetFlightSalesRoomShows]    Script Date: 09/21/2016 17:07:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetFlightSalesRoomShows]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetFlightSalesRoomShows]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelBookings]    Script Date: 09/21/2016 17:07:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetHotelBookings]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetHotelBookings]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelSales]    Script Date: 09/21/2016 17:07:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetHotelSales]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetHotelSales]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelSalesAmount]    Script Date: 09/21/2016 17:07:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetHotelSalesAmount]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetHotelSalesAmount]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelSalesRoomBookings]    Script Date: 09/21/2016 17:07:37 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetHotelSalesRoomBookings]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetHotelSalesRoomBookings]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelSalesRoomSales]    Script Date: 09/21/2016 17:07:38 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetHotelSalesRoomSales]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetHotelSalesRoomSales]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelSalesRoomSalesAmount]    Script Date: 09/21/2016 17:07:38 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetHotelSalesRoomSalesAmount]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetHotelSalesRoomSalesAmount]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelSalesRoomShows]    Script Date: 09/21/2016 17:07:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetHotelSalesRoomShows]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetHotelSalesRoomShows]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelShows]    Script Date: 09/21/2016 17:07:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetHotelShows]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetHotelShows]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetLinerSales]    Script Date: 09/21/2016 17:07:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetLinerSales]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetLinerSales]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetLinerSalesAmount]    Script Date: 09/21/2016 17:07:41 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetLinerSalesAmount]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetLinerSalesAmount]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetLinerShows]    Script Date: 09/21/2016 17:07:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetLinerShows]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetLinerShows]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveBookings]    Script Date: 09/21/2016 17:07:43 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetWaveBookings]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetWaveBookings]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveSales]    Script Date: 09/21/2016 17:07:43 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetWaveSales]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetWaveSales]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveSalesAmount]    Script Date: 09/21/2016 17:07:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetWaveSalesAmount]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetWaveSalesAmount]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveSalesRoomBookings]    Script Date: 09/21/2016 17:07:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetWaveSalesRoomBookings]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetWaveSalesRoomBookings]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveSalesRoomSales]    Script Date: 09/21/2016 17:07:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetWaveSalesRoomSales]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetWaveSalesRoomSales]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveSalesRoomSalesAmount]    Script Date: 09/21/2016 17:07:46 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetWaveSalesRoomSalesAmount]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetWaveSalesRoomSalesAmount]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveSalesRoomShows]    Script Date: 09/21/2016 17:07:46 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetWaveSalesRoomShows]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetWaveSalesRoomShows]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveShows]    Script Date: 09/21/2016 17:07:47 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetWaveShows]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetWaveShows]
GO

USE [OrigosVCPalace]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetCloserSales]    Script Date: 09/21/2016 17:07:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas por Closer
** 
** [edgrodriguez]	07/May/2016 Created.
**
*/
CREATE function [dbo].[UFN_IM_GetCloserSales](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@SalesRooms varchar(max) = 'ALL',	-- Clave de las salas de ventas
	
	@ConsiderOutOfPending bit = 0,	-- Indica si se debe considerar Out Of Pending
	@ConsiderCancel bit = 0,		-- Indica si se debe considerar canceladas	
	@ConsiderPending bit = 0		-- Indica si se debe considerar pendientes
)
returns @Table table (
	Closer varchar(10),
	Sales money
)
as
begin

insert @Table

-- Numero de ventas (Closer 1)
-- =============================================
select
	S.saCloser1,
	Sum(dbo.UFN_OR_SecureDivision(S.saCloser1P,(S.saCloser1P + S.saCloser2P + S.saCloser3P + S.saExit1P + S.saExit2P)))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
where
	-- Closer1 1
	S.saCloser1 is not null
	
	-- Fecha de procesable
	and ((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo)))))
		
	-- No downgrades
	and ST.ststc <> 'DG'
	
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
group by S.saCloser1
UNION ALL
-- Numero de ventas (Closer 2)
-- =============================================
select
	S.saCloser2,
	Sum(dbo.UFN_OR_SecureDivision(S.saCloser2P,(S.saCloser1P + S.saCloser2P + S.saCloser3P + S.saExit1P + S.saExit2P)))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
where
	-- Closer1 1
	S.saCloser2 is not null
	
	-- Fecha de procesable
	and ((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo)))))
		
	-- No downgrades
	and ST.ststc <> 'DG'
	
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
group by S.saCloser2
UNION
-- Numero de ventas (Closer 3)
-- =============================================
select
	S.saCloser3,
	Sum(dbo.UFN_OR_SecureDivision(S.saCloser3P,(S.saCloser1P + S.saCloser2P + S.saCloser3P + S.saExit1P + S.saExit2P)))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
where
	-- Closer1 1
	S.saCloser3 is not null
	
	-- Fecha de procesable
	and ((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo)))))
		
	-- No downgrades
	and ST.ststc <> 'DG'
	
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
group by S.saCloser3
UNION ALL
-- Numero de ventas (Exit 1)
-- =============================================
select
	S.saExit1,
	Sum(dbo.UFN_OR_SecureDivision(S.saExit1P,(S.saCloser1P + S.saCloser2P + S.saCloser3P + S.saExit1P + S.saExit2P)))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
where
	-- Closer1 1
	S.saExit1 is not null
	
	-- Fecha de procesable
	and ((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo)))))
		
	-- No downgrades
	and ST.ststc <> 'DG'
	
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
group by S.saExit1
UNION ALL
-- Numero de ventas (Closer 1)
-- =============================================
select
	S.saExit2,
	Sum(dbo.UFN_OR_SecureDivision(S.saExit2P,(S.saCloser1P + S.saCloser2P + S.saCloser3P + S.saExit1P + S.saExit2P)))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
where
	-- Closer1 1
	S.saExit2 is not null
	
	-- Fecha de procesable
	and ((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo)))))
		
	-- No downgrades
	and ST.ststc <> 'DG'
	
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
group by S.saExit2
return
end


GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetCloserSalesAmount]    Script Date: 09/21/2016 17:07:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de ventas por Closer
** 
** [edgrodriguez]	07/May/2016 Created.
** [edgrodriguez]	24/Jun/2016 Modified. Se agrego el parametro @Post para filtrar puestos.
**
*/
CREATE function [dbo].[UFN_IM_GetCloserSalesAmount](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@SalesRooms varchar(max) = 'ALL',	-- Clave de las salas de ventas
	@ConsiderOutOfPending bit = 0,	-- Indica si se debe considerar Out Of Pending	
	@ConsiderCancel bit = 0,		-- Indica si se debe considerar canceladas
	@ConsiderPending bit = 0,		-- Indica si se debe considerar pendientes
	@Post varchar(max) = 'ALL',		-- Clave de puestos
	@AllowRepeated bit = 1,			-- Indica si permite vendedores repetidos en diferentes posiciones
	@Roles varchar(max) = 'ALL'					-- Indicar los roles que deben considerar
)
returns @Table table (
	Closer varchar(10),
	LastPost varchar(50),
	SalesAmount money
)
as
begin
insert @Table
-- Monto de ventas (Closer 1)
SELECT 
	S.saCloser1,
	dbo.UFN_IM_GetPersonnelPostIDByDate((CASE
	 WHEN @AllowRepeated = 1 THEN @DateTo
	 ELSE S.saD end), S.saCloser1) [LastPost],
	sum(S.saGrossAmount * (CASE 
		WHEN @Roles = 'ALL' THEN dbo.UFN_OR_GetPercentageSalesman(s.saCloser1,s.saCloser2,s.saCloser3,s.saExit1,s.saExit2)
		WHEN @Roles = 'CLOSER' THEN dbo.UFN_OR_GetPercentageSalesman(s.saCloser1,s.saCloser2,s.saCloser3,default,default)
		ELSE
		cast(0 as Money)
	END))
FROM Sales S
	left join SaleTypes ST on S.sast = ST.stID
where
	-- Closer1
	S.saCloser1 is not null
	-- Fecha de procesable
	and ((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo))))
	-- No downgrades
	and (ST.ststc <> 'DG'
	-- Downgrades cuya membresia de referencia esta dentro del periodo
	or (@ConsiderCancel = 0 and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)))
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD) 
	-- Salas de ventas
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
	-- Puestos
	and ((@Post = 'ALL' or dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saCloser1) in (select item from split(@Post, ',')))
	OR
	((@Post = 'ALL' or dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saCloser1)='NP')))
	-- Permitir repetidos
	AND (@AllowRepeated = 1 OR S.saCloser1 NOT IN (IsNull(S.saLiner1, ''), IsNull(S.saLiner2, '')))
group by S.saCloser1, S.saD

-- Monto de Ventas (Closer 2)
union all
SELECT 
	S.saCloser2,
	dbo.UFN_IM_GetPersonnelPostIDByDate((CASE
	 WHEN @AllowRepeated = 1 THEN @DateTo
	 ELSE S.saD end), S.saCloser2) [LastPost],
	sum(S.saGrossAmount * (CASE 
		WHEN @Roles = 'ALL' THEN dbo.UFN_OR_GetPercentageSalesman(s.saCloser1,s.saCloser2,s.saCloser3,s.saExit1,s.saExit2)
		WHEN @Roles = 'CLOSER' THEN dbo.UFN_OR_GetPercentageSalesman(s.saCloser1,s.saCloser2,s.saCloser3,default,default)
		ELSE
		Cast(0 as Money)
	END))
FROM Sales S
	left join SaleTypes ST on S.sast = ST.stID
where
	-- Closer 2
	S.saCloser2 is not null
	-- Fecha de procesable
	and ((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo))))
	-- No downgrades
	and (ST.ststc <> 'DG'
	-- Downgrades cuya membresia de referencia esta dentro del periodo
	or (@ConsiderCancel = 0 and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)))
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD) 
	-- Salas de ventas
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
	-- Puestos
	and ((@Post = 'ALL' or dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saCloser2) in (select item from split(@Post, ',')))
	OR
	((@Post = 'ALL' or dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saCloser2)='NP')))
	-- Permitir repetidos
	AND (@AllowRepeated = 1 OR S.saCloser2 NOT IN (IsNull(S.saLiner1, ''), IsNull(S.saLiner2, ''), IsNull(S.saCloser1, '')))
	
group by S.saCloser2, S.saD

union all

-- Monto de Ventas (Closer 3)
SELECT 
	S.saCloser3,
	dbo.UFN_IM_GetPersonnelPostIDByDate((CASE
	 WHEN @AllowRepeated = 1 THEN @DateTo
	 ELSE S.saD end), S.saCloser3) [LastPost],
	sum(S.saGrossAmount * (CASE 
		WHEN @Roles = 'ALL' THEN dbo.UFN_OR_GetPercentageSalesman(s.saCloser1,s.saCloser2,s.saCloser3,s.saExit1,s.saExit2)
		WHEN @Roles = 'CLOSER' THEN dbo.UFN_OR_GetPercentageSalesman(s.saCloser1,s.saCloser2,s.saCloser3,default,default)
		ELSE
		Cast(0 as money)
	END))
FROM Sales S
	left join SaleTypes ST on S.sast = ST.stID
where
	-- Closer 3
	S.saCloser3 is not null
	-- Fecha de procesable
	and ((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo))))
	-- No downgrades
	and (ST.ststc <> 'DG'
	-- Downgrades cuya membresia de referencia esta dentro del periodo
	or (@ConsiderCancel = 0 and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)))
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD) 
	-- Salas de ventas
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
	-- Puestos
	and ((@Post = 'ALL' or dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saCloser3) in (select item from split(@Post, ',')))
	OR
	((@Post = 'ALL' or dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saCloser3)='NP')))
	-- Permitir repetidos
	AND (@AllowRepeated = 1 OR S.saCloser3 NOT IN (IsNull(S.saLiner1, ''), IsNull(S.saLiner2, ''), IsNull(S.saCloser1, ''), IsNull(S.saCloser2,'')))	
group by S.saCloser3, S.saD

union all

-- Monto de Ventas (Exit 1)
SELECT 
	S.saExit1,
	dbo.UFN_IM_GetPersonnelPostIDByDate((CASE
	 WHEN @AllowRepeated = 1 THEN @DateTo
	 ELSE S.saD end), S.saExit1) [LastPost],
	sum(S.saGrossAmount * (CASE 
		WHEN @Roles = 'ALL' THEN dbo.UFN_OR_GetPercentageSalesman(s.saCloser1,s.saCloser2,s.saCloser3,s.saExit1,s.saExit2)
		WHEN @Roles = 'EXIT' THEN dbo.UFN_OR_GetPercentageSalesman(default,default,default,s.saExit1,s.saExit2)
		ELSE
		CAST(0 AS MONEY)
	END))
FROM Sales S
	left join SaleTypes ST on S.sast = ST.stID
where	
	-- Exit 1
	S.saExit1 is not null
	-- Fecha de procesable
	and ((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo))))
	-- No downgrades
	and (ST.ststc <> 'DG'
	-- Downgrades cuya membresia de referencia esta dentro del periodo
	or (@ConsiderCancel = 0 and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)))
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD) 
	-- Salas de ventas
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
	-- Puestos
	and ((@Post = 'ALL' or dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saExit1) in (select item from split(@Post, ',')))
	OR
	((@Post = 'ALL' or dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saExit1)='NP')))
	-- Permitir repetidos
	AND (@AllowRepeated = 1 OR S.saExit1 NOT IN (IsNull(S.saLiner1, ''), IsNull(S.saLiner2, ''), IsNull(S.saCloser1, ''), IsNull(S.saCloser2,''),IsNull(S.saCloser3,'')))	
group by S.saExit1, S.saD

union all

-- Monto de Ventas (Exit 2)
SELECT 
	S.saExit2,
	dbo.UFN_IM_GetPersonnelPostIDByDate((CASE
	 WHEN @AllowRepeated = 1 THEN @DateTo
	 ELSE S.saD end), S.saExit2) [LastPost],
	sum(S.saGrossAmount * (CASE 
		WHEN @Roles = 'ALL' THEN dbo.UFN_OR_GetPercentageSalesman(s.saCloser1,s.saCloser2,s.saCloser3,s.saExit1,s.saExit2)
		WHEN @Roles = 'EXIT' THEN dbo.UFN_OR_GetPercentageSalesman(default,default,default,s.saExit1,s.saExit2)
		ELSE
		CAST(0 AS MONEY)
	END))FROM Sales S
	left join SaleTypes ST on S.sast = ST.stID
where
	-- Closer 2
	S.saExit2 is not null
	-- Fecha de procesable
	and ((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo))))
	-- No downgrades
	and (ST.ststc <> 'DG'
	-- Downgrades cuya membresia de referencia esta dentro del periodo
	or (@ConsiderCancel = 0 and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)))
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD) 
	-- Salas de ventas
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
	-- Puestos
	and ((@Post = 'ALL' or dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saExit2) in (select item from split(@Post, ',')))
	OR
	((@Post = 'ALL' or dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saExit2)='NP')))
	-- Permitir repetidos
	AND (@AllowRepeated = 1 OR S.saExit2 NOT IN (IsNull(S.saLiner1, ''), IsNull(S.saLiner2, ''), IsNull(S.saCloser1, ''), IsNull(S.saCloser2,''),IsNull(S.saCloser3,''),IsNull(S.saExit1,'')))	
group by S.saExit2, S.saD
return
end

GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetCloserShows]    Script Date: 09/21/2016 17:07:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por Closer
** 
** [edgrodriguez]		09/May/2016 Created
**
*/
CREATE function [dbo].[UFN_IM_GetCloserShows](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@SalesRooms varchar(max) = 'ALL'	-- Clave de las salas de ventas
)
returns @Table table (
	Closer varchar(10),
	Shows money
)
as
begin

insert @Table

-- Shows (Closer 1)
-- =============================================
select
	G.guCloser1,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guCloser1, G.guCloser2, G.guCloser3, G.guExit1, G.guExit2))
from Guests G
where
	-- Closer 1
	G.guCloser1 is not null
	and G.guTour = 1
	-- Fecha de show
	and (G.guShowD between @DateFrom and @DateTo)
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
group by G.guCloser1
UNION ALL
-- Shows (Closer 2)
-- =============================================
select
	G.guCloser2,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guCloser1, G.guCloser2, G.guCloser3, G.guExit1, G.guExit2))
from Guests G
where
	-- Closer 1
	G.guCloser2 is not null
	and G.guTour = 1
	-- Fecha de show
	and (G.guShowD between @DateFrom and @DateTo)
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
group by G.guCloser2
UNION ALL
-- Shows (Closer 3)
-- =============================================
select
	G.guCloser3,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guCloser1, G.guCloser2, G.guCloser3, G.guExit1, G.guExit2))
from Guests G
where
	-- Closer 3
	G.guCloser3 is not null
	and G.guTour = 1
	-- Fecha de show
	and (G.guShowD between @DateFrom and @DateTo)
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
group by G.guCloser3
UNION ALL
-- Shows (Exit 1)
-- =============================================
select
	G.guExit1,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guCloser1, G.guCloser2, G.guCloser3, G.guExit1, G.guExit2))
from Guests G
where
	-- Exit 1
	G.guExit1 is not null
	and G.guTour = 1
	-- Fecha de show
	and (G.guShowD between @DateFrom and @DateTo)
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
group by G.guExit1
UNION ALL
-- Shows (Exit 2)
-- =============================================
select
	G.guExit2,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guCloser1, G.guCloser2, G.guCloser3, G.guExit1, G.guExit2))
from Guests G
where
	-- Exit 2
	G.guExit2 is not null
	and G.guTour = 1
	-- Fecha de show
	and (G.guShowD between @DateFrom and @DateTo)
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
group by G.guExit2
return
end

GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetFlightSalesRoomBookings]    Script Date: 09/21/2016 17:07:57 ******/
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

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetFlightSalesRoomSales]    Script Date: 09/21/2016 17:08:00 ******/
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

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetFlightSalesRoomSalesAmount]    Script Date: 09/21/2016 17:08:04 ******/
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

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetFlightSalesRoomShows]    Script Date: 09/21/2016 17:08:08 ******/
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

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelBookings]    Script Date: 09/21/2016 17:08:11 ******/
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

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelSales]    Script Date: 09/21/2016 17:08:15 ******/
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

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelSalesAmount]    Script Date: 09/21/2016 17:08:19 ******/
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

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelSalesRoomBookings]    Script Date: 09/21/2016 17:08:22 ******/
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

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelSalesRoomSales]    Script Date: 09/21/2016 17:08:26 ******/
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

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelSalesRoomSalesAmount]    Script Date: 09/21/2016 17:08:30 ******/
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

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelSalesRoomShows]    Script Date: 09/21/2016 17:08:34 ******/
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

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetHotelShows]    Script Date: 09/21/2016 17:08:37 ******/
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

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetLinerSales]    Script Date: 09/21/2016 17:08:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas por Closer
** 
** [edgrodriguez]	07/May/2016 Created.
** [ecanul]			29/06/2016 Modified. Agregado parameto @Regen Solo para validar shows Regen
**
*/
CREATE function [dbo].[UFN_IM_GetLinerSales](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@SalesRooms varchar(max) = 'ALL',	-- Clave de las salas de ventas
	
	@ConsiderOutOfPending bit = 0,	-- Indica si se debe considerar Out Of Pending
	@ConsiderCancel bit = 0,		-- Indica si se debe considerar canceladas	
	@ConsiderPending bit = 0,		-- Indica si se debe considerar pendientes
	@saLiner1Type int = -1,			--Filtro de Tipo de liner.
									-- -1.- Sin Filtro
									--	0.- Liner
									--	1.- Front to Middle
									--	2.- Front to Back
	@Regen bit = 0					-- Solo ventas regen Regen
)
returns @Table table (
	Liner varchar(10),
	Sales money
)
as
begin

insert @Table

-- Numero de ventas (Liner 1)
-- =============================================
select
	S.saLiner1,
	Sum(dbo.UFN_OR_GetPercentageSalesman(S.saLiner1,S.saLiner2,default,default,default))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
where
	-- Closer1 1
	S.saLiner1 is not null
	
	-- Fecha de procesable
	and ((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo)))))
		
	-- No downgrades
	and ST.ststc <> 'DG'
	
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
	
	--Tipo de Liner
	and(@saLiner1Type = -1 or S.saLiner1Type=@saLiner1Type)
	-- Ventas Regen
	AND (@Regen = 0 OR S.sast = 'REGEN')
group by S.saLiner1
UNION ALL
-- Numero de ventas (Liner 2)
-- =============================================
select
	S.saLiner2,
	Sum(dbo.UFN_OR_GetPercentageSalesman(S.saLiner1,S.saLiner2,default,default,default))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
where
	-- Liner 2
	S.saLiner2 is not null
	
	-- Fecha de procesable
	and ((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo)))))
		
	-- No downgrades
	and ST.ststc <> 'DG'
	
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
	
	--Tipo de Liner
	and(@saLiner1Type = -1 or S.saLiner1Type=@saLiner1Type)
	-- Ventas Regen
	AND (@Regen = 0 OR S.sast = 'REGEN')
group by S.saLiner2
return
end


GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetLinerSalesAmount]    Script Date: 09/21/2016 17:08:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de ventas por Liner
** 
** [edgrodriguez]	07/May/2016 Created.
** [edgrodriguez]	24/Jun/2016 Modified. Se agrego el parametro @Post para filtrar puestos.
** [ecanul]			29/06/2016 Modified. Agregado parameto @Regen Solo para validar shows Regen
**
*/
CREATE function [dbo].[UFN_IM_GetLinerSalesAmount](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@SalesRooms varchar(max) = 'ALL',	-- Clave de las salas de ventas
	@ConsiderOutOfPending bit = 0,	-- Indica si se debe considerar Out Of Pending	
	@ConsiderCancel bit = 0,		-- Indica si se debe considerar canceladas
	@ConsiderPending bit = 0,		-- Indica si se debe considerar pendientes
	@saLiner1Type int = -1,			--Filtro de Tipo de liner.
									-- -1.- Sin Filtro
									--	0.- Liner
									--	1.- Front to Middle
									--	2.- Front to Back
	@Post varchar(max) = 'ALL',		-- Claves de los puestos.
	@AllowRepeated bit = 1,			-- Indica si permite vendedores repetidos en diferentes posiciones
	@Regen bit = 0					-- Solo ventas regen Regen
	
	
)
returns @Table table (
	Liner varchar(10),
	LastPost varchar(50),
	SalesAmount money
)
as
begin

insert @Table

-- Monto de ventas (Liner 1)

SELECT 
S.saLiner1,
	dbo.UFN_IM_GetPersonnelPostIDByDate((CASE
	 WHEN @AllowRepeated = 1 THEN @DateTo
	 ELSE S.saD end), S.saLiner1) [LastPost],
sum(S.saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(s.saLiner1,s.saLiner2,default,default,default))
FROM
Sales S
left join SaleTypes ST on S.sast = ST.stID
left join Personnel Pe on S.saLiner1 = Pe.peID 
where
	-- Liner1
	S.saLiner1 is not null
	-- Fecha de procesable
	and ((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo))))
	-- No downgrades
	and (ST.ststc <> 'DG'
	-- Downgrades cuya membresia de referencia esta dentro del periodo
	or (@ConsiderCancel = 0 and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)))
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD) 
	-- Salas de ventas
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
	--Tipo de Liner
	and(@saLiner1Type = -1 or S.saLiner1Type=@saLiner1Type)
	-- Puestos
	and ((@Post = 'ALL' or dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saLiner1) in (select item from split(@Post, ',')))
	OR
	((@Post = 'ALL' or dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saLiner1)='NP')))
	-- Ventas Regen
	AND (@Regen = 0 OR S.sast = 'REGEN')
group by S.saLiner1, S.saD

union all

-- Monto de Ventas (Liner 2)
SELECT 
S.saLiner2,
dbo.UFN_IM_GetPersonnelPostIDByDate((CASE
 WHEN @AllowRepeated = 1 THEN @DateTo
 ELSE S.saD end), S.saLiner2) [LastPost],
sum(S.saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(s.saLiner1,s.saLiner2,default,default,default))
FROM
Sales S
left join SaleTypes ST on S.sast = ST.stID
left join Personnel Pe on S.saLiner2 = Pe.peID 
where
	-- Liner 2
	S.saLiner2 is not null
	-- Fecha de procesable
	and ((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo))))
	-- No downgrades
	and (ST.ststc <> 'DG'
	-- Downgrades cuya membresia de referencia esta dentro del periodo
	or (@ConsiderCancel = 0 and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)))
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD) 
	-- Salas de ventas
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
	--Tipo de Liner
	and(@saLiner1Type = -1 or S.saLiner1Type=@saLiner1Type)
	-- Puestos
	and ((@Post = 'ALL' or dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saLiner2) in (select item from split(@Post, ',')))
	OR
	((@Post = 'ALL' or dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saLiner2)='NP')))
	-- Permitir repetidos
	AND (@AllowRepeated = 1 OR S.saLiner2 NOT IN (IsNull(S.saLiner1, '')))	
	-- Ventas Regen
	AND (@Regen = 0 OR S.sast = 'REGEN')
group by S.saLiner2, S.saD
return
end

GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetLinerShows]    Script Date: 09/21/2016 17:08:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por Liner
** 
** [edgrodriguez]		11/May/2016 Created
** [edgrodriguez]		20/Jun/2016 Modified. Se agrego el parametro ShowType para filtrar los tipos de show.
** [ecanul]				27/jun/2016 Modified. Se agrego filtro para obtener los shows reales
** [ecanul]				05/Sep/2016 Modified. Eliminado Join con tabla Sales, Agregaba un Show mas al resultado 
**
*/
CREATE function [dbo].[UFN_IM_GetLinerShows](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@SalesRooms varchar(max) = 'ALL',	-- Clave de las salas de ventas
	@ShowType int = 0,					-- Tipo de Show
										-- 0. Sin Filtro
										-- 1. Regular Tour
										-- 2. In&Out
										-- 3. WalkOut
										-- 4. Courtesy Tour
										-- 5. Save Tour
										-- 6. With Quinellas
	@RealShows bit = 0					-- Real Shows
										-- 0 Shows = Total Shows - Walk Outs & CTours
										-- 1 Shows Reales 
)
returns @Table table (
	Liner varchar(10),
	Shows money
)
as
begin

insert @Table

-- Shows (Liner 1)
-- =============================================
select
	G.guLiner1,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guLiner1, G.guLiner2,default,default,default))
from dbo.Guests G
where
	-- Liner 1
	G.guLiner1 is not null
	-- Fecha de show
	and G.guShowD between @DateFrom and @DateTo
	-- Filtro tipo de show
	and (@ShowType = 0 or (@ShowType = 1 and G.guTour = 1) or (@ShowType = 2 and G.guInOut = 1) or (@ShowType = 3 and G.guWalkOut = 1) 
	or (@ShowType = 4 and G.guCTour = 1) or (@ShowType = 5 and G.guSaveProgram = 1) or (@ShowType = 6 and G.guWithQuinella = 1))
	-- Real Shows			Shows Reales
	AND (@RealShows = 0 OR (G.guTour = 1 OR G.guWalkOut = 1 OR ((G.guCTour = 1 OR g.guSaveProgram = 1) AND g.guSale = 1)))
	-- Sales Rooms ---- 
	and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
group by G.guLiner1
UNION ALL
-- Shows (Liner 1)
-- =============================================
select
	G.guLiner2,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guLiner1, G.guLiner2,default,default,default))
from dbo.Guests G
where
	-- Liner 2
	G.guLiner2 is not null
	-- Fecha de show
	and G.guShowD between @DateFrom and @DateTo
	-- Filtro tipo de show
	and (@ShowType = 0 or (@ShowType = 1 and G.guTour = 1) or (@ShowType = 2 and G.guInOut = 1) or (@ShowType = 3 and G.guWalkOut = 1) 
	or (@ShowType = 4 and G.guCTour = 1) or (@ShowType = 5 and G.guSaveProgram = 1) or (@ShowType = 6 and G.guWithQuinella = 1))
	-- Real Shows			Shows Reales
	AND (@RealShows = 0 OR (G.guTour = 1 OR G.guWalkOut = 1 OR ((G.guCTour = 1 OR g.guSaveProgram = 1) AND g.guSale = 1)))
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
group by G.guLiner2
return
end


GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveBookings]    Script Date: 09/21/2016 17:08:50 ******/
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

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveSales]    Script Date: 09/21/2016 17:08:54 ******/
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

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveSalesAmount]    Script Date: 09/21/2016 17:08:58 ******/
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

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveSalesRoomBookings]    Script Date: 09/21/2016 17:09:02 ******/
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

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveSalesRoomSales]    Script Date: 09/21/2016 17:09:05 ******/
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

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveSalesRoomSalesAmount]    Script Date: 09/21/2016 17:09:09 ******/
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

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveSalesRoomShows]    Script Date: 09/21/2016 17:09:13 ******/
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

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetWaveShows]    Script Date: 09/21/2016 17:09:16 ******/
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


