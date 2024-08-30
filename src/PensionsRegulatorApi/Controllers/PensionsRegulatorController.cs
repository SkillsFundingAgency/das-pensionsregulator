using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Domain;

namespace PensionsRegulatorApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PensionsRegulatorController(IMediator mediator, ILogger<PensionsRegulatorController> logger)
    : ControllerBase
{
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
    [HttpGet("{id:long}")]
    public async Task<ActionResult<Organisation>> Query(long? id)
    {
        try
        {
            if (!id.HasValue)
            {
                ModelState.AddModelError(nameof(id), "Value cannot be null.");
                return BadRequest(ModelState);
            }

            logger.LogInformation("Get the organisation for pension regulator unique id: {Id}", id);

            var organisation = await mediator.Send(new GetOrganisationById(id));
            return organisation != null ? new ActionResult<Organisation>(organisation) : NotFound();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "An exception occurred in {Controller}.{Action}", nameof(PensionsRegulatorController), nameof(Query));
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

            logger.LogInformation("Get the organisation for PAYE reference: {PayeRef}", payeRef);

            var organisations = await mediator.Send(new GetOrganisationsByPayeRef(payeRef));
            return organisations.Any() ? new ActionResult<IEnumerable<Organisation>>(organisations) : NotFound();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "An exception occurred in {Controller}.{Action}", nameof(PensionsRegulatorController), nameof(PayeRef));
            throw;
        }
    }

    /// <summary>
    /// Gets the organisations from the pensions regulator for a given PAYE reference
    /// </summary>
    /// <param name="aorn">The AORN reference from which to get matching organisations from the pensions regulator.</param>
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

            logger.LogInformation("Get the organisation for aorn: {Aorn} and PAYE reference: {PayeRef}", aorn, payeRef);

            var organisations = await mediator.Send(new GetOrganisationsByPayeRefAndAorn(payeRef, aorn));
            return organisations.Any() ? new ActionResult<IEnumerable<Organisation>>(organisations) : NotFound();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "An exception occurred in {Controller}.{Action}", nameof(PensionsRegulatorController), nameof(Aorn));
            throw;
        }
    }
}