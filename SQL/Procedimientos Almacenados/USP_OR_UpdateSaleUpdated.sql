if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_UpdateSaleUpdated]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_UpdateSaleUpdated]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Marca o desmarca una venta como actualizada
**
** [wtorres]	07/Ago/2015	Created
**
*/
create procedure [dbo].[USP_OR_UpdateSaleUpdated]
	@SaleID int,
	@Updated bit
as

update Sales set saUpdated = @Updated where saID = @SaleID
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

