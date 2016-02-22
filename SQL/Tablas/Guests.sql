USE [OrigosVCPalace]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Guests_Agencies]') AND parent_object_id = OBJECT_ID(N'[dbo].[Guests]'))
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [FK_Guests_Agencies]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Guests_Clubs]') AND parent_object_id = OBJECT_ID(N'[dbo].[Guests]'))
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [FK_Guests_Clubs]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Guests_Countries]') AND parent_object_id = OBJECT_ID(N'[dbo].[Guests]'))
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [FK_Guests_Countries]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Guests_NotBookingMotives]') AND parent_object_id = OBJECT_ID(N'[dbo].[Guests]'))
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [FK_Guests_NotBookingMotives]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Guests_PaymentTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[Guests]'))
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [FK_Guests_PaymentTypes]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Guests_Personnel_PRFollow]') AND parent_object_id = OBJECT_ID(N'[dbo].[Guests]'))
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [FK_Guests_Personnel_PRFollow]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Guests_Personnel_PRNoBook]') AND parent_object_id = OBJECT_ID(N'[dbo].[Guests]'))
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [FK_Guests_Personnel_PRNoBook]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Guests_RoomTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[Guests]'))
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [FK_Guests_RoomTypes]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Guests_guOnGroup]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF_Guests_guOnGroup]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guChkIn__1E3A7A34]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guChkIn__1E3A7A34]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__gums1__1F2E9E6D]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__gums1__1F2E9E6D]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__gums2__2022C2A6]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__gums2__2022C2A6]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guPax__2116E6DF]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guPax__2116E6DF]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Guests_guag]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF_Guests_guag]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Guests_guco]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF_Guests_guco]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Guests_gumk]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF_Guests_gumk]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__gula__2AA05119]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__gula__2AA05119]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guLW__2B947552]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guLW__2B947552]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guRLW__2C88998B]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guRLW__2C88998B]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guAvail__220B0B18]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guAvail__220B0B18]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guInfo__23F3538A]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guInfo__23F3538A]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guInvit__24E777C3]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guInvit__24E777C3]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Guests_guInvit1]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF_Guests_guInvit1]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guum__22FF2F51]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guum__22FF2F51]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__gumoA__29AC2CE0]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__gumoA__29AC2CE0]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guQuinel__2D7CBDC4]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guQuinel__2D7CBDC4]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Guests_guResch]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF_Guests_guResch]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Guests_guBookCanc1]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF_Guests_guBookCanc1]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guBookCa__2E70E1FD]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guBookCa__2E70E1FD]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guDeposi__2F650636]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guDeposi__2F650636]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Guests_guDeposit1]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF_Guests_guDeposit1]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Guests_guDeposit1_1]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF_Guests_guDeposit1_1]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__gucu__30592A6F]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__gucu__30592A6F]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guTaxiIn__3335971A]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guTaxiIn__3335971A]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guTaxiOu__351DDF8C]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guTaxiOu__351DDF8C]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guShow__25DB9BFC]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guShow__25DB9BFC]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guQ__26CFC035]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guQ__26CFC035]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Guests_guNQ]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF_Guests_guNQ]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guCTour__39E294A9]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guCTour__39E294A9]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guInOut__37FA4C37]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guInOut__37FA4C37]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guWalkOu__38EE7070]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guWalkOu__38EE7070]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Guests_guMealTicket]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF_Guests_guMealTicket]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Guests_guMeelTicketQyt]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF_Guests_guMeelTicketQyt]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guSale__27C3E46E]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guSale__27C3E46E]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Guests_guSale1]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF_Guests_guSale1]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Guests_saLiner1Type]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF_Guests_saLiner1Type]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Guests_guInternetTransfer]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF_Guests_guInternetTransfer]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guTwiste__314D4EA8]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guTwiste__314D4EA8]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guDeposi__324172E1]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guDeposi__324172E1]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guTaxiIn__3429BB53]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guTaxiIn__3429BB53]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guTaxiOu__361203C5]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guTaxiOu__361203C5]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guTour__0F6D37F0]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guTour__0F6D37F0]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guShow2__10615C29]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guShow2__10615C29]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guQ2__11558062]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guQ2__11558062]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guNQ2__1249A49B]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guNQ2__1249A49B]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guCTour2__133DC8D4]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guCTour2__133DC8D4]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guDirect__1431ED0D]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guDirect__1431ED0D]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guQuinel__15261146]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guQuinel__15261146]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guTour2__161A357F]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guTour2__161A357F]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guInOut2__170E59B8]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guInOut2__170E59B8]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guWalkOu__18027DF1]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guWalkOu__18027DF1]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guDepSal__55FFB06A]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guDepSal__55FFB06A]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guAntesI__7E0DA1C4]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guAntesI__7E0DA1C4]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__guests__guRoomsQ__088B3037]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__guests__guRoomsQ__088B3037]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guChildr__18C19800]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guChildr__18C19800]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guQuinel__2062B9C8]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guQuinel__2062B9C8]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guOrigin__32816A03]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guOrigin__32816A03]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guDivRes__4D35603F]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guDivRes__4D35603F]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guShowsQ__613C58EC]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guShowsQ__613C58EC]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Guests__guFamily__67E9567B]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF__Guests__guFamily__67E9567B]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Guests_guOverflow]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF_Guests_guOverflow]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Guests_guIdentification]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF_Guests_guIdentification]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Guests_guQtyGiftsCard]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF_Guests_guQtyGiftsCard]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Guests_guIncludedTour]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF_Guests_guIncludedTour]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Guests_guFollow]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF_Guests_guFollow]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Guests_guGroup]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF_Guests_guGroup]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Guests_guPRNote]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF_Guests_guPRNote]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Guests_guCompany]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF_Guests_guCompany]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Guests_guSave]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF_Guests_guSave]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Guests_guWithQuinella]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF_Guests_guWithQuinella]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Guests_guIdProfileOpera]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF_Guests_guIdProfileOpera]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Guests_guAvailBySystem]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF_Guests_guAvailBySystem]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Guests_gupt]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT [DF_Guests_gupt]
END

