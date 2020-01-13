CREATE TABLE [Tpr].[Organisation](
	[OrgSK] [bigint] IDENTITY(1,1) NOT NULL,
	[TPRUniqueId] [bigint] NOT NULL,
	[OrganisationName] [varchar](256) NULL,
	[DistrictNumber] [char](3) NOT NULL,
	[Reference] [char](10) NOT NULL,
	[CompanyRegNo] [varchar](256) NULL,
	[TradeClass] [varchar](256) NULL,
	[AODistrict] [int] NOT NULL,
	[AOTaxType] [char](1) NOT NULL,
	[AOCheckChar] [char](1) NOT NULL,
	[AOReference] [char](8) NOT NULL,
	[AORN] [char](13) NOT NULL,
	[AOInd] [char](1) NULL,
	[AOIndDesc] [varchar](256) NULL,
	[TransferType] [tinyint] NULL,
	[TransferTypeDesc] [varchar](256) NULL,
	[UniqueRefDistrict] [char](3) NULL,
	[UniqueReference] [char](10) NULL,
	[SourceSK] [bigint] NULL,
	[RunId] [bigint] NULL,
	[SourceFileName] [varchar](255) NULL,
	[RecordCreatedDate] [datetime2](7) DEFAULT(GETDATE()) NULL,
 CONSTRAINT [PK_Org_OrgSK] PRIMARY KEY CLUSTERED([OrgSK] ASC)
)
GO
CREATE NONCLUSTERED INDEX [NCI_Organisation_AORN] ON [Tpr].[Organisation]([AORN] ASC)
GO
CREATE NONCLUSTERED INDEX [NCI_Organisation_SK] ON [Tpr].[Organisation]([TPRUniqueId] ASC,[SourceSK] ASC)
GO


