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
  END                                        as OrganisationStatus
, PayeDerived                                as PayeRef
, AORN                                       as AORN
, UniqueID                                   as TPRUniqueKey
, LTRIM(RTRIM(AddressLine1))                 as AddressLine1
, LTRIM(RTRIM(AddressLine2))                 as AddressLine2
, LTRIM(RTRIM(AddressLine3))                 as AddressLine3
, LTRIM(RTRIM(AddressLine4))                 as AddressLine4
, LTRIM(RTRIM(AddressLine5))                 as AddressLine5
, LTRIM(RTRIM(PostCode))                     as PostCode
FROM [dbo].Stg_Tpr_1218
GO


