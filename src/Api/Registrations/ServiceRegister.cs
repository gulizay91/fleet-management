using Api.Services;

namespace Api.Registrations;

public static class ServiceRegister
{
  public static void RegisterService(this IServiceCollection serviceCollection)
  {
    serviceCollection.AddTransient<IRouteService, RouteService>();
  }
}