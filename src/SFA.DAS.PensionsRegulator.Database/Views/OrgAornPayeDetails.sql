/* Paye Aorn View for Registrations */

CREATE VIEW [dbo].[OrgAornPayeDetails]
AS
	SELECT 
          Org.OrganisationName              as OrganisationName
         ,OrgPAYE.SchemeEndDateCodeDesc     as OrganisationStatus
         ,OrgPAYE.PAYEScheme                as PayeRef
         ,Org.AOReference                   as AORN
         ,Org.TPRUniqueId                   as TPRUniqueKey
         ,OrgAdd.AddressLine1               as AddressLine1
         ,OrgAdd.AddressLine2               as AddressLine2
         ,OrgAdd.AddressLine3               as AddressLine3
         ,OrgAdd.AddressLine4               as AddressLine4
         ,OrgAdd.AddressLine5               as AddressLine5
         ,OrgAdd.PostCode                   as PostCode
     FROM
          Tpr.Organisation Org
	 JOIN Tpr.OrganisationPAYEScheme OrgPAYE
	   on OrgPAYE.OrgSK = Org.OrgSK 
     JOIN Tpr.OrganisationAddress OrgAdd
       on OrgAdd.OrgSK = Org.OrgSK 
GO


