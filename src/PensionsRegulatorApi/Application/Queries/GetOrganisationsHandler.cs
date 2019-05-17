using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MediatR;
using PensionsRegulatorApi.Data;
using PensionsRegulatorApi.Domain;

namespace PensionsRegulatorApi.Application.Queries
{
    

    public class GetOrganisationsHandler : RequestHandler<GetOrganisations, IEnumerable<Organisation>>
    {
        public GetOrganisationsHandler(OrganisationRepository repository)
        {

        }
        
        protected override IEnumerable<Organisation> Handle(GetOrganisations request)
        {
            return Enumerable.Empty<Organisation>();
        }
    }
}