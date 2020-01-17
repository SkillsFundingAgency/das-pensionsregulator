CREATE PROCEDURE [dbo].[ImportDataFromFile]
(@Run_Id bigint,@DataSource varchar(255))
AS
-- =========================================================================
-- Author:      Himabindu Uddaraju
-- Create Date: 10/07/2019
-- Description: Imports Data to Staging Table
-- =========================================================================
BEGIN TRY

DECLARE @vSQL NVARCHAR(MAX)
DECLARE @DateStamp VARCHAR(10)
DECLARE @LogID int

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
	   ,'Step-2'
	   ,'ImportDataFromFile'
	   ,getdate()
	   ,0

  
  SELECT @LogID=MAX(LogId) FROM Mgmt.Log_Execution_Results 
   WHERE StoredProcedureName='ImportDataFromFile'
     AND RunId=@Run_ID


  /* Choose the Format File */

  DECLARE @FormatFile varchar(255)

 -- SET @FormatFile='C:\Users\huddaraju\Documents\Hima_Docs\Testing_TPRStrategic\TPRFormatFile_v2.fmt'

  SET @FormatFile='TPRFormatFile.fmt'

  /* Truncate Staging Table */

  TRUNCATE TABLE Tpr.StagingData

  /* Drop Existing Index before the load */

   DROP INDEX IF EXISTS NCI_Staging_TPR ON Tpr.StagingData



