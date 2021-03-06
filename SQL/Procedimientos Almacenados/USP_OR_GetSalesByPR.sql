if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetSalesByPR]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetSalesByPR]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene las ventas de un PR
** 
** [wtorres]	14/Ago/2014 Created
** [wtorres]	05/Ene/2015 Modified. Desplegar los PR de la venta, no de la invitacion
** [lchairez]	19/Abr/2016 Modified. Se agrego el parametro @SearchBySalePR
** [wtorres]	26/Jul/2016 Modified. Simplifique la busqueda por PR de venta o PR de contacto
**
*/
create procedure [dbo].[USP_OR_GetSalesByPR] 
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSource varchar(10) = 'ALL',	-- Clave del Lead Source
	@PR varchar(10) = 'ALL',			-- Clave del PR
	@SearchBySalePR bit = 1				-- Busqueda por PR de venta o PR de contacto. 1 = Buscar por PR de venta, 0 = Buscar por PR de contacto
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
	-- PR's de venta o PR de contacto
	and (@PR = 'ALL'
		-- PR's de venta
		or (@SearchBySalePR = 1 and (S.saPR1 = @PR or S.saPR2 = @PR or S.saPR3 = @PR))
		-- PR de contacto
		or (@SearchBySalePR = 0 and G.guPRInfo = @PR))
order by S.saMembershipNum