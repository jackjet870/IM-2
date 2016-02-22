USE [OrigosVCPalace];
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Agrega los textos de un reporte
** 
** [wtorres]	12/Jun/2013 Created
** [LorMartinez] 18/11/2015 Modified, Se agrega texto en portugues
**
*/
CREATE procedure [dbo].[USP_OR_AddReportText]
	@Report varchar(50),		-- Clave del reporte
	@TextId varchar(50),		-- Clave del texto
	@TextSpanish varchar(max),	-- Texto en español
	@TextEnglish varchar(max),	-- Texto en ingles
  @TextPortugese varchar(MAX), --Texto en portugues
	@IsSection bit = 1			-- Indica si los textos representan en verdad una seccion del reporte
as
set nocount on

declare @Order int

-- si es una seccion del reporte
if @IsSection = 1

	-- obtenemos el ultimo registro del reporte
	select @Order = IsNull(Max(reO), 0) + 1
	from ReportsTexts
	where reReport = @Report and reIsSection = 1

else
	set @Order = 9999

-- agregamos los textos en los diferentes idiomas
insert into ReportsTexts (reReport, reTextId, rela, reO, reIsSection, reText)
select @Report, @TextId, 'ES', @Order, @IsSection, @TextSpanish
union all 
select @Report, @TextId, 'EN', @Order, @IsSection, @TextEnglish
union all 
select @Report, @TextId, 'PO', @Order, @IsSection, @TextPortugese
GO