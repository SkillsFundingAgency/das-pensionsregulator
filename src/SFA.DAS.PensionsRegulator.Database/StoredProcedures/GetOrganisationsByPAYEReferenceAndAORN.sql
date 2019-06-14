CREATE PROCEDURE [dbo].GetOrganisationsByPAYEReferenceAndAORN
	@PAYESchemeReference varchar(25),
	@AORN varchar(25)
AS
SELECT 
Name,
Status,
UniqueIdentity,
AddressLine1,
AddressLine2,
AddressLine3,
AddressLine4,
AddressLine5,
PostCode
FROM
OrganisationsWithAddressesAndReferences
WHERE PAYERef = @PAYESchemeReference
AND       AORN = @AORN
