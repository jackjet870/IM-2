if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[MailOutTexts]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[MailOutTexts]
GO

CREATE TABLE [dbo].[MailOutTexts] (
	[mtls] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[mtmoCode] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[mtla] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[mtRTF] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[mtA] [bit] NOT NULL ,
	[mtU] [datetime] NOT NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

