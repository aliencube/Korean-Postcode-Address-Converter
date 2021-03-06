/****** Object:  Table [dbo].[LotBasedAddress]    Script Date: 06/25/2013 11:22:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LotBasedAddress]') AND type in (N'U'))
DROP TABLE [dbo].[LotBasedAddress]
GO
/****** Object:  Table [dbo].[StreetBasedAddress]    Script Date: 06/25/2013 11:22:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StreetBasedAddress]') AND type in (N'U'))
DROP TABLE [dbo].[StreetBasedAddress]
GO
/****** Object:  Table [dbo].[StreetBasedAddress]    Script Date: 06/25/2013 11:22:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StreetBasedAddress]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[StreetBasedAddress](
	[AddressId] [int] IDENTITY(1,1) NOT NULL,
	[Postcode] [nvarchar](8) NOT NULL,
	[SequenceNumber] [nvarchar](32) NOT NULL,
	[Province] [nvarchar](16) NULL,
	[ProvinceEng] [nvarchar](32) NULL,
	[County] [nvarchar](16) NULL,
	[CountyEng] [nvarchar](32) NULL,
	[District] [nvarchar](16) NULL,
	[DistrictEng] [nvarchar](32) NULL,
	[Suburb] [nvarchar](16) NULL,
	[SuburbEng] [nvarchar](32) NULL,
	[StreetNameCode] [nvarchar](32) NULL,
	[StreetName] [nvarchar](16) NULL,
	[StreetNameEng] [nvarchar](32) NULL,
	[Basement] [int] NULL,
	[BuildingNumberMajor] [int] NULL,
	[BuildingNumberMinor] [int] NULL,
	[BuildingCode] [nvarchar](32) NULL,
	[BuildingNameForBulk] [nvarchar](16) NULL,
	[BuildingName] [nvarchar](16) NULL,
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
GO
/****** Object:  Table [dbo].[LotBasedAddress]    Script Date: 06/25/2013 11:22:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LotBasedAddress]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[LotBasedAddress](
	[AddressId] [int] IDENTITY(1,1) NOT NULL,
	[Postcode] [nvarchar](8) NOT NULL,
	[SequenceNumber] [int] NOT NULL,
	[Province] [nvarchar](16) NULL,
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
GO
