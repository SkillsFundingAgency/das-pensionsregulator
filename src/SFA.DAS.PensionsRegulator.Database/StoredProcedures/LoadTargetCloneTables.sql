CREATE PROCEDURE [dbo].[LoadTargetCloneTables]
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
	   ,'Step-4'
	   ,'LoadTargetCloneTables'
	   ,getdate()
	   ,0

  SELECT @LogID=MAX(LogId) FROM Mgmt.Log_Execution_Results 
   WHERE StoredProcedureName='LoadTargetCloneTables'
     AND RunId=@Run_ID

  /* Insert Organisation Table */

  set xact_abort on

  IF ((SELECT COUNT(*) FROM Tpr.Staging_Data) > 4500000) --- Checking if the No. Of Valid Records Count are within the threshold, If not don't refresh

  BEGIN

  --BEGIN TRANSACTION

  /* Drop Existing Indexes and Rebuild later after the load is complete */

  DROP INDEX IF EXISTS NCI_Organisation_AORN ON ShadowTpr.Organisation

  DROP INDEX IF EXISTS NCI_Organisation_PAYEScheme ON ShadowTpr.OrganisationPAYEScheme

  DROP INDEX IF EXISTS NCI_Organisation_SK ON ShadowTpr.Organisation

  DROP INDEX IF EXISTS NCI_Organisation_SK ON ShadowTpr.OrganisationAddress

  DROP INDEX IF EXISTS NCI_Organisation_SK ON ShadowTpr.OrganisationPAYEScheme

  --ALTER INDEX PK_Org_OrgSK ON Shadow.Organisation DISABLE

  --ALTER INDEX PK_OrgAdd_OrgAddSK ON Shadow.OrganisationAddress DISABLE;  

  --ALTER INDEX PK_OrgPAYEScheme_OrgSchemeSK ON Shadow.OrganisationPAYEScheme DISABLE;  

  /* Clear Existing Tables before Load */
  
  TRUNCATE TABLE ShadowTpr.OrganisationPAYEScheme
  TRUNCATE TABLE ShadowTpr.OrganisationAddress
  TRUNCATE TABLE ShadowTpr.Organisation
  
  /* Break down the Insert Statement */

	--DECLARE @SourceSK INT
 --   DECLARE @Loop INT

 --   SET @SourceSK = 0
 --   SET @Loop = 1

 --   WHILE @Loop = 1
 --   BEGIN
   
  /* Start Data Load into Main Tables */

  INSERT INTO [ShadowTpr].[Organisation]
           ([TPRUniqueId]
           ,[OrganisationName]
           ,[DistrictNumber]
           ,[Reference]
           ,[CompanyRegNo]
           ,[TradeClass]
           ,[AODistrict]
           ,[AOTaxType]
           ,[AOCheckChar]
           ,[AOReference]
           ,[AORN]
           ,[AOInd]
           ,[AOIndDesc]
           ,[TransferType]
           ,[TransferTypeDesc]
           ,[UniqueRefDistrict]
           ,[UniqueReference]
           ,[SourceSK]
           ,[RunId]
           ,[SourceFileName]
           ,[RecordCreatedDate])
   SELECT  --TOP 100000
           TPRUniqueID
          ,COALESCE(LTRIM(RTRIM(REPLACE(Name1,'NULL',''))),'')+' '+COALESCE(LTRIM(RTRIM(REPLACE(Name2,'NULL',''))),'')
		  ,DistrictNumber
		  ,Reference
		  ,CASE WHEN CompanyRegNo='NULL' THEN NULL  ELSE CompanyRegNo END as CompanyRegNo
		  ,CASE WHEN TradeClass='NULL' THEN NULL ELSE TradeClass END  as TradeClass
		  ,cast(AODistrict as int) as AODistrict
		  ,AOTaxType as AOTaxType
		  ,AOCheckChar as AOCheckChar
		  ,AOReference as AOReference
		  ,right('000'+CAST(AODistrict AS VARCHAR(3)),3)+AOTaxType+AOCheckChar+AOReference as AORN
		  ,CASE WHEN AOInd='NULL' THEN NULL ELSE AOInd END as AOInd
		  ,CASE WHEN REPLACE(AOInd,'NULL','N/A')='Y' THEN  'The xfer is within an AO'
                WHEN REPLACE(AOInd,'NULL','N/A')='N' THEN 'The xfer is across AOs'
				ELSE 'Unknown'
		    END as AOIndDesc
		  ,CASE WHEN XferType='NULL' THEN NULL ELSE cast(XferType AS tinyint) END as TransferType
		  ,CASE WHEN REPLACE(XferType,'NULL',-1)=1 THEN 'Transfer'
                WHEN REPLACE(XferType,'NULL',-1)=2 THEN 'Merger'
                WHEN REPLACE(XferType,'NULL',-1)=3 THEN 'Succession'
                WHEN REPLACE(XferType,'NULL',-1)=4 THEN 'Reversal'
			    ELSE 'Unknown'
		    END as TransferTypeDesc
		  ,CASE WHEN UniqueRefDistrict='NULL' THEN NULL 
		        ELSE UniqueRefDistrict
				END as UniqueRefDistrict
		  ,CASE WHEN UniqueReference='NULL' THEN NULL
		        ELSE UniqueReference
				END as UniqueReference
		  ,SourceSK
		  ,RunID
		  ,SourceFileName
		  ,GETDATE()
     FROM Tpr.Staging_Data
	WHERE IsValid=1
	 -- AND SourceSK > @SourceSK
  --  ORDER BY SourceSK

	
  --  IF @@ROWCOUNT > 0 
  --      SELECT @SourceSK = MAX(SOURCESK) 
  --        FROM shadow.Organisation
  --  ELSE
  --      SET @Loop = 0

		--PRINT @SOURCESK
  --  END
	


