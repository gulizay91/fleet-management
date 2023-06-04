using Api.V1.Exchanges.Requests;
using Api.V1.Exchanges.Responses;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Persistence.Aggregates.Route;
using Domain.Persistence.Aggregates.Shipment;
using Mapster;
using Route = Domain.Persistence.Aggregates.Route.Route;

namespace Api.Services;

public class RouteService: IRouteService
{
  private readonly ILogger<RouteService> _logger;
  private readonly IPackageRepository _packageRepository;
  private readonly IRouteRepository _routeRepository;
  private readonly ISackRepository _sackRepository;

  public RouteService(ILogger<RouteService> logger, IRouteRepository routeRepository, ISackRepository sackRepository,
    IPackageRepository packageRepository)
  {
    _logger = logger;
    _routeRepository = routeRepository;
    _sackRepository = sackRepository;
    _packageRepository = packageRepository;
  }


  public async Task<DistributeResponse> Distribute(string vehicle, DistributeRequest distribute)
  {
    var response = new DistributeResponse
    {
      Vehicle = vehicle,
      RouteModels = new List<V1.Exchanges.Responses.RouteModel>()
    };

    foreach (var distributeRoute in distribute.RouteModels)
    {
      var deliveryPointOnRoute = new Route(distributeRoute.DeliveryPoint);
      foreach (var distributeRouteDelivery in distributeRoute.Deliveries)
      {
        var shipment = await FindShipment(distributeRouteDelivery.Barcode);

        deliveryPointOnRoute.LoadDelivery(shipment);
        deliveryPointOnRoute.DownloadDelivery(shipment);
        
        var deliverState = (State)deliveryPointOnRoute.FindDelivery(shipment.Barcode).State;
        if (deliverState == State.Unloaded)
        {
          _logger.LogInformation(
            $"Success Delivered {deliveryPointOnRoute.DeliveryPoint} - {shipment.Barcode} - {deliverState}");
        }
        else
        {
          _logger.LogWarning(
            $"Failed Delivered {deliveryPointOnRoute.DeliveryPoint} - {shipment.Barcode} - {deliverState}");
        }
      }

      var addedRoute = await _routeRepository.InsertRoute(deliveryPointOnRoute);
      response.RouteModels.Add(addedRoute.Adapt<V1.Exchanges.Responses.RouteModel>());
    }

    // todo: will be delete
    // var emptySack = await _sackRepository.FindSackByBarcode(Barcodes.C725800);
    // _logger.LogInformation($"Empty Sack: {emptySack.Barcode} - {(State)emptySack.GetState()}.");
    //
    // var emptySack2 = await _sackRepository.FindSackByBarcode(Barcodes.C725799);
    // foreach (var package in emptySack2.Packages)
    // {
    //   _logger.LogInformation($"{emptySack2.Barcode} state: {(State)emptySack2.State} - Package In Sack: {package.Barcode} - {(State)package.GetState()}.");
    // }
    //
    // var createdPackage = await _packageRepository.FindPackageByBarcode(Barcodes.P8_988000_120);
    // _logger.LogInformation($"{createdPackage.Barcode} state: {(State)createdPackage.State}.");
    
    return response;
  }

  private async Task<IShipment> FindShipment(string barcode)
  {
    var deliveryItem = await FindPackage(barcode) ?? (IShipment?)await FindSack(barcode);

    if (deliveryItem is null)
      throw new ShipmentNotFoundException(barcode);

    return deliveryItem;
  }

  private async Task<Package?> FindPackage(string barcode)
  {
    return await _packageRepository.FindPackageByBarcode(barcode);
  }

  private async Task<Sack?> FindSack(string barcode)
  {
    return await _sackRepository.FindSackByBarcode(barcode);
  }
}
