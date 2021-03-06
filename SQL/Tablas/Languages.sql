USE [OrigosVCPalace]
GO
/****** Objeto:  Table [dbo].[Languages]    Fecha de la secuencia de comandos: 05/17/2012 10:46:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Languages]') AND type in (N'U'))
DROP TABLE [dbo].[Languages]
GO
USE [OrigosVCPalace]
GO
/****** Objeto:  Table [dbo].[Languages]    Fecha de la secuencia de comandos: 05/17/2012 10:46:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Languages](
	[laID] [varchar](2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[laN] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[laA] [bit] NOT NULL CONSTRAINT [DF_Langs_laA]  DEFAULT (1),
	[laMrMrs] [nvarchar](15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Languages_laMrMrs]  DEFAULT (N'Mr. & Mrs.'),
	[laRoom] [nvarchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Languages_laRoom]  DEFAULT (N'Room'),
	[laMember] [nvarchar](15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT (''),
	[laDate] [nvarchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT (''),
	[laTime] [nvarchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT (''),
	[laName] [nvarchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT (''),
	[laMaritalSt] [nvarchar](15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT (''),
	[laAge] [nvarchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT (''),
	[laOccupation] [nvarchar](15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT (''),
	[laCountry] [nvarchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT (''),
	[laAgency] [nvarchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT (''),
	[laHotel] [nvarchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT (''),
	[laPax] [nvarchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT (''),
	[laDesposit] [nvarchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT (''),
	[laGift] [nvarchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT (''),
	[laLocation] [nvarchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT (''),
	[laPR] [nvarchar](15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT (''),
	[laExtraInfo] [nvarchar](25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL DEFAULT (''),
 CONSTRAINT [PK_Langs] PRIMARY KEY CLUSTERED 
(
	[laID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF