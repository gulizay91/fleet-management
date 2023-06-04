using Api.V1.Exchanges.Requests;
using Api.V1.Exchanges.Responses;
using Domain.Enums;
using DeliveryModel = Api.V1.Exchanges.Requests.DeliveryModel;
using RouteModel = Api.V1.Exchanges.Requests.RouteModel;

namespace Api.Tests.UnitTests.ObjectMothers;

// todo: will be adding builder pattern, not enough object mother pattern for especially scenario tests
public static class DistributeMother
{
  public static DistributeRequest SimpleDistributeRequest()
  {
    return new DistributeRequest
    {
      RouteModels = new List<RouteModel>
      {
        new()
        {
          DeliveryPoint = DeliveryPoint.Branch,
          Deliveries = new List<DeliveryModel>
          {
            new() { Barcode = "P7988000121" }
          }
        }
      }
    };
  }

  public static DistributeResponse SimpleDistributeResponse(string vehicle)
  {
    return new DistributeResponse
    {
      Vehicle = vehicle,
      RouteModels = new List<V1.Exchanges.Responses.RouteModel>
      {
        new()
        {
          DeliveryPoint = DeliveryPoint.Branch,
          Deliveries = new List<V1.Exchanges.Responses.DeliveryModel>
          {
            new() { Barcode = "P7988000121", State = 4 }
          }
        }
      }
    };
  }

  public static DistributeRequest SimpleBadDistributeRequest_EmptyRoute()
  {
    return new DistributeRequest
    {
      RouteModels = new List<RouteModel>()
    };
  }

  public static RouteModel SimpleBadDistributeRequest_EmptyDelivery()
  {
    return new RouteModel { DeliveryPoint = 0, Deliveries = new List<DeliveryModel>() };
  }
}