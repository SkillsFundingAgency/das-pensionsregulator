﻿CREATE PROCEDURE [dbo].GetOrganisationsByPAYEReference
	@PAYESchemeReference nvarchar(50)
AS
SELECT 
Organisation.OrganisationName as Name,
Organisation.Status as Status,
Organisation.TPR_Unique_Id as UniqueIdentity,
OrganisationAddress.AddressLine1 as AddressLine1,
OrganisationAddress.AddressLine2 as AddressLine2,
OrganisationAddress.AddressLine3 as AddressLine3,
OrganisationAddress.AddressLine4 as AddressLine4,
OrganisationAddress.AddressLine5 as AddressLine5,
OrganisationAddress.PostCode as PostCode
FROM
OrganisationPAYEScheme JOIN
Organisation on Organisation.Employer_SK = OrganisationPAYEScheme.Employer_SK JOIN
OrganisationAddress on OrganisationAddress.Employer_SK = OrganisationPAYEScheme.Employer_SK 
WHERE OrganisationPAYEScheme.PAYESchemeRef = @PAYESchemeReference
