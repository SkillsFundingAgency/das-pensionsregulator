CREATE TABLE [dbo].[OrganisationPAYEScheme]
(
	[Id] BIGINT NOT NULL PRIMARY KEY, 
    [OrganisationId] BIGINT NOT NULL, 
    [PAYESchemeRef] NVARCHAR(50) NOT NULL, 
    [SchemeType] NVARCHAR(50) NOT NULL, 
    [CountOfEmployments] INT NOT NULL, 
    [TaxYear] DATETIME2 NOT NULL, 
    [LatestAnnualisedGrossEarningForPAYEScheme] DECIMAL NOT NULL, 
    [CountOfEmployees] INT NOT NULL, 
    [Status] NVARCHAR(50) NOT NULL, 
    [CreatedDate] DATETIME2 NOT NULL, 
    [UpdatedDate] DATETIME2 NOT NULL, 
    [SourceTableName] NVARCHAR(50) NOT NULL, 
    [SourceSK] NCHAR(10) NOT NULL, 
    CONSTRAINT [FK_OrganisationPAYEScheme_Organisation_Id] FOREIGN KEY ([OrganisationId]) REFERENCES [Organisation]([Id])
)
