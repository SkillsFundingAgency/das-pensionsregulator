using System;
using System.Collections.Generic;
using MediatR;
using PensionsRegulatorApi.Data;
using PensionsRegulatorApi.Domain;

namespace PensionsRegulatorApi.Application.Queries
{
    

    public class GetOrganisationsByPayeRefHandler : RequestHandler<GetOrganisationsByPayeRef, IEnumerable<Organisation>>
    {
        private readonly IOrganisationRepository _repository;

        public GetOrganisationsByPayeRefHandler(IOrganisationRepository repository)
        {
            _repository = repository;
        }

        protected override IEnumerable<Organisation> Handle(GetOrganisationsByPayeRef request)
        {
            return
                _repository
                    .GetOrganisationsForPAYEReference(
                        request.PAYEReference);
        }
    }
}