if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_InMaintenance]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_InMaintenance]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Procedimiento:	En Mantenimiento
-- Descripción:		Indica si la base de datos está en mantenimiento
-- Histórico:		[wtorres] 27/Jul/2010 Depurado
-- =============================================
create procedure [dbo].[USP_OR_InMaintenance]
	@LeadSource varchar(10) = '',	-- Clave del Lead Source
	@SalesRoom varchar(10) = ''		-- Clave de la sala de ventas
as
set nocount on

declare
	@BDInMaintenance bit,			-- Indica si la BD está en mantenimiento
	@LeadSourceInMaintenance bit,	-- Indica si el Lead Source está en mantenimiento
	@SalesRoomInMaintenance bit	-- Indica si la sala está en mantenimiento

set @BDInMaintenance = 0
set @LeadSourceInMaintenance = 0
set @SalesRoomInMaintenance = 0

-- Determina si la BD está en mantenimiento
set @BDInMaintenance = (select ocDBMaint from osConfig)
if @BDInMaintenance = 0

	-- Determina si el Lead Source está en mantenimiento
	if @LeadSource <> ''
		set @LeadSourceInMaintenance = (select lsDBMaint from LeadSources where lsID = @LeadSource)

if @BDInMaintenance = 0 and @LeadSourceInMaintenance = 0

	-- Determina si la sala está en mantenimiento
	if @SalesRoom <> ''
		set @SalesRoomInMaintenance = (select srDBMaint from SalesRooms where srID = @SalesRoom)

-- Indica si hay algo en mantenimiento y el número de revisión de la BD
select 
	Cast((case when (@BDInMaintenance = 1 or @SalesRoomInMaintenance = 1 or @LeadSourceInMaintenance = 1) then 1 else 0 end) as bit)
		as InMaintenance,
	ocDBRevision
from osConfig

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

