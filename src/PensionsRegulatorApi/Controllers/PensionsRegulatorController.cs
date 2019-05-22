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
        private readonly IRequestHandler<GetOrganisations, IEnumerable<Organisation>> _getOrganisationsHandler;

        public PensionsRegulatorController(IRequestHandler<GetOrganisations, IEnumerable<Organisation>> getOrganisationsHandler)
        {
            _getOrganisationsHandler = getOrganisationsHandler;
        }

        /// <summary>
        /// Gets the organisations from the pensions regulator for a given PAYE reference
        /// </summary>
        /// <param name="payeRef">The PAYE reference from which to get matching organisations from the pensions regulator</param>
        /// <returns>The organisations for the given PAYE reference from the pensions regulator</returns>
        /// <response code="200">Health check successful</response>
        /// <response code="401">The client is not authorized to access this endpoint</response>
        /// <response code="500">Internal server error</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [HttpGet("{payeRef}", Name = "Get")]
        public async Task<IEnumerable<Organisation>> Get(string payeRef)
        {
            return await _getOrganisationsHandler.Handle(new GetOrganisations(payeRef), CancellationToken.None);
        }
    }
}