/* Insert Organisation Address Table */


    --SET @SourceSK = 0
    --SET @Loop = 1

    --WHILE @Loop = 1
    --BEGIN

 INSERT INTO [ShadowTpr].[OrganisationAddress]
           ([OrgSK]
           ,[TPRUniqueID]
           ,[OrganisationFullAddress]
           ,[AddressLine1]
           ,[AddressLine2]
           ,[AddressLine3]
           ,[AddressLine4]
           ,[AddressLine5]
           ,[PostCode]
           ,[EmailAddress]
           ,[TelephoneNumber]
           ,[AddressType]
           ,[AddressTypeDesc]
           ,[ForeignCountry]
           ,[AddressVerifiedIndicator]
           ,[AddressVerifiedIndicatorDesc]
           ,[ADI]
           ,[SourceSK]
           ,[RunId]
           ,[SourceFileName]
           ,[RecordCreatedDate])
  SELECT --TOP 100000
         Org.OrgSK
        ,Stpr.TPRUniqueID
		,LTRIM(RTRIM(CASE WHEN Stpr.AddressLine1='NULL' THEN NULL ELSE Stpr.AddressLine1 END))+' ' 
		+LTRIM(RTRIM(CASE WHEN Stpr.AddressLine2='NULL' THEN NULL ELSE Stpr.AddressLine2 END))+' '
		+LTRIM(RTRIM(CASE WHEN Stpr.AddressLine3='NULL' THEN NULL ELSE Stpr.AddressLine3 END))+' '
		+LTRIM(RTRIM(CASE WHEN Stpr.AddressLine4='NULL' THEN NULL ELSE Stpr.AddressLine4 END))+' '
		+LTRIM(RTRIM(CASE WHEN Stpr.AddressLine5='NULL' THEN NULL ELSE Stpr.AddressLine5 END)) as FullAddress
		,LTRIM(RTRIM(CASE WHEN Stpr.AddressLine1='NULL' THEN NULL ELSE Stpr.AddressLine1 END)) as AddressLine1
		,LTRIM(RTRIM(CASE WHEN Stpr.AddressLine2='NULL' THEN NULL ELSE Stpr.AddressLine2 END)) as AddressLine2
		,LTRIM(RTRIM(CASE WHEN Stpr.AddressLine3='NULL' THEN NULL ELSE Stpr.AddressLine3 END)) as AddressLine3
		,LTRIM(RTRIM(CASE WHEN Stpr.AddressLine4='NULL' THEN NULL ELSE Stpr.AddressLine4 END)) as AddressLine4
		,LTRIM(RTRIM(CASE WHEN Stpr.AddressLine5='NULL' THEN NULL ELSE Stpr.AddressLine5 END)) as AddressLine5
		,LTRIM(RTRIM(CASE WHEN Stpr.PostCode='NULL' THEN NULL ELSE Stpr.PostCode END)) as PostCode
		,LTRIM(RTRIM(CASE WHEN Stpr.EmailAddress='NULL' THEN NULL ELSE Stpr.EmailAddress END)) as EmailAddress
		,LTRIM(RTRIM(CASE WHEN Stpr.TelephoneNumber='NULL' THEN NULL ELSE Stpr.TelephoneNumber END)) as TelephoneNumber
		,LTRIM(RTRIM(CASE WHEN Stpr.AddressType='NULL' THEN NULL ELSE Stpr.AddressType END)) as AddressType
		,CASE WHEN Stpr.AddressType=1 THEN 'UK' WHEN Stpr.AddressType=2 THEN 'NON-UK' ELSE 'Unknown' END as AddressTypeDesc
		,LTRIM(RTRIM(CASE WHEN Stpr.ForeignCountry='NULL' THEN NULL ELSE Stpr.ForeignCountry END)) as ForeignCountry
		,LTRIM(RTRIM(CASE WHEN Stpr.AddressVerifiedIndicator='NULL' THEN NULL ELSE Stpr.AddressVerifiedIndicator END)) as AddressVerifiedIndicator
		,CASE WHEN LTRIM(RTRIM(Stpr.AddressVerifiedIndicator))=1 THEN 'Verified by PAF' 
		      WHEN LTRIM(RTRIM(Stpr.AddressVerifiedIndicator))=3 THEN 'Input for Verification'
			  ELSE 'Unknown'
		  END as AddressVerifiedIndicatorDesc
		,LTRIM(RTRIM(CASE WHEN Stpr.ADI='NULL' THEN NULL ELSE Stpr.ADI END)) as ADI
		,stpr.SourceSK
		,stpr.RunID
		,stpr.SourceFileName
		,getdate() as insertdate
	--into #temp1
    FROM Tpr.Staging_Data stpr
	LEFT
	JOIN ShadowTpr.Organisation Org
	  ON stpr.SourceSK=Org.SourceSK
	 AND stpr.TPRUniqueID=Org.TPRUniqueId
   WHERE stpr.IsValid=1
   --  AND stpr.SourceSK > @SourceSK
   --ORDER BY stpr.SourceSK

	
   -- IF @@ROWCOUNT > 0 
   --     SELECT @SourceSK = MAX(SOURCESK) 
   --       FROM shadow.OrganisationAddress
   -- ELSE
   --     SET @Loop = 0
   -- END
	

   



	/* Insert into Organisation PAYEScheme */
    --SET @SourceSK = 0
    --SET @Loop = 1

    --WHILE @Loop = 1
    --BEGIN

	INSERT INTO [ShadowTpr].[OrganisationPAYEScheme]
           ([OrgSK]
           ,[TPRUniqueID]
           ,[PAYEScheme]
           ,[SchemeStartDate]
           ,[SchemeEndDate]
           ,[SchemeEndDateCode]
           ,[SchemeEndDateCodeDesc]
           ,[RestartDate]
           ,[ReviewYear]
           ,[LiveEmployeeCount]
           ,[EmployeeCountDateTaken]
           ,[EmployeeCountCode]
           ,[EmployeeCountCodeDesc]
           ,[SignalCode]
		   ,[SignalCodeDesc]
           ,[SignalChanged]
           ,[SignalChangedDesc]
           ,[SignalStartYear]
           ,[SignalEndYear]
           ,[SignalValue]
           ,[HistorySchemeStartYear]
           ,[HistorySchemeEndYear]
           ,[HistorySchemeType]
           ,[HistorySchemeTypeDesc]
           ,[SourceSK]
           ,[RunId]
           ,[SourceFileName]
           ,[RecordCreatedDate])
   SELECT --TOP 100000
          Org.OrgSK
         ,stpr.TPRUniqueId
		 ,ltrim(rtrim(stpr.DistrictNumber))+'/'+ltrim(rtrim(stpr.Reference))
		 ,CASE WHEN stpr.StartDate='NULL' THEN NULL ELSE CONVERT(VARCHAR,stpr.StartDate,23) END as SchemeStartDate
		 ,CASE WHEN stpr.EndDate='NULL' THEN NULL ELSE CONVERT(VARCHAR,stpr.EndDate,23)  END as SchemeEndDate
		 ,CASE WHEN stpr.EndDateCode='NULL' THEN NULL ELSE CAST(stpr.EndDateCode as tinyint) END as SchemeEndDateCode
		 ,CASE WHEN stpr.EndDateCode=0 THEN 'Not Closed' 
		       WHEN stpr.EndDateCode=1 THEN 'Transfer Out'
			   WHEN stpr.EndDateCode=2 THEN 'Merger Out'
			   WHEN stpr.EndDateCode=3 THEN 'Succession Out'
			   WHEN stpr.EndDateCode=4 THEN 'Ceased'
			   WHEN stpr.EndDateCode=5 THEN 'Cancelled'
			   ELSE 'Unknown'
		   END as SchemeStatus
		 ,CASE WHEN stpr.RestartDate='NULL' THEN NULL ELSE CONVERT(VARCHAR,stpr.RestartDate,23) END as RestartDate
		 ,CASE WHEN stpr.ReviewYr='NULL' THEN NULL ELSE stpr.ReviewYr END as ReviewYear
		 ,CASE WHEN stpr.NumberCounted='NULL' THEN NULL ELSE stpr.NumberCounted END as LiveEmployeeCount
		 ,CASE WHEN stpr.DateTaken='NULL' THEN NULL ELSE CONVERT(VARCHAR,stpr.DateTaken,23) END as EmployeeCountDateTaken
		 ,CASE WHEN stpr.EEC_Code='NULL' THEN NULL ELSE stpr.EEC_Code END as EmployeeCountCode
		 ,CASE WHEN stpr.EEC_Code='L' THEN 'Live' ELSE 'Not-Live' END as EmployeeCountCodeDesc
		 ,CASE WHEN stpr.SignalCode='NULL' THEN NULL ELSE stpr.SignalCode END as SignalCode
		 ,CASE WHEN stpr.SignalCode='AGNT' THEN 'Agent'
		       WHEN stpr.SignalCode='C1Y' THEN 'Cancel 1 Year Only(COYO)'
			   WHEN stpr.SignalCode='CRO' THEN 'Correspondence Address Is Registered Office'
			   WHEN stpr.SignalCode='RO' THEN 'Address is Registered Office'
			   WHEN stpr.SignalCode='DORM' THEN 'Dormant'
			   WHEN stpr.SignalCode='DEAD' THEN 'Deceased'
			   WHEN stpr.SignalCode='INSL' THEN 'Insolvent'
			   WHEN stpr.SignalCode='LONG' THEN 'Long Name'
			   WHEN stpr.SignalCode='POBF' THEN 'Post in BF Box'
			   WHEN stpr.SignalCode='ENFT' THEN 'Enforcement'
			   WHEN stpr.SignalCode='SPEC' THEN 'Special Care'
			   WHEN stpr.SignalCode='AURP' THEN 'Audit Report'
			   WHEN stpr.SignalCode='DSSR' THEN 'DSS Report'
			   WHEN stpr.SignalCode='RCAC' THEN 'Recovery Action'
			   WHEN stpr.SignalCode='PIOP' THEN 'PAYE Incorrect Operator'
			   ELSE 'Unknown'
		  END as SignalCodeDesc
		 ,CASE WHEN stpr.SignalChanged='NULL' THEN NULL ELSE stpr.SignalChanged END
		 ,CASE WHEN stpr.SignalChanged='C' THEN 'Created'
		       WHEN stpr.SignalChanged='U' THEN 'Updated'
			   WHEN stpr.SignalChanged='D' THEN 'Deleted'
			   ELSE 'Unknown'
			END as SignalChangedDesc
		 ,CAST((CASE WHEN stpr.SignalStartYear='NULL' THEN NULL ELSE stpr.SignalStartYear END)as Int) as SignalStartYear
		 ,CAST((CASE WHEN stpr.SignalEndYear='NULL' THEN NULL ELSE stpr.SignalEndYear END)as Int) as SignalEndYear
		 ,CASE WHEN stpr.SignalValue='NULL' THEN NULL ELSE stpr.SignalValue END
	     ,CAST((CASE WHEN stpr.EmployerSchemeHistStartYear='NULL' THEN NULL ELSE stpr.EmployerSchemeHistStartYear END) as Int) as EmployerSchemeHistStartYear
		 ,CAST((CASE WHEN stpr.EmployerSchemeHistEndYear='NULL' THEN NULL ELSE stpr.EmployerSchemeHistEndYear END) as Int) as EmployerSchemeHistEndYear
		 ,CAST((CASE WHEN stpr.SchemeType='NULL' THEN NULL ELSE stpr.SchemeType END) as Int) as EmployerSchemeHistType
		 ,CASE WHEN stpr.SchemeType='00' THEN 'No Longer Of Interest to TPR'
		       WHEN stpr.SchemeType='01' THEN 'P (PAYE)'
			   WHEN stpr.SchemeType='06' THEN 'PSC (PAYE SUB CONTRACTORS)'
			   WHEN stpr.SchemeType='02' THEN 'DOME (DOMESTIC)'
			   WHEN stpr.SchemeType='04' THEN 'NI (NATIONAL INSURANCE)'
			   WHEN stpr.SchemeType='15' THEN 'ELECT (ELECTORAL)'
			   WHEN stpr.SchemeType='12' THEN 'EXAM (EXAMINATION)'
			   WHEN stpr.SchemeType='18' THEN 'PSS (PROFIT SHARING)'
			   ELSE 'Unknown'
		   END as EmployerSchemeHistTypeDesc
		 ,stpr.SourceSK
         ,stpr.RunID
         ,stpr.SourceFileName
         ,getdate()
     FROM Tpr.Staging_Data stpr
	 LEFT
	 JOIN ShadowTpr.Organisation Org
	   ON stpr.SourceSK=Org.SourceSK
	  AND stpr.TPRUniqueID=Org.TPRUniqueId
    WHERE stpr.IsValid=1
	  --AND stpr.SourceSK > @SourceSK
   -- ORDER BY stpr.SourceSK

	
   -- IF @@ROWCOUNT > 0 
   --     SELECT @SourceSK = MAX(SOURCESK) 
   --       FROM shadow.OrganisationPAYEScheme
   -- ELSE
   --     SET @Loop = 0
   -- END
	
   /* Log Record Counts */
   INSERT INTO Mgmt.Log_Record_Counts
   (LogId,RunId,SourceTableName,TargetTableName,SourceRecordCount,TargetRecordCount)
   SELECT @LogID
         ,@Run_Id
		 ,'Staging_TPR'
	     ,'Organisation-shadow'
		 ,(SELECT COUNT(*) FROM Tpr.Staging_Data WHERE RunID=@Run_ID)
         ,(SELECT COUNT(*) FROM ShadowTpr.Organisation WHERE RunId=@Run_ID)	
		 
     /* Log Record Counts */
   INSERT INTO Mgmt.Log_Record_Counts
   (LogId,RunId,SourceTableName,TargetTableName,SourceRecordCount,TargetRecordCount)
   SELECT @LogID
         ,@Run_Id
		 ,'Staging_TPR'
	     ,'OrganisationAddress-shadow'
		 ,(SELECT COUNT(*) FROM Tpr.Staging_Data WHERE RunID=@Run_ID)
         ,(SELECT COUNT(*) FROM ShadowTpr.Organisation WHERE RunId=@Run_ID)		                                   
		 	                                   


