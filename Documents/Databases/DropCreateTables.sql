USE [KOR_Postcode_Address_20130625]
GO
/****** Object:  StoredProcedure [dbo].[DropCreateTables]    Script Date: 06/26/2013 14:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Justin Yoo
-- Create date: 2013-06-26
-- Description:	Drops and recreate both LOT based address and Street based address tables.
-- =============================================

-- EXECUTE DropCreateTables 'Lot'
-- EXECUTE DropCreateTables 'Street'

ALTER PROCEDURE [dbo].[DropCreateTables]
	-- Add the parameters for the stored procedure here
	@AddressType NVARCHAR(6)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON
	
	IF @AddressType = 'Lot'
	BEGIN
		IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LotBasedAddress]') AND type in (N'U'))
		DROP TABLE [dbo].[LotBasedAddress]

		IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LotBasedAddress]') AND type in (N'U'))
		BEGIN
			CREATE TABLE [dbo].[LotBasedAddress](
				[AddressId] [int] IDENTITY(1,1) NOT NULL,
				[Postcode] [nvarchar](8) NOT NULL,
				[SequenceNumber] [int] NOT NULL,
				[Province] [nvarchar](16) NULL,
				[City] [nvarchar](16) NULL,
				[County] [nvarchar](16) NULL,
				[District] [nvarchar](16) NULL,
				[Suburb] [nvarchar](16) NULL,
				[Village] [nvarchar](16) NULL,
				[Island] [nvarchar](16) NULL,
				[San] [nvarchar](4) NULL,
				[LotNumberMajorFrom] [int] NULL,
				[LotNumberMinorFrom] [int] NULL,
				[LotNumberMajorTo] [int] NULL,
				[LotNumberMinorTo] [int] NULL,
				[BuildingName] [nvarchar](64) NULL,
				[BuildingNumberFrom] [int] NULL,
				[BuildingNumberTo] [int] NULL,
				[DateUpdated] [datetime] NULL,
				[Address] [nvarchar](256) NULL,
			 CONSTRAINT [PK_LotBasedAddress] PRIMARY KEY CLUSTERED 
			(
				[AddressId] ASC
			)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
			) ON [PRIMARY]
		END
	END
	
	ELSE IF @AddressType = 'Street'
	BEGIN
		IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StreetBasedAddress]') AND type in (N'U'))
		DROP TABLE [dbo].[StreetBasedAddress]

		IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StreetBasedAddress]') AND type in (N'U'))
		BEGIN
			CREATE TABLE [dbo].[StreetBasedAddress](
				[AddressId] [int] IDENTITY(1,1) NOT NULL,
				[Postcode] [nvarchar](8) NOT NULL,
				[SequenceNumber] [nvarchar](32) NOT NULL,
				[Province] [nvarchar](16) NULL,
				[ProvinceEng] [nvarchar](32) NULL,
				[City] [nvarchar](16) NULL,
				[CityEng] [nvarchar](32) NULL,
				[County] [nvarchar](16) NULL,
				[CountyEng] [nvarchar](32) NULL,
				[District] [nvarchar](16) NULL,
				[DistrictEng] [nvarchar](32) NULL,
				[Suburb] [nvarchar](16) NULL,
				[SuburbEng] [nvarchar](32) NULL,
				[StreetNameCode] [nvarchar](32) NULL,
				[StreetName] [nvarchar](256) NULL,
				[StreetNameEng] [nvarchar](256) NULL,
				[Basement] [int] NULL,
				[BuildingNumberMajor] [int] NULL,
				[BuildingNumberMinor] [int] NULL,
				[BuildingCode] [nvarchar](32) NULL,
				[BuildingNameForBulk] [nvarchar](64) NULL,
				[BuildingName] [nvarchar](64) NULL,
				[RegisteredSuburbCode] [nvarchar](32) NULL,
				[RegisteredSuburb] [nvarchar](16) NULL,
				[SuburbSequenceNumber] [int] NULL,
				[Village] [nvarchar](16) NULL,
				[San] [bit] NULL,
				[LotNumberMajor] [int] NULL,
				[LotNumberMinor] [int] NULL,
			 CONSTRAINT [PK_StreetBasedAddress] PRIMARY KEY CLUSTERED 
			(
				[AddressId] ASC
			)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
			) ON [PRIMARY]
		END
	END
END
