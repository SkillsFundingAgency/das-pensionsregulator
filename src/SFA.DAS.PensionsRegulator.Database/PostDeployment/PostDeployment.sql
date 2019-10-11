/*
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE NAME = N'CI_TPR_UniqueKey' AND object_id = OBJECT_ID('Stg_Tpr_1218')) 
CREATE CLUSTERED INDEX [CI_TPR_UniqueKey] ON [dbo].[Stg_Tpr_1218]([UniqueID])



IF NOT EXISTS (SELECT * FROM sys.indexes WHERE NAME = N'NCI_TPR_AORN' AND object_id = OBJECT_ID('Stg_Tpr_1218')) 
CREATE NONCLUSTERED INDEX [NCI_TPR_AORN] ON [dbo].[Stg_Tpr_1218]([AORN])



IF NOT EXISTS (SELECT * FROM sys.indexes WHERE NAME = N'NCI_TPR_PAYE' AND object_id = OBJECT_ID('Stg_Tpr_1218')) 
CREATE NONCLUSTERED INDEX [NCI_TPR_PAYE] ON [dbo].[Stg_Tpr_1218]([PAYESchemeRef])


IF NOT  EXISTS (SELECT * FROM sys.indexes WHERE NAME = N'NCI_TPR_PAYE_AORN' AND object_id = OBJECT_ID('Stg_Tpr_1218')) 
CREATE NONCLUSTERED INDEX [NCI_TPR_PAYE_AORN] ON [dbo].[Stg_Tpr_1218]([PAYESchemeRef],[AORN])



IF NOT  EXISTS (SELECT * FROM sys.indexes WHERE NAME = N'NCI_TPR_PAYE_DERIVED' AND object_id = OBJECT_ID('Stg_Tpr_1218')) 
CREATE NONCLUSTERED INDEX [NCI_TPR_PAYE_DERIVED] ON [dbo].[Stg_Tpr_1218](PayeDerived)

*/

/* Copy Data from Stg_Tpr_1218 to Stg_Tpr_1218_2 */

IF ((SELECT COUNT(*) FROM Stg_Tpr_1218_2 )=0)
BEGIN
INSERT INTO Stg_Tpr_1218_2
([Record Type1]
           ,[UniqueID]
           ,[DistrictNumber]
           ,[Reference]
           ,[XferFromDist]
           ,[XferFromRef]
           ,[XferToDstrct]
           ,[XferToRef]
           ,[AODistrict]
           ,[AOTaxType]
           ,[AOCheckChar]
           ,[AOReference]
           ,[StartDate]
           ,[EndDate]
           ,[EndDateCode]
           ,[RestartDate]
           ,[LbltyFirstYr]
           ,[LbltyLastYr]
           ,[ReviewYr]
           ,[CompanyRegNo]
           ,[TradeClass]
           ,[CntrctOutRef]
           ,[ToEmpInstance]
           ,[ToEmpFirstYear]
           ,[AOInd]
           ,[XferType]
           ,[UniqueRefDistrict]
           ,[UniqueReference]
           ,[RecordType2]
           ,[Name1]
           ,[Name2]
           ,[TelephoneNmbr]
           ,[FaxNumber]
           ,[EmailAddress]
           ,[AddressType]
           ,[ADI]
           ,[AddressLine1]
           ,[AddressLine2]
           ,[AddressLine3]
           ,[AddressLine4]
           ,[AddressLine5]
           ,[PostCode]
           ,[RLSInd]
           ,[ForeignCountry]
           ,[AddressVerifiedIndicator]
           ,[RecordType3]
           ,[SignalUniqueID]
           ,[Code]
           ,[SignalChgd]
           ,[SigStartYear]
           ,[SigEndYear]
           ,[Value]
           ,[RecordType4]
           ,[EmployerSchemeHistoryUniqueID]
           ,[StartYear]
           ,[EndYear]
           ,[SchemeType]
           ,[RecordType5]
           ,[EmployerCountID]
           ,[EEC_Code]
           ,[DateTaken]
           ,[NumberCounted]
           ,[e-filedExemptionIndicator]
           ,[RecordType6]
           ,[UniqueID-Employment]
           ,[UniqueID-Employer]
           ,[LatestAnnualisedGrossEarningForPAYESscheme(fromP14)]
           ,[CurrencyIndicator]
           ,[TaxYear]
           ,[CountEmploymentsP14]
           ,[PAYESchemeRef]
           ,[AORN]
		   )
