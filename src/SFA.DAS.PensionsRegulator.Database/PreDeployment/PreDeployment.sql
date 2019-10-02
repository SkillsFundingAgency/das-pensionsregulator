IF EXISTS (select * from INFORMATION_SCHEMA.ROUTINES
            where ROUTINE_NAME='uSP_LoadTPRFile'
              and ROUTINE_SCHEMA='dbo'
		  )
DROP PROCEDURE dbo.uSP_LoadTPRFile


IF EXISTS (select * from INFORMATION_SCHEMA.ROUTINES
            where ROUTINE_NAME='uSP_LoadTargetTables'
              and ROUTINE_SCHEMA='dbo'
		  )
DROP PROCEDURE dbo.uSP_LoadTargetTables







