CREATE PROCEDURE [dbo].[LoadTPRFile]

AS
-- ==========================================================
-- Author:      Himabindu Uddaraju
-- Create Date: 29/07/2019
-- Description: Master Stored Proc that Imports Data From File
--              Executes ETL and Loads History Table
-- ==========================================================

/* Generate Run Id for Logging Execution */

DECLARE @RunId int
DECLARE @DataSource varchar(255)
DECLARE @RetentionDate datetime

SELECT @RetentionDate=DATEADD(MONTH, -3, GETDATE()) 

SET @DataSource='dastprStorConnection'

/* Generate Run Id */

EXEC @RunId= dbo.GenerateRunId



/* Import Data to Staging from File */

EXEC [dbo].[ImportDataFromFile] @RunId,@DataSource



/* Run Validation Checks if Import File is Successful */

IF EXISTS (SELECT * FROM Mgmt.Log_Execution_Results where StoredProcedureName='ImportDataFromFile' and Execution_Status=1 and RunId=@RunId)
BEGIN 
EXEC dbo.RunValidationChecks @RunId
END
ELSE RAISERROR( 'Import Data From File Failed -Check Log Table For Errors',1,1)



/* Import Valid Records to Target Tables */

IF EXISTS (SELECT * FROM Mgmt.Log_Execution_Results where StoredProcedureName='RunValidationChecks' and Execution_Status=1 and RunId=@RunId) 
BEGIN
EXEC dbo.LoadTargetTables @RunId
END
ELSE RAISERROR( 'Validation Checks Failed-Check Log Table For Errors',1,1)



/* Update History Table with Processed File */

IF EXISTS (SELECT * FROM Mgmt.Log_Execution_Results where StoredProcedureName='LoadTargetTables' and Execution_Status=1 and RunId=@RunId)
BEGIN
EXEC dbo.UpdateHistoryTable @RunId,@RetentionDate
END
ELSE RAISERROR( 'Loading Target Tables Failed-Check Log Table For Errors',1,1)




/* Raise Error if Updating History Table Failed */

IF EXISTS (SELECT * FROM Mgmt.Log_Execution_Results where StoredProcedureName='UpdateHistoryTable' and Execution_Status<>1 and RunId=@RunId)
BEGIN
RAISERROR( 'Updating History Table Failed-Check Log Table For Errors',1,1)
END