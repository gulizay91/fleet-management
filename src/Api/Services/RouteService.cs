using Api.V1.Exchanges.Requests;
using Api.V1.Exchanges.Responses;
using RouteModel = Api.V1.Exchanges.Responses.RouteModel;

namespace Api.Services;

public class RouteService : IRouteService
{
  private readonly ILogger<RouteService> _logger;

  public RouteService(ILogger<RouteService> logger)
  {
    _logger = logger;
  }


  public async Task<DistributeResponse> Distribute(string vehicle, DistributeRequest distribute)
  {
    var response = new DistributeResponse
    {
      Vehicle = vehicle,
      RouteModels = new List<RouteModel>()
    };
    return response;
  }
}