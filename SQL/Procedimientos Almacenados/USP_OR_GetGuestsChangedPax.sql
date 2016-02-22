if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetGuestsChangedPax]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetGuestsChangedPax]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Procedimiento:	Obtener hu�spedes que cambiaron su pax
-- Descripci�n:		Devuelve los hu�spedes a que cambiaron su pax
-- Hist�rico:		[wtorres] 05/Abr/2010 Creado
-- =============================================
create procedure [dbo].[USP_OR_GetGuestsChangedPax] 
as
	set nocount on;

select gulsOriginal, guHReservID, IsNull(guAccountGiftsCard, '') as guAccountGiftsCard
from Guests
	inner join osTransfer on guHReservID = tHReservID and gulsOriginal = tls
where (guPax is null or guPax <> tPax)
	-- Actualizar s�lo si aun no tiene show
	and guShow = 0

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

