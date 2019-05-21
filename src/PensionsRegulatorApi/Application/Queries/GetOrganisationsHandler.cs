using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using PensionsRegulatorApi.Data;
using PensionsRegulatorApi.Domain;

namespace PensionsRegulatorApi.Application.Queries
{
    

    public class GetOrganisationsHandler : RequestHandler<GetOrganisations, IEnumerable<Organisation>>
    {
        private readonly IOrganisationRepository _repository;

        public GetOrganisationsHandler(IOrganisationRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        protected override IEnumerable<Organisation> Handle(GetOrganisations request)
        {
            return
                _repository
                    .GetOrganisationsForPAYEReference(
                        request.PAYEReference);
        }
    }
}