/* Get list of Files on Blob Storage and loop around to load the files into Staging */
  
      IF OBJECT_ID('Tempdb..#tFiles') IS NOT NULL
    DROP TABLE #tFiles

  CREATE TABLE #tFiles
 (ID INT UNIQUE NOT NULL
  ,Value Varchar(255)
  )

  INSERT INTO #tFiles(ID,Value)
  SELECT SFL.[SourceFileId]
        ,SFL.[FileName]
    FROM (select *
                 ,row_number() over (partition by FileName order by FileUploadedDateTime desc) rn
            from mgmt.SourceFileList) SFL
   WHERE Cast(FileUploadedDateTime as Date)
 BETWEEN Cast(DATEADD(DAY,-6,GETDATE()) as Date)
     and Cast(GETDATE() as Date)
	 and ISNULL(LoadedToStaging,0)<>1
	 and ISNULL(FileProcessed,0)<>1
	 and SFL.FileName like '%.txt%'
	 and SFL.FileName not like '%.fmt%'
	 and SFL.rn=1

 DECLARE @Counter Int
  SELECT @Counter=MIN(ID) FROM #tFiles

   WHILE (@Counter<=(SELECT MAX(ID) FROM #tFiles))
   BEGIN 
 DECLARE @FileName Varchar(255)
  SELECT @FileName=Value 
    FROM #tFiles
   WHERE ID=@Counter


/* Update SourceFileList with the  RunId that's processing the File */
 UPDATE SFL
    SET SFL.RunId=@Run_Id
   FROM Mgmt.SourceFileList SFL
  WHERE SFL.SourceFileID=@Counter


 DECLARE @ExecuteSQL1 NVARCHAR(MAX)
 DECLARE @ExecuteSQL2 NVARCHAR(MAX)

     SET @ExecuteSQL1='

	INSERT INTO [Tpr].[StagingData]
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
		   ,[SignalCode]
		   ,[SignalChanged]
           ,[SignalStartYear]
		   ,[SignalEndYear]
		   ,[SignalValue]
		   ,[RecordType7]
		   ,[EmployerSchemeHistoryUniqueID]
		   ,[EmployerSchemeHistStartYear]
		   ,[EmployerSchemeHistEndYear]
		   ,[SchemeType]
		   ,[RecordType12]
		   ,[EmployerCountID]
		   ,[EEC_Code]
		   ,[DateTaken]
		   ,[NumberCounted]
		   ,[SourceFileName]
		   ,[RunID])
    SELECT tpr.[RecordType1]
	      ,tpr.[TPRUniqueID]
		  ,tpr.[DistrictNumber]
		  ,tpr.[Reference]
		  ,tpr.[AODistrict]
		  ,tpr.[AOTaxType]
		  ,tpr.[AOCheckChar]
		  ,tpr.[AOReference]
		  ,tpr.[StartDate]
		  ,tpr.[EndDate]
		  ,tpr.[EndDateCode]
		  ,tpr.[RestartDate]
		  ,tpr.[ReviewYr]
		  ,tpr.[CompanyRegNo]
		  ,tpr.[TradeClass]
          ,tpr.[AOInd]
		  ,tpr.[XferType]
		  ,tpr.[UniqueRefDistrict]
		  ,tpr.[UniqueReference]
		  ,tpr.[RecordType2]
		  ,tpr.[Name1]
		  ,tpr.[Name2]
		  ,tpr.[TelephoneNumber]
		  ,tpr.[EmailAddress]
          ,tpr.[AddressType]
		  ,tpr.[ADI]
		  ,tpr.[AddressLine1]
		  ,tpr.[AddressLine2]
		  ,tpr.[AddressLine3]
		  ,tpr.[AddressLine4]
		  ,tpr.[AddressLine5]
		  ,tpr.[PostCode]
		  ,tpr.[ForeignCountry]
		  ,tpr.[AddressVerifiedIndicator]
		  ,tpr.[RecordType3]
		  ,tpr.[SignalUniqueID]
		  ,tpr.[SignalCode]
		  ,tpr.[SignalChanged]
		  ,tpr.[SignalStartYear]
		  ,tpr.[SignalEndYear]
		  ,tpr.[SignalValue]
		  ,tpr.[RecordType7]
		  ,tpr.[EmployerSchemeHistoryUniqueID]
		  ,tpr.[EmployerSchemeHistStartYear]
		  ,tpr.[EmployerSchemeHistEndYear]
		  ,tpr.[SchemeType]
		  ,tpr.[RecordType12]
		  ,tpr.[EmployerCountID]
		  ,tpr.[EEC_Code]
		  ,tpr.[DateTaken]
		  ,tpr.[NumberCounted]
		  ,'''+@FileName+'''
		  ,'+CAST(@Run_Id AS VARCHAR(15))+'
    FROM OPENROWSET (
		             BULK '''+@FileName+'''
		            ,DATA_SOURCE='''+@DataSource+'''
                    ,FIRSTROW=2
					,FORMATFILE='''+@FormatFile+'''
				    ,FORMATFILE_DATA_SOURCE='''+@DataSource+'''
		             ) AS tpr;		
					 
'

SET @ExecuteSQL2='

/* Update Counts in RecordCounts */

   INSERT INTO Mgmt.Log_Record_Counts
   (LogId,RunId,SourceTableName,TargetTableName,SourceRecordCount,TargetRecordCount)
   SELECT '''+CAST(@LogID AS VARCHAR(15))+'''
         ,'''+CAST(@Run_Id AS VARCHAR(15))+'''
		 ,'''+@FileName+'''
	     ,''StagingData''
		 ,(SELECT COUNT(*) FROM OPENROWSET (
		                                     BULK '''+@FileName+'''
		                                    ,DATA_SOURCE='''+@DataSource+'''
                                            ,FIRSTROW=2
					                        ,FORMATFILE='''+@FormatFile+'''
				                            ,FORMATFILE_DATA_SOURCE='''+@DataSource+'''
		                                   ) AS tpr
           )
		 ,(SELECT COUNT(*) FROM Tpr.StagingData WHERE SourceFileName='''+@FileName+''')

  /* Update Flag in SourceFileList as Loaded */

  UPDATE SFL
     SET SFL.LoadedToStaging=1
	    ,SFL.StagingLoadDate=getdate()
	--	,SFL.RunId='+CAST(@Run_Id AS VARCHAR(15))+'
    FROM Mgmt.SourceFileList SFL
   WHERE SFL.SourceFileID='''+cast(@counter as varchar(15))+'''
			 			 
'

   EXEC (@ExecuteSQL1+@ExecuteSQL2)


     SET @Counter=@Counter+1
     END

/* Recreate the Index */

  CREATE NONCLUSTERED INDEX NCI_Staging_TPR
      ON Tpr.StagingData(TPRUniqueID)



/* Update Log Execution Results as Success if the query ran succesfully*/

UPDATE Mgmt.Log_Execution_Results
   SET Execution_Status=1
      ,EndDateTime=getdate()
	  ,FullJobStatus='Pending- Go To Step3 RunValidationChecks'
 WHERE LogId=@LogID
   AND RunID=@Run_Id


END TRY

BEGIN CATCH
  
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
	    'ImportDataFromFile' AS ErrorProcedure,
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


