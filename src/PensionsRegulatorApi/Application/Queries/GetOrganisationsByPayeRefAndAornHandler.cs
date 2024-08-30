using PensionsRegulatorApi.Data;
using PensionsRegulatorApi.Domain;

namespace PensionsRegulatorApi.Application.Queries;

public class GetOrganisationsByPayeRefAndAornHandler(IOrganisationRepository repository)
    : IRequestHandler<GetOrganisationsByPayeRefAndAorn, IEnumerable<Organisation>>
{
    public async Task<IEnumerable<Organisation>> Handle(GetOrganisationsByPayeRefAndAorn request, CancellationToken cancellationToken)
    {
        return await repository.GetOrganisationsForPAYEReferenceAndAORN(request.PAYEReference, request.AccountOfficeReferenceNumber);
    }
}