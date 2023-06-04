using Domain.Exceptions;
using Domain.Persistence.SeedWork;

namespace Domain.Persistence.Aggregates.Route;

public static class RouteGuardExtensions
{
  public static void DuplicateDelivery(this IGuard guard, IEnumerable<Delivery> existingDeliveries,
    string newDeliveryBarcode)
  {
    if (existingDeliveries.Any(a => a.Barcode == newDeliveryBarcode))
      throw new DuplicateDeliveryException($"Cannot add duplicate delivery to route for barcode {newDeliveryBarcode}");
  }

  public static void NotFoundDelivery(this IGuard guard, IEnumerable<Delivery> existingDeliveries,
    string deliveryBarcode)
  {
    if (existingDeliveries.All(a => a.Barcode != deliveryBarcode))
      throw new ShipmentNotFoundException($"Cannot found delivery {deliveryBarcode}");
  }
}