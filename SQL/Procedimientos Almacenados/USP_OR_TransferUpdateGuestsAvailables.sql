if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_TransferUpdateGuestsAvailables]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_TransferUpdateGuestsAvailables]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Actualiza la disponibilidad de huespedes disponibles en el proceso de transferencia
**
** [wtorres] 	23/Abr/2009 Creado
** [wtorres] 	03/Nov/2011 Ahora tambien actualiza el campo de disponible por sistema
**
*/
create procedure [dbo].[USP_OR_TransferUpdateGuestsAvailables] 
	@DateFrom datetime,	-- Fecha desde
	@DateTo datetime	-- Fecha hasta
as
set nocount on

update Guests
set guAvail = 1,
	guOriginAvail = 1,
	guAvailBySystem = 1
where
	-- Motivo disponible
	guum = 0 
	-- No disponible
	and guAvail = 0
	-- Con Check In
	and guCheckIn = 1
	-- Fecha de llegada
	and guCheckInD between @DateFrom and @DateTo
	-- No contactado
	and guInfo = 0
	-- Ningun PR ha modificado su disponibilidad
	and guPRAvail is null

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

