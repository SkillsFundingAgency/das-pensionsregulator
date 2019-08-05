CREATE PROCEDURE [dbo].[uSP_UpdateHistoryTable]
(@RetentionDate Date)
AS
-- ====================================================================================
-- Author:      Himabindu Uddaraju
-- Create Date: 10/07/2019
-- Description: Update History Table with the Current Processed File and Remove any
--              History that's existing for more than 6 weeks as Retention Policy
-- ====================================================================================
BEGIN TRY

DECLARE @vSQL NVARCHAR(MAX)
DECLARE @DateStamp VARCHAR(10)
DECLARE @LogID int
DECLARE @Run_ID int

SELECT @Run_ID= RunId FROM dbo.Staging_TPR

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
	   ,'uSP_UpdateHistoryTable'
	   ,getdate()
	   ,0

   SELECT @LogID=MAX(LogId) FROM Mgmt.Log_Execution_Results 
   WHERE StoredProcedureName='uSP_UpdateHistoryTable'
     AND RunId=@Run_ID


/* Update History Table with Latest Processed File */

INSERT INTO [dbo].[TPR_StagingHistory]
           ([RecordType1]
           ,[TPRUniqueID]
           ,[DistrictNumber]
           ,[Reference]
           ,[AODistrict]
           ,[AOTaxType]
           ,[AOCheckChar]
           ,[AOReference]
           ,[StartDate]
           ,[EndDate]
           ,[EndDateCode]
           ,[RestartDate]
           ,[ReviewYr]
           ,[CompanyRegNo]
           ,[TradeClass]
           ,[AOInd]
           ,[XferType]
           ,[UniqueRefDistrict]
           ,[UniqueReference]
           ,[RecordType2]
           ,[AddressId]
           ,[AddressDistrictNumber]
           ,[AddressReference]
           ,[AddressCode]
           ,[AddressFromDate]
           ,[AddressToDate]
           ,[Name1]
           ,[Name2]
           ,[TelephoneNumber]
           ,[EmailAddress]
           ,[AddressType]
           ,[ADI]
           ,[AddressLine1]
           ,[AddressLine2]
           ,[AddressLine3]
           ,[AddressLine4]
           ,[AddressLine5]
           ,[PostCode]
           ,[ForeignCountry]
           ,[AddressVerifiedIndicator]
           ,[RecordType3]
           ,[SignalUniqueID]
           ,[SignalDistrictNumber]
           ,[SignalReference]
           ,[SignalCode]
           ,[SignalChanged]
           ,[SignalStartYear]
           ,[SignalEndYear]
           ,[SignalValue]
           ,[RecordType7]
           ,[EmployerSchemeHistoryUniqueID]
           ,[EmployerSchemeDistrictNumber]
           ,[EmployerSchemeReference]
           ,[EmployerSchemeHistStartYear]
           ,[EmployerSchemeHistEndYear]
           ,[SchemeType]
           ,[RecordType12]
           ,[EmployerCountDistrictNumber]
           ,[EmployerCountReference]
           ,[EmployerCountID]
           ,[EEC_Code]
           ,[DateTaken]
           ,[NumberCounted]
           ,[SourceSK]
           ,[SourceFileName]
           ,[RunID]
           ,[IsValid]
           ,[InvalidReason]
           ,[RecordCreatedDate])
   SELECT [RecordType1]
           ,[TPRUniqueID]
           ,[DistrictNumber]
           ,[Reference]
           ,[AODistrict]
           ,[AOTaxType]
           ,[AOCheckChar]
           ,[AOReference]
           ,[StartDate]
           ,[EndDate]
           ,[EndDateCode]
           ,[RestartDate]
           ,[ReviewYr]
           ,[CompanyRegNo]
           ,[TradeClass]
           ,[AOInd]
           ,[XferType]
           ,[UniqueRefDistrict]
           ,[UniqueReference]
           ,[RecordType2]
           ,[AddressId]
           ,[AddressDistrictNumber]
           ,[AddressReference]
           ,[AddressCode]
           ,[AddressFromDate]
           ,[AddressToDate]
           ,[Name1]
           ,[Name2]
           ,[TelephoneNumber]
           ,[EmailAddress]
           ,[AddressType]
           ,[ADI]
           ,[AddressLine1]
           ,[AddressLine2]
           ,[AddressLine3]
           ,[AddressLine4]
           ,[AddressLine5]
           ,[PostCode]
           ,[ForeignCountry]
           ,[AddressVerifiedIndicator]
           ,[RecordType3]
           ,[SignalUniqueID]
           ,[SignalDistrictNumber]
           ,[SignalReference]
           ,[SignalCode]
           ,[SignalChanged]
           ,[SignalStartYear]
           ,[SignalEndYear]
           ,[SignalValue]
           ,[RecordType7]
           ,[EmployerSchemeHistoryUniqueID]
           ,[EmployerSchemeDistrictNumber]
           ,[EmployerSchemeReference]
           ,[EmployerSchemeHistStartYear]
           ,[EmployerSchemeHistEndYear]
           ,[SchemeType]
           ,[RecordType12]
           ,[EmployerCountDistrictNumber]
           ,[EmployerCountReference]
           ,[EmployerCountID]
           ,[EEC_Code]
           ,[DateTaken]
           ,[NumberCounted]
           ,[SourceSK]
           ,[SourceFileName]
           ,[RunID]
           ,[IsValid]
           ,[InvalidReason]
           ,[RecordCreatedDate]
	FROM dbo.Staging_TPR stpr
   WHERE NOT EXISTS (SELECT 1
                       FROM dbo.TPR_StagingHistory tsh
					  WHERE tsh.SourceSK=stpr.SourceSK
					    AND tsh.RunId=stpr.RunID)

   /* Update Record Counts */

   INSERT INTO Mgmt.Log_Record_Counts
   (LogId,RunId,SourceTableName,TargetTableName,SourceRecordCount,TargetRecordCount)
   SELECT @LogID
         ,@Run_Id
		 ,'Staging_TPR'
	     ,'TPR_StagingHistory'
		 ,(SELECT COUNT(*) FROM dbo.Staging_TPR WHERE RunID=@Run_ID)
         ,(SELECT COUNT(*) FROM dbo.TPR_StagingHistory WHERE RunId=@Run_ID)	
  
						

 
 /* Remove History that is More than 6 weeks and Update Log Table*/

 Declare @ToDeleteRunId table
 (ToDeleteRID INT)

 INSERT INTO @ToDeleteRunId(ToDeleteRID)
 SELECT RunId 
   FROM dbo.TPR_StagingHistory TPRHist
  WHERE RecordCreatedDate < @RetentionDate

 DELETE TPRHist
   FROM dbo.TPR_StagingHistory TPRHist
  WHERE RecordCreatedDate < @RetentionDate

 UPDATE SFL
    SET SFL.FileRemovedFromHistory=1
   FROM Mgmt.SourceFileList SFL
  WHERE SFL.RunID IN (SELECT ToDeleteRID FROM @ToDeleteRunId)


/* Update Source File List as Processed */

UPDATE SFL
   SET SFL.FileLoadedToHistory=1
  FROM Mgmt.SourceFileList sfl
 WHERE RunId=@Run_ID
  

 

/* Update Log Execution Results as Success if the query ran succesfully*/

UPDATE Mgmt.Log_Execution_Results
   SET Execution_Status=1
      ,EndDateTime=getdate()
	  ,FullJobStatus='Finish'
 WHERE LogId=@LogID
   AND RunID=@Run_Id


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
	    'uSP_UpdateHistoryTable' AS ErrorProcedure,
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