/* Log Record Counts */

   INSERT INTO Mgmt.Log_Record_Counts
   (LogId,RunId,SourceTableName,TargetTableName,SourceRecordCount,TargetRecordCount)
   SELECT @LogID
         ,@Run_Id
         ,'Staging_TPR'
	     ,'OrganisationPAYEScheme-shadow'
	     ,(SELECT COUNT(*) FROM Tpr.Staging_Data WHERE RunID=@Run_ID)
         ,(SELECT COUNT(*) FROM ShadowTpr.OrganisationPAYEScheme WHERE RunId=@Run_ID)	
  
/* Recreate Indexes after finishing Load */

CREATE NONCLUSTERED  INDEX NCI_Organisation_AORN ON ShadowTpr.Organisation(AORN)

CREATE NONCLUSTERED  INDEX NCI_Organisation_PAYEScheme ON ShadowTpr.OrganisationPAYEScheme(PAYEScheme)

CREATE NONCLUSTERED INDEX NCI_Organisation_SK ON ShadowTpr.Organisation(TPRUniqueID,SourceSK)

CREATE NONCLUSTERED  INDEX NCI_Organisation_SK ON ShadowTpr.OrganisationAddress(OrgSK,TPRUniqueId,SourceSK)  

CREATE NONCLUSTERED  INDEX NCI_Organisation_SK ON ShadowTpr.OrganisationPAYEScheme(OrgSK,TPRUniqueId,SourceSK) 

