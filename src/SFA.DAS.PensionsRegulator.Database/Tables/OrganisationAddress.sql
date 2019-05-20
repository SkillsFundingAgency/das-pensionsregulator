CREATE TABLE [dbo].[OrganisationAddress]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Employer_SK] INT NOT NULL, 
    [OrganisationFullName] VARCHAR(255) NOT NULL, 
    [OrganisationFullAddress] VARCHAR(255) NOT NULL, 
    [AddressLine1] VARCHAR(35) NULL, 
    [AddressLine2] VARCHAR(35) NULL, 
    [AddressLine3] VARCHAR(35) NULL, 
    [AddressLine4] VARCHAR(35) NULL, 
    [AddressLine5] VARCHAR(35) NULL, 
    [PostCode] VARCHAR(10) NULL, 
    [EmailAddress] VARCHAR(100) NULL, 
    [TelephoneNumber] VARCHAR(25) NULL, 
    [FaxNumber] VARCHAR(25) NULL, 
    [ForeignCountry] VARCHAR(35) NULL, 
    [AddressVerifiedIndicator] SMALLINT NULL, 
    [CreatedDate] DATETIME2 NULL, 
    [UpdatedDate] DATETIME2 NULL, 
    CONSTRAINT [FK_OrganisationAddress_Organisation] FOREIGN KEY ([Employer_SK]) REFERENCES [Organisation]([Employer_SK])
)
