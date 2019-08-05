CREATE PROCEDURE [dbo].[uSP_RunValidationChecks]
AS
-- ====================================================================================
-- Author:      Himabindu Uddaraju
-- Create Date: 10/07/2019
-- Description: Run Validation Checks before loading Main Tables to Flag Invalid Rows 
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
	   ,'Step-3'
	   ,'uSP_RunValidationChecks'
	   ,getdate()
	   ,0

  SELECT @LogID=MAX(LogId) FROM Mgmt.Log_Execution_Results


  /* Delete existing rejected rows for the same Run id */

  DELETE STPR
    FROM dbo.Staging_TPR_Rejected STPR
   WHERE STPR.RunId=@Run_ID

/* String Tests */

  DECLARE @ColumnName VARCHAR(255), @ColumnType VARCHAR(255), @ColumnLength INT, @TestName VARCHAR(255), @ErrorMessage VARCHAR(255), 
				@ColumnStopOnErrorFlag BIT, 
				@ColumnPrecision INT,
				@ColumnPatternMatching nvarchar(255),
				@Sql nvarchar(2000),
				@ColumnMinValue varchar(255), 
				@ColumnMaxValue varchar(255) 

			DECLARE StringTestConfig CURSOR FOR
			SELECT DVR.ColumnName
				,DVR.ColumnType
				, DVR.ColumnLength
				, 'String Length Test'
				, 'String length exceeds Specification.' AS ErrorMessage
			FROM Mgmt.Data_Validation_Rules AS DVR
			WHERE DVR.ColumnType IN ('VARCHAR', 'NVARCHAR','CHAR','NCHAR')
			  AND RunChecks=1 and RunTextLengthChecks=1 

			OPEN StringTestConfig
			FETCH NEXT FROM StringTestConfig INTO
			@ColumnName, @ColumnType, @ColumnLength, @TestName, @ErrorMessage

			WHILE @@FETCH_STATUS = 0
			BEGIN
				   SET @SQL='
				   INSERT INTO dbo.Staging_TPR_Rejected
				  ( SourceSK,RunId,TPRUniqueID, ColumnName, TestName, ErrorMessage ) 
				   SELECT SourceSK,
				          RunId,
						  TPRUniqueID,
						'''+ @ColumnName + ''' AS ColumnName, 
						'''+ @TestName + ''' AS TestName,
						'''+ @ErrorMessage +' Actual: '' + CAST(LEN('+ @ColumnName + ')AS VARCHAR(255))+ '' Against spec size: '+CAST(@columnLength AS VARCHAR(255)) + ''' AS ErrorMessage
					FROM dbo.Staging_TPR
				   WHERE LEN(REPLACE('+ @ColumnName + ',''NULL'','''')) > '+ CAST(@ColumnLength AS varchar(255)) +''
	   
				EXEC (@SQL)

				FETCH NEXT FROM StringTestConfig INTO
				@ColumnName, @ColumnType, @ColumnLength, @TestName, @ErrorMessage
			END

			CLOSE StringTestConfig
			DEALLOCATE StringTestConfig

print 'STRING TESTS COMPLETE'

			-- End String length test

-- String not null tests

			DECLARE StringNotNullConfig CURSOR FOR
			SELECT DVR.ColumnName
				,DVR.ColumnType
				, DVR.ColumnLength
				, 'String Not Null Test'
				, 'String Not Null is null.' AS ErrorMessage
			FROM Mgmt.Data_Validation_Rules AS DVR
			WHERE DVR.ColumnType IN ('VARCHAR', 'NVARCHAR','CHAR','NCHAR')
			  AND RunChecks=1 and RunTextLengthChecks=1 and ColumnNullable=0

			OPEN StringNotNullConfig
			FETCH NEXT FROM StringNotNullConfig INTO
			@ColumnName, @ColumnType, @ColumnLength, @TestName, @ErrorMessage

			WHILE @@FETCH_STATUS = 0
			BEGIN
				   SET @SQL='
				   INSERT INTO dbo.Staging_TPR_Rejected
				  ( SourceSK,RunId,TPRUniqueID, ColumnName, TestName, ErrorMessage ) 
				   SELECT SourceSK,
				          RunId,
						  TPRUniqueID,
						'''+ @ColumnName + ''' AS ColumnName, 
						'''+ @TestName + ''' AS TestName,
						'''+ @ErrorMessage +''' AS ErrorMessage
					FROM dbo.Staging_TPR
				   WHERE REPLACE('+ @ColumnName + ',''NULL'','''') IS NULL'
	   
				EXEC (@SQL)

				FETCH NEXT FROM StringNotNullConfig INTO
				@ColumnName, @ColumnType, @ColumnLength, @TestName, @ErrorMessage
			END

			CLOSE StringNotNullConfig
			DEALLOCATE StringNotNullConfig

print 'STRING not null tests COMPLETE'

-- Decimal place test

			DECLARE DecimalPlacesTestConfig CURSOR FOR
			SELECT DVR.ColumnName
				,DVR.ColumnType
				, DVR.ColumnLength
				, 'Decimal Places Test'
				, 'Decimal places do not match specification.' AS ErrorMessage
				, DVR.ColumnPrecision
			FROM Mgmt.Data_Validation_Rules AS DVR
		   WHERE ColumnType = 'DECIMAL'
		     AND RunChecks=1 and RunDecimalPlacesCheck=1

			OPEN DecimalPlacesTestConfig
			FETCH NEXT FROM DecimalPlacesTestConfig INTO 
			@ColumnName, @ColumnType, @ColumnLength, @TestName, @ErrorMessage, @ColumnPrecision

			WHILE @@FETCH_STATUS = 0
			BEGIN
				SET @SQL='
				INSERT INTO dbo.Staging_TPR_Rejected
				 ( SourceSK,RunId,TPRUniqueID, ColumnName, TestName, ErrorMessage ) 
				SELECT SourceSK,
				       RunID,
					   TPRUniqueID,
					'''+ @ColumnName + ''' AS ColumnName, 
					'''+ @TestName + ''' AS TestName,
					'''+ @ErrorMessage +' Actual: ''+ CAST('+ @ColumnName +' AS VARCHAR(255)) +'' Expected Decimal Places: '+ CAST(@ColumnPrecision  AS VARCHAR(255)) +''' AS ErrorMessage, 
				FROM dbo.Staging_TPR
				WHERE ISNUMERIC(COALESCE(LTRIM(RTRIM('+ @ColumnName + ')),''0'')) = 1
					AND LEN('+ @ColumnName + ') > 0
					AND (CHARINDEX(''.'','+ @ColumnName + ',1) =  0
					OR LEN(RIGHT('+ @ColumnName + ',LEN('+ @ColumnName + ')-CHARINDEX(''.'','+ @ColumnName + ',1))) <> '+ CAST(@ColumnPrecision  AS VARCHAR(255)) +')'
				
				EXEC (@SQL)

				FETCH NEXT FROM DecimalPlacesTestConfig INTO 
				@ColumnName, @ColumnType, @ColumnLength, @TestName, @ErrorMessage, @ColumnPrecision
			END

			CLOSE DecimalPlacesTestConfig
			DEALLOCATE DecimalPlacesTestConfig
			   

print 'DECIMAL TESTS COMPLETE'

-- IsNumeric and Nullable match test
  

			DECLARE IsNumericTestConfig CURSOR FOR
			SELECT DVR.ColumnName
				,DVR.ColumnType
				, DVR.ColumnLength
				, 'IsNumeric Test'
				, 'Numeric type field not Numeric.' AS ErrorMessage
			FROM Mgmt.Data_Validation_Rules AS DVR
			WHERE   DVR.ColumnType IN ('BIT', 'BIGINT','Long','Int','DECIMAL','SMALLINT', 'TINYINT')
			 AND RunChecks=1 and RunNumericChecks=1 and ColumnNullable=1

			OPEN IsNumericTestConfig
			FETCH NEXT FROM IsNumericTestConfig INTO 
			@ColumnName, @ColumnType, @ColumnLength, @TestName, @ErrorMessage

			WHILE @@FETCH_STATUS = 0
			BEGIN
				SET @SQL='
				INSERT INTO dbo.Staging_TPR_Rejected
					  ( SourceSK,RunId,TPRUniqueID, ColumnName, TestName, ErrorMessage
					  ) 
			   SELECT SourceSK
			        , RunId
					, TPRUniqueID
					,'''+ @ColumnName + ''' AS ColumnName
				    , '''+ @TestName + ''' AS TestName
					, '''+ @ErrorMessage +' Actual: ''+['+@ColumnName+'] AS ErrorMessage
     			 FROM dbo.Staging_TPR
				WHERE ISNUMERIC(COALESCE(LTRIM(RTRIM(REPLACE('+ @ColumnName + ',''NULL'',''0''))),''0'')) = 0 
				  AND LEN('+ @ColumnName + ') > 0
		  
					   '
				
				EXEC (@SQL)

				FETCH NEXT FROM IsNumericTestConfig INTO 
				@ColumnName, @ColumnType, @ColumnLength, @TestName, @ErrorMessage
			END

			CLOSE IsNumericTestConfig
			DEALLOCATE IsNumericTestConfig

print 'NUMERIC TESTS COMPLETE'


-- Numeric not null  test

			DECLARE IsNumNotNullConfig CURSOR FOR
			SELECT DVR.ColumnName
				,DVR.ColumnType
				, DVR.ColumnLength
				, 'Number Not Null Test'
				, 'Not Null Fields contains Nulls.' AS ErrorMessage
			FROM Mgmt.Data_Validation_Rules AS DVR
			WHERE   DVR.ColumnType IN ('BIT', 'BIGINT','Long','Int','DECIMAL','SMALLINT', 'TINYINT')
			 AND RunChecks=1 and RunNumericChecks=1 and ColumnNullable=0


			OPEN IsNumNotNullConfig
			FETCH NEXT FROM IsNumNotNullConfig INTO 
			@ColumnName, @ColumnType, @ColumnLength, @TestName, @ErrorMessage

			WHILE @@FETCH_STATUS = 0
			BEGIN
				SET @SQL='
				INSERT INTO dbo.Staging_TPR_Rejected
					  ( SourceSK,RunId,TPRUniqueID, ColumnName, TestName, ErrorMessage
					  ) 
			   SELECT SourceSK
			        , RunId
					, TPRUniqueID
					,'''+ @ColumnName + ''' AS ColumnName
				    , '''+ @TestName + ''' AS TestName
					, '''+ @ErrorMessage +' Actual: ''+['+@ColumnName+'] AS ErrorMessage
     			 FROM dbo.Staging_TPR
				WHERE LTRIM(RTRIM(REPLACE('+ @ColumnName + ',''NULL'',0))) IS NULL 
 
					   '
				
				EXEC (@SQL)



				FETCH NEXT FROM IsNumNotNullConfig INTO 
				@ColumnName, @ColumnType, @ColumnLength, @TestName, @ErrorMessage
			END

			CLOSE IsNumNotNullConfig
			DEALLOCATE IsNumNotNullConfig

print 'NUMERIC NOT  NULL TESTS COMPLETE'

 		   
-- Date Checks

           DECLARE IsDateTestConfig CURSOR FOR
			SELECT DVR.ColumnName
				,DVR.ColumnType
				, DVR.ColumnLength
				, 'IsDate Test'
				, 'Date type field not Date.' AS ErrorMessage
			FROM Mgmt.Data_Validation_Rules AS DVR
			WHERE   DVR.ColumnType IN ('DATE')
			  AND RunChecks=1 and RunDataChecks=1

			OPEN IsDateTestConfig
			FETCH NEXT FROM IsDateTestConfig INTO 
			@ColumnName, @ColumnType, @ColumnLength, @TestName, @ErrorMessage

			WHILE @@FETCH_STATUS = 0
			BEGIN
				SET @SQL='
				INSERT INTO dbo.Staging_TPR_Rejected
					  ( SourceSK,RunId,TPRUniqueID, ColumnName, TestName, ErrorMessage
					  ) 
			   SELECT SourceSK
			        , RunId
					, TPRUniqueID
					,'''+ @ColumnName + ''' AS ColumnName
				    , '''+ @TestName + ''' AS TestName
					, '''+ @ErrorMessage +' Actual: ''+['+@ColumnName+'] AS ErrorMessage
     			 FROM dbo.Staging_TPR
				WHERE ISDATE(COALESCE(LTRIM(RTRIM(REPLACE(REPLACE('+ @ColumnName + ',''NULL'',''1900-01-01''),''0001-01-01'',''1973-01-01''))),''0'')) = 0 	  
					   '
				
				EXEC (@SQL)

				FETCH NEXT FROM IsDateTestConfig INTO 
				@ColumnName, @ColumnType, @ColumnLength, @TestName, @ErrorMessage
			END

			CLOSE IsDateTestConfig
			DEALLOCATE IsDateTestConfig			

print 'DATE TESTS COMPLETE'

/* Update Staging Tables with Valid/Invalid Flag */

   
   UPDATE STPR
      SET STPR.IsValid=0
	     ,STPR.InvalidReason=STPRR.ErrorMessage
	 FROM dbo.Staging_TPR STPR
	 JOIN dbo.Staging_TPR_Rejected STPRR
	   ON STPR.SourceSK=STPRR.SourceSK



/* Update Log Execution Results as Success if the query ran succesfully*/

UPDATE Mgmt.Log_Execution_Results
   SET Execution_Status=1
      ,EndDateTime=getdate()
	  ,FullJobStatus='Pending- Go To Step4 uSP_LoadTargetTables'
 WHERE LogId=@LogID
   AND RunID=@Run_Id

/* Update Invalid Record Counts in Log Table */

   INSERT INTO Mgmt.Log_Record_Counts
   (LogId,RunId,SourceTableName,TargetTableName,SourceRecordCount,TargetRecordCount,InvalidRecordCount)
   SELECT @LogID
         ,@Run_Id
		 ,'Staging_TPR'
	     ,'Staging_TPR_Rejected'
		 ,(SELECT COUNT(*) FROM dbo.Staging_TPR WHERE RunID=@Run_ID)
		 ,(SELECT COUNT(*) FROM dbo.Staging_TPR WHERE RunID=@Run_ID and IsValid=0)
         ,(SELECT COUNT(*) FROM dbo.Staging_TPR_Rejected WHERE RunId=@Run_ID)		                                   



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
	    'uSP_RunValidationChecks' AS ErrorProcedure,
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




