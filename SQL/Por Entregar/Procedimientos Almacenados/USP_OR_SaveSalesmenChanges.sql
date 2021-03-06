if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_SaveSalesmenChanges]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_SaveSalesmenChanges]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*** Palace Resorts** Grupo de Desarrollo Palace**** Guarda los cambios de vendedores de una venta** ** [wtorres]	19/Ago/2015 Created** [emoguel] 11/10/2016 Modified se agregaron los parametros Guest y TyoeMovement***/
create procedure [dbo].[USP_OR_SaveSalesmenChanges]
	@Sale int,					-- Clave de la venta
	@AuthorizedBy varchar(10),	-- Clave de la persona que autorizo los cambios
	@MadeBy varchar(10),		-- Clave del usuario que hizo los cambios
	@Role varchar(10),			-- Clave del rol
	@Position tinyint,			-- Posicion
	@OldSalesman varchar(10),	-- Clave del antiguo vendedor
	@NewSalesman varchar(10),	-- Clave del nuevo vendedor
	@Guest int, 				-- Clave del Guest
	@TypeMovement varchar(10)	-- Clave del tipo de movimiento
as
set nocount on

insert into SalesmenChanges (schDT, schsa, schAuthorizedBy, schMadeBy, schro, schPosition, schOldSalesman, schNewSalesman,schgu,schmt)
values (GetDate(), @Sale, @AuthorizedBy, @MadeBy, @Role, @Position,
	case when @OldSalesman <> '' then @OldSalesman else NULL end,  
	case when @NewSalesman <> '' then @NewSalesman else NULL end,@Guest,@TypeMovement)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

