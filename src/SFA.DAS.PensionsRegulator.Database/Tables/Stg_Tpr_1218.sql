﻿/* Staging Table for Tpr */

CREATE TABLE [dbo].[Stg_Tpr_1218](
	[Record Type1] [varchar](max) NULL,
	[UniqueID] [bigint] NULL,
	[DistrictNumber] [smallint] NULL,
	[Reference] [varchar](10) NULL,
	[XferFromDist] [smallint] NULL,
	[XferFromRef] [varchar](10) NULL,
	[XferToDstrct] [smallint] NULL,
	[XferToRef] [varchar](10) NULL,
	[AODistrict] [smallint] NULL,
	[AOTaxType] [varchar](1) NULL,
	[AOCheckChar] [varchar](1) NULL,
	[AOReference] [varchar](50) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[EndDateCode] [smallint] NULL,
	[RestartDate] [datetime] NULL,
	[LbltyFirstYr] [smallint] NULL,
	[LbltyLastYr] [smallint] NULL,
	[ReviewYr] [smallint] NULL,
	[CompanyRegNo] [varchar](10) NULL,
	[TradeClass] [varchar](5) NULL,
	[CntrctOutRef] [varchar](8) NULL,
	[ToEmpInstance] [varchar](50) NULL,
	[ToEmpFirstYear] [varchar](50) NULL,
	[AOInd] [varchar](50) NULL,
	[XferType] [smallint] NULL,
	[UniqueRefDistrict] [smallint] NULL,
	[UniqueReference] [bigint] NULL,
	[RecordType2] [varchar](max) NULL,
	[Name1] [varchar](28) NULL,
	[Name2] [varchar](28) NULL,
	[TelephoneNmbr] [varchar](15) NULL,
	[FaxNumber] [varchar](15) NULL,
	[EmailAddress] [varchar](70) NULL,
	[AddressType] [varchar](100) NULL,
	[ADI] [varchar](35) NULL,
	[AddressLine1] [varchar](35) NULL,
	[AddressLine2] [varchar](35) NULL,
	[AddressLine3] [varchar](35) NULL,
	[AddressLine4] [varchar](35) NULL,
	[AddressLine5] [varchar](35) NULL,
	[PostCode] [varchar](8) NULL,
	[RLSInd] [varchar](50) NULL,
	[ForeignCountry] [varchar](35) NULL,
	[AddressVerifiedIndicator] [smallint] NULL,
	[RecordType3] [varchar](max) NULL,
	[SignalUniqueID] [int] NULL,
	[Code] [varchar](50) NULL,
	[SignalChgd] [varchar](50) NULL,
	[SigStartYear] [varchar](50) NULL,
	[SigEndYear] [varchar](50) NULL,
	[Value] [varchar](50) NULL,
	[RecordType4] [varchar](max) NULL,
	[EmployerSchemeHistoryUniqueID] [varchar](50) NULL,
	[StartYear] [varchar](50) NULL,
	[EndYear] [varchar](50) NULL,
	[SchemeType] [varchar](50) NULL,
	[RecordType5] [varchar](max) NULL,
	[EmployerCountID] [varchar](50) NULL,
	[EEC_Code] [varchar](50) NULL,
	[DateTaken] [varchar](50) NULL,
	[NumberCounted] [varchar](50) NULL,
	[e-filedExemptionIndicator] [varchar](50) NULL,
	[RecordType6] [varchar](max) NULL,
	[UniqueID-Employment] [varchar](50) NULL,
	[UniqueID-Employer] [varchar](50) NULL,
	[LatestAnnualisedGrossEarningForPAYESscheme(fromP14)] [varchar](50) NULL,
	[CurrencyIndicator] [varchar](50) NULL,
	[TaxYear] [varchar](50) NULL,
	[CountEmploymentsP14] [varchar](50) NULL,
	[PAYESchemeRef] [varchar](20) NULL,
	[AORN] [varchar](13) NULL,
	[PayeDerived] AS RIGHT('000'+left(PAYESchemeRef,CHARINDEX('/',PayeSchemeRef)-1),3)+SUBSTRING(PAYESchemeRef,CHARINDEX('/',PAYESchemeRef),LEN(PAYESCHEMEREF))  
) 
GO
CREATE CLUSTERED INDEX [CI_TPR_UniqueKey] ON [dbo].[Stg_Tpr_1218]([UniqueID])
GO
CREATE NONCLUSTERED INDEX [NCI_TPR_AORN] ON [dbo].[Stg_Tpr_1218]([AORN])
GO
CREATE NONCLUSTERED INDEX [NCI_TPR_PAYE] ON [dbo].[Stg_Tpr_1218]([PAYESchemeRef])
GO
CREATE NONCLUSTERED INDEX [NCI_TPR_PAYE_AORN] ON [dbo].[Stg_Tpr_1218]([PAYESchemeRef],[AORN])
GO
CREATE NONCLUSTERED INDEX [NCI_TPR_PAYEDerived] ON [dbo].[Stg_Tpr_1218](PayeDerived)
GO
CREATE NONCLUSTERED INDEX [NCI_TPR_PAYEDerived_AORN] ON [dbo].[Stg_Tpr_1218](PayeDerived,AORN)
GO