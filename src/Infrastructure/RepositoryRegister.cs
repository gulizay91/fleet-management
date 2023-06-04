using Domain.Persistence.Aggregates.Route;
using Domain.Persistence.Aggregates.Shipment;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class RepositoryRegister
{
  public static void RegisterRepositories(this IServiceCollection serviceCollection, IConfiguration configuration)
  {
    // ps: dataanotation or fluent api validations not working in memory db!
    // ref: https://github.com/dotnet/efcore/issues/7228
    // in-memory db
    var useInMemory = configuration.GetSection("ConnectionStrings:UseInMemory").Value??"false";
    Console.Out.WriteLine($"ConnectionStrings:UseInMemory: {useInMemory}");
    Console.Out.WriteLine($"ConnectionStrings:Default: {configuration.GetConnectionString("Default")}");
    if (Convert.ToBoolean(useInMemory))
      serviceCollection.AddDbContextPool<FleetDbContext>(options =>
        options.UseInMemoryDatabase("fleet-management"));
    else
      // sqllocaldb
      serviceCollection.AddDbContextPool<FleetDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("Default")));

    serviceCollection.AddTransient<IRouteRepository>(provider =>
      new RouteRepository(provider.GetRequiredService<FleetDbContext>()));
    serviceCollection.AddTransient<ISackRepository>(provider =>
      new SackRepository(provider.GetRequiredService<FleetDbContext>()));
    serviceCollection.AddTransient<IPackageRepository>(provider =>
      new PackageRepository(provider.GetRequiredService<FleetDbContext>()));
    serviceCollection.AddTransient<FleetDbContextSeed>();
  }
}