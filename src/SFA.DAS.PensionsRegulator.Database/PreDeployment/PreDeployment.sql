/* Drop Existing Tables and Re-Build */

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
            WHERE TABLE_NAME = N'Organisation'
		      AND TABLE_SCHEMA=N'dbo'
	      )
DROP TABLE dbo.Organisation


IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
            WHERE TABLE_NAME = N'OrganisationAddress'
		      AND TABLE_SCHEMA=N'dbo'
	      )
DROP TABLE dbo.OrganisationAddress

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
            WHERE TABLE_NAME = N'OrganisationPAYEScheme'
		      AND TABLE_SCHEMA=N'dbo'
	      )
DROP TABLE dbo.OrganisationPAYEScheme