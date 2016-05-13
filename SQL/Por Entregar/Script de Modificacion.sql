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
--		1. Huespedes
--		2. Ventas

-- =============================================
--			MODIFICACIONES DE TABLAS
-- =============================================
-- Tabla:		Huespedes
-- [wtorres]	10/May/2016 Agregue las columnas de Front To Backs
-- =============================================
ALTER TABLE Guests ADD
	guFTB1 VARCHAR(10) NULL,
	guFTB2 VARCHAR(10) NULL,
	guFTB3 VARCHAR(10) NULL
GO

-- Llaves foraneas - Huespedes - Personal - Front To Back 1
ALTER TABLE Guests
	ADD CONSTRAINT FK_Guests_Personnel_FrontToBack1 FOREIGN KEY(guFTB1)
	REFERENCES Personnel(peID)
GO

-- Llaves foraneas - Huespedes - Personal - Front To Back 2
ALTER TABLE Guests
	ADD CONSTRAINT FK_Guests_Personnel_FrontToBack2 FOREIGN KEY(guFTB2)
	REFERENCES Personnel(peID)
GO

-- Llaves foraneas - Huespedes - Personal - Front To Back 3
ALTER TABLE Guests
	ADD CONSTRAINT FK_Guests_Personnel_FrontToBack3 FOREIGN KEY(guFTB3)
	REFERENCES Personnel(peID)
GO

-- Descripciones
exec sp_addextendedproperty N'MS_Description', N'Clave del Front To Back', N'user', N'dbo', N'table', N'Guests', N'column', N'guFTB1'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del 2do Front To Back', N'user', N'dbo', N'table', N'Guests', N'column', N'guFTB2'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del 3er Front To Back', N'user', N'dbo', N'table', N'Guests', N'column', N'guFTB3'
GO

-- =============================================
-- Tabla:		Ventas
-- [wtorres]	10/May/2016 Agregue las columnas de Front To Backs
-- =============================================
ALTER TABLE Sales ADD
	saFTB1 VARCHAR(10) NULL,
	saFTB2 VARCHAR(10) NULL,
	saFTB3 VARCHAR(10) NULL
GO

-- Llaves foraneas - Huespedes - Personal - Front To Back 1
ALTER TABLE Sales
	ADD CONSTRAINT FK_Sales_Personnel_FrontToBack1 FOREIGN KEY(saFTB1)
	REFERENCES Personnel(peID)
GO

-- Llaves foraneas - Huespedes - Personal - Front To Back 2
ALTER TABLE Sales
	ADD CONSTRAINT FK_Sales_Personnel_FrontToBack2 FOREIGN KEY(saFTB2)
	REFERENCES Personnel(peID)
GO

-- Llaves foraneas - Huespedes - Personal - Front To Back 3
ALTER TABLE Sales
	ADD CONSTRAINT FK_Sales_Personnel_FrontToBack3 FOREIGN KEY(saFTB3)
	REFERENCES Personnel(peID)
GO

-- Descripciones
exec sp_addextendedproperty N'MS_Description', N'Clave del Front To Back', N'user', N'dbo', N'table', N'Sales', N'column', N'saFTB1'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del 2do Front To Back', N'user', N'dbo', N'table', N'Sales', N'column', N'saFTB2'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del 3er Front To Back', N'user', N'dbo', N'table', N'Sales', N'column', N'saFTB3'
GO