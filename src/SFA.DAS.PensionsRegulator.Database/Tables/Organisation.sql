CREATE TABLE [dbo].[Organisation]
(
	[Id] BIGINT NOT NULL PRIMARY KEY, 
    [OranisationName] NVARCHAR(50) NOT NULL, 
    [CompanyRegNo] NVARCHAR(50) NULL, 
    [Status] NVARCHAR(50) NULL
)
