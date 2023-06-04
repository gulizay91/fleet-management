using Domain.Enums;
using Domain.Persistence.SeedWork;

namespace Domain.Persistence.Aggregates.Shipment;

public class Package : IShipment, IEntity<Guid>, IAggregateRoot
{
  public Package(string barcode, DeliveryPoint deliveryPoint, decimal desi)
  {
    Id = Guid.NewGuid();
    Barcode = barcode;
    DeliveryPoint = deliveryPoint;
    Desi = desi;
    State = (int)PackageState.Created;
  }

  public int State { get; private set; }
  public decimal Desi { get; private set; }
  public Sack? Sack { get; }
  public Guid Id { get; init; }
  public string Barcode { get; init; }
  public DeliveryPoint DeliveryPoint { get; protected set; }
  public ShipmentType ShipmentType => ShipmentType.Package;

  public int GetState()
  {
    return State;
  }

  public void SetState(int state)
  {
    State = state;
    if (State is (int)SackState.Unloaded or (int)SackState.Loaded)
      Sack?.SyncStateByPackages(state);
  }

  public bool AllowDownload(DeliveryPoint deliveryPoint)
  {
    // if package in sack not download, only package not in sack
    if (deliveryPoint == DeliveryPoint.Branch && Sack != null) return false;
    // only packages in sack
    if (deliveryPoint == DeliveryPoint.TransferCentre && Sack == null) return false;
    // incorrect destination
    if (deliveryPoint != DeliveryPoint) return false;

    SetState((int)Enums.State.Unloaded);
    return true;
  }
}