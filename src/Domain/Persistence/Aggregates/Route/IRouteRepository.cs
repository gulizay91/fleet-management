namespace Domain.Persistence.Aggregates.Route;

public interface IRouteRepository
{
  Task<Route> InsertRoute(Route route);
}