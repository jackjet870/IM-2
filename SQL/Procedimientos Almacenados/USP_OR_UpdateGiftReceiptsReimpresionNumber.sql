if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_UpdateGiftReceiptsReimpresionNumber]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_UpdateGiftReceiptsReimpresionNumber]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Actualiza el contador de reimpresion de un recibo de regalos
** 
** [wtorres]	31/Dic/2013 Creado
**
*/
create procedure [dbo].[USP_OR_UpdateGiftReceiptsReimpresionNumber]
	@Receipt int	-- Clave del recibo de regalos
as
set nocount on

update GiftsReceipts
set grReimpresion = IsNull(grReimpresion, 0) + 1
where grID = @Receipt

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

