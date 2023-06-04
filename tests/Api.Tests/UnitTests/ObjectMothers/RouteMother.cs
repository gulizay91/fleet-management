using Domain.Enums;
using Domain.Persistence.Aggregates.Route;

namespace Api.Tests.UnitTests.ObjectMothers;

public static class RouteMother
{
  public static Route SimpleRoute()
  {
    return new Route(DeliveryPoint.Branch);
  }
}