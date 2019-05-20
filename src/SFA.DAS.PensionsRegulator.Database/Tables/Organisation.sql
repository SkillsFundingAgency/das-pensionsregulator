CREATE TABLE [dbo].[Organisation]
(
	[Id] BIGINT NOT NULL PRIMARY KEY, 
    [Employer_SK] INT Identity(1000,1) UNIQUE,
    [OranisationName] VARCHAR(255) NOT NULL, 
    [CompanyRegNo] VARCHAR(25) NULL, 
    [Status] VARCHAR(30) NULL

)
