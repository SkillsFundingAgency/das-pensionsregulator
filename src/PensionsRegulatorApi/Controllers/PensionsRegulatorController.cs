using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
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
        /// Gets the organisation from the pensions regulator by pension regulator unique id
        /// </summary>
        /// <returns>The organisation for the given pension regulator unique id</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">The client is not authorized to access this endpoint</response>
        /// <response code="404">Not found</response>
        /// <response code="500">Internal server error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Organisation>> Query(long? id)
        {
            try
            {
                if (!id.HasValue)
                {
                    ModelState.AddModelError(nameof(id), "Value cannot be null.");
                    return BadRequest(ModelState);
                }

                var organisation = await _mediator.Send(new GetOrganisationById(id));
                return organisation != null ? new ActionResult<Organisation>(organisation) : NotFound();
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
        /// <param name="payeRef">The PAYE reference from which to get matching organisations from the pensions regulator
        /// This needs to be a query parameter due to decoding of slash character
        /// <see cref="https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-2.2"/> </param>
        /// <returns>The organisations for the given PAYE reference from the pensions regulator</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">The client is not authorized to access this endpoint</response>
        /// <response code="404">Not found</response>
        /// <response code="500">Internal server error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("organisations")]
        public async Task<ActionResult<IEnumerable<Organisation>>> PayeRef([FromQuery] string payeRef)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(payeRef))
                {
                    ModelState.AddModelError(nameof(payeRef), "Value cannot be null or whitespace.");
                    return BadRequest(ModelState);
                }

                var organisations = await _mediator.Send(new GetOrganisationsByPayeRef(payeRef));
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
        /// <response code="400">Bad request</response>
        /// <response code="401">The client is not authorized to access this endpoint</response>
        /// <response code="404">Not found</response>
        /// <response code="500">Internal server error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("organisations/{aorn}")]
        public async Task<ActionResult<IEnumerable<Organisation>>> Aorn([FromRoute] string aorn, [FromQuery] string payeRef)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(payeRef))
                {
                    ModelState.AddModelError(nameof(payeRef), "Value cannot be null or whitespace.");
                }

                if (string.IsNullOrWhiteSpace(aorn))
                {
                    ModelState.AddModelError(nameof(aorn), "Value cannot be null or whitespace.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var organisations = await _mediator.Send(new GetOrganisationsByPayeRefAndAorn(payeRef, aorn));
                return organisations.Any() ? new ActionResult<IEnumerable<Organisation>>(organisations) : NotFound();
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception, exception.Message);
                throw;
            }
        }
    }
}
