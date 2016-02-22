/****** Object: Table [dbo].[DepositsRefund]   Script Date: 30/12/2015 09:31:01 a.m. ******/
USE [OrigosVCPalace];
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[DepositsRefund] (
[drID] int IDENTITY(1, 1) NOT NULL,
[drFolio] int NOT NULL,
[drD] datetime NOT NULL,
[drgu] int NOT NULL)
ON [PRIMARY];
GO

