/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Script de modificacion de tablas
** 
** [wtorres]	05/Sep/2016 Created
**
*/
use OrigosVCPalace

-- I. MODIFICACIONES DE TABLAS
--		1. Recibos de regalos
--		2. Salas de ventas
--		3. Huespedes
--		4. Segmentos por agencia
--		5. Segmentos por Lead Source

-- =============================================
--			MODIFICACIONES DE TABLAS
-- =============================================
-- Tabla:			Recibos de regalos
-- [lormartinez]	12/Ago/2015 Renombramos el campo grAmountPaid por grAmountToPay
-- =============================================

-- renombramos el campo grAmountPaid por grAmountToPay
exec sp_rename 'dbo.GiftsReceipts.grAmountPaid', 'grAmountToPay', 'column'
GO

-- =============================================
-- Tabla:		Salas de ventas
-- [lmartinez]	13/Jul/2016 Agregue el campo de zona
-- =============================================
ALTER TABLE dbo.SalesRooms
ADD srzn varchar(10) null
GO

-- Llaves foraneas - Salas de ventas - Zonas
ALTER TABLE dbo.SalesRooms
	ADD CONSTRAINT FK_SalesRooms_Zones FOREIGN KEY(srzn)
	REFERENCES dbo.Zones(znID)
GO

-- Descripciones
exec dbo.USP_OR_AddColumnDescription 'dbo', 'SalesRooms', 'srzn', 'Enlace con la tabla de zonas'
GO

-- =============================================
-- Tabla:		Huespedes
-- [lchairez]	02/Ago/2016 Agregue el campo guNotifiedEmailShowNotInvited
-- =============================================
ALTER TABLE Guests 
	ADD guNotifiedEmailShowNotInvited BIT DEFAULT 0
GO

-- Descripciones
exec sp_addextendedproperty N'MS_Description', N'Indica si se envió notificación de presentación sin invitacion', N'user', N'dbo', N'table', N'Guests', N'column', N'guNotifiedEmailShowNotInvited'
GO

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