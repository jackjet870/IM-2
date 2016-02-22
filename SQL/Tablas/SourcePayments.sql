if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SourcePayments]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[SourcePayments]
GO

CREATE TABLE [dbo].[SourcePayments] (
	[sbID] [varchar](10) NOT NULL,
	[sbN] [varchar](30) NOT NULL,
	[sbA] [bit] NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SourcePayments] WITH NOCHECK ADD 
	CONSTRAINT [PK_SourcePayments] PRIMARY KEY  CLUSTERED 
	(
		[sbID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 

GO