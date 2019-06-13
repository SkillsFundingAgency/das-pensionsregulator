using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Domain;

namespace PensionsRegulatorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PensionsRegulatorController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PensionsRegulatorController> _logger;

        public PensionsRegulatorController(IMediator mediator, ILogger<PensionsRegulatorController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Gets the organisations from the pensions regulator for a given PAYE reference
        /// </summary>
        /// <param name="payeRef">The PAYE reference from which to get matching organisations from the pensions regulator
        /// This needs to be a query parameter due to decoding of slash character
        /// <see cref="https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-2.2"/> </param>
        /// <returns>The organisations for the given PAYE reference from the pensions regulator</returns>
        /// <response code="200">Success</response>
        /// <response code="401">The client is not authorized to access this endpoint</response>
        /// <response code="500">Internal server error</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [HttpGet("organisations")]
        public async Task<ActionResult<IEnumerable<Organisation>>> Get([FromQuery] string payeRef)
        {
            try
            {
                var organisations = await _mediator.Send(new GetOrganisations(payeRef));
                return organisations.Any() ? new ActionResult<IEnumerable<Organisation>>(organisations) : NotFound();
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception, exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the organisations from the pensions regulator for a given PAYE reference
        /// </summary>
        /// <param name="payeRef">The PAYE reference from which to get matching organisations from the pensions regulator.
        /// This needs to be a query parameter due to decoding of slash character
        /// <see cref="https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-2.2"/></param>
        /// <returns>The organisations for the given PAYE reference from the pensions regulator</returns>
        /// <response code="200">Success</response>
        /// <response code="401">The client is not authorized to access this endpoint</response>
        /// <response code="500">Internal server error</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [HttpGet("organisations/{aorn}")]
        public async Task<ActionResult<IEnumerable<Organisation>>> Get(
            [FromRoute] string aorn,
            [FromQuery] string payeRef)
        {
            try
            {
                var organisations = await _mediator.Send(
                    new GetValidatedOrganisations(
                        payeRef,
                        aorn));
                return organisations.Any() ? new ActionResult<IEnumerable<Organisation>>(organisations) : NotFound();
            }
            catch (Exception exception)
            {
                _logger.Log(
                    LogLevel.Error,
                    exception,
                    exception.Message);
                throw;
            }
        }
    }
}
