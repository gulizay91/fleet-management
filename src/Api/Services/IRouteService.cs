using Api.V1.Exchanges.Requests;
using Api.V1.Exchanges.Responses;

namespace Api.Services;

public interface IRouteService
{
  Task<DistributeResponse> Distribute(string vehicle, DistributeRequest distribute);
}