CREATE TABLE [dbo].[OrganisationPAYEScheme]
(
	[Id] INT Identity(1,1) NOT NULL PRIMARY KEY, 
    [Employer_SK] INT NOT NULL, 
    [PAYESchemeRef] VARCHAR(25) NOT NULL, 
    [SchemeType] SMALLINT NOT NULL, 
    [CountOfEmployments] INT NOT NULL, 
    [TaxYear] INT NOT NULL, 
    [LatestAnnualisedGrossEarningForPAYEScheme] BIGINT NOT NULL, 
    [CountOfEmployees] INT NOT NULL, 
    [Status] VARCHAR(30) NOT NULL, 
    [CreatedDate] DATETIME2 NOT NULL, 
    [UpdatedDate] DATETIME2 NOT NULL, 
    [SourceTableName] VARCHAR(25) NOT NULL, 
    [SourceSK] INT NOT NULL, 
    CONSTRAINT [FK_OrganisationPAYEScheme_Organisation] FOREIGN KEY ([Employer_SK]) REFERENCES [Organisation]([Employer_SK]), 
)
