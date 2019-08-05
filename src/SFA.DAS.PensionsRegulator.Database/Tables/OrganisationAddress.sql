CREATE TABLE [dbo].[OrganisationAddress](
	[OrgAddSK] [bigint] IDENTITY(1,1) NOT NULL,
	[OrgSK] [bigint] NULL,
	[TPRUniqueID] [bigint] UNIQUE NOT NULL,
	[AddressCode] [char](1) NOT NULL,
	[AddressCodeDesc] [varchar](255) NOT NULL,
	[FromDate] [date] NOT NULL,
	[ToDate] [date] NULL,
	[OrganisationFullAddress] [nvarchar](255) NULL,
	[AddressLine1] [nvarchar](100) NULL,
	[AddressLine2] [nvarchar](100) NULL,
	[AddressLine3] [nvarchar](100) NULL,
	[AddressLine4] [nvarchar](100) NULL,
	[AddressLine5] [nvarchar](100) NULL,
	[PostCode] [varchar](10) NULL,
	[EmailAddress] [nvarchar](256) MASKED WITH (FUNCTION = 'email()') NULL,
	[TelephoneNumber] [nvarchar](25) NULL,
	[AddressType] [char](1) NULL,
	[AddressTypeDesc] [varchar](100) NULL,
	[ForeignCountry] [varchar](100) NULL,
	[AddressVerifiedIndicator] [char](1) NULL,
	[AddressVerifiedIndicatorDesc] [varchar](255) NULL,
	[ADI] [varchar](100) NULL,
	[SourceSK] [bigint] NULL,
	[RunId] [bigint] NULL,
	[SourceFileName] [varchar](255) NULL,
	[RecordCreatedDate] [datetime2](7) DEFAULT(GETDATE()) NULL,
	CONSTRAINT [PK_OrgAdd_OrgSK] PRIMARY KEY CLUSTERED (OrgAddSK ASC),
	CONSTRAINT [FK_OrganisationAddress_OrgSK] FOREIGN KEY (OrgSK) REFERENCES [Organisation](OrgSK)
    )



