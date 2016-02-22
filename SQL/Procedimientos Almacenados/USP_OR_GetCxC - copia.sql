if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetCxC]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetCxC]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene las CxC cargadas a los PR
** 
** [wtorres]		02/Jul/2010 Created
** [lchairez]		21/Dic/2013 Modified. Agregue los campos de Autorizado por, Monto pagado, motivo de pago incompleto y Log
** [lchairez]		08/Ene/2014 Modified. Agregue los campos de CxC de regalos, CxC de deposito y CxC de taxi de salida
** [wtorres]		17/Feb/2014 Modified. Correccion del calculo del CxC de taxi de salida, faltaba multiplicarlo por el tipo de cambio de su moneda
**								Agregue la columna de Lead Source
** [lormartinez]	15/Sep/2014 Modified. Agregue los aprametros @User, @DateFrom, @DateTo, @LeadSource y @PR
** [wtorres]		15/Oct/2014 Modified. Exclui los recibos cancelados
**
*/		
create procedure [dbo].[USP_OR_GetCxC]
	@Authorized bit,				-- Indica si se desean las CxC autorizadas
	@SalesRoom varchar(10),			-- Clave de la sala de ventas
	@DateFrom datetime = null,		-- Fecha desde
	@DateTo datetime = null,		-- Fecha hasta
	@User varchar(10),				-- Clave de usuario
	@LeadSource varchar(10) = 'ALL',-- Clave de Lead Source
	@PR varchar(10) = 'ALL'			-- Clave de PR
as
set nocount on

-- Tabla de Lead Sources a los que tiene permiso el usuario
declare @LeadSourcesByUser table (
	ID varchar(10), 
	Name varchar(30)
)
				  
insert into @LeadSourcesByUser
exec USP_OR_GetLeadSourcesByUser @User

-- Recibos de regalos
select
	R.grID,
	R.grNum,
	R.grls,
	R.grgu,
	R.grGuest,
	R.grD,
	R.grpe,
	PR.peN,
	R.grCxCGifts + R.grCxCAdj as grCxCGifts,
	(R.grCxCPRDeposit * IsNull(ED.exExchRate, 1)) as grCxCPRDeposit,
	(R.grCxCTaxiOut * IsNull(ET.exExchRate, 1)) as grCxCTaxiOut,
	-- CxC = CxC de regalos (Cargo + Ajuste) + CxC del deposito + CxC del taxi de salida
	(R.grCxCGifts + R.grCxCAdj) + (R.grCxCPRDeposit * IsNull(ED.exExchRate, 1)) + (R.grCxCTaxiOut * IsNull(ET.exExchRate, 1)) as CxC,
	@Authorized as Authorized,
	R.grCxCAppD,
	R.grAuthorizedBy,
	PA.peN as grAuthorizedName,
	R.grAmountPaid,
	R.grup,
	R.grcxcAuthComments,
	R.grcxcComments,
	'' as [Log]
from GiftsReceipts R
	left join SalesRooms S on R.grsr = S.srID
	left join Personnel PR on R.grpe = PR.peID
	left join Personnel PA on R.grAuthorizedBy = PA.peID
	left join ExchangeRate ED on ED.exD = R.grD and ED.excu = R.grcuCxCPRDeposit
	left join ExchangeRate ET on ET.exD = R.grD and ET.excu = R.grcuCxCTaxiOut
where
	-- Autorizadas
    ((@Authorized = 1 and R.grCxCAppD is not null)
	-- No autorizadas
	or (@Authorized = 0 and R.grCxCAppD is null))
	-- Sala de ventas
	and R.grsr = @SalesRoom
	-- Tengan cargo (de regalos, de deposito o de taxi de salida)
    and ((grCxCGifts + grCxCAdj <> 0) or grCxCPRDeposit > 0 or grCxCTaxiOut > 0)
    -- No recibos cancelados
    and R.grCancel = 0
	-- Fecha del recibo
    and R.grD between @DateFrom and @DateTo
    -- Lead Sources por usuario
    and R.grls in (select ID from @LeadSourcesByUser)
	-- Lead Source
	and (@LeadSource = 'ALL' or R.grls = @LeadSource)
	-- PR
	and (@PR = 'ALL' or R.grpe = @PR)
order by PR.peN, R.grD

-- Fecha de cierre de CxC
select srCxCCloseD from SalesRooms where srID = @SalesRoom

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

