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
        private readonly OrganisationRepository _repository;

        public GetOrganisationsHandler(OrganisationRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        
        protected override IEnumerable<Organisation> Handle(GetOrganisations request)
        {
            try
            {
                return
                    _repository
                        .GetOrganisationsForPAYEReference(
                            request.PAYEReference);
            }
            catch
            {
                return 
                    Enumerable
                        .Empty<Organisation>();
            }
        }
    }
}