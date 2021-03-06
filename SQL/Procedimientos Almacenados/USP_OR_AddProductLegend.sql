if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_AddProductLegend]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_AddProductLegend]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Agrega las leyendas de un producto
** 
** [wtorres]	12/Jun/2013 Created
**
*/
create procedure [dbo].[USP_OR_AddProductLegend]
	@Product varchar(10),		-- Clave del producto
	@TextSpanish varchar(max),	-- Texto en español
	@TextEnglish varchar(max)	-- Texto en ingles
as
set nocount on

-- agregamos las leyendas en los diferentes idiomas
insert into ProductsLegends (pxpr, pxla, pxText)
select @Product, 'ES', @TextSpanish
union all select @Product, 'EN', @TextEnglish

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

