if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GuestsCreditCards]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[GuestsCreditCards]
GO

CREATE TABLE [dbo].[GuestsCreditCards] (
	[gdgu] [int] NOT NULL ,
	[gdQuantity] [tinyint] NOT NULL ,
	[gdcc] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

