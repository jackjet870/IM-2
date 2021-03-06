if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EfficiencySalesmen]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[EfficiencySalesmen]
GO

CREATE TABLE [dbo].[EfficiencySalesmen] (
	[esef] [bigint] NOT NULL ,
	[espe] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[EfficiencySalesmen] WITH NOCHECK ADD 
	CONSTRAINT [PK_EfficiencySalesmen] PRIMARY KEY  CLUSTERED 
	(
		[esef],
		[espe]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[EfficiencySalesmen] ADD 
	CONSTRAINT [FK_EfficiencySalesmen_Efficiency] FOREIGN KEY 
	(
		[esef]
	) REFERENCES [dbo].[Efficiency] (
		[efID]
	),
	CONSTRAINT [FK_EfficiencySalesmen_Personnel] FOREIGN KEY 
	(
		[espe]
	) REFERENCES [dbo].[Personnel] (
		[peID]
	)
GO


exec sp_addextendedproperty N'MS_Description', N'Clave de la eficiencia', N'user', N'dbo', N'table', N'EfficiencySalesmen', N'column', N'esef'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del personal', N'user', N'dbo', N'table', N'EfficiencySalesmen', N'column', N'espe'


GO

