using Domain.Persistence.Aggregates.Route;
using Domain.Persistence.Aggregates.Shipment;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

public static class RepositoryRegister
{
  public static void RegisterRepositories(this IServiceCollection serviceCollection, IConfiguration configuration)
  {
    serviceCollection.AddDbContextPool<FleetDbContext>(options =>
      options.UseInMemoryDatabase("fleet-management").EnableSensitiveDataLogging()
        .EnableThreadSafetyChecks().EnableDetailedErrors()
        .LogTo(Console.WriteLine, LogLevel.Information));

    serviceCollection.AddTransient<IRouteRepository>(provider =>
      new RouteRepository(provider.GetRequiredService<FleetDbContext>()));
    serviceCollection.AddTransient<ISackRepository>(provider =>
      new SackRepository(provider.GetRequiredService<FleetDbContext>()));
    serviceCollection.AddTransient<IPackageRepository>(provider =>
      new PackageRepository(provider.GetRequiredService<FleetDbContext>()));
    serviceCollection.AddTransient<FleetDbContextSeed>();
  }
}