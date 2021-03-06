if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RegComments]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[RegComments]
GO

CREATE TABLE [dbo].[RegComments] (
	[rcID] [int] IDENTITY (1, 1) NOT NULL ,
	[rcgu] [int] NOT NULL ,
	[rcDT] [datetime] NOT NULL ,
	[rcBy] [varchar] (10) COLLATE Modern_Spanish_CI_AS NOT NULL ,
	[rcComments] [nvarchar] (50) COLLATE Modern_Spanish_CI_AS NOT NULL 
) ON [PRIMARY]
GO

