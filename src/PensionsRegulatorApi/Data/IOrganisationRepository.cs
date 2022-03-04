using System.Collections.Generic;
using PensionsRegulatorApi.Domain;

namespace PensionsRegulatorApi.Data
{
    public interface IOrganisationRepository
    {
        Organisation GetOrganisationById(long tprUniqueKey);
        IEnumerable<Organisation> GetOrganisationsForPAYEReference(string payeReference);
        IEnumerable<Organisation> GetOrganisationsForPAYEReferenceAndAORN(string payeReference, string aorn);
    }
}