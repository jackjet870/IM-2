if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetSalesByCloser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetSalesByCloser]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene las ventas de un closer
** 
** [wtorres]	14/Ago/2014 Created
**
*/
create procedure [dbo].[USP_OR_GetSalesByCloser] 
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@SalesRoom varchar(10) = 'ALL',	-- Clave de la sala de ventas
	@Closer varchar(10) = 'ALL'		-- Clave del closer
as
set nocount on

select
	S.saID,
	S.sals,
	S.sasr,
	S.saMembershipNum,
	ST.stN,
	S.saD,
	S.saProcD,
	S.saLastName1,
	S.saCloser1,
	C1.peN as Closer1N,
	S.saCloser2,
	C2.peN as Closer2N,
	S.saCloser3,
	C3.peN as Closer3N,
	S.saExit1,
	E1.peN as Exit1N,
	S.saExit2,
	E2.peN as Exit2N,
	S.saGrossAmount,
	S.saCancel,
	S.saCancelD
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Personnel C1 on C1.peID = S.saCloser1
	left join Personnel C2 on C2.peID = S.saCloser2
	left join Personnel C3 on C3.peID = S.saCloser3
	left join Personnel E1 on E1.peID = S.saExit1
	left join Personnel E2 on E2.peID = S.saExit2
where
	-- Sala de ventas
	(@SalesRoom = 'ALL' or S.sasr = @SalesRoom)
	-- Fecha de procesable
	and S.saProcD between @DateFrom and @DateTo
	-- Liners
	and (@Closer = 'ALL' or (S.saCloser1 = @Closer or S.saCloser2 = @Closer or S.saCloser3 = @Closer
		or S.saExit1 = @Closer or S.saExit2 = @Closer))
order by S.saMembershipNum

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