--ALTER INDEX PK_Org_OrgSK ON Shadow.Organisation REBUILD

--ALTER INDEX PK_OrgAdd_OrgAddSK ON Shadow.OrganisationAddress REBUILD

--ALTER INDEX PK_OrgPAYEScheme_OrgSchemeSK ON Shadow.OrganisationPAYEScheme REBUILD   


/* Update Log Execution Results as Success if the query ran succesfully*/

UPDATE Mgmt.Log_Execution_Results
   SET Execution_Status=1
      ,EndDateTime=getdate()
	  ,FullJobStatus='Pending- Go To Step5 LoadTargetTables'
 WHERE LogId=@LogID
   AND RunID=@Run_Id

--COMMIT TRANSACTION 

END
ELSE
BEGIN
  DECLARE @RangeErrorId int

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
	    99999,
	    ERROR_STATE(),
	    9,
	    ERROR_LINE(),
	    'LoadTargetCloneTables' AS ErrorProcedure,
	    'Valid Staged Records are less than expected',
	    GETDATE(),
		@Run_Id as RunId; 

  SELECT @RangeErrorId=MAX(ErrorId) FROM Mgmt.Log_Error_Details

/* Update Log Execution Results as Fail if there is an Error*/

UPDATE Mgmt.Log_Execution_Results
   SET Execution_Status=0
      ,EndDateTime=getdate()
	  ,ErrorId=@RangeErrorId
 WHERE LogId=@LogID
   AND RunID=@Run_Id

END

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
	    'LoadTargetCloneTables' AS ErrorProcedure,
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