if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_UpdateGiftReceiptsReimpresionMotive]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_UpdateGiftReceiptsReimpresionMotive]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Actualiza el contador de reimpresion y el motivo de un recibo de regalos
** 
** [axperez]	19/Dic/2013 Creado
**
*/
create procedure [dbo].[USP_OR_UpdateGiftReceiptsReimpresionMotive]
	@Receipt int,				-- Clave del recibo de regalos
	@ReimpresionMotive tinyint	-- Clave del motivo de reimpresion
as
set nocount on

update GiftsReceipts
set grReimpresion = IsNull(grReimpresion, 0) + 1,
	grrm = @ReimpresionMotive
where grID = @Receipt

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

