if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptRepsPayment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptRepsPayment]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de pago de agentes
** 
** [wtorres]	12/Oct/2010 Ahora se pasa la lista de Lead Sources como un sólo parámetro
**
*/
create procedure [dbo].[USP_OR_RptRepsPayment]
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000)	-- Claves de Lead Sources
as
set nocount on

-- Obtiene los shows a pagar
-- =============================================
select
	-- Clave del huésped
	G.guID,
	-- Agente
	A.agrp,
	-- Agencia
	G.guag,
	-- Fecha de book
	G.guBookD,
	-- Nombre completo del huésped
	dbo.UFN_OR_GetFullName(G.guLastName1, G.guFirstName1) as GuestName,
	-- Calificado
	case G.guQ when 1 then 'ü' else '' end as Q,
	-- Monto total de shows
	A.agShowPay,
	-- Número de ventas
	Cast(0 as int) as agSale,
	-- Monto por venta
	A.agSalePay,
	-- Monto total de ventas
	Cast(0 as money) as agTotalSalePay,
	-- Monto total
	A.agShowPay as agTotalPay,
	-- In & Out
	G.guInOut,
	-- Walk Out
	G.guWalkOut
into	
	#tblData
from Guests G
	inner join Agencies A on G.guag = A.agID
	inner join LeadSources L on G.guls = L.lsID
where
	-- Lead Sources
	G.guls in (select item from Split(@LeadSources, ','))
	-- Fecha de book
	and G.guBookD between @DateFrom and @DateTo
	-- Con show
	and G.guShow = 1
	-- Mercado de agencias
	and A.agmk = 'AGENCIES' 
	-- Agencias a las que se les pagan los shows
	and A.agShowPay > 0
	-- Huéspedes no In & Out ni Walk Out
	and ((G.guInOut = 0 and G.guWalkOut = 0)
	-- Huéspedes In & Out y que el Lead Source pague por In & Out
	or (G.guInOut = 1 and L.lsPayInOut = 1) 
	-- Huéspedes Walk Out y que el Lead Source pague por Walk Out
	or (G.guWalkOut = 1 and L.lsPayWalkOut = 1))	
order by A.agrp, G.guag, G.guBookD, G.guLastName1

-- Obtiene las ventas a pagar
-- =============================================
update #tblData
set agSale = 1,
	agTotalSalePay = agSalePay,
	agTotalPay = agTotalPay + agSalePay
from #tblData D
	inner join Sales S on S.sagu = D.guID
where
	-- Fecha de venta procesable
	S.saProcD between @DateFrom and @DateTo 
	-- Ventas no canceladas
	and S.saCancel = 0 

-- Devuelve la información para el reporte
-- =============================================
select * from #tblData
order by agrp, agSalePay, Q desc

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

