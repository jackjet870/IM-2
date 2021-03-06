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
--		1. SalesmenChanges
--		2. GuestLog
--		3. SalesLog

-- =============================================
--			MODIFICACIONES DE TABLAS
-- =============================================
-- Tabla:			SalesmenChanges
-- [edgrodriguez]	11/Oct/2016 Se elimina la llave principal.
--								Se modifica la columna schsa para que acepte Nulos.
--								Se agregan los campos schID(PK),schgu(FK con Guest),schmt(FK con GuestMovementsType)
-- =============================================

-- Eliminamos la llave primaria
ALTER TABLE [dbo].[SalesmenChanges]
DROP CONSTRAINT PK_SalesmenChanges
GO

-- Modificamos la columna ID de venta
ALTER TABLE [dbo].[SalesmenChanges]
ALTER COLUMN [schsa] int NULL
GO

-- Agregamos las nuevas columnas
ALTER TABLE [dbo].[SalesmenChanges]
ADD [schID] INT IDENTITY,
	[schgu]int NULL,
	[schmt] varchar(10) NOT NULL DEFAULT 'SL'
GO

-- Agregamos las descripciones
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave Primaria de la tabla' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SalesmenChanges', @level2type=N'COLUMN',@level2name=N'schID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'GuestID a quién se le hizo el cambio' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SalesmenChanges', @level2type=N'COLUMN',@level2name=N'schgu'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del tipo de movimiento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SalesmenChanges', @level2type=N'COLUMN',@level2name=N'schmt'
GO

-- Agregamos la llave primaria
ALTER TABLE [dbo].[SalesmenChanges] 
ADD CONSTRAINT PK_SalesmenChanges
PRIMARY KEY(schID)
GO

-- Llaves foraneas - Cambios de vendedores - Tipos de movimientos de huespedes
ALTER TABLE [dbo].[SalesmenChanges] WITH CHECK ADD
CONSTRAINT [FK_SalesmenChanges_GuestMovementsType] FOREIGN KEY([schmt])
REFERENCES [dbo].[GuestsMovementsTypes] ([gnID])
GO

-- Llaves foraneas - Cambios de vendedores - Huespedes
ALTER TABLE [dbo].[SalesmenChanges] WITH CHECK ADD
CONSTRAINT [FK_SalesmenChanges_Guests] FOREIGN KEY([schgu])
REFERENCES [dbo].[Guests] ([guID])
GO
ALTER TABLE [dbo].[SalesmenChanges]
WITH CHECK ADD
CONSTRAINT [FK_SalesmenChanges_Guests] FOREIGN KEY([schgu])
REFERENCES [dbo].[Guests] ([guID])

-- =============================================
 --Tabla:			GuestLog
-- [emoguel]		10/10/2016 Agregue las columnas Liner3,Closer4,Exit3,FTB1,FTB2,FTM1,FTM2
-- =============================================

ALTER TABLE GuestLog ADD
glLiner3 VARCHAR(10) NULL,
glCloser4 VARCHAR(10)NULL,
glExit3 VARCHAR(10) NULL,
glFTB1 VARCHAR(10) NULL,
glFTB2 VARCHAR(10) NULL,
glFTM1 VARCHAR(10) NULL,
glFTM2 VARCHAR(10) NULL
GO
  
-- Llaves foraneas - Huesped Log - Personal - Liner3
ALTER TABLE GuestLog ADD
	CONSTRAINT FK_GuestLog_Personnel_Liner3 FOREIGN KEY(glLiner3)REFERENCES Personnel(peID)
GO

-- Llaves foraneas - Huesped Log - Personal - Closer4
ALTER TABLE GuestLog ADD
	CONSTRAINT FK_GuestLog_Personnel_Closer4 FOREIGN KEY(glCloser4)REFERENCES Personnel(peID)
GO


-- Llaves foraneas - Huesped Log - Personal - ExitCloser3
ALTER TABLE GuestLog ADD
	CONSTRAINT FK_GuestLog_Personnel_ExitCloser3 FOREIGN KEY(glExit3)REFERENCES Personnel(peID)
GO

-- Llaves foraneas - Huesped Log - Personal - Front to Back1
ALTER TABLE GuestLog ADD
	CONSTRAINT FK_GuestLog_Personnel_FrontToBack1 FOREIGN KEY(glFTB1)REFERENCES Personnel(peID)
GO

-- Llaves foraneas - Huesped Log - Personal - Front to Back2
ALTER TABLE GuestLog ADD
	CONSTRAINT FK_GuestLog_Personnel_FrontToBack2 FOREIGN KEY(glFTB2)REFERENCES Personnel(peID)
GO

-- Llaves foraneas - Huesped Log - Personal - Front to Middle1
ALTER TABLE GuestLog ADD
	CONSTRAINT FK_GuestLog_Personnel_FrontToMiddle1 FOREIGN KEY(glFTM1)REFERENCES Personnel(peID)
GO

-- Llaves foraneas - Huesped Log - Personal - Front to Middle2
ALTER TABLE GuestLog ADD
	CONSTRAINT FK_GuestLog_Personnel_FrontToMiddle2 FOREIGN KEY(glFTM2)REFERENCES Personnel(peID)	
GO

--Descripciones
--Liner 3
exec sp_addextendedproperty N'MS_Description', N'Clave del 3er Liner', N'user', N'dbo', N'table', N'GuestLog', N'column', N'glLiner3'
GO

