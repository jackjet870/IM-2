if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PRNotes]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PRNotes]
GO

CREATE TABLE [dbo].[PRNotes] (
	[pngu] [int] NOT NULL ,
	[pnDT] [datetime] NOT NULL ,
	[pnPR] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[pnText] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[PRNotes] WITH NOCHECK ADD 
	CONSTRAINT [PK_PRNotes] PRIMARY KEY  CLUSTERED 
	(
		[pngu],
		[pnDT]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[PRNotes] ADD 
	CONSTRAINT [FK_PRNotes_Guests] FOREIGN KEY 
	(
		[pngu]
	) REFERENCES [dbo].[Guests] (
		[guID]
	),
	CONSTRAINT [FK_PRNotes_Personnel] FOREIGN KEY 
	(
		[pnPR]
	) REFERENCES [dbo].[Personnel] (
		[peID]
	)
GO


exec sp_addextendedproperty N'MS_Description', N'Fecha y hora', N'user', N'dbo', N'table', N'PRNotes', N'column', N'pnDT'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del huésped', N'user', N'dbo', N'table', N'PRNotes', N'column', N'pngu'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del PR', N'user', N'dbo', N'table', N'PRNotes', N'column', N'pnPR'
GO
exec sp_addextendedproperty N'MS_Description', N'Texto de la nota', N'user', N'dbo', N'table', N'PRNotes', N'column', N'pnText'


GO

