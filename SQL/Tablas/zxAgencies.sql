if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[zxAgencies]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[zxAgencies]
GO

CREATE TABLE [dbo].[zxAgencies] (
	[agtID] [varchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

