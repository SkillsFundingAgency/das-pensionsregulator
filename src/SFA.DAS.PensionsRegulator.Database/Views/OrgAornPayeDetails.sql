/* Paye Aorn View for Registrations */

CREATE VIEW [dbo].[OrgAornPayeDetails]
AS
SELECT
 LTRIM(RTRIM(Name1))+' '+LTRIM(RTRIM(Name2)) as OrganisationName
,CASE WHEN EndDateCode=0 THEN 'Not Closed' 
      WHEN EndDateCode=1 THEN 'Transfer Out' 
	  WHEN EndDateCode=2 THEN 'Merger Out'
	  WHEN EndDateCode=3 THEN 'Succession Out'
	  WHEN EndDateCode=4 THEN 'Ceased'
	  WHEN EndDateCode=5 THEN 'Cancelled'
	  ELSE 'Unknown'
  END as OrganisationStatus
, PAYESchemeRef as PayeRef
, AORN as AORN
, UniqueID as TPRUniqueKey
, AddressLine1 as AddressLine1
, AddressLine2 as AddressLine2
, AddressLine3 as AddressLine3
, AddressLine4 as AddressLine4
, AddressLine5 as AddressLine5
, PostCode as PostCode
FROM [dbo].Stg_Tpr_1218
GO


