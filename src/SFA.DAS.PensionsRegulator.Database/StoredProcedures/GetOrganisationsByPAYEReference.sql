CREATE PROCEDURE [dbo].[GetOrganisationsByPAYEReference]
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
[dbo].[OrgAornPayeDetails]
WHERE PAYERef = @PAYESchemeReference
GO