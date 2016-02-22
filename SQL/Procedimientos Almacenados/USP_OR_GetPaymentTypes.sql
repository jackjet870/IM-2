if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetPaymentTypes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetPaymentTypes]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta el catalogo de formas de pago
** 
** [wtorres]	14/May/2014 Creado
**
*/
create procedure [dbo].[USP_OR_GetPaymentTypes]
	@Status tinyint = 1	-- Filtro de estatus
						--		0. Sin filtro
						--		1. Activas
						--		2. Inactivas
as
set nocount on

select ptID, ptN
from PaymentTypes
where
	-- Estatus
	(@Status = 0 or (@Status = 1 and ptA = 1) or (@Status = 2 and ptA = 0))
order by ptN

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

