if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LoginsLog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[LoginsLog]
GO

CREATE TABLE [dbo].[LoginsLog] (
	[llID] [datetime] NOT NULL ,
	[lllo] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[llpe] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[llPCName] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

