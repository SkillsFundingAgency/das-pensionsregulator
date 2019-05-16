using System.Collections.Generic;
using System.Linq;
using MediatR;
using PensionsRegulatorApi.Domain;

namespace PensionsRegulatorApi.Application.Queries
{
    public class GetOrganisationsHandler : RequestHandler<GetOrganisations, IEnumerable<Organisation>>
    {
        protected override IEnumerable<Organisation> Handle(GetOrganisations request)
        {
            return Enumerable.Empty<Organisation>();
        }
    }
}