GO

USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[Guests]    Script Date: 11/14/2013 09:54:12 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Guests]') AND type in (N'U'))
DROP TABLE [dbo].[Guests]
GO

USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[Guests]    Script Date: 11/14/2013 09:54:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Guests](
	[guID] [int] IDENTITY(1,1) NOT NULL,
	[guHReservID] [varchar](15) NULL,
	[guls] [varchar](10) NOT NULL,
	[guMembershipNum] [varchar](15) NULL,
	[guOnGroup] [bit] NOT NULL,
	[guHotel] [varchar](30) NULL,
	[guRoomNum] [varchar](15) NULL,
	[guCheckInD] [datetime] NOT NULL,
	[guCheckOutD] [datetime] NOT NULL,
	[guCheckIn] [bit] NOT NULL,
	[guLastName1] [varchar](65) NULL,
	[guFirstName1] [varchar](30) NULL,
	[guAge1] [tinyint] NULL,
	[gums1] [varchar](10) NOT NULL,
	[guLastname2] [varchar](25) NULL,
	[guFirstName2] [varchar](30) NULL,
	[guAge2] [tinyint] NULL,
	[gums2] [varchar](10) NOT NULL,
	[guPax] [decimal](4, 1) NOT NULL,
	[guag] [varchar](35) NULL,
	[guco] [varchar](25) NULL,
	[gumk] [varchar](10) NOT NULL,
	[gula] [varchar](2) NOT NULL,
	[guLW] [bit] NOT NULL,
	[guNW] [bit] NOT NULL,
	[guAvail] [bit] NOT NULL,
	[guInfo] [bit] NOT NULL,
	[guInvit] [bit] NOT NULL,
	[guloInfo] [varchar](10) NULL,
	[guloInvit] [varchar](10) NULL,
	[guPRAssign] [varchar](10) NULL,
	[guPRAvail] [varchar](10) NULL,
	[guPRInfo] [varchar](10) NULL,
	[guPRInvit1] [varchar](10) NULL,
	[guSelfGen] [bit] NOT NULL,
	[guPRInvit2] [varchar](10) NULL,
	[guPRInvit3] [varchar](10) NULL,
	[guPRCaptain1] [varchar](10) NULL,
	[guPRCaptain2] [varchar](10) NULL,
	[guPRCaptain3] [varchar](10) NULL,
	[guum] [tinyint] NOT NULL,
	[guInfoD] [datetime] NULL,
	[gumo] [varchar](15) NULL,
	[gumoA] [bit] NOT NULL,
	[guComments] [nvarchar](100) NULL,
	[guQuinella] [bit] NOT NULL,
	[guOutInvitNum] [varchar](8) NULL,
	[guCity] [varchar](40) NULL,
	[guState] [varchar](30) NULL,
	[guInvitD] [datetime] NULL,
	[guInvitT] [datetime] NULL,
	[guResch] [bit] NOT NULL,
	[guReschD] [datetime] NULL,
	[guReschT] [datetime] NULL,
	[guBookD] [datetime] NULL,
	[guBookT] [datetime] NULL,
	[guPickUpT] [datetime] NULL,
	[guDirect] [bit] NOT NULL,
	[guBookCanc] [bit] NOT NULL,
	[guShowD] [datetime] NULL,
	[gusr] [varchar](10) NULL,
	[guHotelB] [varchar](30) NULL,
	[guDeposit] [money] NOT NULL,
	[guDepositTwisted] [money] NOT NULL,
	[guDepositReceived] [money] NOT NULL,
	[gucu] [varchar](10) NOT NULL,
	[guCCType] [varchar](30) NULL,
	[guExtraInfo] [varchar](50) NULL,
	[guTimeInT] [datetime] NULL,
	[guTimeOutT] [datetime] NULL,
	[guOccup1] [varchar](25) NULL,
	[guOccup2] [varchar](25) NULL,
	[guTaxiIn] [money] NOT NULL,
	[guTaxiOut] [money] NOT NULL,
	[guDepositgr] [int] NULL,
	[guTaxiIngr] [int] NULL,
	[guTaxiOutgr] [int] NULL,
	[guShow] [bit] NOT NULL,
	[guShowSeq] [tinyint] NULL,
	[guQ] [bit] NOT NULL,
	[guQType] [varchar](4) NULL,
	[guNQ] [bit] NOT NULL,
	[guCTour] [bit] NOT NULL,
	[guInOut] [bit] NOT NULL,
	[guWalkOut] [bit] NOT NULL,
	[guMealTicket] [bit] NOT NULL,
	[guMealTicketQty] [int] NOT NULL,
	[guMealTicketFolios] [varchar](30) NULL,
	[guSale] [bit] NOT NULL,
	[guGiftsReceived] [bit] NOT NULL,
	[guEntryHost] [varchar](10) NULL,
	[guGiftsHost] [varchar](10) NULL,
	[guExitHost] [varchar](10) NULL,
	[guLiner1Type] [tinyint] NOT NULL,
	[guLiner1] [varchar](10) NULL,
	[guLiner2] [varchar](10) NULL,
	[guLinerCaptain1] [varchar](10) NULL,
	[guCloser1] [varchar](10) NULL,
	[guCloser2] [varchar](10) NULL,
	[guCloser3] [varchar](10) NULL,
	[guExit1] [varchar](10) NULL,
	[guExit2] [varchar](10) NULL,
	[guCloserCaptain1] [varchar](10) NULL,
	[guPodium] [varchar](10) NULL,
	[guVLO] [varchar](10) NULL,
	[guWComments] [nvarchar](100) NULL,
	[guGComments] [nvarchar](100) NULL,
	[guEComments] [nvarchar](100) NULL,
	[guStatus] [varchar](1) NULL,
	[guInternetTransfer] [bit] NOT NULL,
	[guO1] [varchar](20) NULL,
	[guO2] [varchar](20) NULL,
	[guO3] [varchar](20) NULL,
	[guO4] [varchar](20) NULL,
	[guO5] [varchar](20) NULL,
	[guO6] [varchar](20) NULL,
	[guTwistedDeposit] [bit] NOT NULL,
	[guInvitNum] [int] NULL,
	[guTO] [varchar](10) NULL,
	[guoc] [varchar](3) NULL,
	[guDepositRet] [bit] NOT NULL,
	[guTaxiInRet] [bit] NOT NULL,
	[guTaxiOutRet] [bit] NOT NULL,
	[guFirstBookD] [datetime] NULL,
	[guHReservIDC] [varchar](15) NULL,
	[guRef] [int] NULL,
	[guTimeCloserT] [datetime] NULL,
	[guTimeExitT] [datetime] NULL,
	[guTour] [bit] NOT NULL,
	[guShow2] [bit] NOT NULL,
	[guShow2D] [datetime] NULL,
	[guQ2] [bit] NOT NULL,
	[guQType2] [varchar](4) NULL,
	[guNQ2] [bit] NOT NULL,
	[guCTour2] [bit] NOT NULL,
	[guDirect2] [bit] NOT NULL,
	[guQuinella2] [bit] NOT NULL,
	[guTour2] [bit] NOT NULL,
	[guInOut2] [bit] NOT NULL,
	[guWalkOut2] [bit] NOT NULL,
	[guTimeIn2T] [datetime] NULL,
	[guTimeCloser2T] [datetime] NULL,
	[guTimeExit2T] [datetime] NULL,
	[guTimeOut2T] [datetime] NULL,
	[guFlight] [varchar](10) NULL,
	[guGStatus] [varchar](5) NULL,
	[guDepSale] [money] NOT NULL,
	[gulsOriginal] [varchar](10) NOT NULL,
	[guAntesIO] [bit] NOT NULL,
	[guReschDT] [datetime] NULL,
	[guDepositSaleNum] [varchar](10) NULL,
	[guDepositSaleD] [datetime] NULL,
	[guRoomsQty] [int] NOT NULL,
	[guChildren] [tinyint] NOT NULL,
	[guChildrenAges] [varchar](20) NULL,
	[guQuinellaSplit] [bit] NOT NULL,
	[guOriginAvail] [bit] NOT NULL,
	[guDivResConsec] [tinyint] NOT NULL,
	[guDivResLeadSource] [varchar](10) NULL,
	[guDivResResNum] [varchar](15) NULL,
	[guGuestRef] [varchar](12) NULL,
	[guShowsQty] [tinyint] NOT NULL,
	[guFamily] [bit] NOT NULL,
	[guBirthDate1] [datetime] NULL,
	[guBirthDate2] [datetime] NULL,
	[guBirthDate3] [datetime] NULL,
	[guBirthDate4] [datetime] NULL,
	[guts] [varchar](10) NULL,
	[guEmail1] [varchar](50) NULL,
	[guEmail2] [varchar](50) NULL,
	[guMainInvit] [int] NULL,
	[guOverflow] [bit] NOT NULL,
	[guIdentification] [bit] NOT NULL,
	[guAccountGiftsCard] [varchar](16) NULL,
	[guQtyGiftsCard] [int] NOT NULL,
	[guCheckOutHotelD] [datetime] NOT NULL,
	[guIncludedTour] [bit] NOT NULL,
	[guFollow] [bit] NOT NULL,
	[guFollowD] [datetime] NULL,
	[guPRFollow] [varchar](10) NULL,
	[guReservationType] [varchar](1) NULL,
	[guHotelPrevious] [varchar](10) NULL,
	[guFolioPrevious] [varchar](15) NULL,
	[guGroup] [bit] NOT NULL,
	[gunb] [int] NULL,
	[guNoBookD] [datetime] NULL,
	[guPRNoBook] [varchar](10) NULL,
	[guPRNote] [bit] NOT NULL,
	[guCompany] [decimal](2, 0) NOT NULL,
	[guSaveProgram] [bit] NOT NULL,
	[guWithQuinella] [bit] NOT NULL,
	[guIdProfileOpera] [varchar](15) NOT NULL,
	[guAvailBySystem] [bit] NOT NULL,
	[gurt] [varchar](10) NULL,
	[gucl] [int] NULL,
	[gupt] [varchar](10) NOT NULL,
 CONSTRAINT [PK_Guests] PRIMARY KEY CLUSTERED 
(
	[guID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Folio de la reservación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guHReservID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del Lead Source' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guls'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número de membresía' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guMembershipNum'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el huésped viene en un grupo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guOnGroup'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del hotel' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guHotel'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número de habitación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guRoomNum'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de llegada' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guCheckInD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de salida' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guCheckOutD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el huésped ha hecho Check-In' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guCheckIn'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Apellido' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guLastName1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nombre' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guFirstName1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Edad' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guAge1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del estado civil' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'gums1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Apellido del acompañante' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guLastname2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nombre del acompañante' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guFirstName2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Edad del acompañante' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guAge2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del estado civil del acompañante' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'gums2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número de adultos y menores' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guPax'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la agencia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guag'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del país' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guco'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del mercado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'gumk'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del idioma' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'gula'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el huésped está disponible' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guAvail'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el huésped está contactado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guInfo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el huésped está invitado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guInvit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la locación de contacto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guloInfo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la locación de invitación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guloInvit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del PR asignado al huésped' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guPRAssign'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del PR que modificó la disponibilidad del huésped' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guPRAvail'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del PR que contactó al huésped' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guPRInfo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del PR que invitó al huésped' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guPRInvit1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si la invitación es Self Gen' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guSelfGen'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del 2do PR que invitó al huésped' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guPRInvit2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del 3er PR que invitó al huésped' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guPRInvit3'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del capitán del PR que invitó al huésped' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guPRCaptain1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del capitán del 2do PR que invitó al huésped' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guPRCaptain2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del capitán del 3er PR que invitó al huésped' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guPRCaptain3'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del motivo de indisponibilidad' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guum'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de contacto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guInfoD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del formato de correo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'gumo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si se desea imprimir el correo del huésped' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'gumoA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Comentarios del PR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guComments'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si la invitación es quiniela' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guQuinella'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número de invitación outside' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guOutInvitNum'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ciudad' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guCity'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Estado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guState'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de invitación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guInvitD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hora de invitación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guInvitT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si es una reprogramación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guResch'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de reprogramación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guReschD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hora de reprogramación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guReschT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de booking' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guBookD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hora de booking' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guBookT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hora de recoja' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guPickUpT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si la invitación es directa' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guDirect'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si la invitación está cancelada' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guBookCanc'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de show' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guShowD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la sala de ventas' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'gusr'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del hotel del depósito quemado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guHotelB'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto de depósito' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guDeposit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto de depósito quemado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guDepositTwisted'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto de depósito recibido' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guDepositReceived'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la moneda del depósito' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'gucu'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la tarjeta de crédito principal' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guCCType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Comentarios extra del PR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guExtraInfo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hora de ingreso al show' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guTimeInT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hora de salida del show' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guTimeOutT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ocupación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guOccup1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ocupación del acompañante' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guOccup2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto del taxi de llegada' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guTaxiIn'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto del taxi de salida' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guTaxiOut'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del recibo de depósito' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guDepositgr'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del recibo del taxi de llegada' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guTaxiIngr'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del recibo del taxi de salida' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guTaxiOutgr'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el huésped asistió al show' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guShow'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el huésped está calificado para poder comprar una membresía' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guQ'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tipo de calificación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guQType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el huésped no está calificado para poder comprar una membresía' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guNQ'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el huésped tuvo tour de cortesía' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guCTour'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el show fue In & Out' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guInOut'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el show fue Walk Out' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guWalkOut'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si tiene cupones de comida' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guMealTicket'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cantidad de cupones de comida' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guMealTicketQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Folios de los cupones de comida' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guMealTicketFolios'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si tiene ventas' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guSale'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si tiene recibos de regalos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guGiftsReceived'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del Host de llegada' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guEntryHost'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del Host de regalos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guGiftsHost'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del Host de salida' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guExitHost'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tipo de liner' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guLiner1Type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del Liner' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guLiner1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del 2do Liner' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guLiner2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del capitán de Liners' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guLinerCaptain1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del Closer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guCloser1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del 2do Closer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guCloser2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del 3er Closer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guCloser3'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del Exit Closer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guExit1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del 2do Exit Closer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guExit2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del capitán de Closers' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guCloserCaptain1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del Podium' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guPodium'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del verificador legal' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guVLO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Comentarios del Host de llegada' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guWComments'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Comentarios del Host de regalos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guGComments'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Comentarios del Host de salida' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guEComments'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Estatus de invitación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si es una transferencia de Internet' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guInternetTransfer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del contrato' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guO1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tipo de huésped' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guO2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo opcional 3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guO3'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo opcional 4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guO4'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo opcional 5' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guO5'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo opcional 6' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guO6'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto del depósito quemado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guTwistedDeposit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número de la invitación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guInvitNum'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha del primer booking' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guFirstBookD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del huésped de referencia en el caso de invitaciones rebook' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guRef'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hora en que entró el Closer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guTimeCloserT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hora en que entró el Exit Closer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guTimeExitT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el show fue Tour' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guTour'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número de vuelo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guFlight'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del estatus principal' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guGStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto de venta depósito' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guDepSale'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del Lead Source original' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'gulsOriginal'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si un show fue In & Out' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guAntesIO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha y hora de reprogramación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guReschDT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Consecutivo de la venta depósito' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guDepositSaleNum'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de la venta depósito' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guDepositSaleD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número de habitaciones' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guRoomsQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número de niños' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guChildren'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Lista de edades de los niños' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guChildrenAges'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si la invitación es una quiniela split' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guQuinellaSplit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el huésped es originalmente disponible' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guOriginAvail'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Consecutivo de una reservación enlazada' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guDivResConsec'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hotel anterior de una reservación enlazada' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guDivResLeadSource'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Folio de la reservación anterior de una reservación enlazada' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guDivResResNum'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tipo de socio' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guGuestRef'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número de shows' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guShowsQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si se trata de una familia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guFamily'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de cumpleaños' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guBirthDate1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de cumpleaños del acompañante' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guBirthDate2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de cumpleaños del 3er huésped' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guBirthDate3'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de cumpleaños del 4to huésped' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guBirthDate4'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del equipo de vendedores al que se le debe contar el huésped en caso de ser Self Gen' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guts'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Correo electrónico' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guEmail1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Correo electrónico del acompañante' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guEmail2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la invitación principal en caso de ser quiniela split' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guMainInvit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el huésped fue atendido en la sala de ventas por un Guest Service a falta de personal de ventas' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guOverflow'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el huésped tiene identificación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guIdentification'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cuenta del monedero electrónico' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guAccountGiftsCard'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cantidad de monederos electrónicos asociados' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guQtyGiftsCard'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de salida del sistema de Hotel' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guCheckOutHotelD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si ya se le dió el tour incluido a una invitación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guIncludedTour'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si a un huésped se le ha dado seguimiento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guFollow'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de seguimiento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guFollowD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del PR de seguimiento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guPRFollow'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tipo de reservación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guReservationType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del hotel anterior' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guHotelPrevious'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Folio anterior' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guFolioPrevious'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si pertenece a un grupo de huéspedes' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guGroup'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del motivo No Booking' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'gunb'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de No Booking' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guNoBookD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del PR de No Booking' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guPRNoBook'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si tiene notas de PR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guPRNote'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la compañía de la membresía' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guCompany'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si es un tour de rescate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guSaveProgram'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si es un show que tomo el tour con otra quiniela' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guWithQuinella'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id del perfil de Opera' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guIdProfileOpera'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el huésped está disponible por sistema' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'guAvailBySystem'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del tipo de habitacion' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'gurt'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del club' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'gucl'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'la forma de pago de depósitos quemados en una Invitación a un Show' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Guests', @level2type=N'COLUMN',@level2name=N'gupt'
GO

ALTER TABLE [dbo].[Guests]  WITH CHECK ADD  CONSTRAINT [FK_Guests_Agencies] FOREIGN KEY([guag])
REFERENCES [dbo].[Agencies] ([agID])
GO

ALTER TABLE [dbo].[Guests] CHECK CONSTRAINT [FK_Guests_Agencies]
GO

ALTER TABLE [dbo].[Guests]  WITH CHECK ADD  CONSTRAINT [FK_Guests_Clubs] FOREIGN KEY([gucl])
REFERENCES [dbo].[Clubs] ([clID])
GO

ALTER TABLE [dbo].[Guests] CHECK CONSTRAINT [FK_Guests_Clubs]
GO

ALTER TABLE [dbo].[Guests]  WITH CHECK ADD  CONSTRAINT [FK_Guests_Countries] FOREIGN KEY([guco])
REFERENCES [dbo].[Countries] ([coID])
GO

ALTER TABLE [dbo].[Guests] CHECK CONSTRAINT [FK_Guests_Countries]
GO

ALTER TABLE [dbo].[Guests]  WITH CHECK ADD  CONSTRAINT [FK_Guests_NotBookingMotives] FOREIGN KEY([gunb])
REFERENCES [dbo].[NotBookingMotives] ([nbID])
GO

ALTER TABLE [dbo].[Guests] CHECK CONSTRAINT [FK_Guests_NotBookingMotives]
GO

ALTER TABLE [dbo].[Guests]  WITH CHECK ADD  CONSTRAINT [FK_Guests_PaymentTypes] FOREIGN KEY([gupt])
REFERENCES [dbo].[PaymentTypes] ([ptID])
GO

ALTER TABLE [dbo].[Guests] CHECK CONSTRAINT [FK_Guests_PaymentTypes]
GO

ALTER TABLE [dbo].[Guests]  WITH CHECK ADD  CONSTRAINT [FK_Guests_Personnel_PRFollow] FOREIGN KEY([guPRFollow])
REFERENCES [dbo].[Personnel] ([peID])
GO

ALTER TABLE [dbo].[Guests] CHECK CONSTRAINT [FK_Guests_Personnel_PRFollow]
GO

ALTER TABLE [dbo].[Guests]  WITH CHECK ADD  CONSTRAINT [FK_Guests_Personnel_PRNoBook] FOREIGN KEY([guPRNoBook])
REFERENCES [dbo].[Personnel] ([peID])
GO

ALTER TABLE [dbo].[Guests] CHECK CONSTRAINT [FK_Guests_Personnel_PRNoBook]
GO

ALTER TABLE [dbo].[Guests]  WITH CHECK ADD  CONSTRAINT [FK_Guests_RoomTypes] FOREIGN KEY([gurt])
REFERENCES [dbo].[RoomTypes] ([rtID])
GO

ALTER TABLE [dbo].[Guests] CHECK CONSTRAINT [FK_Guests_RoomTypes]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF_Guests_guOnGroup]  DEFAULT (0) FOR [guOnGroup]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guChkIn__1E3A7A34]  DEFAULT (0) FOR [guCheckIn]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__gums1__1F2E9E6D]  DEFAULT ('N') FOR [gums1]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__gums2__2022C2A6]  DEFAULT ('N') FOR [gums2]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guPax__2116E6DF]  DEFAULT (0) FOR [guPax]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF_Guests_guag]  DEFAULT ('?') FOR [guag]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF_Guests_guco]  DEFAULT ('?') FOR [guco]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF_Guests_gumk]  DEFAULT ('AGENCIES') FOR [gumk]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__gula__2AA05119]  DEFAULT ('EN') FOR [gula]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guLW__2B947552]  DEFAULT (0) FOR [guLW]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guRLW__2C88998B]  DEFAULT (0) FOR [guNW]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guAvail__220B0B18]  DEFAULT (0) FOR [guAvail]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guInfo__23F3538A]  DEFAULT (0) FOR [guInfo]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guInvit__24E777C3]  DEFAULT (0) FOR [guInvit]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF_Guests_guInvit1]  DEFAULT (0) FOR [guSelfGen]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guum__22FF2F51]  DEFAULT (0) FOR [guum]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__gumoA__29AC2CE0]  DEFAULT (1) FOR [gumoA]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guQuinel__2D7CBDC4]  DEFAULT (0) FOR [guQuinella]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF_Guests_guResch]  DEFAULT (0) FOR [guResch]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF_Guests_guBookCanc1]  DEFAULT (0) FOR [guDirect]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guBookCa__2E70E1FD]  DEFAULT (0) FOR [guBookCanc]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guDeposi__2F650636]  DEFAULT (0) FOR [guDeposit]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF_Guests_guDeposit1]  DEFAULT (0) FOR [guDepositTwisted]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF_Guests_guDeposit1_1]  DEFAULT (0) FOR [guDepositReceived]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__gucu__30592A6F]  DEFAULT ('US') FOR [gucu]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guTaxiIn__3335971A]  DEFAULT (0) FOR [guTaxiIn]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guTaxiOu__351DDF8C]  DEFAULT (0) FOR [guTaxiOut]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guShow__25DB9BFC]  DEFAULT (0) FOR [guShow]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guQ__26CFC035]  DEFAULT (0) FOR [guQ]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF_Guests_guNQ]  DEFAULT (1) FOR [guNQ]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guCTour__39E294A9]  DEFAULT (0) FOR [guCTour]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guInOut__37FA4C37]  DEFAULT (0) FOR [guInOut]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guWalkOu__38EE7070]  DEFAULT (0) FOR [guWalkOut]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF_Guests_guMealTicket]  DEFAULT (0) FOR [guMealTicket]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF_Guests_guMeelTicketQyt]  DEFAULT (0) FOR [guMealTicketQty]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guSale__27C3E46E]  DEFAULT (0) FOR [guSale]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF_Guests_guSale1]  DEFAULT (0) FOR [guGiftsReceived]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF_Guests_saLiner1Type]  DEFAULT (0) FOR [guLiner1Type]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF_Guests_guInternetTransfer]  DEFAULT (0) FOR [guInternetTransfer]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guTwiste__314D4EA8]  DEFAULT (0) FOR [guTwistedDeposit]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guDeposi__324172E1]  DEFAULT (0) FOR [guDepositRet]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guTaxiIn__3429BB53]  DEFAULT (0) FOR [guTaxiInRet]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guTaxiOu__361203C5]  DEFAULT (0) FOR [guTaxiOutRet]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guTour__0F6D37F0]  DEFAULT (0) FOR [guTour]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guShow2__10615C29]  DEFAULT (0) FOR [guShow2]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guQ2__11558062]  DEFAULT (0) FOR [guQ2]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guNQ2__1249A49B]  DEFAULT (0) FOR [guNQ2]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guCTour2__133DC8D4]  DEFAULT (0) FOR [guCTour2]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guDirect__1431ED0D]  DEFAULT (0) FOR [guDirect2]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guQuinel__15261146]  DEFAULT (0) FOR [guQuinella2]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guTour2__161A357F]  DEFAULT (0) FOR [guTour2]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guInOut2__170E59B8]  DEFAULT (0) FOR [guInOut2]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF__Guests__guWalkOu__18027DF1]  DEFAULT (0) FOR [guWalkOut2]
GO

