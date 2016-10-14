USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[PersonnelLeadSourcesLog]    Script Date: 10/14/2016 10:07:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PersonnelLeadSourcesLog](
	[pllplg] [int] NOT NULL,
	[pllls] [varchar](10) NOT NULL,
 CONSTRAINT [PK_PersonnelLeadSourcesLog] PRIMARY KEY CLUSTERED 
(
	[pllplg] ASC,
	[pllls] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del Personnel Log' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PersonnelLeadSourcesLog', @level2type=N'COLUMN',@level2name=N'pllplg'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del Lead Source' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PersonnelLeadSourcesLog', @level2type=N'COLUMN',@level2name=N'pllls'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Historico de cabios de los Lead Source del personal' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PersonnelLeadSourcesLog'
GO

ALTER TABLE [dbo].[PersonnelLeadSourcesLog]  WITH CHECK ADD  CONSTRAINT [FK_PersonnelLeadSourcesLog_LeadSources] FOREIGN KEY([pllls])
REFERENCES [dbo].[LeadSources] ([lsID])
GO

ALTER TABLE [dbo].[PersonnelLeadSourcesLog] CHECK CONSTRAINT [FK_PersonnelLeadSourcesLog_LeadSources]
GO

ALTER TABLE [dbo].[PersonnelLeadSourcesLog]  WITH CHECK ADD  CONSTRAINT [FK_PersonnelLeadSourcesLog_PersonnelLog] FOREIGN KEY([pllplg])
REFERENCES [dbo].[PersonnelLog] ([plgID])
GO

ALTER TABLE [dbo].[PersonnelLeadSourcesLog] CHECK CONSTRAINT [FK_PersonnelLeadSourcesLog_PersonnelLog]
GO


