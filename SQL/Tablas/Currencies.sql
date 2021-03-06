if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_GiftsReceiptsPayments_Currencies]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[GiftsReceiptsPayments] DROP CONSTRAINT FK_GiftsReceiptsPayments_Currencies
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Currencies]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Currencies]
GO

CREATE TABLE [dbo].[Currencies] (
	[cuID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[cuN] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[cuA] [bit] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Currencies] WITH NOCHECK ADD 
	CONSTRAINT [PK_Curs] PRIMARY KEY  CLUSTERED 
	(
		[cuID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Currencies] ADD 
	CONSTRAINT [DF_Curs_cuA] DEFAULT (1) FOR [cuA]
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GiftsReceiptsPayments]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[GiftsReceiptsPayments] ADD 
	CONSTRAINT [FK_GiftsReceiptsPayments_Currencies] FOREIGN KEY 
	(
		[gycu]
	) REFERENCES [dbo].[Currencies] (
		[cuID]
	)
GO
