CREATE PROCEDURE [dbo].uSP_GetOrganisationsByPAYEReference
	@PAYESchemeReference varchar(25)
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
vw_OrgAornPayeDetails
WHERE PAYERef = UPPER(@PAYESchemeReference)
