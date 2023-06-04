using Domain.Persistence.Aggregates.Route;
using Domain.Persistence.Aggregates.Shipment;

namespace Domain.Persistence.SeedWork;

public interface IUnitOfWork : IDisposable
{
  ISackRepository SackRepository { get; }
  IPackageRepository PackageRepository { get; }
  IRouteRepository RouteRepository { get; }

  Task<int> SaveChangesAsync();
}