using Domain.Persistence.Aggregates.Route;

namespace Infrastructure.Persistence.Repositories;

public class RouteRepository : IRouteRepository
{
  private readonly FleetDbContext _fleetDbContext;

  public RouteRepository(FleetDbContext fleetDbContext)
  {
    _fleetDbContext = fleetDbContext;
  }

  public async Task<Route> InsertRoute(Route route)
  {
    _fleetDbContext.Add(route);
    await _fleetDbContext.SaveChangesAsync();
    return route;
  }
}