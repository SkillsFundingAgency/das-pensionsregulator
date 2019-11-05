CREATE VIEW [dbo].[OrganisationsWithAddressesAndReferences]
AS SELECT
Organisation.OrganisationName as OrganisationName,
OrganisationPAYEScheme.SchemeEndDateCodeDesc as OrganisationStatus,
OrganisationPAYEScheme.PAYEScheme as PayeRef,
Organisation.AOReference as AORN,
Organisation.TPRUniqueId as TPRUniqueKey,
OrganisationAddress.AddressLine1 as AddressLine1,
OrganisationAddress.AddressLine2 as AddressLine2,
OrganisationAddress.AddressLine3 as AddressLine3,
OrganisationAddress.AddressLine4 as AddressLine4,
OrganisationAddress.AddressLine5 as AddressLine5,
OrganisationAddress.PostCode as PostCode
FROM
Organisation JOIN
OrganisationPAYEScheme on OrganisationPAYEScheme.OrgSK = Organisation.OrgSK LEFT JOIN
OrganisationAddress on OrganisationAddress.OrgSK = OrganisationPAYEScheme.OrgSK