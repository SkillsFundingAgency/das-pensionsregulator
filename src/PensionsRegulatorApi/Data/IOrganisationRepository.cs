using PensionsRegulatorApi.Domain;

namespace PensionsRegulatorApi.Data;

public interface IOrganisationRepository
{
    Task<Organisation> GetOrganisationById(long tprUniqueKey);
    Task<IEnumerable<Organisation>> GetOrganisationsForPAYEReference(string payeReference);
    Task<IEnumerable<Organisation>> GetOrganisationsForPAYEReferenceAndAORN(string payeReference, string aorn);
}