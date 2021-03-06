if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_AddFieldReport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_AddFieldReport]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Agrega un campo a un layout de reportes
** 
** [wtorres]	25/Oct/2008 Created
** [wtorres]	04/Feb/2010 Modified. Aumente el tamaño de la clave del reporte a 40
** [asanchez]	22/Ago/2013 Modified. Aumente el tamaño de la clave del reporte a 50
** [lchairez]   01/Nov/2013 Modified. Aumente el tamaño del nombre del campo
** [wtorres]	17/Sep/2014 Modified. Agregue los parametros de nombre de la fuente y alineacion.
**							Renombrado. Antes se llamaba USP_OR_AgregarCampoReporte
*/
create procedure [dbo].[USP_OR_AddFieldReport]	
	@Report varchar(50),			-- Clave del reporte
	@FieldName varchar(30),			-- Nombre del campo
	@Title varchar(25),				-- Título
	@ToolTip varchar(50) = NULL,	-- Tool Tip
	@Width int = 500,				-- Ancho
	@Visible tinyint = 1,			-- Visibilidad
									--		1. Visible
									--		2. Oculto por default
									--		3. Oculto siempre
	@Format varchar(20) = NULL,		-- Formato
	@Operation varchar(200) = NULL,	-- Operacion
	@FontSize tinyint = NULL,		-- Tamaño de la fuente
	@Alignment tinyint = NULL,		-- Alineacion:
									--		0. Izquierda
									--		1. Derecha
									--		2. Centro
	@FontName varchar(50) = NULL	-- Nombre de la fuente
as
set nocount on

declare @Position smallint

-- obtenemos el consecutivo de posicion
select @Position = IsNull(Max(frColPosition), 0) + 1
from FieldsByReport 
where frReport = @Report

insert into FieldsByReport (frReport, frColPosition, frFieldName, frHeading, frToolTipText, frWidth, frVisible, frFormat, frOperation, 
	frFontSize, frAlignment, frFontName)
values (@Report, @Position, @FieldName, @Title, @ToolTip, @Width, @Visible, @Format, @Operation, @FontSize, @Alignment, @FontName)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

