using Api.V1.Exchanges.Requests;
using FluentValidation;

namespace Api.V1.Exchanges.Validations;

public class DistributeRequestValidator : AbstractValidator<DistributeRequest>
{
  public DistributeRequestValidator()
  {
    RuleFor(t => t.RouteModels).NotNull().NotEmpty();
    RuleFor(t => t.RouteModels.Count).GreaterThan(0);
    RuleForEach(t => t.RouteModels).SetValidator(new RouteModelValidator());
  }
}

public class RouteModelValidator : AbstractValidator<RouteModel>
{
  public RouteModelValidator()
  {
    RuleFor(t => t.DeliveryPoint).NotNull().NotEmpty();
    RuleFor(t => t.Deliveries).NotNull().NotEmpty();
    RuleFor(t => t.Deliveries.Count).GreaterThan(0);
    RuleForEach(x => x.Deliveries).ChildRules(delivery => { delivery.RuleFor(x => x.Barcode).NotNull().NotEmpty(); });
  }
}