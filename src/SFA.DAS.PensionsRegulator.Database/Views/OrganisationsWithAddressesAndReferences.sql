CREATE VIEW [dbo].[OrganisationsWithAddressesAndReferences]
AS SELECT
Organisation.OrganisationName as OrganisationName,
Organisation.Status as OrganisationStatus,
OrganisationPAYEScheme.PAYESchemeRef as PayeRef,
Organisation.AOReference as AORN,
Organisation.TPR_Unique_Id as TPRUniqueKey,
OrganisationAddress.AddressLine1 as AddressLine1,
OrganisationAddress.AddressLine2 as AddressLine2,
OrganisationAddress.AddressLine3 as AddressLine3,
OrganisationAddress.AddressLine4 as AddressLine4,
OrganisationAddress.AddressLine5 as AddressLine5,
OrganisationAddress.PostCode as PostCode
FROM
Organisation JOIN
OrganisationPAYEScheme on OrganisationPAYEScheme.Employer_SK = Organisation.Employer_SK LEFT JOIN
OrganisationAddress on OrganisationAddress.Employer_SK = OrganisationPAYEScheme.Employer_SK 