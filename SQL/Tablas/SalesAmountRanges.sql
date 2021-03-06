if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SalesAmountRanges]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[SalesAmountRanges]
GO

CREATE TABLE [dbo].[SalesAmountRanges] (
	[snID] [int] IDENTITY (1, 1) NOT NULL ,
	[snN] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[snFrom] [money] NOT NULL ,
	[snTo] [money] NOT NULL ,
	[snA] [bit] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SalesAmountRanges] WITH NOCHECK ADD 
	CONSTRAINT [PK_SalesAmountRanges] PRIMARY KEY  CLUSTERED 
	(
		[snID]
	)  ON [PRIMARY] 
GO


exec sp_addextendedproperty N'MS_Description', N'Clave', N'user', N'dbo', N'table', N'SalesAmountRanges', N'column', N'snID'
GO
exec sp_addextendedproperty N'MS_Description', N'Descripción', N'user', N'dbo', N'table', N'SalesAmountRanges', N'column', N'snN'
GO
exec sp_addextendedproperty N'MS_Description', N'Monto desde', N'user', N'dbo', N'table', N'SalesAmountRanges', N'column', N'snFrom'
GO
exec sp_addextendedproperty N'MS_Description', N'Monto hasta', N'user', N'dbo', N'table', N'SalesAmountRanges', N'column', N'snTo'
GO
exec sp_addextendedproperty N'MS_Description', N'Activo', N'user', N'dbo', N'table', N'SalesAmountRanges', N'column', N'snA'


GO

