if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GuestsGroupsIntegrants]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[GuestsGroupsIntegrants]
GO

CREATE TABLE [dbo].[GuestsGroupsIntegrants] (
	[gjgx] [int] NOT NULL ,
	[gjgu] [int] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[GuestsGroupsIntegrants] WITH NOCHECK ADD 
	CONSTRAINT [PK_GuestsGroupsIntegrants] PRIMARY KEY  CLUSTERED 
	(
		[gjgx],
		[gjgu]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[GuestsGroupsIntegrants] ADD 
	CONSTRAINT [FK_GuestsGroupsIntegrants_Guests] FOREIGN KEY 
	(
		[gjgu]
	) REFERENCES [dbo].[Guests] (
		[guID]
	),
	CONSTRAINT [FK_GuestsGroupsIntegrants_GuestsGroups] FOREIGN KEY 
	(
		[gjgx]
	) REFERENCES [dbo].[GuestsGroups] (
		[gxID]
	)
GO


exec sp_addextendedproperty N'MS_Description', N'Clave del huésped', N'user', N'dbo', N'table', N'GuestsGroupsIntegrants', N'column', N'gjgu'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del grupo', N'user', N'dbo', N'table', N'GuestsGroupsIntegrants', N'column', N'gjgx'


GO

