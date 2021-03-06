if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SeasonsDates]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[SeasonsDates]
GO

CREATE TABLE [dbo].[SeasonsDates] (
	[sdss] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[sdStartD] [datetime] NOT NULL ,
	[sdEndD] [datetime] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SeasonsDates] WITH NOCHECK ADD 
	CONSTRAINT [PK_SeasonsDates] PRIMARY KEY  CLUSTERED 
	(
		[sdss],
		[sdStartD],
		[sdEndD]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[SeasonsDates] ADD 
	CONSTRAINT [FK_SeasonsDates_Seasons] FOREIGN KEY 
	(
		[sdss]
	) REFERENCES [dbo].[Seasons] (
		[ssID]
	)
GO


exec sp_addextendedproperty N'MS_Description', N'Fecha hasta', N'user', N'dbo', N'table', N'SeasonsDates', N'column', N'sdEndD'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave de la temporada', N'user', N'dbo', N'table', N'SeasonsDates', N'column', N'sdss'
GO
exec sp_addextendedproperty N'MS_Description', N'Fecha desde', N'user', N'dbo', N'table', N'SeasonsDates', N'column', N'sdStartD'


GO

