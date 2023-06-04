namespace Api.Registrations;

public static class SeedDataRegister
{
  public static async Task RegisterSeedData(this IApplicationBuilder applicationBuilder)
  {
    var scopedFactory = applicationBuilder.ApplicationServices.GetService<IServiceScopeFactory>();

    // using var scope = scopedFactory!.CreateScope();
    // var service = scope.ServiceProvider.GetService<FleetDbContextSeed>();
    // if(service is not null)
    //   await service.Seed();
  }
}