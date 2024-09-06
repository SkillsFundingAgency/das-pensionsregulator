using PensionsRegulatorApi.Domain;

namespace PensionsRegulatorApi.Application.Queries;

public class GetOrganisationById : IRequest<Organisation>
{
    public long TPRUniqueKey { get;  }
    
    public GetOrganisationById(long? id)
    {
        if (!id.HasValue)
        {
            throw new ArgumentException("Value cannot be null.", nameof(id));
        }
        
        TPRUniqueKey = id.Value;
    }
}