if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FieldsByGrid]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[FieldsByGrid]
GO

CREATE TABLE [dbo].[FieldsByGrid] (
	[fgGrid] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[fgColPosition] [smallint] NOT NULL ,
	[fgFieldName] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[fgHeading] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[fgToolTipText] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[fgWidth] [int] NOT NULL ,
	[fgVisible] [tinyint] NOT NULL 
) ON [PRIMARY]
GO

