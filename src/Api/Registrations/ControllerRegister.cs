using System.Text.Json.Serialization;
using Api.Filters;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;

namespace Api.Registrations;

public static class ControllerRegister
{
  public static void RegisterControllers(this IServiceCollection serviceCollection)
  {
    serviceCollection.AddControllers().AddJsonOptions(options =>
    {
      options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
    serviceCollection.AddFluentValidationAutoValidation();
    serviceCollection.AddValidatorsFromAssembly(typeof(ValidationFilterAttribute<ValidationResult>).Assembly);
  }
}