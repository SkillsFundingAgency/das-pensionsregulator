CREATE PROCEDURE [dbo].[LoadTargetTables]
(@Run_Id bigint)
AS
-- ====================================================================================
-- Author:      Himabindu Uddaraju
-- Create Date: 10/07/2019
-- Description: Load Target Tables from Staging Post Validation Checks 
-- ====================================================================================
BEGIN TRY

DECLARE @vSQL NVARCHAR(MAX)
DECLARE @DateStamp VARCHAR(10)
DECLARE @LogID int
--DECLARE @Run_ID int

--SELECT @Run_ID= RunId FROM dbo.Staging_TPR

select @DateStamp =  CAST(CAST(YEAR(GETDATE()) AS VARCHAR)+RIGHT('0' + RTRIM(cast(MONTH(getdate()) as varchar)), 2) +RIGHT('0' +RTRIM(CAST(DAY(GETDATE()) AS VARCHAR)),2) as int)

/* Start Logging Execution */

  INSERT INTO Mgmt.Log_Execution_Results
	  (
	    RunId
	   ,StepNo
	   ,StoredProcedureName
	   ,StartDateTime
	   ,Execution_Status
	  )
  SELECT 
        @Run_Id
	   ,'Step-5'
	   ,'LoadTargetTables'
	   ,getdate()
	   ,0

  SELECT @LogID=MAX(LogId) FROM Mgmt.Log_Execution_Results 
   WHERE StoredProcedureName='LoadTargetTables'
     AND RunId=@Run_ID

  /* Insert Organisation Table */

  --set xact_abort on

  IF ((SELECT COUNT(*) FROM dbo.Staging_TPR) > 4500000)

  BEGIN

  IF EXISTS (SELECT * FROM Mgmt.Log_Execution_Results where StoredProcedureName='LoadTargetCloneTables' and Execution_Status=1 and RunId=@Run_Id)

  BEGIN

  --Swap schema
ALTER SCHEMA swap TRANSFER dbo.Organisation;
ALTER SCHEMA dbo TRANSFER shadow.Organisation;
ALTER SCHEMA shadow TRANSFER swap.Organisation;


ALTER SCHEMA swap TRANSFER dbo.OrganisationAddress;
ALTER SCHEMA dbo TRANSFER shadow.OrganisationAddress;
ALTER SCHEMA shadow TRANSFER swap.OrganisationAddress;

ALTER SCHEMA swap TRANSFER dbo.OrganisationPAYEScheme;
ALTER SCHEMA dbo TRANSFER shadow.OrganisationPAYEScheme;
ALTER SCHEMA shadow TRANSFER swap.OrganisationPAYEScheme;

BEGIN TRANSACTION

DELETE FROM shadow.OrganisationAddress
DELETE FROM shadow.OrganisationPAYEScheme
DELETE FROM shadow.Organisation

COMMIT TRANSACTION

END
END




/* Update Log Execution Results as Success if the query ran succesfully*/

UPDATE Mgmt.Log_Execution_Results
   SET Execution_Status=1
      ,EndDateTime=getdate()
	  ,FullJobStatus='Pending- Go To Step6 UpdateHistoryTable'
 WHERE LogId=@LogID
   AND RunID=@Run_Id


/* Update Source File List as Processed */

UPDATE SFL
   SET SFL.FileProcessed=1
  FROM Mgmt.SourceFileList SFL
 WHERE RunId=@Run_ID
  


END TRY

BEGIN CATCH

  IF @@TRANCOUNT > 0
  ROLLBACK TRAN
  
  DECLARE @ErrorId int

  INSERT INTO Mgmt.Log_Error_Details
	  (UserName
	  ,ErrorNumber
	  ,ErrorState
	  ,ErrorSeverity
	  ,ErrorLine
	  ,ErrorProcedure
	  ,ErrorMessage
	  ,ErrorDateTime
	  ,Run_Id
	  )
  SELECT 
        SUSER_SNAME(),
	    ERROR_NUMBER(),
	    ERROR_STATE(),
	    ERROR_SEVERITY(),
	    ERROR_LINE(),
	    'LoadTargetTables' AS ErrorProcedure,
	    ERROR_MESSAGE(),
	    GETDATE(),
		@Run_Id as RunId; 

  SELECT @ErrorId=MAX(ErrorId) FROM Mgmt.Log_Error_Details

/* Update Log Execution Results as Fail if there is an Error*/

UPDATE Mgmt.Log_Execution_Results
   SET Execution_Status=0
      ,EndDateTime=getdate()
	  ,ErrorId=@ErrorId
 WHERE LogId=@LogID
   AND RunID=@Run_Id

END CATCH
GO


