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
-- [wtorres]	24/Jun/2016 Agregue las columnas de Liner 3, Front To Middles, Closer 4 y Exit Closer 3
-- =============================================
ALTER TABLE Guests ADD
	guLiner3 VARCHAR(10) NULL,
	guFTM1 VARCHAR(10) NULL,
	guFTM2 VARCHAR(10) NULL,
	guFTB1 VARCHAR(10) NULL,
	guFTB2 VARCHAR(10) NULL,
	guCloser4 VARCHAR(10) NULL,
	guExit3 VARCHAR(10) NULL
GO

-- Llaves foraneas - Huespedes - Personal - Liner 3
ALTER TABLE Guests
	ADD CONSTRAINT FK_Guests_Personnel_Liner3 FOREIGN KEY(guLiner3)
	REFERENCES Personnel(peID)
GO

-- Llaves foraneas - Huespedes - Personal - Front To Middle 1
ALTER TABLE Guests
	ADD CONSTRAINT FK_Guests_Personnel_FrontToMiddle1 FOREIGN KEY(guFTM1)
	REFERENCES Personnel(peID)
GO

-- Llaves foraneas - Huespedes - Personal - Front To Middle 2
ALTER TABLE Guests
	ADD CONSTRAINT FK_Guests_Personnel_FrontToMiddle2 FOREIGN KEY(guFTM2)
	REFERENCES Personnel(peID)
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

-- Llaves foraneas - Huespedes - Personal - Closer 4
ALTER TABLE Guests
	ADD CONSTRAINT FK_Guests_Personnel_Closer4 FOREIGN KEY(guCloser4)
	REFERENCES Personnel(peID)
GO

-- Llaves foraneas - Huespedes - Personal - Exit Closer 3
ALTER TABLE Guests
	ADD CONSTRAINT FK_Guests_Personnel_Exit3 FOREIGN KEY(guExit3)
	REFERENCES Personnel(peID)
GO

-- Descripciones
exec sp_addextendedproperty N'MS_Description', N'Clave del 3er Liner', N'user', N'dbo', N'table', N'Guests', N'column', N'guLiner3'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del Front To Middle', N'user', N'dbo', N'table', N'Guests', N'column', N'guFTM1'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del 2do Front To Middle', N'user', N'dbo', N'table', N'Guests', N'column', N'guFTM2'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del Front To Back', N'user', N'dbo', N'table', N'Guests', N'column', N'guFTB1'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del 2do Front To Back', N'user', N'dbo', N'table', N'Guests', N'column', N'guFTB2'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del 4to Closer', N'user', N'dbo', N'table', N'Guests', N'column', N'guCloser4'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del 3er Exit Closer', N'user', N'dbo', N'table', N'Guests', N'column', N'guExit3'
GO

-- =============================================
-- Tabla:		Ventas
-- [wtorres]	10/May/2016 Agregue las columnas de Front To Backs
-- [wtorres]	24/Jun/2016 Agregue las columnas de Liner 3, Front To Middles, Closer 4 y Exit Closer 3
-- =============================================
ALTER TABLE Sales ADD
	saLiner3 VARCHAR(10) NULL,
	saFTM1 VARCHAR(10) NULL,
	saFTM2 VARCHAR(10) NULL,
	saFTB1 VARCHAR(10) NULL,
	saFTB2 VARCHAR(10) NULL,
	saCloser4 VARCHAR(10) NULL,
	saExit3 VARCHAR(10) NULL
GO

-- Llaves foraneas - Ventas - Personal - Liner 3
ALTER TABLE Sales
	ADD CONSTRAINT FK_Sales_Personnel_Liner3 FOREIGN KEY(saLiner3)
	REFERENCES Personnel(peID)
GO

-- Llaves foraneas - Ventas - Personal - Front To Middle 1
ALTER TABLE Sales
	ADD CONSTRAINT FK_Sales_Personnel_FrontToMiddle1 FOREIGN KEY(saFTM1)
	REFERENCES Personnel(peID)
GO

-- Llaves foraneas - Ventas - Personal - Front To Middle 2
ALTER TABLE Sales
	ADD CONSTRAINT FK_Sales_Personnel_FrontToMiddle2 FOREIGN KEY(saFTM2)
	REFERENCES Personnel(peID)
GO

-- Llaves foraneas - Ventas - Personal - Front To Back 1
ALTER TABLE Sales
	ADD CONSTRAINT FK_Sales_Personnel_FrontToBack1 FOREIGN KEY(saFTB1)
	REFERENCES Personnel(peID)
GO

-- Llaves foraneas - Ventas - Personal - Front To Back 2
ALTER TABLE Sales
	ADD CONSTRAINT FK_Sales_Personnel_FrontToBack2 FOREIGN KEY(saFTB2)
	REFERENCES Personnel(peID)
GO

-- Llaves foraneas - Ventas - Personal - Closer 4
ALTER TABLE Sales
	ADD CONSTRAINT FK_Sales_Personnel_Closer4 FOREIGN KEY(saCloser4)
	REFERENCES Personnel(peID)
GO

-- Llaves foraneas - Ventas - Personal - Exit Closer 3
ALTER TABLE Sales
	ADD CONSTRAINT FK_Sales_Personnel_Exit3 FOREIGN KEY(saExit3)
	REFERENCES Personnel(peID)
GO

-- Descripciones
exec sp_addextendedproperty N'MS_Description', N'Clave del 3er Liner', N'user', N'dbo', N'table', N'Sales', N'column', N'saLiner3'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del Front To Middle', N'user', N'dbo', N'table', N'Sales', N'column', N'saFTM1'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del 2do Front To Middle', N'user', N'dbo', N'table', N'Sales', N'column', N'saFTM2'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del Front To Back', N'user', N'dbo', N'table', N'Sales', N'column', N'saFTB1'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del 2do Front To Back', N'user', N'dbo', N'table', N'Sales', N'column', N'saFTB2'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del 4to Closer', N'user', N'dbo', N'table', N'Sales', N'column', N'saCloser4'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del 3er Exit Closer', N'user', N'dbo', N'table', N'Sales', N'column', N'saExit3'
GO