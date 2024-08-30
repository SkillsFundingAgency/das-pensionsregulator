using PensionsRegulatorApi.Data;
using PensionsRegulatorApi.Domain;

namespace PensionsRegulatorApi.Application.Queries;

public class GetOrganisationByIdHandler(IOrganisationRepository repository) : IRequestHandler<GetOrganisationById, Organisation>
{
    public async Task<Organisation> Handle(GetOrganisationById request, CancellationToken cancellationToken)
    {
        return await repository.GetOrganisationById(request.TPRUniqueKey);
    }
}