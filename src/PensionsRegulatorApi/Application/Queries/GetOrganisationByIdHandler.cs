using MediatR;
using PensionsRegulatorApi.Data;
using PensionsRegulatorApi.Domain;

namespace PensionsRegulatorApi.Application.Queries
{
    

    public class GetOrganisationByIdHandler : RequestHandler<GetOrganisationById, Organisation>
    {
        private readonly IOrganisationRepository _repository;

        public GetOrganisationByIdHandler(IOrganisationRepository repository)
        {
            _repository = repository;
        }

        protected override Organisation Handle(GetOrganisationById request)
        {
            return
                _repository
                    .GetOrganisationById(
                        request.TPRUniqueKey);
        }
    }
}