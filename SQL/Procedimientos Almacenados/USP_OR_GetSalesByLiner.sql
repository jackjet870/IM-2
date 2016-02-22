if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetSalesByLiner]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetSalesByLiner]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene las ventas de un liner
** 
** [wtorres]	14/Ago/2014 Created
**
*/
create procedure [dbo].[USP_OR_GetSalesByLiner] 
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@SalesRoom varchar(10) = 'ALL',	-- Clave de la sala de ventas
	@Liner varchar(10) = 'ALL'		-- Clave del liner
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
	S.saLiner1,
	L1.peN as Liner1N,
	S.saLiner2,
	L2.peN as Liner2N,
	S.saGrossAmount,
	S.saCancel,
	S.saCancelD
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Personnel L1 on L1.peID = S.saLiner1
	left join Personnel L2 on L2.peID = S.saLiner2
where
	-- Sala de ventas
	(@SalesRoom = 'ALL' or S.sasr = @SalesRoom)
	-- Fecha de procesable
	and S.saProcD between @DateFrom and @DateTo
	-- Liners
	and (@Liner = 'ALL' or (S.saLiner1 = @Liner or S.saLiner2 = @Liner))
order by S.saMembershipNum

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

