CREATE PROCEDURE [dbo].GetOrganisationsByPAYEReference
	@PAYESchemeReference nvarchar(50)
AS
SELECT 
organisation.OranisationName as Name,
organisation.StartDate as RegistrationDate,
OrganisationAddress.AddressLine1 as AddressLine1,
OrganisationAddress.AddressLine2 as AddressLine2,
OrganisationAddress.AddressLine3 as AddressLine3,
OrganisationAddress.AddressLine4 as AddressLine4,
OrganisationAddress.AddressLine5 as AddresssLine5,
OrganisationAddress.PostCode as PostCode
FROM
OrganisationPAYEScheme join
Organisation on Organisation.Id = OrganisationPAYEScheme.OrganisationId join
OrganisationAddress on OrganisationAddress.OrganisationId = OrganisationPAYEScheme.OrganisationId 
WHERE OrganisationPAYEScheme.PAYESchemeRef = @PAYESchemeReference
