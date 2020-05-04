CREATE TABLE [Mgmt].[SourceFileList](
	[SourceFileID] [bigint] IDENTITY(1,1) NOT NULL,
	[FileName] [varchar](max) NOT NULL,
	[FileUploadedDateTime] [datetime] DEFAULT(GETDATE()) NOT NULL,
	[LoadedToStaging] [bit] NULL,
	[StagingLoadDate] [datetime2](7) NULL,
	[FileProcessed] [bit] NULL,
	[FileProcessedDate] [datetime2](7) NULL,
	[FileLoadedToHistory] [bit] NULL,
	[FileRemovedFromHistory] [bit] DEFAULT(0) NULL,
	[RunId] [bigint] NULL,
 CONSTRAINT [PK_SFL_SourceFileId] PRIMARY KEY CLUSTERED ([SourceFileID] ASC)
 )

