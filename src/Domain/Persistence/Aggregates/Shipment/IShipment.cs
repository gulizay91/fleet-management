using Domain.Enums;

namespace Domain.Persistence.Aggregates.Shipment;

public interface IShipment
{
  public string Barcode { get; }
  public DeliveryPoint DeliveryPoint { get; }
  public ShipmentType ShipmentType { get; }

  public bool AllowDownload(DeliveryPoint deliveryPoint);

  public void SetState(int state);
  public int GetState();
}