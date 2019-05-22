using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Domain;

namespace PensionsRegulatorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PensionsRegulatorController : ControllerBase
    {
        private IRequestHandler<GetOrganisations, IEnumerable<Organisation>> _getOrganisationsHandler;

        public PensionsRegulatorController(IRequestHandler<GetOrganisations, IEnumerable<Organisation>> getOrganisationsHandler)
        {
            _getOrganisationsHandler = getOrganisationsHandler;
        }

        [HttpGet("{payeRef}", Name = "Get")]
        public async Task<IEnumerable<Organisation>> Get(string payeRef)
        {
            return await _getOrganisationsHandler.Handle(new GetOrganisations(payeRef), CancellationToken.None);
        }
    }
}
