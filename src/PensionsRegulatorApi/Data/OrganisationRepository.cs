using System.Collections.Generic;
using PensionsRegulatorApi.Domain;

namespace PensionsRegulatorApi.Data
{
    public interface OrganisationRepository
    {
        IEnumerable<Organisation> GetOrganisationsForPAYEReference(string payeReference);
    }
}