if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EXEVersions]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[EXEVersions]
GO

CREATE TABLE [dbo].[EXEVersions] (
	[evEXEName] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[evDepFile] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[evMajor] [int] NULL ,
	[evMinor] [int] NULL ,
	[evRevision] [int] NULL ,
	[evDate] [datetime] NULL 
) ON [PRIMARY]
GO

