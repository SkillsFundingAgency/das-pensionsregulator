using System;
using System.Collections.Generic;
using MediatR;
using PensionsRegulatorApi.Data;
using PensionsRegulatorApi.Domain;

namespace PensionsRegulatorApi.Application.Queries
{
    public class GetValidatedOrganisationsHandler : RequestHandler<GetValidatedOrganisations, IEnumerable<Organisation>>
    {
        private readonly IOrganisationRepository _repository;

        public GetValidatedOrganisationsHandler(IOrganisationRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        protected override IEnumerable<Organisation> Handle(GetValidatedOrganisations request)
        {
            return
                _repository
                    .GetOrganisationsForPAYEReferenceAndAORN(
                        request.PAYEReference,
                        request.AccountOfficeReferenceNumber);
        }
    }
}