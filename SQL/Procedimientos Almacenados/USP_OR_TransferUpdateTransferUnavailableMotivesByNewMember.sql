if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferUpdateTransferUnavailableMotivesByNewMember]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferUpdateTransferUnavailableMotivesByNewMember]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Procedimiento:	Actualizar motivo de indisponibilidad de reservaciones migradas por ser nuevo socio en el proceso de transferencia
-- Descripción:		Actualiza el motivo de indisponibilidad de reservaciones migradas por ser nuevo socio (18 = NEW MEMBER)
--					Un huésped es definido como nuevo socio si tiene una venta de cuando mucho una semana
-- Histórico:		[wtorres] 13/Jul/2009 Creado
-- =============================================
create procedure [dbo].[USP_OR_TransferUpdateTransferUnavailableMotivesByNewMember]
as
set nocount on

-- Obtiene la fecha actual
declare @Date datetime
set @Date = Convert(varchar, GetDate(), 112)

-- Actualiza el motivo de indisponibilidad
update osTransfer
set tum = 18
from osTransfer
	inner join Sales on tMembershipNum = saMembershipNum
where 
	-- Fecha de procesable de cuando mucho 1 semana
	saProcD between @Date - 7 and @Date
	-- No cancelada
	and saCancel = 0

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

