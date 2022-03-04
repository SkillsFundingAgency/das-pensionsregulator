CREATE PROCEDURE [dbo].[GetOrganisationByTPRUniqueKey]
	@TPRUniqueKey BIGINT
AS
SELECT 
OrganisationName,
OrganisationStatus,
TPRUniqueKey,
AddressLine1,
AddressLine2,
AddressLine3,
AddressLine4,
AddressLine5,
PostCode
FROM
[dbo].[OrgAornPayeDetails]
WHERE TPRUniqueKey = @TPRUniqueKey
GO