--Closer 3
exec sp_addextendedproperty N'MS_Description', N'Clave del 4to Closer', N'user', N'dbo', N'table', N'GuestLog', N'column', N'glCloser4'
GO

--Exit Closer 3
exec sp_addextendedproperty N'MS_Description', N'Clave del 3er Exit Closer', N'user', N'dbo', N'table', N'GuestLog', N'column', N'glExit3'
GO

--Front To Back 
exec sp_addextendedproperty N'MS_Description', N'Clave del Front To Back', N'user', N'dbo', N'table', N'GuestLog', N'column', N'glFTB1'
GO

--Front To Back 2
exec sp_addextendedproperty N'MS_Description', N'Clave del 2do Front To Back', N'user', N'dbo', N'table', N'GuestLog', N'column', N'glFTB2'
GO

--Front To Middle
exec sp_addextendedproperty N'MS_Description', N'Clave del Front To Middle', N'user', N'dbo', N'table', N'GuestLog', N'column', N'glFTM1'
GO

--Front To Middle 2
exec sp_addextendedproperty N'MS_Description', N'Clave del 2do Front To Middle', N'user', N'dbo', N'table', N'GuestLog', N'column', N'glFTM2'
GO 
-- =============================================
-- Tabla:		BookingDeposits
-- [wtorres]	12/Oct/2016 Modifique el campo de numero de tarjeta de credito para que sea varchar
-- =============================================
ALTER TABLE BookingDeposits
	ALTER COLUMN bdCardNum varchar(4) NULL
GO

-- Tabla:			SalesLog
-- [erosado]		13/10/2016 Agregué las columnas Liner3,Closer4,Exit3,FTB1,FTB2,FTM1,FTM2
-- =============================================

ALTER TABLE SalesLog ADD
slLiner3 VARCHAR(10) NULL,
slCloser4 VARCHAR(10)NULL,
slExit3 VARCHAR(10) NULL,
slFTB1 VARCHAR(10) NULL,
slFTB2 VARCHAR(10) NULL,
slFTM1 VARCHAR(10) NULL,
slFTM2 VARCHAR(10) NULL
GO

-- Llaves foraneas - Venta Log - Personal - Liner3
ALTER TABLE SalesLog ADD
	CONSTRAINT FK_SalesLog_Personnel_Liner3 FOREIGN KEY(slLiner3)REFERENCES Personnel(peID)
GO

-- Llaves foraneas - Venta Log - Personal - Closer4
ALTER TABLE SalesLog ADD
	CONSTRAINT FK_SalesLog_Personnel_Closer4 FOREIGN KEY(slCloser4)REFERENCES Personnel(peID)
GO

-- Llaves foraneas - Venta Log - Personal - ExitCloser3
ALTER TABLE SalesLog ADD
	CONSTRAINT FK_SalesLog_Personnel_ExitCloser3 FOREIGN KEY(slExit3)REFERENCES Personnel(peID)
GO

-- Llaves foraneas - Venta Log - Personal - Front to Back1
ALTER TABLE SalesLog ADD
	CONSTRAINT FK_SalesLog_Personnel_FrontToBack1 FOREIGN KEY(slFTB1)REFERENCES Personnel(peID)
GO

-- Llaves foraneas - Venta Log - Personal - Front to Back2
ALTER TABLE SalesLog ADD
	CONSTRAINT FK_SalesLog_Personnel_FrontToBack2 FOREIGN KEY(slFTB2)REFERENCES Personnel(peID)
GO

-- Llaves foraneas - Venta Log - Personal - Front to Middle1
ALTER TABLE SalesLog ADD
	CONSTRAINT FK_SalesLog_Personnel_FrontToMiddle1 FOREIGN KEY(slFTM1)REFERENCES Personnel(peID)
GO

-- Llaves foraneas - Venta Log - Personal - Front to Middle2
ALTER TABLE SalesLog ADD
	CONSTRAINT FK_SalesLog_Personnel_FrontToMiddle2 FOREIGN KEY(slFTM2)REFERENCES Personnel(peID)	
GO

--Descripciones
--Liner 3
exec sp_addextendedproperty N'MS_Description', N'Clave del 3er Liner', N'user', N'dbo', N'table', N'SalesLog', N'column', N'slLiner3'
GO

--Closer 3
exec sp_addextendedproperty N'MS_Description', N'Clave del 4to Closer', N'user', N'dbo', N'table', N'SalesLog', N'column', N'slCloser4'
GO

--Exit Closer 3
exec sp_addextendedproperty N'MS_Description', N'Clave del 3er Exit Closer', N'user', N'dbo', N'table', N'SalesLog', N'column', N'slExit3'
GO

--Front To Back 
exec sp_addextendedproperty N'MS_Description', N'Clave del Front To Back', N'user', N'dbo', N'table', N'SalesLog', N'column', N'slFTB1'
GO

--Front To Back 2
exec sp_addextendedproperty N'MS_Description', N'Clave del 2do Front To Back', N'user', N'dbo', N'table', N'SalesLog', N'column', N'slFTB2'
GO

--Front To Middle
exec sp_addextendedproperty N'MS_Description', N'Clave del Front To Middle', N'user', N'dbo', N'table', N'SalesLog', N'column', N'slFTM1'
GO

--Front To Middle 2
exec sp_addextendedproperty N'MS_Description', N'Clave del 2do Front To Middle 2', N'user', N'dbo', N'table', N'SalesLog', N'column', N'slFTM2'
GO