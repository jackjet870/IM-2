USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[LeadSourcesByAgencies]    Script Date: 05/14/2014 11:30:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LeadSourcesByAgencies](
	[lgls] [varchar](10) NOT NULL,
	[lgag] [varchar](35) NOT NULL,
 CONSTRAINT [PK_LeadSourcesByAgencies] PRIMARY KEY CLUSTERED 
(
	[lgls] ASC,
	[lgag] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave Lead Source' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSourcesByAgencies', @level2type=N'COLUMN',@level2name=N'lgls'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la agencia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSourcesByAgencies', @level2type=N'COLUMN',@level2name=N'lgag'
GO

ALTER TABLE [dbo].[LeadSourcesByAgencies]  WITH CHECK ADD  CONSTRAINT [FK_LeadSourcesByAgencies_Agencies] FOREIGN KEY([lgag])
REFERENCES [dbo].[Agencies] ([agID])
GO

ALTER TABLE [dbo].[LeadSourcesByAgencies] CHECK CONSTRAINT [FK_LeadSourcesByAgencies_Agencies]
GO

ALTER TABLE [dbo].[LeadSourcesByAgencies]  WITH CHECK ADD  CONSTRAINT [FK_LeadSourcesByAgencies_LeadSources] FOREIGN KEY([lgls])
REFERENCES [dbo].[LeadSources] ([lsID])
GO

ALTER TABLE [dbo].[LeadSourcesByAgencies] CHECK CONSTRAINT [FK_LeadSourcesByAgencies_LeadSources]
GO


