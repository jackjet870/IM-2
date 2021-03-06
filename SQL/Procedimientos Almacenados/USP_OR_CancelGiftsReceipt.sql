if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_CancelGiftsReceipt]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_CancelGiftsReceipt]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Cancela un recibo de regalos
** 
** [wtorres]	25/Ago/2014 Created
**
*/
create procedure [dbo].[USP_OR_CancelGiftsReceipt]
	@Receipt int,	-- Clave del recibo de regalos
	@Date datetime	-- Fecha de cancelacion
as 
set nocount on

update GiftsReceipts
set grCancel = 1,
	grCancelD = @Date
where grID = @Receipt

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

