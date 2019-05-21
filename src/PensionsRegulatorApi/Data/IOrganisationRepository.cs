using System.Collections.Generic;
using PensionsRegulatorApi.Domain;

namespace PensionsRegulatorApi.Data
{
    public interface IOrganisationRepository
    {
        IEnumerable<Organisation> GetOrganisationsForPAYEReference(string payeReference);
    }
}