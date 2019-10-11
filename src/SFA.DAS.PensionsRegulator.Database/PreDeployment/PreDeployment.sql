﻿
DECLARE @Date Date

SET @Date=getdate()

IF @Date < '2019-10-30'
BEGIN

IF  EXISTS (SELECT * FROM sys.indexes WHERE NAME = N'CI_TPR_UniqueKey' AND object_id = OBJECT_ID('dbo.Stg_Tpr_1218')) 
BEGIN
DROP INDEX [CI_TPR_UniqueKey] ON dbo.Stg_Tpr_1218
END


IF  EXISTS (SELECT * FROM sys.indexes WHERE NAME = N'NCI_TPR_AORN' AND object_id = OBJECT_ID('dbo.Stg_Tpr_1218')) 
BEGIN
DROP INDEX [NCI_TPR_AORN] ON dbo.Stg_Tpr_1218
END


IF  EXISTS (SELECT * FROM sys.indexes WHERE NAME = N'NCI_TPR_PAYE' AND object_id = OBJECT_ID('dbo.Stg_Tpr_1218')) 
BEGIN
DROP INDEX [NCI_TPR_PAYE] ON dbo.Stg_Tpr_1218
END



IF  EXISTS (SELECT * FROM sys.indexes WHERE NAME = N'NCI_TPR_PAYE_AORN' AND object_id = OBJECT_ID('dbo.Stg_Tpr_1218')) 
BEGIN
DROP INDEX [NCI_TPR_PAYE_AORN] ON dbo.Stg_Tpr_1218
END


IF  EXISTS (SELECT * FROM sys.indexes WHERE NAME = N'NCI_TPR_PAYE_DERIVED' AND object_id = OBJECT_ID('dbo.Stg_Tpr_1218')) 
BEGIN
DROP INDEX [NCI_TPR_PAYE_DERIVED] ON dbo.Stg_Tpr_1218
END

END



