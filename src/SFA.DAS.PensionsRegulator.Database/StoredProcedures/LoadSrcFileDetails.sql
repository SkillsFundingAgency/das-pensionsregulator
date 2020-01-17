CREATE PROCEDURE [dbo].[LoadSrcFileDetails]
(@SourceFileName varchar(255))
AS
-- ===============================================================================================================
-- Author:      Himabindu Uddaraju
-- Create Date: 04/11/2019
-- Description: Inserts FileName of the Latest File on Blob Storage into SourceFileList Table ready to be loaded.
-- ===============================================================================================================
BEGIN TRY

/* Insert File Name  */


INSERT INTO Mgmt.SourceFileList
(FileName)
VALUES (''+@SourceFileName+'')



END TRY

BEGIN CATCH

  SELECT 
        SUSER_SNAME(),
	    ERROR_NUMBER(),
	    ERROR_STATE(),
	    ERROR_SEVERITY(),
	    ERROR_LINE(),
	    ERROR_MESSAGE(),
	    GETDATE()
		;


/* Update Log Execution Results as Fail if there is an Error*/

END CATCH

GO


