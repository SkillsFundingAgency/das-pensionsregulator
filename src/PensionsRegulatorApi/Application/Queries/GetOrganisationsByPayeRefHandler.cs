using PensionsRegulatorApi.Data;
using PensionsRegulatorApi.Domain;

namespace PensionsRegulatorApi.Application.Queries;

public class GetOrganisationsByPayeRefHandler(IOrganisationRepository repository)
    : IRequestHandler<GetOrganisationsByPayeRef, IEnumerable<Organisation>>
{
    public async Task<IEnumerable<Organisation>> Handle(GetOrganisationsByPayeRef request, CancellationToken cancellationToken)
    {
        return await repository.GetOrganisationsForPAYEReference(request.PAYEReference);
        
    }
}