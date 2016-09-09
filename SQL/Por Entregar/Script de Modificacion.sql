/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Script de modificacion de tablas
** 
** [wtorres]	20/Feb/2016 Created
**
*/
use OrigosVCPalace

-- I. MODIFICACIONES DE TABLAS
--		1. Segmentos por agencia
--		2. Segmentos por Lead Source

-- =============================================
--			MODIFICACIONES DE TABLAS
-- =============================================
-- Tabla:			Segmentos por agencia
-- [wtorres]		08/Sep/2016 Agregue la llave foranea con el catalogo de categorias de segmentos
-- =============================================

-- Llaves foraneas - Segmentos por agencia - Categorias de segmentos
ALTER TABLE SegmentsByAgency ADD 
	CONSTRAINT FK_SegmentsByAgency_SegmentsCategories FOREIGN KEY (sesc)
	REFERENCES SegmentsCategories (scID)
GO

-- =============================================
-- Tabla:			Segmentos por Lead Source
-- [wtorres]		08/Sep/2016 Agregue la llave foranea con el catalogo de categorias de segmentos
-- =============================================

-- Llaves foraneas - Segmentos por Lead Source - Categorias de segmentos
ALTER TABLE SegmentsByLeadSource ADD 
	CONSTRAINT FK_SegmentsByLeadSource_SegmentsCategories FOREIGN KEY (sosc)
	REFERENCES SegmentsCategories (scID)
GO