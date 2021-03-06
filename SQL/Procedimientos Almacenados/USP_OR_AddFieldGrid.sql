if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_AddFieldGrid]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_AddFieldGrid]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Agrega un campo a un layout de grid
** 
** [wtorres]	16/Oct/2009 Created
** [wtorres]	17/Sep/2014 Modified. Renombrado. Antes se llamaba USP_OR_AgregarCampoGrid
**
*/
create procedure [dbo].[USP_OR_AddFieldGrid]	
	@Grid varchar(20),			-- Clave del grid
	@FieldName varchar(20),			-- Nombre del campo
	@Title varchar(25),				-- Título
	@ToolTip varchar(50) = NULL,	-- Tool Tip
	@Width int = 500,				-- Ancho
	@Visibility tinyint = 1			-- Visibilidad:
									--		1. Visible
									--		2. Oculto por default
									--		3. Oculto siempre
as
set nocount on

declare @Position smallint

-- obtenemos el consecutivo de posicion
select @Position = IsNull(Max(fgColPosition), 0) + 1
from FieldsByGrid
where fgGrid = @Grid

insert into FieldsByGrid (fgGrid, fgColPosition, fgFieldName, fgHeading, fgToolTipText, fgWidth, fgVisible)
values (@Grid, @Position, @FieldName, @Title, @ToolTip, @Width, @Visibility)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

