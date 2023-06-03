using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace Api.V1.Controllers;

[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
[ApiVersion("1")]
[ApiController]
[Route("v{version:apiVersion}/vehicles")]
public class VehiclesController : ControllerBase
{
  private readonly ILogger<VehiclesController> _logger;

  public VehiclesController(ILogger<VehiclesController> logger)
  {
    _logger = logger;
  }

  [HttpPost("{vehiclePlate}/distribute")]
  [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
  public async Task<IActionResult> Distribute([FromRoute] string vehiclePlate)
  {
    _logger.LogInformation("VehiclePlate: {vehiclePlate}", vehiclePlate);
    return Ok();
  }

  [ApiExplorerSettings(IgnoreApi = true)]
  [HttpGet]
  [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetInitialData()
  {
    _logger.LogInformation("initial values");
    return Ok();
  }
}