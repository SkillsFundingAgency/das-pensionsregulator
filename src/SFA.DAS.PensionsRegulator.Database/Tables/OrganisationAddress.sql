CREATE TABLE [dbo].[OrganisationAddress]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [OrganisationId] BIGINT NOT NULL, 
    [OrganisationFullName] NVARCHAR(50) NOT NULL, 
    [OrganisationFullAddress] NVARCHAR(50) NOT NULL, 
    [AddressLine1] NVARCHAR(50) NULL, 
    [AddressLine2] NVARCHAR(50) NULL, 
    [AddressLine3] NVARCHAR(50) NULL, 
    [AddressLine4] NVARCHAR(50) NULL, 
    [AddressLine5] NVARCHAR(50) NULL, 
    [PostCode] NVARCHAR(50) NULL, 
    [EmailAddress] NVARCHAR(50) NULL, 
    [TelephoneNumber] NVARCHAR(50) NULL, 
    [FaxNumber] NVARCHAR(50) NULL, 
    [ForeignCountery] NVARCHAR(50) NULL, 
    [AddressVerifiedIndicator] BIT NULL, 
    [CreatedDate] DATETIME2 NULL, 
    [UpdatedDate] DATETIME2 NULL, 
    CONSTRAINT [FK_OrganisationAddress_Organisation] FOREIGN KEY ([OrganisationId]) REFERENCES [Organisation]([Id])
)
