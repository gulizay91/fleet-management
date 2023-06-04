using System.Runtime.Serialization;

namespace Domain.Exceptions;

[Serializable]
public class ShipmentNotFoundException : ArgumentException
{
  public ShipmentNotFoundException(string barcode) : base($"No shipment found with id {barcode}")
  {
  }

  protected ShipmentNotFoundException(SerializationInfo info,
    StreamingContext context) : base(info, context)
  {
  }
}