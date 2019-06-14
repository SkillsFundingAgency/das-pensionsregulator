using System;
using System.Collections.Generic;
using MediatR;
using PensionsRegulatorApi.Data;
using PensionsRegulatorApi.Domain;

namespace PensionsRegulatorApi.Application.Queries
{
    public class GetOrganisationsByPayeRefAndAornHandler : RequestHandler<GetOrganisationsByPayeRefAndAorn, IEnumerable<Organisation>>
    {
        private readonly IOrganisationRepository _repository;

        public GetOrganisationsByPayeRefAndAornHandler(IOrganisationRepository repository)
        {
            _repository = repository;
        }

        protected override IEnumerable<Organisation> Handle(GetOrganisationsByPayeRefAndAorn request)
        {
            return
                _repository
                    .GetOrganisationsForPAYEReferenceAndAORN(
                        request.PAYEReference,
                        request.AccountOfficeReferenceNumber);
        }
    }
}