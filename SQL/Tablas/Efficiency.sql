if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_EfficiencySalesmen_Efficiency]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[EfficiencySalesmen] DROP CONSTRAINT FK_EfficiencySalesmen_Efficiency
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Efficiency]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Efficiency]
GO

CREATE TABLE [dbo].[Efficiency] (
	[efID] [bigint] IDENTITY (1, 1) NOT NULL ,
	[efsr] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[efDateFrom] [datetime] NOT NULL ,
	[efDateTo] [datetime] NOT NULL ,
	[efet] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[efpd] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Efficiency] WITH NOCHECK ADD 
	CONSTRAINT [PK_Efficiency] PRIMARY KEY  CLUSTERED 
	(
		[efID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Efficiency] ADD 
	CONSTRAINT [FK_Efficiency_EfficiencyTypes] FOREIGN KEY 
	(
		[efet]
	) REFERENCES [dbo].[EfficiencyTypes] (
		[etID]
	),
	CONSTRAINT [FK_Efficiency_Periods] FOREIGN KEY 
	(
		[efpd]
	) REFERENCES [dbo].[Periods] (
		[pdID]
	),
	CONSTRAINT [FK_Efficiency_SalesRooms] FOREIGN KEY 
	(
		[efsr]
	) REFERENCES [dbo].[SalesRooms] (
		[srID]
	)
GO


exec sp_addextendedproperty N'MS_Description', N'Fecha desde', N'user', N'dbo', N'table', N'Efficiency', N'column', N'efDateFrom'
GO
exec sp_addextendedproperty N'MS_Description', N'Fecha hasta', N'user', N'dbo', N'table', N'Efficiency', N'column', N'efDateTo'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del tipo de eficiencia', N'user', N'dbo', N'table', N'Efficiency', N'column', N'efet'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave', N'user', N'dbo', N'table', N'Efficiency', N'column', N'efID'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del periodo de eficiencia', N'user', N'dbo', N'table', N'Efficiency', N'column', N'efpd'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave de la sala de ventas', N'user', N'dbo', N'table', N'Efficiency', N'column', N'efsr'


GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EfficiencySalesmen]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[EfficiencySalesmen] ADD 
	CONSTRAINT [FK_EfficiencySalesmen_Efficiency] FOREIGN KEY 
	(
		[esef]
	) REFERENCES [dbo].[Efficiency] (
		[efID]
	)
GO
