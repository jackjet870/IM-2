if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GuestsStatus]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[GuestsStatus]
GO

CREATE TABLE [dbo].[GuestsStatus] (
	[gtgu] [int] NOT NULL ,
	[gtQuantity] [tinyint] NOT NULL ,
	[gtgs] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

