if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetSalesByPR]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetSalesByPR]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene las ventas de un PR
** 
** [wtorres]	14/Ago/2014 Created
** [wtorres]	05/Ene/2015 Modified. Desplegar los PR de la venta, no de la invitación
**
*/
create procedure [dbo].[USP_OR_GetSalesByPR] 
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSource varchar(10) = 'ALL',	-- Clave del Lead Source
	@PR varchar(10) = 'ALL'				-- Clave del PR
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
	G.guCheckOutD,
	G.guag,
	A.agN,
	S.saPR1,
	P1.peN as PR1N,
	S.saPR2,
	P2.peN as PR2N,
	S.saPR3,
	P3.peN as PR3N,
	G.guQ,
	S.saGrossAmount,
	S.saCancel,
	S.saCancelD
from Sales S
	left join Guests G on G.guID = S.sagu
	left join SaleTypes ST on ST.stID = S.sast
	left join Agencies A on A.agID = G.guag
	left join Personnel P1 on P1.peID = S.saPR1
	left join Personnel P2 on P2.peID = S.saPR2
	left join Personnel P3 on P3.peID = S.saPR3	
where
	-- Lead Source
	(@LeadSource = 'ALL' or S.sals = @LeadSource)
	-- Fecha de procesable
	and S.saProcD between @DateFrom and @DateTo
	-- PRs
	and (@PR = 'ALL' or (S.saPR1 = @PR or S.saPR2 = @PR or S.saPR3 = @PR))
order by S.saMembershipNum

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

