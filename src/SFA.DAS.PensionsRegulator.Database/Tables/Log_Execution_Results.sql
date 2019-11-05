
CREATE TABLE [Mgmt].[Log_Execution_Results](
	[LogId] [bigint] IDENTITY(1,1) NOT NULL,
	[RunId] [bigint] NOT NULL,
	[ErrorId] [bigint] NULL,
	[StepNo] [varchar](100) NULL,
	[StoredProcedureName] [varchar](100) NOT NULL,
	[Execution_Status] [bit] NOT NULL,
	[Execution_Status_Desc]  AS (case when [Execution_Status]=(1) then 'Success' else 'Fail' end),
	[StartDateTime] [datetime2](7) NULL,
	[EndDateTime] [datetime2](7) NULL,
	[FullJobStatus] [varchar](256) NULL,
 CONSTRAINT [PK_LER_LogID] PRIMARY KEY CLUSTERED (	[LogId] ASC),
 CONSTRAINT [FK_LER_ErrorID] FOREIGN KEY (ErrorId) REFERENCES [Mgmt].[Log_Error_Details] ([ErrorId]),
 CONSTRAINT [FK_LER_RunID] FOREIGN KEY(RunId) REFERENCES [Mgmt].[Log_RunId] ([Run_Id])
) 






