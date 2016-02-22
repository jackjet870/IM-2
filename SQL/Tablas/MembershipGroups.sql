if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[MembershipGroups]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[MembershipGroups]
GO

CREATE TABLE [dbo].[MembershipGroups] (
	[mgID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[mgN] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[mgA] [bit] NOT NULL 
) ON [PRIMARY]
GO

