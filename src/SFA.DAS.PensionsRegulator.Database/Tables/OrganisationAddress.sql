CREATE TABLE [Tpr].[OrganisationAddress](
	[OrgAddSK] [bigint] IDENTITY(1,1) NOT NULL,
	[OrgSK] [bigint] NULL,
	[TPRUniqueID] [bigint] NOT NULL,
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
	[RecordCreatedDate] [datetime2](7) DEFAULT(getdate()) NULL,
    CONSTRAINT [PK_OrgAdd_OrgAddSK] PRIMARY KEY CLUSTERED ([OrgAddSK] ASC)
	)
GO
CREATE NONCLUSTERED INDEX [NCI_Organisation_SK] ON [Tpr].[OrganisationAddress]
(
	[OrgSK] ASC,
	[TPRUniqueID] ASC,
	[SourceSK] ASC
)
GO

