using Api.Registrations;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("Api starting...");
ConfigureHostSettings(builder.Host);
Console.WriteLine("Configured Host Settings...");
ConfigurationSettings(builder.Configuration);
RegisterServices(builder.Services, builder.Configuration);
Console.WriteLine("Services Registered...");

var app = builder.Build();

await ConfigureWebApplication(app);

app.Run();

void ConfigurationSettings(IConfigurationBuilder configurationBuilder)
{
  configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
  configurationBuilder.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true);
  configurationBuilder.AddEnvironmentVariables();
}

void ConfigureHostSettings(IHostBuilder hostBuilder)
{
  // Wait 30 seconds for graceful shutdown.
  hostBuilder.ConfigureHostOptions(o => o.ShutdownTimeout = TimeSpan.FromSeconds(30));
}

void RegisterServices(IServiceCollection serviceCollection, IConfiguration configurationRoot)
{
  serviceCollection.AddHealthChecks();
  serviceCollection.AddProblemDetails();
  serviceCollection.RegisterSwagger();
  serviceCollection.RegisterLoggers(configurationRoot);
  serviceCollection.RegisterControllers();
  serviceCollection.RegisterService();
}

async Task ConfigureWebApplication(IApplicationBuilder applicationBuilder)
{
  var provider = applicationBuilder.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
  applicationBuilder.UseExceptionHandler();
  applicationBuilder.UseRouting();
  applicationBuilder.UseEndpoints(endpoints => { endpoints.MapControllers(); });
  applicationBuilder.RegisterHealthCheck();
  applicationBuilder.UseSwagger();
  applicationBuilder.UseSwaggerUI(options =>
  {
    // build a swagger endpoint for each discovered Api version
    foreach (var description in provider.ApiVersionDescriptions)
      options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
        description.GroupName.ToUpperInvariant());
  });

  Console.WriteLine("Seeding Database...");
  await applicationBuilder.RegisterSeedData();
}