ALTER TABLE [dbo].[Guests] ADD  DEFAULT (0) FOR [guDepSale]
GO

ALTER TABLE [dbo].[Guests] ADD  DEFAULT (0) FOR [guAntesIO]
GO

ALTER TABLE [dbo].[Guests] ADD  DEFAULT (1) FOR [guRoomsQty]
GO

ALTER TABLE [dbo].[Guests] ADD  DEFAULT (0) FOR [guChildren]
GO

ALTER TABLE [dbo].[Guests] ADD  DEFAULT (0) FOR [guQuinellaSplit]
GO

ALTER TABLE [dbo].[Guests] ADD  DEFAULT (1) FOR [guOriginAvail]
GO

ALTER TABLE [dbo].[Guests] ADD  DEFAULT (0) FOR [guDivResConsec]
GO

ALTER TABLE [dbo].[Guests] ADD  DEFAULT (1) FOR [guShowsQty]
GO

ALTER TABLE [dbo].[Guests] ADD  DEFAULT (0) FOR [guFamily]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF_Guests_guOverflow]  DEFAULT (0) FOR [guOverflow]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF_Guests_guIdentification]  DEFAULT (0) FOR [guIdentification]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF_Guests_guQtyGiftsCard]  DEFAULT (0) FOR [guQtyGiftsCard]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF_Guests_guIncludedTour]  DEFAULT (0) FOR [guIncludedTour]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF_Guests_guFollow]  DEFAULT (0) FOR [guFollow]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF_Guests_guGroup]  DEFAULT (0) FOR [guGroup]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF_Guests_guPRNote]  DEFAULT (0) FOR [guPRNote]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF_Guests_guCompany]  DEFAULT (0) FOR [guCompany]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF_Guests_guSave]  DEFAULT (0) FOR [guSaveProgram]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF_Guests_guWithQuinella]  DEFAULT (0) FOR [guWithQuinella]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF_Guests_guIdProfileOpera]  DEFAULT ('') FOR [guIdProfileOpera]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF_Guests_guAvailBySystem]  DEFAULT (0) FOR [guAvailBySystem]
GO

ALTER TABLE [dbo].[Guests] ADD  CONSTRAINT [DF_Guests_gupt]  DEFAULT ('CS') FOR [gupt]
GO

ALTER TABLE dbo.Guests ADD guReimpresion tinyint DEFAULT 0
GO

ALTER TABLE dbo.Guests ADD gurm tinyint DEFAULT NULL
GO

ALTER TABLE [dbo].[Guests]  WITH CHECK ADD  CONSTRAINT [FK_Guest_ReimpresionMotives] FOREIGN KEY([gurm])
REFERENCES [dbo].[ReimpresionMotives] ([rmID])
GO
