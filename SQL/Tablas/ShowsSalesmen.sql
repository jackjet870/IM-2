if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ShowsSalesmen]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ShowsSalesmen]
GO

CREATE TABLE [dbo].[ShowsSalesmen] (
	[shgu] [int] NOT NULL ,
	[shpe] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[shUp] [bit] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ShowsSalesmen] WITH NOCHECK ADD 
	CONSTRAINT [PK_ShowsSalesmen] PRIMARY KEY  CLUSTERED 
	(
		[shgu],
		[shpe]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ShowsSalesmen] WITH NOCHECK ADD 
	CONSTRAINT [DF_ShowsSalesmen_shUp] DEFAULT (1) FOR [shUp]
GO

ALTER TABLE [dbo].[ShowsSalesmen] ADD 
	CONSTRAINT [FK_ShowsSalesmen_Guests] FOREIGN KEY 
	(
		[shgu]
	) REFERENCES [dbo].[Guests] (
		[guID]
	),
	CONSTRAINT [FK_ShowsSalesmen_Personnel] FOREIGN KEY 
	(
		[shpe]
	) REFERENCES [dbo].[Personnel] (
		[peID]
	)
GO


exec sp_addextendedproperty N'MS_Description', N'Clave de la invitación', N'user', N'dbo', N'table', N'ShowsSalesmen', N'column', N'shgu'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del personal', N'user', N'dbo', N'table', N'ShowsSalesmen', N'column', N'shpe'
GO
exec sp_addextendedproperty N'MS_Description', N'Indica si se debe de tomar en cuenta la pareja en las estadísticas del vendedor', N'user', N'dbo', N'table', N'ShowsSalesmen', N'column', N'shUp'


GO

