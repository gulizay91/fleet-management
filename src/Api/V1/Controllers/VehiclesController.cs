using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Api.Middlewares;
using Api.Services;
using Api.V1.Exchanges.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Api.V1.Controllers;

[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
[ApiController]
[Route("v{version:apiVersion}/vehicles")]
public class VehiclesController : ControllerBase
{
  private readonly ILogger<VehiclesController> _logger;
  private readonly IRouteService _routeService;

  public VehiclesController(ILogger<VehiclesController> logger, IRouteService routeService)
  {
    _logger = logger;
    _routeService = routeService;
  }

  [HttpPost("{vehiclePlate}/distribute")]
  [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
  public async Task<IActionResult> Distribute([FromRoute] string vehiclePlate, [FromBody] DistributeRequest request)
  {
    _logger.LogInformation("VehiclePlate: {vehiclePlate}", vehiclePlate);
    var result = await _routeService.Distribute(vehiclePlate, request);
    return Ok(result);
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