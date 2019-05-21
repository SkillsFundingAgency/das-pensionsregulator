CREATE TABLE [dbo].[Organisation]
(
	[TPR_Unique_Id] BIGINT NOT NULL UNIQUE, 
    [Employer_SK] INT Identity(1,1) PRIMARY KEY ,
    [OrganisationName] VARCHAR(255) NOT NULL, 
    [CompanyRegNo] VARCHAR(25) NULL, 
    [Status] VARCHAR(30) NULL

)
