if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InvitsTexts]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[InvitsTexts]
GO

CREATE TABLE [dbo].[InvitsTexts] (
	[itls] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[itla] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[itRTF] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[itRTFHeader] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[itRTFFooter] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

