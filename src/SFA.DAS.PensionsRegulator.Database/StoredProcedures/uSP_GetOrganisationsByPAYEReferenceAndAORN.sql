﻿CREATE PROCEDURE [dbo].uSP_GetOrganisationsByPAYEReferenceAndAORN
	@PAYESchemeReference varchar(25),
	@AORN varchar(25)
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
AND       AORN = UPPER(@AORN)
