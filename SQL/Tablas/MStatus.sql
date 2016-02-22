if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[MStatus]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[MStatus]
GO

CREATE TABLE [dbo].[MStatus] (
	[msID] [varchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[msN] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[msA] [bit] NOT NULL 
) ON [PRIMARY]
GO

