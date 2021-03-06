if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetReservationsWithoutAccount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetReservationsWithoutAccount]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Procedimiento:	Obtener reservaciones sin cuenta
-- Descripción:		Obtiene las reservaciones que no tengan cuenta y que tengan fecha de llegada igual a la
--					fecha proporcionada
-- Histórico:		[wtorres] 28/Jul/2009 Creado
--					[wtorres] 26/Oct/2009 Ahora maneja un rango de fechas y agregué el parámetro @ConsiderCheckIn
--					[wtorres] 12/Nov/2009 Agregué el parámetro @ConsiderWithoutAccount
--					[wtorres] 06/Abr/2010 Eliminé la validación de "No Rebook" y agregué el campo guRef
-- =============================================
create procedure [dbo].[USP_OR_GetReservationsWithoutAccount]
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@ConsiderCheckIn bit = 1,		-- Indica si se debe considerar sólo los huéspedes que han hecho Check In
	@ConsiderWithoutAccount bit = 1	-- Indica si se debe considerar sólo los huéspedes sin cuenta
as
set nocount on

select
	guID,
	gulsOriginal,
	guHReservID,
	dbo.UFN_OR_GetFullName(guLastName1, guFirstName1) as Guest,
	guCheckInD,
	guCheckOutD,
	guAccountGiftsCard,
	guRef
from Guests
	left join LeadSources on guls = lsID
where
	-- Ya tengan Check In
	(@ConsiderCheckIn = 0 or guCheckIn = 1)
	-- Fecha de llegada
	and guCheckInD between @DateFrom and @DateTo
	-- Sin cuenta
	and (@ConsiderWithoutAccount = 0 or (guAccountGiftsCard is null or guAccountGiftsCard = ''))
	-- In House
	and lspg = 'IH'

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

