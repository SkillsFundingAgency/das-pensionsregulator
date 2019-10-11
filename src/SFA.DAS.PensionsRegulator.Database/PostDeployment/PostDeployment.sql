﻿
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE NAME = N'NCI_TPR_AORN' AND object_id = OBJECT_ID('dbo.Stg_Tpr_1218')) 
BEGIN
CREATE NONCLUSTERED INDEX [NCI_TPR_AORN] ON [dbo].[Stg_Tpr_1218]([AORN])
END



IF NOT EXISTS (SELECT * FROM sys.indexes WHERE NAME = N'NCI_TPR_PAYE' AND object_id = OBJECT_ID('dbo.Stg_Tpr_1218')) 
BEGIN
CREATE NONCLUSTERED INDEX [NCI_TPR_PAYE] ON [dbo].[Stg_Tpr_1218]([PAYESchemeRef])
END


IF NOT  EXISTS (SELECT * FROM sys.indexes WHERE NAME = N'NCI_TPR_PAYE_AORN' AND object_id = OBJECT_ID('dbo.Stg_Tpr_1218')) 
BEGIN
CREATE NONCLUSTERED INDEX [NCI_TPR_PAYE_AORN] ON [dbo].[Stg_Tpr_1218]([PAYESchemeRef],[AORN])
END



IF NOT  EXISTS (SELECT * FROM sys.indexes WHERE NAME = N'NCI_TPR_PAYE_DERIVED' AND object_id = OBJECT_ID('dbo.Stg_Tpr_1218')) 
BEGIN
CREATE NONCLUSTERED INDEX [NCI_TPR_PAYE_DERIVED] ON [dbo].[Stg_Tpr_1218](PayeDerived)
END




