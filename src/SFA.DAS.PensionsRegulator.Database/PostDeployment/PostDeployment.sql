/*
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
*/


/* Insert Metadata to Data Validation Rules */

TRUNCATE TABLE [Mgmt].[Data_Validation_Rules]

INSERT INTO [Mgmt].[Data_Validation_Rules]
           ([ColumnName]
           ,[ColumnNullable]
           ,[ColumnType]
           ,[ColumnLength]
           ,[ColumnPrecision]
           ,[ColumnDefault]
           ,[ColumnPattern]
           ,[ColumnMinValue]
           ,[ColumnMaxValue]
           ,[RunChecks]
           ,[RunTextLengthChecks]
           ,[RunNumericChecks]
           ,[RunPatternMatchChecks]
           ,[RunValueRangeChecks]
           ,[RunDecimalPlacesCheck]
           ,[RunDataChecks])
     VALUES
('TPRUniqueID',0,'BIGINT',0,0,'','',-9223372036854770000,9223372036854770000,1,0,1,0,0,0,0),
('DistrictNumber',0,'CHAR',3,0,'','',0,0,1,1,0,0,0,0,0),
('Reference',0,'CHAR',10,0,'','',0,0,1,1,0,0,0,0,0),
('AODistrict',0,'INT',3,0,'','',-2147483648,2147483647,1,0,1,0,0,0,0),
('AOTaxType',0,'CHAR',1,0,'','',0,0,1,1,0,0,0,0,0),
('AOCheckChar',0,'CHAR',1,0,'','',0,0,1,1,0,0,0,0,0),
('AOReference',0,'CHAR',8,0,'','',0,0,1,1,0,0,0,0,0),
('Name1',1,'NVARCHAR',256,0,'','',0,0,1,1,0,0,0,0,0),
('Name2',1,'NVARCHAR',256,0,'','',0,0,1,1,0,0,0,0,0),
('CompanyRegNo',1,'VARCHAR',256,0,'','',0,0,1,1,0,0,0,0,0),
('TradeClass',1,'VARCHAR',256,0,'','',0,0,1,1,0,0,0,0,0),
('AOInd',1,'CHAR',1,0,'','',0,0,1,1,0,0,0,0,0),
('XferType',1,'TINYINT',1,0,'','',0,255,1,1,0,0,0,0,0),
('UniqueReference',1,'CHAR',10,0,'','',0,0,1,1,0,0,0,0,0),
('UniqueRefDistrict',1,'CHAR',3,0,'','',0,0,1,1,0,0,0,0,0),
('AddressCode',1,'CHAR',1,0,'','',0,0,0,1,0,0,0,0,0),
('AddressFromDate',1,'DATE',0,0,'','',0,0,0,0,0,0,0,0,1),
('AddressToDate',1,'DATE',0,0,'','',0,0,0,0,0,0,0,0,1),
('AddressLine1',1,'NVARCHAR',100,0,'','',0,0,1,1,0,0,0,0,0),
('AddressLine2',1,'NVARCHAR',100,0,'','',0,0,1,1,0,0,0,0,0),
('AddressLine3',1,'NVARCHAR',100,0,'','',0,0,1,1,0,0,0,0,0),
('AddressLine4',1,'NVARCHAR',100,0,'','',0,0,1,1,0,0,0,0,0),
('AddressLine5',1,'NVARCHAR',100,0,'','',0,0,1,1,0,0,0,0,0),
('TelephoneNumber',1,'NVARCHAR',25,0,'','',0,0,1,1,0,0,0,0,0),
('PostCode',1,'VARCHAR',10,0,'','',0,0,1,1,0,0,0,0,0),
('EmailAddress',1,'NVARCHAR',256,0,'','',0,0,1,1,0,0,0,0,0),
('AddressType',1,'CHAR',1,0,'','',0,0,1,1,0,0,0,0,0),
('ForeignCountry',1,'VARCHAR',100,0,'','',0,0,1,1,0,0,0,0,0),
('AddressVerifiedIndicator',1,'CHAR',1,0,'','',0,0,1,1,0,0,0,0,0),
('ADI',1,'VARCHAR',100,0,'','',0,0,1,1,0,0,0,0,0),
('StartDate',1,'Date',0,0,'','',0,0,1,0,0,0,0,0,1),
('EndDate',1,'Date',0,0,'','',0,0,1,0,0,0,0,0,1),
('EndDateCode',1,'TinyInt',0,0,'','',0,255,1,0,1,0,0,0,0),
('RestartDate',1,'Date',0,0,'','',0,0,1,0,0,0,0,0,1),
('ReviewYr',1,'INT',0,0,'','',-2147483648,2147483647,1,0,1,0,0,0,0),
('NumberCounted',1,'Int',0,0,'','',-2147483648,2147483647,1,0,1,0,0,0,0),
('DateTaken',1,'Date',0,0,'','',0,0,1,0,0,0,0,0,1),
('EEC_Code',1,'CHAR',1,0,'','',0,0,1,1,0,0,0,0,0),
('SignalCode',1,'VARCHAR',10,0,'','',0,0,1,1,0,0,0,0,0),
('SignalChanged',1,'CHAR',1,0,'','',0,0,1,1,0,0,0,0,0),
('SignalStartYear',1,'INT',0,0,'','',-2147483648,2147483647,1,0,1,0,0,0,0),
('SignalEndYear',1,'INT',0,0,'','',-2147483648,2147483647,1,0,1,0,0,0,0),
('SignalValue',1,'CHAR',1,0,'','',0,0,1,1,0,0,0,0,0),
('EmployerSchemeHistStartYear',1,'Int',0,0,'','',-2147483648,2147483647,1,0,1,0,0,0,0),
('EmployerSchemeHistEndYear',1,'Int',0,0,'','',-2147483648,2147483647,1,0,1,0,0,0,0),
('SchemeType',1,'Int',0,0,'','',-2147483648,2147483647,1,0,1,0,0,0,0)
