/*
DECLARE @Date Date

SET @Date=getdate()

IF @Date < '2019-10-30'
BEGIN


DROP INDEX IF EXISTS [CI_TPR_UniqueKey] ON dbo.Stg_Tpr_1218


DROP INDEX IF EXISTS [NCI_TPR_AORN] ON dbo.Stg_Tpr_1218


DROP INDEX IF EXISTS [NCI_TPR_PAYE] ON dbo.Stg_Tpr_1218


DROP INDEX IF EXISTS [NCI_TPR_PAYE_AORN] ON dbo.Stg_Tpr_1218


DROP INDEX IF EXISTS [NCI_TPR_PAYE_DERIVED] ON dbo.Stg_Tpr_1218


END

*/

/* Drop OrganisationAddress Table as it's replaced with Tpr.OrganistionAddress */

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
            WHERE TABLE_NAME = N'OrganisationAddress'
		      AND TABLE_SCHEMA=N'dbo'
	      )
DROP TABLE dbo.OrganisationAddress

/* Drop OrganisationPAYEScheme Table as it's replaced with Tpr.OrganistionPAYEScheme */

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
            WHERE TABLE_NAME = N'OrganisationPAYEScheme'
		      AND TABLE_SCHEMA=N'dbo'
	      )
DROP TABLE dbo.OrganisationPAYEScheme

/* Drop Organisation Table as it's replaced with Tpr.Organistion */

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
            WHERE TABLE_NAME = N'Organisation'
		      AND TABLE_SCHEMA=N'dbo'
	      )
DROP TABLE dbo.Organisation

/* Drop OrganisationAddress Table as it's replaced with Tpr.OrganistionAddress */

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
            WHERE TABLE_NAME = N'OrganisationAddress'
		      AND TABLE_SCHEMA=N'shadow'
	      )
DROP TABLE Shadow.OrganisationAddress

/* Drop OrganisationPAYEScheme Table as it's replaced with Tpr.OrganistionPAYEScheme */

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
            WHERE TABLE_NAME = N'OrganisationPAYEScheme'
		      AND TABLE_SCHEMA=N'shadow'
	      )
DROP TABLE Shadow.OrganisationPAYEScheme

/* Drop Organisation Table as it's replaced with Tpr.Organistion */

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
            WHERE TABLE_NAME = N'Organisation'
		      AND TABLE_SCHEMA=N'shadow'
	      )
DROP TABLE Shadow.Organisation


/* Drop Staging_TPR Table as it's replaced with Tpr.Staging_TPR */

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
            WHERE TABLE_NAME = N'Staging_TPR'
		      AND TABLE_SCHEMA=N'dbo'
	      )
DROP TABLE dbo.Staging_TPR

/* Drop Staging_TPR_Rejected Table as it's replaced with Tpr.Staging_TPR_Rejected */

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
            WHERE TABLE_NAME = N'Staging_TPR_Rejected'
		      AND TABLE_SCHEMA=N'dbo'
	      )
DROP TABLE dbo.Staging_TPR_Rejected

/* Drop Staging_TPR_Rejected Table as it's replaced with Tpr.Tpr_StaigingHistory */

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
            WHERE TABLE_NAME = N'TPR_StagingHistory'
		      AND TABLE_SCHEMA=N'dbo'
	      )
DROP TABLE dbo.TPR_StagingHistory

/* Drop dbo.LoadTPRFile Stored Proc as it's not used in the New ADF Sol */

DROP PROCEDURE IF EXISTS dbo.LoadTPRFile;  

/* Drop dbo.LoadSrcFileDetails Stored Proc as it's not used in the New ADF Sol */

DROP PROCEDURE IF EXISTS dbo.LoadSrcFileDetails;  

/* Drop dbo.ImportDataFromFile Stored Proc as it's not used in the New ADF Sol */

DROP PROCEDURE IF EXISTS dbo.ImportDataFromFile;  