SELECT [Record Type1]
           ,[UniqueID]
           ,[DistrictNumber]
           ,[Reference]
           ,[XferFromDist]
           ,[XferFromRef]
           ,[XferToDstrct]
           ,[XferToRef]
           ,[AODistrict]
           ,[AOTaxType]
           ,[AOCheckChar]
           ,[AOReference]
           ,[StartDate]
           ,[EndDate]
           ,[EndDateCode]
           ,[RestartDate]
           ,[LbltyFirstYr]
           ,[LbltyLastYr]
           ,[ReviewYr]
           ,[CompanyRegNo]
           ,[TradeClass]
           ,[CntrctOutRef]
           ,[ToEmpInstance]
           ,[ToEmpFirstYear]
           ,[AOInd]
           ,[XferType]
           ,[UniqueRefDistrict]
           ,[UniqueReference]
           ,[RecordType2]
           ,[Name1]
           ,[Name2]
           ,[TelephoneNmbr]
           ,[FaxNumber]
           ,[EmailAddress]
           ,[AddressType]
           ,[ADI]
           ,[AddressLine1]
           ,[AddressLine2]
           ,[AddressLine3]
           ,[AddressLine4]
           ,[AddressLine5]
           ,[PostCode]
           ,[RLSInd]
           ,[ForeignCountry]
           ,[AddressVerifiedIndicator]
           ,[RecordType3]
           ,[SignalUniqueID]
           ,[Code]
           ,[SignalChgd]
           ,[SigStartYear]
           ,[SigEndYear]
           ,[Value]
           ,[RecordType4]
           ,[EmployerSchemeHistoryUniqueID]
           ,[StartYear]
           ,[EndYear]
           ,[SchemeType]
           ,[RecordType5]
           ,[EmployerCountID]
           ,[EEC_Code]
           ,[DateTaken]
           ,[NumberCounted]
           ,[e-filedExemptionIndicator]
           ,[RecordType6]
           ,[UniqueID-Employment]
           ,[UniqueID-Employer]
           ,[LatestAnnualisedGrossEarningForPAYESscheme(fromP14)]
           ,[CurrencyIndicator]
           ,[TaxYear]
           ,[CountEmploymentsP14]
           ,[PAYESchemeRef]
           ,[AORN]
from [dbo].[Stg_Tpr_1218]
END


IF NOT EXISTS (SELECT * FROM sys.indexes WHERE NAME = N'NCI_TPR_AORN' AND object_id = OBJECT_ID('Stg_Tpr_1218_2')) 
CREATE NONCLUSTERED INDEX [NCI_TPR_AORN] ON [dbo].[Stg_Tpr_1218_2]([AORN])



IF NOT EXISTS (SELECT * FROM sys.indexes WHERE NAME = N'NCI_TPR_PAYE' AND object_id = OBJECT_ID('Stg_Tpr_1218_2')) 
CREATE NONCLUSTERED INDEX [NCI_TPR_PAYE] ON [dbo].[Stg_Tpr_1218_2]([PAYESchemeRef])


IF NOT  EXISTS (SELECT * FROM sys.indexes WHERE NAME = N'NCI_TPR_PAYE_AORN' AND object_id = OBJECT_ID('Stg_Tpr_1218_2')) 
CREATE NONCLUSTERED INDEX [NCI_TPR_PAYE_AORN] ON [dbo].[Stg_Tpr_1218_2]([PAYESchemeRef],[AORN])



IF NOT  EXISTS (SELECT * FROM sys.indexes WHERE NAME = N'NCI_TPR_PAYE_DERIVED' AND object_id = OBJECT_ID('Stg_Tpr_1218_2')) 
CREATE NONCLUSTERED INDEX [NCI_TPR_PAYE_DERIVED] ON [dbo].[Stg_Tpr_1218_2](PayeDerived)
