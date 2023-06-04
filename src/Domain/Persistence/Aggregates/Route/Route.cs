using Domain.Enums;
using Domain.Persistence.Aggregates.Shipment;
using Domain.Persistence.SeedWork;

namespace Domain.Persistence.Aggregates.Route;

public class Route : IEntity<Guid>, IAggregateRoot
{
  private readonly List<Delivery> _deliveries = new();

  public Route(DeliveryPoint deliveryPoint)
  {
    Id = Guid.NewGuid();
    DeliveryPoint = deliveryPoint;
  }

  public DeliveryPoint DeliveryPoint { get; }
  public IReadOnlyCollection<Delivery> Deliveries => _deliveries.AsReadOnly();
  public Guid Id { get; init; }

  // public void LoadDelivery(Delivery newDelivery)
  // {
  //   Guard.Against.DuplicateDelivery(_deliveries, newDelivery.Barcode);
  //   
  //   newDelivery.SetState((int)State.Loaded);
  //   _deliveries.Add(newDelivery);
  // }
  //
  // public void DownloadDelivery(Delivery delivery)
  // {
  //   Guard.Against.NotFoundDelivery(_deliveries, delivery);
  //   _deliveries.Find(r => r.Id == delivery.Id)!.SetState((int)State.Unloaded);
  // }

  public void LoadDelivery<T>(T shipment) where T : IShipment
  {
    Guard.Against.DuplicateDelivery(_deliveries, shipment.Barcode);
    var newDelivery = new Delivery(shipment.Barcode);
    newDelivery.SetState((int)State.Loaded);
    shipment.SetState((int)State.Loaded);
    _deliveries.Add(newDelivery);
  }

  public void DownloadDelivery<T>(T shipment) where T : IShipment
  {
    var existDelivery = FindDelivery(shipment.Barcode);
    if (!shipment.AllowDownload(DeliveryPoint)) return;
    existDelivery.SetState((int)State.Unloaded);
    shipment.SetState((int)State.Unloaded);
  }

  public Delivery FindDelivery(string barcode)
  {
    Guard.Against.NotFoundDelivery(_deliveries, barcode);
    return _deliveries.Find(r => r.Barcode == barcode)!;
  }
}