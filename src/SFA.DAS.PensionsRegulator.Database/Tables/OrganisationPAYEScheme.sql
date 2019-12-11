CREATE TABLE [dbo].[OrganisationPAYEScheme](
	[OrgSchemeSK] [bigint] IDENTITY(1,1) NOT NULL,
	[OrgSK] [bigint] NULL,
	[TPRUniqueID] [bigint] UNIQUE NOT NULL,
	[PAYEScheme] [varchar](100) NULL,
	[SchemeStartDate] [date] NULL,
	[SchemeEndDate] [date] NULL,
	[SchemeEndDateCode] [tinyint] NULL,
	[SchemeEndDateCodeDesc] [varchar](256) NULL,
	[RestartDate] [date] NULL,
	[ReviewYear] [int] NULL,
	[LiveEmployeeCount] [int] NULL,
	[EmployeeCountDateTaken] [date] NULL,
	[EmployeeCountCode] [char](1) NULL,
	[EmployeeCountCodeDesc] [varchar](256) NULL,
	[SignalCode] [varchar](10) NULL,
	[SignalChanged] [char](1) NULL,
	[SignalChangedDesc] [varchar](256) NULL,
	[SignalStartYear] [int] NULL,
	[SignalEndYear] [int] NULL,
	[SignalValue] [char](1) NULL,
	[HistorySchemeStartYear] [int] NULL,
	[HistorySchemeEndYear] [int] NULL,
	[HistorySchemeType] [int] NULL,
	[HistorySchemeTypeDesc] [varchar](256) NULL,
	[SourceSK] [bigint] NULL,
	[RunId] [bigint] NULL,
	[SourceFileName] [varchar](255) NULL,
	[RecordCreatedDate] [datetime2](7) DEFAULT(GETDATE()) NULL,
	[SignalCodeDesc] [varchar](256) NULL,
    CONSTRAINT [PK_OrgPAYEScheme_OrgSchemeSK] PRIMARY KEY CLUSTERED (OrgSchemeSK ASC),
    CONSTRAINT [FK_OrganisationPAYEScheme_OrgSK] FOREIGN KEY (OrgSK) REFERENCES dbo.[Organisation](OrgSK)
	)

GO
CREATE NONCLUSTERED INDEX [NCI_Organisation_PAYEScheme] ON [dbo].[OrganisationPAYEScheme]
(
	[PAYEScheme] ASC
)
GO
CREATE NONCLUSTERED INDEX [NCI_Organisation_SK] ON [dbo].[OrganisationPAYEScheme]
(
	[OrgSK] ASC,
	[TPRUniqueID] ASC,
	[SourceSK] ASC
)
GO
