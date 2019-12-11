﻿CREATE TABLE [dbo].[Staging_TPR](
	[RecordType1] [varchar](256) NULL,
	[TPRUniqueID] [varchar](256) NULL,
	[DistrictNumber] [varchar](256) NULL,
	[Reference] [varchar](256) NULL,
	[AODistrict] [varchar](256) NULL,
	[AOTaxType] [varchar](256) NULL,
	[AOCheckChar] [varchar](256) NULL,
	[AOReference] [varchar](256) NULL,
	[StartDate] [varchar](256) NULL,
	[EndDate] [varchar](256) NULL,
	[EndDateCode] [varchar](256) NULL,
	[RestartDate] [varchar](256) NULL,
	[ReviewYr] [varchar](256) NULL,
	[CompanyRegNo] [varchar](256) NULL,
	[TradeClass] [varchar](256) NULL,
	[AOInd] [varchar](256) NULL,
	[XferType] [varchar](256) NULL,
	[UniqueRefDistrict] [varchar](256) NULL,
	[UniqueReference] [varchar](256) NULL,
	[RecordType2] [varchar](256) NULL,
	[Name1] [nvarchar](256) NULL,
	[Name2] [nvarchar](256) NULL,
	[TelephoneNumber] [varchar](256) NULL,
	[EmailAddress] [nvarchar](256) MASKED WITH (FUNCTION = 'email()') NULL,
	[AddressType] [varchar](256) NULL,
	[ADI] [varchar](256) NULL,
	[AddressLine1] [varchar](256) NULL,
	[AddressLine2] [varchar](256) NULL,
	[AddressLine3] [varchar](256) NULL,
	[AddressLine4] [varchar](256) NULL,
	[AddressLine5] [varchar](256) NULL,
	[PostCode] [varchar](256) NULL,
	[ForeignCountry] [varchar](256) NULL,
	[AddressVerifiedIndicator] [varchar](256) NULL,
	[RecordType3] [varchar](256) NULL,
	[SignalUniqueID] [varchar](256) NULL,
	[SignalCode] [varchar](256) NULL,
	[SignalChanged] [varchar](256) NULL,
	[SignalStartYear] [varchar](256) NULL,
	[SignalEndYear] [varchar](256) NULL,
	[SignalValue] [varchar](256) NULL,
	[RecordType7] [varchar](256) NULL,
	[EmployerSchemeHistoryUniqueID] [varchar](256) NULL,
	[EmployerSchemeHistStartYear] [varchar](256) NULL,
	[EmployerSchemeHistEndYear] [varchar](256) NULL,
	[SchemeType] [varchar](256) NULL,
	[RecordType12] [varchar](256) NULL,
	[EmployerCountID] [varchar](256) NULL,
	[EEC_Code] [varchar](256) NULL,
	[DateTaken] [varchar](256) NULL,
	[NumberCounted] [varchar](256) NULL,
	[SourceSK] [int] IDENTITY(1,1) NOT NULL,
	[SourceFileName] [varchar](256) NULL,
	[RunID] [int] NULL,
	[IsValid] [bit] DEFAULT(1) NULL,
	[InvalidReason] [varchar](512) NULL,
	[RecordCreatedDate] [datetime2](7) DEFAULT(getdate()) NULL,
 CONSTRAINT [PK_Staging_SourceSK] PRIMARY KEY CLUSTERED (SourceSK ASC)
  )
go
CREATE NONCLUSTERED INDEX NCI_Staging_TPR
      ON dbo.Staging_TPR(TPRUniqueID)

GO

